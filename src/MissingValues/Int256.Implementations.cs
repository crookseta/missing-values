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
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using System.Text.Unicode;

namespace MissingValues
{
	public partial struct Int256 :
		IBinaryInteger<Int256>,
		IMinMaxValue<Int256>,
		ISignedNumber<Int256>,
		IFormattableSignedInteger<Int256, UInt256>
	{
		private static UInt128 _upperMin => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		private static UInt128 _lowerMin => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		private static UInt128 _upperMax => new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static UInt128 _lowerMax => new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		static Int256 IBinaryNumber<Int256>.AllBitsSet => new(_lowerMax, _lowerMax);
		// MaxValue = 57896044618658097711785492504343953926634992332820282019728792003956564819967
		public static Int256 MaxValue => new(_upperMax, _lowerMax);
		// MinValue = -57896044618658097711785492504343953926634992332820282019728792003956564819968
		public static Int256 MinValue => new(_upperMin, _lowerMin);

		public static Int256 NegativeOne => new(_lowerMax, _lowerMax);

		public static Int256 One => new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);

		static int INumberBase<Int256>.Radix => 2;

		public static Int256 Zero => default;

		static Int256 IAdditiveIdentity<Int256, Int256>.AdditiveIdentity => default;

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

		public static Int256 Abs(Int256 value)
		{
			if (IsNegative(value))
			{
				value = -value;

				if (IsNegative(value))
				{
					Thrower.MinimumSignedAbsoluteValue<Int256>();
				}
			}
			return value;
		}

		public static (Int256 Quotient, Int256 Remainder) DivRem(Int256 left, Int256 right)
		{
			Int256 quotient = left / right;
			return (quotient, left - (quotient * right));
		}

		static bool INumberBase<Int256>.IsCanonical(Int256 value) => true;

		static bool INumberBase<Int256>.IsComplexNumber(Int256 value) => false;

		public static bool IsEvenInteger(Int256 value) => (value._lower & 1) == UInt128.Zero;

		static bool INumberBase<Int256>.IsFinite(Int256 value) => true;

		static bool INumberBase<Int256>.IsImaginaryNumber(Int256 value) => false;

		static bool INumberBase<Int256>.IsInfinity(Int256 value) => false;

		static bool INumberBase<Int256>.IsInteger(Int256 value) => true;

		static bool INumberBase<Int256>.IsNaN(Int256 value) => false;

		public static bool IsNegative(Int256 value) => (Int128)value._upper < Int128.Zero;

		static bool INumberBase<Int256>.IsNegativeInfinity(Int256 value) => false;

		static bool INumberBase<Int256>.IsNormal(Int256 value) => value != Zero;

		public static bool IsOddInteger(Int256 value) => (value._lower & 1) != 0;

		public static bool IsPositive(Int256 value) => (Int128)value._upper >= Int128.Zero;

		static bool INumberBase<Int256>.IsPositiveInfinity(Int256 value) => false;

		public static bool IsPow2(Int256 value) => (PopCount(value) == One) && IsPositive(value);

		static bool INumberBase<Int256>.IsRealNumber(Int256 value) => true;

		static bool INumberBase<Int256>.IsSubnormal(Int256 value) => false;

		static bool INumberBase<Int256>.IsZero(Int256 value) => (value == Zero);

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

		public static Int256 CopySign(Int256 value, Int256 sign)
		{
			var absValue = value;

			if (IsNegative(absValue))
			{
				absValue = -absValue;
			}

			if (IsPositive(sign))
			{
				if (IsNegative(absValue))
				{
					Thrower.MinimumSignedAbsoluteValue<Int256>();
				}
				return absValue;
			}
			return -absValue;
		}

		public static Int256 LeadingZeroCount(Int256 value)
		{
			if (value._upper == 0)
			{
				return (Int256)(128 + UInt128.LeadingZeroCount(value._lower));
			}
			return (Int256)UInt128.LeadingZeroCount(value._upper);
		}

		public static Int256 Log2(Int256 value)
		{
			if (IsNegative(value))
			{
				Thrower.NeedsNonNegative<Int256>();
			}

			if (value._upper == 0)
			{
				return (Int256)UInt128.Log2(value._lower);
			}
			return (Int256)(128 + UInt128.Log2(value._upper));
		}

		public static Int256 MaxMagnitude(Int256 x, Int256 y)
		{
			Int256 absX = x;

			if (IsNegative(absX))
			{
				absX = -absX;

				if (IsNegative(absX))
				{
					return x;
				}
			}

			Int256 absY = y;

			if (IsNegative(absY))
			{
				absY = -absY;

				if (IsNegative(absY))
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
				return IsNegative(x) ? y : x;
			}

			return y;
		}

		static Int256 INumberBase<Int256>.MaxMagnitudeNumber(Int256 x, Int256 y) => MaxMagnitude(x, y);

		public static Int256 MinMagnitude(Int256 x, Int256 y)
		{
			Int256 absX = x;

			if (IsNegative(absX))
			{
				absX = -absX;

				if (IsNegative(absX))
				{
					return y;
				}
			}

			Int256 absY = y;

			if (IsNegative(absY))
			{
				absY = -absY;

				if (IsNegative(absY))
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
				return IsNegative(x) ? x : y;
			}

			return y;
		}

		static Int256 INumberBase<Int256>.MinMagnitudeNumber(Int256 x, Int256 y) => MinMagnitude(x, y);

		public static Int256 Max(Int256 x, Int256 y) => (x >= y) ? x : y;

		static Int256 INumber<Int256>.MaxNumber(Int256 x, Int256 y) => Max(x, y);

		public static Int256 Min(Int256 x, Int256 y) => (x <= y) ? x : y;

		static Int256 INumber<Int256>.MinNumber(Int256 x, Int256 y) => Min(x, y);

		public static int Sign(Int256 value)
		{
			if (IsNegative(value))
			{
				return -1;
			}
			else if (value != default)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public static Int256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int256 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		public static Int256 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out Int256 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		public static Int256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int256 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		public static Int256 Parse(string s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int256 output);
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

#if NET8_0_OR_GREATER
		public static Int256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out Int256 output);
			if (!status)
			{
				string input = new string(Encoding.UTF8.GetChars(utf8Text.ToArray()));
				status.Throw<Int256>(input);
			}
			return output;
		}
		public static Int256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out Int256 output);
			if (!status)
			{
				string input = new string(Encoding.UTF8.GetChars(utf8Text.ToArray()));
				status.Throw<Int256>(input);
			}
			return output;
		}
#endif

		public static Int256 PopCount(Int256 value)
		{
			return (Int256)(UInt128.PopCount(value._lower) + UInt128.PopCount(value._upper));
		}

		public static Int256 RotateLeft(Int256 value, int rotateAmount)
		{
			return (value << rotateAmount) | (value >>> (256 - rotateAmount));
		}

		public static Int256 RotateRight(Int256 value, int rotateAmount)
		{
			return (value >>> rotateAmount) | (value << (256 - rotateAmount));
		}

		public static Int256 TrailingZeroCount(Int256 value)
		{
			if (value._lower == 0)
			{
				return (Int256)(128 + UInt128.TrailingZeroCount(value._upper));
			}
			return (Int256)UInt128.TrailingZeroCount(value._lower);
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

#if NET8_0_OR_GREATER
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}
#endif

		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
		{
			Int256 result = default;

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

					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<Int256>(ref sourceRef);

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

					if (!isUnsigned)
					{
						result |= ((One << ((Size * 8) - 1)) >> (((Size - source.Length) * 8) - 1));
					}
				}
			}

			value = result;
			return true;
		}

		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
		{
			Int256 result = default;

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
					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<Int256>(ref sourceRef);

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
						result <<= 8;
						result |= Unsafe.Add(ref sourceRef, i);
					}

					result <<= ((Size - source.Length) * 8);
					result = BitHelper.ReverseEndianness(result);

					if (!isUnsigned)
					{
						result |= ((One << ((Size * 8) - 1)) >> (((Size - source.Length) * 8) - 1));
					}
				}
			}

			value = result;
			return true;
		}

		static bool INumberBase<Int256>.TryConvertFromChecked<TOther>(TOther value, out Int256 result) => TryConvertFromChecked(value, out result);
		private static bool TryConvertFromChecked<TOther>(TOther value, out Int256 result)
			where TOther : INumberBase<TOther>
		{
			bool converted = true;

			checked
			{
				result = value switch
				{
					char actual => (Int256)actual,
					Half actual => (Int256)actual,
					float actual => (Int256)actual,
					double actual => (Int256)actual,
					decimal actual => (Int256)actual,
					byte actual => (Int256)actual,
					ushort actual => (Int256)actual,
					uint actual => (Int256)actual,
					ulong actual => (Int256)actual,
					UInt128 actual => (Int256)actual,
					UInt256 actual => (Int256)actual,
					UInt512 actual => (Int256)actual,
					nuint actual => (Int256)actual,
					sbyte actual => (Int256)actual,
					short actual => (Int256)actual,
					int actual => (Int256)actual,
					long actual => (Int256)actual,
					Int128 actual => (Int256)actual,
					Int256 actual => actual,
					Int512 actual => (Int256)actual,
					nint actual => (Int256)actual,
					_ => BitHelper.DefaultConvert<Int256>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int256>.TryConvertFromSaturating<TOther>(TOther value, out Int256 result) => TryConvertFromSaturating(value, out result);
		private static bool TryConvertFromSaturating<TOther>(TOther value, out Int256 result)
			where TOther : INumberBase<TOther>
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (Int256)actual,
				float actual => (Int256)actual,
				double actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
				decimal actual => (Int256)actual,
				byte actual => (Int256)actual,
				ushort actual => (Int256)actual,
				uint actual => (Int256)actual,
				ulong actual => (Int256)actual,
				UInt128 actual => (Int256)actual,
				UInt256 actual => (actual > (UInt256)MaxValue) ? MaxValue : (Int256)actual,
				nuint actual => (Int256)actual,
				sbyte actual => actual,
				short actual => actual,
				int actual => actual,
				long actual => actual,
				Int128 actual => actual,
				Int256 actual => actual,
				Int512 actual => (actual <= MinValue) ? MinValue : (actual >= MaxValue) ? MaxValue : (Int256)actual,
				nint actual => actual,
				_ => BitHelper.DefaultConvert<Int256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<Int256>.TryConvertFromTruncating<TOther>(TOther value, out Int256 result) => TryConvertFromTruncating(value, out result);
		private static bool TryConvertFromTruncating<TOther>(TOther value, out Int256 result)
			where TOther : INumberBase<TOther>
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			bool converted = true;
			result = value switch
			{
				char actual => actual,
				Half actual => (actual == Half.PositiveInfinity) ? MaxValue : (actual == Half.NegativeInfinity) ? MinValue : (Int256)actual,
				float actual => (actual == float.PositiveInfinity) ? MaxValue : (actual == float.NegativeInfinity) ? MinValue : (Int256)actual,
				double actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
				decimal actual => (Int128)actual,
				byte actual => (Int256)actual,
				ushort actual => (Int256)actual,
				uint actual => (Int256)actual,
				ulong actual => (Int256)actual,
				UInt128 actual => (Int256)actual,
				UInt256 actual => (actual > (UInt256)MaxValue) ? MaxValue : (Int256)actual,
				nuint actual => (Int256)actual,
				sbyte actual => actual,
				short actual => actual,
				int actual => actual,
				long actual => actual,
				Int128 actual => actual,
				Int256 actual => actual,
				nint actual => actual,
				_ => BitHelper.DefaultConvert<Int256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToChecked<TOther>(Int256 value, out TOther result)
		{
			bool converted = true;
			result = default!;
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
					UInt512 => (TOther)(object)(UInt512)value,
					nuint => (TOther)(object)(nuint)value,
					sbyte => (TOther)(object)(sbyte)value,
					short => (TOther)(object)(short)value,
					int => (TOther)(object)(int)value,
					long => (TOther)(object)(long)value,
					Int128 => (TOther)(object)(Int128)value,
					Int256 => (TOther)(object)value,
					Int512 => (TOther)(object)(Int512)value,
					nint => (TOther)(object)(nint)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToSaturating<TOther>(Int256 value, out TOther result)
		{
			bool converted = true;
			result = default!;

			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)((value >= (Int256)byte.MaxValue) ? byte.MaxValue : (value <= (Int256)byte.MinValue) ? byte.MinValue : (byte)value),
				ushort => (TOther)(object)((value >= (Int256)ushort.MaxValue) ? ushort.MaxValue : (value <= (Int256)ushort.MinValue) ? ushort.MinValue : (ushort)value),
				uint => (TOther)(object)((value >= (Int256)uint.MaxValue) ? uint.MaxValue : (value <= (Int256)uint.MinValue) ? uint.MinValue : (uint)value),
				ulong => (TOther)(object)((value >= (Int256)ulong.MaxValue) ? ulong.MaxValue : (value <= (Int256)ulong.MinValue) ? ulong.MinValue : (ulong)value),
				UInt128 => (TOther)(object)((value >= (Int256)UInt128.MaxValue) ? UInt128.MaxValue : (value <= (Int256)UInt128.MinValue) ? UInt128.MinValue : (UInt128)value),
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)((value >= (Int256)nuint.MaxValue) ? nuint.MaxValue : (value <= (Int256)nuint.MinValue) ? nuint.MinValue : (nuint)value),
				sbyte => (TOther)(object)((value >= (Int256)sbyte.MaxValue) ? sbyte.MaxValue : (value <= (Int256)sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
				short => (TOther)(object)((value >= (Int256)short.MaxValue) ? short.MaxValue : (value <= (Int256)short.MinValue) ? short.MinValue : (short)value),
				int => (TOther)(object)((value >= (Int256)int.MaxValue) ? int.MaxValue : (value <= (Int256)int.MinValue) ? int.MinValue : (int)value),
				long => (TOther)(object)((value >= (Int256)long.MaxValue) ? long.MaxValue : (value <= (Int256)long.MinValue) ? long.MinValue : (long)value),
				Int128 => (TOther)(object)((value >= (Int256)Int128.MaxValue) ? Int128.MaxValue : (value <= (Int256)Int128.MinValue) ? Int128.MinValue : (Int128)value),
				Int256 => (TOther)(object)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)((value >= (Int256)nint.MaxValue) ? nint.MaxValue : (value <= (Int256)nint.MinValue) ? nint.MinValue : (nint)value),
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToTruncating<TOther>(Int256 value, out TOther result)
		{
			bool converted = true;
			result = default!;
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
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)(nuint)value,
				sbyte => (TOther)(object)(sbyte)value,
				short => (TOther)(object)(short)value,
				int => (TOther)(object)(int)value,
				long => (TOther)(object)(long)value,
				Int128 => (TOther)(object)(Int128)value,
				Int256 => (TOther)(object)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)(nint)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

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

		public static Int256 CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int256 result;

			if (value is Int256 v)
			{
				result = v;
			}
			else if (!Int256.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<Int256>(value, out result))
			{
				Thrower.NotSupported<Int256, TOther>();
			}

			return result;
		}

		public static Int256 CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int256 result;

			if (value is Int256 v)
			{
				result = v;
			}
			else if (!Int256.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<Int256>(value, out result))
			{
				Thrower.NotSupported<Int256, TOther>();
			}

			return result;
		}

		public static Int256 CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int256 result;

			if (value is Int256 v)
			{
				result = v;
			}
			else if (!Int256.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<Int256>(value, out result))
			{
				Thrower.NotSupported<Int256, TOther>();
			}

			return result;
		}

		public bool Equals(Int256 other)
		{
			return this == other;
		}

		public int GetByteCount()
		{
			return Size;
		}

		public int GetShortestBitLength()
		{
			Int256 value = this;

			if (IsPositive(value))
			{
				return (Size * 8) - BitHelper.LeadingZeroCount(value);
			}
			else
			{
				return (Size * 8) + 1 - BitHelper.LeadingZeroCount(~value);
			}
		}

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FormatSignedInteger<Int256, UInt256>(in this, format, NumberStyles.Integer, formatProvider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatSignedInteger<Int256, UInt256, Utf16Char>(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

#if NET8_0_OR_GREATER
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatSignedInteger<Int256, UInt256, Utf8Char>(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
		} 
#endif

		bool IBinaryInteger<Int256>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
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

		bool IBinaryInteger<Int256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
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

		UInt256 IFormattableSignedInteger<Int256, UInt256>.ToUnsigned()
		{
			return (UInt256)this;
		}

		char IFormattableInteger<Int256>.ToChar()
		{
			return (char)this;
		}

		int IFormattableInteger<Int256>.ToInt32()
		{
			return (int)this;
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

		public static Int256 operator +(Int256 value)
		{
			return value;
		}

		public static Int256 operator +(Int256 left, Int256 right)
		{
			// For unsigned addition, we can detect overflow by checking `(x + y) < x`
			// This gives us the carry to add to upper to compute the correct result

			UInt128 lower = left._lower + right._lower;
			UInt128 carry = (lower < left._lower) ? 1UL : 0UL;

			UInt128 upper = left._upper + right._upper + carry;
			return new Int256(upper, lower);
		}
		public static Int256 operator checked +(Int256 left, Int256 right)
		{
			// For signed addition, we can detect overflow by checking if the sign of
			// both inputs are the same and then if that differs from the sign of the
			// output.

			Int256 result = left + right;

			ulong sign = (ulong)(left._upper >> 127);

			if (sign == (ulong)(right._upper >> 127) && 
				sign != (ulong)(result._upper >> 127))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Addition);
			}
			return result;
		}

		public static Int256 operator -(Int256 value)
		{
			return Zero - value;
		}
		public static Int256 operator checked -(Int256 value)
		{
			return checked(Zero - value);
		}

		public static Int256 operator -(Int256 left, Int256 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			UInt128 lower = left._lower - right._lower;
			UInt128 borrow = (lower > left._lower) ? UInt128.One : UInt128.Zero;

			UInt128 upper = left._upper - right._upper - borrow;
			return new Int256(upper, lower);
		}
		public static Int256 operator checked -(Int256 left, Int256 right)
		{
			// For signed subtraction, we can detect overflow by checking if the sign of
			// both inputs are different and then if that differs from the sign of the
			// output.

			Int256 result = left - right;

			uint sign = (uint)(left._upper >> 127);

			if (sign != (uint)(right._upper >> 127) && sign != (uint)(result._upper >> 127))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Subtraction);
			}
			return result;
		}

		public static Int256 operator ~(Int256 value)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in value));
				return Unsafe.As<Vector256<ulong>, Int256>(ref Unsafe.AsRef(~v));
			}
			else
			{
				return new(~value._upper, ~value._lower);
			}
		}

		public static Int256 operator ++(Int256 value)
		{
			return value + One;
		}
		public static Int256 operator checked ++(Int256 value)
		{
			return checked(value + One);
		}

		public static Int256 operator --(Int256 value)
		{
			return value - One;
		}
		public static Int256 operator checked --(Int256 value)
		{
			return checked(value - One);
		}

		public static Int256 operator *(Int256 left, Int256 right)
		{
			return (Int256)((UInt256)(left) * (UInt256)(right));
		}
		public static Int256 operator checked *(Int256 left, Int256 right)
		{
			Int256 upper = BigMul(left, right, out Int256 lower);

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

		public static Int256 operator /(Int256 left, Int256 right)
		{
			if ((right == NegativeOne) && (left._upper == _upperMin) && (left._lower == _lowerMin))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Division);
			}

			// We simplify the logic here by just doing unsigned division on the
			// two's complement representation and then taking the correct sign.

			UInt128 sign = (left._upper ^ right._upper) & (UInt128.One << 127);

			if (IsNegative(left))
			{
				left = ~left + One;
			}

			if (IsNegative(right))
			{
				right = ~right + One;
			}

			UInt256 result = (UInt256)(left) / (UInt256)(right);

			if (sign != 0)
			{
				result = ~result + 1U;
			}

			return new Int256(
				result.Upper,
				result.Lower
			);
		}

		public static Int256 operator %(Int256 left, Int256 right)
		{
			Int256 quotient = left / right;
			return left - (quotient * right);
		}

		public static Int256 operator &(Int256 left, Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, Int256>(ref Unsafe.AsRef(v1 & v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.And(v1, v2);
				return Unsafe.As<Vector256<ulong>, Int256>(ref result);
			}
			else
			{
				return new(left._upper & right._upper, left._lower & right._lower);
			}
		}

		public static Int256 operator |(Int256 left, Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, Int256>(ref Unsafe.AsRef(v1 | v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.Or(v1, v2);
				return Unsafe.As<Vector256<ulong>, Int256>(ref result);
			}
			else
			{
				return new(left._upper | right._upper, left._lower | right._lower);
			}
		}

		public static Int256 operator ^(Int256 left, Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return Unsafe.As<Vector256<ulong>, Int256>(ref Unsafe.AsRef(v1 ^ v2));
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				var result = Avx2.Xor(v1, v2);
				return Unsafe.As<Vector256<ulong>, Int256>(ref result);
			}
			else
			{
				return new(left._upper ^ right._upper, left._lower ^ right._lower);
			}
		}

		public static Int256 operator <<(Int256 value, int shiftAmount)
		{
			shiftAmount &= 0xFF;

			if ((shiftAmount & 0x80) != 0)
			{
				// In the case it is set, we know the entire lower bits must be zero
				// and so the upper bits are just the lower shifted by the remaining
				// masked amount

				UInt128 upper = value._lower << shiftAmount;
				return new Int256(upper, 0);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of lower

				UInt128 lower = value._lower << shiftAmount;
				UInt128 upper = (value._upper << shiftAmount) | (value._lower >> (128 - shiftAmount));

				return new Int256(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static Int256 operator >>(Int256 value, int shiftAmount)
		{
			// need to specially handle things if the 15th bit is set.

			shiftAmount &= 0xFF;

			if ((shiftAmount & 0x80) != 0)
			{
				// In the case it is set, we know the entire upper bits must be the sign
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt128 lower = (UInt128)((Int128)value._upper >> shiftAmount);
				UInt128 upper = (UInt128)((Int128)value._upper >> 127);

				return new Int256(upper, lower);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of upper

				UInt128 lower = (value._lower >> shiftAmount) | (value._upper << (128 - shiftAmount));
				UInt128 upper = (UInt128)((Int128)value._upper >> shiftAmount);

				return new Int256(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static Int256 operator >>>(Int256 value, int shiftAmount)
		{
			// We need to specially handle things if the 15th bit is set.

			shiftAmount &= 0xFF;

			if ((shiftAmount & 0x80) != 0)
			{
				// In the case it is set, we know the entire upper bits must be zero
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt128 lower = value._upper >> shiftAmount;
				return new Int256(0, lower);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of upper

				UInt128 lower = (value._lower >> shiftAmount) | (value._upper << (128 - shiftAmount));
				UInt128 upper = value._upper >> shiftAmount;

				return new Int256(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static bool operator ==(Int256 left, Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return v1 == v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<Int256, Vector256<byte>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<byte>>(ref Unsafe.AsRef(in right));
				var equals = Avx2.CompareEqual(v1, v2);
				var result = Avx2.MoveMask(equals);
				return (result & 0xFFFF_FFFF) == 0xFFFF_FFFF;
			}
			else
			{
				return (left._upper == right._upper) && (left._lower == right._lower);
			}
		}

		public static bool operator !=(Int256 left, Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<ulong>>(ref Unsafe.AsRef(in right));
				return v1 != v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.As<Int256, Vector256<byte>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<Int256, Vector256<byte>>(ref Unsafe.AsRef(in right));
				var equals = Avx2.CompareEqual(v1, v2);
				var result = Avx2.MoveMask(equals);
				return (result & 0xFFFF_FFFF) != 0xFFFF_FFFF;
			}
			else
			{
				return (left._upper != right._upper) || (left._lower != right._lower);
			}
		}

		public static bool operator <(Int256 left, Int256 right)
		{
			if (IsNegative(left) == IsNegative(right))
			{
				return (left._upper < right._upper)
					|| ((left._upper == right._upper) && (left._lower < right._lower));
			}
			else
			{
				return IsNegative(left);
			}
		}

		public static bool operator >(Int256 left, Int256 right)
		{
			if (IsNegative(left) == IsNegative(right))
			{
				return (left._upper > right._upper)
					|| ((left._upper == right._upper) && (left._lower > right._lower));
			}
			else
			{
				return IsNegative(right);
			}
		}

		public static bool operator <=(Int256 left, Int256 right)
		{
			if (IsNegative(left) == IsNegative(right))
			{
				return (left._upper < right._upper)
					|| ((left._upper == right._upper) && (left._lower <= right._lower));
			}
			else
			{
				return IsNegative(left);
			}
		}

		public static bool operator >=(Int256 left, Int256 right)
		{
			if (IsNegative(left) == IsNegative(right))
			{
				return (left._upper > right._upper)
					|| ((left._upper == right._upper) && (left._lower >= right._lower));
			}
			else
			{
				return IsNegative(right);
			}
		}
	}
}
