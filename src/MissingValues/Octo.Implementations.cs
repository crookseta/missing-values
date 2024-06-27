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
		public static Octo Epsilon => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo NaN => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo NegativeInfinity => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo NegativeZero => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo PositiveInfinity => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo NegativeOne => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo E => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo Pi => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo Tau => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo One => throw new NotImplementedException();

		static int INumberBase<Octo>.Radix => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo Zero => throw new NotImplementedException();

		static Octo IAdditiveIdentity<Octo, Octo>.AdditiveIdentity => throw new NotImplementedException();

		static Octo IMultiplicativeIdentity<Octo, Octo>.MultiplicativeIdentity => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo MaxValue => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo MinValue => throw new NotImplementedException();

		static ReadOnlySpan<Octo> IFormattableFloatingPoint<Octo>.PowersOfTen => throw new NotImplementedException();

		static bool IBinaryFloatingPointInfo<Octo, UInt256>.ExplicitLeadingBit => false;

		static int IBinaryFloatingPointInfo<Octo, UInt256>.NormalMantissaBits => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.DenormalMantissaBits => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinimumDecimalExponent => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaximumDecimalExponent => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MinBiasedExponent => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxBiasedExponent => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.MaxSignificandPrecision => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.ExponentBits => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.ExponentBias => throw new NotImplementedException();

		static int IBinaryFloatingPointInfo<Octo, UInt256>.OverflowDecimalExponent => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.DenormalMantissaMask => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.NormalMantissaMask => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.TrailingSignificandMask => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.PositiveZeroBits => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.PositiveInfinityBits => throw new NotImplementedException();

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.NegativeInfinityBits => throw new NotImplementedException();

		/// <inheritdoc/>
		public static Octo Abs(Octo value)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo BitIncrement(Octo x)
		{
			throw new NotImplementedException();
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

		/// <inheritdoc/>
		public static bool IsCanonical(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsComplexNumber(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsEvenInteger(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsFinite(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsImaginaryNumber(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsInfinity(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsInteger(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsNaN(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsNegative(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsNegativeInfinity(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsNormal(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsOddInteger(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsPositive(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsPositiveInfinity(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsPow2(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsRealNumber(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsSubnormal(Octo value)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool IsZero(Octo value)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(string s, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static Octo Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			throw new NotImplementedException();
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
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Octo result)
		{
			throw new NotImplementedException();
		}

		static Octo IBinaryFloatingPointInfo<Octo, UInt256>.BitsToFloat(UInt256 bits)
		{
			throw new NotImplementedException();
		}

		static UInt256 IBinaryFloatingPointInfo<Octo, UInt256>.FloatToBits(Octo value)
		{
			throw new NotImplementedException();
		}

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

		/// <inheritdoc/>
		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			//return NumberFormatter.FloatToString<Octo, UInt256, FloatingDecimal>(in this, format, formatProvider);
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			throw new NotImplementedException();
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
