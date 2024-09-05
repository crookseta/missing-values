using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace MissingValues
{
	public readonly partial struct Octo :
		IBigBinaryNumber<Octo>,
		IBinaryFloatingPointIeee754<Octo>,
		IBinaryFloatingPointInfo<Octo, UInt256>,
		IMinMaxValue<Octo>
	{ // TODO: Implement interface methods.
		/// <inheritdoc/>
		public static Octo Epsilon => UInt256BitsToOcto(EpsilonBits);

		/// <inheritdoc/>
		public static Octo NaN => UInt256BitsToOcto(PositiveQNaNBits);

		/// <inheritdoc/>
		public static Octo NegativeInfinity => UInt256BitsToOcto(NegativeInfinityBits);

		/// <inheritdoc/>
		public static Octo NegativeZero => UInt256BitsToOcto(NegativeZeroBits);

		/// <inheritdoc/>
		public static Octo PositiveInfinity => UInt256BitsToOcto(PositiveInfinityBits);

		/// <inheritdoc/>
		public static Octo NegativeOne => UInt256BitsToOcto(NegativeOneBits);

		/// <inheritdoc/>
		public static Octo E => UInt256BitsToOcto(EBits);

		/// <inheritdoc/>
		public static Octo Pi => UInt256BitsToOcto(PiBits);

		/// <inheritdoc/>
		public static Octo Tau => UInt256BitsToOcto(TauBits);

		/// <inheritdoc/>
		public static Octo One => UInt256BitsToOcto(PositiveOneBits);

		static int INumberBase<Octo>.Radix => 2;

		/// <inheritdoc/>
		public static Octo Zero => UInt256BitsToOcto(PositiveZeroBits);

		static Octo IAdditiveIdentity<Octo, Octo>.AdditiveIdentity => UInt256BitsToOcto(PositiveZeroBits);

		static Octo IMultiplicativeIdentity<Octo, Octo>.MultiplicativeIdentity => UInt256BitsToOcto(PositiveOneBits);

		/// <inheritdoc/>
		public static Octo MaxValue => UInt256BitsToOcto(MaxValueBits);

		/// <inheritdoc/>
		public static Octo MinValue => UInt256BitsToOcto(MinValueBits);

		static ReadOnlySpan<Octo> IFormattableFloatingPoint<Octo>.PowersOfTen => RoundPower10;

		static bool IBinaryFloatingPointInfo<Octo, UInt256>.ExplicitLeadingBit => false;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.NormalMantissaBits => BiasedExponentShift + 1;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.DenormalMantissaBits => BiasedExponentShift;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinimumDecimalExponent => -78984;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaximumDecimalExponent => 78913;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinBiasedExponent => MinBiasedExponent;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxBiasedExponent => MaxBiasedExponent;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxSignificandPrecision => 70;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.ExponentBits => 19;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.ExponentBias => ExponentBias;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.OverflowDecimalExponent => (ExponentBias + (2 * 237) / 3);

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.DenormalMantissaMask => TrailingSignificandMask;

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.NormalMantissaMask => new UInt256(0x0000_1FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.TrailingSignificandMask => TrailingSignificandMask;

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.PositiveZeroBits => PositiveZeroBits;

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.PositiveInfinityBits => PositiveInfinityBits;

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.NegativeInfinityBits => NegativeInfinityBits;

		/// <inheritdoc/>
		public static Octo Abs(Octo value)
		{
			return Octo.UInt256BitsToOcto(Octo.OctoToUInt256Bits(value) & Octo.InvertedSignMask);
		}

		/// <inheritdoc/>
		public static Octo Acos(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Acosh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo AcosPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Asin(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Asinh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo AsinPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Atan(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Atan2(Octo y, Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Atan2Pi(Octo y, Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Atanh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo AtanPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo BitDecrement(Octo x)
		{
			UInt256 bits = Octo.OctoToUInt256Bits(x);

			if ((bits & Octo.PositiveInfinityBits) >= Octo.PositiveInfinityBits)
			{
				// NaN returns NaN
				// -Infinity returns -Infinity
				// +Infinity returns MaxValue
				return (bits == Octo.PositiveInfinityBits) ? Octo.MaxValue : x;
			}

			if (bits == Octo.PositiveZeroBits)
			{
				// +0.0 returns -Epsilon
				return -Octo.Epsilon;
			}

			// Negative values need to be incremented
			// Positive values need to be decremented

			bits += unchecked((UInt256)(((Int256)bits < Int256.Zero) ? Int256.One : Int256.NegativeOne));
			return Octo.UInt256BitsToOcto(bits);
		}

		/// <inheritdoc/>
		public static Octo BitIncrement(Octo x)
		{
			UInt256 bits = Octo.OctoToUInt256Bits(x);

			if ((bits & Octo.PositiveInfinityBits) >= Octo.PositiveInfinityBits)
			{
				// NaN returns NaN
				// -Infinity returns MinValue
				// +Infinity returns +Infinity
				return (bits == Octo.NegativeInfinityBits) ? Octo.MinValue : x;
			}

			if (bits == Octo.NegativeZeroBits)
			{
				// -0.0 returns Epsilon
				return Octo.Epsilon;
			}

			// Negative values need to be decremented
			// Positive values need to be incremented

			bits += unchecked((UInt256)(((Int256)bits < Int256.Zero) ? Int256.NegativeOne : Int256.One));
			return Octo.UInt256BitsToOcto(bits);
		}

		/// <inheritdoc/>
		public static Octo Cbrt(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Ceiling(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Clamp(Octo value, Octo min, Octo max)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo CopySign(Octo value, Octo sign)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Cos(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Cosh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo CosPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo DegreesToRadians(Octo degrees)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Exp(Octo x)
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo ExpM1(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Exp10(Octo x)
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo Exp10M1(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Exp2(Octo x)
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo Exp2M1(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Floor(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo FusedMultiplyAdd(Octo left, Octo right, Octo addend)
		{
			UInt256 result = BitHelper.MulAddOctoBits(Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), Octo.OctoToUInt256Bits(addend));

			return Octo.UInt256BitsToOcto(result);
		}

		/// <inheritdoc/>
		public static Octo Hypot(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Ieee754Remainder(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static int ILogB(Octo x)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.IsCanonical(Octo value)
		{
			return true;
		}

		static bool INumberBase<Octo>.IsComplexNumber(Octo value)
		{
			return false;
		}

		/// <inheritdoc/>
		public static bool IsEvenInteger(Octo value)
		{
			return IsInteger(value) && (Abs(value % Two) == Zero);
		}

		/// <inheritdoc/>
		public static bool IsFinite(Octo value)
		{
			Int256 bits = Octo.OctoToInt256Bits(value);
			return (bits & new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) 
				< new Int256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		}

		static bool INumberBase<Octo>.IsImaginaryNumber(Octo value) => false;

		/// <inheritdoc/>
		public static bool IsInfinity(Octo value)
		{
			Int256 bits = Octo.OctoToInt256Bits(value);
			return (bits & new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF))
				== new Int256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		}

		/// <inheritdoc/>
		public static bool IsInteger(Octo value)
		{
			return IsFinite(value) && (value == Truncate(value));
		}

		/// <inheritdoc/>
		public static bool IsNaN(Octo value)
		{
			return StripSign(value) > PositiveInfinityBits;
		}

		/// <inheritdoc/>
		public static bool IsNegative(Octo value)
		{
			return Int256.IsNegative(Octo.OctoToInt256Bits(value));
		}

		/// <inheritdoc/>
		public static bool IsNegativeInfinity(Octo value)
		{
			return value == NegativeInfinity;
		}

		/// <inheritdoc/>
		public static bool IsNormal(Octo value)
		{
			Int256 bits = Octo.OctoToInt256Bits(value);
			bits &= new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			Int256 infBits = new Int256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			return (bits < infBits) && (bits != Int256.Zero) && ((bits & infBits) != Int256.Zero);
		}

		/// <inheritdoc/>
		public static bool IsOddInteger(Octo value)
		{
			return IsInteger(value) && (Abs(value % Two) == One);
		}

		/// <inheritdoc/>
		public static bool IsPositive(Octo value)
		{
			return Int256.IsPositive(Octo.OctoToInt256Bits(value));
		}

		/// <inheritdoc/>
		public static bool IsPositiveInfinity(Octo value)
		{
			return value == PositiveInfinity;
		}

		/// <inheritdoc/>
		public static bool IsPow2(Octo value)
		{
			UInt256 bits = Octo.OctoToUInt256Bits(value);

			if ((Int256)bits <= Int256.Zero)
			{
				// Zero and negative values cannot be powers of 2
				return false;
			}

			uint biasedExponent = ExtractBiasedExponentFromBits(bits); ;
			UInt256 trailingSignificand = ExtractTrailingSignificandFromBits(bits);

			if (biasedExponent == MinBiasedExponent)
			{
				// Subnormal values have 1 bit set when they're powers of 2
				return UInt256.PopCount(trailingSignificand) == UInt256.One;
			}
			else if (biasedExponent == MaxBiasedExponent)
			{
				// NaN and Infinite values cannot be powers of 2
				return false;
			}

			return trailingSignificand == MinTrailingSignificand;
		}

		/// <inheritdoc/>
		public static bool IsRealNumber(Octo value)
		{
			return !IsNaN(value);
		}

		/// <inheritdoc/>
		public static bool IsSubnormal(Octo value)
		{
			Int256 bits = Octo.OctoToInt256Bits(value);
			bits &= new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			Int256 infBits = new Int256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			return (bits < infBits) && (bits != Int256.Zero) && ((bits & infBits) == Int256.Zero);
		}

		static bool INumberBase<Octo>.IsZero(Octo value) => value == Zero;

		/// <inheritdoc/>
		public static Octo Lerp(Octo value1, Octo value2, Octo amount)
		{
			return (value1 * (One - amount)) + (value2 * amount);
		}

		/// <inheritdoc/>
		public static Octo Log(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Log(Octo x, Octo newBase)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo LogP1(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Log10(Octo x)
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo Log10P1(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Log2(Octo value)
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public static Octo Log2P1(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Max(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MaxNumber(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MaxMagnitude(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MaxMagnitudeNumber(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Min(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MinNumber(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MinMagnitude(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo MinMagnitudeNumber(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(s.ToString());
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(s);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(s.ToString());
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Parse(string s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(s);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(utf8Text, style, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(utf8Text.ToString());
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			if (!TryParse(utf8Text, NumberStyles.Float, provider, out Octo result))
			{
				Thrower.ParsingError<Octo>(utf8Text.ToString());
			}
			return result;
		}

		/// <inheritdoc/>
		public static Octo Pow(Octo x, Octo y)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo RadiansToDegrees(Octo radians)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo ReciprocalEstimate(Octo x)
		{
			/*
			 * source:
			 * Gaurav Agrawal, Ankit Khandelwal. 
			 * A Newton Raphson Divider Based on 
			 * Improved Reciprocal Approximation Algorithm
			 * http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.134.725&rep=rep1&type=pd
			 */

			if (x == Octo.Zero)
			{
				return Octo.PositiveInfinity;
			}
			if (Octo.IsInfinity(x))
			{
				return Octo.IsNegative(x) ? Octo.NegativeZero : Octo.Zero;
			}
			if (Octo.IsNaN(x))
			{
				return x;
			}

			// Uses Newton Raphton Series to find 1/y
			Octo x0;

			// we save the original sign and exponent for later
			bool sign = IsNegative(x);
			uint exp = x.BiasedExponent;

			// Expresses D as M × 2e where 1 ≤ M < 2
			// we also get the absolute value while we are at it.
			Octo normalizedValue = new Octo(true, Octo.ExponentBias, x.TrailingSignificand);

			x0 = Octo.One;
			Octo two = Octo.Two;

			// 15 iterations should be enough.
			for (int i = 0; i < 15; i++)
			{
				// X1 = X(2 - DX)
				// x1 = f * (two - (normalizedValue * f))
				Octo x1 = x0 * FusedMultiplyAdd(normalizedValue, x0, two);
				// Since we need: two - (normalizedValue * f)
				// to make use of FusedMultiplyAdd, we can rewrite it to (-normalizedValue * f) + two
				// which requires normalizedValue to be negative...

				if (Octo.Abs(x1 - x) < Octo.Epsilon)
				{
					x0 = x1;
					break;
				}

				x0 = x1;
			}

			var output = new Octo(sign, x0.BiasedExponent - (exp - Octo.ExponentBias), x0.TrailingSignificand);
			return output;
		}

		/// <inheritdoc/>
		public static Octo ReciprocalSqrtEstimate(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo RootN(Octo x, int n)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Round(Octo x)
		{
			throw new NotImplementedException();
		}
		/// <inheritdoc/>
		public static Octo Round(Octo x, int digits)
		{
			throw new NotImplementedException();
		}
		/// <inheritdoc/>
		public static Octo Round(Octo x, MidpointRounding mode)
		{
			throw new NotImplementedException();
		}
		/// <inheritdoc/>
		public static Octo Round(Octo x, int digits, MidpointRounding mode)
		{
			if ((uint)digits > 70)
			{
				Thrower.OutOfRange(nameof(digits));
			}

			if (Abs(x) < new Octo() /*1E70*/)
			{
				Octo power10 = RoundPower10[digits];

				x *= power10;

				x = mode switch
				{
					MidpointRounding.ToEven => Round(x),
					MidpointRounding.AwayFromZero => Truncate(x + CopySign(BitDecrement(Octo.HalfOne), x)),
					MidpointRounding.ToZero => Truncate(x),
					MidpointRounding.ToNegativeInfinity => Floor(x),
					MidpointRounding.ToPositiveInfinity => Ceiling(x),
					_ => throw new ArgumentException("Invalid enum value.", nameof(mode)),
				};

				x /= power10;
			}

			return x;
		}

		/// <inheritdoc/>
		public static Octo ScaleB(Octo x, int n)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static int Sign(Octo value)
		{
			if (value < Octo.Zero)
			{
				return -1;
			}
			else if (value > Octo.Zero)
			{
				return 1;
			}
			else if (value == Octo.Zero)
			{
				return 0;
			}

			Thrower.InvalidNaN(value);
			return default;
		}

		/// <inheritdoc/>
		public static Octo Sin(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static (Octo Sin, Octo Cos) SinCos(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static (Octo SinPi, Octo CosPi) SinCosPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Sinh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo SinPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Sqrt(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Tan(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Tanh(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo TanPi(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Truncate(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Float, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Float, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			return NumberParser.TryParseFloat<Octo, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Float, provider, out result);
		}

		static Octo IBinaryFloatingPointInfo<Octo, UInt256>.BitsToFloat(UInt256 bits) => UInt256BitsToOcto(bits);

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.FloatToBits(Octo value) => OctoToUInt256Bits(value);

		static bool INumberBase<Octo>.TryConvertFromChecked<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

		static bool INumberBase<Octo>.TryConvertFromSaturating<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

		static bool INumberBase<Octo>.TryConvertFromTruncating<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

		private static bool TryConvertFrom<TOther>(TOther value, out Octo result)
		{
			bool converted = true;

			result = value switch
			{
				Half actual => (Octo)actual,
				float actual => (Octo)actual,
				double actual => (Octo)actual,
				Quad actual => (Octo)actual,
				Octo actual => actual,
				decimal actual => (Octo)actual,
				byte actual => (Octo)actual,
				ushort actual => (Octo)actual,
				uint actual => (Octo)actual,
				ulong actual => (Octo)actual,
				UInt128 actual => (Octo)actual,
				UInt256 actual => (Octo)actual,
				UInt512 actual => (Octo)actual,
				nuint actual => (Octo)actual,
				sbyte actual => (Octo)actual,
				short actual => (Octo)actual,
				int actual => (Octo)actual,
				long actual => (Octo)actual,
				Int128 actual => (Octo)actual,
				Int256 actual => (Octo)actual,
				Int512 actual => (Octo)actual,
				_ => BitHelper.DefaultConvert<Octo>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Octo>.TryConvertToChecked<TOther>(Octo value, out TOther result)
		{
			bool converted = true;
			result = default;
			checked
			{
				result = result switch
				{
					Half => (TOther)(object)(Half)value,
					float => (TOther)(object)(float)value,
					double => (TOther)(object)(double)value,
					Quad => (TOther)(object)(Quad)value,
					Octo => (TOther)(object)value,
					decimal => (TOther)(object)(decimal)value,
					byte => (TOther)(object)(byte)value,
					ushort => (TOther)(object)(ushort)value,
					uint => (TOther)(object)(uint)value,
					ulong => (TOther)(object)(ulong)value,
					UInt128 => (TOther)(object)(UInt128)value,
					UInt256 => (TOther)(object)(UInt256)value,
					UInt512 => (TOther)(object)(UInt512)value,
					nuint => (TOther)(object)(nuint)value,
					sbyte => (TOther)(object)(sbyte)value,
					short => (TOther)(object)(short)value,
					int => (TOther)(object)(int)value,
					long => (TOther)(object)(long)value,
					Int128 => (TOther)(object)(Int128)value,
					Int256 => (TOther)(object)(Int256)value,
					Int512 => (TOther)(object)(Int512)value,
					nint => (TOther)(object)(nint)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Octo>.TryConvertToSaturating<TOther>(Octo value, out TOther result)
		{
			return TryConvertTo(value, out result);
		}

		static bool INumberBase<Octo>.TryConvertToTruncating<TOther>(Octo value, out TOther result)
		{
			return TryConvertTo(value, out result);
		}

		private static bool TryConvertTo<TOther>(Octo value, out TOther result)
		{
			bool converted = true;
			result = default;

			result = result switch
			{
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				Quad => (TOther)(object)(Quad)value,
				Octo => (TOther)(object)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)((value >= byte.MaxValue) ? byte.MaxValue : (value <= Octo.Zero) ? byte.MinValue : (byte)value),
				ushort => (TOther)(object)((value >= ushort.MaxValue) ? ushort.MaxValue : (value <= Octo.Zero) ? ushort.MinValue : (ushort)value),
				uint => (TOther)(object)((value >= uint.MaxValue) ? uint.MaxValue : (value <= Octo.Zero) ? uint.MinValue : (uint)value),
				ulong => (TOther)(object)((value >= ulong.MaxValue) ? ulong.MaxValue : (value <= Octo.Zero) ? ulong.MinValue : (ulong)value),
				UInt128 => (TOther)(object)((value >= UInt128.MaxValue) ? UInt128.MaxValue : (value <= Octo.Zero) ? UInt128.MinValue : (UInt128)value),
				UInt256 => (TOther)(object)((value >= new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt256.MaxValue 
				: (value <= Quad.Zero) ? UInt256.MinValue : (UInt256)value),
				UInt512 => (TOther)(object)((value >= new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue 
				: (value <= Quad.Zero) ? UInt512.MinValue : (UInt512)value),
				nuint => (TOther)(object)((value >= nuint.MaxValue) ? nuint.MaxValue : (value <= nuint.MinValue) ? nuint.MinValue : (nuint)value),
				sbyte => (TOther)(object)((value >= sbyte.MaxValue) ? sbyte.MaxValue : (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
				short => (TOther)(object)((value >= short.MaxValue) ? short.MaxValue : (value <= short.MinValue) ? short.MinValue : (short)value),
				int => (TOther)(object)((value >= int.MaxValue) ? int.MaxValue : (value <= int.MinValue) ? int.MinValue : (int)value),
				long => (TOther)(object)((value >= long.MaxValue) ? long.MaxValue : (value <= long.MinValue) ? long.MinValue : (long)value),
				Int128 => (TOther)(object)((value >= Int128.MaxValue) ? Int128.MaxValue : (value <= Int128.MinValue) ? Int128.MinValue : (Int128)value),
				Int256 => (TOther)(object)((value >= new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MaxValue 
				: (value <= new Octo(0xC00F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MinValue : (Int256)value),
				Int512 => (TOther)(object)((value >= new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MaxValue 
				: (value <= new Octo(0xC01F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MinValue : (Int512)value),
				nint => (TOther)(object)((value >= nint.MaxValue) ? nint.MaxValue : (value <= nint.MinValue) ? nint.MinValue : (nint)value),
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		/// <inheritdoc/>
		public int CompareTo(object? obj)
		{
			if (obj is not Octo o)
			{
				if (obj is null)
				{
					return 1;
				}
				else
				{
					Thrower.MustBeType<Octo>();
					return 0;
				}
			}
			return CompareTo(o);
		}

		/// <inheritdoc/>
		public int CompareTo(Octo other)
		{
			if (this < other)
			{
				return -1;
			}

			if (this > other)
			{
				return 1;
			}

			if (this == other)
			{
				return 0;
			}

			if (IsNaN(this))
			{
				return IsNaN(other) ? 0 : -1;
			}

			return 1;
		}

		/// <inheritdoc/>
		public bool Equals(Octo other)
		{
			return (_bits3 == other._bits3 && _bits2 == other._bits2 && _bits1 == other._bits1 && _bits0 == other._bits0)
				|| AreZero(this, other)
				|| (IsNaN(this) && IsNaN(other));
		}

		int IFloatingPoint<Octo>.GetExponentByteCount()
		{
			throw new NotImplementedException();
		}

		int IFloatingPoint<Octo>.GetExponentShortestBitLength()
		{
			throw new NotImplementedException();
		}

		int IFloatingPoint<Octo>.GetSignificandBitLength()
		{
			throw new NotImplementedException();
		}

		int IFloatingPoint<Octo>.GetSignificandByteCount()
		{
			throw new NotImplementedException();
		}
		
		/// <inheritdoc/>
		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FloatToString<Octo, UInt256>(in this, format, formatProvider);
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatFloat<Octo, UInt256, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatFloat<Octo, UInt256, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
		}

		bool IFloatingPoint<Octo>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		bool IFloatingPoint<Octo>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		bool IFloatingPoint<Octo>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		bool IFloatingPoint<Octo>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator +(in Octo value)
		{
			return value;
		}

		/// <inheritdoc/>
		public static Octo operator +(in Octo left, in Octo right)
		{
			bool signA;
			bool signB;

			signA = Octo.IsNegative(left);

			signB = Octo.IsNegative(right);

			if (signA == signB)
			{
				return Octo.UInt256BitsToOcto(BitHelper.AddOctoBits(
					 Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), signA));
			}
			else
			{
				return Octo.UInt256BitsToOcto(BitHelper.SubOctoBits(
					Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), signA));
			}
		}

		/// <inheritdoc/>
		public static Octo operator -(in Octo value)
		{
			// Invert the sign bit
			return value ^ new Octo(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
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
				return Octo.UInt256BitsToOcto(BitHelper.SubOctoBits(
					Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), signA));
			}
			else
			{
				return Octo.UInt256BitsToOcto(BitHelper.AddOctoBits(
					Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), signA));
			}
		}

		/// <inheritdoc/>
		public static Octo operator ~(in Octo value) => new(~value._bits3, ~value._bits2, ~value._bits1, ~value._bits0);

		/// <inheritdoc/>
		public static Octo operator ++(in Octo value)
		{
			return value + One;
		}

		/// <inheritdoc/>
		public static Octo operator --(in Octo value)
		{
			return value - One;
		}

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
			sig512 = Calculator.BigMul(sigA, sigB);
			UInt128 sigZExtra = Convert.ToUInt64(sig512 != UInt512.Zero);
			sigZ = sig512.Upper + sigA;
			if (0x0000_2000_0000_0000 <= sigZ.Part3)
			{
				++expZ;
				sigZ = BitHelper.ShortShiftRightJamExtra(sigZ, sigZExtra, 1, out sigZExtra);
			}

			return Octo.UInt256BitsToOcto(BitHelper.RoundPackToOcto(signZ, (int)expZ, sigZ, sigZExtra));
		}

		/// <inheritdoc/>
		public static Octo operator /(in Octo left, in Octo right)
		{
			bool signA = Octo.IsNegative(left);
			var expA = left.BiasedExponent;
			var sigA = left.Significand;
			bool signB = Octo.IsNegative(right);
			var expB = right.BiasedExponent;
			var sigB = right.Significand;
			bool signZ = signA ^ signB;

			const int MaxExp = 0x7FFFF;
			if (expA == MaxExp)
			{
				if (sigA != UInt256.Zero)
				{
					return Octo.NaN;
				}
				if (expB == MaxExp)
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

				return signZ ? Octo.NegativeZero : Octo.Zero;
			}

			if (expB == 0)
			{
				if (sigB == UInt256.Zero)
				{
					if ((expA | sigA) == UInt256.Zero)
					{
						return Octo.NaN;
					}
					return signZ ? Octo.NegativeInfinity : Octo.PositiveInfinity;
				}
				(expB, sigB) = BitHelper.NormalizeSubnormalF256Sig(sigB);
			}
			if (expA == 0)
			{
				if (sigA == UInt256.Zero)
				{
					return signZ ? Octo.NegativeZero : Octo.Zero;
				}

				(expA, sigA) = BitHelper.NormalizeSubnormalF256Sig(sigA);
			}

			int expZ = ((int)(expA - expB + (Octo.ExponentBias - 1)));
			sigA |= new UInt256(0x0000_1000_0000_0000, 0, 0, 0);
			sigB |= new UInt256(0x0000_1000_0000_0000, 0, 0, 0);
			UInt256 rem = sigA;
			if (sigA < sigB)
			{
				--expZ;
				rem = sigA + sigA;
			}

			uint recip32 = BitHelper.ReciprocalApproximate((uint)(sigB >> 205));

			uint q;
			const int Iterations = 5;
			Span<uint> qs = stackalloc uint[Iterations];
			UInt256 term;
			for (int ix = Iterations; ;)
			{
				ulong q64 = (ulong)(uint)(rem >> 207) * recip32;
				q = (uint)((q64 + 0x8000_0000) >> 32);
				if (--ix < 0)
				{
					break;
				}
				rem <<= 29;
				term = sigB * q;
				rem -= term;
				if ((rem.Part3 & 0x8000_0000_0000_0000) != 0)
				{
					--q;
					rem += sigB;
				}
				qs[ix] = q;
			}

			if (((q + 1) & 7) < 2)
			{
				rem <<= 29;
				term = sigB * q;
				rem -= term;
				if ((rem.Part3 & 0x8000_0000_0000_0000) != 0)
				{
					--q;
					rem += sigB;
				}
				else if (sigB <= rem)
				{
					++q;
					rem -= sigB;
				}
				if (rem != UInt128.Zero)
				{
					q |= 1;
				}
			}

			UInt128 sigZExtra = ((UInt128)q << 120);
			term = new UInt256(0, 0, 0, qs[3]) << 108;
			UInt256 sigZ = new UInt256((UInt128)qs[4] << 83, ((UInt128)qs[1] << 89) + (q >> 4)) + term;
			return Octo.UInt256BitsToOcto(BitHelper.RoundPackToOcto(signZ, expZ, sigZ, sigZExtra));
		}

		/// <inheritdoc/>
		public static Octo operator %(in Octo left, in Octo right)
		{
			return (Abs(left) - (Abs(right) * (Floor(Abs(left) / Abs(right))))) * Sign(left);
		}

		/// <inheritdoc/>
		public static Octo operator &(in Octo left, in Octo right) => new(left._bits3 & right._bits3, left._bits2 & right._bits2, left._bits1 & right._bits1, left._bits0 & right._bits0);

		/// <inheritdoc/>
		public static Octo operator |(in Octo left, in Octo right) => new(left._bits3 | right._bits3, left._bits2 | right._bits2, left._bits1 | right._bits1, left._bits0 | right._bits0);

		/// <inheritdoc/>
		public static Octo operator ^(in Octo left, in Octo right) => new(left._bits3 ^ right._bits3, left._bits2 ^ right._bits2, left._bits1 ^ right._bits1, left._bits0 ^ right._bits0);

		/// <inheritdoc/>
		public static bool operator ==(in Octo left, in Octo right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				// IEEE defines that NaN is not equal to anything, including itself.
				return false;
			}

			// IEEE defines that positive and negative zero are equivalent.
			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

			return (lvalue == rvalue) || AreZero(left, right);
		}

		/// <inheritdoc/>
		public static bool operator !=(in Octo left, in Octo right)
		{
			return !(left == right);
		}

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
				return leftIsNegative && !AreZero(left, right);
			}

			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

			return (lvalue != rvalue) && ((lvalue < rvalue) ^ leftIsNegative);
		}

		/// <inheritdoc/>
		public static bool operator >(in Octo left, in Octo right)
		{
			return right < left;
		}

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
				return leftIsNegative || AreZero(left, right);
			}

			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

			return (lvalue == rvalue) || ((lvalue < rvalue) ^ leftIsNegative);
		}

		/// <inheritdoc/>
		public static bool operator >=(in Octo left, in Octo right)
		{
			return right <= left;
		}
	}
}
