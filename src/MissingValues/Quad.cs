﻿using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Represents a quadruple-precision floating-point number.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.QuadConverter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct Quad
	{
		internal const int SignShift = 127;
		internal static UInt128 SignMask => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 InvertedSignMask => new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal const int MantissaDigits = 113;
		internal const int ExponentBias = 16383;
		internal const int BiasedExponentShift = 112;
		internal const ulong ShiftedBiasedExponentMask = 32767;

		internal const int MinBiasedExponent = 0x0000;
		internal const int MaxBiasedExponent = 0x7FFF;

		internal static UInt128 BiasedExponentMask => new UInt128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);

		internal static UInt128 TrailingSignificandMask => new UInt128(0x0000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt128 SignificandSignMask => new UInt128(0x0001_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 InvertedSignificandMask => ~(SignificandSignMask | TrailingSignificandMask);
		internal static UInt128 MinTrailingSignificand => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 MaxTrailingSignificand => new UInt128(0x0000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		#region Bit representation of constants
		internal static UInt128 EpsilonBits => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		internal static UInt128 PositiveZeroBits => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 NegativeZeroBits => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 PositiveOneBits => new UInt128(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 NegativeOneBits => new UInt128(0xBFFF_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 PositiveQNaNBits => new UInt128(0x7FFF_8000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 NegativeQNaNBits => new UInt128(0xFFFF_8000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 PositiveInfinityBits => new UInt128(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 NegativeInfinityBits => new UInt128(0xFFFF_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 MaxValueBits => new UInt128(0x7FFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt128 MinValueBits => new UInt128(0xFFFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt128 PiBits => new UInt128(0x4000_921F_B544_42D1, 0x8469_898C_C517_01B8);
		internal static UInt128 TauBits => new UInt128(0x4001_921F_B544_42D1, 0x8469_898C_C517_01B8);
		internal static UInt128 EBits => new UInt128(0x4000_5BF0_A8B1_4576, 0x9535_5FB8_AC40_4E7A);
		#endregion
		#region Constants
		internal static Quad Quarter => new(0x3FFD_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static Quad HalfOne => new(0x3FFE_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static Quad ThreeFourth => new(0x3FFE_8000_0000_0000, 0x0000_0000_0000_0000);
		internal static Quad Two => new(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);
		#endregion

		internal ushort BiasedExponent
		{
			get
			{
				UInt128 bits = QuadToUInt128Bits(this);
				return ExtractBiasedExponentFromBits(bits);
			}
		}
		internal short Exponent
		{
			get
			{
				return (short)(BiasedExponent - ExponentBias);
			}
		}
		internal UInt128 Significand
		{
			get
			{
				return (TrailingSignificand | ((BiasedExponent != 0) ? (SignificandSignMask) : 0U));
			}
		}
		internal UInt128 TrailingSignificand
		{
			get
			{
				UInt128 bits = QuadToUInt128Bits(this);
				return ExtractTrailingSignificandFromBits(bits);
			}
		}

#if BIGENDIAN
        internal readonly ulong _upper;
        internal readonly ulong _lower;
#else
		internal readonly ulong _lower;
		internal readonly ulong _upper;
#endif
		internal Quad(ulong upper, ulong lower)
		{
			_upper = upper;
			_lower = lower;
		}
		[CLSCompliant(false)]
		public Quad(bool sign, ushort exp, UInt128 sig)
		{
			UInt128 value = (((sign ? UInt128.One : UInt128.Zero) << SignShift) + ((((UInt128)exp) << BiasedExponentShift) & BiasedExponentMask) + (sig & TrailingSignificandMask));
			_lower = unchecked((ulong)value);
			_upper = unchecked((ulong)(value >> 64));
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is Quad other) && Equals(other);
		}

		public override int GetHashCode()
		{
			if (IsNaNOrZero(this))
			{
				// All NaNs should have the same hash code, as should both Zeros.
				return HashCode.Combine(QuadToUInt128Bits(this) & PositiveInfinityBits);
			}
			return HashCode.Combine(_lower, _upper);
		}

		public override string? ToString()
		{
			return NumberFormatter.FloatToString(in this, "G33", NumberFormatInfo.CurrentInfo);
		}

		public static Quad Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		public static bool TryParse(ReadOnlySpan<char> s, out Quad result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Quad UInt128BitsToQuad(UInt128 bits) => *((Quad*)&bits);
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Quad Int128BitsToQuad(Int128 bits) => *((Quad*)&bits);

		[CLSCompliant(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe UInt128 QuadToUInt128Bits(Quad value) => *((UInt128*)&value);
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Int128 QuadToInt128Bits(Quad value) => *((Int128*)&value);

		
		internal static ushort ExtractBiasedExponentFromBits(UInt128 bits)
		{
			return (ushort)((bits >> BiasedExponentShift) & ShiftedBiasedExponentMask);
		}
		internal static UInt128 ExtractTrailingSignificandFromBits(UInt128 bits)
		{
			return (bits & TrailingSignificandMask);
		}
		internal static (bool sign, ushort exponent, UInt128 matissa) ExtractFromBits(UInt128 bits)
		{
			return ((bits & SignMask) != 0, (ushort)(bits >> BiasedExponentShift), (bits & TrailingSignificandMask));
		}

		internal static bool AreZero(Quad x, Quad y)
		{
			return ((QuadToUInt128Bits(x) | QuadToUInt128Bits(y)) & ~SignMask) == UInt128.Zero;
		}
		
		internal static bool IsNaNOrZero(Quad value)
		{
			return ((QuadToUInt128Bits(value) - 1) & ~SignMask) >= PositiveInfinityBits;
		}
		
		internal static UInt128 StripSign(Quad value)
		{
			return QuadToUInt128Bits(value) & ~SignMask;
		}

		internal static Quad CreateQuadNaN(bool sign, UInt128 significand)
		{
			return UInt128BitsToQuad(CreateQuadNaNBits(sign, significand));
		}
		internal static UInt128 CreateQuadNaNBits(bool sign, UInt128 significand)
		{
			UInt128 signInt = (sign ? 1UL : 0UL) << 63;
			UInt128 sigInt = significand >> 12;

			return signInt | (BiasedExponentMask | new UInt128(0x0000_8000_0000_0000, 0x0)) | sigInt;
		}

		#region From Quad
		// Unsigned
		public static explicit operator byte(Quad value)
		{
			Quad twoPow8 = new Quad(0x4007_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return byte.MinValue;
			}
			if (value >= twoPow8)
			{
				return byte.MaxValue;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				byte result = (byte)((uint)(bits >> 105) | 0x80);

				result >>= (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		public static explicit operator checked byte(Quad value)
		{
			Quad twoPow8 = new Quad(0x4007_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || value >= twoPow8)
			{
				Thrower.IntegerOverflow();
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				byte result = (byte)((uint)(bits >> 105) | 0x80);

				result >>= (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator ushort(Quad value)
		{
			Quad twoPow16 = new Quad(0x400F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return ushort.MinValue;
			}
			if (value >= twoPow16)
			{
				return ushort.MaxValue;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				ushort result = (ushort)((uint)(bits >> 97) | 0x8000);

				result >>= Quad.ExponentBias + 16 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked ushort(Quad value)
		{
			Quad twoPow16 = new Quad(0x400F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || value >= twoPow16)
			{
				Thrower.IntegerOverflow();
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				ushort result = (ushort)((uint)(bits >> 97) | 0x8000);

				result >>= Quad.ExponentBias + 16 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator uint(Quad value)
		{
			Quad twoPow32 = new Quad(0x401F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return uint.MinValue;
			}
			if (value >= twoPow32)
			{
				return uint.MaxValue;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				uint result = (uint)(bits >> 81) | 0x8000_0000;

				result >>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked uint(Quad value)
		{
			Quad twoPow32 = new Quad(0x401F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || value >= twoPow32)
			{
				Thrower.IntegerOverflow();
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				uint result = (uint)(bits >> 81) | 0x8000_0000;

				result >>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator ulong(Quad value)
		{
			Quad twoPow64 = new Quad(0x403F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return ulong.MinValue;
			}
			if (value >= twoPow64)
			{
				return ulong.MaxValue;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				ulong result = (ulong)(bits >> 49) | 0x8000_0000_0000_0000;

				result >>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked ulong(Quad value)
		{
			Quad twoPow64 = new Quad(0x403F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || value >= twoPow64)
			{
				Thrower.IntegerOverflow();
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				ulong result = (ulong)(bits >> 49) | 0x8000_0000_0000_0000;

				result >>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator UInt128(Quad value)
		{
			Quad twoPow128 = new Quad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return UInt128.MinValue;
			}
			if (value >= twoPow128)
			{
				return UInt128.MaxValue;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt128 result = (bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0);

				result >>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt128(Quad value)
		{
			Quad twoPow128 = new Quad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || (value >= twoPow128))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt128 result = (bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0);

				result >>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator UInt256(Quad value)
		{
			Quad twoPow256 = new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return UInt256.MinValue;
			}
			else if ((value >= twoPow256))
			{
				return UInt256.MaxValue;
			}

			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt256 result = new UInt256((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero);

				result >>= Quad.ExponentBias + 256 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt256(Quad value)
		{
			Quad twoPow256 = new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || (value >= twoPow256))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt256 result = new UInt256((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero);

				result >>= Quad.ExponentBias + 256 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator UInt512(Quad value)
		{
			Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative)
			{
				return UInt512.MinValue;
			}
			else if ((value >= twoPow512))
			{
				return UInt512.MaxValue;
			}

			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt512 result = new UInt512((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero, UInt128.Zero, UInt128.Zero);

				result >>= Quad.ExponentBias + 512 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		[CLSCompliant(false)]
		public static explicit operator checked UInt512(Quad value)
		{
			Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Quad.IsNegative(value);

			if (Quad.IsNaN(value) || isNegative || (value >= twoPow512))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				UInt512 result = new UInt512((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero, UInt128.Zero, UInt128.Zero);

				result >>= Quad.ExponentBias + 512 - 1 - (int)(bits >> 112);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		// Signed
		[CLSCompliant(false)]
		public static explicit operator sbyte(Quad value)
		{
			return (sbyte)(long)value;
		}
		[CLSCompliant(false)]
		public static explicit operator checked sbyte(Quad value)
		{
			return checked((sbyte)(long)value);
		}
		public static explicit operator short(Quad value)
		{
			return (short)(long)value;
		}
		public static explicit operator checked short(Quad value)
		{
			return checked((short)(long)value);
		}
		public static explicit operator int(Quad value)
		{
			return (int)(long)value;
		}
		public static explicit operator checked int(Quad value)
		{
			return checked((int)(long)value);
		}
		public static explicit operator long(Quad value)
		{
			Quad twoPow63 = new Quad(0x403E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63)
			{
				return long.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow63)
			{
				return long.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				long result = (long)((ulong)(bits >> 49) | 0x8000_0000_0000_0000);

				result >>>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);

				if (isNegative)
				{
					result = -result;
				}
				return result;
			}
			else
			{
				return 0L;
			}
		}
		public static explicit operator checked long(Quad value)
		{
			Quad twoPow63 = new Quad(0x403E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63 || Quad.IsNaN(value) || value >= +twoPow63)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				long result = (long)((ulong)(bits >> 49) | 0x8000_0000_0000_0000);

				result >>>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);

				if (isNegative)
				{
					result = -result;
				}
				return result;
			}
			else
			{
				return 0L;
			}
		}
		public static explicit operator Int128(Quad value)
		{
			Quad twoPow127 = new Quad(0x407E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127)
			{
				return Int128.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return Int128.Zero;
			}
			else if (value >= +twoPow127)
			{
				return Int128.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int128 result = (Int128)(((bits << 16) >> 1) | Quad.SignMask);

				result >>>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);

				if (isNegative)
				{
					result = -result;
				}
				return result;
			}
			else
			{
				return Int128.Zero;
			}
		}
		public static explicit operator checked Int128(Quad value)
		{
			Quad twoPow127 = new Quad(0x407E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127 || Quad.IsNaN(value) || value >= +twoPow127)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int128 result = (Int128)(((bits << 16) >> 1) | Quad.SignMask);

				result >>>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);

				if (isNegative)
				{
					result = -result;
				}
				return result;
			}
			else
			{
				return Int128.Zero;
			}
		}
		public static explicit operator Int256(Quad value)
		{
			Quad twoPow255 = new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255)
			{
				return Int256.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return Int256.Zero;
			}
			else if (value >= +twoPow255)
			{
				return Int256.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

            if (isNegative)
            {
                value = -value;
            }

			if (value >= Quad.One)
			{
				// In order to convert from Quad to int256 we first need to extract the signficand,
				// including the implicit leading bit, as a full 256-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 256-bits.

				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int256 result = new Int256((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero);

				result >>>= (Quad.ExponentBias + 256 - 1 - (int)(bits >> 112));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int256.Zero;
			}
		}
		public static explicit operator checked Int256(Quad value)
		{
			Quad twoPow255 = new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255 || Quad.IsNaN(value) || value >= +twoPow255)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Quad.IsNegative(value);

            if (isNegative)
            {
                value = -value;
            }

			if (value >= Quad.One)
			{
				// In order to convert from Quad to int256 we first need to extract the signficand,
				// including the implicit leading bit, as a full 256-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 256-bits.

				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int256 result = new Int256((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero);

				result >>>= (Quad.ExponentBias + 256 - 1 - (int)(bits >> 112));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int256.Zero;
			}
		}
		public static explicit operator Int512(Quad value)
		{
			Quad twoPow511 = new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511)
			{
				return Int512.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return Int512.Zero;
			}
			else if (value >= +twoPow511)
			{
				return Int512.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Quad.One)
			{
				// In order to convert from Quad to int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int512 result = new Int512((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero, UInt128.Zero, UInt128.Zero);

				result >>>= (Quad.ExponentBias + 512 - 1 - (int)(bits >> 112));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int512.Zero;
			}
		}
		public static explicit operator checked Int512(Quad value)
		{
			Quad twoPow511 = new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511 || Quad.IsNaN(value) || value >= +twoPow511)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Quad.One)
			{
				// In order to convert from Quad to int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt128 bits = Quad.QuadToUInt128Bits(value);
				Int512 result = new Int512((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero, UInt128.Zero, UInt128.Zero);

				result >>>= (Quad.ExponentBias + 512 - 1 - (int)(bits >> 112));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int512.Zero;
			}
		}
		// Floating
		public static explicit operator decimal(Quad value)
		{
			return (decimal)(double)value;
		}
		public static explicit operator double(Quad value)
		{
			UInt128 quadInt = QuadToUInt128Bits(value);
			bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
			int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
			UInt128 sig = quadInt & Quad.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return CreateDoubleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? double.NegativeInfinity : double.PositiveInfinity;
			}

			sig <<= 14;
			ulong sigQuad = (ulong)(sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL);

			if (((uint)exp | sigQuad) == 0)
			{
				return CreateDouble(sign, 0, 0);
			}

			exp -= 0x3C01;

			exp = exp < -0x1000 ? -0x1000 : exp;

			return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, (short)(exp), (sigQuad | 0x4000_0000_0000_0000)));
		}
		public static explicit operator float(Quad value)
		{
			UInt128 quadInt = QuadToUInt128Bits(value);
			bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
			int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
			UInt128 sig = quadInt & Quad.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return CreateSingleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? float.NegativeInfinity : float.PositiveInfinity;
			}

			uint sigQuad = (uint)BitHelper.ShiftRightJam((ulong)((sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL)), 18);

			if (((uint)exp | sigQuad) == 0)
			{
				return CreateSingle(sign, 0, 0);
			}

			exp -= 0x3F81;

			exp = exp < -0x1000 ? -0x1000 : exp;

			return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, (short)(exp), (uint)(sigQuad | 0x4000_0000)));
		}
		public static explicit operator Half(Quad value)
		{
			UInt128 quadInt = QuadToUInt128Bits(value);
			bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
			int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
			UInt128 sig = quadInt & Quad.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return CreateHalfNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? Half.NegativeInfinity : Half.PositiveInfinity;
			}

			ushort sigHalf = (ushort)BitHelper.ShiftRightJam((ulong)((sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL)), 34);

			if (((uint)exp | sigHalf) == 0)
			{
				return CreateHalf(sign, 0, 0);
			}

			exp -= 0x3FF1;

			exp = exp < -0x40 ? -0x40 : exp;

			return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, (short)(exp), (ushort)(sigHalf | 0x4000)));
		}
		#endregion
		#region To Quad
		// Unsigned
		public static implicit operator Quad(byte value)
		{
			return (Quad)(uint)value;
		}
		[CLSCompliant(false)]
		public static implicit operator Quad(ushort value)
		{
			return (Quad)(uint)value;
		}
		[CLSCompliant(false)]
		public static implicit operator Quad(uint value)
		{
			int shiftDist;
			ulong z = 0;

			if (value != 0)
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 17;
				z = BitHelper.PackToQuadUI64(false, 0x402E - shiftDist, (ulong)value << shiftDist);
			}

			return new Quad(z, 0);
		}
		[CLSCompliant(false)]
		public static implicit operator Quad(ulong value)
		{
			UInt128 sig;
			int shiftDist;

			if ((value) == 0)
			{
				return Quad.Zero;
			}
			else
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 49;
				if (shiftDist >= 64)
				{
					sig = new UInt128(value << (shiftDist - 64), 0);
				}
				else
				{
					sig = (UInt128)value << shiftDist;
				}
			}

			return new Quad(false, (ushort)(0x406F - shiftDist), sig);
		}
		[CLSCompliant(false)]
		public static implicit operator Quad(UInt128 value)
		{
			if (value == UInt128.Zero)
			{
				return Quad.Zero;
			}
			int shiftDist = (int)UInt128.LeadingZeroCount(value);
			UInt128 a = (value << shiftDist >> 15); // Significant bits, with bit 113 still intact
			UInt128 b = (value << shiftDist << 113); // Insignificant bits, only relevant for rounding.
			UInt128 m = a + ((b - (b >> 127 & (a == UInt128.Zero ? UInt128.One : UInt128.Zero))) >> 127); // Add one when we need to round up. Break ties to even.
			UInt128 e = (UInt128)(0x407D - shiftDist); // Exponent plus 16383, minus one, except for zero.
			return Quad.UInt128BitsToQuad((e << 112) + m);
		}
		// Signed
		[CLSCompliant(false)]
		public static implicit operator Quad(sbyte value)
		{
			if (sbyte.IsNegative(value))
			{
				value = (sbyte)-value;
				return -(Quad)(byte)value;
			}
			return (Quad)(byte)value;
		}
		public static implicit operator Quad(short value)
		{
			if (short.IsNegative(value))
			{
				value = (short)-value;
				return -(Quad)(ushort)value;
			}
			return (Quad)(ushort)value;
		}
		public static implicit operator Quad(int value)
		{
			if (int.IsNegative(value))
			{
				value = -value;
				return -(Quad)(uint)value;
			}
			return (Quad)(uint)value;
		}
		public static implicit operator Quad(long value)
		{
			if (long.IsNegative(value))
			{
				value = -value;
				return -(Quad)(ulong)value;
			}
			return (Quad)(ulong)value;
		}
		public static implicit operator Quad(Int128 value)
		{
			if (Int128.IsNegative(value))
			{
				value = -value;
				return -(Quad)(UInt128)value;
			}
			return (Quad)(UInt128)value;
		}
		// Floating
		public static explicit operator Quad(decimal value)
		{
			return (Quad)(double)value;
		}
		public static implicit operator Quad(double value)
		{
			const int MaxBiasedExponentDouble = 0x07FF;
			const int DoubleExponentBias = 1023;

			ulong bits = BitConverter.DoubleToUInt64Bits(value);
			bool sign = double.IsNegative(value);
			int exp = (ushort)((bits >> 52) & 0x07FF);
			ulong sig = bits & 0x000F_FFFF_FFFF_FFFF;

			if (exp == MaxBiasedExponentDouble)
			{
				if (sig != 0)
				{
					return CreateQuadNaN(sign, (UInt128)sig << 76);
				}
				return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF64Sig(sig);
				exp -= 1;
			}

			return new Quad(sign, (ushort)(exp + (ExponentBias - DoubleExponentBias)), (UInt128)sig << 60);
		}
		public static implicit operator Quad(float value)
		{
			const int MaxBiasedExponentSingle = 0xFFFF;
			const int SingleExponentBias = 127;

			uint bits = BitConverter.SingleToUInt32Bits(value);
			bool sign = float.IsNegative(value);
			int exp = (ushort)((bits >> 23) & 0xFF);
			uint sig = bits & 0x007F_FFFF;

			if (exp == MaxBiasedExponentSingle)
			{
				if (sig != 0)
				{
					return CreateQuadNaN(sign, (UInt128)sig << 105);
				}
				return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
				exp -= 1;
			}

			return new Quad(sign, (ushort)(exp + (ExponentBias - SingleExponentBias)), (UInt128)sig << 89);
		}
		public static implicit operator Quad(Half value)
		{
			const int MaxBiasedExponentHalf = 0x1F;
			const int HalfExponentBias = 15;

			ushort bits = BitConverter.HalfToUInt16Bits(value);
			bool sign = Half.IsNegative(value);
			int exp = (ushort)((bits >> 10) & 0x1F);
			uint sig = (uint)(bits & 0x03FF);

			if (exp == MaxBiasedExponentHalf)
			{
				if (sig != 0)
				{
					return CreateQuadNaN(sign, (UInt128)sig << 118);
				}
				return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
				exp -= 1;
			}

			return new Quad(sign, (ushort)(exp + (ExponentBias - HalfExponentBias)), (UInt128)sig << 102);
		}
		#endregion

		private static double CreateDoubleNaN(bool sign, ulong significand)
		{
			const ulong NaNBits = 0x7FF0_0000_0000_0000 | 0x80000_00000000; // Most significant significand bit

			ulong signInt = (sign ? 1UL : 0UL) << 63;
			ulong sigInt = significand >> 12;

			return BitConverter.UInt64BitsToDouble(signInt | NaNBits | sigInt);
		}
		private static double CreateDouble(bool sign, ushort exp, ulong sig)
		{
			return BitConverter.UInt64BitsToDouble(((sign ? 1UL : 0UL) << 63) + ((ulong)exp << 52) + sig);
		}

		private static float CreateSingleNaN(bool sign, ulong significand)
		{
			const uint NaNBits = 0x7F80_0000 | 0x400000; // Most significant significand bit

			uint signInt = (sign ? 1U : 0U) << 31;
			uint sigInt = (uint)(significand >> 41);

			return BitConverter.UInt32BitsToSingle(signInt | NaNBits | sigInt);
		}
		private static float CreateSingle(bool sign, byte exp, uint sig)
		{
			return BitConverter.UInt32BitsToSingle(((sign ? 1U : 0U) << 31) + ((uint)exp << 23) + sig);
		}

		private static Half CreateHalfNaN(bool sign, ulong significand)
		{
			const uint NaNBits = 0x7C00 | 0x200; // Most significant significand bit

			uint signInt = (sign ? 1U : 0U) << 15;
			uint sigInt = (uint)(significand >> 54);

			return BitConverter.UInt16BitsToHalf((ushort)(signInt | NaNBits | sigInt));
		}
		private static Half CreateHalf(bool sign, ushort exp, ushort sig)
		{
			return BitConverter.UInt16BitsToHalf(((ushort)(((sign ? 1 : 0) << 15) + (exp << 10) + sig)));
		}
	}
}
