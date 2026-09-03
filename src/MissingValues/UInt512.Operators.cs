using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MissingValues.Internals;

namespace MissingValues;

public partial struct UInt512
{
	/// <inheritdoc/>
	public static UInt512 operator +(in UInt512 value) => value;

	/// <inheritdoc/>
	public static UInt512 operator +(in UInt512 left, in UInt512 right)
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

		return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
	}

	/// <inheritdoc/>
	public static UInt512 operator checked +(in UInt512 left, in UInt512 right)
	{
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

		ulong part7 = checked(left._p7 + right._p7 + carry);

		return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
	}

	/// <inheritdoc/>
	public static UInt512 operator -(in UInt512 value) => Zero - value;

	/// <inheritdoc/>
	public static UInt512 operator checked -(in UInt512 value) => checked(Zero - value);

	/// <inheritdoc/>
	public static UInt512 operator -(in UInt512 left, in UInt512 right)
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

		return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
	}

	/// <inheritdoc/>
	public static UInt512 operator checked -(in UInt512 left, in UInt512 right)
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

		ulong part7 = checked(left._p7 - right._p7 - borrow);

		return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
	}

	/// <inheritdoc/>
	public static UInt512 operator ~(in UInt512 value)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v = Unsafe.BitCast<UInt512, Vector512<ulong>>(value);
			var result = ~v;
			return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
		}
		else
		{
			return new(~value._p7, ~value._p6, ~value._p5, ~value._p4, ~value._p3, ~value._p2, ~value._p1, ~value._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator ++(in UInt512 value) => value + One;

	/// <inheritdoc/>
	public static UInt512 operator checked ++(in UInt512 value) => checked(value + One);

	/// <inheritdoc/>
	public static UInt512 operator --(in UInt512 value) => value - One;

	/// <inheritdoc/>
	public static UInt512 operator checked --(in UInt512 value) => checked(value - One);

	/// <inheritdoc/>
	public static UInt512 operator *(in UInt512 left, in UInt512 right)
	{
		if (BitHelper.PopCount(in right) == 1)
		{
			return left << BitHelper.TrailingZeroCount(in right);
		}
		if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
		{
			return Multiply512X256(in left, in right);
		}
		if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0)
		{
			return Multiply512X256(in right, in left);
		}
		if (BitHelper.PopCount(in left) == 1)
		{
			return right << BitHelper.TrailingZeroCount(in left);
		}
		return MultiplySlow(in left, in right);

		static UInt512 MultiplySlow(in UInt512 left, in UInt512 right)
		{
			(ulong up, ulong low) = Calculator.BigMulAdd(left._p0, right._p0, 0);
			ulong p0 = low;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p0, up);
			ulong p1 = low;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p0, up);
			ulong p2 = low;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p0, up);
			ulong p3 = low;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p0, up);
			ulong p4 = low;
			(up, low) = Calculator.BigMulAdd(left._p5, right._p0, up);
			ulong p5 = low;
			(up, low) = Calculator.BigMulAdd(left._p6, right._p0, up);
			ulong p6 = low;
			(_, low) = Calculator.BigMulAdd(left._p7, right._p0, up);
			ulong p7 = low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p1, 0);
			p1 = Calculator.AddWithCarry(p1, low, out ulong carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p1, up);
			p2 = Calculator.AddWithCarry(p2, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p1, up);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p1, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p1, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p5, right._p1, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p6, right._p1, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p2, 0);
			p2 = Calculator.AddWithCarry(p2, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p2, up);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p2, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p2, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p2, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p5, right._p2, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p3, 0);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p3, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p3, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p3, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p4, right._p3, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p4, 0);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p4, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p4, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p3, right._p4, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p5, 0);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p5, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p2, right._p5, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p6, 0);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p1, right._p6, up);
			p7 += low;

			(_, low) = Calculator.BigMulAdd(left._p0, right._p7, 0);
			p7 += low;

			return new UInt512(p7, p6, p5, p4, p3, p2, p1, p0);
		}

		static UInt512 Multiply512X256(in UInt512 left, in UInt512 right)
		{
			if (right._p3 == 0 && right._p2 == 0)
			{
				if (right._p1 == 0)
				{
					if (right._p0 == 0)
					{
						return Zero;
					}
					if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0 && left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
					{
						if (left._p0 == 0)
						{
							return Zero;
						}
						ulong up = Calculator.BigMul(left._p0, right._p0, out ulong low);
						return new UInt512(0, 0, 0, 0, 0, 0, up, low);
					}

					return Calculator.Multiply(in left, right._p0, out _);
				}
				if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0)
				{
					UInt256 temp;
					
					if (left._p3 == 0 && left._p2 == 0)
					{
						if (left._p1 == 0 && left._p0 == 0)
						{
							return Zero;
						}
						temp = MathQ.BigMul(new UInt128(left._p1, left._p0), new UInt128(right._p1, right._p0));
						return new UInt512(0, 0, 0, 0, temp.Part3, temp.Part2, temp.Part1, temp.Part0);
					}

					temp = Calculator.Multiply(left.Lower, new UInt128(right._p1, right._p0), out UInt128 carry);
					return new UInt512(0, 0, carry.Upper, carry.Lower, temp.Part3, temp.Part2, temp.Part1, temp.Part0);
				}
				return Calculator.Multiply(in left, new UInt128(right._p1, right._p0), out _);
			}
			if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0)
			{
				if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					if (left._p0 == 0)
					{
						return Zero;
					}
					var temp = Calculator.Multiply(right.Lower, left._p0, out ulong low);
					return new UInt512(0, 0, 0, low, temp.Part3, temp.Part2, temp.Part1, temp.Part0);
				}

				return MathQ.BigMul(left.Lower, right.Lower);
			}

			return MultiplySlow512X256(in left, in right);
		}

		static UInt512 MultiplySlow512X256(in UInt512 left, in UInt512 right)
		{
			(ulong up, ulong low) = Calculator.BigMulAdd(left._p0, right._p0, 0);
			ulong p0 = low;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p0, up);
			ulong p1 = low;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p0, up);
			ulong p2 = low;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p0, up);
			ulong p3 = low;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p0, up);
			ulong p4 = low;
			(up, low) = Calculator.BigMulAdd(left._p5, right._p0, up);
			ulong p5 = low;
			(up, low) = Calculator.BigMulAdd(left._p6, right._p0, up);
			ulong p6 = low;
			(_, low) = Calculator.BigMulAdd(left._p7, right._p0, up);
			ulong p7 = low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p1, 0);
			p1 = Calculator.AddWithCarry(p1, low, out ulong carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p1, up);
			p2 = Calculator.AddWithCarry(p2, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p1, up);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p1, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p1, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p5, right._p1, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p6, right._p1, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p2, 0);
			p2 = Calculator.AddWithCarry(p2, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p2, up);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p2, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p2, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p4, right._p2, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p5, right._p2, up);
			p7 += low;

			(up, low) = Calculator.BigMulAdd(left._p0, right._p3, 0);
			p3 = Calculator.AddWithCarry(p3, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right._p3, up);
			p4 = Calculator.AddWithCarry(p4, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right._p3, up);
			p5 = Calculator.AddWithCarry(p5, low, out carry);
			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right._p3, up);
			p6 = Calculator.AddWithCarry(p6, low, out carry);
			up += carry;
			(_, low) = Calculator.BigMulAdd(left._p4, right._p3, up);
			p7 += low;
			
			return new UInt512(p7, p6, p5, p4, p3, p2, p1, p0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator checked *(in UInt512 left, in UInt512 right)
	{
		UInt512 upper = BigMul(left, right, out UInt512 lower);

		if (upper != Zero)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Multiplication);
		}

		return lower;
	}

	/// <inheritdoc/>
	public static UInt512 operator /(in UInt512 left, in UInt512 right)
	{
		const int UIntCount = Size / sizeof(ulong);

		if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
		{
			if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
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

		return new UInt512(rawBits);
	}

	/// <inheritdoc/>
	public static UInt512 operator checked /(in UInt512 left, in UInt512 right) => left / right;

	/// <inheritdoc/>
	public static UInt512 operator %(in UInt512 left, in UInt512 right)
	{
		const int UIntCount = Size / sizeof(ulong);

		if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
		{
			if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
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

		return new UInt512(rawBits);
	}

	/// <inheritdoc/>
	public static UInt512 operator &(in UInt512 left, in UInt512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
			var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
			var result = v1 & v2;
			return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
		}
		else
		{
			return new(left._p7 & right._p7, left._p6 & right._p6, left._p5 & right._p5, left._p4 & right._p4, left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator |(in UInt512 left, in UInt512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
			var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
			var result = v1 | v2;
			return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
		}
		else
		{
			return new(left._p7 | right._p7, left._p6 | right._p6, left._p5 | right._p5, left._p4 | right._p4, left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator ^(in UInt512 left, in UInt512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
			var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
			var result = v1 ^ v2;
			return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
		}
		else
		{
			return new(left._p7 ^ right._p7, left._p6 ^ right._p6, left._p5 ^ right._p5, left._p4 ^ right._p4, left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator <<(in UInt512 value, int shiftAmount)
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

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0);
			}

			ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
			ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part6, part5, part4, part3, part2, part1, part0, 0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0);
			}

			ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part5, part4, part3, part2, part1, part0, 0, 0);
		}
		else if (shiftAmount < 256)
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0, 0);
			}

			ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part4, part3, part2, part1, part0, 0, 0, 0);
		}
		else if (shiftAmount < 320)
		{
			shiftAmount -= 256;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p3, value._p2, value._p1, value._p0, 0, 0, 0, 0);
			}

			ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part3, part2, part1, part0, 0, 0, 0, 0);
		}
		else if (shiftAmount < 384)
		{
			shiftAmount -= 320;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p2, value._p1, value._p0, 0, 0, 0, 0, 0);
			}

			ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part2, part1, part0, 0, 0, 0, 0, 0);
		}
		else if (shiftAmount < 448)
		{
			shiftAmount -= 384;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p1, value._p0, 0, 0, 0, 0, 0, 0);
			}

			ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part1, part0, 0, 0, 0, 0, 0, 0);
		}
		else // shiftAmount < 512
		{
			shiftAmount -= 448;

			if (shiftAmount == 0)
			{
				return new UInt512(value._p0, 0, 0, 0, 0, 0, 0, 0);
			}

			ulong part0 = value._p0 << shiftAmount;

			return new UInt512(part0, 0, 0, 0, 0, 0, 0, 0);
		}
	}

	/// <inheritdoc/>
	public static UInt512 operator >>(in UInt512 value, int shiftAmount) => value >>> shiftAmount;

	/// <inheritdoc/>
	public static bool operator ==(in UInt512 left, in UInt512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = Vector512.Create(left._p0, left._p1, left._p2, left._p3, left._p4, left._p5, left._p6, left._p7);
			var v2 = Vector512.Create(right._p0, right._p1, right._p2, right._p3, right._p4, right._p5, right._p6, right._p7);
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
	public static bool operator !=(in UInt512 left, in UInt512 right)
	{
		if (Vector512.IsHardwareAccelerated)
		{
			var v1 = Vector512.Create(left._p0, left._p1, left._p2, left._p3, left._p4, left._p5, left._p6, left._p7);
			var v2 = Vector512.Create(right._p0, right._p1, right._p2, right._p3, right._p4, right._p5, right._p6, right._p7);
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
	public static bool operator <(in UInt512 left, in UInt512 right)
	{
		// Successively compare each part.
		return (left._p7 < right._p7)
			|| (left._p7 == right._p7 && ((left._p6 < right._p6)
			|| (left._p6 == right._p6 && ((left._p5 < right._p5)
			|| (left._p5 == right._p5 && ((left._p4 < right._p4)
			|| (left._p4 == right._p4 && ((left._p3 < right._p3)
			|| (left._p3 == right._p3 && ((left._p2 < right._p2)
			|| (left._p2 == right._p2 && ((left._p1 < right._p1)
			|| (left._p1 == right._p1 && (left._p0 < right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator >(in UInt512 left, in UInt512 right)
	{
		return (left._p7 > right._p7)
			|| (left._p7 == right._p7 && ((left._p6 > right._p6)
			|| (left._p6 == right._p6 && ((left._p5 > right._p5)
			|| (left._p5 == right._p5 && ((left._p4 > right._p4)
			|| (left._p4 == right._p4 && ((left._p3 > right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 > right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator <=(in UInt512 left, in UInt512 right)
	{
		return (left._p7 < right._p7)
			|| (left._p7 == right._p7 && ((left._p6 < right._p6)
			|| (left._p6 == right._p6 && ((left._p5 < right._p5)
			|| (left._p5 == right._p5 && ((left._p4 < right._p4)
			|| (left._p4 == right._p4 && ((left._p3 < right._p3)
			|| (left._p3 == right._p3 && ((left._p2 < right._p2)
			|| (left._p2 == right._p2 && ((left._p1 < right._p1)
			|| (left._p1 == right._p1 && (left._p0 <= right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static bool operator >=(in UInt512 left, in UInt512 right)
	{
		return (left._p7 > right._p7)
			|| (left._p7 == right._p7 && ((left._p6 > right._p6)
			|| (left._p6 == right._p6 && ((left._p5 > right._p5)
			|| (left._p5 == right._p5 && ((left._p4 > right._p4)
			|| (left._p4 == right._p4 && ((left._p3 > right._p3)
			|| (left._p3 == right._p3 && ((left._p2 > right._p2)
			|| (left._p2 == right._p2 && ((left._p1 > right._p1)
			|| (left._p1 == right._p1 && (left._p0 >= right._p0))))))))))))));
	}

	/// <inheritdoc/>
	public static UInt512 operator >>>(in UInt512 value, int shiftAmount)
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

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 128)
		{
			shiftAmount -= 64;

			if (shiftAmount == 0)
			{
				return new UInt512(0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
			}

			ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
			ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part6 = value._p7 >> shiftAmount;

			return new UInt512(0, part6, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 192)
		{
			shiftAmount -= 128;

			if (shiftAmount == 0)
			{
				return new UInt512(0, 0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
			}

			ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
			ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part5 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, part5, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 256)
		{
			shiftAmount -= 192;

			if (shiftAmount == 0)
			{
				return new UInt512(0, 0, 0, value._p7, value._p6, value._p5, value._p4, value._p3);
			}

			ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
			ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part4 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, 0, part4, part3, part2, part1, part0);
		}
		else if (shiftAmount < 320)
		{
			shiftAmount -= 256;

			if (shiftAmount == 0)
			{
				return new UInt512(0, 0, 0, 0, value._p7, value._p6, value._p5, value._p4);
			}

			ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
			ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part3 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, 0, 0, part3, part2, part1, part0);
		}
		else if (shiftAmount < 384)
		{
			shiftAmount -= 320;

			if (shiftAmount == 0)
			{
				return new UInt512(0, 0, 0, 0, 0, value._p7, value._p6, value._p5);
			}

			ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
			ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part2 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, 0, 0, 0, part2, part1, part0);
		}
		else if (shiftAmount < 448)
		{
			shiftAmount -= 384;

			if (shiftAmount == 0)
			{
				return new UInt512(0, 0, 0, 0, 0, 0, value._p7, value._p6);
			}

			ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
			ulong part1 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, 0, 0, 0, 0, part1, part0);
		}
		else // shiftAmount < 512
		{
			shiftAmount -= 448;

			ulong part0 = value._p7 >> shiftAmount;

			return new UInt512(0, 0, 0, 0, 0, 0, 0, part0);
		}
	}
}