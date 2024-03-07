using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Represents a x86 extended-precision floating-point number.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public readonly partial struct LongDouble
	{
		internal static LongDouble SignMask => new LongDouble(0b1000_0000_0000_0000, 0);
		internal const ulong SignShift = 79;

		internal const ushort ShiftedBiasedExponentMask = 0x7FFF;

		internal ulong Significand
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _lower;
			}
		}
		internal short Exponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (short)(_upper & 0x7FFF);
			}
		}
		internal bool Sign
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (_upper >>> 15) != 0;
			}
		}

		internal static LongDouble Two => new LongDouble(0x4000, 0x8000000000000000);

#if BIGENDIAN
        internal readonly ushort _upper;
        internal readonly ulong _lower;
#else
		internal readonly ulong _lower;
		internal readonly ushort _upper;
#endif

		internal LongDouble(ushort upper, ulong lower)
		{
			_lower = lower;
			_upper = upper;
		}
		[CLSCompliant(false)]
		public LongDouble(bool sign, ushort exp, ulong sig)
        {
			_lower = sig;
			_upper = (ushort)(sign ? ((1 << 15) | (exp & ShiftedBiasedExponentMask)) : (exp & ShiftedBiasedExponentMask));
        }

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is LongDouble other) && Equals(other);
		}

		public override int GetHashCode()
		{
			if (IsNaN(this) || this == Zero)
			{
				// All NaNs should have the same hash code, as should both Zeros.
				LongDouble temp = this & PositiveInfinity;
				return HashCode.Combine(temp._upper, temp._lower);
			}
			return HashCode.Combine(_lower, _upper);
		}

		public override string? ToString()
		{
			return NumberFormatter.FloatToString(in this, "G18", NumberFormatInfo.CurrentInfo);
		}

		public static LongDouble Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		public static bool TryParse(ReadOnlySpan<char> s, out LongDouble result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		#region From LongDouble
		// Unsigned
		public static explicit operator byte(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked byte(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator ushort(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked ushort(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator uint(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked uint(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator ulong(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked ulong(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator UInt128(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt128(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator UInt256(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt256(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator UInt512(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt512(LongDouble value)
		{
			throw new NotImplementedException();
		}
		// Signed
		[CLSCompliant(false)]
		public static explicit operator sbyte(LongDouble value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static explicit operator checked sbyte(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator short(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked short(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator int(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked int(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator long(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked long(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator Int128(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked Int128(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator Int256(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked Int256(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator Int512(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator checked Int512(LongDouble value)
		{
			throw new NotImplementedException();
		}
		// Floating
		public static explicit operator decimal(LongDouble value)
		{
			return (decimal)(double)value;
		}
		public static implicit operator Quad(LongDouble value)
		{
			// Source: berkeley-softfloat-3/source/extF80_to_f128.c
			var sign = value.Sign;
			var exp = value.Exponent;
			var sig = value.Significand;
			var frac = sig & 0x7FFF_FFFF_FFFF_FFFF;

			if ((exp == 0x7FFF) && frac != 0)
			{
				return Quad.NaN;
			}
            else
            {
				var frac128 = (UInt128)frac << 49;
				return new Quad(sign, (ushort)exp, frac128);
            }
		}
		public static explicit operator double(LongDouble value)
		{
			// Source: berkeley-softfloat-3/source/extF80_to_f64.c
			var sign = value.Sign;
			var exp = value.Exponent;
			var sig = value.Significand;

			if (exp == 0x7FFF)
			{
				ulong uiZBits;
				if ((sig & 0x7FFF_FFFF_FFFF_FFFF) != 0)
				{
					return float.NaN;
				}
				uiZBits = BitHelper.PackToDouble(sign, 0x7FF, 0);
				return BitConverter.UInt64BitsToDouble(uiZBits);
			}

			if (((ushort)exp | sig) == 0)
			{
				return BitConverter.UInt64BitsToDouble(BitHelper.PackToDouble(sign, 0x0, 0));
			}
			sig = BitHelper.ShiftRightJam(sig, 1);

			exp -= 0x3C01;
			if (exp < -0x1000) exp = -0x1000;

			return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, exp, sig));
		}
		public static explicit operator float(LongDouble value)
		{
			// Source: berkeley-softfloat-3/source/extF80_to_f32.c
			var sign = value.Sign;
			var exp = value.Exponent;
			var sig = value.Significand;

			if (exp == 0x7FFF)
			{
				uint uiZBits;
				if ((sig & 0x7FFF_FFFF_FFFF_FFFF) != 0)
				{
					return float.NaN;
				}
				uiZBits = BitHelper.PackToSingle(sign, 0xFF, 0);
				return BitConverter.UInt32BitsToSingle(uiZBits);
			}

			var sig32 = BitHelper.ShiftRightJam(sig, 33);
			if (((ushort)exp | sig32) == 0)
			{
				return BitConverter.UInt32BitsToSingle(BitHelper.PackToSingle(sign, 0x0, 0));
			}

			exp -= 0x3F81;
			if (exp < -0x1000) exp = -0x1000;

			return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, exp, (uint)sig32));
		}
		public static explicit operator Half(LongDouble value)
		{
			// Source: berkeley-softfloat-3/source/extF80_to_f16.c
			var sign = value.Sign;
			var exp = value.Exponent;
			var sig = value.Significand;

			if (exp == 0x7FFF)
			{
				ushort uiZBits;
				if ((sig & 0x7FFF_FFFF_FFFF_FFFF) != 0)
				{
					return Half.NaN;
				}
				uiZBits = BitHelper.PackToHalf(sign, 0x1F, 0);
				return BitConverter.UInt16BitsToHalf(uiZBits);
			}

			var sig16 = BitHelper.ShiftRightJam(sig, 49);
			if (((ushort)exp | sig16) == 0)
			{
				return BitConverter.UInt16BitsToHalf(BitHelper.PackToHalf(sign, 0, 0));
			}

			exp -= 0x3FF1;
			if (exp < -0x40)
			{
				exp = -0x40;
			}

			return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, exp, (ushort)sig16));
		}
		#endregion
		#region To LongDouble
		// Unsigned
		public static implicit operator LongDouble(byte value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static implicit operator LongDouble(ushort value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static implicit operator LongDouble(uint value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static implicit operator LongDouble(ulong value)
		{
			throw new NotImplementedException();
		}
		[CLSCompliant(false)]
		public static implicit operator LongDouble(UInt128 value)
		{
			throw new NotImplementedException();
		}
		// Signed
		[CLSCompliant(false)]
		public static implicit operator LongDouble(sbyte value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(short value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(int value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(long value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(Int128 value)
		{
			throw new NotImplementedException();
		}
		// Floating
		public static implicit operator LongDouble(decimal value)
		{
			return (LongDouble)(double)value;
		}
		public static implicit operator LongDouble(double value)
		{
			const int SignificandBitShift = 52;
			const ulong ExponentMask = 0x7FF0_0000_0000_0000;
			const ulong SignificandMask = 0x000F_FFFF_FFFF_FFFF;

			var bits = BitConverter.DoubleToUInt64Bits(value);
			var sign = double.IsNegative(value);
			var exp = ((bits & ExponentMask) >> SignificandBitShift);
			var frac = bits & SignificandMask;

			if (exp == 0x7FF)
			{
				if (frac != 0)
				{
					return LongDouble.NaN;
				}
				else
				{
					return new LongDouble(sign, 0x7FFF, 0x8000000000000000);
				}
			}

			if (exp == 0)
			{
				if (frac == 0)
				{
					return new LongDouble(sign, 0, 0);
				}
				(exp, frac) = BitHelper.NormalizeSubnormalF64Sig(frac);
			}

			return new LongDouble((ushort)((sign ? 1U << 15 : 0) | (uint)(exp + 0x3C00)), (frac | 0x0010000000000000) << 11);
		}
		public static implicit operator LongDouble(float value)
		{
			const int SignificandBitShift = 23;
			const uint ExponentMask = 0x7F80_0000;
			const uint SignificandMask = 0x007F_FFFF;

			var bits = BitConverter.SingleToUInt32Bits(value);
			var sign = float.IsNegative(value);
			var exp = ((bits & ExponentMask) >> SignificandBitShift);
			var frac = bits & SignificandMask;

			if (exp == 0xFF)
			{
				if (frac != 0)
				{
					return LongDouble.NaN;
				}
				else
				{
					return new LongDouble(sign, 0x7FFF, 0x8000000000000000);
				}
			}

			if (exp == 0)
			{
				if (frac == 0)
				{
					return new LongDouble(sign, 0, 0);
				}
				(exp, frac) = BitHelper.NormalizeSubnormalF32Sig(frac);
			}

			return new LongDouble((ushort)((sign ? 1U << 15 : 0) | (exp + 0x3F80)), (ulong)(frac | 0x00800000) << 40);
		}
		public static implicit operator LongDouble(Half value)
		{
			const int SignificandBitShift = 10;
			const ushort ExponentMask = 0x7C00;
			const ushort SignificandMask = 0x03FF;

			var bits = BitConverter.HalfToUInt16Bits(value);
			var sign = Half.IsNegative(value);
			var exp = ((bits & ExponentMask) >> SignificandBitShift);
			var frac = bits & SignificandMask;

			if (exp == 0xFF)
			{
				if (frac != 0)
				{
					return LongDouble.NaN;
				}
				else
				{
					return new LongDouble(sign, 0x7FFF, 0x8000000000000000);
				}
			}

			if (exp == 0)
			{
				if (frac == 0)
				{
					return new LongDouble(sign, 0, 0);
				}
				(exp, frac) = BitHelper.NormalizeSubnormalF16Sig((ushort)frac);
			}

			return new LongDouble((ushort)((sign ? 1U << 15 : 0) | (exp + 0x3FF0)), (ulong)(frac | 0x0400) << 53);
		}
		#endregion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool LessThan(ushort a64, ulong a0, ushort b64, ulong b0)
		{
			return (a64 < b64) || ((a64 == b64) && (a0 < b0));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool LessThanOrEqual(ushort a64, ulong a0, ushort b64, ulong b0)
		{
			return (a64 < b64) || ((a64 == b64) && (a0 <= b0));
		}
		private static bool AreZero(LongDouble left, LongDouble right)
		{
			var temp = ((left | right) & ~SignMask);
			return (temp._upper | temp._lower) == 0;
		}
		private static LongDouble StripSign(LongDouble value)
		{
			return new LongDouble((ushort)(value._upper & ~0x8000), value._lower & 0xFFFF_FFFF_FFFF_FFFF);
		}
	}
}
