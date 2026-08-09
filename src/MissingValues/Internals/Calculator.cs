using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace MissingValues.Internals;

internal static class Calculator
{
	internal const int StackAllocThreshold = 128;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong AddWithCarry(ulong a, ulong b, out ulong carry)
	{
		ulong result = a + b;
		
		// For unsigned addition, we can detect overflow by checking `(x + y) < x`
		
		carry = (result < a) ? 1UL : 0UL;
    
		return result;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong AddWithCarry(ulong a, ulong b, ulong carryIn, out ulong carryOut)
	{
		ulong sum1 = a + b;
		ulong c1 = (sum1 < a) ? 1 : (ulong)0;
		ulong sum2 = sum1 + carryIn;
		ulong c2 = (sum2 < sum1) ? 1 : (ulong)0;
		carryOut = c1 + c2;
		return sum2;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static (ulong hi, ulong lo) BigMulAdd(ulong a, ulong b, ulong c)
	{
		ulong highProd = Math.BigMul(a, b, out ulong lowProd);
		
		ulong lower = lowProd + c;
		ulong carry = (lower < lowProd) ? 1UL : 0UL;

		ulong upper = highProd + carry;
            
		return (upper, lower);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static (ulong Quotient, uint Remainder) DivRemByUInt32(ulong left, uint right)
	{
		ulong quotient = left / right;
		return (quotient, (uint)left - ((uint)quotient * right));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static (UInt128 Quotient, ulong Remainder) DivRemByUInt64(UInt128 left, ulong right)
	{
		if (X86Base.X64.IsSupported)
		{
			ulong highRes = 0ul;
			ulong remainder = left.Upper;
            
#pragma warning disable SYSLIB5004
			if (remainder >= right)
			{
				(highRes, remainder) = X86Base.X64.DivRem(remainder, 0, right);
			}

			(ulong lowRes, remainder) = X86Base.X64.DivRem((ulong)left, remainder, right);
#pragma warning restore SYSLIB5004
			return (new UInt128(highRes, lowRes), remainder);
		}
		UInt128 quotient = left / right;
		return (quotient, left.Lower - (quotient.Lower * right));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong DivRemByUInt64(ulong hi, ulong lo, ulong divisor)
	{
		if (hi == 0)
		{
			(ulong q, ulong r) = Math.DivRem(lo, divisor);
			return q;
		}
		if (divisor <= uint.MaxValue)
		{
			ulong loHi = lo >> 32;
			ulong loLo = lo & 0xFFFFFFFF;

			(ulong qHi, ulong r1) = Math.DivRem((hi << 32) | loHi, divisor);
			(ulong qLo, ulong r2) = Math.DivRem((r1 << 32) | loLo, divisor);

			return ((qHi << 32) | qLo);
		}
#pragma warning disable SYSLIB5004 // X86Base.DivRem is experimental
		if (X86Base.X64.IsSupported)
		{
			(ulong q, ulong r) = X86Base.X64.DivRem(lo, hi, divisor);
			return (ulong)q;
		}
#pragma warning restore SYSLIB5004
		UInt128 value = new UInt128(hi, lo);
		ulong quotient = (value / divisor).Lower;
		return quotient;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt128 DivideByUInt64(UInt128 left, ulong right)
	{
		if (X86Base.X64.IsSupported)
		{
			ulong highRes = 0ul;
			ulong remainder = left.Upper;
            
#pragma warning disable SYSLIB5004
			if (remainder >= right)
			{
				(highRes, remainder) = X86Base.X64.DivRem(remainder, 0, right);
			}

			return new UInt128(highRes, X86Base.X64.DivRem((ulong)left, remainder, right).Quotient);
#pragma warning restore SYSLIB5004
		}
		return left / right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Square(Span<ulong> value, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		Debug.Assert(bits.Length == value.Length + value.Length);

		// Executes different algorithms for computing z = a * a
		// based on the actual length of a. If a is "small" enough
		// we stick to the classic "grammar-school" method; for the
		// rest we switch to implementations with less complexity
		// albeit more overhead (which needs to pay off!).
		
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
			UInt128 carry = default;
			ulong v = value[i];
			for (int j = 0; j < i; j++)
			{
				UInt128 digit1 = bits[i + j] + carry;
				UInt128 digit2 = Math.BigMul(value[j], v);
				bits[i + j] = unchecked((ulong)(digit1 + (digit2 << 1)));
				carry = (digit2 + (digit1 >> 1)) >> 63;
			}
			UInt128 digits = Math.BigMul(v, v) + carry;
			bits[i + i] = digits.Lower;
			bits[i + i + 1] = digits.Upper;
		}
	}

	internal static UInt256 Multiply(in UInt256 left, ulong right, out ulong carry)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		// Executes the multiplication for one big and one 64-bit integer.
		// Since every step holds the already slightly familiar equation
		// a_i * b + c <= 2^64 - 1 + (2^64 - 1)^2 < 2^128 - 1,
		// we are safe regarding to overflows.

		ulong p3, p2, p1, p0;

		carry = Math.BigMul(left.Part0, right, out p0);
		(carry, p1) = BigMulAdd(left.Part1, right, carry);
		(carry, p2) = BigMulAdd(left.Part2, right, carry);
		(carry, p3) = BigMulAdd(left.Part3, right, carry);

		return new UInt256(p3, p2, p1, p0);
	}
	internal static UInt512 Multiply(in UInt512 left, ulong right, out ulong carry)
	{
		ulong p7, p6, p5, p4, p3, p2, p1, p0;
		
		carry = Math.BigMul(left.Part0, right, out p0);
		(carry, p1) = BigMulAdd(left.Part1, right, carry);
		(carry, p2) = BigMulAdd(left.Part2, right, carry);
		(carry, p3) = BigMulAdd(left.Part3, right, carry);
		(carry, p4) = BigMulAdd(left.Part4, right, carry);
		(carry, p5) = BigMulAdd(left.Part5, right, carry);
		(carry, p6) = BigMulAdd(left.Part6, right, carry);
		(carry, p7) = BigMulAdd(left.Part7, right, carry);
		
		return new UInt512(
			p7, p6, p5, p4,
			p3, p2, p1, p0
			);
	}
	internal static void Multiply(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> bits)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs
		Debug.Assert(left.Length < 32);
		Debug.Assert(right.Length < 32);

		// Multiplies the bits using the "grammar-school" method.
		// Envisioning the "rhombus" of a pen-and-paper calculation
		// should help getting the idea of these two loops...
		// The inner multiplication operations are safe, because
		// z_i+j + a_j * b_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
		// = 2^64 - 1 (which perfectly matches with ulong!).

		for (int i = 0; i < right.Length; i++)
		{
			bits[i + left.Length] = MulAdd1(bits.Slice(i), left, right[i]);;
		}

		return;

		static ulong MulAdd1(Span<ulong> result, ReadOnlySpan<ulong> left, ulong multiplier)
		{
			Debug.Assert(result.Length >= left.Length);
		
			int length = left.Length;
			int i = 0;
			ulong carry = 0;
		
			// Unroll by 4: mulx has 3-5 cycle latency but 1 cycle throughput,
			// so issuing 4 multiplies allows the CPU to pipeline them while
			// carry chains complete sequentially behind.
			for (; i + 3 < length; i += 4)
			{
				UInt128 p0 = (UInt128)left[i] * multiplier + result[i] + carry;
				result[i] = p0.Lower;

				UInt128 p1 = (UInt128)left[i + 1] * multiplier + result[i + 1] + p0.Upper;
				result[i + 1] = p1.Lower;

				UInt128 p2 = (UInt128)left[i + 2] * multiplier + result[i + 2] + p1.Upper;
				result[i + 2] = p2.Lower;

				UInt128 p3 = (UInt128)left[i + 3] * multiplier + result[i + 3] + p2.Upper;
				result[i + 3] = p3.Lower;

				carry = p3.Upper;
			}

			for (; i < length; i++)
			{
				UInt128 product = Math.BigMul(left[i], multiplier) + result[i] + carry;
				result[i] = product.Lower;
				carry = product.Upper;
			}
		
			return carry;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void DivRem(in UInt256 left, ulong right, out UInt256 quotient, out ulong remainder)
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
			(digit, carry) = Math.DivRem(left.Part3, right);
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

			(digit, carry) = Math.DivRem(left.Part2, right);
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
			quotient = new UInt256(0, 0, value.Upper, (ulong)value);

			return;
		}

		remainder = carry;
		quotient = new UInt256(p3, p2, p1, p0);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void DivRem(in UInt512 left, ulong right, out UInt512 quotient, out ulong remainder)
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
			(digit, carry) = Math.DivRem(left.Part7, right);
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

			(digit, carry) = Math.DivRem(left.Part6, right);
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

			(digit, carry) = Math.DivRem(left.Part5, right);
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

			(digit, carry) = Math.DivRem(left.Part4, right);
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

			(digit, carry) = Math.DivRem(left.Part3, right);
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

			(digit, carry) = Math.DivRem(left.Part2, right);
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
			quotient = new UInt512(0, 0, 0, 0, 0, 0, value.Upper, value.Lower);
			return;
		}

		remainder = carry;
		quotient = new UInt512(
			p07, p06, p05, p04,
			p03, p02, p01, p00);
	}
	internal static void DivRem(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> quotient, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		left.CopyTo(remainder);
		Divide(remainder, right, quotient);
	}

	internal static UInt256 Divide(in UInt256 left, ulong right)
	{
		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p3, p2, p1, p0;
		ulong carry;
		UInt128 digit, value;

		if (left.Part3 != 0)
		{
			(digit, carry) = Math.DivRem(left.Part3, right);
			p3 = (ulong)digit;
			
			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			digit = DivideByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p3 = 0;
			
			(digit, carry) = Math.DivRem(left.Part2, right);
			p2 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p1 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			digit = DivideByUInt64(value, right);
			p0 = (ulong)digit;
		}
		else
		{
			value = DivideByUInt64(new UInt128(left.Part1, left.Part0), right);

			return new UInt256(0, 0, value.Upper, value.Lower);
		}

		return new UInt256(p3, p2, p1, p0);
	}
	
	internal static UInt512 Divide(in UInt512 left, ulong right)
	{
		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong p07, p06, p05, p04, p03, p02, p01, p00;
		ulong carry;
		UInt128 value, digit;

		if (left.Part7 != 0)
		{
			(digit, carry) = Math.DivRem(left.Part7, right);
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
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part6 != 0)
		{
			p07 = 0;

			(digit, carry) = Math.DivRem(left.Part6, right);
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
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part5 != 0)
		{
			p07 = 0;
			p06 = 0;

			(digit, carry) = Math.DivRem(left.Part5, right);
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
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part4 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;

			(digit, carry) = Math.DivRem(left.Part4, right);
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
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part3 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;

			(digit, carry) = Math.DivRem(left.Part3, right);
			p03 = (ulong)digit;

			value = new UInt128(carry, left.Part2);
			(digit, carry) = DivRemByUInt64(value, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else if (left.Part2 != 0)
		{
			p07 = 0;
			p06 = 0;
			p05 = 0;
			p04 = 0;
			p03 = 0;

			(digit, carry) = Math.DivRem(left.Part2, right);
			p02 = (ulong)digit;

			value = new UInt128(carry, left.Part1);
			(digit, carry) = DivRemByUInt64(value, right);
			p01 = (ulong)digit;

			value = new UInt128(carry, left.Part0);
			digit = DivideByUInt64(value, right);
			p00 = (ulong)digit;
		}
		else
		{
			value = DivideByUInt64(new UInt128(left.Part1, left.Part0), right);
			return new UInt512(0, 0, 0, 0, 0, 0, value.Upper, value.Lower);
		}

		return new UInt512(
			p07, p06, p05, p04,
			p03, p02, p01, p00);
	}
	
	internal static void Divide(Span<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> bits)
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

			ulong valHi1 = t;
			ulong valHi0 = left[i - 1];
			ulong valLo = (i > 1) ? left[i - 2] : 0;

			// We shifted the divisor, we shift the dividend too
			if (shift > 0)
			{
				ulong valNx = i > 2 ? left[i - 3] : 0;

				valHi1 = (valHi1 << shift) | (valHi0 >> backShift);
				valHi0 = (valHi0 << shift) | (valLo >> backShift);
				valLo = (valLo << shift) | (valNx >> backShift);
			}

			// First guess for the current digit of the quotient,
			// which naturally must have only 64 bits...
			ulong digit = (valHi1 >= divHi) ? ulong.MaxValue : DivRemByUInt64(valHi1, valHi0, divHi);

			// Our first guess may be a little bit too big
			while (DivideGuessTooBig(digit, valHi1, valHi0, valLo, divHi, divLo))
			{
				--digit;
			}

			if (digit > 0)
			{
				// Now it's time to subtract our current quotient
				ulong carry = SubtractDivisor(left[n..], right, digit);

				if (carry != t)
				{
					Debug.Assert(carry == (t + 1));

					// Our guess was still exactly one too high
					carry = AddDivisor(left[n..], right);

					--digit;
					Debug.Assert(carry == 1);
				}
			}

			// We have the digit!
			if ((uint)n < (uint)bits.Length)
			{
				bits[n] = digit;
			}

			if ((uint)i < (uint)left.Length)
			{
				left[i] = 0;
			}
		}

		return;
		
		static ulong AddDivisor(Span<ulong> left, ReadOnlySpan<ulong> right)
		{
			ulong carry = 0;

			// Repairs the dividend, if the last subtract was too much

			for (int i = 0; i < right.Length; i++)
			{
				ref ulong leftElement = ref left[i];
				leftElement = AddWithCarry(leftElement, right[i], carry, out carry);
			}

			return carry;
		}

		static bool DivideGuessTooBig(ulong q, ulong valHi1, ulong valHi0, ulong valLo, ulong divHi, ulong divLo)
		{
			// We multiply the two most significant limbs of the divisor
			// with the current guess for the quotient. If those are bigger
			// than the three most significant limbs of the current dividend
			// we return true, which means the current guess is still too big.

			ulong chkHiHi = Math.BigMul(divHi, q, out ulong chkHiLo);
			ulong chkLoHi = Math.BigMul(divLo, q, out ulong chkLoLo);

			chkHiLo += chkLoHi;
			if (chkHiLo < chkLoHi)
			{
				chkHiHi++;
			}

			return (chkHiHi > valHi1)
			       || ((chkHiHi == valHi1) && ((chkHiLo > valHi0) || ((chkHiLo == valHi0) && (chkLoLo > valLo))));
		}

		static ulong SubtractDivisor(Span<ulong> left, ReadOnlySpan<ulong> right, ulong multiplier)
		{
			// Combines a subtract and a multiply operation, which is naturally
			// more efficient than multiplying and then subtracting...

			int length = right.Length;
			int i = 0;
			ulong carry = 0;
			
			for (; i + 3 < length; i += 4)
            {
                UInt128 prod0 = (UInt128)(ulong)right[i] * (ulong)multiplier + (ulong)carry;
                ulong lo0 = (ulong)prod0;
                ulong hi0 = (ulong)(prod0 >> 64);
                ulong orig0 = left[i];
                left[i] = orig0 - lo0;
                hi0 += (orig0 < lo0) ? 1UL : 0;

                UInt128 prod1 = (UInt128)(ulong)right[i + 1] * (ulong)multiplier + (ulong)hi0;
                ulong lo1 = (ulong)prod1;
                ulong hi1 = (ulong)(prod1 >> 64);
                ulong orig1 = left[i + 1];
                left[i + 1] = orig1 - lo1;
                hi1 += (orig1 < lo1) ? 1UL : 0;

                UInt128 prod2 = (UInt128)(ulong)right[i + 2] * (ulong)multiplier + (ulong)hi1;
                ulong lo2 = (ulong)prod2;
                ulong hi2 = (ulong)(prod2 >> 64);
                ulong orig2 = left[i + 2];
                left[i + 2] = orig2 - lo2;
                hi2 += (orig2 < lo2) ? 1UL : 0;

                UInt128 prod3 = (UInt128)(ulong)right[i + 3] * (ulong)multiplier + (ulong)hi2;
                ulong lo3 = (ulong)prod3;
                ulong hi3 = (ulong)(prod3 >> 64);
                ulong orig3 = left[i + 3];
                left[i + 3] = orig3 - lo3;
                hi3 += (orig3 < lo3) ? 1UL : 0;

                carry = hi3;
            }

            for (; i < length; i++)
            {
                UInt128 product = (UInt128)(ulong)right[i] * (ulong)multiplier + (ulong)carry;
                ulong lo = (ulong)product;
                ulong hi = (ulong)(product >> 64);
                ulong orig = left[i];
                left[i] = orig - lo;
                hi += (orig < lo) ? 1UL : 0;
                carry = hi;
            }

			return carry;
		}
	}
	
	internal static ulong Remainder(in UInt256 left, ulong right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong carry;
		UInt128 value;

		if (left.Part3 != 0)
		{
			carry = left.Part3 % right;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part2 != 0)
		{
			carry = left.Part2 % right;

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
	
	internal static ulong Remainder(in UInt512 left, ulong right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		ulong carry;
		UInt128 value;

		if (left.Part7 != 0)
		{
			carry = left.Part7 % right;

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
			carry = left.Part6 % right;

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
			carry = left.Part5 % right;
			
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
			carry = left.Part4 % right;

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
			carry = left.Part3 % right;

			value = new UInt128(carry, left.Part2);
			carry = DivRemByUInt64(value, right).Remainder;

			value = new UInt128(carry, left.Part1);
			carry = DivRemByUInt64(value, right).Remainder;
			
			value = new UInt128(carry, left.Part0);
			carry = DivRemByUInt64(value, right).Remainder;
		}
		else if (left.Part2 != 0)
		{
			carry = left.Part2 % right;

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
	internal static void Remainder(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs
		// Same as above, but only returning the remainder.

		left.CopyTo(remainder);

		Divide(remainder, right, default);
	}

	internal static void Pow(ulong value, uint power, Span<ulong> bits)
	{
		Pow(value != 0 ? new ReadOnlySpan<ulong>(in value) : default, power, bits);
	}
	internal static void Pow(ReadOnlySpan<ulong> value, uint power, Span<ulong> bits)
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
			Multiply(left[..leftLength], right, temp[..resultLength]);
		}
		else
		{
			Multiply(right, left[..leftLength], temp[..resultLength]);
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

		Square(value[..valueLength], temp[..resultLength]);

		value.Clear();
		//switch buffers
		Span<ulong> t = value;
		value = temp;
		temp = t;
		return ActualLength(value[..resultLength]);
	}

	internal static int PowBound(uint power, int valueLength)
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

	internal static int ActualLength(ReadOnlySpan<ulong> value)
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