using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Octo
{
	/// <inheritdoc/>
	public static Octo operator +(in Octo value) => value;

	/// <inheritdoc/>
	public static Octo operator +(in Octo left, in Octo right)
	{
		bool signA;
		bool signB;

		signA = Octo.IsNegative(left);

		signB = Octo.IsNegative(right);

		if (signA == signB)
		{
			return BinaryOperations.UInt256BitsToOcto(BitHelper.AddOctoBits(
				BinaryOperations.OctoToUInt256Bits(left), BinaryOperations.OctoToUInt256Bits(right), signA));
		}
		else
		{
			return BinaryOperations.UInt256BitsToOcto(BitHelper.SubOctoBits(
				BinaryOperations.OctoToUInt256Bits(left), BinaryOperations.OctoToUInt256Bits(right), signA));
		}
	}

	/// <inheritdoc/>
	public static Octo operator -(in Octo value)
	{
		// Invert the sign bit
		return BinaryOperations.UInt256BitsToOcto(BinaryOperations.OctoToUInt256Bits(value) ^ new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	/// <inheritdoc/>
	public static Octo operator -(in Octo left, in Octo right)
	{
		bool signA;
		bool signB;

		signA = Octo.IsNegative(left);

		signB = Octo.IsNegative(right);

		if (signA == signB)
		{
			return BinaryOperations.UInt256BitsToOcto(BitHelper.SubOctoBits(
				BinaryOperations.OctoToUInt256Bits(left), BinaryOperations.OctoToUInt256Bits(right), signA));
		}
		else
		{
			return BinaryOperations.UInt256BitsToOcto(BitHelper.AddOctoBits(
				BinaryOperations.OctoToUInt256Bits(left), BinaryOperations.OctoToUInt256Bits(right), signA));
		}
	}

	/// <inheritdoc/>
	static Octo IBigBinaryNumber<Octo>.operator ~(in Octo value)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v = Unsafe.BitCast<Octo, Vector256<ulong>>(value);
			var result = ~v;
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else
		{
			return new Octo(~value._bits3, ~value._bits2, ~value._bits1, ~value._bits0);
		}
	}

	/// <inheritdoc/>
	public static Octo operator ++(in Octo value) => value + One;

	/// <inheritdoc/>
	public static Octo operator --(in Octo value) => value - One;

	/// <inheritdoc/>
	public static Octo operator *(in Octo left, in Octo right)
	{
		bool signA, signB, signZ;
		uint expA, expB, expZ;
		UInt256 sigA, sigB, sigZ;
		UInt512 sig512;

		signA = Octo.IsNegative(left);
		expA = left.BiasedExponent;
		sigA = left.TrailingSignificand;

		signB = Octo.IsNegative(right);
		expB = right.BiasedExponent;
		sigB = right.TrailingSignificand;
		signZ = signA ^ signB;

		const int MaxExp = 0x7FFFF;

		if (expA == MaxExp)
		{
			if ((sigA != UInt256.Zero) || ((expB == MaxExp) && (sigB != UInt256.Zero)))
			{
				return Octo.NaN;
			}

			bool magBits = (sigB | expB) != UInt256.Zero;
			if (!magBits)
			{
				return Octo.NaN;
			}

			return signZ ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}
		if (expB == MaxExp)
		{
			if (sigB != UInt256.Zero)
			{
				return Octo.NaN;
			}
			bool magBits = (sigA | expA) != UInt256.Zero;
			if (!magBits)
			{
				return Octo.NaN;
			}

			return signZ ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}

		if (expA == 0)
		{
			if (sigA == UInt256.Zero)
			{
				return signZ ? Octo.NegativeZero : Octo.Zero;
			}
			(expA, sigA) = BitHelper.NormalizeSubnormalF256Sig(sigA);
		}
		if (expB == 0)
		{
			if (sigB == UInt256.Zero)
			{
				return signZ ? Octo.NegativeZero : Octo.Zero;
			}
			(expB, sigB) = BitHelper.NormalizeSubnormalF256Sig(sigB);
		}

		expZ = expA + expB - (Octo.ExponentBias + 1);
		sigA |= new UInt256(0x0000_1000_0000_0000, 0, 0, 0);
		sigB <<= 20;
		sig512 = MathQ.BigMul(sigA, sigB);
		UInt128 sigZExtra = Convert.ToUInt64(sig512 != UInt512.Zero);
		sigZ = sig512.Upper + sigA;
		if (0x0000_2000_0000_0000 <= sigZ.Part3)
		{
			++expZ;
			sigZ = BitHelper.ShortShiftRightJamExtra(sigZ, sigZExtra, 1, out sigZExtra);
		}

		return BinaryOperations.UInt256BitsToOcto(BitHelper.RoundPackToOcto(signZ, (int)expZ, sigZ, sigZExtra));
	}

	/// <inheritdoc/>
	public static Octo operator /(in Octo left, in Octo right)
	{
		// Special case handling
		if (IsNaN(left))
		{
			return left;
		}

		if (IsNaN(right))
		{
			return right;
		}

		if (right == Zero || IsInfinity(left) || IsInfinity(right))
		{
			if (right == Zero)
			{
				if (left == Zero)
				{
					return NaN;
				}
				return IsNegative(left) != IsNegative(right) ? NegativeInfinity : PositiveInfinity;
			}

			if (IsInfinity(right))
			{
				if (IsInfinity(left))
				{
					return NaN;
				}
				return IsNegative(left) != IsNegative(right) ? NegativeZero : Zero;
			}

			if (IsInfinity(left))
			{
				return IsNegative(left) != IsNegative(right) ? NegativeInfinity : PositiveInfinity;
			}
		}
		
		// Calculate Exponent
		long exp = (long)(int)left.BiasedExponent - (int)right.BiasedExponent + ExponentBias;
		
		// Normalize inputs
		UInt256 leftMantissa = left.Significand;
		UInt256 rightMantissa = right.Significand;
		int sl = BitHelper.LeadingZeroCount(in leftMantissa);
		int sr = BitHelper.LeadingZeroCount(in rightMantissa);
		exp = exp - sl + sr;
		
		// Perform division
		UInt512 dividend = (UInt512)(leftMantissa << sl) << BiasedExponentShift;
		UInt256 divisor = rightMantissa << sr;
		var (quotient, remainder) = UInt512.DivRem(dividend, divisor);
		
		// Build extended precision result
		UInt512 am = quotient;

		if (am != UInt512.Zero)
		{
			int nlz = BitHelper.LeadingZeroCount(in am);
			int shift = nlz - (UInt512.Size * 8 - (BiasedExponentShift + 1));
			if (shift < 0)
			{
				am >>= -shift;
			}
			else
			{
				am <<= shift;
			}

			exp -= shift;
		}
		
		// Rounding
		bool roundUp = false;
		if (remainder != UInt512.Zero)
		{
			UInt512 half = UInt512.One << (BiasedExponentShift - 1);
			if (remainder > half)
			{
				roundUp = true;
			}
			else if (remainder == half)
			{
				// Round to nearest even
				if ((am & UInt512.One) != UInt512.Zero)
				{
					roundUp = true;
				}
			}
		}

		if (roundUp)
		{
			am++;
		}
		
		// Final normalization after rounding
		if ((am >> (BiasedExponentShift + 1)) != UInt512.Zero)
		{
			am >>= 1;
			exp++;
		}
		
		// Handle overflow
		if (exp >= (1 << BiasedExponentLength) - 1)
		{
			return IsNegative(left) != IsNegative(right) ? NegativeInfinity : PositiveInfinity;
		}

		if (exp <= 0)
		{
			return IsNegative(left) != IsNegative(right) ? NegativeZero : Zero;
		}

		return am == UInt512.Zero ? Zero : new Octo(IsNegative(left) != IsNegative(right), (uint)exp, (UInt256)am);
	}

	/// <inheritdoc/>
	public static Octo operator %(in Octo left, in Octo right)
	{
		return (Abs(left) - (Abs(right) * (Floor(Abs(left) / Abs(right))))) * Sign(left);
	}

	/// <inheritdoc/>
	static Octo IBigBinaryNumber<Octo>.operator &(in Octo left, in Octo right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = v1 & v2;
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = Avx2.And(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else
		{
			return new Octo(left._bits3 & right._bits3, left._bits2 & right._bits2, left._bits1 & right._bits1, left._bits0 & right._bits0);
		}
	}

	/// <inheritdoc/>
	static Octo IBigBinaryNumber<Octo>.operator |(in Octo left, in Octo right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = v1 | v2;
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = Avx2.Or(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else
		{
			return new Octo(left._bits3 | right._bits3, left._bits2 | right._bits2, left._bits1 | right._bits1, left._bits0 | right._bits0);
		}
	}

	/// <inheritdoc/>
	static Octo IBigBinaryNumber<Octo>.operator ^(in Octo left, in Octo right)
	{
		if (Vector256.IsHardwareAccelerated)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = v1 ^ v2;
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else if (Avx2.IsSupported)
		{
			var v1 = Unsafe.BitCast<Octo, Vector256<ulong>>(left);
			var v2 = Unsafe.BitCast<Octo, Vector256<ulong>>(right);
			var result = Avx2.Xor(v1, v2);
			return Unsafe.BitCast<Vector256<ulong>, Octo>(result);
		}
		else
		{
			return new Octo(left._bits3 ^ right._bits3, left._bits2 ^ right._bits2, left._bits1 ^ right._bits1, left._bits0 ^ right._bits0);
		}
	}

	/// <inheritdoc/>
	public static bool operator ==(in Octo left, in Octo right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is not equal to anything, including itself.
			return false;
		}

		// IEEE defines that positive and negative zero are equivalent.
		var lvalue = BinaryOperations.OctoToUInt256Bits(left);
		var rvalue = BinaryOperations.OctoToUInt256Bits(right);

		return (lvalue == rvalue) || AreZero(in left, in right);
	}

	/// <inheritdoc/>
	public static bool operator !=(in Octo left, in Octo right) => !(left == right);

	/// <inheritdoc/>
	public static bool operator <(in Octo left, in Octo right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool leftIsNegative = IsNegative(left);

		if (leftIsNegative != IsNegative(right))
		{
			// When the signs of left and right differ, we know that left is less than right if it is
			// the negative value. The exception to this is if both values are zero, in which case IEEE
			// says they should be equal, even if the signs differ.
			return leftIsNegative && !AreZero(in left, in right);
		}

		var lvalue = BinaryOperations.OctoToUInt256Bits(left);
		var rvalue = BinaryOperations.OctoToUInt256Bits(right);

		return (lvalue != rvalue) && ((lvalue < rvalue) ^ leftIsNegative);
	}

	/// <inheritdoc/>
	public static bool operator >(in Octo left, in Octo right) => right < left;

	/// <inheritdoc/>
	public static bool operator <=(in Octo left, in Octo right)
	{
		if (IsNaN(left) || IsNaN(right))
		{
			// IEEE defines that NaN is unordered with respect to everything, including itself.
			return false;
		}

		bool leftIsNegative = IsNegative(left);

		if (leftIsNegative != IsNegative(right))
		{
			// When the signs of left and right differ, we know that left is less than right if it is
			// the negative value. The exception to this is if both values are zero, in which case IEEE
			// says they should be equal, even if the signs differ.
			return leftIsNegative || AreZero(in left, in right);
		}

		var lvalue = BinaryOperations.OctoToUInt256Bits(left);
		var rvalue = BinaryOperations.OctoToUInt256Bits(right);

		return (lvalue == rvalue) || ((lvalue < rvalue) ^ leftIsNegative);
	}

	/// <inheritdoc/>
	public static bool operator >=(in Octo left, in Octo right) => right <= left;
}