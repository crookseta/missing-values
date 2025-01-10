using MissingValues.Info;
using MissingValues.Internals;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MissingValues
{
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

		/// <inheritdoc/>
		public static Int512 CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int512 result;

			if (value is Int512 v)
			{
				result = v;
			}
			else if (!Int512.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<Int512>(value, out result))
			{
				Thrower.NotSupported<Int512, TOther>();
			}

			return result;
		}

		/// <inheritdoc/>
		public static Int512 CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int512 result;

			if (value is Int512 v)
			{
				result = v;
			}
			else if (!Int512.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<Int512>(value, out result))
			{
				Thrower.NotSupported<Int512, TOther>();
			}

			return result;
		}

		/// <inheritdoc/>
		public static Int512 CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int512 result;

			if (value is Int512 v)
			{
				result = v;
			}
			else if (!Int512.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<Int512>(value, out result))
			{
				Thrower.NotSupported<Int512, TOther>();
			}

			return result;
		}

		/// <inheritdoc/>
		public static (Int512 Quotient, Int512 Remainder) DivRem(Int512 left, Int512 right)
		{
			Int512 quotient = left / right;
			return (quotient, left - (quotient * right));
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

#if NET9_0_OR_GREATER
		static Int512 INumberBase<Int512>.MultiplyAddEstimate(Int512 left, Int512 right, Int512 addend) => (left * right) + addend;
#endif

		/// <inheritdoc/>
		public static Int512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int512 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int512 Parse(string s, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int512 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(utf8Text);
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int512 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out Int512 output);
			if (!status)
			{
				status.Throw<Int256>(utf8Text);
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

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}

		static bool IBinaryInteger<Int512>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int512 value)
		{
			Int512 result = default;

			if (source.Length != 0)
			{
				// Propagate the most significant bit so we have `0` or `-1`
				sbyte sign = (sbyte)(source[0]);
				sign >>= 31;
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					sourceRef = ref Unsafe.Add(ref sourceRef, source.Length - Size);

					// We have at least 64 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<Int512>(ref sourceRef);

					if (BitConverter.IsLittleEndian)
					{
						result = BitHelper.ReverseEndianness(in result);
					}
				}
				else
				{
					// We have between 1 and 63 bytes, so construct the relevant value directly
					// since the data is in Big Endian format, we can just read the bytes and
					// shift left by 8-bits for each subsequent part

					for (int i = 0; i < source.Length; i++)
					{
						result <<= 8;
						result |= Unsafe.Add(ref sourceRef, i);
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
				sign >>= 31;
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					// We have at least 64 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<Int512>(ref sourceRef);

					if (!BitConverter.IsLittleEndian)
					{
						result = BitHelper.ReverseEndianness(in result);
					}
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
						result |= Unsafe.Add(ref sourceRef, i);
					}

					result <<= ((Size - source.Length) * 8);
					result = BitHelper.ReverseEndianness(in result);

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

		static bool INumberBase<Int512>.TryConvertFromChecked<TOther>(TOther value, out Int512 result) => TryConvertFromChecked(value, out result);
		private static bool TryConvertFromChecked<TOther>(TOther value, out Int512 result)
			where TOther : INumberBase<TOther>
		{
			bool converted = true;

			checked
			{
				result = value switch
				{
					char actual => (Int512)actual,
					Half actual => (Int512)actual,
					float actual => (Int512)actual,
					double actual => (Int512)actual,
					Quad actual => (Int512)actual,
					decimal actual => (Int512)actual,
					byte actual => (Int512)actual,
					ushort actual => (Int512)actual,
					uint actual => (Int512)actual,
					ulong actual => (Int512)actual,
					UInt128 actual => (Int512)actual,
					UInt256 actual => (Int512)actual,
					UInt512 actual => (Int512)actual,
					nuint actual => (Int512)actual,
					sbyte actual => (Int512)actual,
					short actual => (Int512)actual,
					int actual => (Int512)actual,
					long actual => (Int512)actual,
					Int128 actual => (Int512)actual,
					Int256 actual => (Int512)actual,
					Int512 actual => actual,
					nint actual => (Int512)actual,
					BigInteger actual => (Int512)actual,
					_ => BitHelper.DefaultConvert<Int512>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertFromSaturating<TOther>(TOther value, out Int512 result) => TryConvertFromSaturating(value, out result);
		private static bool TryConvertFromSaturating<TOther>(TOther value, out Int512 result)
			where TOther : INumberBase<TOther>
		{
			const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
				float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
				double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
				Quad actual => (actual <= (new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MinValue : (actual > (new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MaxValue : (Int512)actual,
				decimal actual => (Int512)actual,
				byte actual => (Int512)actual,
				ushort actual => (Int512)actual,
				uint actual => (Int512)actual,
				ulong actual => (Int512)actual,
				UInt128 actual => (Int512)actual,
				UInt256 actual => (Int512)actual,
				UInt512 actual => (actual > (UInt512)MaxValue) ? MaxValue : (Int512)actual,
				nuint actual => (Int512)actual,
				sbyte actual => actual,
				short actual => actual,
				int actual => actual,
				long actual => actual,
				Int128 actual => actual,
				Int256 actual => actual,
				Int512 actual => actual,
				nint actual => actual,
				BigInteger actual => (actual < (BigInteger)MinValue) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (Int512)actual,
				_ => BitHelper.DefaultConvert<Int512>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertFromTruncating<TOther>(TOther value, out Int512 result) => TryConvertFromTruncating(value, out result);
		private static bool TryConvertFromTruncating<TOther>(TOther value, out Int512 result)
			where TOther : INumberBase<TOther>
		{
			const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
				float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
				double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
				Quad actual => (actual <= (new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MinValue : (actual > (new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MaxValue : (Int512)actual,
				decimal actual => (Int512)actual,
				byte actual => (Int512)actual,
				ushort actual => (Int512)actual,
				uint actual => (Int512)actual,
				ulong actual => (Int512)actual,
				UInt128 actual => (Int512)actual,
				UInt256 actual => (Int512)actual,
				UInt512 actual => (actual > (UInt512)MaxValue) ? MaxValue : (Int512)actual,
				nuint actual => (Int512)actual,
				sbyte actual => actual,
				short actual => actual,
				int actual => actual,
				long actual => actual,
				Int128 actual => actual,
				Int256 actual => actual,
				Int512 actual => actual,
				nint actual => actual,
				BigInteger actual => (Int512)actual,
				_ => BitHelper.DefaultConvert<Int512>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToChecked<TOther>(Int512 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;
			checked
			{
				result = result switch
				{
					char => (TOther)(object)(char)value,
					Half => (TOther)(object)(Half)value,
					float => (TOther)(object)(float)value,
					double => (TOther)(object)(double)value,
					Quad => (TOther)(object)(Quad)value,
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
					Int512 => (TOther)(object)value,
					nint => (TOther)(object)(nint)value,
					BigInteger => (TOther)(object)(BigInteger)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToSaturating<TOther>(Int512 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;

			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				Quad => (TOther)(object)(Quad)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)((value >= (Int512)byte.MaxValue) ? byte.MaxValue : (value <= (Int512)byte.MinValue) ? byte.MinValue : (byte)value),
				ushort => (TOther)(object)((value >= (Int512)ushort.MaxValue) ? ushort.MaxValue : (value <= (Int512)ushort.MinValue) ? ushort.MinValue : (ushort)value),
				uint => (TOther)(object)((value >= (Int512)uint.MaxValue) ? uint.MaxValue : (value <= (Int512)uint.MinValue) ? uint.MinValue : (uint)value),
				ulong => (TOther)(object)((value >= (Int512)ulong.MaxValue) ? ulong.MaxValue : (value <= (Int512)ulong.MinValue) ? ulong.MinValue : (ulong)value),
				UInt128 => (TOther)(object)((value >= (Int512)UInt128.MaxValue) ? UInt128.MaxValue : (value <= (Int512)UInt128.MinValue) ? UInt128.MinValue : (UInt128)value),
				UInt256 => (TOther)(object)((value >= (Int512)UInt256.MaxValue) ? UInt256.MaxValue : (value <= (Int512)UInt256.MinValue) ? UInt256.MinValue : (UInt256)value),
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)((value >= (Int512)nuint.MaxValue) ? nuint.MaxValue : (value <= (Int512)nuint.MinValue) ? nuint.MinValue : (nuint)value),
				sbyte => (TOther)(object)((value >= (Int512)sbyte.MaxValue) ? sbyte.MaxValue : (value <= (Int512)sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
				short => (TOther)(object)((value >= (Int512)short.MaxValue) ? short.MaxValue : (value <= (Int512)short.MinValue) ? short.MinValue : (short)value),
				int => (TOther)(object)((value >= (Int512)int.MaxValue) ? int.MaxValue : (value <= (Int512)int.MinValue) ? int.MinValue : (int)value),
				long => (TOther)(object)((value >= (Int512)long.MaxValue) ? long.MaxValue : (value <= (Int512)long.MinValue) ? long.MinValue : (long)value),
				Int128 => (TOther)(object)((value >= (Int512)Int128.MaxValue) ? Int128.MaxValue : (value <= (Int512)Int128.MinValue) ? Int128.MinValue : (Int128)value),
				Int256 => (TOther)(object)((value >= (Int512)Int256.MaxValue) ? Int256.MaxValue : (value <= (Int512)Int256.MinValue) ? Int128.MinValue : (Int256)value),
				Int512 => (TOther)(object)value,
				nint => (TOther)(object)((value >= (Int512)nint.MaxValue) ? nint.MaxValue : (value <= (Int512)nint.MinValue) ? nint.MinValue : (nint)value),
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToTruncating<TOther>(Int512 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;
			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				Quad => (TOther)(object)(Quad)value,
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
				Int512 => (TOther)(object)value,
				nint => (TOther)(object)(nint)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		bool IBinaryInteger<Int512>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
		{
			if (TryWriteBigEndian(destination))
			{
				bytesWritten = Size;
				return true;
			}
			bytesWritten = 0;
			return false;
		}

		internal bool TryWriteBigEndian(Span<byte> destination)
		{
			if (destination.Length >= Size)
			{
				WriteBigEndianUnsafe(destination);
				return true;
			}
			else
			{
				return false;
			}
		}

		private void WriteBigEndianUnsafe(Span<byte> destination)
		{
			ulong p0 = _p0;
			ulong p1 = _p1;
			ulong p2 = _p2;
			ulong p3 = _p3;
			ulong p4 = _p4;
			ulong p5 = _p5;
			ulong p6 = _p6;
			ulong p7 = _p7;

			if (BitConverter.IsLittleEndian)
			{
				p0 = BinaryPrimitives.ReverseEndianness(p0);
				p1 = BinaryPrimitives.ReverseEndianness(p1);
				p2 = BinaryPrimitives.ReverseEndianness(p2);
				p3 = BinaryPrimitives.ReverseEndianness(p3);
				p4 = BinaryPrimitives.ReverseEndianness(p4);
				p5 = BinaryPrimitives.ReverseEndianness(p5);
				p6 = BinaryPrimitives.ReverseEndianness(p6);
				p7 = BinaryPrimitives.ReverseEndianness(p7);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, p7);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong)), p6);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 2), p5);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 3), p4);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 4), p3);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 5), p2);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 6), p1);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 7), p0);
		}

		bool IBinaryInteger<Int512>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (TryWriteLittleEndian(destination))
			{
				bytesWritten = Size;
				return true;
			}
			bytesWritten = 0;
			return false;
		}

		internal bool TryWriteLittleEndian(Span<byte> destination)
		{
			if (destination.Length >= Size)
			{
				WriteLittleEndianUnsafe(destination);
				return true;
			}
			else
			{
				return false;
			}
		}

		private void WriteLittleEndianUnsafe(Span<byte> destination)
		{
			Debug.Assert(destination.Length >= Size);

			ulong p0 = _p0;
			ulong p1 = _p1;
			ulong p2 = _p2;
			ulong p3 = _p3;
			ulong p4 = _p4;
			ulong p5 = _p5;
			ulong p6 = _p6;
			ulong p7 = _p7;

			if (!BitConverter.IsLittleEndian)
			{
				p0 = BinaryPrimitives.ReverseEndianness(p0);
				p1 = BinaryPrimitives.ReverseEndianness(p1);
				p2 = BinaryPrimitives.ReverseEndianness(p2);
				p3 = BinaryPrimitives.ReverseEndianness(p3);
				p4 = BinaryPrimitives.ReverseEndianness(p4);
				p5 = BinaryPrimitives.ReverseEndianness(p5);
				p6 = BinaryPrimitives.ReverseEndianness(p6);
				p7 = BinaryPrimitives.ReverseEndianness(p7);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, p0);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong)), p1);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 2), p2);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 3), p3);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 4), p4);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 5), p5);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 6), p6);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 7), p7);
		}

		static int IFormattableInteger<Int512>.UnsignedCompare(in Int512 value1, in Int512 value2) => unchecked(((UInt512)value1).CompareTo((UInt512)value2));
		static int IFormattableInteger<Int512>.Log2Int32(in Int512 value) => BitHelper.Log2(in value);
		static int IFormattableInteger<Int512>.LeadingZeroCountInt32(in Int512 value) => BitHelper.LeadingZeroCount(in value);

		/// <inheritdoc/>
		public static Int512 operator +(in Int512 value) => value;

		/// <inheritdoc/>
		public static Int512 operator +(in Int512 left, in Int512 right)
		{
			// For unsigned addition, we can detect overflow by checking `(x + y) < x`
			// This gives us the carry to add to upper to compute the correct result

			ulong part0 = left._p0 + right._p0;
			ulong carry = (part0 < left._p0) ? 1UL : 0UL;

			ulong part1 = left._p1 + right._p1 + carry;
			carry = (part1 < left._p1 || (carry == 1 && part1 == left._p1)) ? 1UL : 0UL;

			ulong part2 = left._p2 + right._p2 + carry;
			carry = (part2 < left._p2 || (carry == 1 && part2 == left._p2)) ? 1UL : 0UL;

			ulong part3 = left._p3 + right._p3 + carry;
			carry = (part3 < left._p3 || (carry == 1 && part3 == left._p3)) ? 1UL : 0UL;

			ulong part4 = left._p4 + right._p4 + carry;
			carry = (part4 < left._p4 || (carry == 1 && part4 == left._p4)) ? 1UL : 0UL;

			ulong part5 = left._p5 + right._p5 + carry;
			carry = (part5 < left._p5 || (carry == 1 && part5 == left._p5)) ? 1UL : 0UL;

			ulong part6 = left._p6 + right._p6 + carry;
			carry = (part6 < left._p6 || (carry == 1 && part6 == left._p6)) ? 1UL : 0UL;

			ulong part7 = left._p7 + right._p7 + carry;
			return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static Int512 operator checked +(in Int512 left, in Int512 right)
		{
			Int512 result = left + right;

			uint sign = (uint)(left._p7 >> 63);

			if (sign == (uint)(right._p7 >> 63) &&
				sign != (uint)(result._p7 >> 63))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Addition);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Int512 operator -(in Int512 value) => Zero - value;
		/// <inheritdoc/>
		public static Int512 operator checked -(in Int512 value) => checked(Zero - value);

		/// <inheritdoc/>
		public static Int512 operator -(in Int512 left, in Int512 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			ulong part0 = left._p0 - right._p0;
			ulong borrow = (part0 > left._p0) ? 1UL : 0UL;

			ulong part1 = left._p1 - right._p1 - borrow;
			borrow = (part1 > left._p1 || (borrow == 1UL && part1 == left._p1)) ? 1UL : 0UL;

			ulong part2 = left._p2 - right._p2 - borrow;
			borrow = (part2 > left._p2 || (borrow == 1UL && part2 == left._p2)) ? 1UL : 0UL;

			ulong part3 = left._p3 - right._p3 - borrow;
			borrow = (part3 > left._p3 || (borrow == 1UL && part3 == left._p3)) ? 1UL : 0UL;

			ulong part4 = left._p4 - right._p4 - borrow;
			borrow = (part4 > left._p4 || (borrow == 1UL && part4 == left._p4)) ? 1UL : 0UL;

			ulong part5 = left._p5 - right._p5 - borrow;
			borrow = (part5 > left._p5 || (borrow == 1UL && part5 == left._p5)) ? 1UL : 0UL;

			ulong part6 = left._p6 - right._p6 - borrow;
			borrow = (part6 > left._p6 || (borrow == 1UL && part6 == left._p6)) ? 1UL : 0UL;

			ulong part7 = left._p7 - right._p7 - borrow;

			return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static Int512 operator checked -(in Int512 left, in Int512 right)
		{
			// For signed subtraction, we can detect overflow by checking if the sign of
			// both inputs are different and then if that differs from the sign of the
			// output.

			Int512 result = left - right;

			uint sign = (uint)(left._p7 >> 63);

			if (sign != (uint)(right._p7 >> 63) && sign != (uint)(result._p7 >> 63))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Subtraction);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Int512 operator ~(in Int512 value)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v = Unsafe.BitCast<Int512, Vector512<ulong>>(value);
				var result = ~v;
				return Unsafe.BitCast<Vector512<ulong>, Int512>(result);
			}
			else
			{
				return new(~value._p7, ~value._p6, ~value._p5, ~value._p4, ~value._p3, ~value._p2, ~value._p1, ~value._p0);
			}
		}

		/// <inheritdoc/>
		public static Int512 operator ++(in Int512 value) => value + One;
		/// <inheritdoc/>
		public static Int512 operator checked ++(in Int512 value) => checked(value + One);

		/// <inheritdoc/>
		public static Int512 operator --(in Int512 value) => value - One;
		/// <inheritdoc/>
		public static Int512 operator checked --(in Int512 value) => checked(value - One);

		/// <inheritdoc/>
		public static Int512 operator *(in Int512 left, in Int512 right) => (Int512)((UInt512)left * (UInt512)right);
		/// <inheritdoc/>
		public static Int512 operator checked *(in Int512 left, in Int512 right)
		{
			Int512 upper = BigMul(left, right, out Int512 lower);

			if (((upper != 0) || (lower < 0)) && ((~upper != 0) || (lower >= 0)))
			{
				// The upper bits can safely be either Zero or AllBitsSet
				// where the former represents a positive value and the
				// latter a negative value.
				//
				// However, when the upper bits are Zero, we also need to
				// confirm the lower bits are positive, otherwise we have
				// a positive value greater than MaxValue and should throw
				//
				// Likewise, when the upper bits are AllBitsSet, we also
				// need to confirm the lower bits are negative, otherwise
				// we have a large negative value less than MinValue and
				// should throw.

				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
			}

			return lower;
		}

		/// <inheritdoc/>
		public static Int512 operator /(in Int512 left, in Int512 right)
		{
			if ((right == NegativeOne) && (left.Upper == _upperMin) && (left.Lower == _lowerMin))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Division);
			}

			// We simplify the logic here by just doing unsigned division on the
			// two's complement representation and then taking the correct sign.

			ulong sign = (left._p7 ^ right._p7) & (1UL << 63);

			Int512 a = left, b = right;

			if ((long)left._p7 < 0)
			{
				a = ~left + One;
			}

			if ((long)right._p7 < 0)
			{
				b = ~right + One;
			}

			UInt512 result = (UInt512)(a) / (UInt512)(b);

			if (sign != 0)
			{
				result = ~result + UInt512.One;
			}

			return unchecked((Int512)result);
		}

		/// <inheritdoc/>
		public static Int512 operator %(in Int512 left, in Int512 right)
		{
			Int512 quotient = left / right;
			return left - (quotient * right);
		}

		/// <inheritdoc/>
		public static Int512 operator &(in Int512 left, in Int512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<Int512, Vector512<ulong>>(right);
				var result = v1 & v2;
				return Unsafe.BitCast<Vector512<ulong>, Int512>(result);
			}
			else
			{
				return new(left._p7 & right._p7, left._p6 & right._p6, left._p5 & right._p5, left._p4 & right._p4, left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int512 operator |(in Int512 left, in Int512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<Int512, Vector512<ulong>>(right);
				var result = v1 | v2;
				return Unsafe.BitCast<Vector512<ulong>, Int512>(result);
			}
			else
			{
				return new(left._p7 | right._p7, left._p6 | right._p6, left._p5 | right._p5, left._p4 | right._p4, left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int512 operator ^(in Int512 left, in Int512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<Int512, Vector512<ulong>>(right);
				var result = v1 ^ v2;
				return Unsafe.BitCast<Vector512<ulong>, Int512>(result);
			}
			else
			{
				return new(left._p7 ^ right._p7, left._p6 ^ right._p6, left._p5 ^ right._p5, left._p4 ^ right._p4, left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int512 operator <<(in Int512 value, int shiftAmount)
		{
			// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
			// need to specially handle things if the shift amount exceeds 0x3F.

			shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 255]

			if (shiftAmount == 0)
			{
				return value;
			}

			if (shiftAmount < 64)
			{
				ulong part7 = (value._p7 << shiftAmount) | (value._p6 >> (64 - shiftAmount));
				ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
				ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int512(value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0);
				}

				ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
				ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part6, part5, part4, part3, part2, part1, part0, 0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int512(value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0);
				}

				ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part5, part4, part3, part2, part1, part0, 0, 0);
			}
			else if (shiftAmount < 256)
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new Int512(value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0, 0);
				}

				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part4, part3, part2, part1, part0, 0, 0, 0);
			}
			else if (shiftAmount < 320)
			{
				shiftAmount -= 256;

				if (shiftAmount == 0)
				{
					return new Int512(value._p3, value._p2, value._p1, value._p0, 0, 0, 0, 0);
				}

				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part3, part2, part1, part0, 0, 0, 0, 0);
			}
			else if (shiftAmount < 384)
			{
				shiftAmount -= 320;

				if (shiftAmount == 0)
				{
					return new Int512(value._p2, value._p1, value._p0, 0, 0, 0, 0, 0);
				}

				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part2, part1, part0, 0, 0, 0, 0, 0);
			}
			else if (shiftAmount < 448)
			{
				shiftAmount -= 384;

				if (shiftAmount == 0)
				{
					return new Int512(value._p1, value._p0, 0, 0, 0, 0, 0, 0);
				}

				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part1, part0, 0, 0, 0, 0, 0, 0);
			}
			else // shiftAmount < 512
			{
				shiftAmount -= 448;

				if (shiftAmount == 0)
				{
					return new Int512(value._p0, 0, 0, 0, 0, 0, 0, 0);
				}

				ulong part0 = value._p0 << shiftAmount;

				return new Int512(part0, 0, 0, 0, 0, 0, 0, 0);
			}
		}

		/// <inheritdoc/>
		public static Int512 operator >>(in Int512 value, int shiftAmount)
		{
			// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
			// need to specially handle things if the shift amount exceeds 0x3F.

			shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 511]

			if (shiftAmount == 0)
			{
				return value;
			}

			if (shiftAmount < 64)
			{
				ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
				ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part3 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part4 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part5 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part6 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part7 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
			}

			ulong preservedSign = (ulong)((long)value._p7 >> 63);

			if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part6 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part5 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 256)
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4, value._p3);
				}

				ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part4 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, preservedSign, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 320)
			{
				shiftAmount -= 256;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5, value._p4);
				}

				ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part3 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, part3, part2, part1, part0);
			}
			else if (shiftAmount < 384)
			{
				shiftAmount -= 320;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6, value._p5);
				}

				ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part2 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part2, part1, part0);
			}
			else if (shiftAmount < 448)
			{
				shiftAmount -= 384;

				if (shiftAmount == 0)
				{
					return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, value._p7, value._p6);
				}

				ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part1 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part1, part0);
			}
			else // shiftAmount < 512
			{
				shiftAmount -= 448;

				ulong part0 = (ulong)((long)value._p7 >> shiftAmount);

				return new Int512(preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, preservedSign, part0);
			}
		}

		/// <inheritdoc/>
		public static bool operator ==(in Int512 left, in Int512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<Int512, Vector512<ulong>>(right);
				return v1 == v2;
			}
			else
			{
				return (left._p7 == right._p7) && (left._p6 == right._p6) && (left._p5 == right._p5) && (left._p4 == right._p4)
					&& (left._p3 == right._p3) && (left._p2 == right._p2) && (left._p1 == right._p1) && (left._p0 == right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator !=(in Int512 left, in Int512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<Int512, Vector512<ulong>>(right);
				return v1 != v2;
			}
			else
			{
				return (left._p7 != right._p7) || (left._p6 != right._p6) || (left._p5 != right._p5) || (left._p4 != right._p4)
					|| (left._p3 != right._p3) || (left._p2 != right._p2) || (left._p1 != right._p1) || (left._p0 != right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator <(in Int512 left, in Int512 right)
		{
			// Successively compare each part.
			// If left and right have different signs: Signed comparison of _p7 gives result since it is stored as two's complement
			// If signs are equal and left._p7 < right._p7: left < right for negative and positive values,
			//                                                    since _p7 is upper 64 bits in two's complement.
			// If signs are equal and left._p7 > right._p7: left > right for negative and positive values,
			//                                                    since _p7 is upper 64 bits in two's complement.
			// If left._p7 == right._p7: unsigned comparison of lower bits gives the result for both negative and positive values since
			//                                 lower values are lower 64 bits in two's complement.
			return ((long)left._p7 < (long)right._p7)
				|| (left._p7 == right._p7 && ((left._p6 < right._p6)
				|| (left._p6 == right._p6 && ((left._p5 < right._p5)
				|| (left._p5 == right._p5 && ((left._p4 < right._p4)
				|| (left._p4 == right._p4 && ((left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 < right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator >(in Int512 left, in Int512 right)
		{
			return ((long)left._p7 > (long)right._p7)
				|| (left._p7 == right._p7 && ((left._p6 > right._p6)
				|| (left._p6 == right._p6 && ((left._p5 > right._p5)
				|| (left._p5 == right._p5 && ((left._p4 > right._p4)
				|| (left._p4 == right._p4 && ((left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 > right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator <=(in Int512 left, in Int512 right)
		{
			return ((long)left._p7 < (long)right._p7)
				|| (left._p7 == right._p7 && ((left._p6 < right._p6)
				|| (left._p6 == right._p6 && ((left._p5 < right._p5)
				|| (left._p5 == right._p5 && ((left._p4 < right._p4)
				|| (left._p4 == right._p4 && ((left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 <= right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator >=(in Int512 left, in Int512 right)
		{
			return ((long)left._p7 > (long)right._p7)
				|| (left._p7 == right._p7 && ((left._p6 > right._p6)
				|| (left._p6 == right._p6 && ((left._p5 > right._p5)
				|| (left._p5 == right._p5 && ((left._p4 > right._p4)
				|| (left._p4 == right._p4 && ((left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 >= right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static Int512 operator >>>(in Int512 value, int shiftAmount)
		{
			// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
			// need to specially handle things if the shift amount exceeds 0x3F.

			shiftAmount &= 0x1FF; // mask the shift amount to be within [0, 511]

			if (shiftAmount == 0)
			{
				return value;
			}

			if (shiftAmount < 64)
			{
				ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
				ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part3 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part4 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part5 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part6 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part7 = value._p7 >> shiftAmount;

				return new Int512(part7, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int512(0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part6 = value._p7 >> shiftAmount;

				return new Int512(0, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int512(0, 0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part5 = value._p7 >> shiftAmount;

				return new Int512(0, 0, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 256)
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new Int512(0, 0, 0, value._p7, value._p6, value._p5, value._p4, value._p3);
				}

				ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part4 = value._p7 >> shiftAmount;

				return new Int512(0, 0, 0, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 320)
			{
				shiftAmount -= 256;

				if (shiftAmount == 0)
				{
					return new Int512(0, 0, 0, 0, value._p7, value._p6, value._p5, value._p4);
				}

				ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part3 = value._p7 >> shiftAmount;

				return new Int512(0, 0, 0, 0, part3, part2, part1, part0);
			}
			else if (shiftAmount < 384)
			{
				shiftAmount -= 320;

				if (shiftAmount == 0)
				{
					return new Int512(0, 0, 0, 0, 0, value._p7, value._p6, value._p5);
				}

				ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part2 = value._p7 >> shiftAmount;

				return new Int512(0, 0, 0, 0, 0, part2, part1, part0);
			}
			else if (shiftAmount < 448)
			{
				shiftAmount -= 384;

				if (shiftAmount == 0)
				{
					return new Int512(0, 0, 0, 0, 0, 0, value._p7, value._p6);
				}

				ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part1 = value._p7 >> shiftAmount;

				return new Int512(0, 0, 0, 0, 0, 0, part1, part0);
			}
			else // shiftAmount < 512
			{
				shiftAmount -= 448;

				ulong part0 = value._p7 >> shiftAmount;

				return new Int512(0, 0, 0, 0, 0, 0, 0, part0);
			}
		}
	}
}
