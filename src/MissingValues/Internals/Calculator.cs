using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Internals;

internal static class Calculator
{
	public const int StackAllocThreshold = 64;

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

	public static void Square(ReadOnlySpan<uint> value, Span<uint> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		Debug.Assert(bits.Length == value.Length + value.Length);

		// Executes different algorithms for computing z = a * a
		// based on the actual length of a. If a is "small" enough
		// we stick to the classic "grammar-school" method; for the
		// rest we switch to implementations with less complexity
		// albeit more overhead (which needs to pay off!).

		// Switching to managed references helps eliminating
		// index bounds check...
		ref uint resultPtr = ref MemoryMarshal.GetReference(bits);

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
		for (int i = 0; i < value.Length; i++)
		{
			ulong carry = 0UL;
			uint v = value[i];
			for (int j = 0; j < i; j++)
			{
				ulong digit1 = Unsafe.Add(ref resultPtr, i + j) + carry;
				ulong digit2 = (ulong)value[j] * v;
				Unsafe.Add(ref resultPtr, i + j) = unchecked((uint)(digit1 + (digit2 << 1)));
				carry = (digit2 + (digit1 >> 1)) >> 31;
			}
			ulong digits = (ulong)v * v + carry;
			Unsafe.Add(ref resultPtr, i + i) = unchecked((uint)digits);
			Unsafe.Add(ref resultPtr, i + i + 1) = (uint)(digits >> 32);
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
	public static void Multiply(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs
		Debug.Assert(right.Length < 32);

		// Switching to managed references helps eliminating
		// index bounds check...
		ref uint resultPtr = ref MemoryMarshal.GetReference(bits);

		// Multiplies the bits using the "grammar-school" method.
		// Envisioning the "rhombus" of a pen-and-paper calculation
		// should help getting the idea of these two loops...
		// The inner multiplication operations are safe, because
		// z_i+j + a_j * b_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
		// = 2^64 - 1 (which perfectly matches with ulong!).

		for (int i = 0; i < right.Length; i++)
		{
			uint rv = right[i];
			ulong carry = 0UL;
			for (int j = 0; j < left.Length; j++)
			{
				ref uint elementPtr = ref Unsafe.Add(ref resultPtr, i + j);
				ulong digits = elementPtr + carry + (ulong)left[j] * rv;
				elementPtr = unchecked((uint)digits);
				carry = digits >> 32;
			}
			Unsafe.Add(ref resultPtr, i + left.Length) = (uint)carry;
		}
	}

	public static void DivRem(ReadOnlySpan<uint> left, uint right, Span<uint> quotient, out uint remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			ulong value = (carry << 32) | left[i];
			(ulong digit, carry) = DivRemByUInt32(value, right);
			quotient[i] = (uint)digit;
		}
		remainder = (uint)carry;
	}
	public static void DivRem(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient, Span<uint> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		left.CopyTo(remainder);
		Divide(remainder, right, quotient);
	}

	public static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Same as above, but only computing the quotient.

		ulong carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			ulong value = (carry << 32) | left[i];
			(ulong digit, carry) = DivRemByUInt32(value, right);
			quotient[i] = (uint)digit;
		}
	}
	public static void Divide(Span<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the "grammar-school" algorithm for computing q = a / b.
		// Before calculating q_i, we get more bits into the highest bit
		// block of the divisor. Thus, guessing digits of the quotient
		// will be more precise. Additionally we'll get r = a % b.

		uint divHi = right[right.Length - 1];
		uint divLo = right.Length > 1 ? right[right.Length - 2] : 0;

		// We measure the leading zeros of the divisor
		int shift = BitOperations.LeadingZeroCount(divHi);
		int backShift = 32 - shift;

		// And, we make sure the most significant bit is set
		if (shift > 0)
		{
			uint divNx = right.Length > 2 ? right[right.Length - 3] : 0;

			divHi = (divHi << shift) | (divLo >> backShift);
			divLo = (divLo << shift) | (divNx >> backShift);
		}

		// Then, we divide all of the bits as we would do it using
		// pen and paper: guessing the next digit, subtracting, ...
		for (int i = left.Length; i >= right.Length; i--)
		{
			int n = i - right.Length;
			uint t = ((uint)(i) < (uint)(left.Length)) ? left[i] : 0;

			ulong valHi = ((ulong)(t) << 32) | left[i - 1];
			uint valLo = (i > 1) ? left[i - 2] : 0;

			// We shifted the divisor, we shift the dividend too
			if (shift > 0)
			{
				uint valNx = i > 2 ? left[i - 3] : 0;

				valHi = (valHi << shift) | (valLo >> backShift);
				valLo = (valLo << shift) | (valNx >> backShift);
			}

			// First guess for the current digit of the quotient,
			// which naturally must have only 32 bits...
			ulong digit = valHi / divHi;

			if (digit > 0xFFFF_FFFF)
			{
				digit = 0xFFFF_FFFF;
			}

			// Our first guess may be a little bit to big
			while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo))
			{
				--digit;
			}

			if (digit > 0)
			{
				// Now it's time to subtract our current quotient
				uint carry = SubtractDivisor(left.Slice(n), right, digit);

				if (carry != t)
				{
					Debug.Assert(carry == (t + 1));

					// Our guess was still exactly one too high
					carry = AddDivisor(left.Slice(n), right);

					--digit;
					Debug.Assert(carry == 1);
				}
			}

			// We have the digit!
			if ((uint)(n) < (uint)(bits.Length))
			{
				bits[n] = (uint)(digit);
			}

			if ((uint)(i) < (uint)(left.Length))
			{
				left[i] = 0;
			}
		}

		static uint AddDivisor(Span<uint> left, ReadOnlySpan<uint> right)
		{
			ulong carry = 0;

			// Repairs the dividend, if the last subtract was too much

			for (int i = 0; i < right.Length; i++)
			{
				ref uint leftElement = ref left[i];
				ulong digit = (leftElement + carry) + right[i];

				leftElement = unchecked((uint)digit);
				carry = digit >> 32;
			}

			return (uint)carry;
		}

		static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo, uint divHi, uint divLo)
		{
			Debug.Assert(q <= 0xFFFFFFFF);

			// We multiply the two most significant limbs of the divisor
			// with the current guess for the quotient. If those are bigger
			// than the three most significant limbs of the current dividend
			// we return true, which means the current guess is still too big.

			ulong chkHi = divHi * q;
			ulong chkLo = divLo * q;

			chkHi += (chkLo >> 32);
			chkLo = (uint)(chkLo);

			return (chkHi > valHi) || ((chkHi == valHi) && (chkLo > valLo));
		}

		static uint SubtractDivisor(Span<uint> left, ReadOnlySpan<uint> right, ulong q)
		{
			// Combines a subtract and a multiply operation, which is naturally
			// more efficient than multiplying and then subtracting...

			ulong carry = 0;

			for (int i = 0; i < right.Length; i++)
			{
				carry += right[i] * q;

				uint digit = unchecked((uint)(carry));
				carry >>= 32;

				ref uint leftElement = ref left[i];

				if (leftElement < digit)
				{
					++carry;
				}
				leftElement = unchecked(leftElement - digit);
			}

			return (uint)(carry);
		}
	}

	public static uint Remainder(ReadOnlySpan<uint> left, uint right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Same as above, but only computing the remainder.

		ulong carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			ulong value = (carry << 32) | left[i];
			carry = value % right;
		}

		return (uint)carry;
	}
	public static void Remainder(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs
		// Same as above, but only returning the remainder.

		left.CopyTo(remainder);

		Divide(remainder, right, default);
	}

	public static void Pow(uint value, uint power, Span<uint> bits)
	{
		Pow(value != 0 ? new ReadOnlySpan<uint>(in value) : default, power, bits);
	}
	public static void Pow(ReadOnlySpan<uint> value, uint power, Span<uint> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs

		Debug.Assert(bits.Length == PowBound(power, value.Length));

		Span<uint> temp = stackalloc uint[bits.Length];
		temp.Clear();

		Span<uint> valueCopy = stackalloc uint[bits.Length];
		value.CopyTo(valueCopy);
		valueCopy[value.Length..].Clear();

		Span<uint> result = PowCore(valueCopy, value.Length, temp, power, bits);
		result.CopyTo(bits);
		bits[result.Length..].Clear();
	}

	private static Span<uint> PowCore(Span<uint> value, int valueLength, Span<uint> temp, uint power, Span<uint> result)
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

	private static int MultiplySelf(ref Span<uint> left, int leftLength, ReadOnlySpan<uint> right, ref Span<uint> temp)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		Debug.Assert(leftLength <= left.Length);

		int resultLength = leftLength + right.Length;

		if (leftLength >= right.Length)
		{
			Multiply(left[..leftLength], right, temp[..resultLength]);
		}
		else
		{
			Multiply(right, left[..leftLength], temp[..resultLength]);
		}

		left.Clear();
		//switch buffers
		Span<uint> t = left;
		left = temp;
		temp = t;
		return ActualLength(left[..resultLength]);
	}

	private static int SquareSelf(ref Span<uint> value, int valueLength, ref Span<uint> temp)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.PowMod.cs
		Debug.Assert(valueLength <= value.Length);
		Debug.Assert(temp.Length >= valueLength + valueLength);

		int resultLength = valueLength + valueLength;

		Square(value[..valueLength], temp[..resultLength]);

		value.Clear();
		//switch buffers
		Span<uint> t = value;
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

	public static int ActualLength(ReadOnlySpan<uint> value)
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