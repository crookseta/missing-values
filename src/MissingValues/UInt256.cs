using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MissingValues
{
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct UInt256
	{
		internal const int Size = 32;
#if BIGENDIAN
		private readonly UInt128 _upper;
		private readonly UInt128 _lower;
#else
		private readonly UInt128 _lower;
		private readonly UInt128 _upper;
#endif

		internal UInt128 Upper => _upper;
		internal UInt128 Lower => _lower;


		public UInt256(ulong u1, ulong u2, ulong l1, ulong l2)
		{
			_upper = new(u1, u2);
			_lower = new(l1, l2);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
		public UInt256(UInt128 lower)
		{
			_upper = UInt128.Zero;
			_lower = lower;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="upper">The upper 128-bits of the 256-bit value.</param>
		/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
		public UInt256(UInt128 upper, UInt128 lower)
		{
			_upper = upper;
			_lower = lower;
		}

		public override string ToString()
		{
			return NumberFormatter.UnsignedNumberToString(in this, new(10));
		}

		public override bool Equals(object? obj)
		{
			return obj is UInt256 @int && Equals(@int);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_upper, _lower);
		}

		/// <summary>Parses a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s" />.</returns>
		/// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="UInt256"/>.</exception>
		public static UInt256 Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		/// <summary>Tries to parse a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
		/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out UInt256 result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		#region From UInt256
		public static explicit operator char(UInt256 value) => (char)value._lower;
		public static explicit operator checked char(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((char)value._lower);
		}
		// Unsigned
		public static explicit operator byte(UInt256 value) => (byte)value._lower;
		public static explicit operator checked byte(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._lower);
		}
		public static explicit operator ushort(UInt256 value) => (ushort)value._lower;
		public static explicit operator checked ushort(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._lower);
		}
		public static explicit operator uint(UInt256 value) => (uint)value._lower;
		public static explicit operator checked uint(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._lower);
		}
		public static explicit operator ulong(UInt256 value) => (ulong)value._lower;
		public static explicit operator checked ulong(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ulong)value._lower);
		}
		public static explicit operator UInt128(UInt256 value) => value._lower;
		public static explicit operator checked UInt128(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value._lower;
		}
		public static implicit operator UInt512(UInt256 value) => new(value);
		public static explicit operator nuint(UInt256 value) => (nuint)value._lower;
		public static explicit operator checked nuint(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return (nuint)value._lower;
		}
		//Signed
		public static explicit operator sbyte(UInt256 value) => (sbyte)value._lower;
		public static explicit operator checked sbyte(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._lower);
		}
		public static explicit operator short(UInt256 value) => (short)value._lower;
		public static explicit operator checked short(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._lower);
		}
		public static explicit operator int(UInt256 value) => (int)value._lower;
		public static explicit operator checked int(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._lower);
		}
		public static explicit operator long(UInt256 value) => (long)value._lower;
		public static explicit operator checked long(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._lower);
		}
		public static explicit operator Int128(UInt256 value) => (Int128)value._lower;
		public static explicit operator checked Int128(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return (Int128)value._lower;
		}
		public static explicit operator Int256(UInt256 value) => new(value._upper, value._lower);
		public static explicit operator checked Int256(UInt256 value)
		{
			if ((Int128)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(value._upper, value._lower);
		}
		public static explicit operator Int512(UInt256 value) => new(value);
		public static explicit operator checked Int512(UInt256 value)
		{
			if ((Int128)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(value);
		}
		public static explicit operator nint(UInt256 value) => (nint)value._lower;
		public static explicit operator checked nint(UInt256 value)
		{
			if (value._upper != 0)
			{
				Thrower.IntegerOverflow();
			}
			return (nint)value._lower;
		}
		// Floating
		public static explicit operator decimal(UInt256 value)
		{

			if (value._upper != 0)
			{
				// The default behavior of decimal conversions is to always throw on overflow
				Thrower.IntegerOverflow();
			}

			return (decimal)value._lower;
		}
		public static explicit operator double(UInt256 value)
		{
			const double TwoPow204 = 25711008708143844408671393477458601640355247900524685364822016.0d;
			const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0d;

			const ulong TwoPow204bits = 0x4CB0_0000_0000_0000;
			const ulong TwoPow256bits = 0x4FF0_0000_0000_0000;

			if (value._upper == 0)
			{
				return (double)value._lower;
			}


			double lower = BitConverter.UInt64BitsToDouble(TwoPow204bits | ((ulong)(value._lower >> 12) >> 12) | ((ulong)(value._lower) & 0xFFFFFF)) - TwoPow204;
			double upper = BitConverter.UInt64BitsToDouble(TwoPow256bits | (ulong)(value >> 204)) - TwoPow256;

			return lower + upper;
		}
		public static explicit operator Half(UInt256 value) => (Half)(double)value;
		public static explicit operator float(UInt256 value) => (float)(double)value;
		#endregion

		#region To UInt256
		public static implicit operator UInt256(char value) => new UInt256(value);
		// Floating
		public static explicit operator UInt256(Half value) => (UInt256)(double)value;
		public static explicit operator checked UInt256(Half value) => checked((UInt256)(double)value);
		public static explicit operator UInt256(float value) => (UInt256)(double)value;
		public static explicit operator checked UInt256(float value) => checked((UInt256)(double)value);
		public static explicit operator UInt256(double value)
		{
			const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

			if (double.IsNegative(value) || double.IsNaN(value))
			{
				return MinValue;
			}
			else if (value >= TwoPow256)
			{
				return MaxValue;
			}

			return ToUInt256(value);
		}
		public static explicit operator checked UInt256(double value)
		{
			const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

			// We need to convert -0.0 to 0 and not throw, so we compare
			// value against 0 rather than checking IsNegative

			if ((value < 0.0) || double.IsNaN(value) || (0.0 < TwoPow256 - value))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow256 - value)
			{
				return MaxValue;
			}

			return ToUInt256(value);
		}
		public static explicit operator UInt256(decimal value) => (UInt256)(double)value;
		public static explicit operator checked UInt256(decimal value) => checked((UInt256)(double)value);
		// Unsigned
		public static implicit operator UInt256(byte value) => new UInt256(0, value);
		public static implicit operator UInt256(ushort value) => new UInt256(0, value);
		public static implicit operator UInt256(uint value) => new UInt256(0, value);
		public static implicit operator UInt256(ulong value) => new UInt256(0, value);
		public static implicit operator UInt256(UInt128 value) => new UInt256(0, value);
		public static implicit operator UInt256(nuint value) => new UInt256(0, value);
		// Signed
		public static explicit operator UInt256(sbyte value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(sbyte value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (byte)value);
		}
		public static explicit operator UInt256(short value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(short value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		}
		public static explicit operator UInt256(int value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(int value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		}
		public static explicit operator UInt256(long value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(long value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		}
		public static explicit operator UInt256(Int128 value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(Int128 value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		} 
		public static explicit operator UInt256(nint value)
		{
			Int128 lower = value;
			return new((UInt128)(lower >> 127), (UInt128)lower);
		}
		public static explicit operator checked UInt256(nint value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		} 
		#endregion

		private static UInt256 ToUInt256(double value)
		{
			const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

			Debug.Assert(value >= 0);
			Debug.Assert(double.IsFinite(value));
			Debug.Assert(0.0 >= TwoPow256 - value);

			// This code is based on `f64_to_u128` from m-ou-se/floatconv
			// Copyright (c) 2020 Mara Bos <m-ou.se@m-ou.se>. All rights reserved.
			//
			// Licensed under the BSD 2 - Clause "Simplified" License
			// See THIRD-PARTY-NOTICES.TXT for the full license text

			if (value >= 1.0)
			{
				// In order to convert from double to uint256 we first need to extract the signficand,
				// including the implicit leading bit, as a full 256-bit significand. We can then adjust
				// this down to the represented integer by right shifting by the unbiased exponent, taking
				// into account the significand is now represented as 256-bits.

				ulong bits = BitConverter.DoubleToUInt64Bits(value);

				var exponent = ((bits >> 52) & 0x7FF) - 1023;
				var matissa = (bits & 0x0F_FFFF_FFFF_FFFF) | 0x10_0000_0000_0000;

				if (exponent <= 52)
				{
					return (UInt256)(matissa >> (int)(52 - exponent));
				}
				else if (exponent >= 256)
				{
					return UInt256.MaxValue;
				}
				else
				{
					return ((UInt256)matissa) << ((int)(exponent - 52));
				}
			}
			else
			{
				return MinValue;
			}
		}
	}
}
