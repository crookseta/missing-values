using MissingValues.Internals;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public readonly partial struct UInt256 : 
		IBigInteger<UInt256>,
		IMinMaxValue<UInt256>,
		IUnsignedNumber<UInt256>,
		IFormattableUnsignedInteger<UInt256, Int256>
	{
		public static UInt256 One => new(1);

		public static int Radix => 2;

		public static UInt256 Zero => default;

		public static UInt256 AdditiveIdentity => default;

		public static UInt256 MultiplicativeIdentity => One;

		// 115792089237316195423570985008687907853269984665640564039457584007913129639935
		public static UInt256 MaxValue => new(UInt128.MaxValue, UInt128.MaxValue);

		public static UInt256 MinValue => new(0);

		static UInt256 IFormattableInteger<UInt256>.Two => new(0x2);

		static UInt256 IFormattableInteger<UInt256>.Sixteen => new(0x10);

		static UInt256 IFormattableInteger<UInt256>.Ten => new(0xA);

		static char IFormattableInteger<UInt256>.LastDecimalDigitOfMaxValue => '1';

		static int IFormattableInteger<UInt256>.MaxDecimalDigits => 78;

		static int IFormattableInteger<UInt256>.MaxHexDigits => 64;

		static int IFormattableInteger<UInt256>.MaxBinaryDigits => 256;

		static UInt256 IFormattableUnsignedInteger<UInt256, Int256>.SignedMaxMagnitude => new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		static UInt256 IFormattableInteger<UInt256>.TwoPow2 => new(4);

		static UInt256 IFormattableInteger<UInt256>.SixteenPow2 => new(256);

		static UInt256 IFormattableInteger<UInt256>.TenPow2 => new(100);

		static UInt256 IFormattableInteger<UInt256>.TwoPow3 => new(8);

		static UInt256 IFormattableInteger<UInt256>.SixteenPow3 => new(4096);

		static UInt256 IFormattableInteger<UInt256>.TenPow3 => new(1000);

		static UInt256 INumberBase<UInt256>.Abs(UInt256 value) => value;

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

		public int CompareTo(UInt256 other)
		{
			if (this < other) return -1;
			else if (this > other) return 1;
			else return 0;
		}

		public int CompareTo(object? obj)
		{
			if(obj is UInt256 value)
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

		public static UInt256 CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt256 result;

			if (value is UInt256 v)
			{
				result = v;
			}
			else if (!UInt256.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<UInt256>(value, out result))
			{
				Thrower.NotSupported<UInt256, TOther>();
			}

			return result;
		}

		public static UInt256 CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt256 result;

			if (value is UInt256 v)
			{
				result = v;
			}
			else if (!UInt256.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<UInt256>(value, out result))
			{
				Thrower.NotSupported<UInt256, TOther>();
			}

			return result;
		}

		public static UInt256 CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			UInt256 result;

			if (value is UInt256 v)
			{
				result = v;
			}
			else if (!UInt256.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<UInt256>(value, out result))
			{
				Thrower.NotSupported<UInt256, TOther>();
			}

			return result;
		}

		public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
		{
			UInt256 quotient = left / right;
			return (quotient, (left - (quotient * right)));
		}

		public bool Equals(UInt256 other) => this == other;

		static bool INumberBase<UInt256>.IsCanonical(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsComplexNumber(UInt256 value) => false;

		public static bool IsEvenInteger(UInt256 value) => (value._p0 & 1) == 0;

		static bool INumberBase<UInt256>.IsFinite(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsImaginaryNumber(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInteger(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsNaN(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegative(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegativeInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNormal(UInt256 value) => value != UInt128.Zero;

		public static bool IsOddInteger(UInt256 value) => (value._p0 & 1) != 0;

		static bool INumberBase<UInt256>.IsPositive(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsPositiveInfinity(UInt256 value) => false;

		public static bool IsPow2(UInt256 value) => PopCount(in value) == 1;

		static bool INumberBase<UInt256>.IsRealNumber(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsSubnormal(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsZero(UInt256 value) => value == Zero;

		public static UInt256 LeadingZeroCount(UInt256 value)
		{
			if (value.Upper == UInt128.Zero)
			{
				return 128 + UInt128.LeadingZeroCount(value.Lower);
			}
			return UInt128.LeadingZeroCount(value.Upper);
		}

		public static UInt256 Log2(UInt256 value)
		{
			if(value.Upper == 0)
			{
				return UInt128.Log2(value.Lower);
			}
			return 128 + UInt128.Log2(value.Upper);
		}

		public static UInt256 Max(UInt256 x, UInt256 y) => (x >= y) ? x : y;

		static UInt256 INumber<UInt256>.MaxNumber(UInt256 x, UInt256 y) => Max(x, y);

		static UInt256 INumberBase<UInt256>.MaxMagnitude(UInt256 x, UInt256 y) => Max(x, y);

		static UInt256 INumberBase<UInt256>.MaxMagnitudeNumber(UInt256 x, UInt256 y) => Max(x, y);

		public static UInt256 Min(UInt256 x, UInt256 y) => (x <= y) ? x : y;

		static UInt256 INumber<UInt256>.MinNumber(UInt256 x, UInt256 y) => Min(x, y);

		static UInt256 INumberBase<UInt256>.MinMagnitude(UInt256 x, UInt256 y) => Min(x, y);

		static UInt256 INumberBase<UInt256>.MinMagnitudeNumber(UInt256 x, UInt256 y) => Min(x, y);

		public static UInt256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		public static UInt256 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		public static UInt256 Parse(string s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(utf8Text);
			}
			return output;
		}
		public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(utf8Text);
			}
			return output;
		}

		private static int PopCount(in UInt256 value) =>
			BitOperations.PopCount(value._p0) + BitOperations.PopCount(value._p1) + BitOperations.PopCount(value._p2) + BitOperations.PopCount(value._p3);

		public static UInt256 PopCount(UInt256 value)
		{
			return (UInt256)(BitOperations.PopCount(value._p0) + BitOperations.PopCount(value._p1) + BitOperations.PopCount(value._p2) + BitOperations.PopCount(value._p3));
		}

		public static UInt256 RotateLeft(UInt256 value, int rotateAmount)
		{
			return (value << rotateAmount) | (value >>> (256 - rotateAmount));
		}

		public static UInt256 RotateRight(UInt256 value, int rotateAmount)
		{
			return (value >>> rotateAmount) | (value << (256 - rotateAmount));
		}

		public static UInt256 TrailingZeroCount(UInt256 value)
		{
			if (value.Lower == 0)
			{
				return 128 + UInt128.TrailingZeroCount(value.Upper);
			}
			return UInt128.TrailingZeroCount(value.Lower);
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}

		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					sourceRef = ref Unsafe.Add(ref sourceRef, source.Length - Size);

					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt256>(ref sourceRef);

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

		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt256>(ref sourceRef);

					if (!BitConverter.IsLittleEndian)
					{
						result = BitHelper.ReverseEndianness(result);
					}
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
						UInt256 part = Unsafe.Add(ref sourceRef, i);
						part <<= (i * 8);
						result |= part;
					}
				}
			}

			value = result;
			return true;
		}

		static bool INumberBase<UInt256>.TryConvertFromChecked<TOther>(TOther value, out UInt256 result) => TryConvertFromChecked(value, out result);
		private static bool TryConvertFromChecked<TOther>(TOther value, out UInt256 result)
			where TOther : INumberBase<TOther>
		{
			bool converted = true;
			checked
			{
				result = value switch
				{
					char actual => (UInt256)actual,
					Half actual => (UInt256)actual,
					float actual => (UInt256)actual,
					double actual => (UInt256)actual,
					decimal actual => (UInt256)actual,
					byte actual => (UInt256)actual,
					ushort actual => (UInt256)actual,
					uint actual => (UInt256)actual,
					ulong actual => (UInt256)actual,
					UInt128 actual => (UInt256)actual,
					UInt256 actual => actual,
					UInt512 actual => (UInt256)actual,
					nuint actual => (UInt256)actual,
					sbyte actual => (UInt256)actual,
					short actual => (UInt256)actual,
					int actual => (UInt256)actual,
					long actual => (UInt256)actual,
					Int128 actual => (UInt256)actual,
					Int256 actual => (UInt256)actual,
					Int512 actual => (UInt256)actual,
					_ => BitHelper.DefaultConvert<UInt256>(out converted)
				}; 
			}
			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertFromSaturating<TOther>(TOther value, out UInt256 result) => TryConvertFromSaturating(value, out result);
		private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt256 result)
			where TOther : INumberBase<TOther>
		{
			const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (actual < Half.Zero) ? MinValue : (UInt256)actual,
				float actual => (actual < 0) ? MinValue : (UInt256)actual,
				double actual => (actual < 0) ? MinValue : (actual > TwoPow256) ? MaxValue : (UInt256)actual,
				decimal actual => (actual < 0) ? MinValue : (UInt128)actual,
				byte actual => actual,
				ushort actual => actual,
				uint actual => actual,
				ulong actual => actual,
				UInt128 actual => actual,
				UInt256 actual => actual,
				UInt512 actual => (actual > MaxValue) ? MaxValue : (UInt256)actual,
				nuint actual => actual,
				sbyte actual => (actual < 0) ? MinValue : (UInt256)actual,
				short actual => (actual < 0) ? MinValue : (UInt256)actual,
				int actual => (actual < 0) ? MinValue : (UInt256)actual,
				long actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int128 actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int256 actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int512 actual => (actual < 0) ? MinValue : (actual > (Int512)MaxValue) ? MaxValue : (UInt256)actual,
				_ => BitHelper.DefaultConvert<UInt256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertFromTruncating<TOther>(TOther value, out UInt256 result) => TryConvertFromTruncating(value, out result);
		private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt256 result)
			where TOther : INumberBase<TOther>
		{
			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (actual < Half.Zero) ? MinValue : (UInt256)actual,
				float actual => (actual < 0) ? MinValue : (UInt256)actual,
				double actual => (actual < 0) ? MinValue : (UInt256)actual,
				decimal actual => (actual < 0) ? MinValue : (UInt128)actual,
				byte actual => actual,
				ushort actual => actual,
				uint actual => actual,
				ulong actual => actual,
				UInt128 actual => actual,
				UInt256 actual => actual,
				UInt512 actual => (UInt256)actual,
				nuint actual => actual,
				sbyte actual => (actual < 0) ? MinValue : (UInt256)actual,
				short actual => (actual < 0) ? MinValue : (UInt256)actual,
				int actual => (actual < 0) ? MinValue : (UInt256)actual,
				long actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int128 actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int256 actual => (actual < 0) ? MinValue : (UInt256)actual,
				Int512 actual => (actual < 0) ? MinValue : (UInt256)actual,
				_ => BitHelper.DefaultConvert<UInt256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertToChecked<TOther>(UInt256 value, out TOther result)
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
					UInt256 => (TOther)(object)value,
					UInt512 => (TOther)(object)(UInt512)value,
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

		static bool INumberBase<UInt256>.TryConvertToSaturating<TOther>(UInt256 value, out TOther result)
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
				UInt256 => (TOther)(object)value,
				UInt512 => (TOther)(object)(UInt512)value,
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
				Int512 => (TOther)(object)(Int512)value,
#if TARGET_32BIT
				nint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? nint.MaxValue : (nint)value),
#else
				nint => (TOther)(object)((value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? nint.MaxValue : (nint)value),
#endif
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertToTruncating<TOther>(UInt256 value, out TOther result)
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
				UInt256 => (TOther)(object)value,
				UInt512 => (TOther)(object)(UInt512)value,
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

		public int GetByteCount()
		{
			return Size;
		}

		public int GetShortestBitLength()
		{
			UInt256 value = this;
			return (Size * 8) - BitHelper.LeadingZeroCount(value);
		}

		public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FormatUnsignedInteger<UInt256, Int256>(in this, format, formatProvider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUnsignedInteger<UInt256, Int256, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUnsignedInteger<UInt256, Int256, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
		}

		bool IBinaryInteger<UInt256>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
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

		bool IBinaryInteger<UInt256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
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

		private void WriteBigEndianUnsafe(Span<byte> destination)
		{
			UInt128 lower = Lower;
			UInt128 upper = Upper;

			if (BitConverter.IsLittleEndian)
			{
				lower = BitHelper.ReverseEndianness(lower);
				upper = BitHelper.ReverseEndianness(upper);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, upper);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, BitHelper.SizeOf<UInt128>()), lower);
		}
		private void WriteLittleEndianUnsafe(Span<byte> destination)
		{
			Debug.Assert(destination.Length >= Size);

			UInt128 lower = Lower;
			UInt128 upper = Upper;

			if (!BitConverter.IsLittleEndian)
			{
				lower = BitHelper.ReverseEndianness(lower);
				upper = BitHelper.ReverseEndianness(upper);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, lower);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, BitHelper.SizeOf<UInt128>()), upper);
		}

		char IFormattableInteger<UInt256>.ToChar()
		{
			return (char)this;
		}

		int IFormattableInteger<UInt256>.ToInt32()
		{
			return (int)this;
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

		Int256 IFormattableUnsignedInteger<UInt256, Int256>.ToSigned()
		{
			return (Int256)this;
		}

		internal static int CountDigits(in UInt256 value)
		{
			if (value.Upper == 0)
			{
				return NumberFormatter.CountDigits(value.Lower);
			}

			// We have more than 1e38, so we have at least 39 digits
			int digits = 39;

			if (value.Upper > 0x2)
			{
				var lower = value / new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0002, 0xF050_FE93_8943_ACC4, 0x5F65_5680_0000_0000);

				digits += NumberFormatter.CountDigits(lower.Lower);
			}
			else if ((value.Upper == 0x2) && (value.Lower >= new UInt128(0xF050_FE93_8943_ACC4, 0x5F65_5680_0000_0000)))
			{
				// We're greater than 1e39, but definitely less than 1e40
				// so we have exactly 40 digits
				digits++;
			}

			return digits;
		}
		static int IFormattableUnsignedInteger<UInt256, Int256>.CountDigits(in UInt256 value)
		{
			return CountDigits(in value);
		}

		/// <inheritdoc/>
		public static UInt256 operator +(in UInt256 value)
		{
			return value;
		}

		/// <inheritdoc/>
		public static UInt256 operator +(in UInt256 left, in UInt256 right)
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

			return new UInt256(part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static UInt256 operator checked +(in UInt256 left, in UInt256 right)
		{
			ulong part0 = left._p0 + right._p0;
			ulong carry = (part0 < left._p0) ? 1UL : 0UL;

			ulong part1 = left._p1 + right._p1 + carry;
			carry = (part1 < left._p1 || (carry == 1 && part1 == left._p1)) ? 1UL : 0UL;

			ulong part2 = left._p2 + right._p2 + carry;
			carry = (part2 < left._p2 || (carry == 1 && part2 == left._p2)) ? 1UL : 0UL;

			ulong part3 = checked(left._p3 + right._p3 + carry);

			return new UInt256(part3, part2, part1, part0);
		}

		/// <inheritdoc/>
		public static UInt256 operator -(in UInt256 value)
		{
			return Zero - value;
		}
		/// <inheritdoc/>
		public static UInt256 operator checked -(in UInt256 value)
		{
			return checked(Zero - value);
		}

		/// <inheritdoc/>
		public static UInt256 operator -(in UInt256 left, in UInt256 right)
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

			return new UInt256(part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static UInt256 operator checked -(in UInt256 left, in UInt256 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			ulong part0 = left._p0 - right._p0;
			ulong borrow = (part0 > left._p0) ? 1UL : 0UL;

			ulong part1 = left._p1 - right._p1 - borrow;
			borrow = (part1 > left._p1 || (borrow == 1UL && part1 == left._p1)) ? 1UL : 0UL;

			ulong part2 = left._p2 - right._p2 - borrow;
			borrow = (part2 > left._p2 || (borrow == 1UL && part2 == left._p2)) ? 1UL : 0UL;

			ulong part3 = checked(left._p3 - right._p3 - borrow);

			return new UInt256(part3, part2, part1, part0);

		}

		/// <inheritdoc/>
		public static UInt256 operator ~(in UInt256 value)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v = Vector256.OnesComplement(Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in value)));
				return Unsafe.As<Vector256<ulong>, UInt256>(ref v);
			}
			else
			{
				return new(~value._p3, ~value._p2, ~value._p1, ~value._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator ++(in UInt256 value)
		{
			return value + One;
		}
		/// <inheritdoc/>
		public static UInt256 operator checked ++(in UInt256 value)
		{
			return checked(value + One);
		}

		/// <inheritdoc/>
		public static UInt256 operator --(in UInt256 value)
		{
			return value - One;
		}
		/// <inheritdoc/>
		public static UInt256 operator checked --(in UInt256 value)
		{
			return checked(value - One);
		}

		/// <inheritdoc/>
		public static UInt256 operator *(in UInt256 left, in UInt256 right)
		{
			UInt128 upper = BitHelper.BigMul(left.Lower, right.Lower, out UInt128 lower);
			upper += (left.Upper * right.Lower) + (left.Lower * right.Upper);
			return new UInt256(upper, lower);
		}
		/// <inheritdoc/>
		public static UInt256 operator checked *(in UInt256 left, in UInt256 right)
		{
			UInt256 upper = BigMul(left, right, out UInt256 lower);

			if(upper != Zero)
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
			}

			return lower;
		}

		/// <inheritdoc/>
		public static UInt256 operator /(in UInt256 left, in UInt256 right)
		{
			if (right._p3 == 0 && right._p2 == 0)
			{
				if (right._p1 == 0 && right._p0 == 0)
				{
					Thrower.DivideByZero();
				}

				if (left._p3 == 0 && left._p2 == 0)
				{
					return left.Lower / right.Lower;
				}
			}

			if (right >= left)
			{
				return (right == left) ? One : Zero;
			}

			return DivideSlow(in left, in right);

			unsafe static UInt256 DivideSlow(in UInt256 quotient, in UInt256 divisor)
			{
				const int UlongCount = Size / sizeof(ulong);

				ulong* pLeft = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 0), (quotient._p0));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 1), (quotient._p1));

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 2), (quotient._p2));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 3), (quotient._p3));

				Span<ulong> left = new Span<ulong>(pLeft, (UlongCount) - (BitHelper.LeadingZeroCount(quotient) / 64));


				ulong* pRight = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 0), (divisor._p0));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 1), (divisor.	_p1));

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 2), (divisor._p2));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 3), (divisor._p3));

				Span<ulong> right = new Span<ulong>(pRight, (UlongCount) - (BitHelper.LeadingZeroCount(divisor) / 64));


				Span<ulong> rawBits = stackalloc ulong[UlongCount];
				rawBits.Clear();

				BitHelper.DivideSlow(left, right, rawBits);

				return new UInt256(
					rawBits[3], rawBits[2],
					rawBits[1], rawBits[0]
					);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator %(in UInt256 left, in UInt256 right)
		{
			UInt256 quotient = left / right;
			return left - (quotient * right);
		}

		/// <inheritdoc/>
		public static UInt256 operator &(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, UInt256>(ref Unsafe.AsRef(v1 & v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.And(v1, v2);
				return Unsafe.As<Vector256<ulong>, UInt256>(ref result);
			}
			else
			{
				return new(left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator |(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, UInt256>(ref Unsafe.AsRef(v1 | v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.Or(v1, v2);
				return Unsafe.As<Vector256<ulong>, UInt256>(ref result);
			}
			else
			{
				return new(left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator ^(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, UInt256>(ref Unsafe.AsRef(v1 ^ v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.Xor(v1, v2);
				return Unsafe.As<Vector256<ulong>, UInt256>(ref result);
			}
			else
			{
				return new(left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator <<(in UInt256 value, int shiftAmount)
		{
			// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
			// need to specially handle things if the shift amount exceeds 0x3F.

			shiftAmount &= 0xFF; // mask the shift amount to be within [0, 255]

			if (shiftAmount == 0)
			{
				return value;
			}

			if (shiftAmount < 64)
			{
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt256(part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new UInt256(value._p2, value._p1, value._p0, 0);
				}

				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt256(part2, part1, part0, 0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new UInt256(value._p1, value._p0, 0, 0);
				}

				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt256(part1, part0, 0, 0);
			}
			else // shiftAmount < 256
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new UInt256(value._p0, 0, 0, 0);
				}

				ulong part0 = value._p0 << shiftAmount;

				return new UInt256(part0, 0, 0, 0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator >>(in UInt256 value, int shiftAmount) => value >>> shiftAmount;

		/// <inheritdoc/>
		public static bool operator ==(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return v1 == v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<UInt256, Vector256<byte>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<byte>>(ref Unsafe.AsRef(in right));
				var equals = Avx2.CompareEqual(v1, v2);
				var result = Avx2.MoveMask(equals);
				return (result & 0xFFFF_FFFF) == 0xFFFF_FFFF;
			}
			else
			{
				return (left._p3 == right._p3) && (left._p2 == right._p2) && (left._p1 == right._p1) && (left._p0 == right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator !=(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return v1 != v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<UInt256, Vector256<byte>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt256, Vector256<byte>>(ref Unsafe.AsRef(in right));
				var equals = Avx2.CompareEqual(v1, v2);
				var result = Avx2.MoveMask(equals);
				return (result & 0xFFFF_FFFF) != 0xFFFF_FFFF;
			}
			else
			{
				return (left._p3 != right._p3) || (left._p2 != right._p2) || (left._p1 != right._p1) || (left._p0 != right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator <(in UInt256 left, in UInt256 right)
		{
			// Successively compare each part.
			return (left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 < right._p0))))));
			
		}

		/// <inheritdoc/>
		public static bool operator >(in UInt256 left, in UInt256 right)
		{
			return (left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 > right._p0))))));
		}

		/// <inheritdoc/>
		public static bool operator <=(in UInt256 left, in UInt256 right)
		{
			return (left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 <= right._p0))))));
		}

		/// <inheritdoc/>
		public static bool operator >=(in UInt256 left, in UInt256 right)
		{
			return (left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 >= right._p0))))));
		}

		/// <inheritdoc/>
		public static UInt256 operator >>>(in UInt256 value, int shiftAmount)
		{
			// C# automatically masks the shift amount for UInt64 to be 0x3F. So we
			// need to specially handle things if the shift amount exceeds 0x3F.

			shiftAmount &= 0xFF; // mask the shift amount to be within [0, 255]

			if (shiftAmount == 0)
			{
				return value;
			}
			
			if (shiftAmount < 64)
			{
				ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
				ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part3 = value._p3 >> shiftAmount;

				return new UInt256(part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new UInt256(0, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = value._p3 >> shiftAmount;

				return new UInt256(0, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new UInt256(0, 0, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = value._p3 >> shiftAmount;

				return new UInt256(0, 0, part1, part0);
			}
			else // shiftAmount < 256
			{
				shiftAmount -= 192;

				ulong part0 = value._p3 >> shiftAmount;

				return new UInt256(0, 0, 0, part0);
			}
		}
	}
}
