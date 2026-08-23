using MissingValues.Info;
using MissingValues.Internals;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MissingValues.Primitives;

namespace MissingValues
{
	public partial struct UInt512 :
		IBigInteger<UInt512>,
		IMinMaxValue<UInt512>,
		IUnsignedNumber<UInt512>,
		IPowerFunctions<UInt512>,
		IFormattableUnsignedInteger<UInt512>
	{
		static UInt512 INumberBase<UInt512>.One => One;

		static int INumberBase<UInt512>.Radix => 2;

		static UInt512 INumberBase<UInt512>.Zero => Zero;

		static UInt512 IAdditiveIdentity<UInt512, UInt512>.AdditiveIdentity => Zero;

		static UInt512 IMultiplicativeIdentity<UInt512, UInt512>.MultiplicativeIdentity => One;

		static UInt512 IMinMaxValue<UInt512>.MaxValue => MaxValue;

		static UInt512 IMinMaxValue<UInt512>.MinValue => MinValue;

		static UInt512 IFormattableUnsignedInteger<UInt512>.SignedMaxMagnitude => new UInt512(
			   0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			   0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		static UInt512 IFormattableInteger<UInt512>.Two => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x2);

		static UInt512 IFormattableInteger<UInt512>.Sixteen => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x10);

		static UInt512 IFormattableInteger<UInt512>.Ten => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xA);

		static UInt512 IFormattableInteger<UInt512>.TwoPow2 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4);

		static UInt512 IFormattableInteger<UInt512>.SixteenPow2 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x100);

		static UInt512 IFormattableInteger<UInt512>.TenPow2 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x64);

		static UInt512 IFormattableInteger<UInt512>.TwoPow3 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x8);

		static UInt512 IFormattableInteger<UInt512>.SixteenPow3 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1000);

		static UInt512 IFormattableInteger<UInt512>.TenPow3 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3E8);

		static UInt512 IFormattableInteger<UInt512>.E19 => new UInt512(0, 0, 0, 0, 0, 0, 0, 10000000000000000000UL);

		static char IFormattableInteger<UInt512>.LastDecimalDigitOfMaxValue => '1';

		static int IFormattableInteger<UInt512>.MaxDecimalDigits => 155;

		static int IFormattableInteger<UInt512>.MaxHexDigits => 128;

		static int IFormattableInteger<UInt512>.MaxBinaryDigits => 512;

		static UInt512 INumberBase<UInt512>.Abs(UInt512 value) => value;

		/// <inheritdoc/>
		public static UInt512 Clamp(UInt512 value, UInt512 min, UInt512 max)
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
			if (obj is UInt512 value)
			{
				return CompareTo(value);
			}
			else if (obj is null)
			{
				return 1;
			}
			Thrower.MustBeType<UInt512>();
			return default;
		}

		/// <inheritdoc/>
		public int CompareTo(UInt512 other)
		{
			if (this < other) return -1;
			else if (this > other) return 1;
			else return 0;
		}
		
		static UInt512 IBigInteger<UInt512>.Create(ReadOnlySpan<ulong> parts) => new(parts);

		/// <inheritdoc/>
		public static (UInt512 Quotient, UInt512 Remainder) DivRem(UInt512 left, UInt512 right)
		{
			DivRem(in left, in right, out UInt512 quotient, out UInt512 remainder);

			return (quotient, remainder);
		}
		internal static void DivRem(in UInt512 left, in UInt512 right, out UInt512 quotient, out UInt512 remainder)
		{
			const int UIntCount = Size / sizeof(ulong);

			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
			{
				if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
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

			quotient = new UInt512(quoBits);
			remainder = new UInt512(remBits);
		}

		/// <inheritdoc/>
		public bool Equals(UInt512 other) => this == other;

		int IBinaryInteger<UInt512>.GetByteCount() => Size;

		int IBinaryInteger<UInt512>.GetShortestBitLength()
		{
			UInt512 value = this;
			return (Size * 8) - BitHelper.LeadingZeroCount(in value);
		}

		static bool INumberBase<UInt512>.IsCanonical(UInt512 value) => true;

		static bool INumberBase<UInt512>.IsComplexNumber(UInt512 value) => false;

		/// <inheritdoc/>
		public static bool IsEvenInteger(UInt512 value) => (value._p0 & 1) == 0;

		static bool INumberBase<UInt512>.IsFinite(UInt512 value) => true;

		static bool INumberBase<UInt512>.IsImaginaryNumber(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsInfinity(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsInteger(UInt512 value) => true;

		static bool INumberBase<UInt512>.IsNaN(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsNegative(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsNegativeInfinity(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsNormal(UInt512 value) => value != Zero;

		/// <inheritdoc/>
		public static bool IsOddInteger(UInt512 value) => (value._p0 & 1) != 0;

		static bool INumberBase<UInt512>.IsPositive(UInt512 value) => true;

		static bool INumberBase<UInt512>.IsPositiveInfinity(UInt512 value) => false;

		/// <inheritdoc/>
		public static bool IsPow2(UInt512 value) => BitHelper.PopCount(in value) == 1;

		static bool INumberBase<UInt512>.IsRealNumber(UInt512 value) => true;

		static bool INumberBase<UInt512>.IsSubnormal(UInt512 value) => false;

		static bool INumberBase<UInt512>.IsZero(UInt512 value) => value == Zero;

		/// <inheritdoc/>
		public static UInt512 LeadingZeroCount(UInt512 value) => (UInt512)BitHelper.LeadingZeroCount(in value);

		/// <inheritdoc/>
		public static UInt512 Log2(UInt512 value) => (UInt512)BitHelper.Log2(in value);

		/// <inheritdoc/>
		public static UInt512 Max(UInt512 x, UInt512 y) => (x >= y) ? x : y;

		static UInt512 INumber<UInt512>.MaxNumber(UInt512 x, UInt512 y) => Max(x, y);

		static UInt512 INumberBase<UInt512>.MaxMagnitude(UInt512 x, UInt512 y) => Max(x, y);

		static UInt512 INumberBase<UInt512>.MaxMagnitudeNumber(UInt512 x, UInt512 y) => Max(x, y);

		/// <inheritdoc/>
		public static UInt512 Min(UInt512 x, UInt512 y) => (x <= y) ? x : y;

		static UInt512 INumber<UInt512>.MinNumber(UInt512 x, UInt512 y) => Min(x, y);

		static UInt512 INumberBase<UInt512>.MinMagnitude(UInt512 x, UInt512 y) => Min(x, y);

		static UInt512 INumberBase<UInt512>.MinMagnitudeNumber(UInt512 x, UInt512 y) => Min(x, y);

		static UInt512 INumberBase<UInt512>.MultiplyAddEstimate(UInt512 left, UInt512 right, UInt512 addend) => (left * right) + addend;

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider);
		}

		/// <inheritdoc/>
		public static UInt512 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			return NumberParser.ParseToUnsigned<UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider);
		}

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider);
		}

		/// <inheritdoc/>
		public static UInt512 Parse(string s, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			return NumberParser.ParseToUnsigned<UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider);
		}

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider);
		}
		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider);
		}

		/// <inheritdoc/>
		public static UInt512 PopCount(UInt512 value) => (UInt512)(BitHelper.PopCount(in value));

		static UInt512 IPowerFunctions<UInt512>.Pow(UInt512 x, UInt512 y) => Pow(x, checked((int)y));

		/// <inheritdoc/>
		public static UInt512 RotateLeft(UInt512 value, int rotateAmount) => (value << rotateAmount) | (value >>> (512 - rotateAmount));

		/// <inheritdoc/>
		public static UInt512 RotateRight(UInt512 value, int rotateAmount) => (value >>> rotateAmount) | (value << (512 - rotateAmount));

		/// <inheritdoc/>
		public static UInt512 TrailingZeroCount(UInt512 value) => (UInt512)BitHelper.TrailingZeroCount(in value);
		
		bool IBigInteger<UInt512>.TryCopyTo(Span<ulong> destination)
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
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}

		static bool IBinaryInteger<UInt512>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
		{
			UInt512 result = default;

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
					// We have at least 64 bytes, so just read the ones we need directly
					result = BinaryOperations.ReadUInt512BigEndian(source[^Size..]);
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
				}
			}

			value = result;
			return true;
		}

		static bool IBinaryInteger<UInt512>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
		{
			UInt512 result = default;

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
					// We have at least 64 bytes, so just read the ones we need directly
					return BinaryOperations.TryReadUInt512LittleEndian(source, out value);
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
						UInt512 part = source[i];
						part <<= (i * 8);
						result |= part;
					}
				}
			}

			value = result;
			return true;
		}

		static UInt512 IFormattableNumber<UInt512>.GetDecimalValue(char value)
		{
			if (!char.IsDigit(value))
			{
				throw new FormatException();
			}
			return (UInt512)CharUnicodeInfo.GetDecimalDigitValue(value);
		}

		static UInt512 IFormattableInteger<UInt512>.GetHexValue(char value)
		{
			if (char.IsDigit(value))
			{
				return (UInt512)CharUnicodeInfo.GetDecimalDigitValue(value);
			}
			else if (char.IsAsciiHexDigit(value))
			{
				return (UInt512)(char.ToLowerInvariant(value) - 'W'); // 'W' = 87
			}
			throw new FormatException();
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

		bool IBinaryInteger<UInt512>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
		{
			if (BinaryOperations.TryWriteUInt512BigEndian(destination, in this))
			{
				bytesWritten = Size;
				return true;
			}
			bytesWritten = 0;
			return false;
		}

		bool IBinaryInteger<UInt512>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (BinaryOperations.TryWriteUInt512LittleEndian(destination, in this))
			{
				bytesWritten = Size;
				return true;
			}
			bytesWritten = 0;
			return false;
		}

		static int IFormattableUnsignedInteger<UInt512>.CountDigits(in UInt512 value) => CountDigits(in value);

		internal static int CountDigits(in UInt512 value)
		{
			if (value.Upper == UInt256.Zero)
			{
				return UInt256.CountDigits(value.Lower);
			}

			return BitHelper.Log10(in value) + 1;
		}

		static int IFormattableInteger<UInt512>.UnsignedCompare(in UInt512 value1, in UInt512 value2)
		{
			if (value1 < value2) return -1;
			else if (value1 > value2) return 1;
			else return 0;
		}

		static int IFormattableInteger<UInt512>.Log2Int32(in UInt512 value) => BitHelper.Log2(in value);
		static int IFormattableInteger<UInt512>.LeadingZeroCountInt32(in UInt512 value) => BitHelper.LeadingZeroCount(in value);
		static void IFormattableUnsignedInteger<UInt512>.ToDecChars<TChar>(in UInt512 number, Span<TChar> destination, int digits) => NumberFormatter.UInt512ToDecChars(number, destination, digits);
	}
}
