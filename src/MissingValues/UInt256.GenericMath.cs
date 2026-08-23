using MissingValues.Info;
using MissingValues.Internals;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct UInt256 :
	IBigInteger<UInt256>,
	IMinMaxValue<UInt256>,
	IUnsignedNumber<UInt256>,
	IPowerFunctions<UInt256>,
	IFormattableUnsignedInteger<UInt256>
{
	static UInt256 INumberBase<UInt256>.One => One;

	static int INumberBase<UInt256>.Radix => 2;

	static UInt256 INumberBase<UInt256>.Zero => Zero;

	static UInt256 IAdditiveIdentity<UInt256, UInt256>.AdditiveIdentity => Zero;

	static UInt256 IMultiplicativeIdentity<UInt256, UInt256>.MultiplicativeIdentity => One;

	// 115792089237316195423570985008687907853269984665640564039457584007913129639935
	static UInt256 IMinMaxValue<UInt256>.MaxValue => MaxValue;

	static UInt256 IMinMaxValue<UInt256>.MinValue => MinValue;

	static UInt256 IFormattableInteger<UInt256>.Two => new(0x2);

	static UInt256 IFormattableInteger<UInt256>.Sixteen => new(0x10);

	static UInt256 IFormattableInteger<UInt256>.Ten => new(0xA);

	static char IFormattableInteger<UInt256>.LastDecimalDigitOfMaxValue => '1';

	static int IFormattableInteger<UInt256>.MaxDecimalDigits => 78;

	static int IFormattableInteger<UInt256>.MaxHexDigits => 64;

	static int IFormattableInteger<UInt256>.MaxBinaryDigits => 256;

	static UInt256 IFormattableUnsignedInteger<UInt256>.SignedMaxMagnitude => new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

	static UInt256 IFormattableInteger<UInt256>.TwoPow2 => new(4);

	static UInt256 IFormattableInteger<UInt256>.SixteenPow2 => new(256);

	static UInt256 IFormattableInteger<UInt256>.TenPow2 => new(100);

	static UInt256 IFormattableInteger<UInt256>.TwoPow3 => new(8);

	static UInt256 IFormattableInteger<UInt256>.SixteenPow3 => new(4096);

	static UInt256 IFormattableInteger<UInt256>.TenPow3 => new(1000);

	static UInt256 IFormattableInteger<UInt256>.E19 => new UInt256(0, 0, 0, 10000000000000000000UL);

	static UInt256 INumberBase<UInt256>.Abs(UInt256 value) => value;

	/// <inheritdoc/>
	public static UInt256 Clamp(UInt256 value, UInt256 min, UInt256 max)
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
	public int CompareTo(UInt256 other)
	{
		if (this < other) return -1;
		else if (this > other) return 1;
		else return 0;
	}

	/// <inheritdoc/>
	public int CompareTo(object? obj)
	{
		if (obj is UInt256 value)
		{
			return CompareTo(value);
		}
		else if (obj is null)
		{
			return 1;
		}
		Thrower.MustBeType<UInt256>();
		return default;
	}
		
	static UInt256 IBigInteger<UInt256>.Create(ReadOnlySpan<ulong> parts) => new(parts);

	/// <inheritdoc/>
	public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
	{
		DivRem(in left, in right, out UInt256 quotient, out UInt256 remainder);

		return (quotient, remainder);
	}
	internal static void DivRem(in UInt256 left, in UInt256 right, out UInt256 quotient, out UInt256 remainder)
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
				Calculator.DivRem(in left, right._p0, out quotient, out var r);
				remainder = r;
				return;
			}
		}

		if (right == left)
		{
			quotient = One;
			remainder = Zero;
			return;
		}
		if (right > left)
		{
			remainder = left;
			quotient = Zero;
			return;
		}

		Span<ulong> quotientSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(quotientSpan, in left);

		Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
		BitHelper.Write(divisorSpan, in right);

		Span<ulong> quoBits = stackalloc ulong[UIntCount];
		quoBits.Clear();
		Span<ulong> remBits = stackalloc ulong[UIntCount];
		remBits.Clear();

		Calculator.DivRem(
			quotientSpan[..BitHelper.GetTrimLength(in left)],
			divisorSpan[..BitHelper.GetTrimLength(in right)],
			quoBits,
			remBits);

		quotient = new UInt256(quoBits);
		remainder = new UInt256(remBits);
	}

	/// <inheritdoc/>
	public bool Equals(UInt256 other) => this == other;

	static bool INumberBase<UInt256>.IsCanonical(UInt256 value) => true;

	static bool INumberBase<UInt256>.IsComplexNumber(UInt256 value) => false;

	/// <inheritdoc/>
	public static bool IsEvenInteger(UInt256 value) => (value._p0 & 1) == 0;

	static bool INumberBase<UInt256>.IsFinite(UInt256 value) => true;

	static bool INumberBase<UInt256>.IsImaginaryNumber(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsInfinity(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsInteger(UInt256 value) => true;

	static bool INumberBase<UInt256>.IsNaN(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsNegative(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsNegativeInfinity(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsNormal(UInt256 value) => value != Zero;

	/// <inheritdoc/>
	public static bool IsOddInteger(UInt256 value) => (value._p0 & 1) != 0;

	static bool INumberBase<UInt256>.IsPositive(UInt256 value) => true;

	static bool INumberBase<UInt256>.IsPositiveInfinity(UInt256 value) => false;

	/// <inheritdoc/>
	public static bool IsPow2(UInt256 value) => BitHelper.PopCount(in value) == 1;

	static bool INumberBase<UInt256>.IsRealNumber(UInt256 value) => true;

	static bool INumberBase<UInt256>.IsSubnormal(UInt256 value) => false;

	static bool INumberBase<UInt256>.IsZero(UInt256 value) => value == Zero;

	/// <inheritdoc/>
	public static UInt256 LeadingZeroCount(UInt256 value) => (UInt256)BitHelper.LeadingZeroCount(in value);

	/// <inheritdoc/>
	public static UInt256 Log2(UInt256 value) => (UInt256)BitHelper.Log2(in value);

	/// <inheritdoc/>
	public static UInt256 Max(UInt256 x, UInt256 y) => (x >= y) ? x : y;

	static UInt256 INumber<UInt256>.MaxNumber(UInt256 x, UInt256 y) => Max(x, y);

	static UInt256 INumberBase<UInt256>.MaxMagnitude(UInt256 x, UInt256 y) => Max(x, y);

	static UInt256 INumberBase<UInt256>.MaxMagnitudeNumber(UInt256 x, UInt256 y) => Max(x, y);

	static UInt256 INumberBase<UInt256>.MultiplyAddEstimate(UInt256 left, UInt256 right, UInt256 addend) => (left * right) + addend;

	/// <inheritdoc/>
	public static UInt256 Min(UInt256 x, UInt256 y) => (x <= y) ? x : y;

	static UInt256 INumber<UInt256>.MinNumber(UInt256 x, UInt256 y) => Min(x, y);

	static UInt256 INumberBase<UInt256>.MinMagnitude(UInt256 x, UInt256 y) => Min(x, y);

	static UInt256 INumberBase<UInt256>.MinMagnitudeNumber(UInt256 x, UInt256 y) => Min(x, y);

	/// <inheritdoc/>
	public static UInt256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		return NumberParser.ParseToUnsigned<UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider);
	}

	/// <inheritdoc/>
	public static UInt256 Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		return NumberParser.ParseToUnsigned<UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider);
	}

	/// <inheritdoc/>
	public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		return NumberParser.ParseToUnsigned<UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider);
	}

	/// <inheritdoc/>
	public static UInt256 Parse(string s, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		return NumberParser.ParseToUnsigned<UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider);
	}

	/// <inheritdoc/>
	public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
	{
		return NumberParser.ParseToUnsigned<UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider);
	}
	/// <inheritdoc/>
	public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		return NumberParser.ParseToUnsigned<UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider);
	}

	/// <inheritdoc/>
	public static UInt256 PopCount(UInt256 value) => (UInt256)BitHelper.PopCount(in value);

	static UInt256 IPowerFunctions<UInt256>.Pow(UInt256 x, UInt256 y) => Pow(x, checked((int)y));

	/// <inheritdoc/>
	public static UInt256 RotateLeft(UInt256 value, int rotateAmount) => (value << rotateAmount) | (value >>> (256 - rotateAmount));

	/// <inheritdoc/>
	public static UInt256 RotateRight(UInt256 value, int rotateAmount) => (value >>> rotateAmount) | (value << (256 - rotateAmount));

	/// <inheritdoc/>
	public static UInt256 TrailingZeroCount(UInt256 value) => (UInt256)BitHelper.TrailingZeroCount(in value);
		
	bool IBigInteger<UInt256>.TryCopyTo(Span<ulong> destination)
	{
		if (destination.Length < 4)
		{
			return false;
		}

		destination[0] = _p0;
		destination[1] = _p1;
		destination[2] = _p2;
		destination[3] = _p3;

		return true;
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
	}

#if NET11_0_OR_GREATER
	/// <inheritdoc/>
	public static bool TryParsePartial([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out UInt256 result, out int charsConsumed)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			charsConsumed = 0;
			result = default;
			return false;
		}
		
		return NumberParser.TryParsePartialToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result, out charsConsumed);
	}

	/// <inheritdoc/>
	public static bool TryParsePartial(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UInt256 result, out int charsConsumed)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			charsConsumed = 0;
			result = default;
			return false;
		}
		
		return NumberParser.TryParsePartialToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result, out charsConsumed);
	}

	/// <inheritdoc/>
	public static bool TryParsePartial(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out UInt256 result, out int bytesConsumed)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			bytesConsumed = 0;
			result = default;
			return false;
		}
		
		return NumberParser.TryParsePartialToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result, out bytesConsumed);
	}
#endif

	static bool IBinaryInteger<UInt256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
	{
		UInt256 result = default;

		if (source.Length != 0)
		{
			if (!isUnsigned && sbyte.IsNegative((sbyte)source[0]))
			{
				// When we are signed and the sign bit is set, we are negative and therefore
				// definitely out of range

				value = result;
				return false;
			}

			if ((source.Length > Size) && (source[..^Size].IndexOfAnyExcept((byte)0x00) >= 0))
			{
				// When we have any non-zero leading data, we are a large positive and therefore
				// definitely out of range

				value = result;
				return false;
			}

			if (source.Length >= Size)
			{
				// We have at least 32 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadUInt256BigEndian(source[^Size..]);
			}
			else
			{
				// We have between 1 and 31 bytes, so construct the relevant value directly
				// since the data is in Big Endian format, we can just read the bytes and
				// shift left by 8-bits for each subsequent part

				for (int i = 0; i < source.Length; i++)
				{
					result <<= 8;
					result |= source[i];
				}
			}
		}

		value = result;
		return true;
	}

	static bool IBinaryInteger<UInt256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
	{
		UInt256 result = default;

		if (source.Length != 0)
		{
			if (!isUnsigned && sbyte.IsNegative((sbyte)source[^1]))
			{
				// When we are signed and the sign bit is set, we are negative and therefore
				// definitely out of range

				value = result;
				return false;
			}

			if ((source.Length > Size) && (source[Size..].IndexOfAnyExcept((byte)0x00) >= 0))
			{
				// When we have any non-zero leading data, we are a large positive and therefore
				// definitely out of range

				value = result;
				return false;
			}

			if (source.Length >= Size)
			{
				// We have at least 32 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadUInt256LittleEndian(source);
			}
			else
			{
				// We have between 1 and 31 bytes, so construct the relevant value directly
				// since the data is in Little Endian format, we can just read the bytes and
				// shift left by 8-bits for each subsequent part, then reverse endianness to
				// ensure the order is correct. This is more efficient than iterating in reverse
				// due to current JIT limitations

				for (int i = 0; i < source.Length; i++)
				{
					UInt256 part = source[i];
					part <<= (i * 8);
					result |= part;
				}
			}
		}

		value = result;
		return true;
	}

	int IBinaryInteger<UInt256>.GetByteCount() => Size;

	int IBinaryInteger<UInt256>.GetShortestBitLength()
	{
		UInt256 value = this;
		return (Size * 8) - BitHelper.LeadingZeroCount(in value);
	}

	/// <inheritdoc/>
	public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
	{
		return NumberFormatter.FormatUInt(in this, format, formatProvider);
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatUInt(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatUInt(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
	}

	bool IBinaryInteger<UInt256>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteUInt256BigEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	bool IBinaryInteger<UInt256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteUInt256LittleEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	static UInt256 IFormattableNumber<UInt256>.GetDecimalValue(char value)
	{
		if (!char.IsDigit(value))
		{
			throw new FormatException();
		}
		return (UInt256)CharUnicodeInfo.GetDecimalDigitValue(value);
	}

	static UInt256 IFormattableInteger<UInt256>.GetHexValue(char value)
	{
		if (char.IsDigit(value))
		{
			return (UInt256)CharUnicodeInfo.GetDecimalDigitValue(value);
		}
		else if (char.IsAsciiHexDigit(value))
		{
			return (UInt256)(char.ToLowerInvariant(value) - 'W'); // 'W' = 87
		}
		throw new FormatException();
	}

	internal static int CountDigits(in UInt256 value)
	{
		if (value.Upper == UInt128.Zero)
		{
			return UInt128.CountDigits(value.Lower);
		}
			
		return BitHelper.Log10(in value) + 1;
	}
	static int IFormattableUnsignedInteger<UInt256>.CountDigits(in UInt256 value) => CountDigits(in value);
	static int IFormattableInteger<UInt256>.UnsignedCompare(in UInt256 value1, in UInt256 value2)
	{
		if (value1 < value2) return -1;
		else if (value1 > value2) return 1;
		else return 0;
	}
	static int IFormattableInteger<UInt256>.Log2Int32(in UInt256 value) => BitHelper.Log2(in value);
	static int IFormattableInteger<UInt256>.LeadingZeroCountInt32(in UInt256 value) => BitHelper.LeadingZeroCount(in value);
	static void IFormattableUnsignedInteger<UInt256>.ToDecChars<TChar>(in UInt256 number, Span<TChar> destination, int digits) => NumberFormatter.UInt256ToDecChars(number, destination, digits);
}