using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	public readonly partial struct Quad
	{
        internal const int Size = 16;

		internal const int SignShift = 127;

		internal const int BiasedExponentShift = 112;


		private readonly UInt128 _value;

        internal Quad(UInt128 value)
        {
            _value = value;
        }
        private Quad(bool sign, UInt128 exp, UInt128 sig)
        {
            _value = (((sign ? UInt128.One : UInt128.Zero) << SignShift) + (exp << BiasedExponentShift) + (sig));
        }

        public static Quad FromUInt128Bits(UInt128 bits)
        {
            return new Quad(bits);
        }
        public static Quad FromInt128Bits(Int128 bits)
        {
            return new Quad((UInt128)bits);
        }
        public static Quad FromByteArray(byte[] bytes)
        {
            return Unsafe.ReadUnaligned<Quad>(ref bytes[0]);
        }
        public static Quad FromByteSpan(ReadOnlySpan<byte> bytes)
        {
			return Unsafe.ReadUnaligned<Quad>(ref MemoryMarshal.GetReference(bytes));
		}

        public static UInt128 ToUInt128Bits(Quad value)
        {
            return value._value;
        }
        public static Int128 ToInt128Bits(Quad value)
        {
            return (Int128)value._value;
        }
        public static byte[] ToByteArray(Quad value)
        {
			byte[] bytes = new byte[Size];
			Unsafe.As<byte, Quad>(ref bytes[0]) = value;
			return bytes;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string? ToString()
		{
			return base.ToString();
		}

		#region From Quad
		#region Signed
		public static explicit operator sbyte(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator short(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator int(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator nint(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator long(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Int128(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Int256(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Int512(Quad value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Unsigned
		public static explicit operator byte(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator ushort(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator uint(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator nuint(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator ulong(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator UInt128(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator UInt256(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator UInt512(Quad value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Floating
		public static explicit operator Half(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator float(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator double(Quad value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator decimal(Quad value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion
		#region To Quad
		#region Signed
		public static explicit operator Quad(sbyte value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(short value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(int value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(nint value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(long value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(Int128 value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Unsigned
		public static explicit operator Quad(byte value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(ushort value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(uint value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(nuint value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(ulong value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(UInt128 value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Floating
		public static implicit operator Quad(Half value)
		{
			throw new NotImplementedException();
		}

		public static implicit operator Quad(float value)
		{
			throw new NotImplementedException();
		}

		public static implicit operator Quad(double value)
		{
			throw new NotImplementedException();
		}

		public static explicit operator Quad(decimal value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion
	}
}
