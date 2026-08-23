using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MissingValues.Internals;

namespace MissingValues;

public partial struct UInt256
{
	/// <inheritdoc/>
	public static UInt256 operator +(in UInt256 value) => value;

	/// <inheritdoc/>
	public static UInt256 operator +(in UInt256 left, in UInt256 right)
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

		return new UInt256(part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static UInt256 operator checked +(in UInt256 left, in UInt256 right)
	{
		ulong part0 = left._p0 + right._p0;
		ulong carry = (part0 < left._p0) ? 1UL : 0UL;

		ulong part1 = left._p1 + right._p1 + carry;
		carry = (part1 < left._p1 || (carry == 1 && part1 == left._p1)) ? 1UL : 0UL;

		ulong part2 = left._p2 + right._p2 + carry;
		carry = (part2 < left._p2 || (carry == 1 && part2 == left._p2)) ? 1UL : 0UL;

		ulong part3 = checked(left._p3 + right._p3 + carry);

		return new UInt256(part3, part2, part1, part0);
	}

	/// <inheritdoc/>
	public static UInt256 operator -(in UInt256 value) => Zero - value;
	/// <inheritdoc/>
	public static UInt256 operator checked -(in UInt256 value) => checked(Zero - value);

	/// <inheritdoc/>
	public static UInt256 operator -(in UInt256 left, in UInt256 right)
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

		return new UInt256(part3, part2, part1, part0);
	}
	/// <inheritdoc/>
	public static UInt256 operator checked -(in UInt256 left, in UInt256 right)
	{
		// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
		// This gives us the borrow to subtract from upper to compute the correct result

		ulong part0 = left._p0 - right._p0;
		ulong borrow = (part0 > left._p0) ? 1UL : 0UL;

		ulong part1 = left._p1 - right._p1 - borrow;
		borrow = (part1 > left._p1 || (borrow == 1UL && part1 == left._p1)) ? 1UL : 0UL;

		ulong part2 = left._p2 - right._p2 - borrow;
		borrow = (part2 > left._p2 || (borrow == 1UL && part2 == left._p2)) ? 1UL : 0UL;

		ulong part3 = checked(left._p3 - right._p3 - borrow);

		return new UInt256(part3, part2, part1, part0);

	}

	/// <inheritdoc/>
	public static UInt256 operator ~(in UInt256 value)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v = Vector256.OnesComplement(value.AsVector<ulong>());
			return Create(v);
		}
		else
		{
			return new(~value._p3, ~value._p2, ~value._p1, ~value._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt256 operator ++(in UInt256 value) => value + One;
	/// <inheritdoc/>
	public static UInt256 operator checked ++(in UInt256 value) => checked(value + One);

	/// <inheritdoc/>
	public static UInt256 operator --(in UInt256 value) => value - One;
	/// <inheritdoc/>
	public static UInt256 operator checked --(in UInt256 value) => checked(value - One);

	/// <inheritdoc/>
	public static UInt256 operator *(in UInt256 left, in UInt256 right)
	{
		ulong up, low;
			
		if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
		{
			if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
			{
				up = Math.BigMul(left._p0, right._p0, out low);
				return new UInt256(0, 0, up, low);
			}

			return Calculator.Multiply(in left, right._p0, out _);
		}
		else if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
		{
			return Calculator.Multiply(in right, left._p0, out _);
		}

		(up, low) = Calculator.BigMulAdd(left._p0, right._p0, 0);
		ulong p0 = low;
		(up, low) = Calculator.BigMulAdd(left._p1, right._p0, up);
		ulong p1 = low;
		(up, low) = Calculator.BigMulAdd(left._p2, right._p0, up);
		ulong p2 = low;
		(_, low) = Calculator.BigMulAdd(left._p3, right._p0, up);
		ulong p3 = low;
        
		(up, low) = Calculator.BigMulAdd(left._p0, right._p1, 0);
		p1 = Calculator.AddWithCarry(p1, low, out ulong carry);
		up += carry;
		(up, low) = Calculator.BigMulAdd(left._p1, right._p1, up);
		p2 = Calculator.AddWithCarry(p2, low, out carry);
		up += carry;
		(_, low) = Calculator.BigMulAdd(left._p2, right._p1, up);
		p3 += low;

		(up, low) = Calculator.BigMulAdd(left._p0, right._p2, 0);
		p2 = Calculator.AddWithCarry(p2, low, out carry);
		up += carry;
		(_, low) = Calculator.BigMulAdd(left._p1, right._p2, up);
		p3 += low;
        
		(_, low) = Calculator.BigMulAdd(left._p0, right._p3, 0);
		p3 += low;
        
		return new UInt256(p3, p2, p1, p0);
	}
	/// <inheritdoc/>
	public static UInt256 operator checked *(in UInt256 left, in UInt256 right)
	{
		UInt256 upper = BigMul(left, right, out UInt256 lower);

		if (upper != Zero)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Multiplication);
		}

		return lower;
	}

	/// <inheritdoc/>
	public static UInt256 operator /(in UInt256 left, in UInt256 right)
	{
		const int UIntCount = Size / sizeof(ulong);

		if (right._p3 == 0 && right._p2 == 0)
		{
			if (right._p1 == 0)
			{
				if (right._p0 == 0)
				{
					Thrower.DivideByZero();
				}
				return Calculator.Divide(in left, right._p0);
			}
		}

		if (right >= left)
		{
			return (right == left) ? One : Zero;
		}

		Span<ulong> quotientSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(quotientSpan, in left);

		Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(divisorSpan, in right);

		Span<ulong> rawBits = stackalloc ulong[UIntCount];
		rawBits.Clear();

		Calculator.Divide(
			quotientSpan[..BitHelper.GetTrimLength(in left)],
			divisorSpan[..BitHelper.GetTrimLength(in right)],
			rawBits);

		return new UInt256(rawBits);
	}

	/// <inheritdoc/>
	public static UInt256 operator checked /(in UInt256 left, in UInt256 right) => left / right;

	/// <inheritdoc/>
	public static UInt256 operator %(in UInt256 left, in UInt256 right)
	{
		const int UIntCount = Size / sizeof(ulong);

		if (right._p3 == 0 && right._p2 == 0)
		{
			if (right._p1 == 0)
			{
				if (right._p0 == 0)
				{
					Thrower.DivideByZero();
				}
				return Calculator.Remainder(in left, right._p0);
			}
		}

		if (right == left)
		{
			return Zero;
		}

		if (right > left)
		{
			return left;
		}

		Span<ulong> quotientSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(quotientSpan, in left);

		Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(divisorSpan, in right);

		Span<ulong> rawBits = stackalloc ulong[UIntCount];
		rawBits.Clear();

		Calculator.Remainder(
			quotientSpan[..BitHelper.GetTrimLength(in left)],
			divisorSpan[..BitHelper.GetTrimLength(in right)],
			rawBits);

		return new UInt256(rawBits);
	}

	/// <inheritdoc/>
	public static UInt256 operator &(in UInt256 left, in UInt256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 & v2;
			return Create(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = Avx2.And(v1, v2);
			return Create(result);
		}
		else
		{
			return new(left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt256 operator |(in UInt256 left, in UInt256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 | v2;
			return Create(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = Avx2.Or(v1, v2);
			return Create(result);
		}
		else
		{
			return new(left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt256 operator ^(in UInt256 left, in UInt256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = v1 ^ v2;
			return Create(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			var result = Avx2.Xor(v1, v2);
			return Create(result);
		}
		else
		{
			return new(left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt256 operator <<(in UInt256 value, int shiftAmount)
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

			return new UInt256(part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new UInt256(value._p2, value._p1, value._p0, 0);
			}

			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt256(part2, part1, part0, 0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new UInt256(value._p1, value._p0, 0, 0);
			}

			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt256(part1, part0, 0, 0);
		}
		else // shiftAmount < 256
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new UInt256(value._p0, 0, 0, 0);
			}

			ulong part0 = value._p0 << shiftAmount;

			return new UInt256(part0, 0, 0, 0);
		}
	}

	/// <inheritdoc/>
	public static UInt256 operator >>(in UInt256 value, int shiftAmount) => value >>> shiftAmount;

	/// <inheritdoc/>
	public static bool operator ==(in UInt256 left, in UInt256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			return v1 == v2;
		}
		else if (Avx2.IsSupported)
		{
			var v1 = left.AsVector<byte>();
			var v2 = right.AsVector<byte>();
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
	public static bool operator !=(in UInt256 left, in UInt256 right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = left.AsVector<ulong>();
			var v2 = right.AsVector<ulong>();
			return v1 != v2;
		}
		else if (Avx2.IsSupported)
		{
			var v1 = left.AsVector<byte>();
			var v2 = right.AsVector<byte>();
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
	public static bool operator <(in UInt256 left, in UInt256 right)
	{
		// Successively compare each part.
		return (left._p3 < right._p3)
		       || (left._p3 == right._p3 && ((left._p2 < right._p2)
		                                     || (left._p2 == right._p2 && ((left._p1 < right._p1)
		                                                                   || (left._p1 == right._p1 && (left._p0 < right._p0))))));

	}

	/// <inheritdoc/>
	public static bool operator >(in UInt256 left, in UInt256 right)
	{
		return (left._p3 > right._p3)
		       || (left._p3 == right._p3 && ((left._p2 > right._p2)
		                                     || (left._p2 == right._p2 && ((left._p1 > right._p1)
		                                                                   || (left._p1 == right._p1 && (left._p0 > right._p0))))));
	}

	/// <inheritdoc/>
	public static bool operator <=(in UInt256 left, in UInt256 right)
	{
		return (left._p3 < right._p3)
		       || (left._p3 == right._p3 && ((left._p2 < right._p2)
		                                     || (left._p2 == right._p2 && ((left._p1 < right._p1)
		                                                                   || (left._p1 == right._p1 && (left._p0 <= right._p0))))));
	}

	/// <inheritdoc/>
	public static bool operator >=(in UInt256 left, in UInt256 right)
	{
		return (left._p3 > right._p3)
		       || (left._p3 == right._p3 && ((left._p2 > right._p2)
		                                     || (left._p2 == right._p2 && ((left._p1 > right._p1)
		                                                                   || (left._p1 == right._p1 && (left._p0 >= right._p0))))));
	}

	/// <inheritdoc/>
	public static UInt256 operator >>>(in UInt256 value, int shiftAmount)
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

			return new UInt256(part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new UInt256(0, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = value._p3 >> shiftAmount;

			return new UInt256(0, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new UInt256(0, 0, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = value._p3 >> shiftAmount;

			return new UInt256(0, 0, part1, part0);
		}
		else // shiftAmount < 256
		{
			shiftAmount -= 192;

			ulong part0 = value._p3 >> shiftAmount;

			return new UInt256(0, 0, 0, part0);
		}
	}
}