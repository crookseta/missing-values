using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Internals;

internal static class Calculator
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt128 BigMul(ulong a, ulong b)
	{
		ulong high = Math.BigMul(a, b, out ulong low);
		return new UInt128(high, low);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt256 BigMul(UInt128 a, UInt128 b)
	{
		UInt128 high = BigMul(a, b, out var low);
		return new UInt256(high, low);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt512 BigMul(UInt256 a, UInt256 b)
	{
		UInt256 high = UInt256.BigMul(a, b, out var low);
		return new UInt512(high, low);
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

	public static UInt256 Multiply(in UInt256 left, ulong right, out ulong carry)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.SquMul.cs

		// Executes the multiplication for one big and one 64-bit integer.
		// Since every step holds the already slightly familiar equation
		// a_i * b + c <= 2^64 - 1 + (2^64 - 1)^2 < 2^128 - 1,
		// we are safe regarding to overflows.

		ulong p3, p2, p1, p0;

		UInt128 c = 0UL;
		
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

		UInt128 c = 0UL;

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

		return new UInt512(p7, p6, p5, p4, p3, p2, p1, p0);
	}

	public static void DivRem(ReadOnlySpan<ulong> left, ulong right, Span<ulong> quotient, out ulong remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Executes the division for one big and one 64-bit integer.
		// Thus, we've similar code than below, but there is no loop for
		// processing the 64-bit integer, since it's a single element.

		if (right <= uint.MaxValue)
		{
			DivRem(MemoryMarshal.Cast<ulong, uint>(left), unchecked((uint)right), MemoryMarshal.Cast<ulong, uint>(quotient), out uint rem);
			remainder = rem;
			return;
		}

		UInt128 carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			UInt128 value = (carry << 64) | left[i];
			UInt128 digit = value / right;
			quotient[i] = (ulong)digit;
			carry = value - digit * right;
		}
		remainder = (ulong)carry;
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
			ulong digit = value / right;
			quotient[i] = (uint)digit;
			carry = value - digit * right;
		}
		remainder = (uint)carry;
	}
	public static void DivRem(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> quotient, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		left.CopyTo(remainder);
		Divide(MemoryMarshal.Cast<ulong, uint>(remainder), MemoryMarshal.Cast<ulong, uint>(right), MemoryMarshal.Cast<ulong, uint>(quotient));
	}
	public static void DivRem(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient, Span<uint> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		left.CopyTo(remainder);
		Divide(remainder, right, quotient);
	}

	public static void Divide(ReadOnlySpan<ulong> left, ulong right, Span<ulong> quotient)
	{
        // Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

        // Same as above, but only computing the quotient.

        if (right <= uint.MaxValue)
        {
			Divide(MemoryMarshal.Cast<ulong, uint>(left), unchecked((uint)right), MemoryMarshal.Cast<ulong, uint>(quotient));
            return;
        }

        UInt128 carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			UInt128 value = (carry << 64) | left[i];
			UInt128 digit = value / right;
			quotient[i] = (ulong)digit;
			carry = value - digit * right;
		}
	}
	public static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Same as above, but only computing the quotient.

		ulong carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			ulong value = (carry << 32) | left[i];
			ulong digit = value / right;
			quotient[i] = (uint)digit;
			carry = value - digit * right;
		}
	}
	public static void Divide(Span<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> quotient)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		Span<ulong> bits = quotient.Slice(0, left.Length - right.Length + 1);

		ulong divHi = right[right.Length - 1];
		ulong divLo = right.Length > 1 ? right[right.Length - 2] : 0;

		int shift = BitOperations.LeadingZeroCount(divHi);
		int backShift = 64 - shift;

		if (shift > 0)
		{
			ulong divNx = right.Length > 2 ? right[right.Length - 3] : 0;

			divHi = (divHi << shift) | (divLo >> backShift);
			divLo = (divLo << shift) | (divNx >> backShift);
		}

		// Then, we divide all of the bits as we would do it using
		// pen and paper: guessing the next digit, subtracting, ...
		for (int i = left.Length; i >= right.Length; i--)
		{
			int n = i - right.Length;
			ulong t = ((ulong)(i) < (ulong)(left.Length)) ? left[i] : 0;

			//UInt128 valHi = ((UInt128)(t) << 64) | left[i - 1];
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
			UInt128 digit = valHi / divHi;

			if (digit > 0xFFFF_FFFF_FFFF_FFFF)
			{
				digit = 0xFFFF_FFFF_FFFF_FFFF;
			}

			// Our first guess may be a little bit to big
			while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo))
			{
				--digit;
			}

			if (digit > 0)
			{
				// Now it's time to subtract our current quotient
				ulong carry = SubtractDivisor(left.Slice(n), right, digit);

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
				bits[n] = (ulong)(digit);
			}

			if ((uint)(i) < (uint)(left.Length))
			{
				left[i] = 0;
			}
		}

		static ulong AddDivisor(Span<ulong> left, ReadOnlySpan<ulong> right)
		{
			UInt128 carry = UInt128.Zero;

			for (int i = 0; i < right.Length; i++)
			{
				ref ulong leftElement = ref left[i];
				UInt128 digit = (leftElement + carry) + right[i];

				leftElement = unchecked((ulong)digit);
				carry = digit >> 64;
			}

			return (ulong)carry;
		}

		static bool DivideGuessTooBig(UInt128 q, UInt128 valHi, ulong valLo, ulong divHi, ulong divLo)
		{
			UInt128 chkHi = divHi * q;
			UInt128 chkLo = divLo * q;

			chkHi += (chkLo >> 64);
			chkLo = (ulong)(chkLo);

			return (chkHi > valHi) || ((chkHi == valHi) && (chkLo > valLo));
		}

		static ulong SubtractDivisor(Span<ulong> left, ReadOnlySpan<ulong> right, UInt128 q)
		{
			// Combines a subtract and a multiply operation, which is naturally
			// more efficient than multiplying and then subtracting...

			UInt128 carry = UInt128.Zero;

			for (int i = 0; i < right.Length; i++)
			{
				carry += right[i] * q;

				ulong digit = (ulong)(carry);
				carry >>= 64;

				ref ulong leftElement = ref left[i];

				if (leftElement < digit)
				{
					++carry;
				}
				leftElement -= digit;
			}

			return (ulong)(carry);
		}
	}
	public static void Divide(Span<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		Span<uint> bits = quotient.Slice(0, left.Length - right.Length + 1);

		uint divHi = right[right.Length - 1];
		uint divLo = right.Length > 1 ? right[right.Length - 2] : 0;

		int shift = BitOperations.LeadingZeroCount(divHi);
		int backShift = 32 - shift;

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

				uint digit = (uint)(carry);
				carry >>= 32;

				ref uint leftElement = ref left[i];

				if (leftElement < digit)
				{
					++carry;
				}
				leftElement -= digit;
			}

			return (uint)(carry);
		}
	}

	public static ulong Remainder(ReadOnlySpan<ulong> left, ulong right)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs

		// Same as above, but only computing the remainder.

		if (right <= uint.MaxValue)
		{
			return Remainder(MemoryMarshal.Cast<ulong, uint>(left), unchecked((uint)right));
		}

		UInt128 carry = default;

		for (int i = left.Length - 1; i >= 0; i--)
		{
			//UInt128 value = (carry << 64) | left[i];
			UInt128 value = new UInt128((ulong)carry, left[i]);
			carry = value % right;
		}

		return (ulong)carry;
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
	public static void Remainder(ReadOnlySpan<ulong> left, ReadOnlySpan<ulong> right, Span<ulong> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs
		// Same as above, but only returning the remainder.

		left.CopyTo(remainder);

		var remainder32 = MemoryMarshal.Cast<ulong, uint>(remainder);
		var left32 = MemoryMarshal.Cast<ulong, uint>(left);
		var right32 = MemoryMarshal.Cast<ulong, uint>(right);

		Divide(remainder32, right32, stackalloc uint[left32.Length - right32.Length + 1]);
	}
	public static void Remainder(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> remainder)
	{
		// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Runtime.Numerics/src/System/Numerics/BigIntegerCalculator.DivRem.cs
		// Same as above, but only returning the remainder.

		left.CopyTo(remainder);

		Divide(remainder, right, stackalloc uint[left.Length - right.Length + 1]);
	}
}