using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace MissingValues.Internals;

internal static class Calculator
{
	public const int StackAllocThreshold = 32;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt128 BigMul(ulong a, ulong b)
	{
#if NET9_0_OR_GREATER
		return Math.BigMul(a, b);
#else
		ulong high = Math.BigMul(a, b, out ulong low);
		return new UInt128(high, low);
#endif
	}
	/// <summary>
	/// Produces the full product of two unsigned 128-bit numbers.
	/// </summary>
	/// <param name="a">First number to multiply.</param>
	/// <param name="b">Second number to multiply.</param>
	/// <param name="lower">The low 128-bit of the product of the specified numbers.</param>
	/// <returns>The high 128-bit of the product of the specified numbers.</returns>
	public static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 lower)
	{
		// Adaptation of algorithm for multiplication
		// of 32-bit unsigned integers described
		// in Hacker's Delight by Henry S. Warren, Jr. (ISBN 0-201-91465-4), Chapter 8
		// Basically, it's an optimized version of FOIL method applied to
		// low and high dwords of each operand

		ulong al = a.GetLowerBits();
		ulong ah = a.GetUpperBits();

		ulong bl = b.GetLowerBits();
		ulong bh = b.GetUpperBits();

		UInt128 mull = BigMul(al, bl);
		UInt128 t = BigMul(ah, bl) + mull.GetUpperBits();
		UInt128 tl = BigMul(al, bh) + t.GetLowerBits();

		lower = new UInt128(tl.GetLowerBits(), mull.GetLowerBits());

		return BigMul(ah, bh) + t.GetUpperBits() + tl.GetUpperBits();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (ulong Quotient, uint Remainder) DivRemByUInt32(ulong left, uint right)
	{
		ulong quotient = left / right;
		return (quotient, (uint)left - ((uint)quotient * right));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (UInt128 Quotient, ulong Remainder) DivRemByUInt64(UInt128 left, ulong right)
	{
#if NET9_0_OR_GREATER
		if (X86Base.X64.IsSupported)
		{
			ulong highRes = 0ul;
			ulong remainder = (ulong)(left >> 64);
            
#pragma warning disable SYSLIB5004
			if (remainder >= right)
			{
				(highRes, remainder) = X86Base.X64.DivRem(remainder, 0, right);
			}

			(ulong lowRes, remainder) = X86Base.X64.DivRem((ulong)left, remainder, right);
#pragma warning restore SYSLIB5004
			return (new UInt128(highRes, lowRes), remainder);
		}
#endif
		UInt128 quotient = left / right;
		return (quotient, (ulong)left - ((ulong)quotient * right));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt128 DivideByUInt64(UInt128 left, ulong right)
	{
#if NET9_0_OR_GREATER
		if (X86Base.X64.IsSupported)
		{
			ulong highRes = 0ul;
			ulong remainder = (ulong)(left >> 64);
            
#pragma warning disable SYSLIB5004
			if (remainder >= right)
			{
				(highRes, remainder) = X86Base.X64.DivRem(remainder, 0, right);
			}

			return new UInt128(highRes, X86Base.X64.DivRem((ulong)left, remainder, right).Quotient);
#pragma warning restore SYSLIB5004
		}
#endif
		return left / right;
	}

	public static void Square(ref ulong value, int valueLength, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		Debug.Assert(bits.Length == valueLength + valueLength);

		// Executes different algorithms for computing z = a * a
		// based on the actual length of a. If a is "small" enough
		// we stick to the classic "grammar-school" method; for the
		// rest we switch to implementations with less complexity
		// albeit more overhead (which needs to pay off!).

		// Switching to managed references helps eliminating
		// index bounds check...
		ref ulong resultPtr = ref MemoryMarshal.GetReference(bits);

		// Squares the bits using the "grammar-school" method.
		// Envisioning the "rhombus" of a pen-and-paper calculation
		// we see that computing z_i+j += a_j * a_i can be optimized
		// since a_j * a_i = a_i * a_j (we're squaring after all!).
		// Thus, we directly get z_i+j += 2 * a_j * a_i + c.

		// ATTENTION: an ordinary multiplication is safe, because
		// z_i+j + a_j * a_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
		// = 2^64 - 1 (which perfectly matches with ulong!). But
		// here we would need an UInt65... Hence, we split these
		// operation and do some extra shifts.
		for (int i = 0; i < valueLength; i++)
		{
			UInt128 carry = default;
			ulong v = Unsafe.Add(ref value, i);
			for (int j = 0; j < i; j++)
			{
				UInt128 digit1 = Unsafe.Add(ref resultPtr, i + j) + carry;
				UInt128 digit2 = BigMul(Unsafe.Add(ref value, j), v);
				Unsafe.Add(ref resultPtr, i + j) = unchecked((ulong)(digit1 + (digit2 << 1)));
				carry = (digit2 + (digit1 >> 1)) >> 31;
			}
			UInt128 digits = BigMul(v, v) + carry;
			Unsafe.Add(ref resultPtr, i + i) = unchecked((ulong)digits);
			Unsafe.Add(ref resultPtr, i + i + 1) = (ulong)(digits >> 64);
		}
	}

	public static UInt256 Multiply(in UInt256 left, ulong right, out ulong carry)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		// Executes the multiplication for one big and one 64-bit integer.
		// Since every step holds the already slightly familiar equation
		// a_i * b + c <= 2^64 - 1 + (2^64 - 1)^2 < 2^128 - 1,
		// we are safe regarding to overflows.

		ulong p3, p2, p1, p0;

		UInt128 c = default;

		UInt128 digits = BigMul(left.Part0, right) + c;
		p0 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part1, right) + c;
		p1 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part2, right) + c;
		p2 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part3, right) + c;
		p3 = unchecked((ulong)digits);
		carry = (ulong)(digits >> 64);

		return new UInt256(p3, p2, p1, p0);
	}
	public static UInt512 Multiply(in UInt512 left, ulong right, out ulong carry)
	{
		ulong p7, p6, p5, p4, p3, p2, p1, p0;

		UInt128 c = default;

		UInt128 digits = BigMul(left.Part0, right) + c;
		p0 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part1, right) + c;
		p1 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part2, right) + c;
		p2 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part3, right) + c;
		p3 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part4, right) + c;
		p4 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part5, right) + c;
		p5 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part6, right) + c;
		p6 = unchecked((ulong)digits);
		c = digits >> 64;

		digits = BigMul(left.Part7, right) + c;
		p7 = unchecked((ulong)digits);
		carry = (ulong)(digits >> 64);

		return new UInt512(
			p7, p6, p5, p4,
			p3, p2, p1, p0
			);
	}
	public static void Multiply(ref ulong left, int leftLength, ref ulong right, int rightLength, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs
		Debug.Assert(leftLength < 32);
		Debug.Assert(rightLength < 32);

		// Switching to managed references helps eliminating
		// index bounds check...
		ref ulong resultPtr = ref MemoryMarshal.GetReference(bits);

		// Multiplies the bits using the "grammar-school" method.
		// Envisioning the "rhombus" of a pen-and-paper calculation
		// should help getting the idea of these two loops...
		// The inner multiplication operations are safe, because
		// z_i+j + a_j * b_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
		// = 2^64 - 1 (which perfectly matches with ulong!).

		for (int i = 0; i < rightLength; i++)
		{
			ulong rv = Unsafe.Add(ref right, i);
			UInt128 carry = default;
			for (int j = 0; j < leftLength; j++)
			{
				ref ulong elementPtr = ref Unsafe.Add(ref resultPtr, i + j);
				UInt128 digits = elementPtr + carry + BigMul(Unsafe.Add(ref left, j), rv);
				elementPtr = unchecked((ulong)digits);
				carry = digits >> 64;
			}
			Unsafe.Add(ref resultPtr, i + leftLength) = (ulong)carry;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void DivRem(in UInt256 left, ulong right, out UInt256 quotient, out ulong remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p3, p2, p1, p0;
		ulong carry;
		UInt128 value, digit;

		if (left.Part3 != 0)
		{
			value = new UInt128(0, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p3 = (ulong)digit;
			
			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;
			
			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p3 = 0;

			value = new UInt128(0, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;
			
			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else
		{
			(value, remainder) = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right);
			quotient = new UInt256(0, 0, (ulong)(value >> 64), (ulong)value);

			return;
		}

		remainder = carry;
		quotient = new UInt256(p3, p2, p1, p0);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void DivRem(in UInt512 left, ulong right, out UInt512 quotient, out ulong remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p07, p06, p05, p04, p03, p02, p01, p00;
		ulong carry; 
		UInt128 value, digit;

		if (left.Part7 != 0)
		{
			value = new UInt128(0, left.Part7);
			(digit, carry) = DivRemByUInt64(value, right);
			p07 = (ulong)digit;

			value = new UInt128(carry, left.Part6);
			(digit, carry) = DivRemByUInt64(value, right);
			p06 = (ulong)digit;

			value = new UInt128(carry, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part6 != 0)
		{
			p07 = 0;

			value = new UInt128(0, left.Part6);
			(digit, carry) = DivRemByUInt64(value, right);
			p06 = (ulong)digit;

			value = new UInt128(carry, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part5 != 0)
		{
			p07 = 0;
			p06 = 0;

			value = new UInt128(0, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part4 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;

			value = new UInt128(0, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part3 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;

			value = new UInt128(0, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;
			p03 = 0;

			value = new UInt128(0, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, carry) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else
		{
			(value, remainder) = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right);
			quotient = new UInt512(0, 0, 0, 0, 0, 0, (ulong)(value >> 64), (ulong)value);
			return;
		}

		remainder = carry;
		quotient = new UInt512(
			p07, p06, p05, p04,
			p03, p02, p01, p00);
	}
	public static void DivRem(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> quotient, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		left.CopyTo(remainder);
		Divide(remainder, right, quotient);
	}

	public static UInt256 Divide(in UInt256 left, ulong right)
	{
		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p3, p2, p1, p0;
		ulong carry;
		UInt128 digit, value;

		if (left.Part3 != 0)
		{
			value = new UInt128(0, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p3 = (ulong)digit;
			
			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p3 = 0;
			
			value = new UInt128(0, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else
		{
			(value, _) = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right);

			return new UInt256(0, 0, (ulong)(value >> 64), (ulong)value);
		}

		return new UInt256(p3, p2, p1, p0);
	}
	
	public static UInt512 Divide(in UInt512 left, ulong right)
	{
		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p07, p06, p05, p04, p03, p02, p01, p00;
		ulong carry;
		UInt128 value, digit;

		if (left.Part7 != 0)
		{
			value = new UInt128(0, left.Part7);
			(digit, carry) = DivRemByUInt64(value, right);
			p07 = (ulong)digit;

			value = new UInt128(carry, left.Part6);
			(digit, carry) = DivRemByUInt64(value, right);
			p06 = (ulong)digit;

			value = new UInt128(carry, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part6 != 0)
		{
			p07 = 0;

			value = new UInt128(0, left.Part6);
			(digit, carry) = DivRemByUInt64(value, right);
			p06 = (ulong)digit;

			value = new UInt128(carry, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part5 != 0)
		{
			p07 = 0;
			p06 = 0;

			value = new UInt128(0, left.Part5);
			(digit, carry) = DivRemByUInt64(value, right);
			p05 = (ulong)digit;

			value = new UInt128(carry, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part4 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;

			value = new UInt128(0, left.Part4);
			(digit, carry) = DivRemByUInt64(value, right);
			p04 = (ulong)digit;

			value = new UInt128(carry, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part3 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;

			value = new UInt128(0, left.Part3);
			(digit, carry) = DivRemByUInt64(value, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;
			p03 = 0;

			value = new UInt128(0, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			(digit, _) = DivRemByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else
		{
			(value, _) = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right);
			return new UInt512(0, 0, 0, 0, 0, 0, (ulong)(value >> 64), (ulong)value);
		}

		return new UInt512(
			p07, p06, p05, p04,
			p03, p02, p01, p00);
	}
	
	public static void Divide(Span<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the "grammar-school" algorithm for computing q = a / b.
		// Before calculating q_i, we get more bits into the highest bit
		// block of the divisor. Thus, guessing digits of the quotient
		// will be more precise. Additionally we'll get r = a % b.

		ulong divHi = right[^1];
		ulong divLo = right.Length > 1 ? right[^2] : 0;

		// We measure the leading zeros of the divisor
		int shift = BitOperations.LeadingZeroCount(divHi);
		int backShift = 64 - shift;

		// And, we make sure the most significant bit is set
		if (shift > 0)
		{
			ulong divNx = right.Length > 2 ? right[^3] : 0;

			divHi = (divHi << shift) | (divLo >> backShift);
			divLo = (divLo << shift) | (divNx >> backShift);
		}

		// Then, we divide all of the bits as we would do it using
		// pen and paper: guessing the next digit, subtracting, ...
		for (int i = left.Length; i >= right.Length; i--)
		{
			int n = i - right.Length;
			ulong t = ((uint)i < (uint)left.Length) ? left[i] : 0;

			UInt128 valHi = new UInt128(t, left[i - 1]);
			ulong valLo = (i > 1) ? left[i - 2] : 0;

			// We shifted the divisor, we shift the dividend too
			if (shift > 0)
			{
				ulong valNx = i > 2 ? left[i - 3] : 0;

				valHi = (valHi << shift) | (valLo >> backShift);
				valLo = (valLo << shift) | (valNx >> backShift);
			}

			// First guess for the current digit of the quotient,
			// which naturally must have only 64 bits...
			UInt128 digit = DivideByUInt64(valHi, divHi);
			var digit64 = digit > new UInt128(0, 0xFFFF_FFFF_FFFF_FFFF) ? 0xFFFF_FFFF_FFFF_FFFF : (ulong)digit;

			// Our first guess may be a little bit too big
			while (DivideGuessTooBig(digit64, valHi, valLo, divHi, divLo))
			{
				--digit64;
			}

			if (digit64 > 0)
			{
				// Now it's time to subtract our current quotient
				ulong carry = SubtractDivisor(left[n..], right, digit64);

				if (carry != t)
				{
					Debug.Assert(carry == (t + 1));

					// Our guess was still exactly one too high
					carry = AddDivisor(left[n..], right);

					--digit64;
					Debug.Assert(carry == 1);
				}
			}

			// We have the digit!
			if ((uint)n < (uint)bits.Length)
			{
				bits[n] = digit64;
			}

			if ((uint)i < (uint)left.Length)
			{
				left[i] = 0;
			}
		}

		return;
		
		static ulong AddDivisor(Span<ulong> left, ReadOnlySpan<ulong> right)
		{
			UInt128 carry = default;

			// Repairs the dividend, if the last subtract was too much

			for (int i = 0; i < right.Length; i++)
			{
				ref ulong leftElement = ref left[i];
				UInt128 digit = (leftElement + carry) + right[i];

				leftElement = unchecked((ulong)digit);
				carry = digit >> 64;
			}

			return (ulong)carry;
		}

		static bool DivideGuessTooBig(ulong q, UInt128 valHi, ulong valLo, ulong divHi, ulong divLo)
		{
			// We multiply the two most significant limbs of the divisor
			// with the current guess for the quotient. If those are bigger
			// than the three most significant limbs of the current dividend
			// we return true, which means the current guess is still too big.
			UInt128 chkHi = BigMul(divHi, q);
			UInt128 chkLo = BigMul(divLo, q);

			chkHi += (chkLo >> 64);
			chkLo = (ulong)(chkLo);

			return (chkHi > valHi) || ((chkHi == valHi) && (chkLo > valLo));
		}

		static ulong SubtractDivisor(Span<ulong> left, ReadOnlySpan<ulong> right, ulong q)
		{
			// Combines a subtract and a multiply operation, which is naturally
			// more efficient than multiplying and then subtracting...

			UInt128 carry = default;

			for (int i = 0; i < right.Length; i++)
			{
				carry += BigMul(right[i], q);

				ulong digit = unchecked((ulong)carry);
				carry >>= 64;

				ref ulong leftElement = ref left[i];

				if (leftElement < digit)
				{
					++carry;
				}
				leftElement = unchecked(leftElement - digit);
			}

			return (ulong)carry;
		}
	}
	
	public static ulong Remainder(in UInt256 left, ulong right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong carry;
		UInt128 value;

		if (left.Part3 != 0)
		{
			value = new UInt128(0, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part2 != 0)
		{
			value = new UInt128(0, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;

		}
		else
		{
			carry = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right).Remainder;
		}

		return carry;
	}
	
	public static ulong Remainder(in UInt512 left, ulong right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong carry;
		UInt128 value;

		if (left.Part7 != 0)
		{
			value = new UInt128(0, left.Part7);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part6);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part5);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part4);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part6 != 0)
		{
			value = new UInt128(0, left.Part6);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part5);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part4);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part5 != 0)
		{
			value = new UInt128(0, left.Part5);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part4);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part4 != 0)
		{
			value = new UInt128(0, left.Part4);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part3 != 0)
		{
			value = new UInt128(0, left.Part3);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part2 != 0)
		{
			value = new UInt128(0, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;

		}
		else
		{
			carry = DivRemByUInt64(new UInt128(left.Part1, left.Part0), right).Remainder;
		}

		return carry;
	}
	public static void Remainder(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs
		// Same as above, but only returning the remainder.

		left.CopyTo(remainder);

		Divide(remainder, right, default);
	}

	public static void Pow(ulong value, uint power, Span<ulong> bits)
	{
		Pow(value != 0 ? new ReadOnlySpan<ulong>(in value) : default, power, bits);
	}
	public static void Pow(ReadOnlySpan<ulong> value, uint power, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs

		Debug.Assert(bits.Length == PowBound(power, value.Length));

		Span<ulong> temp = stackalloc ulong[bits.Length];
		temp.Clear();

		Span<ulong> valueCopy = stackalloc ulong[bits.Length];
		value.CopyTo(valueCopy);
		valueCopy[value.Length..].Clear();

		Span<ulong> result = PowCore(valueCopy, value.Length, temp, power, bits);
		result.CopyTo(bits);
		bits[result.Length..].Clear();
	}

	private static Span<ulong> PowCore(Span<ulong> value, int valueLength, Span<ulong> temp, uint power, Span<ulong> result)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		Debug.Assert(value.Length >= valueLength);
		Debug.Assert(temp.Length == result.Length);
		Debug.Assert(value.Length == temp.Length);

		result[0] = 1;
		int resultLength = 1;

		// The basic pow algorithm using square-and-multiply.
		while (power != 0)
		{
			if ((power & 1) == 1)
				resultLength = MultiplySelf(ref result, resultLength, value[..valueLength], ref temp);
			if (power != 1)
				valueLength = SquareSelf(ref value, valueLength, ref temp);
			power >>= 1;
		}

		return result[..resultLength];
	}

	private static int MultiplySelf(ref Span<ulong> left, int leftLength, ReadOnlySpan<ulong> right, ref Span<ulong> temp)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		Debug.Assert(leftLength <= left.Length);

		int resultLength = leftLength + right.Length;

		if (leftLength >= right.Length)
		{
			Multiply(ref MemoryMarshal.GetReference(left), leftLength, ref MemoryMarshal.GetReference(right), right.Length, temp[..resultLength]);
		}
		else
		{
			Multiply(ref MemoryMarshal.GetReference(right), right.Length, ref MemoryMarshal.GetReference(left), leftLength, temp[..resultLength]);
		}

		left.Clear();
		//switch buffers
		Span<ulong> t = left;
		left = temp;
		temp = t;
		return ActualLength(left[..resultLength]);
	}

	private static int SquareSelf(ref Span<ulong> value, int valueLength, ref Span<ulong> temp)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		Debug.Assert(valueLength <= value.Length);
		Debug.Assert(temp.Length >= valueLength + valueLength);

		int resultLength = valueLength + valueLength;

		Square(ref MemoryMarshal.GetReference(value), valueLength, temp[..resultLength]);

		value.Clear();
		//switch buffers
		Span<ulong> t = value;
		value = temp;
		temp = t;
		return ActualLength(value[..resultLength]);
	}

	public static int PowBound(uint power, int valueLength)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		// The basic pow algorithm, but instead of squaring
		// and multiplying we just sum up the lengths.

		int resultLength = 1;
		while (power != 0)
		{
			checked
			{
				if ((power & 1) == 1)
					resultLength += valueLength;
				if (power != 1)
					valueLength += valueLength;
			}
			power >>= 1;
		}

		return resultLength;
	}

	public static int ActualLength(ReadOnlySpan<ulong> value)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.Utils.cs
		// Since we're reusing memory here, the actual length
		// of a given value may be less then the array's length

		int length = value.Length;

		while (length > 0 && value[length - 1] == 0)
			--length;
		return length;
	}
}