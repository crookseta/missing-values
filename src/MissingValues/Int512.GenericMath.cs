using MissingValues.Info;
using MissingValues.Internals;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Int512 :
	IBigInteger<Int512>,
	IMinMaxValue<Int512>,
	ISignedNumber<Int512>,
	IPowerFunctions<Int512>,
	IFormattableSignedInteger<Int512>
{
	private static UInt256 _upperMin => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
	private static UInt256 _lowerMin => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

	private static UInt256 _upperMax => new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	private static UInt256 _lowerMax => new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

	static Int512 IBinaryNumber<Int512>.AllBitsSet => new Int512(_lowerMax, _lowerMax);

	static Int512 INumberBase<Int512>.One => One;

	static int INumberBase<Int512>.Radix => 2;

	static Int512 INumberBase<Int512>.Zero => Zero;

	static Int512 IAdditiveIdentity<Int512, Int512>.AdditiveIdentity => Zero;

	static Int512 IMultiplicativeIdentity<Int512, Int512>.MultiplicativeIdentity => One;

	static Int512 IMinMaxValue<Int512>.MaxValue => MaxValue;

	static Int512 IMinMaxValue<Int512>.MinValue => MinValue;

	static Int512 ISignedNumber<Int512>.NegativeOne => NegativeOne;

	static Int512 IFormattableInteger<Int512>.Two => new Int512(0x2);

	static Int512 IFormattableInteger<Int512>.Sixteen => new Int512(0x10);

	static Int512 IFormattableInteger<Int512>.Ten => new Int512(0xA);

	static Int512 IFormattableInteger<Int512>.TwoPow2 => new Int512(0x4);

	static Int512 IFormattableInteger<Int512>.SixteenPow2 => new Int512(0x100);

	static Int512 IFormattableInteger<Int512>.TenPow2 => new Int512(0x64);

	static Int512 IFormattableInteger<Int512>.TwoPow3 => new Int512(0x8);

	static Int512 IFormattableInteger<Int512>.SixteenPow3 => new Int512(0x1000);

	static Int512 IFormattableInteger<Int512>.TenPow3 => new Int512(0x3E8);

	static char IFormattableInteger<Int512>.LastDecimalDigitOfMaxValue => '6';

	static int IFormattableInteger<Int512>.MaxDecimalDigits => 154;

	static int IFormattableInteger<Int512>.MaxHexDigits => 128;

	static int IFormattableInteger<Int512>.MaxBinaryDigits => 512;

	/// <inheritdoc/>
	public static Int512 Abs(Int512 value)
	{
		if ((long)value._p7 < 0)
		{
			value = -value;

			if ((long)value._p7 < 0)
			{
				Thrower.MinimumSignedAbsoluteValue<Int512>();
			}
		}
		return value;
	}

	/// <inheritdoc/>
	public static Int512 Clamp(Int512 value, Int512 min, Int512 max)
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
	public int CompareTo(object? obj)
	{
		if (obj is Int512 value)
		{
			return CompareTo(value);
		}
		else if (obj is null)
		{
			return 1;
		}
		Thrower.MustBeType<Int512>();
		return default;
	}

	/// <inheritdoc/>
	public int CompareTo(Int512 other)
	{
		if (this < other) return -1;
		else if (this > other) return 1;
		else return 0;
	}

	/// <inheritdoc/>
	public static Int512 CopySign(Int512 value, Int512 sign)
	{
		var absValue = value;

		if ((long)absValue._p7 < 0)
		{
			absValue = -absValue;
		}

		if ((long)sign._p7 >= 0)
		{
			if ((long)absValue._p7 < 0)
			{
				Thrower.MinimumSignedAbsoluteValue<Int512>();
			}
			return absValue;
		}
		return -absValue;
	}
		
	static Int512 IBigInteger<Int512>.Create(ReadOnlySpan<ulong> parts) => new(parts);

	/// <inheritdoc/>
	public static (Int512 Quotient, Int512 Remainder) DivRem(Int512 left, Int512 right)
	{
		DivRem(in left, in right, out Int512 quotient, out Int512 remainder);
		return (quotient, remainder);
	}
		
	private static void DivRem(in Int512 left, in Int512 right, out Int512 quotient, out Int512 remainder)
	{
		if (right == NegativeOne && left == MinValue)
		{
			Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Division);
		}

		// We simplify the logic here by just doing unsigned division on the
		// two's complement representation and then taking the correct sign.

		ulong sign = (left._p7 ^ right._p7) & (1UL << 63);

		UInt512.DivRem(
			(UInt512)((long)left._p7 < 0 ? (~left + One) : left),
			(UInt512)((long)right._p7 < 0 ? (~right + One) : right),
			out UInt512 quo,
			out UInt512 rem);
			
		if (sign != 0)
		{
			quotient = unchecked((Int512)(~quo + UInt512.One));
			remainder = unchecked((Int512)(~rem + UInt512.One));
		}
		else
		{
			quotient = unchecked((Int512)quo);
			remainder = unchecked((Int512)rem);
		}
	}

	/// <inheritdoc/>
	public bool Equals(Int512 other) => this == other;

	int IBinaryInteger<Int512>.GetByteCount() => Size;

	int IBinaryInteger<Int512>.GetShortestBitLength()
	{
		Int512 value = this;

		if ((long)value._p7 >= 0)
		{
			return (Size * 8) - BitHelper.LeadingZeroCount(in value);
		}
		else
		{
			return (Size * 8) + 1 - BitHelper.LeadingZeroCount(~value);
		}
	}

	static bool INumberBase<Int512>.IsCanonical(Int512 value) => true;

	static bool INumberBase<Int512>.IsComplexNumber(Int512 value) => false;

	/// <inheritdoc/>
	public static bool IsEvenInteger(Int512 value) => (value._p0 & 1) == 0;

	static bool INumberBase<Int512>.IsFinite(Int512 value) => true;

	static bool INumberBase<Int512>.IsImaginaryNumber(Int512 value) => false;

	static bool INumberBase<Int512>.IsInfinity(Int512 value) => false;

	static bool INumberBase<Int512>.IsInteger(Int512 value) => true;

	static bool INumberBase<Int512>.IsNaN(Int512 value) => false;

	/// <inheritdoc/>
	public static bool IsNegative(Int512 value) => (long)value._p7 < 0;

	static bool INumberBase<Int512>.IsNegativeInfinity(Int512 value) => false;

	static bool INumberBase<Int512>.IsNormal(Int512 value) => value != Zero;

	/// <inheritdoc/>
	public static bool IsOddInteger(Int512 value) => (value._p0 & 1) != 0;

	/// <inheritdoc/>
	public static bool IsPositive(Int512 value) => (long)value._p7 >= 0;

	static bool INumberBase<Int512>.IsPositiveInfinity(Int512 value) => false;

	/// <inheritdoc/>
	public static bool IsPow2(Int512 value) => (BitHelper.PopCount(in value) == 1) && (long)value._p7 >= 0;

	static bool INumberBase<Int512>.IsRealNumber(Int512 value) => true;

	static bool INumberBase<Int512>.IsSubnormal(Int512 value) => false;

	static bool INumberBase<Int512>.IsZero(Int512 value) => (value == Zero);

	/// <inheritdoc/>
	public static Int512 LeadingZeroCount(Int512 value) => BitHelper.LeadingZeroCount(in value);

	/// <inheritdoc/>
	public static Int512 Log2(Int512 value) => BitHelper.Log2(in value);

	/// <inheritdoc/>
	public static Int512 Max(Int512 x, Int512 y) => (x >= y) ? x : y;

	static Int512 INumber<Int512>.MaxNumber(Int512 x, Int512 y) => Max(x, y);

	/// <inheritdoc/>
	public static Int512 MaxMagnitude(Int512 x, Int512 y)
	{
		Int512 absX = x;

		if ((long)absX._p7 < 0)
		{
			absX = -absX;

			if ((long)absX._p7 < 0)
			{
				return x;
			}
		}

		Int512 absY = y;

		if ((long)absY._p7 < 0)
		{
			absY = -absY;

			if ((long)absY._p7 < 0)
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
			return (long)x._p7 < 0 ? y : x;
		}

		return y;
	}

	static Int512 INumberBase<Int512>.MaxMagnitudeNumber(Int512 x, Int512 y) => MaxMagnitude(x, y);

	/// <inheritdoc/>
	public static Int512 Min(Int512 x, Int512 y) => (x <= y) ? x : y;

	static Int512 INumber<Int512>.MinNumber(Int512 x, Int512 y) => Min(x, y);

	/// <inheritdoc/>
	public static Int512 MinMagnitude(Int512 x, Int512 y)
	{
		Int512 absX = x;

		if ((long)absX._p7 < 0)
		{
			absX = -absX;

			if ((long)absX._p7 < 0)
			{
				return y;
			}
		}

		Int512 absY = y;

		if ((long)absY._p7 < 0)
		{
			absY = -absY;

			if ((long)absY._p7 < 0)
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
			return (long)x._p7 < 0 ? x : y;
		}

		return y;
	}

	static Int512 INumberBase<Int512>.MinMagnitudeNumber(Int512 x, Int512 y) => MinMagnitude(x, y);

	static Int512 INumberBase<Int512>.MultiplyAddEstimate(Int512 left, Int512 right, Int512 addend) => (left * right) + addend;

	/// <inheritdoc/>
	public static Int512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 Parse(string s, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(s.ToString());
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(utf8Text);
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
	{
		var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out Int512 output);
		if (!status.IsSuccessful())
		{
			status.Throw<Int512>(utf8Text);
		}
		return output;
	}

	/// <inheritdoc/>
	public static Int512 PopCount(Int512 value) => (Int512)(BitHelper.PopCount(in value));

	static Int512 IPowerFunctions<Int512>.Pow(Int512 x, Int512 y) => Pow(x, checked((int)y));

	/// <inheritdoc/>
	public static Int512 RotateLeft(Int512 value, int rotateAmount) => (value << rotateAmount) | (value >>> (512 - rotateAmount));

	/// <inheritdoc/>
	public static Int512 RotateRight(Int512 value, int rotateAmount) => (value >>> rotateAmount) | (value << (512 - rotateAmount));

	/// <inheritdoc/>
	public static int Sign(Int512 value)
	{
		if ((long)value._p7 < 0)
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
	public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
	{
		return NumberFormatter.FormatInt<Int512, UInt512>(in this, format, formatProvider);
	}

	/// <inheritdoc/>
	public static Int512 TrailingZeroCount(Int512 value) => BitHelper.TrailingZeroCount(in value);
		
	bool IBigInteger<Int512>.TryCopyTo(Span<ulong> destination)
	{
		if (destination.Length < 8)
		{
			return false;
		}

		destination[0] = _p0;
		destination[1] = _p1;
		destination[2] = _p2;
		destination[3] = _p3;
		destination[4] = _p4;
		destination[5] = _p5;
		destination[6] = _p6;
		destination[7] = _p7;

		return true;
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatInt<Int512, UInt512, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		return NumberFormatter.TryFormatInt<Int512, UInt512, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (s.Length == 0 || s.IsWhiteSpace())
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result).IsSuccessful();
	}
	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
	{
		if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
		{
			result = default;
			return false;
		}

		return NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result).IsSuccessful();
	}

	static bool IBinaryInteger<Int512>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int512 value)
	{
		Int512 result = default;

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
				// We have at least 64 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadInt512BigEndian(source[^Size..]);
			}
			else
			{
				// We have between 1 and 63 bytes, so construct the relevant value directly
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

	static bool IBinaryInteger<Int512>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int512 value)
	{
		Int512 result = default;

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
				// We have at least 64 bytes, so just read the ones we need directly
				result = BinaryOperations.ReadInt512LittleEndian(source);
			}
			else
			{
				// We have between 1 and 63 bytes, so construct the relevant value directly
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

	static Int512 IFormattableNumber<Int512>.GetDecimalValue(char value)
	{
		if (!char.IsDigit(value))
		{
			throw new FormatException();
		}
		return (Int512)CharUnicodeInfo.GetDecimalDigitValue(value);
	}

	static Int512 IFormattableInteger<Int512>.GetHexValue(char value)
	{
		if (char.IsDigit(value))
		{
			return (Int512)CharUnicodeInfo.GetDecimalDigitValue(value);
		}
		else if (char.IsAsciiHexDigit(value))
		{
			return (Int512)(char.ToLowerInvariant(value) - 'W'); // 'W' = 87
		}
		throw new FormatException();
	}

	bool IBinaryInteger<Int512>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteInt512BigEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	bool IBinaryInteger<Int512>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
	{
		if (BinaryOperations.TryWriteInt512LittleEndian(destination, in this))
		{
			bytesWritten = Size;
			return true;
		}
		bytesWritten = 0;
		return false;
	}

	static int IFormattableInteger<Int512>.UnsignedCompare(in Int512 value1, in Int512 value2) => unchecked(((UInt512)value1).CompareTo((UInt512)value2));
	static int IFormattableInteger<Int512>.Log2Int32(in Int512 value) => BitHelper.Log2(in value);
	static int IFormattableInteger<Int512>.LeadingZeroCountInt32(in Int512 value) => BitHelper.LeadingZeroCount(in value);
}