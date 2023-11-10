using MissingValues.Internals;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
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
	public partial struct Quad :
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

		static ReadOnlySpan<Quad> IFormattableFloatingPoint<Quad>.PowersOfTen => MathQ.RoundPower10;

		public static Quad Abs(Quad value) => MathQ.Abs(value);
		
		public static Quad Ceiling(Quad x) => MathQ.Ceiling(x);

		public static Quad Clamp(Quad value, Quad min, Quad max)
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

		public static Quad operator %(Quad left, Quad right) => left - Quad.Truncate(left / right) * right;

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
