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
using System.Runtime.Intrinsics.X86;

namespace MissingValues
{
	public readonly partial struct UInt256 :
		IBigInteger<UInt256>,
		IMinMaxValue<UInt256>,
		IUnsignedNumber<UInt256>,
		IPowerFunctions<UInt256>,
		IFormattableUnsignedInteger<UInt256>
	{
		static UInt256 INumberBase<UInt256>.One => One;

		static int INumberBase<UInt256>.Radix => 2;

		static UInt256 INumberBase<UInt256>.Zero => Zero;

		static UInt256 IAdditiveIdentity<UInt256, UInt256>.AdditiveIdentity => Zero;

		static UInt256 IMultiplicativeIdentity<UInt256, UInt256>.MultiplicativeIdentity => One;

		// 115792089237316195423570985008687907853269984665640564039457584007913129639935
		static UInt256 IMinMaxValue<UInt256>.MaxValue => MaxValue;

		static UInt256 IMinMaxValue<UInt256>.MinValue => MinValue;

		static UInt256 IFormattableInteger<UInt256>.Two => new(0x2);

		static UInt256 IFormattableInteger<UInt256>.Sixteen => new(0x10);

		static UInt256 IFormattableInteger<UInt256>.Ten => new(0xA);

		static char IFormattableInteger<UInt256>.LastDecimalDigitOfMaxValue => '1';

		static int IFormattableInteger<UInt256>.MaxDecimalDigits => 78;

		static int IFormattableInteger<UInt256>.MaxHexDigits => 64;

		static int IFormattableInteger<UInt256>.MaxBinaryDigits => 256;

		static UInt256 IFormattableUnsignedInteger<UInt256>.SignedMaxMagnitude => new(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		static UInt256 IFormattableInteger<UInt256>.TwoPow2 => new(4);

		static UInt256 IFormattableInteger<UInt256>.SixteenPow2 => new(256);

		static UInt256 IFormattableInteger<UInt256>.TenPow2 => new(100);

		static UInt256 IFormattableInteger<UInt256>.TwoPow3 => new(8);

		static UInt256 IFormattableInteger<UInt256>.SixteenPow3 => new(4096);

		static UInt256 IFormattableInteger<UInt256>.TenPow3 => new(1000);

		static UInt256 IFormattableInteger<UInt256>.E19 => new UInt256(0, 0, 0, 10000000000000000000UL);

		static UInt256 INumberBase<UInt256>.Abs(UInt256 value) => value;

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public int CompareTo(UInt256 other)
		{
			if (this < other) return -1;
			else if (this > other) return 1;
			else return 0;
		}

		/// <inheritdoc/>
		public int CompareTo(object? obj)
		{
			if (obj is UInt256 value)
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public static (UInt256 Quotient, UInt256 Remainder) DivRem(UInt256 left, UInt256 right)
		{
			DivRem(in left, in right, out UInt256 quotient, out UInt256 remainder);

			return (quotient, remainder);
		}
		internal static void DivRem(in UInt256 left, in UInt256 right, out UInt256 quotient, out UInt256 remainder)
		{
			const int UIntCount = Size / sizeof(ulong);

			if (right._p3 == 0 && right._p2 == 0)
			{
				if (right._p1 == 0)
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
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(quotientSpan)), left);

			Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(divisorSpan)), right);

			Span<ulong> quoBits = stackalloc ulong[UIntCount];
			quoBits.Clear();
			Span<ulong> remBits = stackalloc ulong[UIntCount];
			remBits.Clear();

			Calculator.DivRem(
				quotientSpan[..BitHelper.GetTrimLength(in left)],
				divisorSpan[..BitHelper.GetTrimLength(in right)],
				quoBits,
				remBits);

			quotient = Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(quoBits)));
			remainder = Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(remBits)));
		}

		/// <inheritdoc/>
		public bool Equals(UInt256 other) => this == other;

		static bool INumberBase<UInt256>.IsCanonical(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsComplexNumber(UInt256 value) => false;

		/// <inheritdoc/>
		public static bool IsEvenInteger(UInt256 value) => (value._p0 & 1) == 0;

		static bool INumberBase<UInt256>.IsFinite(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsImaginaryNumber(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsInteger(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsNaN(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegative(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNegativeInfinity(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsNormal(UInt256 value) => value != Zero;

		/// <inheritdoc/>
		public static bool IsOddInteger(UInt256 value) => (value._p0 & 1) != 0;

		static bool INumberBase<UInt256>.IsPositive(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsPositiveInfinity(UInt256 value) => false;

		/// <inheritdoc/>
		public static bool IsPow2(UInt256 value) => BitHelper.PopCount(in value) == 1;

		static bool INumberBase<UInt256>.IsRealNumber(UInt256 value) => true;

		static bool INumberBase<UInt256>.IsSubnormal(UInt256 value) => false;

		static bool INumberBase<UInt256>.IsZero(UInt256 value) => value == Zero;

		/// <inheritdoc/>
		public static UInt256 LeadingZeroCount(UInt256 value) => (UInt256)BitHelper.LeadingZeroCount(in value);

		/// <inheritdoc/>
		public static UInt256 Log2(UInt256 value) => (UInt256)BitHelper.Log2(in value);

		/// <inheritdoc/>
		public static UInt256 Max(UInt256 x, UInt256 y) => (x >= y) ? x : y;

		static UInt256 INumber<UInt256>.MaxNumber(UInt256 x, UInt256 y) => Max(x, y);

		static UInt256 INumberBase<UInt256>.MaxMagnitude(UInt256 x, UInt256 y) => Max(x, y);

		static UInt256 INumberBase<UInt256>.MaxMagnitudeNumber(UInt256 x, UInt256 y) => Max(x, y);

#if NET9_0_OR_GREATER
		static UInt256 INumberBase<UInt256>.MultiplyAddEstimate(UInt256 left, UInt256 right, UInt256 addend) => (left * right) + addend; 
#endif

		/// <inheritdoc/>
		public static UInt256 Min(UInt256 x, UInt256 y) => (x <= y) ? x : y;

		static UInt256 INumber<UInt256>.MinNumber(UInt256 x, UInt256 y) => Min(x, y);

		static UInt256 INumberBase<UInt256>.MinMagnitude(UInt256 x, UInt256 y) => Min(x, y);

		static UInt256 INumberBase<UInt256>.MinMagnitudeNumber(UInt256 x, UInt256 y) => Min(x, y);

		/// <inheritdoc/>
		public static UInt256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt256 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt256 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt256 Parse(string s, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(utf8Text);
			}
			return output;
		}
		/// <inheritdoc/>
		public static UInt256 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out UInt256 output);
			if (!status)
			{
				status.Throw<UInt256>(utf8Text);
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt256 PopCount(UInt256 value) => (UInt256)BitHelper.PopCount(in value);

		static UInt256 IPowerFunctions<UInt256>.Pow(UInt256 x, UInt256 y) => Pow(x, checked((int)y));

		/// <inheritdoc/>
		public static UInt256 RotateLeft(UInt256 value, int rotateAmount) => (value << rotateAmount) | (value >>> (256 - rotateAmount));

		/// <inheritdoc/>
		public static UInt256 RotateRight(UInt256 value, int rotateAmount) => (value >>> rotateAmount) | (value << (256 - rotateAmount));

		/// <inheritdoc/>
		public static UInt256 TrailingZeroCount(UInt256 value) => (UInt256)BitHelper.TrailingZeroCount(in value);

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (s.Length == 0 || s.IsWhiteSpace())
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out result);
		}

		/// <inheritdoc/>
		public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out UInt256 result)
		{
			if (utf8Text.Length == 0 || !utf8Text.ContainsAnyExcept((byte)' '))
			{
				result = default;
				return false;
			}

			return NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out result);
		}

		static bool IBinaryInteger<UInt256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
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
				}
			}

			value = result;
			return true;
		}

		static bool IBinaryInteger<UInt256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt256 value)
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
					BigInteger actual => (UInt256)actual,
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
				BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (UInt256)actual,
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
				BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (UInt256)actual,
				_ => BitHelper.DefaultConvert<UInt256>(out converted)
			};
			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertToChecked<TOther>(UInt256 value, out TOther result)
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
					BigInteger => (TOther)(object)(BigInteger)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertToSaturating<TOther>(UInt256 value, out TOther result)
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
				byte => (TOther)(object)((value >= 0xFF) ? byte.MaxValue : (byte)value),
				ushort => (TOther)(object)((value >= 0xFFFF) ? ushort.MaxValue : (ushort)value),
				uint => (TOther)(object)((value >= 0xFFFF_FFFF) ? uint.MaxValue : (uint)value),
				ulong => (TOther)(object)((value >= 0xFFFF_FFFF_FFFF_FFFF) ? ulong.MaxValue : (ulong)value),
				UInt128 => (TOther)(object)((value >= new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? UInt128.MaxValue : (UInt128)value),
				UInt256 => (TOther)(object)value,
				UInt512 => (TOther)(object)(UInt512)value,
#if TARGET_32BIT
				nuint => (TOther)(object)((value >= 0xFFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#else
				nuint => (TOther)(object)((value >= 0xFFFF_FFFF_FFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#endif
				sbyte => (TOther)(object)((value >= 0x7F) ? sbyte.MaxValue : (sbyte)value),
				short => (TOther)(object)((value >= 0x7FFF) ? short.MaxValue : (short)value),
				int => (TOther)(object)((value >= 0x7FFF_FFFF) ? int.MaxValue : (int)value),
				long => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? long.MaxValue : (long)value),
				Int128 => (TOther)(object)((value >= new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int128.MaxValue : (Int128)value),
				Int256 => (TOther)(object)((value >= new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int256.MaxValue : (Int256)value),
				Int512 => (TOther)(object)(Int512)value,
#if TARGET_32BIT
				nint => (TOther)(object)((value >= 0x7FFF_FFFF) ? nint.MaxValue : (nint)value),
#else
				nint => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? nint.MaxValue : (nint)value),
#endif
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		static bool INumberBase<UInt256>.TryConvertToTruncating<TOther>(UInt256 value, out TOther result)
		{
			bool converted = true;
			result = TOther.Zero;
			unchecked
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
					BigInteger => (TOther)(object)(BigInteger)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}
			return converted;
		}

		int IBinaryInteger<UInt256>.GetByteCount() => Size;

		int IBinaryInteger<UInt256>.GetShortestBitLength()
		{
			UInt256 value = this;
			return (Size * 8) - BitHelper.LeadingZeroCount(in value);
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
		static int IFormattableUnsignedInteger<UInt256>.CountDigits(in UInt256 value) => CountDigits(in value);
		static int IFormattableInteger<UInt256>.UnsignedCompare(in UInt256 value1, in UInt256 value2)
		{
			if (value1 < value2) return -1;
			else if (value1 > value2) return 1;
			else return 0;
		}
		static int IFormattableInteger<UInt256>.Log2Int32(in UInt256 value) => BitHelper.Log2(in value);
		static int IFormattableInteger<UInt256>.LeadingZeroCountInt32(in UInt256 value) => BitHelper.LeadingZeroCount(in value);
		static void IFormattableUnsignedInteger<UInt256>.ToDecChars<TChar>(in UInt256 number, Span<TChar> destination, int digits) => NumberFormatter.UInt256ToDecChars(number, destination, digits);

		/// <inheritdoc/>
		public static UInt256 operator +(in UInt256 value) => value;

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
		public static UInt256 operator -(in UInt256 value) => Zero - value;
		/// <inheritdoc/>
		public static UInt256 operator checked -(in UInt256 value) => checked(Zero - value);

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
				var v = Vector256.OnesComplement(Unsafe.BitCast<UInt256, Vector256<ulong>>(value));
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(v);
			}
			else
			{
				return new(~value._p3, ~value._p2, ~value._p1, ~value._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt256 operator ++(in UInt256 value) => value + One;
		/// <inheritdoc/>
		public static UInt256 operator checked ++(in UInt256 value) => checked(value + One);

		/// <inheritdoc/>
		public static UInt256 operator --(in UInt256 value) => value - One;
		/// <inheritdoc/>
		public static UInt256 operator checked --(in UInt256 value) => checked(value - One);

		/// <inheritdoc/>
		public static UInt256 operator *(in UInt256 left, in UInt256 right)
		{
			if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
			{
				if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					ulong up = Math.BigMul(left._p0, right._p0, out ulong low);
					return new UInt256(0, 0, up, low);
				}

				return Calculator.Multiply(in left, right._p0, out _);
			}
			else if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
			{
				return Calculator.Multiply(in right, left._p0, out _);
			}

			(ulong hcarry, ulong lcarry) = Calculator.BigMulAdd(left._p0, right._p0, 0);
			ulong p0 = lcarry;
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p1, right._p0, hcarry);
			ulong p1 = lcarry;
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p2, right._p0, hcarry);
			ulong p2 = lcarry;
			(_, lcarry) = Calculator.BigMulAdd(left._p3, right._p0, hcarry);
			ulong p3 = lcarry;
        
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p0, right._p1, 0);
			p1 = Calculator.AddWithCarry(p1, lcarry, out ulong carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p1, right._p1, hcarry);
			p2 = Calculator.AddWithCarry(p2, lcarry, out carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(_, lcarry) = Calculator.BigMulAdd(left._p2, right._p1, hcarry);
			p3 += lcarry;

			(hcarry, lcarry) = Calculator.BigMulAdd(left._p0, right._p2, 0);
			p2 = Calculator.AddWithCarry(p2, lcarry, out carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(_, lcarry) = Calculator.BigMulAdd(left._p1, right._p2, hcarry);
			p3 += lcarry;
        
			(_, lcarry) = Calculator.BigMulAdd(left._p0, right._p3, 0);
			p3 += lcarry;
        
			return new UInt256(p3, p2, p1, p0);
		}
		/// <inheritdoc/>
		public static UInt256 operator checked *(in UInt256 left, in UInt256 right)
		{
			ulong carry;
			
			if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
			{
				if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					ulong up = Math.BigMul(left._p0, right._p0, out ulong low);
					return new UInt256(0, 0, up, low);
				}

				UInt256 lower = Calculator.Multiply(in left, right._p0, out carry);

				if (carry != 0)
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
				}

				return lower;
			}
			else if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
			{
				UInt256 lower = Calculator.Multiply(in right, left._p0, out carry);

				if (carry != 0)
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
				}

				return lower;
			}

			(ulong hcarry, ulong lcarry) = Calculator.BigMulAdd(left._p0, right._p0, 0);
			ulong p0 = lcarry;
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p1, right._p0, hcarry);
			ulong p1 = lcarry;
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p2, right._p0, hcarry);
			ulong p2 = lcarry;
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p3, right._p0, hcarry);
			ulong p3 = lcarry;

			if (hcarry != 0) Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
        
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p0, right._p1, 0);
			p1 = Calculator.AddWithCarry(p1, lcarry, out carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p1, right._p1, hcarry);
			p2 = Calculator.AddWithCarry(p2, lcarry, out carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p2, right._p1, hcarry);
			p3 = Calculator.AddWithCarry(p3, lcarry, out carry);
			
			if (hcarry != 0 || carry != 0) Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);

			(hcarry, lcarry) = Calculator.BigMulAdd(left._p0, right._p2, 0);
			p2 = Calculator.AddWithCarry(p2, lcarry, out carry);
			hcarry = Calculator.AddWithCarry(hcarry, carry, out carry);
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p1, right._p2, hcarry);
			p3 = Calculator.AddWithCarry(p3, lcarry, out carry);
			
			if (hcarry != 0 || carry != 0) Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
        
			(hcarry, lcarry) = Calculator.BigMulAdd(left._p0, right._p3, 0);
			p3 = Calculator.AddWithCarry(p3, lcarry, out carry);
			
			if (hcarry != 0 || carry != 0) Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
        
			return new UInt256(p3, p2, p1, p0);
		}

		/// <inheritdoc/>
		public static UInt256 operator /(in UInt256 left, in UInt256 right)
		{
			const int UIntCount = Size / sizeof(ulong);

			if (right._p3 == 0 && right._p2 == 0)
			{
				if (right._p1 == 0)
				{
					if (right._p0 == 0)
					{
						Thrower.DivideByZero();
					}
					return Calculator.Divide(in left, right._p0);
				}
			}

			if (right >= left)
			{
				return (right == left) ? One : Zero;
			}

			Span<ulong> quotientSpan = stackalloc ulong[UIntCount];
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(quotientSpan)), left);

			Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(divisorSpan)), right);

			Span<ulong> rawBits = stackalloc ulong[UIntCount];
			rawBits.Clear();

			Calculator.Divide(
				quotientSpan[..BitHelper.GetTrimLength(in left)],
				divisorSpan[..BitHelper.GetTrimLength(in right)],
				rawBits);

			return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(rawBits)));
		}

		/// <inheritdoc/>
		public static UInt256 operator %(in UInt256 left, in UInt256 right)
		{
			const int UIntCount = Size / sizeof(ulong);

			if (right._p3 == 0 && right._p2 == 0)
			{
				if (right._p1 == 0)
				{
					if (right._p0 == 0)
					{
						Thrower.DivideByZero();
					}
					return Calculator.Remainder(in left, right._p0);
				}
			}

			if (right == left)
			{
				return Zero;
			}

			if (right > left)
			{
				return left;
			}

			Span<ulong> quotientSpan = stackalloc ulong[UIntCount];
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(quotientSpan)), left);

			Span<ulong> divisorSpan = stackalloc ulong[UIntCount];
			Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(divisorSpan)), right);

			Span<ulong> rawBits = stackalloc ulong[UIntCount];
			rawBits.Clear();

			Calculator.Remainder(
				quotientSpan[..BitHelper.GetTrimLength(in left)],
				divisorSpan[..BitHelper.GetTrimLength(in right)],
				rawBits);

			return Unsafe.ReadUnaligned<UInt256>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(rawBits)));
		}

		/// <inheritdoc/>
		public static UInt256 operator &(in UInt256 left, in UInt256 right)
		{
			if (Vector256.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = v1 & v2;
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = Avx2.And(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
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
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = v1 | v2;
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = Avx2.Or(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
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
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = v1 ^ v2;
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				var result = Avx2.Xor(v1, v2);
				return Unsafe.BitCast<Vector256<ulong>, UInt256>(result);
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
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				return v1 == v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<byte>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<byte>>(right);
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
				var v1 = Unsafe.BitCast<UInt256, Vector256<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<ulong>>(right);
				return v1 != v2;
			}
			else if (Avx2.IsSupported)
			{
				var v1 = Unsafe.BitCast<UInt256, Vector256<byte>>(left);
				var v2 = Unsafe.BitCast<UInt256, Vector256<byte>>(right);
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
