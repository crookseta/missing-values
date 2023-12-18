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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public partial struct Quad :
		IBinaryFloatingPointIeee754<Quad>,
		IFloatingPoint<Quad>,
		IFormattableFloatingPoint<Quad>,
		IMinMaxValue<Quad>
	{
		public static Quad Epsilon => Quad.UInt128BitsToQuad(EpsilonBits);

		public static Quad NaN => Quad.UInt128BitsToQuad(NegativeQNaNBits);

		public static Quad NegativeInfinity => Quad.UInt128BitsToQuad(NegativeInfinityBits);

		public static Quad NegativeZero => Quad.UInt128BitsToQuad(NegativeZeroBits);

		public static Quad PositiveInfinity => Quad.UInt128BitsToQuad(PositiveInfinityBits);

		public static Quad NegativeOne => Quad.UInt128BitsToQuad(NegativeOneBits);

		public static Quad E => Quad.UInt128BitsToQuad(EBits);

		public static Quad Pi => Quad.UInt128BitsToQuad(PiBits);

		public static Quad Tau => Quad.UInt128BitsToQuad(TauBits);

		public static Quad One => Quad.UInt128BitsToQuad(PositiveOneBits);

		static int INumberBase<Quad>.Radix => 2;

		public static Quad Zero => Quad.UInt128BitsToQuad(PositiveZeroBits);

		static Quad IAdditiveIdentity<Quad, Quad>.AdditiveIdentity => Quad.UInt128BitsToQuad(PositiveZeroBits);

		static Quad IMultiplicativeIdentity<Quad, Quad>.MultiplicativeIdentity => Quad.UInt128BitsToQuad(PositiveOneBits);

		public static Quad MaxValue => Quad.UInt128BitsToQuad(MaxValueBits);

		public static Quad MinValue => Quad.UInt128BitsToQuad(MinValueBits);

		static Quad IBinaryNumber<Quad>.AllBitsSet => Quad.UInt128BitsToQuad(new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));

		static ReadOnlySpan<Quad> IFormattableFloatingPoint<Quad>.PowersOfTen => MathQ.RoundPower10;

		public static Quad Abs(Quad value) => MathQ.Abs(value);
		
		public static Quad Ceiling(Quad x) => MathQ.Ceiling(x);

		public static Quad Clamp(Quad value, Quad min, Quad max) => MathQ.Clamp(value, min, max);

		public static Quad CopySign(Quad value, Quad sign) => MathQ.CopySign(value, sign);

		public static Quad CreateChecked<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Quad result;

			if (!TryConvertFrom(value, out result) && !TOther.TryConvertToChecked<Quad>(value, out result))
			{
				Thrower.NotSupported<Quad, TOther>();
			}

			return result;
		}

		public static Quad CreateSaturating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Quad result;

			if (!TryConvertFrom(value, out result) && !TOther.TryConvertToSaturating<Quad>(value, out result))
			{
				Thrower.NotSupported<Quad, TOther>();
			}

			return result;
		}

		public static Quad CreateTruncating<TOther>(TOther value)
			where TOther : INumberBase<TOther>
		{
			Quad result;

			if (!TryConvertFrom(value, out result) && !TOther.TryConvertToTruncating<Quad>(value, out result))
			{
				Thrower.NotSupported<Quad, TOther>();
			}

			return result;
		}

		public static Quad Floor(Quad x) => MathQ.Floor(x);

		static bool INumberBase<Quad>.IsCanonical(Quad value) => true;

		static bool INumberBase<Quad>.IsComplexNumber(Quad value) => false;

		public static bool IsEvenInteger(Quad value) => IsInteger(value) && (Abs(value % 2) == 0);

		public static bool IsFinite(Quad value)
		{
			Int128 bits = Quad.QuadToInt128Bits(value);
			return (bits & new Int128(0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF)) < new Int128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		}

		static bool INumberBase<Quad>.IsImaginaryNumber(Quad value) => false;

		public static bool IsInfinity(Quad value)
		{
			Int128 bits = Quad.QuadToInt128Bits(value);
			return (bits & new Int128(0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF)) == new Int128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		}

		public static bool IsInteger(Quad value) => IsFinite(value) && (value == Truncate(value));

		public static bool IsNaN(Quad value) => StripSign(value) > PositiveInfinityBits;

		public static bool IsNegative(Quad value) => Int128.IsNegative(Quad.QuadToInt128Bits(value));

		public static bool IsNegativeInfinity(Quad value) => value == NegativeInfinity;

		public static bool IsNormal(Quad value)
		{
			Int128 bits = Quad.QuadToInt128Bits(value);
			bits &= new Int128(0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
			Int128 infBits = new Int128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
			return (bits < infBits) && (bits != Int128.Zero) && ((bits & infBits) != Int128.Zero);
		}

		public static bool IsOddInteger(Quad value) => IsInteger(value) && (Abs(value % 2) == 1);

		public static bool IsPositive(Quad value) => Int128.IsPositive(Quad.QuadToInt128Bits(value));

		public static bool IsPositiveInfinity(Quad value) => value == PositiveInfinity;

		public static bool IsPow2(Quad value)
		{
			UInt128 bits = Quad.QuadToUInt128Bits(value);

			ushort biasedExponent = ExtractBiasedExponentFromBits(bits); ;
			UInt128 trailingSignificand = ExtractTrailingSignificandFromBits(bits);

			return (value > UInt128.Zero)
				&& (biasedExponent != MinBiasedExponent) && (biasedExponent != MaxBiasedExponent)
				&& (trailingSignificand == MinTrailingSignificand);
		}

		public static bool IsRealNumber(Quad value) => !IsNaN(value);

		public static bool IsSubnormal(Quad value)
		{
			Int128 bits = Quad.QuadToInt128Bits(value);
			bits &= new Int128(0x7FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
			Int128 infBits = new Int128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
			return (bits < infBits) && (bits != Int128.Zero) && ((bits & infBits) == Int128.Zero);
		}

		static bool INumberBase<Quad>.IsZero(Quad value) => value == Zero;

		public static Quad Max(Quad x, Quad y) => MathQ.Max(x, y);

		public static Quad MaxMagnitude(Quad x, Quad y) => MathQ.MaxMagnitude(x, y);

		public static Quad MaxMagnitudeNumber(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `maximumMagnitudeNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the input with a larger magnitude.
			// It treats +0 as larger than -0 as per the specification.

			Quad ax = Abs(x);
			Quad ay = Abs(y);

			if ((ax > ay) || IsNaN(ay))
			{
				return x;
			}

			if (ax == ay)
			{
				return IsNegative(x) ? y : x;
			}

			return y;
		}

		public static Quad MaxNumber(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `maximumNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the larger of the inputs. It
			// treats +0 as larger than -0 as per the specification.

			if (x != y)
			{
				if (!IsNaN(y))
				{
					return y < x ? x : y;
				}

				return x;
			}

			return IsNegative(y) ? x : y;
		}

		public static Quad Min(Quad x, Quad y) => MathQ.Min(x, y);

		public static Quad MinMagnitude(Quad x, Quad y) => MathQ.MinMagnitude(x, y);

		public static Quad MinMagnitudeNumber(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `minimumMagnitudeNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the input with a larger magnitude.
			// It treats +0 as larger than -0 as per the specification.

			Quad ax = Abs(x);
			Quad ay = Abs(y);

			if ((ax < ay) || IsNaN(ay))
			{
				return x;
			}

			if (ax == ay)
			{
				return IsNegative(x) ? x : y;
			}

			return y;
		}

		public static Quad MinNumber(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `minimumNumber` function
			//
			// It does not propagate NaN inputs back to the caller and
			// otherwise returns the larger of the inputs. It
			// treats +0 as larger than -0 as per the specification.

			if (x != y)
			{
				if (!IsNaN(y))
				{
					return x < y ? x : y;
				}

				return x;
			}

			return IsNegative(x) ? x : y;
		}

		public static Quad Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out Quad result))
			{
				Thrower.ParsingError<Quad>(s.ToString());
			}
			return result;
		}

		public static Quad Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			if (!TryParse(s, style, provider, out Quad result))
			{
				Thrower.ParsingError<Quad>(s);
			}
			return result;
		}

		public static Quad Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out Quad result))
			{
				Thrower.ParsingError<Quad>(s.ToString());
			}
			return result;
		}

		public static Quad Parse(string s, IFormatProvider? provider)
		{
			if (!TryParse(s, NumberStyles.Float, provider, out Quad result))
			{
				Thrower.ParsingError<Quad>(s);
			}
			return result;
		}
		
		public static Quad Round(Quad x) => MathQ.Round(x);

		public static Quad Round(Quad x, int digits) => MathQ.Round(x, digits);

		public static Quad Round(Quad x, MidpointRounding mode) => MathQ.Round(x, mode);

		public static Quad Round(Quad x, int digits, MidpointRounding mode) => MathQ.Round(x, digits, mode);
		
		public static int Sign(Quad value) => MathQ.Sign(value);

		/// <inheritdoc cref="IRootFunctions{TSelf}.Sqrt(TSelf)"/>
		public static Quad Sqrt(Quad x) => MathQ.Sqrt(x);

		public static Quad Truncate(Quad x) => MathQ.Truncate(x);

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			return NumberParser.TryParseQuad(s, style, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			return NumberParser.TryParseQuad(s, style, provider, out result);
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			return NumberParser.TryParseQuad(s, NumberStyles.Float, provider, out result);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Quad result)
		{
			return NumberParser.TryParseQuad(s, NumberStyles.Float, provider, out result);
		}

		static bool INumberBase<Quad>.TryConvertFromChecked<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

		static bool INumberBase<Quad>.TryConvertFromSaturating<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

		static bool INumberBase<Quad>.TryConvertFromTruncating<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

		private static bool TryConvertFrom<TOther>(TOther value, out Quad result)
		{
			bool converted = true;

			result = value switch
			{
				Half actual => (Quad)actual,
				float actual => (Quad)actual,
				double actual => (Quad)actual,
				decimal actual => (Quad)actual,
				byte actual => (Quad)actual,
				ushort actual => (Quad)actual,
				uint actual => (Quad)actual,
				ulong actual => (Quad)actual,
				UInt128 actual => (Quad)actual,
				UInt256 actual => (Quad)actual,
				UInt512 actual => (Quad)actual,
				nuint actual => (Quad)actual,
				sbyte actual => (Quad)actual,
				short actual => (Quad)actual,
				int actual => (Quad)actual,
				long actual => (Quad)actual,
				Int128 actual => (Quad)actual,
				Int256 actual => (Quad)actual,
				Int512 actual => (Quad)actual,
				_ => BitHelper.DefaultConvert<Quad>(out converted)
			};

			return converted;
		}

		static bool INumberBase<Quad>.TryConvertToChecked<TOther>(Quad value, out TOther result)
		{
			bool converted = true;
			result = default;
			checked
			{
				result = result switch
				{
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
					Int512 => (TOther)(object)(Int512)value,
					nint => (TOther)(object)(nint)value,
					_ => BitHelper.DefaultConvert<TOther>(out converted)
				};
			}

			return converted;
		}

		static bool INumberBase<Quad>.TryConvertToSaturating<TOther>(Quad value, out TOther result) => TryConvertTo(value, out result);

		static bool INumberBase<Quad>.TryConvertToTruncating<TOther>(Quad value, out TOther result) => TryConvertTo(value, out result);

		private static bool TryConvertTo<TOther>(Quad value, out TOther result)
		{
			bool converted = true;
			result = default;

			result = result switch
			{
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)((value >= byte.MaxValue) ? byte.MaxValue : (value <= byte.MinValue) ? byte.MinValue : (byte)value),
				ushort => (TOther)(object)((value >= ushort.MaxValue) ? ushort.MaxValue : (value <= ushort.MinValue) ? ushort.MinValue : (ushort)value),
				uint => (TOther)(object)((value >= uint.MaxValue) ? uint.MaxValue : (value <= uint.MinValue) ? uint.MinValue : (uint)value),
				ulong => (TOther)(object)((value >= ulong.MaxValue) ? ulong.MaxValue : (value <= ulong.MinValue) ? ulong.MinValue : (ulong)value),
				UInt128 => (TOther)(object)((value >= UInt128.MaxValue) ? UInt128.MaxValue : (value <= UInt128.MinValue) ? UInt128.MinValue : (UInt128)value),
				UInt256 => (TOther)(object)((value >= new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt256.MaxValue : (value <= Quad.Zero) ? UInt256.MinValue : (UInt256)value),
				UInt512 => (TOther)(object)((value >= new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue : (value <= Quad.Zero) ? UInt512.MinValue : (UInt512)value),
				nuint => (TOther)(object)((value >= nuint.MaxValue) ? nuint.MaxValue : (value <= nuint.MinValue) ? nuint.MinValue : (nuint)value),
				sbyte => (TOther)(object)((value >= sbyte.MaxValue) ? sbyte.MaxValue : (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
				short => (TOther)(object)((value >= short.MaxValue) ? short.MaxValue : (value <= short.MinValue) ? short.MinValue : (short)value),
				int => (TOther)(object)((value >= int.MaxValue) ? int.MaxValue : (value <= int.MinValue) ? int.MinValue : (int)value),
				long => (TOther)(object)((value >= long.MaxValue) ? long.MaxValue : (value <= long.MinValue) ? long.MinValue : (long)value),
				Int128 => (TOther)(object)((value >= Int128.MaxValue) ? Int128.MaxValue : (value <= Int128.MinValue) ? Int128.MinValue : (Int128)value),
				Int256 => (TOther)(object)((value >= new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MaxValue : (value <= new Quad(0xC0FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MinValue : (Int256)value),
				Int512 => (TOther)(object)((value >= new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MaxValue : (value <= new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MinValue : (Int512)value),
				nint => (TOther)(object)((value >= nint.MaxValue) ? nint.MaxValue : (value <= nint.MinValue) ? nint.MinValue : (nint)value),
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};

			return converted;
		}

		public int CompareTo(object? obj)
		{
			if (obj is not Quad o)
			{
				if (obj is null)
				{
					return 1;
				}
				else
				{
					Thrower.MustBeType<Quad>();
					return 0;
				}
			}
			return CompareTo(o);
		}

		public int CompareTo(Quad other)
		{
			if (this < other)
			{
				return -1;
			}

			if (this > other)
			{
				return 1;
			}

			if (this == other)
			{
				return 0;
			}

			if (IsNaN(this))
			{
				return IsNaN(other) ? 0 : -1;
			}

			return 1;
		}

		public bool Equals(Quad other)
		{
			return (_upper == other._upper && _lower == other._lower)
				|| AreZero(this, other)
				|| (IsNaN(this) && IsNaN(other));
		}

		public int GetExponentByteCount() => sizeof(short);

		public int GetExponentShortestBitLength()
		{
			short exponent = Exponent;

			if (exponent >= 0)
			{
				return (sizeof(short) * 8) - short.LeadingZeroCount(exponent);
			}
			else
			{
				return (sizeof(short) * 8) + 1 - short.LeadingZeroCount((short)(~exponent));
			}
		}

		public int GetSignificandBitLength() => 113;

		public int GetSignificandByteCount() => 16;

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			return NumberFormatter.QuadToString(in this, format, formatProvider);
		}

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			return NumberFormatter.TryFormatQuad(in this, destination, out charsWritten, format, provider);
		}

		public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= sizeof(short))
			{
				short exponent = Exponent;

				if (BitConverter.IsLittleEndian)
				{
					exponent = BinaryPrimitives.ReverseEndianness(exponent);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

				bytesWritten = sizeof(short);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= sizeof(short))
			{
				short exponent = Exponent;

				if (!BitConverter.IsLittleEndian)
				{
					exponent = BinaryPrimitives.ReverseEndianness(exponent);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

				bytesWritten = sizeof(short);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= sizeof(ulong))
			{
				UInt128 significand = Significand;

				if (BitConverter.IsLittleEndian)
				{
					significand = BitHelper.ReverseEndianness(significand);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

				bytesWritten = sizeof(ulong);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= sizeof(ulong))
			{
				UInt128 significand = Significand;

				if (!BitConverter.IsLittleEndian)
				{
					significand = BitHelper.ReverseEndianness(significand);
				}

				Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

				bytesWritten = sizeof(ulong);
				return true;
			}
			else
			{
				bytesWritten = 0;
				return false;
			}
		}

		public static Quad Atan2(Quad y, Quad x) => MathQ.Atan2(y, x);

		public static Quad Atan2Pi(Quad y, Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad BitDecrement(Quad x) => MathQ.BitDecrement(x);

		public static Quad BitIncrement(Quad x) => MathQ.BitIncrement(x);

		public static Quad FusedMultiplyAdd(Quad left, Quad right, Quad addend) => MathQ.FusedMultiplyAdd(left, right, addend);

		public static Quad Ieee754Remainder(Quad left, Quad right) => MathQ.IEEERemainder(left, right);

		public static int ILogB(Quad x) => MathQ.ILogB(x);

		public static Quad ScaleB(Quad x, int n) => MathQ.ScaleB(x, n);

		public static Quad Exp(Quad x) => MathQ.Exp(x);
		public static Quad ExpM1(Quad x)
		{
			// TODO: Replace with an actual implementation
			return Exp(x) - One;
			/* origin: FreeBSD /usr/src/lib/msun/ld128/s_expl.c */
			throw new NotImplementedException();
		}

		public static Quad Exp10(Quad x)
		{
			ReadOnlySpan<Quad> P10 = stackalloc Quad[]
			{
				new Quad(0x3FCD_203A_F9EE_7561, 0x59B2_1F3A_6E02_97EC), // 1E-15
				new Quad(0x3FD0_6849_B86A_12B9, 0xB01E_A709_0983_3DE7), // 1E-14
				new Quad(0x3FD3_C25C_2684_9768, 0x1C26_50CB_4BE4_0D61), // 1E-13
				new Quad(0x3FD7_1979_9812_DEA1, 0x1197_F27F_0F6E_885D), // 1E-12
				new Quad(0x3FDA_5FD7_FE17_9649, 0x55FD_EF1E_D34A_2A74), // 1E-11
				new Quad(0x3FDD_B7CD_FD9D_7BDB, 0xAB7D_6AE6_881C_B511), // 1E-10
				new Quad(0x3FE1_12E0_BE82_6D69, 0x4B2E_62D0_1511_F12A), // 1E-09
				new Quad(0x3FE4_5798_EE23_08C3, 0x9DF9_FB84_1A56_6D75), // 1E-08
				new Quad(0x3FE7_AD7F_29AB_CAF4, 0x8578_7A65_20EC_08D2), // 1E-07
				new Quad(0x3FEB_0C6F_7A0B_5ED8, 0xD36B_4C7F_3493_8583), // 1E-06
				new Quad(0x3FEE_4F8B_588E_368F, 0x0846_1F9F_01B8_66E4), // 1E-05
				new Quad(0x3FF1_A36E_2EB1_C432, 0xCA57_A786_C226_809D), // 1E-04
				new Quad(0x3FF5_0624_DD2F_1A9F, 0xBE76_C8B4_3958_1062), // 1E-03
				new Quad(0x3FF8_47AE_147A_E147, 0xAE14_7AE1_47AE_147B), // 1E-02
				new Quad(0x3FFB_9999_9999_9999, 0x9999_9999_9999_999A), // 1E-01
				new Quad(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0000), // 1E00
				new Quad(0x4002_4000_0000_0000, 0x0000_0000_0000_0000), // 1E01
				new Quad(0x4005_9000_0000_0000, 0x0000_0000_0000_0000), // 1E02
				new Quad(0x4008_F400_0000_0000, 0x0000_0000_0000_0000), // 1E03
				new Quad(0x400C_3880_0000_0000, 0x0000_0000_0000_0000), // 1E04
				new Quad(0x400F_86A0_0000_0000, 0x0000_0000_0000_0000), // 1E05
				new Quad(0x4012_E848_0000_0000, 0x0000_0000_0000_0000), // 1E06
				new Quad(0x4016_312D_0000_0000, 0x0000_0000_0000_0000), // 1E07
				new Quad(0x4019_7D78_4000_0000, 0x0000_0000_0000_0000), // 1E08
				new Quad(0x401C_DCD6_5000_0000, 0x0000_0000_0000_0000), // 1E09
				new Quad(0x4020_2A05_F200_0000, 0x0000_0000_0000_0000), // 1E10
				new Quad(0x4023_7487_6E80_0000, 0x0000_0000_0000_0000), // 1E11
				new Quad(0x4026_D1A9_4A20_0000, 0x0000_0000_0000_0000), // 1E12
				new Quad(0x402A_2309_CE54_0000, 0x0000_0000_0000_0000), // 1E13
				new Quad(0x402D_6BCC_41E9_0000, 0x0000_0000_0000_0000), // 1E14
				new Quad(0x4030_C6BF_5263_4000, 0x0000_0000_0000_0000), // 1E15
			};
			Quad y = MathQ.ModF(x, out Quad n);

			// Abs(n) < 16 without raising invalid NaN
			if (n.BiasedExponent < 0x3FFF + 4)
			{
				if (y == Quad.Zero)
				{
					return P10[(int)n + 15];
				}
				y = Exp2(new Quad(0x4000_A934_F097_9A37, 0x15FC_9257_EDFE_9B5F) * y); // y = exp2l(3.32192809488736234787031942948939L * y)
				return y * P10[(int)n + 15];
			}

			return Pow(P10[16], x);
		}
		public static Quad Exp10M1(Quad x) => Exp10(x) - One;

		public static Quad Exp2(Quad x)
		{
			int e = x.BiasedExponent;
			Quad r, z, t;
			uint i0;
			(uint u, int i) k;

			// Filter out exceptional cases
			if (e >= 0x3FFF + 14) // |x| >= 16384 or x is NaN 
			{
				if (x.BiasedExponent >= 0x3FFF + 15 && !IsNegative(x))
				{
					return PositiveInfinity;
				}
				if (Quad.IsNaN(x))
				{
					return x;
				}
				if (e == 0x7FFF)
				{
					return Zero;
				}
				if (x < new Quad(0xC00D_00F8_0000_0000, 0x0000_0000_0000_0000))
				{
					return Zero;
				}
			}
			else if (e < 0x3FFF - 114)
			{
				return One + x;
			}

			/*
			 * Reduce x, computing z, i0, and k. The low bits of x + redux
			 * contain the 16-bit integer part of the exponent (k) followed by
			 * TBLBITS fractional bits (i0). We use bit tricks to extract these
			 * as integers, then set z to the remainder.
			 *
			 * Example: Suppose x is 0xabc.123456p0 and TBLBITS is 8.
			 * Then the low-order word of x + redux is 0x000abc12,
			 * We split this into k = 0xabc and i0 = 0x12 (adjusted to
			 * index into the table), then we compute z = 0x0.003456p0.
			 */
			Quad u = x + MathQConstants.Exp.redux;
			i0 = (uint)(u._lower) + MathQConstants.Exp.TBLSIZE / 2;
			k = (i0 / MathQConstants.Exp.TBLSIZE * MathQConstants.Exp.TBLSIZE, 0);
			k.i = ((int)k.u) / MathQConstants.Exp.TBLSIZE;
			i0 %= MathQConstants.Exp.TBLSIZE;
			u -= MathQConstants.Exp.redux;
			z = x - u;

			// Compute r = exp2(y) = exp2t[i0] * p(z - eps[i]).
			t = MathQConstants.Exp.Tbl[(int)i0];
			z -= MathQConstants.Exp.Eps[(int)i0];
			r = t + t * z * (MathQConstants.Exp.P1 + z * (MathQConstants.Exp.P2 + z * (MathQConstants.Exp.P3 + z * (MathQConstants.Exp.P4 + z * (MathQConstants.Exp.P5 + z * (MathQConstants.Exp.P6
		+ z * (MathQConstants.Exp.P7 + z * (MathQConstants.Exp.P8 + z * (MathQConstants.Exp.P9 + z * MathQConstants.Exp.P10)))))))));

			return MathQ.ScaleB(r, k.i);
		}
		public static Quad Exp2M1(Quad x) => Exp2(x) - One;

		public static Quad Acosh(Quad x) => MathQ.Acosh(x);

		public static Quad Asinh(Quad x) => MathQ.Asinh(x);

		public static Quad Atanh(Quad x) => MathQ.Atanh(x);

		public static Quad Cosh(Quad x) => MathQ.Cosh(x);

		public static Quad Sinh(Quad x) => MathQ.Sinh(x);

		public static Quad Tanh(Quad x) => MathQ.Tanh(x);

		public static Quad Log(Quad x) => MathQ.Log(x);
		public static Quad LogP1(Quad x) 
		{
			// TODO: Replace with an actual implementation
			return MathQ.Log(x + One);
			/* origin: FreeBSD /usr/src/lib/msun/ld128/s_logl.c */
			throw new NotImplementedException();
		}

		public static Quad Log(Quad x, Quad newBase) => MathQ.Log(x, newBase);

		public static Quad Log10(Quad x) => MathQ.Log10(x);
		public static Quad Log10P1(Quad x) => MathQ.Log10(x + One);

		public static Quad Log2(Quad value) => MathQ.Log2(value);
		public static Quad Log2P1(Quad value) => MathQ.Log2(value + One);

		public static Quad Pow(Quad x, Quad y) => MathQ.Pow(x, y);

		public static Quad Cbrt(Quad x) => MathQ.Cbrt(x);

		public static Quad Hypot(Quad x, Quad y)
		{
			int ex = x.BiasedExponent, ey = y.BiasedExponent;

			if (ex < ey)
			{
				(ex, ey) = (ey, ex);
				(x, y) = (Abs(y), Abs(x));
			}
			else
			{
				x = Abs(x);
				y = Abs(y);
			}

			if (ex == 0x7FFF && IsInfinity(y))
			{
				return y;
			}
			if (ex == 0x7FFF || y == Quad.Zero)
			{
				return x;
			}
			if (ex - ey > MantissaDigits)
			{
				return x + y;
			}

			Quad z = Quad.One;
			Quad huge = new Quad(0x670F_0000_0000_0000, 0x0000_0000_0000_0000); // 0x1p10000
			Quad tiny = new Quad(0x18EF_0000_0000_0000, 0x0000_0000_0000_0000); // 0x1p-10000
			if (ex > 0x3FFF+8000)
			{
				z = huge;
				x *= tiny; 
				y *= tiny;
			}
			else if (ey < 0x3FFF-8000)
			{
				z = tiny;
				x *= huge;
				y *= huge;
			}

			Sq(x, out Quad hx, out Quad lx);
			Sq(y, out Quad hy, out Quad ly);
			return z * MathQ.Sqrt(ly + lx + hy + hx);

			static void Sq(Quad x, out Quad hi, out Quad lo)
			{
				Quad xh, xl, xc;
				xc = x * new Quad(0x4038_0000_0000_0000, 0x0080_0000_0000_0000); // SPLIT = 0x1p57 + 1
				xh = x - xc + xc;
				xl = x - xh;
				hi = x * x;
				lo = xh * xh - hi + Quad.Two * xh * xl + xl * xl;
			}
		}

		public static Quad ReciprocalEstimate(Quad x) => MathQ.ReciprocalEstimate(x);

		public static Quad ReciprocalSqrtEstimate(Quad x) => MathQ.ReciprocalSqrtEstimate(x);

		public static Quad RootN(Quad x, int n)
		{
			Quad result;

			if (n > 0)
			{
				if (n == 2)
				{
					result = (x != Quad.Zero) ? Sqrt(x) : Quad.Zero;
				}
				else if (n == 3)
				{
					result = Cbrt(x);
				}
				else
				{
					result = PositiveN(x, n);
				}
			}
			else if (n < 0)
			{
				result = NegativeN(x, n);
			}
			else
			{
				Debug.Assert(n == 0);
				result = NaN;
			}
			return result;

			static Quad PositiveN(Quad x, int n)
			{
				Quad result;

				if (IsFinite(x))
				{
					if (x != Zero)
					{
						if ((x > Zero) || int.IsOddInteger(n))
						{
							result = Pow(Abs(x), ReciprocalEstimate(n));
							result = CopySign(result, x);
						}
						else
						{
							result = NaN;
						}
					}
					else if (int.IsEvenInteger(n))
					{
						result = Zero;
					}
					else
					{
						result = CopySign(Zero, x);
					}
				}
				else if (IsNaN(x))
				{
					result = NaN;
				}
				else if (x > Quad.Zero)
				{
					Debug.Assert(IsPositiveInfinity(x));
					result = PositiveInfinity;
				}
				else
				{
					Debug.Assert(IsNegativeInfinity(x));
					result = int.IsOddInteger(n) ? NegativeInfinity : NaN;
				}

				return result;
			}
			static Quad NegativeN(Quad x, int n)
			{
				Quad result;

				if (IsFinite(x))
				{
					if (x != Zero)
					{
						if ((x > Zero) || int.IsOddInteger(n))
						{
							result = Pow(Abs(x), ReciprocalEstimate(n));
							result = CopySign(result, x);
						}
						else
						{
							result = NaN;
						}
					}
					else if (int.IsEvenInteger(n))
					{
						result = PositiveInfinity;
					}
					else
					{
						result = CopySign(PositiveInfinity, x);
					}
				}
				else if (IsNaN(x))
				{
					result = NaN;
				}
				else if (x > Zero)
				{
					Debug.Assert(IsPositiveInfinity(x));
					result = Zero;
				}
				else
				{
					Debug.Assert(IsNegativeInfinity(x));
					result = int.IsOddInteger(n) ? NegativeZero : NaN;
				}

				return result;
			}
		}

		public static Quad Acos(Quad x) => MathQ.Acos(x);

		public static Quad AcosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Asin(Quad x) => MathQ.Asin(x);

		public static Quad AsinPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Atan(Quad x) => MathQ.Atan(x);

		public static Quad AtanPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Cos(Quad x) => MathQ.Cos(x);

		public static Quad CosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Sin(Quad x) => MathQ.Sin(x);

		public static (Quad Sin, Quad Cos) SinCos(Quad x) => MathQ.SinCos(x);

		public static (Quad SinPi, Quad CosPi) SinCosPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad SinPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad Tan(Quad x) => MathQ.Tan(x);

		public static Quad TanPi(Quad x)
		{
			throw new NotImplementedException();
		}

		public static Quad operator +(Quad value) => value;

		public static Quad operator +(Quad left, Quad right) => MathQ.Add(left, right);

		public static Quad operator -(Quad value)
		{
			Quad signMask = new Quad(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
			// Invert the sign bit
			return value ^ signMask;
		}

		public static Quad operator -(Quad left, Quad right) => MathQ.Sub(left, right);

		public static Quad operator ~(Quad value) => new Quad(~value._upper, ~value._lower);

		public static Quad operator ++(Quad value) => MathQ.Add(value, One);

		public static Quad operator --(Quad value) => MathQ.Sub(value, One);

		public static Quad operator *(Quad left, Quad right) => MathQ.Mul(left, right);

		public static Quad operator /(Quad left, Quad right) => MathQ.Div(left, right);

		public static Quad operator %(Quad left, Quad right)
		{
			return (MathQ.Abs(left) - (MathQ.Abs(right) * (MathQ.Floor(MathQ.Abs(left) / MathQ.Abs(right))))) * MathQ.Sign(left);
		}

		public static Quad operator &(Quad left, Quad right) => new Quad(left._upper & right._upper, left._lower & right._lower);

		public static Quad operator |(Quad left, Quad right) => new Quad(left._upper | right._upper, left._lower | right._lower);

		public static Quad operator ^(Quad left, Quad right) => new Quad(left._upper ^ right._upper, left._lower ^ right._lower);

		public static bool operator ==(Quad left, Quad right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				// IEEE defines that NaN is not equal to anything, including itself.
				return false;
			}

			// IEEE defines that positive and negative zero are equivalent.
			return (left._upper == right._upper && left._lower == right._lower) || AreZero(left, right);
		}

		public static bool operator !=(Quad left, Quad right) => !(left == right);

		public static bool operator <(Quad left, Quad right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				// IEEE defines that NaN is unordered with respect to everything, including itself.
				return false;
			}

			bool leftIsNegative = IsNegative(left);

			if (leftIsNegative != IsNegative(right))
			{
				// When the signs of right and left differ, we know that right is less than left if it is
				// the negative value. The exception to this is if both values are zero, in which case IEEE
				// says they should be equal, even if the signs differ.
				return leftIsNegative && !AreZero(left, right);
			}

			UInt128 leftBits = QuadToUInt128Bits(left);
			UInt128 rightBits = QuadToUInt128Bits(right);

			return (leftBits != rightBits) && ((leftBits < rightBits) ^ leftIsNegative);
		}

		public static bool operator >(Quad left, Quad right)
		{
			if (IsNaN(right) || IsNaN(left))
			{
				// IEEE defines that NaN is unordered with respect to everything, including itself.
				return false;
			}

			bool rightIsNegative = IsNegative(right);

			if (rightIsNegative != IsNegative(left))
			{
				return rightIsNegative && !AreZero(right, left);
			}

			UInt128 leftBits = QuadToUInt128Bits(left);
			UInt128 rightBits = QuadToUInt128Bits(right);

			return (rightBits != leftBits) && ((rightBits < leftBits) ^ rightIsNegative);
		}

		public static bool operator <=(Quad left, Quad right)
		{
			if (IsNaN(left) || IsNaN(right))
			{
				// IEEE defines that NaN is unordered with respect to everything, including itself.
				return false;
			}

			bool leftIsNegative = IsNegative(left);

			if (leftIsNegative != IsNegative(right))
			{
				// When the signs of right and left differ, we know that right is less than left if it is
				// the negative value. The exception to this is if both values are zero, in which case IEEE
				// says they should be equal, even if the signs differ.
				return leftIsNegative || AreZero(left, right);
			}

			UInt128 leftBits = QuadToUInt128Bits(left);
			UInt128 rightBits = QuadToUInt128Bits(right);

			return (leftBits == rightBits) || ((leftBits < rightBits) ^ leftIsNegative);
		}

		public static bool operator >=(Quad left, Quad right)
		{
			if (IsNaN(right) || IsNaN(left))
			{
				// IEEE defines that NaN is unordered with respect to everything, including itself.
				return false;
			}

			bool rightIsNegative = IsNegative(right);

			if (rightIsNegative != IsNegative(left))
			{
				return rightIsNegative || AreZero(right, left);
			}

			UInt128 leftBits = QuadToUInt128Bits(left);
			UInt128 rightBits = QuadToUInt128Bits(right);

			return (rightBits == leftBits) || ((rightBits < leftBits) ^ rightIsNegative);
		}
	}
}
