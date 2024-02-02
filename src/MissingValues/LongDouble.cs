using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
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
		internal const ushort ShiftedBiasedExponentMask = 0x7FFF;


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
			return NumberFormatter.FloatToString(in this, "G21", NumberFormatInfo.CurrentInfo);
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
			throw new NotImplementedException();
		}
		public static explicit operator double(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator float(LongDouble value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator Half(LongDouble value)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(float value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator LongDouble(Half value)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
