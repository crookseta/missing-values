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

		static ReadOnlySpan<Octo> IFormattableFloatingPoint<Octo>.PowersOfTen => throw new NotImplementedException();

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
		public static Octo Exp(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Exp10(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Exp2(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo FusedMultiplyAdd(Octo left, Octo right, Octo addend)
		{
			throw new NotImplementedException();
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

		/// <inheritdoc/>
		public static bool IsZero(Octo value)
		{
			return value == Zero;
		}

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
		public static Octo Log10(Octo x)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Log2(Octo value)
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
		public static Octo RootN(Octo x, int n)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Round(Octo x, int digits, MidpointRounding mode)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo ScaleB(Octo x, int n)
		{
			throw new NotImplementedException();
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

		static bool INumberBase<Octo>.TryConvertFromChecked<TOther>(TOther value, out Octo result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.TryConvertFromSaturating<TOther>(TOther value, out Octo result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.TryConvertFromTruncating<TOther>(TOther value, out Octo result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.TryConvertToChecked<TOther>(Octo value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.TryConvertToSaturating<TOther>(Octo value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Octo>.TryConvertToTruncating<TOther>(Octo value, out TOther result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public int CompareTo(object? obj)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public int CompareTo(Octo other)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public bool Equals(Octo other)
		{
			throw new NotImplementedException();
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
		// TODO: Implement formatting support.
		/// <inheritdoc/>
		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			throw new NotImplementedException();
			return NumberFormatter.FloatToString<Octo, UInt256>(in this, format, formatProvider);
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			throw new NotImplementedException();
			return NumberFormatter.TryFormatFloat<Octo, UInt256, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			throw new NotImplementedException();
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
		public static Octo operator +(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator +(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator -(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator -(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator ~(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator ++(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator --(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator *(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator /(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator %(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator &(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator |(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo operator ^(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator ==(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator !=(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator <(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator >(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator <=(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool operator >=(Octo left, Octo right)
		{
			throw new NotImplementedException();
		}
	}
}
