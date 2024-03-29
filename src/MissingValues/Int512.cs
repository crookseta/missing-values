using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MissingValues
{
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.Int512Converter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct Int512
	{
		internal const int Size = 64;

#if BIGENDIAN
		private readonly UInt256 _upper;
		private readonly UInt256 _lower;
#else
		private readonly UInt256 _lower;
		private readonly UInt256 _upper;
#endif

		internal UInt256 Upper => _upper;
		internal UInt256 Lower => _lower;

		[CLSCompliant(false)]
		public Int512(UInt256 lower)
		{
			_upper = UInt256.Zero;
			_lower = lower;
		}
		[CLSCompliant(false)]
		public Int512(UInt256 upper, UInt256 lower)
		{
			_upper = upper;
			_lower = lower;
		}
		[CLSCompliant(false)]
		public Int512(UInt128 uu, UInt128 ul, UInt128 lu, UInt128 ll)
		{
			_upper = new UInt256(uu, ul);
			_lower = new UInt256(lu, ll);
		}
		[CLSCompliant(false)]
		public Int512(ulong uuu, ulong uul, ulong ulu, ulong ull, ulong luu, ulong lul, ulong llu, ulong lll)
		{
			_upper = new UInt256(new UInt128(uuu, uul), new UInt128(ulu, ull));
			_lower = new UInt256(new UInt128(luu, lul), new UInt128(llu, lll));
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return obj is Int512 @int && Equals(@int);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_upper, _lower);
		}

		public override string? ToString()
		{
			return ToString("D", CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Produces the full product of two signed 512-bit numbers.
		/// </summary>
		/// <param name="left">First number to multiply.</param>
		/// <param name="right">Second number to multiply.</param>
		/// <param name="lower">The low 512-bit of the product of the specified numbers.</param>
		/// <returns>The high 512-bit of the product of the specified numbers.</returns>
		public static Int512 BigMul(Int512 left, Int512 right, out Int512 low)
		{
			// This follows the same logic as is used in `long Math.BigMul(long, long, out long)`

			UInt512 upper = UInt512.BigMul((UInt512)left, (UInt512)right, out UInt512 ulower);
			low = (Int512)ulower;
			return (Int512)(upper) - ((left >> 511) & right) - ((right >> 511) & left);
		}

		/// <summary>Parses a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s" />.</returns>
		/// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="Int512"/>.</exception>
		public static Int512 Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		/// <summary>Tries to parse a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
		/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out Int512 result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		#region From Int512
		public static explicit operator char(Int512 value) => (char)value._lower;
		public static explicit operator checked char(Int512 value)
		{
			if (value._upper == UInt256.Zero)
			{
				throw new OverflowException();
			}
			return checked((char)value._lower);
		}
		// Unsigned
		public static explicit operator byte(Int512 value) => (byte)value._lower;
		public static explicit operator checked byte(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator ushort(Int512 value) => (ushort)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked ushort(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator uint(Int512 value) => (uint)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked uint(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator ulong(Int512 value) => (ulong)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked ulong(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ulong)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator UInt128(Int512 value) => (UInt128)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked UInt128(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((UInt128)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator UInt256(Int512 value) => value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked UInt256(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return value._lower;
		}
		[CLSCompliant(false)]
		public static explicit operator UInt512(Int512 value) => new(value._upper, value._lower);
		[CLSCompliant(false)]
		public static explicit operator checked UInt512(Int512 value)
		{
			if ((Int256)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(value._upper, value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator nuint(Int512 value) => (nuint)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked nuint(Int512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value._lower);
		}
		// Signed
		[CLSCompliant(false)]
		public static explicit operator sbyte(Int512 value) => (sbyte)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked sbyte(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((sbyte)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._lower);
		}
		public static explicit operator short(Int512 value) => (short)value._lower;
		public static explicit operator checked short(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((short)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._lower);
		}
		public static explicit operator int(Int512 value) => (int)value._lower;
		public static explicit operator checked int(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((int)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._lower);
		}
		public static explicit operator long(Int512 value) => (long)value._lower;
		public static explicit operator checked long(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((long)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._lower);
		}
		public static explicit operator Int128(Int512 value) => (Int128)value._lower;
		public static explicit operator checked Int128(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((Int128)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int128)value._lower);
		}
		public static explicit operator Int256(Int512 value) => (Int256)value._lower;
		public static explicit operator checked Int256(Int512 value)
		{
			if (~value._upper == 0)
			{
				return (Int256)value._lower;
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int256)value._lower);
		}
		public static explicit operator nint(Int512 value) => (int)value._lower;
		public static explicit operator checked nint(Int512 value)
		{
			if (~value._upper == 0)
			{
				Int256 lower = (Int256)value._lower;
				return checked((nint)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._lower);
		}
		// Floating
		public static explicit operator decimal(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(decimal)(UInt512)(value);
			}
			return (decimal)(UInt512)(value);
		}
		public static explicit operator Quad(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(Quad)(UInt512)(value);
			}
			return (Quad)(UInt512)(value);
		}
		public static explicit operator double(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(double)(UInt512)(value);
			}
			return (double)(UInt512)(value);
		}
		public static explicit operator Half(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(Half)(UInt512)(value);
			}
			return (Half)(UInt512)(value);
		}
		public static explicit operator float(Int512 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(float)(UInt512)(value);
			}
			return (float)(UInt512)(value);
		}
		#endregion
		#region To Int512
		//Unsigned
		[CLSCompliant(false)]
		public static explicit operator Int512(byte v) => new Int512(v);
		[CLSCompliant(false)]
		public static explicit operator Int512(ushort v) => new Int512(v);
		[CLSCompliant(false)]
		public static explicit operator Int512(uint v) => new Int512(v);
		[CLSCompliant(false)]
		public static explicit operator Int512(nuint v) => new Int512(v);
		[CLSCompliant(false)]
		public static explicit operator Int512(ulong v) => new Int512(v);
		[CLSCompliant(false)]
		public static explicit operator Int512(UInt128 v) => new Int512(v);
		//Signed
		[CLSCompliant(false)]
		public static implicit operator Int512(sbyte v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static implicit operator Int512(short v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static implicit operator Int512(int v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static implicit operator Int512(nint v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static implicit operator Int512(long v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static implicit operator Int512(Int128 v)
		{
			Int256 lower = v;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		//Floating
		public static explicit operator Int512(decimal v) => (Int512)(double)v;
		public static explicit operator checked Int512(decimal v) => checked((Int512)(double)v);
		public static explicit operator Int512(double v)
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if (v <= -TwoPow255)
			{
				return MinValue;
			}
			else if (double.IsNaN(v))
			{
				return 0;
			}
			else if (v >= +TwoPow255)
			{
				return MaxValue;
			}

			return ToInt512(v);
		}
		public static explicit operator checked Int512(double v)
		{
			const double TwoPow511 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if ((0.0d > v + TwoPow511) || double.IsNaN(v) || (v > +TwoPow511))
			{
				throw new OverflowException();
			}
			if (0.0 == TwoPow511 - v)
			{
				return MaxValue;
			}

			return ToInt512(v);
		}
		public static explicit operator Int512(float v) => (Int512)(double)v;
		public static explicit operator checked Int512(float v) => checked((Int512)(double)v);
		public static explicit operator Int512(Half v) => (Int512)(double)v;
		public static explicit operator checked Int512(Half v) => checked((Int512)(double)v);
		#endregion

		private static Int512 ToInt512(double value)
		{
			const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

			Debug.Assert(value >= -TwoPow511);
			Debug.Assert(double.IsFinite(value));
			Debug.Assert(TwoPow511 > value);

			bool isNegative = double.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= 1.0)
			{
				// In order to convert from double to int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by right shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				ulong bits = BitConverter.DoubleToUInt64Bits(value);

				Int512 result = new Int512(new UInt256(new UInt128((bits << 12) >> 1 | 0x8000_0000_0000_0000, 0x0000_0000_0000_0000), UInt128.Zero), UInt256.Zero);
				result >>>= (1023 + 512 - 1 - (int)(bits >> 52));

				if (isNegative)
				{
					return -result;
				}
				return result;
			}
			else
			{
				return Int512.Zero;
			}
		}
	}
}
