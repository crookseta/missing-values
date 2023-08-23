using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public partial struct Quad :
		IBinaryFloatingPointIeee754<Quad>,
		IMinMaxValue<Quad>
	{
		public static Quad Epsilon => throw new NotImplementedException();

		public static Quad NaN => throw new NotImplementedException();

		public static Quad NegativeInfinity => throw new NotImplementedException();

		public static Quad NegativeZero => throw new NotImplementedException();

		public static Quad PositiveInfinity => throw new NotImplementedException();

		public static Quad NegativeOne => throw new NotImplementedException();

		public static Quad E => throw new NotImplementedException();

		public static Quad Pi => throw new NotImplementedException();

		public static Quad Tau => throw new NotImplementedException();

		public static Quad One => throw new NotImplementedException();

		public static int Radix => throw new NotImplementedException();

		public static Quad Zero => throw new NotImplementedException();

		public static Quad AdditiveIdentity => throw new NotImplementedException();

		public static Quad MultiplicativeIdentity => throw new NotImplementedException();

		public static Quad MaxValue => throw new NotImplementedException();

		public static Quad MinValue => throw new NotImplementedException();

		public static Quad Abs(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad Acos(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Acosh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad AcosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Asin(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Asinh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad AsinPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Atan(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Atan2(Quad y, Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Atan2Pi(Quad y, Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Atanh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad AtanPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad BitDecrement(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad BitIncrement(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Cbrt(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Cos(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Cosh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad CosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Exp(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Exp10(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Exp2(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad FusedMultiplyAdd(Quad left, Quad right, Quad addend)
		{
			throw new NotImplementedException();
		}

		public static Quad Hypot(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad Ieee754Remainder(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static int ILogB(Quad x)
		{
			throw new NotImplementedException();
		}

		public static bool IsCanonical(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsComplexNumber(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsEvenInteger(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsFinite(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsImaginaryNumber(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsInfinity(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsInteger(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNaN(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNegative(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNegativeInfinity(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsNormal(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsOddInteger(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsPositive(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsPositiveInfinity(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsPow2(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsRealNumber(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsSubnormal(Quad value)
		{
			throw new NotImplementedException();
		}

		public static bool IsZero(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad Log(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Log(Quad x, Quad newBase)
		{
			throw new NotImplementedException();
		}

		public static Quad Log10(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Log2(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad MaxMagnitude(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad MaxMagnitudeNumber(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad MinMagnitude(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad MinMagnitudeNumber(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public static Quad Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public static Quad Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public static Quad Parse(string s, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public static Quad Pow(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}

		public static Quad RootN(Quad x, int n)
		{
			throw new NotImplementedException();
		}

		public static Quad Round(Quad x, int digits, MidpointRounding mode)
		{
			throw new NotImplementedException();
		}

		public static Quad ScaleB(Quad x, int n)
		{
			throw new NotImplementedException();
		}

		public static Quad Sin(Quad x)
		{
			throw new NotImplementedException();
		}

		public static (Quad Sin, Quad Cos) SinCos(Quad x)
		{
			throw new NotImplementedException();
		}

		public static (Quad SinPi, Quad CosPi) SinCosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Sinh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad SinPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Sqrt(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Tan(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Tanh(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad TanPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertFromChecked<TOther>(TOther value, out Quad result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertFromSaturating<TOther>(TOther value, out Quad result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertFromTruncating<TOther>(TOther value, out Quad result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertToChecked<TOther>(Quad value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertToSaturating<TOther>(Quad value, out TOther result)
		{
			throw new NotImplementedException();
		}

		static bool INumberBase<Quad>.TryConvertToTruncating<TOther>(Quad value, out TOther result)
		{
			throw new NotImplementedException();
		}

		public int CompareTo(object? obj)
		{
			throw new NotImplementedException();
		}

		public int CompareTo(Quad other)
		{
			throw new NotImplementedException();
		}

		public bool Equals(Quad other)
		{
			throw new NotImplementedException();
		}

		public int GetExponentByteCount()
		{
			throw new NotImplementedException();
		}

		public int GetExponentShortestBitLength()
		{
			throw new NotImplementedException();
		}

		public int GetSignificandBitLength()
		{
			throw new NotImplementedException();
		}

		public int GetSignificandByteCount()
		{
			throw new NotImplementedException();
		}

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			throw new NotImplementedException();
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

		public static Quad operator +(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad operator +(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator -(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad operator -(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator ~(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad operator ++(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad operator --(Quad value)
		{
			throw new NotImplementedException();
		}

		public static Quad operator *(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator /(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator %(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator &(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator |(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static Quad operator ^(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator ==(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator !=(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator <(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator >(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator <=(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}

		public static bool operator >=(Quad left, Quad right)
		{
			throw new NotImplementedException();
		}
	}
}
