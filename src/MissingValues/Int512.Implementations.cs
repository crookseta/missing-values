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
	public partial struct Int512 :
		IBinaryInteger<Int512>,
		IMinMaxValue<Int512>,
		ISignedNumber<Int512>,
		IFormattableSignedInteger<Int512, UInt512>
	{
		private static UInt256 _upperMin => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		private static UInt256 _lowerMin => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		private static UInt256 _upperMax => new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static UInt256 _lowerMax => new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		static Int512 IBinaryNumber<Int512>.AllBitsSet => new Int512(_lowerMax, _lowerMax);

		public static Int512 One => new Int512(
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);

		static int INumberBase<Int512>.Radix => 2;

		public static Int512 Zero => default;

		static Int512 IAdditiveIdentity<Int512, Int512>.AdditiveIdentity => default;

		static Int512 IMultiplicativeIdentity<Int512, Int512>.MultiplicativeIdentity => One;

		public static Int512 MaxValue => new Int512(_upperMax, _lowerMax);

		public static Int512 MinValue => new Int512(_upperMin, _lowerMin);

		public static Int512 NegativeOne => new Int512(_lowerMax, _lowerMax);

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

		public static Int512 Abs(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;

				if (IsNegative(value))
				{
					Thrower.MinimumSignedAbsoluteValue<Int512>();
				}
			}
			return value;
		}

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

		public int CompareTo(Int512 other)
		{
			if (this < other) return -1;
			else if (this > other) return 1;
			else return 0;
		}

		public static Int512 CopySign(Int512 value, Int512 sign)
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
					Thrower.MinimumSignedAbsoluteValue<Int512>();
				}
				return absValue;
			}
			return -absValue;
		}

		public static (Int512 Quotient, Int512 Remainder) DivRem(Int512 left, Int512 right)
		{
			Int512 quotient = left / right;
			return (quotient, left - (quotient * right));
		}

		public bool Equals(Int512 other) => this == other;

		public int GetByteCount()
		{
			return Size;
		}

		public int GetShortestBitLength()
		{
			Int512 value = this;

			if (IsPositive(value))
			{
				return (Size * 8) - BitHelper.LeadingZeroCount(value);
			}
			else
			{
				return (Size * 8) + 1 - BitHelper.LeadingZeroCount(~value);
			}
		}

		static bool INumberBase<Int512>.IsCanonical(Int512 value) => true;

		static bool INumberBase<Int512>.IsComplexNumber(Int512 value) => false;

		public static bool IsEvenInteger(Int512 value) => (value._lower & 1) == UInt256.Zero;

		static bool INumberBase<Int512>.IsFinite(Int512 value) => true;

		static bool INumberBase<Int512>.IsImaginaryNumber(Int512 value) => false;

		static bool INumberBase<Int512>.IsInfinity(Int512 value) => false;

		static bool INumberBase<Int512>.IsInteger(Int512 value) => true;

		static bool INumberBase<Int512>.IsNaN(Int512 value) => false;

		public static bool IsNegative(Int512 value) => (Int256)value._upper < Int256.Zero;

		static bool INumberBase<Int512>.IsNegativeInfinity(Int512 value) => false;

		static bool INumberBase<Int512>.IsNormal(Int512 value) => value != Zero;

		public static bool IsOddInteger(Int512 value) => (value._lower & 1) != 0;

		public static bool IsPositive(Int512 value) => (Int256)value._upper >= Int256.Zero;

		static bool INumberBase<Int512>.IsPositiveInfinity(Int512 value) => false;

		public static bool IsPow2(Int512 value) => (PopCount(value) == One) && IsPositive(value);

		static bool INumberBase<Int512>.IsRealNumber(Int512 value) => true;

		static bool INumberBase<Int512>.IsSubnormal(Int512 value) => false;

		static bool INumberBase<Int512>.IsZero(Int512 value) => (value == Zero);

		public static Int512 LeadingZeroCount(Int512 value)
		{
			if (value._upper == 0)
			{
				return (Int512)(256 + UInt256.LeadingZeroCount(value._lower));
			}
			return (Int512)UInt256.LeadingZeroCount(value._upper);
		}

		public static Int512 Log2(Int512 value)
		{
			if (IsNegative(value))
			{
				Thrower.NeedsNonNegative<Int512>();
			}

			if (value._upper == 0)
			{
				return (Int512)UInt256.Log2(value._lower);
			}
			return (Int512)(256 + UInt256.Log2(value._upper));
		}

		public static Int512 Max(Int512 x, Int512 y) => (x >= y) ? x : y;

		static Int512 INumber<Int512>.MaxNumber(Int512 x, Int512 y) => Max(x, y);

		public static Int512 MaxMagnitude(Int512 x, Int512 y)
		{
			Int512 absX = x;

			if (IsNegative(absX))
			{
				absX = -absX;

				if (IsNegative(absX))
				{
					return x;
				}
			}

			Int512 absY = y;

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

		static Int512 INumberBase<Int512>.MaxMagnitudeNumber(Int512 x, Int512 y) => MaxMagnitude(x, y);

		public static Int512 Min(Int512 x, Int512 y) => (x <= y) ? x : y;

		static Int512 INumber<Int512>.MinNumber(Int512 x, Int512 y) => Min(x, y);

		public static Int512 MinMagnitude(Int512 x, Int512 y)
		{
			Int512 absX = x;

			if (IsNegative(absX))
			{
				absX = -absX;

				if (IsNegative(absX))
				{
					return y;
				}
			}

			Int512 absY = y;

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

		static Int512 INumberBase<Int512>.MinMagnitudeNumber(Int512 x, Int512 y) => MinMagnitude(x, y);

		public static Int512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			return NumberParser.ParseToSigned<Int512, UInt512>(s, style, provider);
		}

		public static Int512 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			return NumberParser.ParseToSigned<Int512, UInt512>(s, style, provider);
		}

		public static Int512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			return NumberParser.ParseToSigned<Int512, UInt512>(s, NumberStyles.Integer, provider);
		}

		public static Int512 Parse(string s, IFormatProvider? provider)
		{
			return NumberParser.ParseToSigned<Int512, UInt512>(s, NumberStyles.Integer, provider);
		}

		public static Int512 PopCount(Int512 value)
		{
			return (Int512)(UInt256.PopCount(value._lower) + UInt256.PopCount(value._upper));
		}

		public static Int512 RotateLeft(Int512 value, int rotateAmount)
		{
			return (value << rotateAmount) | (value >>> (512 - rotateAmount));
		}

		public static Int512 RotateRight(Int512 value, int rotateAmount)
		{
			return (value >>> rotateAmount) | (value << (512 - rotateAmount));
		}

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FormatSignedNumber<Int512, UInt512>(in this, format, NumberStyles.Integer, formatProvider);
		}

		public static Int512 TrailingZeroCount(Int512 value)
		{
			if (value._lower == 0)
			{
				return (Int512)(256 + UInt256.TrailingZeroCount(value._upper));
			}
			return (Int512)UInt256.TrailingZeroCount(value._lower);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatSignedNumber<Int512, UInt512>(in this, destination, out charsWritten, format, provider);
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512>(s, style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512>(s, style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512>(s, NumberStyles.Integer, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int512 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int512, UInt512>(s, NumberStyles.Integer, provider, out result);
		}

		public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int512 value)
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
						result = BitHelper.ReverseEndianness(result);
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

		public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int512 value)
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

		static Int512 IFormattableInteger<Int512>.GetDecimalValue(char value)
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

		static bool INumberBase<Int512>.TryConvertFromChecked<TOther>(TOther value, out Int512 result) => TryConvertFromSaturating(value, out result);
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
				Half actual => (Int512)actual,
				float actual => (Int512)actual,
				double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
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
				Half actual => (actual == Half.PositiveInfinity) ? MaxValue : (actual == Half.NegativeInfinity) ? MinValue : (Int512)actual,
				float actual => (actual == float.PositiveInfinity) ? MaxValue : (actual == float.NegativeInfinity) ? MinValue : (Int512)actual,
				double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
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
				_ => BitHelper.DefaultConvert<Int512>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToChecked<TOther>(Int512 value, out TOther result)
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
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToSaturating<TOther>(Int512 value, out TOther result)
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
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int512>.TryConvertToTruncating<TOther>(Int512 value, out TOther result)
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
				Int256 => (TOther)(object)(Int256)value,
				Int512 => (TOther)(object)value,
				nint => (TOther)(object)(nint)value,
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

		char IFormattableInteger<Int512>.ToChar()
		{
			return (char)this;
		}

		int IFormattableInteger<Int512>.ToInt32()
		{
			return (int)this;
		}

		UInt512 IFormattableSignedInteger<Int512, UInt512>.ToUnsigned()
		{
			return (UInt512)this;
		}

		public static Int512 operator +(Int512 value)
		{
			return value;
		}

		public static Int512 operator +(Int512 left, Int512 right)
		{
			// For unsigned addition, we can detect overflow by checking `(x + y) < x`
			// This gives us the carry to add to upper to compute the correct result

			UInt256 lower = left._lower + right._lower;
			UInt256 carry = (lower < left._lower) ? 1UL : 0UL;

			UInt256 upper = left._upper + right._upper + carry;
			return new Int512(upper, lower);
		}
		public static Int512 operator checked +(Int512 left, Int512 right)
		{
			Int512 result = left + right;

			ulong sign = (ulong)(left._upper >> 255);

			if (sign == (ulong)(right._upper >> 255) &&
				sign != (ulong)(result._upper >> 255))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Addition);
			}
			return result;
		}

		public static Int512 operator -(Int512 value)
		{
			return Zero - value;
		}
		public static Int512 operator checked -(Int512 value)
		{
			return checked(Zero - value);
		}

		public static Int512 operator -(Int512 left, Int512 right)
		{
			// For unsigned subtract, we can detect overflow by checking `(x - y) > x`
			// This gives us the borrow to subtract from upper to compute the correct result

			UInt256 lower = left._lower - right._lower;
			UInt256 borrow = (lower > left._lower) ? UInt256.One : UInt256.Zero;

			UInt256 upper = left._upper - right._upper - borrow;
			return new Int512(upper, lower);
		}
		public static Int512 operator checked -(Int512 left, Int512 right)
		{
			// For signed subtraction, we can detect overflow by checking if the sign of
			// both inputs are different and then if that differs from the sign of the
			// output.

			Int512 result = left - right;

			uint sign = (uint)(left._upper >> 255);

			if (sign != (uint)(right._upper >> 255) && sign != (uint)(result._upper >> 255))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Subtraction);
			}
			return result;
		}

		public static Int512 operator ~(Int512 value)
		{
			return new Int512(~value._upper, ~value._lower);
		}

		public static Int512 operator ++(Int512 value)
		{
			return value + One;
		}
		public static Int512 operator checked ++(Int512 value)
		{
			return checked(value + One);
		}

		public static Int512 operator --(Int512 value)
		{
			return value - One;
		}
		public static Int512 operator checked --(Int512 value)
		{
			return checked(value - One);
		}

		public static Int512 operator *(Int512 left, Int512 right)
		{
			return (Int512)((UInt512)left * (UInt512)right);
		}
		public static Int512 operator checked *(Int512 left, Int512 right)
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

		public static Int512 operator /(Int512 left, Int512 right)
		{
			if ((right == NegativeOne) && (left._upper == _upperMin) && (left._lower == _lowerMin))
			{
				Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Division);
			}

			// We simplify the logic here by just doing unsigned division on the
			// two's complement representation and then taking the correct sign.

			UInt256 sign = (left._upper ^ right._upper) & (UInt256.One << 255);

			if (IsNegative(left))
			{
				left = ~left + One;
			}

			if (IsNegative(right))
			{
				right = ~right + One;
			}

			UInt512 result = (UInt512)(left) / (UInt512)(right);

			if (sign != 0)
			{
				result = ~result + 1U;
			}

			return new Int512(
				result.Upper,
				result.Lower
			);
		}

		public static Int512 operator %(Int512 left, Int512 right)
		{
			Int512 quotient = left / right;
			return left - (quotient * right);
		}

		public static Int512 operator &(Int512 left, Int512 right)
		{
			return new Int512(left._upper & right._upper, left._lower & right._lower);
		}

		public static Int512 operator |(Int512 left, Int512 right)
		{
			return new Int512(left._upper | right._upper, left._lower | right._lower);
		}

		public static Int512 operator ^(Int512 left, Int512 right)
		{
			return new Int512(left._upper ^ right._upper, left._lower ^ right._lower);
		}

		public static Int512 operator <<(Int512 value, int shiftAmount)
		{
			shiftAmount &= 0x1FF;

			if ((shiftAmount & 0x100) != 0)
			{
				// In the case it is set, we know the entire lower bits must be zero
				// and so the upper bits are just the lower shifted by the remaining
				// masked amount

				UInt256 upper = value._lower << shiftAmount;
				return new Int512(upper, UInt256.Zero);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of lower

				UInt256 lower = value._lower << shiftAmount;
				UInt256 upper = (value._upper << shiftAmount) | (value._lower >> (256 - shiftAmount));

				return new Int512(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static Int512 operator >>(Int512 value, int shiftAmount)
		{
			shiftAmount &= 0x1FF;

			if ((shiftAmount & 0x100) != 0)
			{
				// In the case it is set, we know the entire upper bits must be the sign
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt256 lower = (UInt256)((Int256)value._upper >> shiftAmount);
				UInt256 upper = (UInt256)((Int256)value._upper >> 255);

				return new Int512(upper, lower);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of upper

				UInt256 lower = (value._lower >> shiftAmount) | (value._upper << (256 - shiftAmount));
				UInt256 upper = (UInt256)((Int256)value._upper >> shiftAmount);

				return new Int512(upper, lower);
			}
			else
			{
				return value;
			}
		}

		public static bool operator ==(Int512 left, Int512 right) => (left._lower == right._lower) && (left._upper == right._upper);

		public static bool operator !=(Int512 left, Int512 right) => (left._lower != right._lower) || (left._upper != right._upper);

		public static bool operator <(Int512 left, Int512 right)
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

		public static bool operator >(Int512 left, Int512 right)
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

		public static bool operator <=(Int512 left, Int512 right)
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

		public static bool operator >=(Int512 left, Int512 right)
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

		public static Int512 operator >>>(Int512 value, int shiftAmount)
		{
			shiftAmount &= 0x1FF;

			if ((shiftAmount & 0x100) != 0)
			{
				// In the case it is set, we know the entire upper bits must be zero
				// and so the lower bits are just the upper shifted by the remaining
				// masked amount

				UInt256 lower = value._upper >> shiftAmount;
				return new Int512(0, lower);
			}
			else if (shiftAmount != 0)
			{
				// Otherwise we need to shift both upper and lower halves by the masked
				// amount and then or that with whatever bits were shifted "out" of upper

				UInt256 lower = (value._lower >> shiftAmount) | (value._upper << (256 - shiftAmount));
				UInt256 upper = value._upper >> shiftAmount;

				return new Int512(upper, lower);
			}
			else
			{
				return value;
			}
		}
	}
}
