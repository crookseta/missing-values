using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public readonly partial struct LongDouble :
		IBinaryFloatingPointIeee754<LongDouble>,
		IFormattableBinaryFloatingPoint<LongDouble>,
		IMinMaxValue<LongDouble>
	{
		public static LongDouble Epsilon => new LongDouble(0x0000, 0x0000_0000_0000_0001);

		public static LongDouble NaN => new LongDouble(0x7FFF, 0xC000_0000_0000_0000);

		public static LongDouble NegativeInfinity => new LongDouble(0xFFFF, 0x8000_0000_0000_0000);

		public static LongDouble NegativeZero => new LongDouble(0x8000, 0x0000_0000_0000_0000);

		public static LongDouble PositiveInfinity => new LongDouble(0x7FFF, 0x8000_0000_0000_0000);

		public static LongDouble NegativeOne => new LongDouble(0xBFFF, 0x8000_0000_0000_0000);

		public static LongDouble E => new LongDouble(0x4000, 0xADF8_5458_A2BB_4A9B);

		public static LongDouble Pi => new LongDouble(0x4000, 0xC90F_DAA2_2168_C235);

		public static LongDouble Tau => new LongDouble(0x4001, 0xC90F_DAA2_2168_C235);

		public static LongDouble One => new LongDouble(0x3FFF, 0x8000_0000_0000_0000);

		public static int Radix => 2;

		public static LongDouble Zero => default;

		static LongDouble IAdditiveIdentity<LongDouble, LongDouble>.AdditiveIdentity => Zero;

		static LongDouble IMultiplicativeIdentity<LongDouble, LongDouble>.MultiplicativeIdentity => One;

		public static LongDouble MaxValue => new LongDouble(0x7FFE, 0xFFFF_FFFF_FFFF_FFFF);

		public static LongDouble MinValue => new LongDouble(0xFFFE, 0xFFFF_FFFF_FFFF_FFFF);

		static bool IFormattableBinaryFloatingPoint<LongDouble>.ExplicitLeadingBit => true;

		static int IFormattableBinaryFloatingPoint<LongDouble>.NormalMantissaBits => 64;

		static int IFormattableBinaryFloatingPoint<LongDouble>.DenormalMantissaBits => 64;

		static int IFormattableBinaryFloatingPoint<LongDouble>.MinimumDecimalExponent => -4951;

		static int IFormattableBinaryFloatingPoint<LongDouble>.MaximumDecimalExponent => 4932;

		static int IFormattableBinaryFloatingPoint<LongDouble>.MinBiasedExponent => 0x0000;

		static int IFormattableBinaryFloatingPoint<LongDouble>.MaxBiasedExponent => 0x7FFF;

		static int IFormattableBinaryFloatingPoint<LongDouble>.MaxSignificandPrecision => 18;

		static int IFormattableBinaryFloatingPoint<LongDouble>.ExponentBits => 15;

		static int IFormattableBinaryFloatingPoint<LongDouble>.ExponentBias => 16383;

		static int IFormattableBinaryFloatingPoint<LongDouble>.OverflowDecimalExponent => (16383 + (2 * 113) / 3);

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.DenormalMantissaMask => 0xFFFF_FFFF_FFFF_FFFF;

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.NormalMantissaMask => 0xFFFF_FFFF_FFFF_FFFF;

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.TrailingSignificandMask => 0xFFFF_FFFF_FFFF_FFFF;

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.PositiveZeroBits => default;

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.PositiveInfinityBits => new UInt128(0x7FFF, 0x0);

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.NegativeInfinityBits => new UInt128(0xFFFF, 0x0);

		static ReadOnlySpan<LongDouble> IFormattableFloatingPoint<LongDouble>.PowersOfTen => ReadOnlySpan<LongDouble>.Empty;

		public static LongDouble Abs(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Acos(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Acosh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble AcosPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Asin(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Asinh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble AsinPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Atan(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Atan2(LongDouble y, LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Atan2Pi(LongDouble y, LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Atanh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble AtanPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble BitDecrement(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble BitIncrement(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Cbrt(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Cos(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Cosh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble CosPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Exp(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Exp10(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Exp2(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble FusedMultiplyAdd(LongDouble left, LongDouble right, LongDouble addend)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Hypot(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Ieee754Remainder(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static int ILogB(LongDouble x)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.IsCanonical(LongDouble value)
		{
			return true;
		}

		static bool INumberBase<LongDouble>.IsComplexNumber(LongDouble value)
		{
			return false;
		}

		public static bool IsEvenInteger(LongDouble value)
		{
			return IsInteger(value) && (Abs(value % Two) == Zero);
		}

		public static bool IsFinite(LongDouble value)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.IsImaginaryNumber(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsInfinity(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsInteger(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNaN(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNegative(LongDouble value)
		{
			return value.Sign;
		}

		public static bool IsNegativeInfinity(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNormal(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsOddInteger(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsPositive(LongDouble value)
		{
			return !value.Sign;
		}

		public static bool IsPositiveInfinity(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsPow2(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsRealNumber(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static bool IsSubnormal(LongDouble value)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.IsZero(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Lerp(LongDouble value1, LongDouble value2, LongDouble amount)
		{
			return (value1 * (One - amount)) + (value2 * amount);
		}

		public static LongDouble Log(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Log(LongDouble x, LongDouble newBase)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Log10(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Log2(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble MaxMagnitude(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble MaxMagnitudeNumber(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble MinMagnitude(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble MinMagnitudeNumber(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out LongDouble result))
			{
				Thrower.ParsingError<LongDouble>(s.ToString());
			}
			return result;
		}

		public static LongDouble Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out LongDouble result))
			{
				Thrower.ParsingError<LongDouble>(s);
			}
			return result;
		}

		public static LongDouble Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out LongDouble result))
			{
				Thrower.ParsingError<LongDouble>(s.ToString());
			}
			return result;
		}

		public static LongDouble Parse(string s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out LongDouble result))
			{
				Thrower.ParsingError<LongDouble>(s);
			}
			return result;
		}

#if NET8_0_OR_GREATER
		public static LongDouble Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(utf8Text, style, provider, out LongDouble result))
			{
				Thrower.ParsingError<Quad>(utf8Text.ToString());
			}
			return result;
		}

		public static LongDouble Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			if (!TryParse(utf8Text, NumberStyles.Float, provider, out LongDouble result))
			{
				Thrower.ParsingError<LongDouble>(utf8Text.ToString());
			}
			return result;
		}
#endif

		public static LongDouble Pow(LongDouble x, LongDouble y)
		{
			throw new NotImplementedException();
		}

		public static LongDouble RootN(LongDouble x, int n)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Round(LongDouble x, int digits, MidpointRounding mode)
		{
			throw new NotImplementedException();
		}

		public static LongDouble ScaleB(LongDouble x, int n)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Sin(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static (LongDouble Sin, LongDouble Cos) SinCos(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static (LongDouble SinPi, LongDouble CosPi) SinCosPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Sinh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble SinPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Sqrt(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Tan(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble Tanh(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static LongDouble TanPi(LongDouble x)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf16Char.CastFromCharSpan(s), NumberStyles.Float, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf16Char.CastFromCharSpan(s), NumberStyles.Float, provider, out result);
		}

#if NET8_0_OR_GREATER
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out LongDouble result)
		{
			return NumberParser.TryParseFloat(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Float, provider, out result);
		}
#endif

		static LongDouble IFormattableBinaryFloatingPoint<LongDouble>.BitsToFloat(UInt128 bits)
		{
			BitHelper.GetUpperAndLowerBits(bits, out ulong upper, out ulong lower);
			return new LongDouble((ushort)upper, lower);
		}

		static UInt128 IFormattableBinaryFloatingPoint<LongDouble>.FloatToBits(LongDouble value)
		{
			return new UInt128(value._upper, value._lower);
		}

		static bool INumberBase<LongDouble>.TryConvertFromChecked<TOther>(TOther value, out LongDouble result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.TryConvertFromSaturating<TOther>(TOther value, out LongDouble result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.TryConvertFromTruncating<TOther>(TOther value, out LongDouble result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.TryConvertToChecked<TOther>(LongDouble value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.TryConvertToSaturating<TOther>(LongDouble value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<LongDouble>.TryConvertToTruncating<TOther>(LongDouble value, out TOther result)
		{
			throw new NotImplementedException();
		}

		public int CompareTo(object? obj)
		{
			if (obj is not LongDouble other)
			{
				return (obj is null) ? 1 : throw new ArgumentException($"Object must be {typeof(LongDouble)}", nameof(obj));
			}
			return CompareTo(other);
		}

		public int CompareTo(LongDouble other)
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

			Debug.Assert(IsNaN(other));
			return 1;
		}

		public bool Equals(LongDouble other)
		{
			return _upper == other._upper
				|| _lower == other._lower
				|| AreZero(this, other)
				|| (IsNaN(this) && IsNaN(other));
		}

		public int GetExponentByteCount()
		{
			return sizeof(ushort);
		}

		public int GetExponentShortestBitLength()
		{
			return 15;
		}

		public int GetSignificandBitLength()
		{
			return 64;
		}

		public int GetSignificandByteCount()
		{
			return sizeof(ulong);
		}

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FloatToString(in this, format, formatProvider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator +(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator +(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator -(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator -(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator ~(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator ++(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator --(LongDouble value)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator *(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator /(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator %(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator &(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator |(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static LongDouble operator ^(LongDouble left, LongDouble right)
		{
			throw new NotImplementedException();
		}

		public static bool operator ==(LongDouble left, LongDouble right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				return false;
			}

			return (left._lower == right._lower)
				&& ((left._upper == right._upper) || (left._lower == 0 && ((left._upper | right._upper) & 0x7FFF) == 0));
		}

		public static bool operator !=(LongDouble left, LongDouble right)
		{
			return !(left == right);
		}

		public static bool operator <(LongDouble left, LongDouble right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				return false;
			}

			var signA = IsNegative(left);
			var signB = IsNegative(right);

			return (signA != signB)
				? signA && ((uint)((left._upper | right._upper) & 0x7FFF) | left._lower | right._lower) != 0
				: ((left._upper != right._upper) && (left._lower != right._lower))
					|| (signA ^ LessThan(left._upper, left._lower, right._upper, right._lower));
		}

		public static bool operator >(LongDouble left, LongDouble right)
		{
			return right < left;
		}

		public static bool operator <=(LongDouble left, LongDouble right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				return false;
			}

			var signA = IsNegative(left);
			var signB = IsNegative(right);

			return (signA != signB)
				? signA || ((uint)((left._upper | right._upper) & 0x7FFF) | left._lower | right._lower) == 0
				: ((left._upper == right._upper) && (left._lower == right._lower))
					|| (signA ^ LessThan(left._upper, left._lower, right._upper, right._lower));
		}

		public static bool operator >=(LongDouble left, LongDouble right)
		{
			return right <= left;
		}
	}
}
