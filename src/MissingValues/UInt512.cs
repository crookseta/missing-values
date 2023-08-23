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
	public readonly partial struct UInt512
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

		public UInt512(UInt256 lower)
		{
			_upper = UInt256.Zero;
			_lower = lower;
		}
		public UInt512(UInt256 upper, UInt256 lower)
		{
			_upper = upper;
			_lower = lower;
		}
        public UInt512(UInt128 uu, UInt128 ul, UInt128 lu, UInt128 ll)
        {
			_upper = new UInt256(uu, ul);
			_lower = new UInt256(lu, ll);
        }
		public UInt512(ulong uuu, ulong uul, ulong ulu, ulong ull, ulong luu, ulong lul, ulong llu, ulong lll)
		{
			_upper = new UInt256(new UInt128(uuu, uul), new UInt128(ulu, ull));
			_lower = new UInt256(new UInt128(luu, lul), new UInt128(llu, lll));
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return obj is UInt512 @int && Equals(@int);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_upper, _lower);
		}

		public override string? ToString()
		{
			return NumberFormatter.UnsignedNumberToDecimalString(in this);
		}

		/// <summary>Parses a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s" />.</returns>
		/// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
		/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="UInt512"/>.</exception>
		public static UInt512 Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		/// <summary>Tries to parse a span of characters into a value.</summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
		/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out UInt512 result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		#region From UInt512
		public static explicit operator char(UInt512 value) => (char)value._lower;
		public static explicit operator checked char(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((char)value._lower);
		}
		// Unsigned
		public static explicit operator byte(UInt512 value) => (byte)value._lower;
		public static explicit operator checked byte(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._lower);
		}
		public static explicit operator ushort(UInt512 value) => (ushort)value._lower;
		public static explicit operator checked ushort(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._lower);
		}
		public static explicit operator uint(UInt512 value) => (uint)value._lower;
		public static explicit operator checked uint(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._lower);
		}
		public static explicit operator ulong(UInt512 value) => (ulong)value._lower;
		public static explicit operator checked ulong(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ulong)value._lower);
		}
		public static explicit operator UInt128(UInt512 value) => (UInt128)value._lower;
		public static explicit operator checked UInt128(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((UInt128)value._lower);
		}
		public static explicit operator UInt256(UInt512 value) => value._lower;
		public static explicit operator checked UInt256(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return value._lower;
		}
		public static explicit operator nuint(UInt512 value) => (nuint)value._lower;
		public static explicit operator checked nuint(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value._lower);
		}
		// Signed
		public static explicit operator sbyte(UInt512 value) => (sbyte)value._lower;
		public static explicit operator checked sbyte(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._lower);
		}
		public static explicit operator short(UInt512 value) => (short)value._lower;
		public static explicit operator checked short(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._lower);
		}
		public static explicit operator int(UInt512 value) => (int)value._lower;
		public static explicit operator checked int(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._lower);
		}
		public static explicit operator long(UInt512 value) => (long)value._lower;
		public static explicit operator checked long(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._lower);
		}
		public static explicit operator Int128(UInt512 value) => (Int128)value._lower;
		public static explicit operator checked Int128(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int128)value._lower);
		}
		public static explicit operator Int256(UInt512 value) => (Int256)value._lower;
		public static explicit operator checked Int256(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return (Int256)value._lower;
		}
		public static explicit operator Int512(UInt512 value) => new Int512(value._upper, value._lower);
		public static explicit operator checked Int512(UInt512 value)
		{
			if ((Int256)value._upper < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new Int512(value._upper, value._lower);
		}
		public static explicit operator nint(UInt512 value) => (nint)value._lower;
		public static explicit operator checked nint(UInt512 value)
		{
			if (value._upper != UInt256.Zero)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._lower);
		}
		// Floating
		public static explicit operator decimal(UInt512 value)
		{
			if (value._upper != 0)
			{
				// The default behavior of decimal conversions is to always throw on overflow
				Thrower.IntegerOverflow();
			}

			return (decimal)value._lower;
		}
		public static explicit operator double(UInt512 value)
		{
			const double TwoPow460 = 2977131414714805823690030317109266572712515013375254774912983855843898524112477893944078543723575564536883288499266264815757728270805630976.0d;
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			const ulong TwoPow460bits = 0x5CB0_0000_0000_0000;
			const ulong TwoPow512bits = 0x5FF0_0000_0000_0000;

			if (value._upper == 0)
			{
				return (double)value._lower;
			}

			double lower = BitConverter.UInt64BitsToDouble(TwoPow460bits | ((ulong)(value >> 12) >> 12) | ((ulong)(value._lower) & 0xFFFFFF)) - TwoPow460;
			double upper = BitConverter.UInt64BitsToDouble(TwoPow512bits | (ulong)(value >> 460)) - TwoPow512;

			return lower + upper;
		}
		public static explicit operator Half(UInt512 value) => (Half)(double)value;
		public static explicit operator float(UInt512 value) => (float)(double)value;
		#endregion
		#region To UInt512
		public static implicit operator UInt512(char value) => new(value);
		//Unsigned
		public static implicit operator UInt512(byte value) => new UInt512(0, value);
		public static implicit operator UInt512(ushort value) => new UInt512(0, value);
		public static implicit operator UInt512(uint value) => new UInt512(0, value);
		public static implicit operator UInt512(ulong value) => new UInt512(0, value);
		public static implicit operator UInt512(UInt128 value) => new UInt512(0, value);
		public static implicit operator UInt512(nuint value) => new UInt512(0, value);
		//Signed
		public static explicit operator UInt512(sbyte value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(sbyte value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (byte)value);
		}
		public static explicit operator UInt512(short value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(short value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (ushort)value);
		}
		public static explicit operator UInt512(int value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(int value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (uint)value);
		}
		public static explicit operator UInt512(long value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(long value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (ulong)value);
		}
		public static explicit operator UInt512(Int128 value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(Int128 value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (UInt128)value);
		}
		public static explicit operator UInt512(nint value)
		{
			Int256 lower = value;
			return new((UInt256)(lower >> 255), (UInt256)lower);
		}
		public static explicit operator checked UInt512(nint value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, (nuint)value);
		}
		//Floating
		public static explicit operator UInt512(Half value) => (UInt512)(double)value;
		public static explicit operator checked UInt512(Half value) => checked((UInt512)(double)value);
		public static explicit operator UInt512(float value) => (UInt512)(double)value;
		public static explicit operator checked UInt512(float value) => checked((UInt512)(double)value);
		public static explicit operator UInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			if (double.IsNegative(value) || double.IsNaN(value))
			{
				return MinValue;
			}
			else if (value >= TwoPow512)
			{
				return MaxValue;
			}

			return ToUInt512(value);
		}
		public static explicit operator checked UInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			// value against 0 rather than checking IsNegative

			if ((value < 0.0) || double.IsNaN(value) || (0.0 < TwoPow512 - value))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow512 - value)
			{
				return MaxValue;
			}

			return ToUInt512(value);
		}
		public static explicit operator UInt512(decimal value) => (UInt512)(double)value;
		public static explicit operator checked UInt512(decimal value) => checked((UInt512)(double)value);
		#endregion

		private static UInt512 ToUInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			Debug.Assert(value >= 0);
			Debug.Assert(double.IsFinite(value));
			Debug.Assert(0.0 >= TwoPow512 - value);

			if (value >= 1.0)
			{
				// In order to convert from double to uint512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by right shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				ulong bits = BitConverter.DoubleToUInt64Bits(value);

				var exponent = ((bits >> 52) & 0x7FF) - 1023;
				var matissa = (bits & 0x0F_FFFF_FFFF_FFFF) | 0x10_0000_0000_0000;

				if (exponent <= 52)
				{
					return (UInt512)(matissa >> (int)(52 - exponent));
				}
				else if (exponent >= 512)
				{
					return UInt512.MaxValue;
				}
				else
				{
					return ((UInt512)matissa) << ((int)(exponent - 52));
				}
			}
			else
			{
				return MinValue;
			}
		}
	}
}
