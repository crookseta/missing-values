﻿using MissingValues.Info;
using MissingValues.Internals;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace MissingValues
{
	public partial struct Int256 :
		IBigInteger<Int256>,
		IMinMaxValue<Int256>,
		ISignedNumber<Int256>,
		IPowerFunctions<Int256>,
		IFormattableSignedInteger<Int256>
	{
		private static UInt128 _upperMin => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		private static UInt128 _lowerMin => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		private static UInt128 _upperMax => new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		private static UInt128 _lowerMax => new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

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
			Int256 quotient = left / right;
			return (quotient, left - (quotient * right));
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
			if (!status)
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
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out Int256 output);
			if (!status)
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
			if (!status)
			{
				status.Throw<Int256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static Int256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out Int256 output);
			if (!status)
			{
				status.Throw<Int256>(utf8Text);
			}
			return output;
		}
		/// <inheritdoc/>
		public static Int256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out Int256 output);
			if (!status)
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

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf16Char>(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}
		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Int256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToSigned<Int256, UInt256, Utf8Char>(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}

		static bool IBinaryInteger<Int256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
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
						result = BitHelper.ReverseEndianness(in result);
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

		static bool IBinaryInteger<Int256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Int256 value)
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
						result = BitHelper.ReverseEndianness(in result);
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
					BigInteger actual => (Int256)actual,
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
				Int512 actual => (actual < MinValue) ? MinValue : (actual > MaxValue) ? MaxValue : (Int256)actual,
				nint actual => actual,
				BigInteger actual => (actual < (BigInteger)MinValue) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (Int256)actual,
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
				Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int256)actual,
				float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int256)actual,
				double actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
				decimal actual => (Int128)actual,
				byte actual => (Int256)actual,
				ushort actual => (Int256)actual,
				uint actual => (Int256)actual,
				ulong actual => (Int256)actual,
				UInt128 actual => (Int256)actual,
				UInt256 actual => (Int256)actual,
				nuint actual => (Int256)actual,
				sbyte actual => actual,
				short actual => actual,
				int actual => actual,
				long actual => actual,
				Int128 actual => actual,
				Int256 actual => actual,
				nint actual => actual,
				BigInteger actual => (Int256)actual,
				_ => BitHelper.DefaultConvert<Int256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToChecked<TOther>(Int256 value, out TOther result)
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
					BigInteger => (TOther)(object)(BigInteger)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToSaturating<TOther>(Int256 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;

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
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Int256>.TryConvertToTruncating<TOther>(Int256 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;
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
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public static Int256 CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Int256 result;

			if (value is Int256 v)
			{
				result = v;
			}
			else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result))
			{
				Thrower.NotSupported<Int256, TOther>();
			}

			return result;
		}

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
			ulong p0 = _p0;
			ulong p1 = _p1;
			ulong p2 = _p2;
			ulong p3 = _p3;

			if (BitConverter.IsLittleEndian)
			{
				p0 = BinaryPrimitives.ReverseEndianness(p0);
				p1 = BinaryPrimitives.ReverseEndianness(p1);
				p2 = BinaryPrimitives.ReverseEndianness(p2);
				p3 = BinaryPrimitives.ReverseEndianness(p3);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, p3);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong)), p2);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 2), p1);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 3), p0);
		}
		private void WriteLittleEndianUnsafe(Span<byte> destination)
		{
			Debug.Assert(destination.Length >= Size);

			ulong p0 = _p0;
			ulong p1 = _p1;
			ulong p2 = _p2;
			ulong p3 = _p3;

			if (!BitConverter.IsLittleEndian)
			{
				p0 = BinaryPrimitives.ReverseEndianness(p0);
				p1 = BinaryPrimitives.ReverseEndianness(p1);
				p2 = BinaryPrimitives.ReverseEndianness(p2);
				p3 = BinaryPrimitives.ReverseEndianness(p3);
			}

			ref byte address = ref MemoryMarshal.GetReference(destination);

			Unsafe.WriteUnaligned(ref address, p0);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong)), p1);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 2), p2);
			Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref address, sizeof(ulong) * 3), p3);
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

		/// <inheritdoc/>
		public static Int256 operator +(in Int256 value) => value;

		/// <inheritdoc/>
		public static Int256 operator +(in Int256 left, in Int256 right)
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

			return new Int256(part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static Int256 operator checked +(in Int256 left, in Int256 right)
		{
			// For signed addition, we can detect overflow by checking if the sign of
			// both inputs are the same and then if that differs from the sign of the
			// output.

			Int256 result = left + right;

			uint sign = (uint)(left._p3 >> 63);

			if ((long)((result._p3 ^ left._p3) & ~(left._p3 ^ right._p3)) < 0)
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Addition);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Int256 operator -(in Int256 value) => Zero - value;
		/// <inheritdoc/>
		public static Int256 operator checked -(in Int256 value) => checked(Zero - value);

		/// <inheritdoc/>
		public static Int256 operator -(in Int256 left, in Int256 right)
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

			return new Int256(part3, part2, part1, part0);
		}
		/// <inheritdoc/>
		public static Int256 operator checked -(in Int256 left, in Int256 right)
		{
			// For signed subtraction, we can detect overflow by checking if the sign of
			// both inputs are different and then if that differs from the sign of the
			// output.

			Int256 result = left - right;

			uint sign = (uint)(left._p3 >> 63);

			if (sign != (uint)(right._p3 >> 63) && sign != (uint)(result._p3 >> 63))
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Subtraction);
			}
			return result;
		}

		/// <inheritdoc/>
		public static Int256 operator ~(in Int256 value)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v = Unsafe.BitCast<Int256, Vector256<ulong>>(value);
				var result = ~v;
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else
			{
				return new(~value._p3, ~value._p2, ~value._p1, ~value._p0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator ++(in Int256 value) => value + One;
		/// <inheritdoc/>
		public static Int256 operator checked ++(in Int256 value) => checked(value + One);

		/// <inheritdoc/>
		public static Int256 operator --(in Int256 value) => value - One;
		/// <inheritdoc/>
		public static Int256 operator checked --(in Int256 value) => checked(value - One);

		/// <inheritdoc/>
		public static Int256 operator *(in Int256 left, in Int256 right) => (Int256)((UInt256)(left) * (UInt256)(right));
		/// <inheritdoc/>
		public static Int256 operator checked *(in Int256 left, in Int256 right)
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

				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Multiplication);
			}

			return lower;
		}

		/// <inheritdoc/>
		public static Int256 operator /(in Int256 left, in Int256 right)
		{
			if ((right == NegativeOne) && (left.Upper == _upperMin) && (left.Lower == _lowerMin))
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Division);
			}

			// We simplify the logic here by just doing unsigned division on the
			// two's complement representation and then taking the correct sign.

			ulong sign = (left._p3 ^ right._p3) & (1UL << 63);

			Int256 a = left, b = right;

			if ((long)left._p3 < 0)
			{
				a = ~left + One;
			}

			if ((long)right._p3 < 0)
			{
				b = ~right + One;
			}

			UInt256 result = (UInt256)(a) / (UInt256)(b);

			if (sign != 0)
			{
				result = ~result + UInt256.One;
			}

			return unchecked((Int256)result);
		}

		/// <inheritdoc/>
		public static Int256 operator %(in Int256 left, in Int256 right)
		{
			Int256 quotient = left / right;
			return left - (quotient * right);
		}

		/// <inheritdoc/>
		public static Int256 operator &(in Int256 left, in Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = v1 & v2;
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = Avx2.And(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else
			{
				return new(left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator |(in Int256 left, in Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = v1 | v2;
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = Avx2.Or(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else
			{
				return new(left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator ^(in Int256 left, in Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = v1 ^ v2;
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				var result = Avx2.Xor(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, Int256>(result);
			}
			else
			{
				return new(left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator <<(in Int256 value, int shiftAmount)
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

				return new Int256(part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int256(value._p2, value._p1, value._p0, 0);
				}

				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int256(part2, part1, part0, 0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int256(value._p1, value._p0, 0, 0);
				}

				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new Int256(part1, part0, 0, 0);
			}
			else // shiftAmount < 256
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new Int256(value._p0, 0, 0, 0);
				}

				ulong part0 = value._p0 << shiftAmount;

				return new Int256(part0, 0, 0, 0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator >>(in Int256 value, int shiftAmount)
		{
			// need to specially handle things if the 15th bit is set.

			shiftAmount &= 0xFF;

			if (shiftAmount == 0)
			{
				return value;
			}

			if (shiftAmount < 64)
			{
				ulong part0 = (value._p0 >> shiftAmount) | (value._p1 << (64 - shiftAmount));
				ulong part1 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part2 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part3 = (ulong)((long)value._p3 >> shiftAmount);

				return new Int256(part3, part2, part1, part0);
			}

			ulong preservedSign = (ulong)((long)value._p3 >> 63);

			if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int256(preservedSign, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = (ulong)((long)value._p3 >> shiftAmount);

				return new Int256(preservedSign, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int256(preservedSign, preservedSign, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = (ulong)((long)value._p3 >> shiftAmount);

				return new Int256(preservedSign, preservedSign, part1, part0);
			}
			else // shiftAmount < 256
			{
				shiftAmount -= 192;

				ulong part0 = (ulong)((long)value._p3 >> shiftAmount);

				return new Int256(preservedSign, preservedSign, preservedSign, part0);
			}
		}

		/// <inheritdoc/>
		public static Int256 operator >>>(in Int256 value, int shiftAmount)
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

				return new Int256(part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new Int256(0, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = value._p3 >> shiftAmount;

				return new Int256(0, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new Int256(0, 0, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = value._p3 >> shiftAmount;

				return new Int256(0, 0, part1, part0);
			}
			else // shiftAmount < 256
			{
				shiftAmount -= 192;

				ulong part0 = value._p3 >> shiftAmount;

				return new Int256(0, 0, 0, part0);
			}
		}

		/// <inheritdoc/>
		public static bool operator ==(in Int256 left, in Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				return v1 == v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<byte>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<byte>>(right);
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
		public static bool operator !=(in Int256 left, in Int256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<ulong>>(right);
				return v1 != v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<Int256, Vector256<byte>>(left);
				var v2 = Unsafe.BitCast<Int256, Vector256<byte>>(right);
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
		public static bool operator <(in Int256 left, in Int256 right)
		{
			// Successively compare each part.
			// If left and right have different signs: Signed comparison of _p3 gives result since it is stored as two's complement
			// If signs are equal and left._p3 < right._p3: left < right for negative and positive values,
			//                                                    since _p3 is upper 64 bits in two's complement.
			// If signs are equal and left._p3 > right._p3: left > right for negative and positive values,
			//                                                    since _p3 is upper 64 bits in two's complement.
			// If left._p3 == right._p3: unsigned comparison of lower bits gives the result for both negative and positive values since
			//                                 lower values are lower 64 bits in two's complement.
			return ((long)left._p3 < (long)right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 < right._p0))))));
		}

		/// <inheritdoc/>
		public static bool operator >(in Int256 left, in Int256 right)
		{
			return ((long)left._p3 > (long)right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 > right._p0))))));
		}

		/// <inheritdoc/>
		public static bool operator <=(in Int256 left, in Int256 right)
		{
			return ((long)left._p3 < (long)right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 <= right._p0))))));
		}

		/// <inheritdoc/>
		public static bool operator >=(in Int256 left, in Int256 right)
		{
			return ((long)left._p3 > (long)right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 >= right._p0))))));
		}
	}
}
