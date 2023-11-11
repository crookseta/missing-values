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
		IBinaryInteger<UInt256>,
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
		{
			UInt256 quotient = left / right;
			return (quotient, (left - (quotient * right)));
		}

		public bool Equals(UInt256 other) => this == other;

		static bool INumberBase<UInt256>.IsCanonical(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsComplexNumber(UInt256 value) => false;

		public static bool IsEvenInteger(UInt256 value) => (value._lower & 1) == 0;

		static bool INumberBase<UInt256>.IsFinite(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsImaginaryNumber(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInteger(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsNaN(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegative(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegativeInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNormal(UInt256 value) => value != UInt128.Zero;

		public static bool IsOddInteger(UInt256 value) => (value._lower & 1) != 0;

		static bool INumberBase<UInt256>.IsPositive(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsPositiveInfinity(UInt256 value) => false;

		public static bool IsPow2(UInt256 value) => PopCount(value) == 1;

		static bool INumberBase<UInt256>.IsRealNumber(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsSubnormal(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsZero(UInt256 value) => value == Zero;

		public static UInt256 LeadingZeroCount(UInt256 value)
		{
			if (value._upper == UInt128.Zero)
			{
				return 128 + UInt128.LeadingZeroCount(value._lower);
			}
			return UInt128.LeadingZeroCount(value._upper);
		}

		public static UInt256 Log2(UInt256 value)
		{
			if(value._upper == 0)
			{
				return UInt128.Log2(value._lower);
			}
			return 128 + UInt128.Log2(value._upper);
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
			return NumberParser.ParseToUnsigned<UInt256>(s, style, provider);
		}

		public static UInt256 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt256>(s, style, provider);
		}

		public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt256>(s, NumberStyles.Integer, provider);
		}

		public static UInt256 Parse(string s, IFormatProvider? provider)
		{
			return NumberParser.ParseToUnsigned<UInt256>(s, NumberStyles.Integer, provider);
		}

		public static UInt256 PopCount(UInt256 value)
		{
			return UInt128.PopCount(value._lower) + UInt128.PopCount(value._upper);
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
			if (value._lower == 0)
			{
				return 128 + UInt128.TrailingZeroCount(value._upper);
			}
			return UInt128.TrailingZeroCount(value._lower);
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(s, style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(s, style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(s, NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(s, NumberStyles.Integer, provider, out result);
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
			return NumberFormatter.FormatUnsignedNumber(in this, format, NumberStyles.Integer, formatProvider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUnsignedNumber(in this, destination, out charsWritten, format, provider);
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
			UInt128 lower = _lower;
			UInt128 upper = _upper;

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

			UInt128 lower = _lower;
			UInt128 upper = _upper;

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

		static UInt256 IFormattableInteger<UInt256>.GetDecimalValue(char value)
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

		public static UInt256 operator +(UInt256 value)
		{
			return value;
		}

		public static UInt256 operator +(UInt256 left, UInt256 right)
		{
			if (Avx2.IsSupported)
			{
				/*
				 * Based on AddImpl from Nethermind.Int256
				 * Source: https://github.com/NethermindEth/int256/blob/master/src/Nethermind.Int256/UInt256.cs
				 */

				var av = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var bv = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));

				var result = Avx2.Add(av, bv);

				var carryFromBothHighBits = Avx2.And(av, bv);
				var eitherHighBit = Avx2.Or(av, bv);
				var highBitNotInResult = Avx2.AndNot(result, eitherHighBit);

				// Set high bits where carry occurs
				var vCarry = Avx2.Or(carryFromBothHighBits, highBitNotInResult);
				// Move carry from Vector space to int
				var carry = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCarry));

				// All bits set will cascade another carry when carry is added to it
				var vCascade = Avx2.CompareEqual(result, Vector256<ulong>.AllBitsSet);
				// Move cascade from Vector space to int
				var cascade = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCascade));

				// Use ints to work out the Vector cross lane cascades
				// Move carry to next bit and add cascade
				carry = cascade + 2 * carry; // lea
											 // Remove cascades not effected by carry
				cascade ^= carry;
				// Choice of 16 vectors
				cascade &= 0x0F;

				// Lookup the carries to broadcast to the Vectors
				var cascadedCarries = Unsafe.Add(ref Unsafe.As<byte, Vector256<ulong>>(ref MemoryMarshal.GetReference(_broadcastLookup)), cascade);

				// Mark res as initalized so we can use it as left said of ref assignment
				Unsafe.SkipInit(out UInt256 res);
				// Add the cascadedCarries to the result
				Unsafe.As<UInt256, Vector256<ulong>>(ref res) = Avx2.Add(result, cascadedCarries);

				return res;
			}
			else
			{
				UInt128 lower = left._lower + right._lower;
				UInt128 carry = (lower < left._lower) ? 1UL : 0UL;

				UInt128 upper = left._upper + right._upper + carry;
				return new UInt256(upper, lower);
			}
		}
		public static UInt256 operator checked +(UInt256 left, UInt256 right)
		{
			if (Avx2.IsSupported)
			{
				/*
				 * Based on AddImpl from Nethermind.Int256
				 * Source: https://github.com/NethermindEth/int256/blob/master/src/Nethermind.Int256/UInt256.cs
				 */

				var av = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var bv = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));

				var result = Avx2.Add(av, bv);

				var carryFromBothHighBits = Avx2.And(av, bv);
				var eitherHighBit = Avx2.Or(av, bv);
				var highBitNotInResult = Avx2.AndNot(result, eitherHighBit);

				// Set high bits where carry occurs
				var vCarry = Avx2.Or(carryFromBothHighBits, highBitNotInResult);
				// Move carry from Vector space to int
				var carry = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCarry));

				// All bits set will cascade another carry when carry is added to it
				var vCascade = Avx2.CompareEqual(result, Vector256<ulong>.AllBitsSet);
				// Move cascade from Vector space to int
				var cascade = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCascade));

				// Use ints to work out the Vector cross lane cascades
				// Move carry to next bit and add cascade
				carry = cascade + 2 * carry; // lea
											 // Remove cascades not effected by carry
				cascade ^= carry;
				// Choice of 16 vectors
				cascade &= 0x0F;

				// Lookup the carries to broadcast to the Vectors
				var cascadedCarries = Unsafe.Add(ref Unsafe.As<byte, Vector256<ulong>>(ref MemoryMarshal.GetReference(_broadcastLookup)), cascade);

				// Mark res as initalized so we can use it as left said of ref assignment
				Unsafe.SkipInit(out UInt256 res);
				// Add the cascadedCarries to the result
				Unsafe.As<UInt256, Vector256<ulong>>(ref res) = Avx2.Add(result, cascadedCarries);

				if ((carry & 0b1_000) != 0)
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Addition);
				}

				return res;
			}
			else
			{
				UInt128 lower = left._lower + right._lower;
				UInt128 carry = (lower < left._lower) ? 1UL : 0UL;

				UInt128 upper = checked(left._upper + right._upper + carry);
				return new UInt256(upper, lower); 
			}
		}

		public static UInt256 operator -(UInt256 value)
		{
			return Zero - value;
		}
		public static UInt256 operator checked -(UInt256 value)
		{
			return checked(Zero - value);
		}

		public static UInt256 operator -(UInt256 left, UInt256 right)
		{
			if (Avx2.IsSupported)
			{
				/*
				 * Based on SubtractImpl from Nethermind.Int256
				 * Source: https://github.com/NethermindEth/int256/blob/master/src/Nethermind.Int256/UInt256.cs
				 */
				var av = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var bv = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));

				var result = Avx2.Subtract(av, bv);
				// Invert top bits as Avx2.CompareGreaterThan is only available for longs, not unsigned
				var resultSigned = Avx2.Xor(result, Vector256.Create<ulong>(0x8000_0000_0000_0000));
				var avSigned = Avx2.Xor(av, Vector256.Create<ulong>(0x8000_0000_0000_0000));

				// Which vectors need to borrow from the next
				var vBorrow = Avx2.CompareGreaterThan(Unsafe.As<Vector256<ulong>, Vector256<long>>(ref resultSigned),
													  Unsafe.As<Vector256<ulong>, Vector256<long>>(ref avSigned));

				// Move borrow from Vector space to int
				var borrow = Avx.MoveMask(Unsafe.As<Vector256<long>, Vector256<double>>(ref vBorrow));

				// All zeros will cascade another borrow when borrow is subtracted from it
				var vCascade = Avx2.CompareEqual(result, Vector256<ulong>.Zero);
				// Move cascade from Vector space to int
				var cascade = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCascade));

				// Use ints to work out the Vector cross lane cascades
				// Move borrow to next bit and add cascade
				borrow = cascade + 2 * borrow; // lea
											   // Remove cascades not effected by borrow
				cascade ^= borrow;
				// Choice of 16 vectors
				cascade &= 0x0f;

				// Lookup the borrows to broadcast to the Vectors
				var cascadedBorrows = Unsafe.Add(ref Unsafe.As<byte, Vector256<ulong>>(ref MemoryMarshal.GetReference(_broadcastLookup)), cascade);

				// Mark res as initalized so we can use it as left said of ref assignment
				Unsafe.SkipInit(out UInt256 res);
				// Subtract the cascadedBorrows from the result
				Unsafe.As<UInt256, Vector256<ulong>>(ref res) = Avx2.Subtract(result, cascadedBorrows);

				return res;
			}
			else
			{
				// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
				// This gives us the borrow to subtract from upper to compute the correct result

				UInt128 lower = left._lower - right._lower;
				UInt128 borrow = (lower > left._lower) ? 1UL : 0UL;

				UInt128 upper = left._upper - right._upper - borrow;
				return new UInt256(upper, lower); 
			}
		}
		public static UInt256 operator checked -(UInt256 left, UInt256 right)
		{
			if (Avx2.IsSupported)
			{
				/*
				 * Based on SubtractImpl from Nethermind.Int256
				 * Source: https://github.com/NethermindEth/int256/blob/master/src/Nethermind.Int256/UInt256.cs
				 */
				var av = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var bv = Unsafe.As<UInt256, Vector256<ulong>>(ref Unsafe.AsRef(in right));

				var result = Avx2.Subtract(av, bv);
				// Invert top bits as Avx2.CompareGreaterThan is only available for longs, not unsigned
				var resultSigned = Avx2.Xor(result, Vector256.Create<ulong>(0x8000_0000_0000_0000));
				var avSigned = Avx2.Xor(av, Vector256.Create<ulong>(0x8000_0000_0000_0000));

				// Which vectors need to borrow from the next
				var vBorrow = Avx2.CompareGreaterThan(Unsafe.As<Vector256<ulong>, Vector256<long>>(ref resultSigned),
													  Unsafe.As<Vector256<ulong>, Vector256<long>>(ref avSigned));

				// Move borrow from Vector space to int
				var borrow = Avx.MoveMask(Unsafe.As<Vector256<long>, Vector256<double>>(ref vBorrow));

				// All zeros will cascade another borrow when borrow is subtracted from it
				var vCascade = Avx2.CompareEqual(result, Vector256<ulong>.Zero);
				// Move cascade from Vector space to int
				var cascade = Avx.MoveMask(Unsafe.As<Vector256<ulong>, Vector256<double>>(ref vCascade));

				// Use ints to work out the Vector cross lane cascades
				// Move borrow to next bit and add cascade
				borrow = cascade + 2 * borrow; // lea
											   // Remove cascades not effected by borrow
				cascade ^= borrow;
				// Choice of 16 vectors
				cascade &= 0x0f;

				// Lookup the borrows to broadcast to the Vectors
				var cascadedBorrows = Unsafe.Add(ref Unsafe.As<byte, Vector256<ulong>>(ref MemoryMarshal.GetReference(_broadcastLookup)), cascade);

				// Mark res as initalized so we can use it as left said of ref assignment
				Unsafe.SkipInit(out UInt256 res);
				// Subtract the cascadedBorrows from the result
				Unsafe.As<UInt256, Vector256<ulong>>(ref res) = Avx2.Subtract(result, cascadedBorrows);

				if ((borrow & 0b1_0000) != 0)
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Addition);
				}

				return res;
			}
			else
			{
				// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
				// This gives us the borrow to subtract from upper to compute the correct result

				UInt128 lower = left._lower - right._lower;
				UInt128 borrow = (lower > left._lower) ? 1UL : 0UL;

				UInt128 upper = checked(left._upper - right._upper - borrow);
				return new UInt256(upper, lower); 
			}
		}

		public static UInt256 operator ~(UInt256 value)
		{
			return new(~value._upper, ~value._lower);
		}

		public static UInt256 operator ++(UInt256 value)
		{
			return value + One;
		}
		public static UInt256 operator checked ++(UInt256 value)
		{
			return checked(value + One);
		}

		public static UInt256 operator --(UInt256 value)
		{
			return value - One;
		}
		public static UInt256 operator checked --(UInt256 value)
		{
			return checked(value - One);
		}

		public static UInt256 operator *(UInt256 left, UInt256 right)
		{
			UInt128 upper = BitHelper.BigMul(left._lower, right._lower, out UInt128 lower);
			upper += (left._upper * right._lower) + (left._lower * right._upper);
			return new UInt256(upper, lower);
		}
		public static UInt256 operator checked *(UInt256 left, UInt256 right)
		{
			UInt256 upper = BigMul(left, right, out UInt256 lower);

			if(upper != UInt256.Zero)
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
			}

			return lower;
		}

		public static UInt256 operator /(UInt256 left, UInt256 right)
		{
			if((left._upper == UInt128.Zero) && (right._upper == UInt128.Zero))
			{
				return left._lower / right._lower;
			}
			if(right >= left)
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

			unsafe static UInt256 DivideSlow(UInt256 quotient, UInt256 divisor)
			{
				const int UlongCount = 32 / sizeof(ulong);

				ulong* pLeft = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 0), (ulong)(quotient._lower >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 1), (ulong)(quotient._lower >> 64));

				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 2), (ulong)(quotient._upper >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pLeft + 3), (ulong)(quotient._upper >> 64));

				Span<ulong> left = new Span<ulong>(pLeft, (UlongCount) - (BitHelper.LeadingZeroCount(quotient) / 64));


				ulong* pRight = stackalloc ulong[UlongCount];

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 0), (ulong)(divisor._lower >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 1), (ulong)(divisor.	_lower >> 64));

				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 2), (ulong)(divisor._upper >> 00));
				Unsafe.WriteUnaligned(ref *(byte*)(pRight + 3), (ulong)(divisor._upper >> 64));

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

				return new UInt256(
					((UInt128)(rawBits[3]) << 64) | rawBits[2],
					((UInt128)(rawBits[1]) << 64) | rawBits[0]
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
		}

		public static UInt256 operator %(UInt256 left, UInt256 right)
		{
			UInt256 quotient = left / right;
			return left - (quotient * right);
		}

		public static UInt256 operator &(UInt256 left, UInt256 right)
		{
			return new(left._upper & right._upper, left._lower & right._lower);
		}

		public static UInt256 operator |(UInt256 left, UInt256 right)
		{
			return new(left._upper | right._upper, left._lower | right._lower);
		}

		public static UInt256 operator ^(UInt256 left, UInt256 right)
		{
			return new(left._upper ^ right._upper, left._lower ^ right._lower);
		}

		public static UInt256 operator <<(UInt256 value, int shiftAmount)
		{
			// We need to specially handle things if the 15th bit is set.

			shiftAmount &= 0xFF;

			if ((shiftAmount & 0x80) != 0)
			{
				// In the case it is set, we know the entire upper bits must be zero
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt128 upper = value._lower << shiftAmount;
				return new UInt256(upper, 0);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of lower

				UInt128 lower = value._lower << shiftAmount;
				UInt128 upper = (value._upper << shiftAmount) | (value._lower >> (128 - shiftAmount));

				return new UInt256(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static UInt256 operator >>(UInt256 value, int shiftAmount) => value >>> shiftAmount;

		public static bool operator ==(UInt256 left, UInt256 right)
		{
			return (left._upper == right._upper) && (left._lower == right._lower);
		}

		public static bool operator !=(UInt256 left, UInt256 right)
		{
			return (left._upper != right._upper) || (left._lower != right._lower);
		}

		public static bool operator <(UInt256 left, UInt256 right)
		{
			return (left._upper < right._upper)
				|| (left._upper == right._upper) && (left._lower < right._lower);
		}

		public static bool operator >(UInt256 left, UInt256 right)
		{
			return (left._upper > right._upper)
				|| (left._upper == right._upper) && (left._lower > right._lower);
		}

		public static bool operator <=(UInt256 left, UInt256 right)
		{
			return (left._upper < right._upper)
			   || (left._upper == right._upper) && (left._lower <= right._lower);
		}

		public static bool operator >=(UInt256 left, UInt256 right)
		{
			return (left._upper > right._upper)
				|| (left._upper == right._upper) && (left._lower >= right._lower);
		}

		public static UInt256 operator >>>(UInt256 value, int shiftAmount)
		{
			// We need to specially handle things if the 15th bit is set.

			shiftAmount &= 0xFF;

			if ((shiftAmount & 0x80) != 0)
			{
				// In the case it is set, we know the entire upper bits must be zero
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt128 lower = value._upper >> shiftAmount;
				return new UInt256(0, lower);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of upper

				UInt128 lower = (value._lower >> shiftAmount) | (value._upper << (128 - shiftAmount));
				UInt128 upper = value._upper >> shiftAmount;

				return new UInt256(upper, lower);
			}
			else
			{
				return value;
			}
		}
	}
}
