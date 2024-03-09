using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public partial struct UInt512 :
		IBinaryInteger<UInt512>,
		IMinMaxValue<UInt512>,
		IUnsignedNumber<UInt512>,
		IFormattableUnsignedInteger<UInt512, Int512>
	{
		public static UInt512 One => new UInt512(0, 0, 0, 0, 0, 0, 0, 1);

		static int INumberBase<UInt512>.Radix => 2;

		public static UInt512 Zero => default;

		public static UInt512 AdditiveIdentity => default;

		public static UInt512 MultiplicativeIdentity => One;

		public static UInt512 MaxValue => new UInt512(
			   0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
			   0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static UInt512 MinValue => default;

		static UInt512 IFormattableUnsignedInteger<UInt512, Int512>.SignedMaxMagnitude => new UInt512(
			   0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			   0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		static UInt512 IFormattableInteger<UInt512>.Two => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x2);

		static UInt512 IFormattableInteger<UInt512>.Sixteen => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x10);

		static UInt512 IFormattableInteger<UInt512>.Ten => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xA);

		static UInt512 IFormattableInteger<UInt512>.TwoPow2 => new UInt512(0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4);

		static UInt512 IFormattableInteger<UInt512>.SixteenPow2 => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x100);

		static UInt512 IFormattableInteger<UInt512>.TenPow2 => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x64);

		static UInt512 IFormattableInteger<UInt512>.TwoPow3 => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x8);

		static UInt512 IFormattableInteger<UInt512>.SixteenPow3 => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x1000);

		static UInt512 IFormattableInteger<UInt512>.TenPow3 => new UInt512(0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x3E8);

		static char IFormattableInteger<UInt512>.LastDecimalDigitOfMaxValue => '1';

		static int IFormattableInteger<UInt512>.MaxDecimalDigits => 155;

		static int IFormattableInteger<UInt512>.MaxHexDigits => 128;

		static int IFormattableInteger<UInt512>.MaxBinaryDigits => 512;

		static UInt512 INumberBase<UInt512>.Abs(UInt512 value) => value;

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

		public int CompareTo(UInt512 other)
		{
			if (this < other) return -1;
			else if (this > other) return 1;
			else return 0;
		}

		public static UInt512 CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt512 result;

			if (value is UInt512 v)
			{
				result = v;
			}
			else if (!UInt512.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<UInt512>(value, out result))
			{
				Thrower.NotSupported<UInt512, TOther>();
			}

			return result;
		}

		public static UInt512 CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt512 result;

			if (value is UInt512 v)
			{
				result = v;
			}
			else if (!UInt512.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<UInt512>(value, out result))
			{
				Thrower.NotSupported<UInt512, TOther>();
			}

			return result;
		}

		public static UInt512 CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt512 result;

			if (value is UInt512 v)
			{
				result = v;
			}
			else if (!UInt512.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<UInt512>(value, out result))
			{
				Thrower.NotSupported<UInt512, TOther>();
			}

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (UInt512 Quotient, UInt512 Remainder) DivRem(UInt512 left, UInt512 right)
		{
			UInt512 quotient = left / right;
			return (quotient, (left - (quotient * right)));
		}

		public bool Equals(UInt512 other) => this == other;

		public int GetByteCount()
		{
			return Size;
		}

		public int GetShortestBitLength()
		{
			UInt512 value = this;
			return (Size * 8) - BitHelper.LeadingZeroCount(value);
		}

		static bool INumberBase<UInt512>.IsCanonical(UInt512 value)
		{
			return true;
		}

		static bool INumberBase<UInt512>.IsComplexNumber(UInt512 value)
		{
			return false;
		}

		public static bool IsEvenInteger(UInt512 value)
		{
			return (value._lower & UInt256.One) == UInt256.Zero;
		}

		static bool INumberBase<UInt512>.IsFinite(UInt512 value)
		{
			return true;
		}

		static bool INumberBase<UInt512>.IsImaginaryNumber(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsInfinity(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsInteger(UInt512 value)
		{
			return true;
		}

		static bool INumberBase<UInt512>.IsNaN(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsNegative(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsNegativeInfinity(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsNormal(UInt512 value)
		{
			return value != Zero;
		}

		public static bool IsOddInteger(UInt512 value)
		{
			return (value._lower & UInt256.One) != UInt256.Zero;
		}

		static bool INumberBase<UInt512>.IsPositive(UInt512 value)
		{
			return true;
		}

		static bool INumberBase<UInt512>.IsPositiveInfinity(UInt512 value)
		{
			return false;
		}

		public static bool IsPow2(UInt512 value)
		{
			return PopCount(value) == 1;
		}

		static bool INumberBase<UInt512>.IsRealNumber(UInt512 value)
		{
			return true;
		}

		static bool INumberBase<UInt512>.IsSubnormal(UInt512 value)
		{
			return false;
		}

		static bool INumberBase<UInt512>.IsZero(UInt512 value)
		{
			return value == Zero;
		}

		public static UInt512 LeadingZeroCount(UInt512 value)
		{
			if (value._upper == UInt256.Zero)
			{
				return 256 + UInt256.LeadingZeroCount(value._lower);
			}

			return UInt256.LeadingZeroCount(value._upper);
		}

		public static UInt512 Log2(UInt512 value)
		{
			if (value._upper == 0)
			{
				return UInt256.Log2(value._lower);
			}
			return 256 + UInt256.Log2(value._upper);
		}

		public static UInt512 Max(UInt512 x, UInt512 y) => (x >= y) ? x : y;

		static UInt512 INumber<UInt512>.MaxNumber(UInt512 x, UInt512 y) => Max(x, y);

		public static UInt512 MaxMagnitude(UInt512 x, UInt512 y) => Max(x, y);

		public static UInt512 MaxMagnitudeNumber(UInt512 x, UInt512 y) => Max(x, y);

		public static UInt512 Min(UInt512 x, UInt512 y) => (x <= y) ? x : y;

		static UInt512 INumber<UInt512>.MinNumber(UInt512 x, UInt512 y) => Min(x, y);

		public static UInt512 MinMagnitude(UInt512 x, UInt512 y) => Min(x, y);

		public static UInt512 MinMagnitudeNumber(UInt512 x, UInt512 y) => Min(x, y);

		public static UInt512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		public static UInt512 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		public static UInt512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		public static UInt512 Parse(string s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

#if NET8_0_OR_GREATER
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(utf8Text);
			}
			return output;
		}
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(utf8Text);
			}
			return output;
		}
#endif

		public static UInt512 PopCount(UInt512 value)
		{
			return UInt256.PopCount(value._lower) + UInt256.PopCount(value._upper);
		}

		public static UInt512 RotateLeft(UInt512 value, int rotateAmount)
		{
			return (value << rotateAmount) | (value >>> (512 - rotateAmount));
		}

		public static UInt512 RotateRight(UInt512 value, int rotateAmount)
		{
			return (value >>> rotateAmount) | (value << (512 - rotateAmount));
		}

		public static UInt512 TrailingZeroCount(UInt512 value)
		{
			if (value._lower == 0)
			{
				return 256 + UInt256.TrailingZeroCount(value._upper);
			}
			return UInt256.TrailingZeroCount(value._lower);
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

#if NET8_0_OR_GREATER
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt512 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}
#endif

		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					sourceRef = ref Unsafe.Add(ref sourceRef, source.Length - Size);

					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

					if (BitConverter.IsLittleEndian)
					{
						result = BitHelper.ReverseEndianness(result);
					}
				}
				else
				{
					// We have between 1 and 31 bytes, so construct the relevant value directly
					// since the data is in Big Endian format, we can just read the bytes and
					// shift left by 8-bits for each subsequent part

					for (int i = 0; i < source.Length; i++)
					{
						result <<= 8;
						result |= Unsafe.Add(ref sourceRef, i);
					}
				}
			}

			value = result;
			return true;
		}

		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt512 value)
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					// We have at least 64 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

					if (!BitConverter.IsLittleEndian)
					{
						result = BitHelper.ReverseEndianness(result);
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
						UInt512 part = Unsafe.Add(ref sourceRef, i);
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

		public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FormatUnsignedInteger<UInt512, Int512>(in this, format, formatProvider);
		}

		static bool INumberBase<UInt512>.TryConvertFromChecked<TOther>(TOther value, out UInt512 result) => TryConvertFromChecked(value, out result);
		private static bool TryConvertFromChecked<TOther>(TOther value, out UInt512 result)
		{
			bool converted = true;
			checked
			{
				result = value switch
				{
					char actual => (UInt512)actual,
					Half actual => (UInt512)actual,
					float actual => (UInt512)actual,
					double actual => (UInt512)actual,
					decimal actual => (UInt512)actual,
					byte actual => (UInt512)actual,
					ushort actual => (UInt512)actual,
					uint actual => (UInt512)actual,
					ulong actual => (UInt512)actual,
					UInt128 actual => (UInt512)actual,
					UInt256 actual => (UInt512)actual,
					UInt512 actual => actual,
					nuint actual => (UInt512)actual,
					sbyte actual => (UInt512)actual,
					short actual => (UInt512)actual,
					int actual => (UInt512)actual,
					long actual => (UInt512)actual,
					Int128 actual => (UInt512)actual,
					Int256 actual => (UInt512)actual,
					Int512 actual => (UInt512)actual,
					_ => BitHelper.DefaultConvert<UInt512>(out converted)
				};
			}
			return converted;
		}

		static bool INumberBase<UInt512>.TryConvertFromSaturating<TOther>(TOther value, out UInt512 result) => TryConvertFromSaturating(value, out result);
		private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt512 result)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (actual < Half.Zero) ? MinValue : (UInt512)actual,
				float actual => (actual < 0) ? MinValue : (UInt512)actual,
				double actual => (actual < 0) ? MinValue : (actual > TwoPow512) ? MaxValue : (UInt512)actual,
				decimal actual => (actual < 0) ? MinValue : (UInt512)actual,
				byte actual => actual,
				ushort actual => actual,
				uint actual => actual,
				ulong actual => actual,
				UInt128 actual => actual,
				UInt256 actual => actual,
				UInt512 actual => actual,
				nuint actual => actual,
				sbyte actual => (actual < 0) ? MinValue : (UInt512)actual,
				short actual => (actual < 0) ? MinValue : (UInt512)actual,
				int actual => (actual < 0) ? MinValue : (UInt512)actual,
				long actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int128 actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int256 actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int512 actual => (actual < 0) ? MinValue : (UInt512)actual,
				_ => BitHelper.DefaultConvert<UInt512>(out converted)
			};
			return converted;
		}

		static bool INumberBase<UInt512>.TryConvertFromTruncating<TOther>(TOther value, out UInt512 result) => TryConvertFromTruncating(value, out result);
		private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt512 result)
		{
			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (actual < Half.Zero) ? MinValue : (UInt512)actual,
				float actual => (actual < 0) ? MinValue : (UInt512)actual,
				double actual => (actual < 0) ? MinValue : (UInt512)actual,
				decimal actual => (actual < 0) ? MinValue : (UInt512)actual,
				byte actual => actual,
				ushort actual => actual,
				uint actual => actual,
				ulong actual => actual,
				UInt128 actual => actual,
				UInt256 actual => actual,
				UInt512 actual => actual,
				nuint actual => actual,
				sbyte actual => (actual < 0) ? MinValue : (UInt512)actual,
				short actual => (actual < 0) ? MinValue : (UInt512)actual,
				int actual => (actual < 0) ? MinValue : (UInt512)actual,
				long actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int128 actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int256 actual => (actual < 0) ? MinValue : (UInt512)actual,
				Int512 actual => (actual < 0) ? MinValue : (UInt512)actual,
				_ => BitHelper.DefaultConvert<UInt512>(out converted)
			};
			return converted;
		}

		static bool INumberBase<UInt512>.TryConvertToChecked<TOther>(UInt512 value, out TOther result)
		{
			bool converted = true;
			result = default;
			checked
			{
				result = result switch
				{
					char => (TOther)(object)(char)value,
					Half => (TOther)(object)(Half)value,
					float => (TOther)(object)(float)value,
					double => (TOther)(object)(double)value,
					decimal => (TOther)(object)(decimal)value,
					byte => (TOther)(object)(byte)value,
					ushort => (TOther)(object)(ushort)value,
					uint => (TOther)(object)(uint)value,
					ulong => (TOther)(object)(ulong)value,
					UInt128 => (TOther)(object)(UInt128)value,
					UInt256 => (TOther)(object)(UInt256)value,
					UInt512 => (TOther)(object)value,
					nuint => (TOther)(object)(nuint)value,
					sbyte => (TOther)(object)(sbyte)value,
					short => (TOther)(object)(short)value,
					int => (TOther)(object)(int)value,
					long => (TOther)(object)(long)value,
					Int128 => (TOther)(object)(Int128)value,
					Int256 => (TOther)(object)(Int256)value,
					Int512 => (TOther)(object)(Int512)value,
					nint => (TOther)(object)(nint)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<UInt512>.TryConvertToSaturating<TOther>(UInt512 value, out TOther result)
		{
			bool converted = true;
			result = default;
			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)(byte)value,
				ushort => (TOther)(object)(ushort)value,
				uint => (TOther)(object)(uint)value,
				ulong => (TOther)(object)(ulong)value,
				UInt128 => (TOther)(object)(UInt128)value,
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)value,
#if TARGET_32BIT
				nuint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? nuint.MaxValue : (nuint)value),
#else
				nuint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? nuint.MaxValue : (nuint)value),
#endif
				sbyte => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_007F)) ? sbyte.MaxValue : (sbyte)value),
				short => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_7FFF)) ? short.MaxValue : (short)value),
				int => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? int.MaxValue : (int)value),
				long => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? long.MaxValue : (long)value),
				Int128 => (TOther)(object)((value >= new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int128.MaxValue : (Int128)value),
				Int256 => (TOther)(object)((value >= new UInt256(new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF))) ? Int256.MaxValue : (Int256)value),
				Int512 => (TOther)(object)((value >= new UInt512(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int512.MaxValue : (Int512)value),
#if TARGET_32BIT
				nint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? nint.MaxValue : (nint)value),
#else
				nint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? nint.MaxValue : (nint)value),
#endif
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<UInt512>.TryConvertToTruncating<TOther>(UInt512 value, out TOther result)
		{
			bool converted = true;
			result = default;
			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)(byte)value,
				ushort => (TOther)(object)(ushort)value,
				uint => (TOther)(object)(uint)value,
				ulong => (TOther)(object)(ulong)value,
				UInt128 => (TOther)(object)(UInt128)value,
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)value,
				nuint => (TOther)(object)(nuint)value,
				sbyte => (TOther)(object)(sbyte)value,
				short => (TOther)(object)(short)value,
				int => (TOther)(object)(int)value,
				long => (TOther)(object)(long)value,
				Int128 => (TOther)(object)(Int128)value,
				Int256 => (TOther)(object)(Int256)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)(nint)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
			return converted;
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUnsignedInteger<UInt512, Int512, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

#if NET8_0_OR_GREATER
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUnsignedInteger<UInt512, Int512, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
		}
#endif

		bool IBinaryInteger<UInt512>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
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
			UInt256 lower = _lower;
			UInt256 upper = _upper;

			if (BitConverter.IsLittleEndian)
			{
				lower = BitHelper.ReverseEndianness(lower);
				upper = BitHelper.ReverseEndianness(upper);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, upper);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, BitHelper.SizeOf<UInt256>()), lower);
		}

		bool IBinaryInteger<UInt512>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
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

			UInt256 lower = _lower;
			UInt256 upper = _upper;

			if (!BitConverter.IsLittleEndian)
			{
				lower = BitHelper.ReverseEndianness(lower);
				upper = BitHelper.ReverseEndianness(upper);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, lower);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, BitHelper.SizeOf<UInt256>()), upper);
		}

		char IFormattableInteger<UInt512>.ToChar()
		{
			return (char)this._lower.Lower;
		}

		int IFormattableInteger<UInt512>.ToInt32()
		{
			return (int)this._lower.Lower;
		}

		Int512 IFormattableUnsignedInteger<UInt512, Int512>.ToSigned()
		{
			return (Int512)this;
		}

		static int IFormattableUnsignedInteger<UInt512, Int512>.CountDigits(in UInt512 value)
		{
			if (value._upper == UInt256.Zero)
			{
				return UInt256.CountDigits(in value._lower);
			}
			// We have more than 1e77, so we have atleast 78 digits
			int digits = 78;

			if (value._upper > 0x8)
			{
				// value / 1e78
				var lower = (value / new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0008, 0xA2DB_F142_DFCC_7AB6, 0xE356_9326_C784_3372, 0xA9F4_D250_5E3A_4000, 0x0000_0000_0000_0000));
				digits += UInt256.CountDigits(in lower._lower);
			}
			else if ((value._upper == 0x59) && (value._lower >= new UInt256(0x5C97_6C9C_BDFC_CB24, 0xE161_BF83_CB2A_027A, 0xA390_3723_AE46_8000, 0x0000_0000_0000_0000)))
			{
				// We are greater than 1e78, but less than 1e79
				// so we have exactly 79 digits

				digits++;
			}

			return digits;
		}

		public static UInt512 operator +(UInt512 value)
		{
			return value;
		}

		public static UInt512 operator +(UInt512 left, UInt512 right)
		{
			UInt256 lower = left._lower + right._lower;
			UInt256 carry = (lower < left._lower) ? 1UL : 0UL;

			UInt256 upper = left._upper + right._upper + carry;
			return new UInt512(upper, lower);
		}

		public static UInt512 operator checked +(UInt512 left, UInt512 right)
		{
			UInt256 lower = left._lower + right._lower;
			UInt256 carry = (lower < left._lower) ? 1UL : 0UL;

			UInt256 upper = checked(left._upper + right._upper + carry);
			return new UInt512(upper, lower);
		}

		public static UInt512 operator -(UInt512 value)
		{
			return Zero - value;
		}
		
		public static UInt512 operator checked -(UInt512 value)
		{
			return checked(Zero - value);
		}

		public static UInt512 operator -(UInt512 left, UInt512 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			UInt256 lower = left._lower - right._lower;
			UInt256 borrow = (lower > left._lower) ? 1UL : 0UL;

			UInt256 upper = left._upper - right._upper - borrow;
			return new UInt512(upper, lower);
		}
		
		public static UInt512 operator checked -(UInt512 left, UInt512 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			UInt256 lower = left._lower - right._lower;
			UInt256 borrow = (lower > left._lower) ? 1UL : 0UL;

			UInt256 upper = checked(left._upper - right._upper - borrow);
			return new UInt512(upper, lower);
		}

		public static UInt512 operator ~(UInt512 value)
		{
			return new(~value._upper, ~value._lower);
		}

		public static UInt512 operator ++(UInt512 value)
		{
			return value + One;
		}
		
		public static UInt512 operator checked ++(UInt512 value)
		{
			return checked(value + One);
		}

		public static UInt512 operator --(UInt512 value)
		{
			return value - One;
		}
		
		public static UInt512 operator checked --(UInt512 value)
		{
			return checked(value - One);
		}

		public static UInt512 operator *(UInt512 left, UInt512 right)
		{
			UInt256 upper = UInt256.BigMul(left._lower, right._lower, out UInt256 lower);
			upper += (left._upper * right._lower) + (left._lower * right._upper);
			return new UInt512(upper, lower);
		}
		
		public static UInt512 operator checked *(UInt512 left, UInt512 right)
		{
			UInt512 upper = BigMul(left, right, out UInt512 lower);

			if (upper != UInt512.Zero)
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
			}

			return lower;
		}

		public static UInt512 operator /(UInt512 left, UInt512 right)
		{
			if ((right._lower == UInt256.Zero) && (right._upper == UInt256.Zero))
			{
				Thrower.DivideByZero();
			}
			if ((left._upper == UInt256.Zero) && (right._upper == UInt256.Zero))
			{
				return left._lower / right._lower;
			}
			if (right >= left)
			{
				return (right == left) ? One : Zero;
			}

			return DivideSlow(left, right);

			static ulong AddDivisor(Span<ulong> left, ReadOnlySpan<ulong> right)
			{
				UInt128 carry = UInt128.Zero;

				for (int i = 0; i < right.Length; i++)
				{
					ref ulong leftElement = ref left[i];
					UInt128 digit = (leftElement + carry) + right[i];

					leftElement = unchecked((ulong)digit);
					carry = digit >> 64;
				}

				return (ulong)carry;
			}

			static bool DivideGuessTooBig(UInt128 q, UInt128 valHi, ulong valLo, ulong divHi, ulong divLo)
			{
				UInt128 chkHi = divHi * q;
				UInt128 chkLo = divLo * q;

				chkHi += (chkLo >> 64);
				chkLo &= 0xFFFF_FFFF_FFFF_FFFF;

				if (chkHi < valHi)
					return false;
				if (chkHi > valHi)
					return true;

				if (chkLo < valLo)
					return false;
				if (chkLo > valLo)
					return true;

				return false;
			}

			unsafe static UInt512 DivideSlow(UInt512 quotient, UInt512 divisor)
			{
				const int UlongCount = 64 / sizeof(ulong);

				ulong* pLeft = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 0), (ulong)(quotient._lower >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 1), (ulong)(quotient._lower >> 64));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 2), (ulong)(quotient._lower >> 128));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 3), (ulong)(quotient._lower >> 192));

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 4), (ulong)(quotient._upper >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 5), (ulong)(quotient._upper >> 64));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 6), (ulong)(quotient._upper >> 128));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 7), (ulong)(quotient._upper >> 192));

				Span<ulong> left = new Span<ulong>(pLeft, (UlongCount) - (BitHelper.LeadingZeroCount(quotient) / 64));


				ulong* pRight = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 0), (ulong)(divisor._lower >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 1), (ulong)(divisor._lower >> 64));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 2), (ulong)(divisor._lower >> 128));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 3), (ulong)(divisor._lower >> 192));

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 4), (ulong)(divisor._upper >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 5), (ulong)(divisor._upper >> 64));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 6), (ulong)(divisor._upper >> 128));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 7), (ulong)(divisor._upper >> 192));

				Span<ulong> right = new Span<ulong>(pRight, (UlongCount) - (BitHelper.LeadingZeroCount(divisor) / 64));


				Span<ulong> rawBits = stackalloc ulong[UlongCount];
				rawBits.Clear();
				Span<ulong> bits = rawBits.Slice(0, left.Length - right.Length + 1);

				ulong divHi = right[right.Length - 1];
				ulong divLo = right.Length > 1 ? right[right.Length - 2] : 0;

				int shift = BitOperations.LeadingZeroCount(divHi);
				int backShift = 64 - shift;

				if (shift > 0)
				{
					ulong divNx = right.Length > 2 ? right[right.Length - 3] : 0;

					divHi = (divHi << shift) | (divLo >> backShift);
					divLo = (divLo << shift) | (divNx >> backShift);
				}

				// Then, we divide all of the bits as we would do it using
				// pen and paper: guessing the next digit, subtracting, ...
				for (int i = left.Length; i >= right.Length; i--)
				{
					int n = i - right.Length;
					ulong t = ((ulong)(i) < (ulong)(left.Length)) ? left[i] : 0;

					UInt128 valHi = ((UInt128)(t) << 64) | left[i - 1];
					ulong valLo = (i > 1) ? left[i - 2] : 0;

					// We shifted the divisor, we shift the dividend too
					if (shift > 0)
					{
						ulong valNx = i > 2 ? left[i - 3] : 0;

						valHi = (valHi << shift) | (valLo >> backShift);
						valLo = (valLo << shift) | (valNx >> backShift);
					}

					// First guess for the current digit of the quotient,
					// which naturally must have only 64 bits...
					UInt128 digit = valHi / divHi;

					if (digit > 0xFFFF_FFFF_FFFF_FFFF)
					{
						digit = 0xFFFF_FFFF_FFFF_FFFF;
					}

					// Our first guess may be a little bit to big
					while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo))
					{
						--digit;
					}

					if (digit > 0)
					{
						// Now it's time to subtract our current quotient
						ulong carry = SubtractDivisor(left.Slice(n), right, digit);

						if (carry != t)
						{
							Debug.Assert(carry == (t + 1));

							// Our guess was still exactly one too high
							carry = AddDivisor(left.Slice(n), right);

							--digit;
							Debug.Assert(carry == 1);
						}
					}

					// We have the digit!
					if ((uint)(n) < (uint)(bits.Length))
					{
						bits[n] = (ulong)(digit);
					}

					if ((uint)(i) < (uint)(left.Length))
					{
						left[i] = 0;
					}
				}

				return new UInt512(
					rawBits[7], rawBits[6], rawBits[5], rawBits[4],
					rawBits[3], rawBits[2], rawBits[1], rawBits[0]
					);
			}

			static ulong SubtractDivisor(Span<ulong> left, ReadOnlySpan<ulong> right, UInt128 q)
			{
				// Combines a subtract and a multiply operation, which is naturally
				// more efficient than multiplying and then subtracting...

				UInt128 carry = UInt128.Zero;

				for (int i = 0; i < right.Length; i++)
				{
					carry += right[i] * q;

					ulong digit = (ulong)(carry);
					carry >>= 64;

					ref ulong leftElement = ref left[i];

					if (leftElement < digit)
					{
						++carry;
					}
					leftElement -= digit;
				}

				return (ulong)(carry);
			}

			throw new NotImplementedException();
		}

		public static UInt512 operator %(UInt512 left, UInt512 right)
		{
			UInt512 quotient = left / right;
			return left - (quotient * right);
		}

		public static UInt512 operator &(UInt512 left, UInt512 right)
		{
			return new(left._upper & right._upper, left._lower & right._lower);
		}

		public static UInt512 operator |(UInt512 left, UInt512 right)
		{
			return new(left._upper | right._upper, left._lower | right._lower);
		}

		public static UInt512 operator ^(UInt512 left, UInt512 right)
		{
			return new(left._upper ^ right._upper, left._lower ^ right._lower);
		}

		public static UInt512 operator <<(UInt512 value, int shiftAmount)
		{
			shiftAmount &= 0x1FF;

			if ((shiftAmount & 0x100) != 0)
			{
				UInt256 upper = value._lower << shiftAmount;
				return new UInt512(upper, UInt256.Zero);
			}
			else if (shiftAmount != 0)
			{
				UInt256 lower = value._lower << shiftAmount;
				UInt256 upper = (value._upper << shiftAmount) | (value._lower >> (256 - shiftAmount));

				return new UInt512(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static UInt512 operator >>(UInt512 value, int shiftAmount)
		{
			return value >>> shiftAmount;
		}

		public static bool operator ==(UInt512 left, UInt512 right)
		{
			return (left._upper == right._upper) && (left._lower == right._lower);
		}

		public static bool operator !=(UInt512 left, UInt512 right)
		{
			return (left._upper != right._upper) || (left._lower != right._lower);
		}

		public static bool operator <(UInt512 left, UInt512 right)
		{
			return (left._upper < right._upper)
				|| (left._upper == right._upper) && (left._lower < right._lower);
		}

		public static bool operator >(UInt512 left, UInt512 right)
		{
			return (left._upper > right._upper)
				|| (left._upper == right._upper) && (left._lower > right._lower);
		}

		public static bool operator <=(UInt512 left, UInt512 right)
		{
			return (left._upper < right._upper)
			   || (left._upper == right._upper) && (left._lower <= right._lower);
		}

		public static bool operator >=(UInt512 left, UInt512 right)
		{
			return (left._upper > right._upper)
				|| (left._upper == right._upper) && (left._lower >= right._lower);
		}

		public static UInt512 operator >>>(UInt512 value, int shiftAmount)
		{
			shiftAmount &= 0x1FF;

			if ((shiftAmount & 0x100) != 0)
			{
				UInt256 lower = value._upper >> shiftAmount;
				return new UInt512(UInt256.Zero, lower);
			}
			else if (shiftAmount != 0)
			{
				UInt256 lower = (value._lower >> shiftAmount) | (value._upper << (256 - shiftAmount));
				UInt256 upper = value._upper >> shiftAmount;

				return new UInt512(upper, lower);
			}
			else
			{
				return value;
			}
		}
	}
}
