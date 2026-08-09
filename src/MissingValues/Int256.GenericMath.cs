using MissingValues.Info;
using MissingValues.Internals;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Int256 :
	IBigInteger<Int256>,
	IMinMaxValue<Int256>,
	ISignedNumber<Int256>,
	IPowerFunctions<Int256>,
	IFormattableSignedInteger<Int256>
{
	static Int256 IBinaryNumber<Int256>.AllBitsSet => new(_lowerMax, _lowerMax);
	// MaxValue = 57896044618658097711785492504343953926634992332820282019728792003956564819967
	static Int256 IMinMaxValue<Int256>.MaxValue => MaxValue;
	// MinValue = -57896044618658097711785492504343953926634992332820282019728792003956564819968
	static Int256 IMinMaxValue<Int256>.MinValue => MinValue;

	static Int256 ISignedNumber<Int256>.NegativeOne => NegativeOne;

	static Int256 INumberBase<Int256>.One => One;

	static int INumberBase<Int256>.Radix => 2;

	static Int256 INumberBase<Int256>.Zero => Zero;

	static Int256 IAdditiveIdentity<Int256, Int256>.AdditiveIdentity => Zero;

	static Int256 IMultiplicativeIdentity<Int256, Int256>.MultiplicativeIdentity => One;

	static Int256 IFormattableInteger<Int256>.Two => new(0x2);

	static Int256 IFormattableInteger<Int256>.Sixteen => new(0x10);

	static Int256 IFormattableInteger<Int256>.Ten => new(0xA);

	static char IFormattableInteger<Int256>.LastDecimalDigitOfMaxValue => '5';

	static int IFormattableInteger<Int256>.MaxDecimalDigits => 77;

	static int IFormattableInteger<Int256>.MaxHexDigits => 64;

	static int IFormattableInteger<Int256>.MaxBinaryDigits => 256;

	static Int256 IFormattableInteger<Int256>.TwoPow2 => new(4);

	static Int256 IFormattableInteger<Int256>.SixteenPow2 => new(256);

	static Int256 IFormattableInteger<Int256>.TenPow2 => new(100);

	static Int256 IFormattableInteger<Int256>.TwoPow3 => new(8);

	static Int256 IFormattableInteger<Int256>.SixteenPow3 => new(4096);

	static Int256 IFormattableInteger<Int256>.TenPow3 => new(1000);

	/// <inheritdoc/>
	public static Int256 Abs(Int256 value)
	{
		if ((long)value._p3 < 0)
		{
			value = -value;

			if ((long)value._p3 < 0)
			{
				Thrower.MinimumSignedAbsoluteValue<Int256>();
			}
		}
		return value;
	}

	/// <inheritdoc/>
	public static (Int256 Quotient, Int256 Remainder) DivRem(Int256 left, Int256 right)
	{
		DivRem(in left, in right, out Int256 quotient, out Int256 remainder);
		return (quotient, remainder);
	}

	private static void DivRem(in Int256 left, in Int256 right, out Int256 quotient, out Int256 remainder)
	{
		if (right == NegativeOne && left == MinValue)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Division);
		}

		// We simplify the logic here by just doing unsigned division on the
		// two's complement representation and then taking the correct sign.

		ulong sign = (left._p3 ^ right._p3) & (1UL << 63);

		UInt256.DivRem(
			(UInt256)((long)left._p3 < 0 ? (~left + One) : left),
			(UInt256)((long)right._p3 < 0 ? (~right + One) : right),
			out UInt256 quo,
			out UInt256 rem);
			
		if (sign != 0)
		{
			quotient = unchecked((Int256)(~quo + UInt256.One));
			remainder = unchecked((Int256)(~rem + UInt256.One));
		}
		else
		{
			quotient = unchecked((Int256)quo);
			remainder = unchecked((Int256)rem);
		}
	}

	static bool INumberBase<Int256>.IsCanonical(Int256 value) => true;

	static bool INumberBase<Int256>.IsComplexNumber(Int256 value) => false;

	/// <inheritdoc/>
	public static bool IsEvenInteger(Int256 value) => (value._p0 & 1) == 0;

	static bool INumberBase<Int256>.IsFinite(Int256 value) => true;

	static bool INumberBase<Int256>.IsImaginaryNumber(Int256 value) => false;

	static bool INumberBase<Int256>.IsInfinity(Int256 value) => false;

	static bool INumberBase<Int256>.IsInteger(Int256 value) => true;

	static bool INumberBase<Int256>.IsNaN(Int256 value) => false;

	/// <inheritdoc/>
	public static bool IsNegative(Int256 value) => (long)value._p3 < 0;

	static bool INumberBase<Int256>.IsNegativeInfinity(Int256 value) => false;

	static bool INumberBase<Int256>.IsNormal(Int256 value) => value != Zero;

	/// <inheritdoc/>
	public static bool IsOddInteger(Int256 value) => (value._p0 & 1) != 0;

	/// <inheritdoc/>
	public static bool IsPositive(Int256 value) => (long)value._p3 >= 0;

	static bool INumberBase<Int256>.IsPositiveInfinity(Int256 value) => false;

	/// <inheritdoc/>
	public static bool IsPow2(Int256 value) => (BitHelper.PopCount(in value) == 1) && ((long)value._p3 >= 0);

	static bool INumberBase<Int256>.IsRealNumber(Int256 value) => true;

	static bool INumberBase<Int256>.IsSubnormal(Int256 value) => false;

	static bool INumberBase<Int256>.IsZero(Int256 value) => (value == Zero);

	/// <inheritdoc/>
	public static Int256 Clamp(Int256 value, Int256 min, Int256 max)
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
	public static Int256 CopySign(Int256 value, Int256 sign)
	{
		var absValue = value;

		if ((long)absValue._p3 < 0)
		{
			absValue = -absValue;
		}

		if ((long)sign._p3 >= 0)
		{
			if ((long)absValue._p3 < 0)
			{
				Thrower.MinimumSignedAbsoluteValue<Int256>();
			}
			return absValue;
		}
		return -absValue;
	}

	/// <inheritdoc/>
	public static Int256 LeadingZeroCount(Int256 value)
	{
		return BitHelper.LeadingZeroCount(in value);
	}

	/// <inheritdoc/>
	public static Int256 Log2(Int256 value)
	{
		return BitHelper.Log2(in value);
	}

	/// <inheritdoc/>
	public static Int256 MaxMagnitude(Int256 x, Int256 y)
	{
		Int256 absX = x;

		if ((long)absX._p3 < 0)
		{
			absX = -absX;

			if ((long)absX._p3 < 0)
			{
				return x;
			}
		}

		Int256 absY = y;

		if ((long)absY._p3 < 0)
		{
			absY = -absY;

			if ((long)absY._p3 < 0)
			{
				return y;
			}
		}

		if (absX > absY)
		{
			return x;
		}

		if (absX == absY)
		{
			return (long)x._p3 < 0 ? y : x;
		}

		return y;
	}

	static Int256 INumberBase<Int256>.MaxMagnitudeNumber(Int256 x, Int256 y) => MaxMagnitude(x, y);

	/// <inheritdoc/>
	public static Int256 MinMagnitude(Int256 x, Int256 y)
	{
		Int256 absX = x;

		if ((long)absX._p3 < 0)
		{
			absX = -absX;

			if ((long)absX._p3 < 0)
			{
				return y;
			}
		}

		Int256 absY = y;

		if ((long)absY._p3 < 0)
		{
			absY = -absY;

			if ((long)absY._p3 < 0)
			{
				return x;
			}
		}

		if (absX < absY)
		{
			return x;
		}

		if (absX == absY)
		{
			return (long)x._p3 < 0 ? x : y;
		}

		return y;
	}

	static Int256 INumberBase<Int256>.MinMagnitudeNumber(Int256 x, Int256 y) => MinMagnitude(x, y);

#if NET9_0_OR_GREATER
		static Int256 INumberBase<Int256>.MultiplyAddEstimate(Int256 left, Int256 right, Int256 addend) => (left * right) + addend;
#endif

	/// <inheritdoc/>
	public static Int256 Max(Int256 x, Int256 y) => (x >= y) ? x : y;

	static Int256 INumber<Int256>.MaxNumber(Int256 x, Int256 y) => Max(x, y);

	/// <inheritdoc/>
	public static Int256 Min(Int256 x, Int256 y) => (x <= y) ? x : y;

	static Int256 INumber<Int256>.MinNumber(Int256 x, Int256 y) => Min(x, y);

	/// <inheritdoc/>
	public static int Sign(Int256 value)
	{
		if ((long)value._p3 < 0)
		{
			return -1;
		}
		else if (value != Zero)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}

	/// <inheritdoc/>
	public static Int256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int256 Parse(string? s, NumberStyles style, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int256 Parse(string? s, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(utf8Text);
		}
		return output;
	}
	/// <inheritdoc/>
	public static Int256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out Int256 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int256>(utf8Text);
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int256 PopCount(Int256 value) => BitHelper.PopCount(in value);

	static Int256 IPowerFunctions<Int256>.Pow(Int256 x, Int256 y) => Pow(x, checked((int)y));

	/// <inheritdoc/>
	public static Int256 RotateLeft(Int256 value, int rotateAmount) => (value << rotateAmount) | (value >>> (256 - rotateAmount));

	/// <inheritdoc/>
	public static Int256 RotateRight(Int256 value, int rotateAmount) => (value >>> rotateAmount) | (value << (256 - rotateAmount));

	/// <inheritdoc/>
	public static Int256 TrailingZeroCount(Int256 value) => BitHelper.TrailingZeroCount(in value);

	bool IBigInteger<Int256>.TryCopyTo(Span<ulong> destination)
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
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result).IsSuccessful();
	}
	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	static bool IBinaryInteger<Int256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
	{
		Int256 result = default;

		if (source.Length != 0)
		{
			// Propagate the most significant bit so we have `0` or `-1`
			sbyte sign = (sbyte)(source[0]);
			sign = (sbyte)(sign >> 31);
			Debug.Assert((sign == 0) || (sign == -1));

			// We need to also track if the input data is unsigned
			isUnsigned |= (sign == 0);

			if (isUnsigned && sbyte.IsNegative(sign) && (source.Length >= Size))
			{
				// When we are unsigned and the most significant bit is set, we are a large positive
				// and therefore definitely out of range

				value = result;
				return false;
			}

			if (source.Length > Size)
			{
				if (source[..^Size].IndexOfAnyExcept((byte)sign) >= 0)
				{
					// When we are unsigned and have any non-zero leading data or signed with any non-set leading
					// data, we are a large positive/negative, respectively, and therefore definitely out of range

					value = result;
					return false;
				}

				if (isUnsigned == sbyte.IsNegative((sbyte)source[^Size]))
				{
					// When the most significant bit of the value being set/clear matches whether we are unsigned
					// or signed then we are a large positive/negative and therefore definitely out of range

					value = result;
					return false;
				}
			}

			if (source.Length >= Size)
			{
				// We have at least 32 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadInt256BigEndian(source[^Size..]);
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

				if (!isUnsigned)
				{
					result |= ((One << ((Size * 8) - 1)) >> (((Size - source.Length) * 8) - 1));
				}
			}
		}

		value = result;
		return true;
	}

	static bool IBinaryInteger<Int256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
	{
		Int256 result = default;

		if (source.Length != 0)
		{
			// Propagate the most significant bit so we have `0` or `-1`
			sbyte sign = (sbyte)(source[^1]);
			sign = (sbyte)(sign >> 31);
			Debug.Assert((sign == 0) || (sign == -1));

			// We need to also track if the input data is unsigned
			isUnsigned |= (sign == 0);

			if (isUnsigned && sbyte.IsNegative(sign) && (source.Length >= Size))
			{
				// When we are unsigned and the most significant bit is set, we are a large positive
				// and therefore definitely out of range

				value = result;
				return false;
			}

			if (source.Length > Size)
			{
				if (source[Size..].IndexOfAnyExcept((byte)sign) >= 0)
				{
					// When we are unsigned and have any non-zero leading data or signed with any non-set leading
					// data, we are a large positive/negative, respectively, and therefore definitely out of range

					value = result;
					return false;
				}

				if (isUnsigned == sbyte.IsNegative((sbyte)source[Size - 1]))
				{
					// When the most significant bit of the value being set/clear matches whether we are unsigned
					// or signed then we are a large positive/negative and therefore definitely out of range

					value = result;
					return false;
				}
			}

			if (source.Length >= Size)
			{
				// We have at least 32 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadInt256LittleEndian(source);
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
					result <<= 8;
					result |= source[i];
				}

				result <<= ((Size - source.Length) * 8);
				result = BinaryOperations.ReverseEndianness(in result);

				if (!isUnsigned)
				{
					result |= ((One << ((Size * 8) - 1)) >> (((Size - source.Length) * 8) - 1));
				}
			}
		}

		value = result;
		return true;
	}

	/// <inheritdoc/>
	public int CompareTo(object? obj)
	{
		if (obj is Int256 other)
		{
			return CompareTo(other);
		}
		else if (obj is null)
		{
			return 1;
		}
		else
		{
			Thrower.MustBeType<Int256>();
			return default;
		}
	}

	/// <inheritdoc/>
	public int CompareTo(Int256 other)
	{
		if (this < other)
		{
			return -1;
		}
		else if (this > other)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
		
	static Int256 IBigInteger<Int256>.Create(ReadOnlySpan<ulong> parts) => new(parts);

	/// <inheritdoc/>
	public bool Equals(Int256 other)
	{
		return this == other;
	}

	/// <inheritdoc/>
	int IBinaryInteger<Int256>.GetByteCount()
	{
		return Size;
	}

	int IBinaryInteger<Int256>.GetShortestBitLength()
	{
		Int256 value = this;

		if ((long)value._p3 >= 0)
		{
			return (Size * 8) - BitHelper.LeadingZeroCount(in value);
		}
		else
		{
			return (Size * 8) + 1 - BitHelper.LeadingZeroCount(~value);
		}
	}

	/// <inheritdoc/>
	public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
	{
		return NumberFormatter.FormatInt<Int256, UInt256>(in this, format, formatProvider);
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatInt<Int256, UInt256, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
	}
	/// <inheritdoc/>
	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatInt<Int256, UInt256, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
	}

	bool IBinaryInteger<Int256>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteInt256BigEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	bool IBinaryInteger<Int256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteInt256LittleEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	static Int256 IFormattableNumber<Int256>.GetDecimalValue(char value)
	{
		if (!char.IsDigit(value))
		{
			throw new FormatException();
		}
		return (Int256)CharUnicodeInfo.GetDecimalDigitValue(value);
	}

	static Int256 IFormattableInteger<Int256>.GetHexValue(char value)
	{
		if (char.IsDigit(value))
		{
			return (Int256)CharUnicodeInfo.GetDecimalDigitValue(value);
		}
		else if (char.IsAsciiHexDigit(value))
		{
			return (Int256)(char.ToLowerInvariant(value) - 'W'); // 'W' = 87
		}
		throw new FormatException();
	}

	static int IFormattableInteger<Int256>.UnsignedCompare(in Int256 value1, in Int256 value2) => unchecked(((UInt256)value1).CompareTo((UInt256)value2));
	static int IFormattableInteger<Int256>.Log2Int32(in Int256 value) => BitHelper.Log2(in value);
	static int IFormattableInteger<Int256>.LeadingZeroCountInt32(in Int256 value) => BitHelper.LeadingZeroCount(in value);
}