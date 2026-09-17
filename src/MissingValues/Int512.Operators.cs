using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MissingValues.Internals;

namespace MissingValues;

public partial struct Int512
{
	/// <inheritdoc/>
	public static Int512 operator +(in Int512 value) => value;

	/// <inheritdoc/>
	public static Int512 operator +(in Int512 left, in Int512 right)
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
		carry = (part3 < left._p3 || (carry == 1 && part3 == left._p3)) ? 1UL : 0UL;

		ulong part4 = left._p4 + right._p4 + carry;
		carry = (part4 < left._p4 || (carry == 1 && part4 == left._p4)) ? 1UL : 0UL;

		ulong part5 = left._p5 + right._p5 + carry;
		carry = (part5 < left._p5 || (carry == 1 && part5 == left._p5)) ? 1UL : 0UL;

		ulong part6 = left._p6 + right._p6 + carry;
		carry = (part6 < left._p6 || (carry == 1 && part6 == left._p6)) ? 1UL : 0UL;

		ulong part7 = left._p7 + right._p7 + carry;
		return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static Int512 operator checked +(in Int512 left, in Int512 right)
	{
		Int512 result = left + right;

		if ((long)((result._p7 ^ left._p7) & ~(left._p7 ^ right._p7)) < 0)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Addition);
		}
		return result;
	}

	/// <inheritdoc/>
	public static Int512 operator -(in Int512 value) => Zero - value;
	/// <inheritdoc/>
	public static Int512 operator checked -(in Int512 value) => checked(Zero - value);

	/// <inheritdoc/>
	public static Int512 operator -(in Int512 left, in Int512 right)
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
		borrow = (part3 > left._p3 || (borrow == 1UL && part3 == left._p3)) ? 1UL : 0UL;

		ulong part4 = left._p4 - right._p4 - borrow;
		borrow = (part4 > left._p4 || (borrow == 1UL && part4 == left._p4)) ? 1UL : 0UL;

		ulong part5 = left._p5 - right._p5 - borrow;
		borrow = (part5 > left._p5 || (borrow == 1UL && part5 == left._p5)) ? 1UL : 0UL;

		ulong part6 = left._p6 - right._p6 - borrow;
		borrow = (part6 > left._p6 || (borrow == 1UL && part6 == left._p6)) ? 1UL : 0UL;

		ulong part7 = left._p7 - right._p7 - borrow;

		return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static Int512 operator checked -(in Int512 left, in Int512 right)
	{
		// For signed subtraction, we can detect overflow by checking if the sign of
		// both inputs are different and then if that differs from the sign of the
		// output.

		Int512 result = left - right;

		uint sign = (uint)(left._p7 >> 63);

		if (sign != (uint)(right._p7 >> 63) && sign != (uint)(result._p7 >> 63))
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Subtraction);
		}
		return result;
	}

	/// <inheritdoc/>
	public static Int512 operator ~(in Int512 value)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v = Unsafe.BitCast<Int512, Vector512<ulong>>(value);
			var result = ~v;
			return Unsafe.BitCast<Vector512<ulong>, Int512>(result);
		}
		else
		{
			return new(~value._p7, ~value._p6, ~value._p5, ~value._p4, ~value._p3, ~value._p2, ~value._p1, ~value._p0);
		}
	}

	/// <inheritdoc/>
	public static Int512 operator ++(in Int512 value) => value + One;
	/// <inheritdoc/>
	public static Int512 operator checked ++(in Int512 value) => checked(value + One);

	/// <inheritdoc/>
	public static Int512 operator --(in Int512 value) => value - One;
	/// <inheritdoc/>
	public static Int512 operator checked --(in Int512 value) => checked(value - One);

	/// <inheritdoc/>
	public static Int512 operator *(in Int512 left, in Int512 right) => (Int512)((UInt512)left * (UInt512)right);
	/// <inheritdoc/>
	public static Int512 operator checked *(in Int512 left, in Int512 right)
	{
		Int512 upper = BigMul(left, right, out Int512 lower);

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
	public static Int512 operator /(in Int512 left, in Int512 right)
	{
		DivRem(in left, in right, out Int512 quotient, out _);
		return quotient;
	}

	/// <inheritdoc/>
	public static Int512 operator checked /(in Int512 left, in Int512 right) => left / right;

	/// <inheritdoc/>
	public static Int512 operator %(in Int512 left, in Int512 right)
	{
		DivRem(in left, in right, out _, out Int512 remainder);
		return remainder;
	}

	/// <inheritdoc/>
	public static Int512 operator &(in Int512 left, in Int512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 & v2;
			return Create(result);
		}
		else
		{
			return new(left._p7 & right._p7, left._p6 & right._p6, left._p5 & right._p5, left._p4 & right._p4, left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int512 operator |(in Int512 left, in Int512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 | v2;
			return Create(result);
		}
		else
		{
			return new(left._p7 | right._p7, left._p6 | right._p6, left._p5 | right._p5, left._p4 | right._p4, left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int512 operator ^(in Int512 left, in Int512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 ^ v2;
			return Create(result);
		}
		else
		{
			return new(left._p7 ^ right._p7, left._p6 ^ right._p6, left._p5 ^ right._p5, left._p4 ^ right._p4, left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
		}
	}

	/// <inheritdoc/>
	public static Int512 operator <<(in Int512 value, int shiftAmount)
	{
		// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
		// need to specially handle things if the shift amount exceeds 0x3F.

		shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 255]

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part7 = (value._p7 << shiftAmount) | (value._p6 >> (64 - shiftAmount));
			ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
			ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int512(value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0);
			}

			ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
			ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part6, part5, part4, part3, part2, part1, part0, 0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int512(value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0);
			}

			ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part5, part4, part3, part2, part1, part0, 0, 0);
		}
		else if (shiftAmount < 256)
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new Int512(value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0, 0);
			}

			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part4, part3, part2, part1, part0, 0, 0, 0);
		}
		else if (shiftAmount < 320)
		{
			shiftAmount -= 256;

			if (shiftAmount == 0)
			{
				return new Int512(value._p3, value._p2, value._p1, value._p0, 0, 0, 0, 0);
			}

			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part3, part2, part1, part0, 0, 0, 0, 0);
		}
		else if (shiftAmount < 384)
		{
			shiftAmount -= 320;

			if (shiftAmount == 0)
			{
				return new Int512(value._p2, value._p1, value._p0, 0, 0, 0, 0, 0);
			}

			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part2, part1, part0, 0, 0, 0, 0, 0);
		}
		else if (shiftAmount < 448)
		{
			shiftAmount -= 384;

			if (shiftAmount == 0)
			{
				return new Int512(value._p1, value._p0, 0, 0, 0, 0, 0, 0);
			}

			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part1, part0, 0, 0, 0, 0, 0, 0);
		}
		else // shiftAmount < 512
		{
			shiftAmount -= 448;

			if (shiftAmount == 0)
			{
				return new Int512(value._p0, 0, 0, 0, 0, 0, 0, 0);
			}

			ulong part0 = value._p0 << shiftAmount;

			return new Int512(part0, 0, 0, 0, 0, 0, 0, 0);
		}
	}

	/// <inheritdoc/>
	public static Int512 operator >>(in Int512 value, int shiftAmount)
	{
		// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
		// need to specially handle things if the shift amount exceeds 0x3F.

		shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 511]

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
			ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part3 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part4 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part5 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part6 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part7 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
		}

		ulong preservedSign = (ulong)((long)value._p7 >> 63);

		if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part6 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part5 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 256)
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3);
			}

			ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part4 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, preservedSign, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 320)
		{
			shiftAmount -= 256;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4);
			}

			ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part3 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, part3, part2, part1, part0);
		}
		else if (shiftAmount < 384)
		{
			shiftAmount -= 320;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5);
			}

			ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part2 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part2, part1, part0);
		}
		else if (shiftAmount < 448)
		{
			shiftAmount -= 384;

			if (shiftAmount == 0)
			{
				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6);
			}

			ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part1 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part1, part0);
		}
		else // shiftAmount < 512
		{
			shiftAmount -= 448;

			ulong part0 = (ulong)((long)value._p7 >> shiftAmount);

			return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part0);
		}
	}

	/// <inheritdoc/>
	public static bool operator ==(in Int512 left, in Int512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			return v1 == v2;
		}
		if (Vector256.IsHardwareAccelerated)
		{
			Vector256<ulong> vUpper1 = Vector256.Create(left._p4, left._p5, left._p6, left._p7);
			Vector256<ulong> vLower1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3);
			
			Vector256<ulong> vUpper2 = Vector256.Create(right._p4, right._p5, right._p6, right._p7);
			Vector256<ulong> vLower2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3);

			return vUpper1 == vUpper2 && vLower1 == vLower2;
		}

		return (left._p7 == right._p7) && (left._p6 == right._p6) && (left._p5 == right._p5) && (left._p4 == right._p4)
		       && (left._p3 == right._p3) && (left._p2 == right._p2) && (left._p1 == right._p1) && (left._p0 == right._p0);
	}

	/// <inheritdoc/>
	public static bool operator !=(in Int512 left, in Int512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			return v1 != v2;
		}
		if (Vector256.IsHardwareAccelerated)
		{
			Vector256<ulong> vUpper1 = Vector256.Create(left._p4, left._p5, left._p6, left._p7);
			Vector256<ulong> vLower1 = Vector256.Create(left._p0, left._p1, left._p2, left._p3);
			
			Vector256<ulong> vUpper2 = Vector256.Create(right._p4, right._p5, right._p6, right._p7);
			Vector256<ulong> vLower2 = Vector256.Create(right._p0, right._p1, right._p2, right._p3);

			return vUpper1 != vUpper2 || vLower1 != vLower2;
		}

		return (left._p7 != right._p7) || (left._p6 != right._p6) || (left._p5 != right._p5) || (left._p4 != right._p4)
		       || (left._p3 != right._p3) || (left._p2 != right._p2) || (left._p1 != right._p1) || (left._p0 != right._p0);
	}

	/// <inheritdoc/>
	public static bool operator <(in Int512 left, in Int512 right)
	{
		// Successively compare each part.
		// If left and right have different signs: Signed comparison of _p7 gives result since it is stored as two's complement
		// If signs are equal and left._p7 < right._p7: left < right for negative and positive values,
		//                                                    since _p7 is upper 64 bits in two's complement.
		// If signs are equal and left._p7 > right._p7: left > right for negative and positive values,
		//                                                    since _p7 is upper 64 bits in two's complement.
		// If left._p7 == right._p7: unsigned comparison of lower bits gives the result for both negative and positive values since
		//                                 lower values are lower 64 bits in two's complement.
		return ((long)left._p7 < (long)right._p7)
			|| (left._p7 == right._p7 && ((left._p6 < right._p6)
			|| (left._p6 == right._p6 && ((left._p5 < right._p5)
			|| (left._p5 == right._p5 && ((left._p4 < right._p4)
			|| (left._p4 == right._p4 && ((left._p3 < right._p3)
			|| (left._p3 == right._p3 && ((left._p2 < right._p2)
			|| (left._p2 == right._p2 && ((left._p1 < right._p1)
			|| (left._p1 == right._p1 && (left._p0 < right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator >(in Int512 left, in Int512 right)
	{
		return ((long)left._p7 > (long)right._p7)
			|| (left._p7 == right._p7 && ((left._p6 > right._p6)
			|| (left._p6 == right._p6 && ((left._p5 > right._p5)
			|| (left._p5 == right._p5 && ((left._p4 > right._p4)
			|| (left._p4 == right._p4 && ((left._p3 > right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 > right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator <=(in Int512 left, in Int512 right)
	{
		return ((long)left._p7 < (long)right._p7)
			|| (left._p7 == right._p7 && ((left._p6 < right._p6)
			|| (left._p6 == right._p6 && ((left._p5 < right._p5)
			|| (left._p5 == right._p5 && ((left._p4 < right._p4)
			|| (left._p4 == right._p4 && ((left._p3 < right._p3)
			|| (left._p3 == right._p3 && ((left._p2 < right._p2)
			|| (left._p2 == right._p2 && ((left._p1 < right._p1)
			|| (left._p1 == right._p1 && (left._p0 <= right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator >=(in Int512 left, in Int512 right)
	{
		return ((long)left._p7 > (long)right._p7)
			|| (left._p7 == right._p7 && ((left._p6 > right._p6)
			|| (left._p6 == right._p6 && ((left._p5 > right._p5)
			|| (left._p5 == right._p5 && ((left._p4 > right._p4)
			|| (left._p4 == right._p4 && ((left._p3 > right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 >= right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static Int512 operator >>>(in Int512 value, int shiftAmount)
	{
		// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
		// need to specially handle things if the shift amount exceeds 0x3F.

		shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 511]

		if (shiftAmount == 0)
		{
			return value;
		}

		if (shiftAmount < 64)
		{
			ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
			ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part3 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part4 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part5 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part6 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part7 = value._p7 >> shiftAmount;

			return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new Int512(0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part6 = value._p7 >> shiftAmount;

			return new Int512(0, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new Int512(0, 0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part5 = value._p7 >> shiftAmount;

			return new Int512(0, 0, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 256)
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new Int512(0, 0, 0, value._p7, value._p6, value._p5, value._p4, value._p3);
			}

			ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part4 = value._p7 >> shiftAmount;

			return new Int512(0, 0, 0, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 320)
		{
			shiftAmount -= 256;

			if (shiftAmount == 0)
			{
				return new Int512(0, 0, 0, 0, value._p7, value._p6, value._p5, value._p4);
			}

			ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part3 = value._p7 >> shiftAmount;

			return new Int512(0, 0, 0, 0, part3, part2, part1, part0);
		}
		else if (shiftAmount < 384)
		{
			shiftAmount -= 320;

			if (shiftAmount == 0)
			{
				return new Int512(0, 0, 0, 0, 0, value._p7, value._p6, value._p5);
			}

			ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part2 = value._p7 >> shiftAmount;

			return new Int512(0, 0, 0, 0, 0, part2, part1, part0);
		}
		else if (shiftAmount < 448)
		{
			shiftAmount -= 384;

			if (shiftAmount == 0)
			{
				return new Int512(0, 0, 0, 0, 0, 0, value._p7, value._p6);
			}

			ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part1 = value._p7 >> shiftAmount;

			return new Int512(0, 0, 0, 0, 0, 0, part1, part0);
		}
		else // shiftAmount < 512
		{
			shiftAmount -= 448;

			ulong part0 = value._p7 >> shiftAmount;

			return new Int512(0, 0, 0, 0, 0, 0, 0, part0);
		}
	}
}