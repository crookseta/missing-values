using MissingValues.Info;
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
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public partial struct UInt512 :
		IBigInteger<UInt512>,
		IMinMaxValue<UInt512>,
		IUnsignedNumber<UInt512>,
		IPowerFunctions<UInt512>,
		IFormattableUnsignedInteger<UInt512, Int512>
	{
		static UInt512 INumberBase<UInt512>.One => One;

		static int INumberBase<UInt512>.Radix => 2;

		static UInt512 INumberBase<UInt512>.Zero => Zero;

		static UInt512 IAdditiveIdentity<UInt512, UInt512>.AdditiveIdentity => Zero;

		static UInt512 IMultiplicativeIdentity<UInt512, UInt512>.MultiplicativeIdentity => One;

		static UInt512 IMinMaxValue<UInt512>.MaxValue => MaxValue;

		static UInt512 IMinMaxValue<UInt512>.MinValue => MinValue;

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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public static (UInt512 Quotient, UInt512 Remainder) DivRem(UInt512 left, UInt512 right)
		{
			DivRem(in left, in right, out UInt512 quotient, out UInt512 remainder);

			return (quotient, remainder);
		}
		internal static void DivRem(in UInt512 left, in UInt512 right, out UInt512 quotient, out UInt512 remainder)
		{
			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
			{
				if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0 && right._p0 <= uint.MaxValue)
				{
					if (right._p0 == 0)
					{
						Thrower.DivideByZero();
					}
					DivRemFast(in left, (uint)right._p0, out quotient, out remainder);
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
				quotient = Zero;
				remainder = left;
				return;
			}

			DivRemSlow(in left, in right, out quotient, out remainder);

			static void DivRemFast(in UInt512 quotient, uint divisor, out UInt512 quo, out UInt512 rem)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				Span<uint> rawBits = stackalloc uint[UIntCount];
				rawBits.Clear();

				Calculator.DivRem(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))], divisor, rawBits, out uint remainder);

				quo = Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
				rem = remainder;
			}
			static void DivRemSlow(in UInt512 quotient, in UInt512 divisor, out UInt512 quo, out UInt512 rem)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				Span<uint> divisorSpan = stackalloc uint[UIntCount];
				divisorSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(divisorSpan)), divisor);

				Span<uint> quoBits = stackalloc uint[UIntCount];
				quoBits.Clear();
				Span<uint> remBits = stackalloc uint[UIntCount];
				remBits.Clear();

				Calculator.DivRem(
					quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))],
					divisorSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in divisor) / 32))],
					quoBits,
					remBits);

				quo = Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quoBits)));
				rem = Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(remBits)));
			}
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

#if NET9_0_OR_GREATER
		static UInt512 INumberBase<UInt512>.MultiplyAddEstimate(UInt512 left, UInt512 right, UInt512 addend) => (left * right) + addend;
#endif

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt512 Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt512 Parse(string s, IFormatProvider? provider)
		{
			ArgumentNullException.ThrowIfNull(s);
			var status = NumberParser.TryParseToUnsigned(Utf16Char.CastFromCharSpan(s), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(s.ToString());
			}
			return output;
		}

		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), style, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(utf8Text);
			}
			return output;
		}
		/// <inheritdoc/>
		public static UInt512 Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
		{
			var status = NumberParser.TryParseToUnsigned(Utf8Char.CastFromByteSpan(utf8Text), NumberStyles.Integer, provider, out UInt512 output);
			if (!status)
			{
				status.Throw<UInt512>(utf8Text);
			}
			return output;
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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					sourceRef = ref Unsafe.Add(ref sourceRef, source.Length - Size);

					// We have at least 32 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

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

				ref byte sourceRef = ref MemoryMarshal.GetReference(source);

				if (source.Length >= Size)
				{
					// We have at least 64 bytes, so just read the ones we need directly
					result = Unsafe.ReadUnaligned<UInt512>(ref sourceRef);

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

		/// <inheritdoc/>
		public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.FormatUInt512(in this, format, formatProvider);
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
					Quad actual => (UInt512)actual,
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
					BigInteger actual => (UInt512)actual,
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
				Quad actual => (actual >= new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue : (actual <= Quad.Zero) ? UInt512.MinValue : (UInt512)actual,
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
				BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (UInt512)actual,
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
				BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (UInt512)actual,
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
					BigInteger => (TOther)(object)(BigInteger)value,
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
				BigInteger => (TOther)(object)(BigInteger)value,
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
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
			return converted;
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUInt512(in this, Utf16Char.CastFromCharSpan(destination), out charsWritten, format, provider);
		}

		/// <inheritdoc/>
		public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatUInt512(in this, Utf8Char.CastFromByteSpan(utf8Destination), out bytesWritten, format, provider);
		}

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

		char IFormattableInteger<UInt512>.ToChar() => (char)_p0;

		int IFormattableInteger<UInt512>.ToInt32() => (int)_p0;

		Int512 IFormattableUnsignedInteger<UInt512, Int512>.ToSigned() => (Int512)this;

		static int IFormattableUnsignedInteger<UInt512, Int512>.CountDigits(in UInt512 value) => CountDigits(in value);

		internal static int CountDigits(in UInt512 value)
		{
			var upper = value.Upper;
			if (upper == UInt256.Zero)
			{
				return UInt256.CountDigits(value.Lower);
			}
			// We have more than 1e77, so we have atleast 78 digits
			int digits = 78;

			if (upper > 0x8)
			{
				// value / 1e78
				var lower = (value / new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0008, 0xA2DB_F142_DFCC_7AB6, 0xE356_9326_C784_3372, 0xA9F4_D250_5E3A_4000, 0x0000_0000_0000_0000));
				digits += UInt256.CountDigits(lower.Lower);
			}
			else if ((upper == 0x59) && (value.Lower >= new UInt256(0x5C97_6C9C_BDFC_CB24, 0xE161_BF83_CB2A_027A, 0xA390_3723_AE46_8000, 0x0000_0000_0000_0000)))
			{
				// We are greater than 1e78, but less than 1e79
				// so we have exactly 79 digits

				digits++;
			}

			return digits;
		}

		static int IFormattableInteger<UInt512>.UnsignedCompare(in UInt512 value1, in UInt512 value2)
		{
			if (value1 < value2) return -1;
			else if (value1 > value2) return 1;
			else return 0;
		}

		/// <inheritdoc/>
		public static UInt512 operator +(in UInt512 value) => value;

		/// <inheritdoc/>
		public static UInt512 operator +(in UInt512 left, in UInt512 right)
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

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}

		/// <inheritdoc/>
		public static UInt512 operator checked +(in UInt512 left, in UInt512 right)
		{
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

			ulong part7 = checked(left._p7 + right._p7 + carry);

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}

		/// <inheritdoc/>
		public static UInt512 operator -(in UInt512 value) => Zero - value;

		/// <inheritdoc/>
		public static UInt512 operator checked -(in UInt512 value) => checked(Zero - value);

		/// <inheritdoc/>
		public static UInt512 operator -(in UInt512 left, in UInt512 right)
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

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}

		/// <inheritdoc/>
		public static UInt512 operator checked -(in UInt512 left, in UInt512 right)
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

			ulong part7 = checked(left._p7 - right._p7 - borrow);

			return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
		}

		/// <inheritdoc/>
		public static UInt512 operator ~(in UInt512 value)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v = Unsafe.BitCast<UInt512, Vector512<ulong>>(value);
				var result = ~v;
				return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
			}
			else
			{
				return new(~value._p7, ~value._p6, ~value._p5, ~value._p4, ~value._p3, ~value._p2, ~value._p1, ~value._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator ++(in UInt512 value) => value + One;

		/// <inheritdoc/>
		public static UInt512 operator checked ++(in UInt512 value) => checked(value + One);

		/// <inheritdoc/>
		public static UInt512 operator --(in UInt512 value) => value - One;

		/// <inheritdoc/>
		public static UInt512 operator checked --(in UInt512 value) => checked(value - One);

		/// <inheritdoc/>
		public static UInt512 operator *(in UInt512 left, in UInt512 right)
		{
			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0 && right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
			{
				if (right._p0 <= uint.MaxValue)
				{
					return Calculator.Multiply(in left, unchecked((uint)right._p0), out _);
				}
				if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0 && left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					ulong up = Math.BigMul(left._p0, right._p0, out ulong low);
					return new UInt512(0, 0, 0, 0, 0, 0, up, low);
				}
			}

			const int UIntCount = Size / sizeof(uint);

			Span<uint> leftSpan = stackalloc uint[UIntCount];
			leftSpan.Clear();
			Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(leftSpan)), left);

			Span<uint> rightSpan = stackalloc uint[UIntCount];
			rightSpan.Clear();
			Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rightSpan)), right);

			Span<uint> rawBits = stackalloc uint[UIntCount * 2];
			rawBits.Clear();

			Calculator.Multiply(
				leftSpan[..(UIntCount - (BitHelper.LeadingZeroCount(in left) / 32))],
				rightSpan[..(UIntCount - (BitHelper.LeadingZeroCount(in right) / 32))],
				rawBits);

			return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
		}
		
		/// <inheritdoc/>
		public static UInt512 operator checked *(in UInt512 left, in UInt512 right)
		{
			UInt512 lower;

			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0 && right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
			{
				if (right._p0 <= uint.MaxValue)
				{
					lower = Calculator.Multiply(in left, unchecked((uint)right._p0), out uint carry);

					if (carry != 0)
					{
						Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
					}

					return lower;
				}
				if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0 && left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					ulong up = Math.BigMul(left._p0, right._p0, out ulong low);
					return new UInt512(0, 0, 0, 0, 0, 0, up, low);
				}
			}

			const int UIntCount = Size / sizeof(uint);

			Span<uint> leftSpan = stackalloc uint[UIntCount];
			leftSpan.Clear();
			Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(leftSpan)), left);

			Span<uint> rightSpan = stackalloc uint[UIntCount];
			rightSpan.Clear();
			Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rightSpan)), right);

			Span<uint> rawBits = stackalloc uint[UIntCount * 2];
			rawBits.Clear();

			Calculator.Multiply(
				leftSpan[..(UIntCount - (BitHelper.LeadingZeroCount(in left) / 32))],
				rightSpan[..(UIntCount - (BitHelper.LeadingZeroCount(in right) / 32))],
				rawBits);
			var overflowBits = rawBits[UIntCount..];

			for (int i = 0; i < overflowBits.Length; i++)
			{
				if (overflowBits[i] != 0)
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Multiplication);
				}
			}

			return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
		}

		/// <inheritdoc/>
		public static UInt512 operator /(in UInt512 left, in UInt512 right)
		{
			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
			{
				if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0 && right._p0 <= uint.MaxValue)
				{
					if (right._p0 == 0)
					{
						Thrower.DivideByZero();
					}
					return DivideFast(in left, (uint)right._p0);
				}
			}
			
			if (right >= left)
			{
				return (right == left) ? One : Zero;
			}

			return DivideSlow(left, right);

			static UInt512 DivideFast(in UInt512 quotient, uint divisor)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				Span<uint> rawBits = stackalloc uint[UIntCount];
				rawBits.Clear();

				Calculator.Divide(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))], divisor, rawBits);

				return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
			}
			static UInt512 DivideSlow(in UInt512 quotient, in UInt512 divisor)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				Span<uint> divisorSpan = stackalloc uint[UIntCount];
				divisorSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(divisorSpan)), divisor);

				Span<uint> rawBits = stackalloc uint[UIntCount];
				rawBits.Clear();

				Calculator.Divide(
					quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))],
					divisorSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in divisor) / 32))],
					rawBits);

				return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator %(in UInt512 left, in UInt512 right)
		{
			if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
			{
				if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0 && right._p0 <= uint.MaxValue)
				{
					if (right._p0 == 0)
					{
						Thrower.DivideByZero();
					}
					return RemainderFast(in left, (uint)right._p0);
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

			return RemainderSlow(in left, in right);

			static UInt512 RemainderFast(in UInt512 quotient, uint divisor)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				return Calculator.Remainder(quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))], divisor);
			}
			static UInt512 RemainderSlow(in UInt512 quotient, in UInt512 divisor)
			{
				const int UIntCount = Size / sizeof(uint);

				Span<uint> quotientSpan = stackalloc uint[UIntCount];
				quotientSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(quotientSpan)), quotient);

				Span<uint> divisorSpan = stackalloc uint[UIntCount];
				divisorSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(divisorSpan)), divisor);

				Span<uint> rawBits = stackalloc uint[UIntCount];
				rawBits.Clear();

				Calculator.Remainder(
					quotientSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in quotient) / 32))],
					divisorSpan[..((UIntCount) - (BitHelper.LeadingZeroCount(in divisor) / 32))],
					rawBits);

				return Unsafe.ReadUnaligned<UInt512>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(rawBits)));
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator &(in UInt512 left, in UInt512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
				var result = v1 & v2;
				return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
			}
			else
			{
				return new(left._p7 & right._p7, left._p6 & right._p6, left._p5 & right._p5, left._p4 & right._p4, left._p3 & right._p3, left._p2 & right._p2, left._p1 & right._p1, left._p0 & right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator |(in UInt512 left, in UInt512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
				var result = v1 | v2;
				return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
			}
			else
			{
				return new(left._p7 | right._p7, left._p6 | right._p6, left._p5 | right._p5, left._p4 | right._p4, left._p3 | right._p3, left._p2 | right._p2, left._p1 | right._p1, left._p0 | right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator ^(in UInt512 left, in UInt512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.BitCast<UInt512, Vector512<ulong>>(left);
				var v2 = Unsafe.BitCast<UInt512, Vector512<ulong>>(right);
				var result = v1 ^ v2;
				return Unsafe.BitCast<Vector512<ulong>, UInt512>(result);
			}
			else
			{
				return new(left._p7 ^ right._p7, left._p6 ^ right._p6, left._p5 ^ right._p5, left._p4 ^ right._p4, left._p3 ^ right._p3, left._p2 ^ right._p2, left._p1 ^ right._p1, left._p0 ^ right._p0);
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator <<(in UInt512 value, int shiftAmount)
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

				return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0);
				}

				ulong part6 = (value._p6 << shiftAmount) | (value._p5 >> (64 - shiftAmount));
				ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part6, part5, part4, part3, part2, part1, part0, 0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p5, value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0);
				}

				ulong part5 = (value._p5 << shiftAmount) | (value._p4 >> (64 - shiftAmount));
				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part5, part4, part3, part2, part1, part0, 0, 0);
			}
			else if (shiftAmount < 256)
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p4, value._p3, value._p2, value._p1, value._p0, 0, 0, 0);
				}

				ulong part4 = (value._p4 << shiftAmount) | (value._p3 >> (64 - shiftAmount));
				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part4, part3, part2, part1, part0, 0, 0, 0);
			}
			else if (shiftAmount < 320)
			{
				shiftAmount -= 256;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p3, value._p2, value._p1, value._p0, 0, 0, 0, 0);
				}

				ulong part3 = (value._p3 << shiftAmount) | (value._p2 >> (64 - shiftAmount));
				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part3, part2, part1, part0, 0, 0, 0, 0);
			}
			else if (shiftAmount < 384)
			{
				shiftAmount -= 320;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p2, value._p1, value._p0, 0, 0, 0, 0, 0);
				}

				ulong part2 = (value._p2 << shiftAmount) | (value._p1 >> (64 - shiftAmount));
				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part2, part1, part0, 0, 0, 0, 0, 0);
			}
			else if (shiftAmount < 448)
			{
				shiftAmount -= 384;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p1, value._p0, 0, 0, 0, 0, 0, 0);
				}

				ulong part1 = (value._p1 << shiftAmount) | (value._p0 >> (64 - shiftAmount));
				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part1, part0, 0, 0, 0, 0, 0, 0);
			}
			else // shiftAmount < 512
			{
				shiftAmount -= 448;

				if (shiftAmount == 0)
				{
					return new UInt512(value._p0, 0, 0, 0, 0, 0, 0, 0);
				}

				ulong part0 = value._p0 << shiftAmount;

				return new UInt512(part0, 0, 0, 0, 0, 0, 0, 0);
			}
		}

		/// <inheritdoc/>
		public static UInt512 operator >>(in UInt512 value, int shiftAmount) => value >>> shiftAmount;

		/// <inheritdoc/>
		public static bool operator ==(in UInt512 left, in UInt512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt512, Vector512<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt512, Vector512<ulong>>(ref Unsafe.AsRef(in right));
				return v1 == v2;
			}
			else
			{
				return (left._p7 == right._p7) && (left._p6 == right._p6) && (left._p5 == right._p5) && (left._p4 == right._p4)
					&& (left._p3 == right._p3) && (left._p2 == right._p2) && (left._p1 == right._p1) && (left._p0 == right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator !=(in UInt512 left, in UInt512 right)
		{
			if (Vector512.IsHardwareAccelerated)
			{
				var v1 = Unsafe.As<UInt512, Vector512<ulong>>(ref Unsafe.AsRef(in left));
				var v2 = Unsafe.As<UInt512, Vector512<ulong>>(ref Unsafe.AsRef(in right));
				return v1 != v2;
			}
			else
			{
				return (left._p7 != right._p7) || (left._p6 != right._p6) || (left._p5 != right._p5) || (left._p4 != right._p4)
					|| (left._p3 != right._p3) || (left._p2 != right._p2) || (left._p1 != right._p1) || (left._p0 != right._p0);
			}
		}

		/// <inheritdoc/>
		public static bool operator <(in UInt512 left, in UInt512 right)
		{
			// Successively compare each part.
			return (left._p7 < right._p7)
				|| (left._p7 == right._p7 && ((left._p6 < right._p6)
				|| (left._p6 == right._p6 && ((left._p5 < right._p5)
				|| (left._p5 == right._p5 && ((left._p4 < right._p4)
				|| (left._p4 == right._p4 && ((left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 < right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator >(in UInt512 left, in UInt512 right)
		{
			return (left._p7 > right._p7)
				|| (left._p7 == right._p7 && ((left._p6 > right._p6) 
				|| (left._p6 == right._p6 && ((left._p5 > right._p5)
				|| (left._p5 == right._p5 && ((left._p4 > right._p4)
				|| (left._p4 == right._p4 && ((left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1) 
				|| (left._p1 == right._p1 && (left._p0 > right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator <=(in UInt512 left, in UInt512 right)
		{
			return (left._p7 < right._p7)
				|| (left._p7 == right._p7 && ((left._p6 < right._p6)
				|| (left._p6 == right._p6 && ((left._p5 < right._p5)
				|| (left._p5 == right._p5 && ((left._p4 < right._p4)
				|| (left._p4 == right._p4 && ((left._p3 < right._p3)
				|| (left._p3 == right._p3 && ((left._p2 < right._p2)
				|| (left._p2 == right._p2 && ((left._p1 < right._p1)
				|| (left._p1 == right._p1 && (left._p0 <= right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static bool operator >=(in UInt512 left, in UInt512 right)
		{
			return (left._p7 > right._p7)
				|| (left._p7 == right._p7 && ((left._p6 > right._p6)
				|| (left._p6 == right._p6 && ((left._p5 > right._p5)
				|| (left._p5 == right._p5 && ((left._p4 > right._p4)
				|| (left._p4 == right._p4 && ((left._p3 > right._p3)
				|| (left._p3 == right._p3 && ((left._p2 > right._p2)
				|| (left._p2 == right._p2 && ((left._p1 > right._p1)
				|| (left._p1 == right._p1 && (left._p0 >= right._p0))))))))))))));
		}

		/// <inheritdoc/>
		public static UInt512 operator >>>(in UInt512 value, int shiftAmount)
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

				return new UInt512(part7, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 128)
			{
				shiftAmount -= 64;

				if (shiftAmount == 0)
				{
					return new UInt512(0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1);
				}

				ulong part0 = (value._p1 >> shiftAmount) | (value._p2 << (64 - shiftAmount));
				ulong part1 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part2 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part3 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part4 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part5 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part6 = value._p7 >> shiftAmount;

				return new UInt512(0, part6, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 192)
			{
				shiftAmount -= 128;

				if (shiftAmount == 0)
				{
					return new UInt512(0, 0, value._p7, value._p6, value._p5, value._p4, value._p3, value._p2);
				}

				ulong part0 = (value._p2 >> shiftAmount) | (value._p3 << (64 - shiftAmount));
				ulong part1 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part2 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part3 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part4 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part5 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, part5, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 256)
			{
				shiftAmount -= 192;

				if (shiftAmount == 0)
				{
					return new UInt512(0, 0, 0, value._p7, value._p6, value._p5, value._p4, value._p3);
				}

				ulong part0 = (value._p3 >> shiftAmount) | (value._p4 << (64 - shiftAmount));
				ulong part1 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part2 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part3 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part4 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, 0, part4, part3, part2, part1, part0);
			}
			else if (shiftAmount < 320)
			{
				shiftAmount -= 256;

				if (shiftAmount == 0)
				{
					return new UInt512(0, 0, 0, 0, value._p7, value._p6, value._p5, value._p4);
				}

				ulong part0 = (value._p4 >> shiftAmount) | (value._p5 << (64 - shiftAmount));
				ulong part1 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part2 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part3 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, 0, 0, part3, part2, part1, part0);
			}
			else if (shiftAmount < 384)
			{
				shiftAmount -= 320;

				if (shiftAmount == 0)
				{
					return new UInt512(0, 0, 0, 0, 0, value._p7, value._p6, value._p5);
				}

				ulong part0 = (value._p5 >> shiftAmount) | (value._p6 << (64 - shiftAmount));
				ulong part1 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part2 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, 0, 0, 0, part2, part1, part0);
			}
			else if (shiftAmount < 448)
			{
				shiftAmount -= 384;

				if (shiftAmount == 0)
				{
					return new UInt512(0, 0, 0, 0, 0, 0, value._p7, value._p6);
				}

				ulong part0 = (value._p6 >> shiftAmount) | (value._p7 << (64 - shiftAmount));
				ulong part1 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, 0, 0, 0, 0, part1, part0);
			}
			else // shiftAmount < 512
			{
				shiftAmount -= 448;

				ulong part0 = value._p7 >> shiftAmount;

				return new UInt512(0, 0, 0, 0, 0, 0, 0, part0);
			}
		}
	}
}
