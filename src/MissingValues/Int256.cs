using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct Int256
	{
		internal const int Size = 32;

#if BIGENDIAN
		private readonly UInt128 _upper;
		private readonly UInt128 _lower;
#else
		private readonly UInt128 _lower;
		private readonly UInt128 _upper;
#endif
		internal UInt128 Lower => _lower;
		internal UInt128 Upper => _upper;

		[CLSCompliant(false)]
		public Int256(ulong u1, ulong u2, ulong l1, ulong l2)
		{
			_upper = new(u1, u2);
			_lower = new(l1, l2);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Int256" /> struct.
		/// </summary>
		/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
		[CLSCompliant(false)]
		public Int256(UInt128 lower) : this(UInt128.Zero, lower)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Int256" /> struct.
		/// </summary>
		/// <param name="upper">The upper 128-bits of the 256-bit value.</param>
		/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
		[CLSCompliant(false)]
		public Int256(UInt128 upper, UInt128 lower)
		{
			_upper = upper;
			_lower = lower;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is Int256 other) && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_upper, _lower);
		}

		public override string ToString()
		{
			return NumberFormatter.SignedNumberToDecimalString<Int256, UInt256>(in this);
		}

		/// <summary>
		/// Produces the full product of two signed 256-bit numbers.
		/// </summary>
		/// <param name="left">First number to multiply.</param>
		/// <param name="right">Second number to multiply.</param>
		/// <param name="lower">The low 256-bit of the product of the specified numbers.</param>
		/// <returns>The high 256-bit of the product of the specified numbers.</returns>
		public static Int256 BigMul(Int256 left, Int256 right, out Int256 low)
		{
			// This follows the same logic as is used in `long Math.BigMul(long, long, out long)`

			UInt256 upper = UInt256.BigMul((UInt256)left, (UInt256)right, out UInt256 ulower);
			low = (Int256)ulower;
			return (Int256)(upper) - ((left >> 255) & right) - ((right >> 255) & left);
		}

		/// <summary>Parses a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s" />.</returns>
		/// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="Int256"/>.</exception>
		public static Int256 Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		/// <summary>Tries to parse a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
		/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out Int256 result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		#region From Int256
		// Signed
		[CLSCompliant(false)]
		public static explicit operator sbyte(Int256 value) => (sbyte)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked sbyte(Int256 value)
		{
			if (~value._upper == 0)
			{
				Int128 lower = (Int128)value._lower;
				return checked((sbyte)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._lower);
		}
		public static explicit operator short(Int256 value) => (short)value._lower;
		public static explicit operator checked short(Int256 value)
		{
			if (~value._upper == 0)
			{
				Int128 lower = (Int128)value._lower;
				return checked((short)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._lower);
		}
		public static explicit operator int(Int256 value) => (int)value._lower;
		public static explicit operator checked int(Int256 value)
		{
			if (~value._upper == 0)
			{
				Int128 lower = (Int128)value._lower;
				return checked((int)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._lower);
		}
		public static explicit operator long(Int256 value) => (long)value._lower;
		public static explicit operator checked long(Int256 value)
		{
			if (~value._upper == 0)
			{
				Int128 lower = (Int128)value._lower;
				return checked((long)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._lower);
		}
		public static explicit operator Int128(Int256 value) => (Int128)value._lower;
		public static explicit operator checked Int128(Int256 value)
		{
			if (~value._upper == 0)
			{
				return (Int128)value._lower;
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int128)value._lower);
		}
		public static implicit operator Int512(Int256 value)
		{
			return new Int512(unchecked((UInt256)value));
		}
		public static explicit operator nint(Int256 value) => (nint)value._lower;
		public static explicit operator checked nint(Int256 value)
		{
			if (~value._upper == 0)
			{
				Int128 lower = (Int128)value._lower;
				return checked((nint)lower);
			}

			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._lower);
		}
		// Unsigned
		public static explicit operator byte(Int256 value) => (byte)value._lower;
		public static explicit operator checked byte(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator ushort(Int256 value) => (ushort)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked ushort(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator uint(Int256 value) => (uint)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked uint(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator ulong(Int256 value) => (ulong)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked ulong(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ulong)value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator UInt128(Int256 value) => value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked UInt128(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value._lower;
		}
		[CLSCompliant(false)]
		public static explicit operator UInt256(Int256 value) => new(value._upper, value._lower);
		[CLSCompliant(false)]
		public static explicit operator checked UInt256(Int256 value)
		{
			if ((Int128)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(value._upper, value._lower);
		}
		[CLSCompliant(false)]
		public static explicit operator UInt512(Int256 value) => new(unchecked((UInt256)value));
		[CLSCompliant(false)]
		public static explicit operator checked UInt512(Int256 value)
		{
			if ((Int128)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(unchecked((UInt256)value));
		}
		[CLSCompliant(false)]
		public static explicit operator nuint(Int256 value) => (nuint)value._lower;
		[CLSCompliant(false)]
		public static explicit operator checked nuint(Int256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value._lower);
		}
		// Floating
		public static explicit operator decimal(Int256 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(decimal)(double)(UInt256)(value);
			}
			return (decimal)(double)(UInt256)(value);
		}
		public static explicit operator double(Int256 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(double)(UInt256)(value);
			}
			return (double)(UInt256)(value);
		}
		public static explicit operator Half(Int256 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(Half)(UInt256)(value);
			}
			return (Half)(UInt256)(value);
		}
		public static explicit operator float(Int256 value)
		{
			if (IsNegative(value))
			{
				value = -value;
				return -(float)(UInt256)(value);
			}
			return (float)(UInt256)(value);
		}
		#endregion

		#region To Int256
		// Signed
		[CLSCompliant(false)]
		public static implicit operator Int256(sbyte v)
		{
			Int128 lower = v;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static implicit operator Int256(short v)
		{
			Int128 lower = v;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static implicit operator Int256(int v)
		{
			Int128 lower = v;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static implicit operator Int256(nint v)
		{
			Int128 lower = v;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static implicit operator Int256(long v)
		{
			Int128 lower = v;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static implicit operator Int256(Int128 v)
		{
			return new((UInt128)(v >> 127), (UInt128)v);
		}
		//Unsigned
		public static explicit operator Int256(byte v) => new Int256(v);
		[CLSCompliant(false)]
		public static explicit operator Int256(ushort v) => new Int256(v);
		[CLSCompliant(false)]
		public static explicit operator Int256(uint v) => new Int256(v);
		[CLSCompliant(false)]
		public static explicit operator Int256(nuint v) => new Int256(v);
		[CLSCompliant(false)]
		public static explicit operator Int256(ulong v) => new Int256(v);
		[CLSCompliant(false)]
		public static explicit operator Int256(UInt128 v) => new Int256(v);
		//Floating
		public static explicit operator Int256(decimal v) => (Int256)(double)v;
		public static explicit operator checked Int256(decimal v) => checked((Int256)(double)v);
		public static explicit operator Int256(double v)
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

			return ToInt256(v);
		}
		public static explicit operator checked Int256(double v)
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if ((0.0d > v + TwoPow255) || double.IsNaN(v) || (v > +TwoPow255))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow255 - v)
			{
				return MaxValue;
			}

			return ToInt256(v);
		}
		public static explicit operator Int256(float v) => (Int256)(double)v;
		public static explicit operator checked Int256(float v) => checked((Int256)(double)v);
		public static explicit operator Int256(Half v) => (Int256)(double)v;
		public static explicit operator checked Int256(Half v) => checked((Int256)(double)v);

		#endregion

		private static Int256 ToInt256(double value)
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			Debug.Assert(value >= -TwoPow255);
			Debug.Assert(double.IsFinite(value));
			Debug.Assert(TwoPow255 > value);

			// This code is based on `f64_to_i128` from m-ou-se/floatconv
			// Copyright (c) 2020 Mara Bos <m-ou.se@m-ou.se>. All rights reserved.
			//
			// Licensed under the BSD 2 - Clause "Simplified" License
			// See THIRD-PARTY-NOTICES.TXT for the full license text

			bool isNegative = double.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= 1.0)
			{
				// In order to convert from double to int256 we first need to extract the signficand,
				// including the implicit leading bit, as a full 256-bit significand. We can then adjust
				// this down to the represented integer by right shifting by the unbiased exponent, taking
				// into account the significand is now represented as 256-bits.

				ulong bits = BitConverter.DoubleToUInt64Bits(value);

				Int256 result = new Int256(new UInt128((bits << 12) >> 1 | 0x8000_0000_0000_0000, 0x0000_0000_0000_0000), UInt128.Zero);
				result >>>= (1023 + 256 - 1 - (int)(bits >> 52));

				if (isNegative)
				{
					return -result;
				}
				return result;
			}
			else
			{
				return Int256.Zero;
			}
		}
	}
}
