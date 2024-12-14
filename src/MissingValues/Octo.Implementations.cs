using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;

namespace MissingValues
{
	public readonly partial struct Octo :
		IBigBinaryNumber<Octo>,
		IBinaryFloatingPointIeee754<Octo>,
		IBinaryFloatingPointInfo<Octo, UInt256>,
		IMinMaxValue<Octo>
	{
		static Octo IFloatingPointIeee754<Octo>.Epsilon => Epsilon;

		static Octo IFloatingPointIeee754<Octo>.NaN => NaN;

		static Octo IFloatingPointIeee754<Octo>.NegativeInfinity => NegativeInfinity;

		static Octo IFloatingPointIeee754<Octo>.NegativeZero => NegativeZero;

		static Octo IFloatingPointIeee754<Octo>.PositiveInfinity => PositiveInfinity;

		static Octo ISignedNumber<Octo>.NegativeOne => NegativeOne;

		static Octo IFloatingPointConstants<Octo>.E => E;

		static Octo IFloatingPointConstants<Octo>.Pi => Pi;

		static Octo IFloatingPointConstants<Octo>.Tau => Tau;

		static Octo INumberBase<Octo>.One => One;

		static int INumberBase<Octo>.Radix => 2;

		static Octo INumberBase<Octo>.Zero => Zero;

		static Octo IAdditiveIdentity<Octo, Octo>.AdditiveIdentity => Zero;

		static Octo IMultiplicativeIdentity<Octo, Octo>.MultiplicativeIdentity => One;

		static Octo IMinMaxValue<Octo>.MaxValue => MaxValue;

		static Octo IMinMaxValue<Octo>.MinValue => MinValue;

		static ReadOnlySpan<Octo> IFormattableFloatingPoint<Octo>.PowersOfTen => RoundPower10;

		static bool IBinaryFloatingPointInfo<Octo, UInt256>.ExplicitLeadingBit => false;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.NormalMantissaBits => BiasedExponentShift + 1;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.DenormalMantissaBits => BiasedExponentShift;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinimumDecimalExponent => -78984;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaximumDecimalExponent => 78913;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinBiasedExponent => MinBiasedExponent;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxBiasedExponent => MaxBiasedExponent;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxSignificandPrecision => 71;

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
		public static Octo Abs(Octo value) => Octo.UInt256BitsToOcto(Octo.OctoToUInt256Bits(value) & Octo.InvertedSignMask);

		/// <inheritdoc/>
		public static Octo Acos(Octo x) => Quad.Acos((Quad)x);

		/// <inheritdoc/>
		public static Octo Acosh(Octo x) => Quad.Acosh((Quad)x);

		/// <inheritdoc/>
		public static Octo AcosPi(Octo x) => Quad.Acos((Quad)x) / Pi;

		/// <inheritdoc/>
		public static Octo Asin(Octo x) => Quad.Asin((Quad)x);

		/// <inheritdoc/>
		public static Octo Asinh(Octo x) => Quad.Asinh((Quad)x);

		/// <inheritdoc/>
		public static Octo AsinPi(Octo x) => Quad.Asin((Quad)x) / Pi;

		/// <inheritdoc/>
		public static Octo Atan(Octo x) => Quad.Atan((Quad)x);

		/// <inheritdoc/>
		public static Octo Atan2(Octo y, Octo x) => Quad.Atan2((Quad)y, (Quad)x);

		/// <inheritdoc/>
		public static Octo Atan2Pi(Octo y, Octo x) => Quad.Atan2((Quad)y, (Quad)x) / Pi;

		/// <inheritdoc/>
		public static Octo Atanh(Octo x) => Quad.Atanh((Quad)x);

		/// <inheritdoc/>
		public static Octo AtanPi(Octo x) => Quad.Atan((Quad)x) / Pi;

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
		public static Octo Cbrt(Octo x) => Quad.Cbrt((Quad)x);

		/// <inheritdoc/>
		public static Octo Ceiling(Octo x)
		{
			var exponent = x.BiasedExponent;
			bool sign = Octo.IsNegative(x);
			Octo y;

			if (exponent >= 0x3FFFF + Octo.MantissaDigits - 1 || x == Octo.Zero)
			{
				return x;
			}
			// newBase = int(x) - x, where int(x) is an integer neighbor of x
			Octo toint = ToInt;
			if (sign)
			{
				y = x - toint + toint - x;
			}
			else
			{
				y = x + toint - toint - x;
			}
			// special case because of non-nearest rounding modes
			if (exponent <= 0x3FFFF - 1)
			{
				return sign ? Octo.NegativeZero : Octo.One;
			}
			if (y < Octo.Zero)
			{
				return x + y + Octo.One;
			}
			return x + y;
		}

		/// <inheritdoc/>
		public static Octo Clamp(Octo value, Octo min, Octo max)
		{
			if (min > max)
			{
				Thrower.MinMaxError(min, max);
			}

			if (value < min)
			{
				return min;
			}
			else if (value > max)
			{
				return max;
			}

			return value;
		}

		/// <inheritdoc/>
		public static Octo CopySign(Octo value, Octo sign)
		{
			// This method is required to work for all inputs,
			// including NaN, so we operate on the raw bits.
			UInt256 xbits = Octo.OctoToUInt256Bits(value);
			UInt256 ybits = Octo.OctoToUInt256Bits(sign);

			// Remove the sign from y, and remove everything but the sign from x
			xbits &= Octo.InvertedSignMask;
			ybits &= Octo.SignMask;

			// Simply OR them to get the correct sign
			return Octo.UInt256BitsToOcto(xbits | ybits);
		}

		/// <inheritdoc/>
		public static Octo Cos(Octo x) => Quad.Cos((Quad)x);

		/// <inheritdoc/>
		public static Octo Cosh(Octo x) => Quad.Cosh((Quad)x);

		/// <inheritdoc/>
		public static Octo CosPi(Octo x) => Quad.CosPi((Quad)x);

		/// <inheritdoc/>
		public static Octo CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Octo result;
			if (value is Octo v)
			{
				result = v;
			}
			else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToChecked<Octo>(value, out result))
			{
				Thrower.NotSupported<Octo, TOther>();
			}

			return result;
		}
		
		/// <inheritdoc/>
		public static Octo CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Octo result;
			if (value is Octo v)
			{
				result = v;
			}
			else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToSaturating<Octo>(value, out result))
			{
				Thrower.NotSupported<Octo, TOther>();
			}

			return result;
		}
		
		/// <inheritdoc/>
		public static Octo CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Octo result;
			if (value is Octo v)
			{
				result = v;
			}
			else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToTruncating<Octo>(value, out result))
			{
				Thrower.NotSupported<Octo, TOther>();
			}

			return result;
		}

		/// <inheritdoc/>
		public static Octo DegreesToRadians(Octo degrees)
		{
			// (degrees * Pi) / 180
			return (degrees * Pi) / new Octo(0x4000_6680_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		}

		/// <inheritdoc/>
		public static Octo Exp(Octo x) => Quad.Exp((Quad)x);

		/// <inheritdoc/>
		public static Octo ExpM1(Octo x) => Quad.Exp((Quad)x) - One;

		/// <inheritdoc/>
		public static Octo Exp10(Octo x) => Quad.Exp10((Quad)x);

		/// <inheritdoc/>
		public static Octo Exp10M1(Octo x) => Quad.Exp10((Quad)x) - One;

		/// <inheritdoc/>
		public static Octo Exp2(Octo x) => Quad.Exp2((Quad)x);

		/// <inheritdoc/>
		public static Octo Exp2M1(Octo x) => Quad.Exp2((Quad)x) - One;

		/// <inheritdoc/>
		public static Octo Floor(Octo x)
		{
			var exponent = x.BiasedExponent;
			bool sign = Octo.IsNegative(x);
			Octo y;

			if (exponent >= 0x3FFFF + Octo.MantissaDigits - 1 || x == Octo.Zero)
			{
				return x;
			}
			// y = int(x) - x, where int(x) is an integer neighbor of x
			Octo toint = ToInt;
			if (sign)
			{
				y = x - toint + toint - x;
			}
			else
			{
				y = x + toint - toint - x;
			}
			// special case because of non-nearest rounding modes
			if (exponent <= 0x3FFFF - 1)
			{
				return sign ? Octo.NegativeOne : Octo.Zero;
			}
			if (y > Octo.Zero)
			{
				return x + y - Octo.One;
			}
			return x + y;
		}

		/// <inheritdoc/>
		public static Octo FusedMultiplyAdd(Octo left, Octo right, Octo addend) => Octo.UInt256BitsToOcto(BitHelper.MulAddOctoBits(Octo.OctoToUInt256Bits(left), Octo.OctoToUInt256Bits(right), Octo.OctoToUInt256Bits(addend)));

		/// <inheritdoc/>
		public static Octo Hypot(Octo x, Octo y) => Quad.Hypot((Quad)x, (Quad)y);

		/// <inheritdoc/>
		public static Octo Ieee754Remainder(Octo left, Octo right)
		{
			UInt256 uiA = Octo.OctoToUInt256Bits(left);
			bool signA = Octo.IsNegative(left);
			int expA = (int)Octo.ExtractBiasedExponentFromBits(in uiA);
			UInt256 sigA = Octo.ExtractTrailingSignificandFromBits(in uiA);

			UInt256 uiB = Octo.OctoToUInt256Bits(right);
			int expB = (int)Octo.ExtractBiasedExponentFromBits(in uiB);
			UInt256 sigB = Octo.ExtractTrailingSignificandFromBits(in uiB);

			if (expA == 0x7FFFF)
			{
				if ((sigA != UInt128.Zero) || ((expB == 0x7FFFF) && (sigB != UInt128.Zero)))
				{
					return BitHelper.CreateOctoNaN(Octo.IsNegative(right), sigB);
				}
				return Octo.NaN;
			}
			if (expB == 0x7FFFF)
			{
				if (sigB != UInt128.Zero)
				{
					return BitHelper.CreateOctoNaN(Octo.IsNegative(right), sigB);
				}
				return left;
			}

			if (expB == 0)
			{
				if (sigB == UInt128.Zero)
				{
					return Quad.NaN;
				}
				(var exp, sigA) = BitHelper.NormalizeSubnormalF256Sig(sigA);
				expA = (int)exp;
			}

			sigA |= Octo.SignificandSignMask;
			sigB |= Octo.SignificandSignMask;

			UInt256 rem = sigA, altRem;
			int expDiff = expA - expB;
			uint q, recip32;

			if (expDiff < 1)
			{
				if (expDiff < -1)
				{
					return left;
				}
				if (expDiff != 0)
				{
					--expB;
					sigB += sigB;
					q = 0;
				}
				else
				{
					q = sigB <= rem ? 1U : 0U;
					if (q != 0)
					{
						rem -= sigB;
					}
				}
			}
			else
			{
				recip32 = BitHelper.ReciprocalApproximate((uint)(sigB.Part3 >> 13));
				expDiff -= 30;

				UInt256 term;
				ulong q64;
				while (true)
				{
					q64 = (rem.Part3 >> 15) * recip32;
					if (expDiff < 0)
					{
						break;
					}
					q = (uint)((q64 + 0x80000000) >> 32);
					rem <<= 29;
					term = sigB * q;
					rem -= term;
					if ((rem.Part3 & 0x8000_0000_0000_0000) != 0)
					{
						rem += sigB;
					}

					expDiff -= 29;
				}
				// ('expDiff' cannot be less than -29 here.)
				Debug.Assert(expDiff >= -29);

				q = (uint)(q64 >> 32) >> (~expDiff & 31);
				rem <<= expDiff + 30;
				term = sigB * q;
				rem -= term;
				if ((rem.Part3 & 0x8000_0000_0000_0000) != 0)
				{
					altRem = rem + sigB;
					goto selectRem;
				}
			}

			do
			{
				altRem = rem;
				++q;
				rem -= sigB;
			} while ((rem.Part3 & 0x8000_0000_0000_0000) == 0);
		selectRem:
			UInt256 meanRem = rem + altRem;
			if (((meanRem.Part3 & 0x8000_0000_0000_0000) != 0)
				|| ((meanRem == UInt256.Zero) && ((q & 1) != 0)))
			{
				rem = altRem;
			}
			bool signRem = signA;
			if ((rem.Part3 & 0x8000_0000_0000_0000) != 0)
			{
				signRem = !signRem;
				rem = -rem;
			}
			UInt256 resultBits = BitHelper.NormalizeRoundPackOcto(signRem, expB - 1, rem);
			return Octo.UInt256BitsToOcto(resultBits);
		}

		/// <inheritdoc/>
		public static int ILogB(Octo x)
		{
			const int NaN = -1 - 0x7FFF_FFFF;
			int exponent = (int)x.BiasedExponent;


			if (exponent == 0)
			{
				if (x == Octo.Zero)
				{
					return NaN;
				}
				// subnormal x
				Debug.Assert(Octo.IsSubnormal(x));
				return Octo.MinExponent - (BitHelper.TrailingZeroCount(x.TrailingSignificand) - Octo.BiasedExponentLength);
			}
			if (exponent == Octo.MaxBiasedExponent)
			{
				return Octo.IsNaN(x) ? NaN : int.MaxValue;
			}

			return exponent - Octo.ExponentBias;
		}

		static bool INumberBase<Octo>.IsCanonical(Octo value) => true;

		static bool INumberBase<Octo>.IsComplexNumber(Octo value) => false;

		/// <inheritdoc/>
		public static bool IsEvenInteger(Octo value) => IsInteger(value) && (Abs(value % Two) == Zero);

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
		public static bool IsInteger(Octo value) => IsFinite(value) && (value == Truncate(value));

		/// <inheritdoc/>
		public static bool IsNaN(Octo value) => StripSign(value) > PositiveInfinityBits;

		/// <inheritdoc/>
		public static bool IsNegative(Octo value) => Int256.IsNegative(Octo.OctoToInt256Bits(value));

		/// <inheritdoc/>
		public static bool IsNegativeInfinity(Octo value) => value == NegativeInfinity;

		/// <inheritdoc/>
		public static bool IsNormal(Octo value)
		{
			Int256 bits = Octo.OctoToInt256Bits(value);
			bits &= new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
			Int256 infBits = new Int256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			return (bits < infBits) && (bits != Int256.Zero) && ((bits & infBits) != Int256.Zero);
		}

		/// <inheritdoc/>
		public static bool IsOddInteger(Octo value) => IsInteger(value) && (Abs(value % Two) == One);

		/// <inheritdoc/>
		public static bool IsPositive(Octo value) => Int256.IsPositive(Octo.OctoToInt256Bits(value));

		/// <inheritdoc/>
		public static bool IsPositiveInfinity(Octo value) => value == PositiveInfinity;

		/// <inheritdoc/>
		public static bool IsPow2(Octo value)
		{
			UInt256 bits = Octo.OctoToUInt256Bits(value);

			if ((Int256)bits <= Int256.Zero)
			{
				// Zero and negative values cannot be powers of 2
				return false;
			}

			uint biasedExponent = ExtractBiasedExponentFromBits(in bits);
			UInt256 trailingSignificand = ExtractTrailingSignificandFromBits(in bits);

			if (biasedExponent == MinBiasedExponent)
			{
				// Subnormal values have 1 bit set when they're powers of 2
				return BitHelper.PopCount(in trailingSignificand) == 1;
			}
			else if (biasedExponent == MaxBiasedExponent)
			{
				// NaN and Infinite values cannot be powers of 2
				return false;
			}

			return trailingSignificand == MinTrailingSignificand;
		}

		/// <inheritdoc/>
		public static bool IsRealNumber(Octo value) => !IsNaN(value);

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
		public static Octo Lerp(Octo value1, Octo value2, Octo amount) => (value1 * (One - amount)) + (value2 * amount);

		/// <inheritdoc/>
		public static Octo Log(Octo x) => Quad.Log((Quad)x);

		/// <inheritdoc/>
		public static Octo Log(Octo x, Octo newBase)
		{
			if (Octo.IsNaN(x))
			{
				return x; // IEEE 754-2008: NaN payload must be preserved
			}

			if (Octo.IsNaN(newBase))
			{
				return newBase; // IEEE 754-2008: NaN payload must be preserved
			}

			if (newBase == 1)
			{
				return Octo.NaN;
			}

			if ((x != 1) && ((newBase == 0) || Octo.IsPositiveInfinity(newBase)))
			{
				return Octo.NaN;
			}

			return Log(x) / Log(newBase);
		}

		/// <inheritdoc/>
		public static Octo LogP1(Octo x) => Quad.Log((Quad)x + Quad.One);

		/// <inheritdoc/>
		public static Octo Log10(Octo x) => Quad.Log10((Quad)x);

		/// <inheritdoc/>
		public static Octo Log10P1(Octo x) => Quad.Log10((Quad)x + Quad.One);

		/// <inheritdoc/>
		public static Octo Log2(Octo x) => Quad.Log2((Quad)x);

		/// <inheritdoc/>
		public static Octo Log2P1(Octo x) => Quad.Log2((Quad)x + Quad.One);

		/// <inheritdoc/>
		public static Octo Max(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `maximum` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the greater of the inputs. It
			// treats +0 as greater than -0 as per the specification.

			if (x != y)
			{
				if (!Octo.IsNaN(x))
				{
					return y < x ? x : y;
				}

				return x;
			}

			return Octo.IsNegative(y) ? x : y;
		}

		/// <inheritdoc/>
		public static Octo MaxNumber(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `maximumNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the larger of the inputs. It
			// treats +0 as larger than -0 as per the specification.

			if (x != y)
			{
				if (!IsNaN(y))
				{
					return y < x ? x : y;
				}

				return x;
			}

			return IsNegative(y) ? x : y;
		}

		/// <inheritdoc/>
		public static Octo MaxMagnitude(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `maximumMagnitude` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the input with a greater magnitude.
			// It treats +0 as greater than -0 as per the specification.

			Octo ax = Abs(x);
			Octo ay = Abs(y);

			if ((ax > ay) || Octo.IsNaN(ax))
			{
				return x;
			}

			if (ax == ay)
			{
				return Octo.IsNegative(x) ? y : x;
			}

			return y;
		}

		/// <inheritdoc/>
		public static Octo MaxMagnitudeNumber(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `maximumMagnitudeNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the input with a larger magnitude.
			// It treats +0 as larger than -0 as per the specification.

			Octo ax = Abs(x);
			Octo ay = Abs(y);

			if ((ax > ay) || IsNaN(ay))
			{
				return x;
			}

			if (ax == ay)
			{
				return IsNegative(x) ? y : x;
			}

			return y;
		}

		/// <inheritdoc/>
		public static Octo Min(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `minimum` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the lesser of the inputs. It
			// treats +0 as greater than -0 as per the specification.

			if (x != y)
			{
				if (!Octo.IsNaN(x))
				{
					return x < y ? x : y;
				}

				return x;
			}

			return Octo.IsNegative(x) ? x : y;
		}

		/// <inheritdoc/>
		public static Octo MinNumber(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `minimumNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the larger of the inputs. It
			// treats +0 as larger than -0 as per the specification.

			if (x != y)
			{
				if (!IsNaN(y))
				{
					return x < y ? x : y;
				}

				return x;
			}

			return IsNegative(x) ? x : y;
		}

		/// <inheritdoc/>
		public static Octo MinMagnitude(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `minimumMagnitude` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the input with a lesser magnitude.
			// It treats +0 as greater than -0 as per the specification.

			Octo ax = Abs(x);
			Octo ay = Abs(y);

			if ((ax < ay) || Octo.IsNaN(ax))
			{
				return x;
			}

			if (ax == ay)
			{
				return Octo.IsNegative(x) ? x : y;
			}

			return y;
		}

		/// <inheritdoc/>
		public static Octo MinMagnitudeNumber(Octo x, Octo y)
		{
			// This matches the IEEE 754:2019 `minimumMagnitudeNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the input with a larger magnitude.
			// It treats +0 as larger than -0 as per the specification.

			Octo ax = Abs(x);
			Octo ay = Abs(y);

			if ((ax < ay) || IsNaN(ay))
			{
				return x;
			}

			if (ax == ay)
			{
				return IsNegative(x) ? x : y;
			}

			return y;
		}

#if NET9_0_OR_GREATER
		static Octo INumberBase<Octo>.MultiplyAddEstimate(Octo left, Octo right, Octo addend) => (left * right) + addend;
#endif

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
		public static Octo Pow(Octo x, Octo y) => Quad.Pow((Quad)x, (Quad)y);

		/// <inheritdoc/>
		public static Octo RadiansToDegrees(Octo radians)
		{
			// (degrees * 180) / Pi
			return (radians * new Octo(0x4000_6680_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) / Pi;
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
			var bits = Octo.ExtractFromBits(Octo.OctoToUInt256Bits(x));

			// we save the original sign and exponent for later
			bool sign = bits.sign;
			uint exp = bits.exponent;

			// Expresses D as M × 2e where 1 ≤ M < 2
			// we also get the absolute value while we are at it.
			Octo normalizedValue = new Octo(true, Octo.ExponentBias, bits.matissa);

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

			bits = Octo.ExtractFromBits(Octo.OctoToUInt256Bits(x0));

			bits.exponent -= (exp - Octo.ExponentBias);

			var output = new Octo(sign, bits.exponent, bits.matissa);
			return output;
		}

		/// <inheritdoc/>
		public static Octo ReciprocalSqrtEstimate(Octo x) => ReciprocalEstimate(Sqrt(x));

		/// <inheritdoc/>
		public static Octo RootN(Octo x, int n)
		{
			Octo result;

			if (n > 0)
			{
				if (n == 2)
				{
					result = (x != Octo.Zero) ? Sqrt(x) : Octo.Zero;
				}
				else if (n == 3)
				{
					result = Cbrt(x);
				}
				else
				{
					result = PositiveN(x, n);
				}
			}
			else if (n < 0)
			{
				result = NegativeN(x, n);
			}
			else
			{
				Debug.Assert(n == 0);
				result = NaN;
			}
			return result;

			static Octo PositiveN(Octo x, int n)
			{
				Octo result;

				if (IsFinite(x))
				{
					if (x != Zero)
					{
						if ((x > Zero) || int.IsOddInteger(n))
						{
							result = Pow(Abs(x), ReciprocalEstimate(n));
							result = CopySign(result, x);
						}
						else
						{
							result = NaN;
						}
					}
					else if (int.IsEvenInteger(n))
					{
						result = Zero;
					}
					else
					{
						result = CopySign(Zero, x);
					}
				}
				else if (IsNaN(x))
				{
					result = NaN;
				}
				else if (x > Octo.Zero)
				{
					Debug.Assert(IsPositiveInfinity(x));
					result = PositiveInfinity;
				}
				else
				{
					Debug.Assert(IsNegativeInfinity(x));
					result = int.IsOddInteger(n) ? NegativeInfinity : NaN;
				}

				return result;
			}
			static Octo NegativeN(Octo x, int n)
			{
				Octo result;

				if (IsFinite(x))
				{
					if (x != Zero)
					{
						if ((x > Zero) || int.IsOddInteger(n))
						{
							result = Pow(Abs(x), ReciprocalEstimate(n));
							result = CopySign(result, x);
						}
						else
						{
							result = NaN;
						}
					}
					else if (int.IsEvenInteger(n))
					{
						result = PositiveInfinity;
					}
					else
					{
						result = CopySign(PositiveInfinity, x);
					}
				}
				else if (IsNaN(x))
				{
					result = NaN;
				}
				else if (x > Zero)
				{
					Debug.Assert(IsPositiveInfinity(x));
					result = Zero;
				}
				else
				{
					Debug.Assert(IsNegativeInfinity(x));
					result = int.IsOddInteger(n) ? NegativeZero : NaN;
				}

				return result;
			}
		}

		/// <inheritdoc/>
		public static Octo Round(Octo x)
		{
			// Based on .Net Math.Round(): https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Math.cs#L1273 

			var exponent = x.BiasedExponent;

			if (exponent >= 0x3FFFF + MantissaDigits - 1)
			{
				return x;
			}

			if (exponent < 0x3FFFF - 1)
			{
				return Zero * x;
			}

			Octo temp = CopySign(ToInt, x);
			return CopySign((x + temp) - temp, x);
		}
		/// <inheritdoc/>
		public static Octo Round(Octo x, int digits) => Round(x, digits, MidpointRounding.ToEven);
		/// <inheritdoc/>
		public static Octo Round(Octo x, MidpointRounding mode) => Round(x, 0, mode);
		/// <inheritdoc/>
		public static Octo Round(Octo x, int digits, MidpointRounding mode)
		{
			if ((uint)digits > 71)
			{
				Thrower.OutOfRange(nameof(digits));
			}

			if (Abs(x) < new Octo(0x400E_E21C_81F7_DD43, 0xA749_5791_2ABD_28DF, 0xC639_B9EC_DEC6_9C20, 0x0000_0000_0000_0000) /*1E72*/)
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
			if (n > Octo.MaxExponent)
			{
				Octo maxExp = new Octo(0x7FFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

				x *= maxExp;
				n -= Octo.MaxExponent;
				if (n > Octo.MaxExponent)
				{
					x *= maxExp;
					n -= Octo.MaxExponent;

					if (n > Octo.MaxExponent)
					{
						n = Octo.MaxExponent;
					}
				}
			}
			else if (n < Octo.MinExponent)
			{
				Octo minExp = new Octo(0x0000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
				Octo b237 = new Octo(0x400E_C000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

				Octo scaleb = minExp * b237;
				x *= scaleb;
				n += -Octo.MinExponent - Octo.MantissaDigits;

				if (n < Octo.MinExponent)
				{
					x *= scaleb;
					n += -Octo.MinExponent - Octo.MantissaDigits;

					if (n < Octo.MinExponent)
					{
						n = Octo.MinExponent;
					}
				}

				Octo result = x * new Octo((ulong)(0x3FFFF + n) << 44, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
				if (Octo.IsInfinity(result))
				{
					return Octo.Zero;
				}
				return result;
			}

			return x * new Octo((ulong)(0x3FFFF + n) << 44, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
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
		public static Octo Sin(Octo x) => Quad.Sin((Quad)x);

		/// <inheritdoc/>
		public static (Octo Sin, Octo Cos) SinCos(Octo x) => Quad.SinCos((Quad)x);

		/// <inheritdoc/>
		public static (Octo SinPi, Octo CosPi) SinCosPi(Octo x) => Quad.SinCosPi((Quad)x);

		/// <inheritdoc/>
		public static Octo Sinh(Octo x) => Quad.Sinh((Quad)x);

		/// <inheritdoc/>
		public static Octo SinPi(Octo x) => Quad.SinPi((Quad)x);

		/// <inheritdoc/>
		public static Octo Sqrt(Octo x)
		{
			UInt256 bits = Octo.OctoToUInt256Bits(x);
			bool signA = Octo.IsNegative(x);
			uint exp = Octo.ExtractBiasedExponentFromBits(in bits);
			UInt256 sig = Octo.ExtractTrailingSignificandFromBits(in bits);

			// Is x NaN?
			if (exp == 0x7FFFF)
			{
				if (sig != UInt256.Zero)
				{
					return Quad.NaN;
				}
				if (!signA)
				{
					return x;
				}
				return Quad.NaN;
			}
			if (signA)
			{
				if (((UInt256)exp | sig) == UInt256.Zero)
				{
					return x;
				}
				return Quad.NaN;
			}

			if (exp == 0)
			{
				if (sig == UInt256.Zero)
				{
					return x;
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF256Sig(sig);
			}

			uint expZ = ((exp - 0x3FFFF) >> 1) + 0x3FFFE;
			exp &= 1;
			sig |= new UInt256(0x0000_1000_0000_0000, 0, 0, 0);
			uint sig32 = (uint)(sig.Part3 >> 13);
			uint recipSqrt32 = BitHelper.SqrtReciprocalApproximate(exp, sig32);
			uint sig32Z = (uint)(((ulong)sig32 * recipSqrt32) >> 32);
			UInt256 rem;
			if (exp != 0)
			{
				sig32Z >>= 1;
				rem = sig << 16;
			}
			else
			{
				rem = sig << 17;
			}
			Span<uint> qs = [0, 0, sig32Z];
			rem -= new UInt256((ulong)sig32Z * sig32Z, 0x0, 0x0, 0x0);

			uint q = (uint)(((uint)(rem.Part3 >> 2) * (ulong)recipSqrt32) >> 32);
			ulong x64 = (ulong)sig32Z << 32;
			ulong sig64Z = x64 + ((ulong)q << 3);
			UInt256 y = rem << 29;

			UInt256 term;
			do
			{
				term = (UInt256)BitHelper.Mul64ByShifted32To128(x64 + sig64Z, q) << 128;
				rem = y - term;
				if (((rem.Part3) & 0x8000_0000_0000_0000) == 0)
				{
					break;
				}
				--q;
				sig64Z -= 1 << 3;
			} while (true);
			qs[1] = q;

			q = (uint)(((rem.Part3 >> 2) * recipSqrt32) >> 32);
			y = rem << 29;
			sig64Z <<= 1;

			do
			{
				term = (UInt256)sig64Z << (32 + 128);
				term += (ulong)q << 6;
				term *= q;
				rem = y - term;
				if ((rem.Part3 & 0x8000_0000_0000_0000) == 0)
				{
					break;
				}
				--q;
			} while (true);
			qs[0] = q;

			q = (uint)((((rem.Part3 >> 2) * recipSqrt32) >> 32) + 2);
			UInt128 sigZExtra = (UInt128)q << (59 + 64);
			term = (UInt256)qs[1] << (53 + 128);
			UInt256 sigZ = (new UInt256((ulong)qs[2] << 18, (((ulong)qs[0] << 24) + (q >> 5)), 0, 0) + term) >> 4;

			if ((q & 0xF) <= 2)
			{
				q &= ~3U;
				sigZExtra = (UInt128)q << 123;
				y = sigZ << 6;
				y |= sigZExtra >> 122;
				term = y - q;
				y = new UInt256(BitHelper.Mul64ByShifted32To128(term.Part2, q), UInt128.Zero);
				term = new UInt256(BitHelper.Mul64ByShifted32To128(term.Part3, q), UInt128.Zero);
				term += y.Part3;
				rem <<= 27;
				term -= rem;

				if ((term.Part3 & 0x8000_0000_0000_0000) != 0)
				{
					sigZExtra |= 1;
				}
				else
				{
					if ((term | y.Part0) != UInt256.Zero)
					{
						if (sigZExtra != 0)
						{
							--sigZExtra;
						}
						else
						{
							sigZ -= UInt256.One;
							sigZExtra = UInt128.MaxValue;
						}
					}
				}
			}

			return Octo.UInt256BitsToOcto(BitHelper.RoundPackToOcto(false, (int)expZ, sigZ, sigZExtra));
		}

		/// <inheritdoc/>
		public static Octo Tan(Octo x) => Quad.Tan((Quad)x);

		/// <inheritdoc/>
		public static Octo Tanh(Octo x) => Quad.Tanh((Quad)x);

		/// <inheritdoc/>
		public static Octo TanPi(Octo x) => Quad.TanPi((Quad)x);

		/// <inheritdoc/>
		public static Octo Truncate(Octo x)
		{
			var exponent = x.BiasedExponent;
			bool sign = Octo.IsNegative(x);

			Octo y;

			if (exponent >= 0x3FFFF + Octo.MantissaDigits - 1)
			{
				return x;
			}
			if (exponent <= 0x3FFFF - 1)
			{
				return sign ? Octo.NegativeZero : Octo.Zero;
			}
			// y = int(|x|) - |x|, where int(|x|) is an integer neighbor of |x|
			if (sign)
			{
				x = -x;
			}
			Octo toint = ToInt;
			y = x + toint - toint - x;
			if (y > Octo.Zero)
			{
				y--;
			}
			x += y;
			return sign ? -x : x;
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
				BigInteger actual => (Octo)actual,
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
					BigInteger => (TOther)(object)(BigInteger)value,
					nint => (TOther)(object)(nint)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Octo>.TryConvertToSaturating<TOther>(Octo value, out TOther result) => TryConvertTo(value, out result);

		static bool INumberBase<Octo>.TryConvertToTruncating<TOther>(Octo value, out TOther result) => TryConvertTo(value, out result);

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
				BigInteger => (TOther)(object)(BigInteger)value,
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
				|| AreZero(in this, in other)
				|| (IsNaN(this) && IsNaN(other));
		}

		int IFloatingPoint<Octo>.GetExponentByteCount() => sizeof(int);

		int IFloatingPoint<Octo>.GetExponentShortestBitLength()
		{
			int exponent = Exponent;

			if (exponent >= 0)
			{
				return (sizeof(int) * 8) - int.LeadingZeroCount(exponent);
			}
			else
			{
				return (sizeof(int) * 8) + 1 - int.LeadingZeroCount((~exponent));
			}
		}

		int IFloatingPoint<Octo>.GetSignificandBitLength() => 237;

		int IFloatingPoint<Octo>.GetSignificandByteCount() => UInt256.Size;

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
			if (destination.Length >= sizeof(int))
			{
				int exponent = Exponent;

				if (BitConverter.IsLittleEndian)
				{
					exponent = BinaryPrimitives.ReverseEndianness(exponent);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

				bytesWritten = sizeof(int);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		bool IFloatingPoint<Octo>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= sizeof(int))
			{
				int exponent = Exponent;

				if (!BitConverter.IsLittleEndian)
				{
					exponent = BinaryPrimitives.ReverseEndianness(exponent);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

				bytesWritten = sizeof(int);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		bool IFloatingPoint<Octo>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= Unsafe.SizeOf<UInt256>())
			{
				UInt256 significand = Significand;

				if (BitConverter.IsLittleEndian)
				{
					significand = BitHelper.ReverseEndianness(in significand);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

				bytesWritten = Unsafe.SizeOf<UInt256>();
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		bool IFloatingPoint<Octo>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= Unsafe.SizeOf<UInt256>())
			{
				UInt256 significand = Significand;

				if (!BitConverter.IsLittleEndian)
				{
					significand = BitHelper.ReverseEndianness(in significand);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

				bytesWritten = Unsafe.SizeOf<UInt256>();
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

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
			return UInt256BitsToOcto(OctoToUInt256Bits(value) ^ new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
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
			const int Iterations = 3;
			Span<uint> qs = stackalloc uint[Iterations];
			UInt256 term;
			for (int ix = Iterations; ;)
			{
				ulong q64 = (ulong)(uint)(rem.Part3 >> 15) * recip32;
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
				rem <<= 153;
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

			UInt128 sigZExtra = new UInt128((ulong)q << 56, 0);
			term = new UInt256(0, 0, 0, qs[1]) << 108;
			UInt256 sigZ = new UInt256((ulong)qs[2] << 15, ((ulong)qs[0] << 25) + (q >> 4), 0, 0) + term;
			return Octo.UInt256BitsToOcto(BitHelper.RoundPackToOcto(signZ, expZ, sigZ, sigZExtra));
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
			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

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

			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

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

			var lvalue = OctoToUInt256Bits(left);
			var rvalue = OctoToUInt256Bits(right);

			return (lvalue == rvalue) || ((lvalue < rvalue) ^ leftIsNegative);
		}

		/// <inheritdoc/>
		public static bool operator >=(in Octo left, in Octo right) => right <= left;
	}
}
