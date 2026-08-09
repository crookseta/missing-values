using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MissingValues.Internals;

namespace MissingValues;

public partial struct Int256
{
	/// <inheritdoc/>
	public static Int256 operator +(in Int256 value) => value;

	/// <inheritdoc/>
	public static Int256 operator +(in Int256 left, in Int256 right)
	{
		// For unsigned addition, we can detect overflow by checking `(x + y) < x`
		// This gives us the carry to add to upper to compute the correct result

		ulong part0 = left._p0 + right._p0;
		ulong carry = (part0 < left._p0) ? 1UL : 0UL;

		ulong part1 = left._p1 + right._p1 + carry;
		carry = (part1 < left._p1 || (carry == 1 && part1 == left._p1)) ? 1UL : 0UL;

		ulong part2 = left._p2 + right._p2 + carry;
		carry = (part2 < left._p2 || (carry == 1 && part2 == left._p2)) ? 1UL : 0UL;

		ulong part3 = left._p3 + right._p3 + carry;

		return new Int256(part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static Int256 operator checked +(in Int256 left, in Int256 right)
	{
		// For signed addition, we can detect overflow by checking if the sign of
		// both inputs are the same and then if that differs from the sign of the
		// output.

		Int256 result = left + right;

		if ((long)((result._p3 ^ left._p3) & ~(left._p3 ^ right._p3)) < 0)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Addition);
		}
		return result;
	}

	/// <inheritdoc/>
	public static Int256 operator -(in Int256 value) => Zero - value;
	/// <inheritdoc/>
	public static Int256 operator checked -(in Int256 value) => checked(Zero - value);

	/// <inheritdoc/>
	public static Int256 operator -(in Int256 left, in Int256 right)
	{
		// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
		// This gives us the borrow to subtract from upper to compute the correct result

		ulong part0 = left._p0 - right._p0;
		ulong borrow = (part0 > left._p0) ? 1UL : 0UL;

		ulong part1 = left._p1 - right._p1 - borrow;
		borrow = (part1 > left._p1 || (borrow == 1UL && part1 == left._p1)) ? 1UL : 0UL;

		ulong part2 = left._p2 - right._p2 - borrow;
		borrow = (part2 > left._p2 || (borrow == 1UL && part2 == left._p2)) ? 1UL : 0UL;

		ulong part3 = left._p3 - right._p3 - borrow;

		return new Int256(part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static Int256 operator checked -(in Int256 left, in Int256 right)
	{
		// For signed subtraction, we can detect overflow by checking if the sign of
		// both inputs are different and then if that differs from the sign of the
		// output.

		Int256 result = left - right;

		uint sign = (uint)(left._p3 >> 63);

		if (sign != (uint)(right._p3 >> 63) && sign != (uint)(result._p3 >> 63))
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Subtraction);
		}
		return result;
	}

	/// <inheritdoc/>
	public static Int256 operator ~(in Int256 value)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v = Unsafe.BitCast<Int256, Vector256<ulong>>(value);
			var result = ~v;
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else
		{
			return new(~value._p3, ~value._p2, ~value._p1, ~value._p0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator ++(in Int256 value) => value + One;
	/// <inheritdoc/>
	public static Int256 operator checked ++(in Int256 value) => checked(value + One);

	/// <inheritdoc/>
	public static Int256 operator --(in Int256 value) => value - One;
	/// <inheritdoc/>
	public static Int256 operator checked --(in Int256 value) => checked(value - One);

	/// <inheritdoc/>
	public static Int256 operator *(in Int256 left, in Int256 right) => (Int256)((UInt256)(left) * (UInt256)(right));
	/// <inheritdoc/>
	public static Int256 operator checked *(in Int256 left, in Int256 right)
	{
		Int256 upper = BigMul(left, right, out Int256 lower);

		if (((upper != Zero) || (lower < Zero)) && ((~upper != Zero) || (lower >= Zero)))
		{
			// The upper bits can safely be either Zero or AllBitsSet
			// where the former represents a positive value and the
			// latter a negative value.
			//
			// However, when the upper bits are Zero, we also need to
			// confirm the lower bits are positive, otherwise we have
			// a positive value greater than MaxValue and should throw
			//
			// Likewise, when the upper bits are AllBitsSet, we also
			// need to confirm the lower bits are negative, otherwise
			// we have a large negative value less than MinValue and
			// should throw.

			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Multiplication);
		}

		return lower;
	}

	/// <inheritdoc/>
	public static Int256 operator /(in Int256 left, in Int256 right)
	{
		DivRem(in left, in right, out Int256 quotient, out _);
		return quotient;
	}

	/// <inheritdoc/>
	public static Int256 operator checked /(in Int256 left, in Int256 right) => left / right;

	/// <inheritdoc/>
	public static Int256 operator %(in Int256 left, in Int256 right)
	{
		DivRem(in left, in right, out _, out Int256 remainder);
		return remainder;
	}

	/// <inheritdoc/>
	public static Int256 operator &(in Int256 left, in Int256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = v1 & v2;
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = Avx2.And(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else
		{
			return new(left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator |(in Int256 left, in Int256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = v1 | v2;
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = Avx2.Or(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else
		{
			return new(left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator ^(in Int256 left, in Int256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = v1 ^ v2;
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
			var result = Avx2.Xor(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
		}
		else
		{
			return new(left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator <<(in Int256 value, int shiftAmount)
	{
		// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
		// need to specially handle things if the shift amount exceeds 0x3F.

		shiftAmount &= 0xFF; // mask the shift amount to be within [0, 255]

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int256(part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int256(value._p2, value._p1, value._p0, 0);
			}

			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int256(part2, part1, part0, 0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int256(value._p1, value._p0, 0, 0);
			}

			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int256(part1, part0, 0, 0);
		}
		else // shiftAmount < 256
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new Int256(value._p0, 0, 0, 0);
			}

			ulong part0 = value._p0 << shiftAmount;

			return new Int256(part0, 0, 0, 0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator >>(in Int256 value, int shiftAmount)
	{
		// need to specially handle things if the 15th bit is set.

		shiftAmount &= 0xFF;

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
			ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part3 = (ulong)((long)value._p3 >> shiftAmount);

			return new Int256(part3, part2, part1, part0);
		}

		ulong preservedSign = (ulong)((long)value._p3 >> 63);

		if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int256(preservedSign, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = (ulong)((long)value._p3 >> shiftAmount);

			return new Int256(preservedSign, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int256(preservedSign, preservedSign, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = (ulong)((long)value._p3 >> shiftAmount);

			return new Int256(preservedSign, preservedSign, part1, part0);
		}
		else // shiftAmount < 256
		{
			shiftAmount -= 192;

			ulong part0 = (ulong)((long)value._p3 >> shiftAmount);

			return new Int256(preservedSign, preservedSign, preservedSign, part0);
		}
	}

	/// <inheritdoc/>
	public static Int256 operator >>>(in Int256 value, int shiftAmount)
	{
		// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
		// need to specially handle things if the shift amount exceeds 0x3F.

		shiftAmount &= 0xFF; // mask the shift amount to be within [0, 255]

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
			ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part3 = value._p3 >> shiftAmount;

			return new Int256(part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int256(0, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = value._p3 >> shiftAmount;

			return new Int256(0, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int256(0, 0, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = value._p3 >> shiftAmount;

			return new Int256(0, 0, part1, part0);
		}
		else // shiftAmount < 256
		{
			shiftAmount -= 192;

			ulong part0 = value._p3 >> shiftAmount;

			return new Int256(0, 0, 0, part0);
		}
	}

	/// <inheritdoc/>
	public static bool operator ==(in Int256 left, in Int256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3);
			var v2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3);
			return v1 == v2;
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3).AsByte();
			var v2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3).AsByte();
			var equals = Avx2.CompareEqual(v1, v2);
			var result = Avx2.MoveMask(equals);
			return (result & 0xFFFF_FFFF) == 0xFFFF_FFFF;
		}
		else
		{
			return (left._p3 == right._p3) && (left._p2 == right._p2) && (left._p1 == right._p1) && (left._p0 == right._p0);
		}
	}

	/// <inheritdoc/>
	public static bool operator !=(in Int256 left, in Int256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3);
			var v2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3);
			return v1 != v2;
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3).AsByte();
			var v2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3).AsByte();
			var equals = Avx2.CompareEqual(v1, v2);
			var result = Avx2.MoveMask(equals);
			return (result & 0xFFFF_FFFF) != 0xFFFF_FFFF;
		}
		else
		{
			return (left._p3 != right._p3) || (left._p2 != right._p2) || (left._p1 != right._p1) || (left._p0 != right._p0);
		}
	}

	/// <inheritdoc/>
	public static bool operator <(in Int256 left, in Int256 right)
	{
		// Successively compare each part.
		// If left and right have different signs: Signed comparison of _p3 gives result since it is stored as two's complement
		// If signs are equal and left._p3 < right._p3: left < right for negative and positive values,
		//                                                    since _p3 is upper 64 bits in two's complement.
		// If signs are equal and left._p3 > right._p3: left > right for negative and positive values,
		//                                                    since _p3 is upper 64 bits in two's complement.
		// If left._p3 == right._p3: unsigned comparison of lower bits gives the result for both negative and positive values since
		//                                 lower values are lower 64 bits in two's complement.
		return ((long)left._p3 < (long)right._p3)
			|| (left._p3 == right._p3 && ((left._p2 < right._p2)
			|| (left._p2 == right._p2 && ((left._p1 < right._p1)
			|| (left._p1 == right._p1 && (left._p0 < right._p0))))));
	}

	/// <inheritdoc/>
	public static bool operator >(in Int256 left, in Int256 right)
	{
		return ((long)left._p3 > (long)right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 > right._p0))))));
	}

	/// <inheritdoc/>
	public static bool operator <=(in Int256 left, in Int256 right)
	{
			return ((long)left._p3 < (long)right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 <= right._p0))))));
	}

	/// <inheritdoc/>
	public static bool operator >=(in Int256 left, in Int256 right)
	{
		return ((long)left._p3 > (long)right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 >= right._p0))))));
	}
}