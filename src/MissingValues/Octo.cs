using MissingValues.Info;
using MissingValues.Internals;
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
	/// Represents an octuple-precision floating-point number.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.OctoConverter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct Octo
	{
		internal static UInt256 SignMask => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 InvertedSignMask => new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal const int SignShift = 255;
		internal const int MantissaDigits = 237;
		internal const int ExponentBias = 262143;
		internal const int BiasedExponentShift = 236;
		internal const ulong ShiftedBiasedExponentMask = 524_287;

		internal const int MinBiasedExponent = 0x00000;
		internal const int MaxBiasedExponent = 0x7FFFF;
		internal static UInt256 BiasedExponentMask => new UInt256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		internal static UInt256 TrailingSignificandMask => new UInt256(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 SignificandSignMask => new UInt256(0x0000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 InvertedSignificandMask => ~(SignificandSignMask | TrailingSignificandMask);
		internal static UInt256 MinTrailingSignificand => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 MaxTrailingSignificand => new UInt256(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal UInt128 Lower => new UInt128(_bits1, _bits0);
		internal UInt128 Upper => new UInt128(_bits3, _bits2);

		#region Bit representation of constants
		internal static UInt256 EpsilonBits => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		internal static UInt256 PositiveZeroBits => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeZeroBits => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveOneBits => new UInt256(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeOneBits => new UInt256(0xBFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveQNaNBits => new UInt256(0x7FFF_F800_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeQNaNBits => new UInt256(0xFFFF_F800_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveInfinityBits => new UInt256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeInfinityBits => new UInt256(0xFFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 MaxValueBits => new UInt256(0x7FFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 MinValueBits => new UInt256(0xFFFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 PiBits => throw new NotImplementedException(); // TODO: Add Pi bits
		internal static UInt256 TauBits => throw new NotImplementedException(); // TODO: Add Tau bits
		internal static UInt256 EBits => throw new NotImplementedException(); // TODO: Add E bits
		#endregion
		#region Constants
		internal static Octo Quarter => new Octo(0x3FFF_D000_0000_0000, 0, 0, 0);
		internal static Octo HalfOne => new Octo(0x3FFF_E000_0000_0000, 0, 0, 0);
		internal static Octo Two => new Octo(0x4000_0000_0000_0000, 0, 0, 0);
		internal static ReadOnlySpan<Octo> RoundPower10 => throw new NotImplementedException() /*new Octo[71]*/;
		#endregion

		internal uint BiasedExponent
		{
			get
			{
				UInt256 bits = OctoToUInt256Bits(this);
				return ExtractBiasedExponentFromBits(bits);
			}
		}
		internal int Exponent
		{
			get
			{
				return (int)(BiasedExponent - ExponentBias);
			}
		}
		internal UInt256 Significand
		{
			get
			{
				return (TrailingSignificand | ((BiasedExponent != 0) ? (SignificandSignMask) : UInt256.Zero));
			}
		}
		internal UInt256 TrailingSignificand
		{
			get
			{
				UInt256 bits = OctoToUInt256Bits(this);
				return ExtractTrailingSignificandFromBits(bits);
			}
		}

#if BIGENDIAN
		private readonly ulong _bits3;
		private readonly ulong _bits2;
		private readonly ulong _bits1;
		private readonly ulong _bits0;
#else
		private readonly ulong _bits0;
		private readonly ulong _bits1;
		private readonly ulong _bits2;
		private readonly ulong _bits3;
#endif

		internal Octo(ulong u1, ulong u2, ulong l1, ulong l2)
		{
			_bits0 = l2;
			_bits1 = l1;
			_bits2 = u2;
			_bits3 = u1;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Octo" /> struct.
		/// </summary>
		/// <param name="sign">A <see cref="bool"/> indicating the sign of the number. <see langword="true"/> represents a negative number, and <see langword="false"/> represents a positive number.</param>
		/// <param name="exp">An <see cref="uint"/> representing the exponent part of the floating-point number.</param>
		/// <param name="sig">An <see cref="UInt256"/> representing the significand part of the floating-point number.</param>
		public Octo(bool sign, uint exp, UInt256 sig)
		{
			var bits = (sig & TrailingSignificandMask) | new UInt256((sign ? 0x8000_0000_0000_0000 : 0) | ((ulong)(exp & 0x7FFFF) << 44),0,0,0);
			_bits0 = bits.Part0;
			_bits1 = bits.Part1;
			_bits2 = bits.Part2;
			_bits3 = bits.Part3;
		}

		/// <inheritdoc/>
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is Octo other) && Equals(other);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <inheritdoc/>
		public override string? ToString()
		{
			return ToString("G70", NumberFormatInfo.CurrentInfo);
		}

		/// <summary>
		/// Reinterprets the specified 256-bit unsigned integer to a octuple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Octo UInt256BitsToOcto(UInt256 bits) => *((Octo*)&bits);
		/// <summary>
		/// Reinterprets the specified 256-bit signed integer to a octuple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Octo Int256BitsToOcto(Int256 bits) => *((Octo*)&bits);

		/// <summary>
		/// Converts the specified octuple-precision floating point number to a 256-bit unsigned integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 256-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe UInt256 OctoToUInt256Bits(Octo value) => *((UInt256*)&value);
		/// <summary>
		/// Converts the specified octuple-precision floating point number to a 256-bit signed integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 256-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Int256 OctoToInt256Bits(Octo value) => *((Int256*)&value);


		internal static bool AreZero(in Octo x, in Octo y)
		{
			return ((OctoToUInt256Bits(x) | OctoToUInt256Bits(y)) & ~SignMask) == UInt256.Zero;
		}
		internal static uint ExtractBiasedExponentFromBits(in UInt256 bits)
		{
			return (uint)((bits >> BiasedExponentShift) & ShiftedBiasedExponentMask);
		}
		internal static UInt256 ExtractTrailingSignificandFromBits(in UInt256 bits)
		{
			return (bits & TrailingSignificandMask);
		}
		internal static UInt256 StripSign(Octo value)
		{
			return OctoToUInt256Bits(value) & ~SignMask;
		}
		// TODO: Add casting to other primitive types.
		#region From Octo
		// Unsigned
		public static explicit operator byte(Octo value)
		{
			Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return byte.MinValue;
			}
			if ((value >= twoPow8))
			{
				return byte.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				byte result = (byte)((byte)(bits >> 229) | 0x80);

				result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		public static explicit operator checked byte(Octo value)
		{
			Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow8))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				byte result = (byte)((byte)(bits >> 229) | 0x80);

				result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		public static explicit operator ushort(Octo value)
		{
			Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return ushort.MinValue;
			}
			if ((value >= twoPow16))
			{
				return ushort.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ushort result = (ushort)((ushort)(bits >> 221) | 0x8000);

				result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		public static explicit operator checked ushort(Octo value)
		{
			Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow16))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ushort result = (ushort)((ushort)(bits >> 221) | 0x8000);

				result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		public static explicit operator uint(Octo value)
		{
			Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return uint.MinValue;
			}
			if ((value >= twoPow32))
			{
				return uint.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				uint result = ((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		public static explicit operator checked uint(Octo value)
		{
			Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow32))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				uint result = ((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		public static explicit operator ulong(Octo value)
		{
			Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return ulong.MinValue;
			}
			if ((value >= twoPow64))
			{
				return ulong.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ulong result = ((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		public static explicit operator checked ulong(Octo value)
		{
			Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow64))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ulong result = ((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		public static explicit operator UInt128(Octo value)
		{
			Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt128.MinValue;
			}
			if ((value >= twoPow128))
			{
				return UInt128.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		public static explicit operator checked UInt128(Octo value)
		{
			Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow128))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		public static explicit operator UInt256(Octo value)
		{
			Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt256.MinValue;
			}
			if ((value >= twoPow256))
			{
				return UInt256.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		public static explicit operator checked UInt256(Octo value)
		{
			Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow256))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		public static explicit operator UInt512(Octo value)
		{
			Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt512.MinValue;
			}
			if ((value >= twoPow512))
			{
				return UInt512.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>= Octo.ExponentBias + 512 - 1 - (int)(bits >> 236);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		public static explicit operator checked UInt512(Octo value)
		{
			Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow512))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>= Octo.ExponentBias + 512 - 1 - (int)(bits >> 236);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		// Signed
		public static explicit operator sbyte(Octo value)
		{
			Octo twoPow7 = new Octo(0x4000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7)
			{
				return sbyte.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow7)
			{
				return sbyte.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				sbyte result = (sbyte)(((byte)(bits >> 229) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (sbyte)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator checked sbyte(Octo value)
		{
			Octo twoPow7 = new Octo(0x4000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7 || Octo.IsNaN(value) || value >= +twoPow7)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				sbyte result = (sbyte)(((byte)(bits >> 229) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (sbyte)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator short(Octo value)
		{
			Octo twoPow15 = new Octo(0x4000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15)
			{
				return short.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow15)
			{
				return short.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				short result = (short)(((ushort)(bits >> 221) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (short)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator checked short(Octo value)
		{
			Octo twoPow15 = new Octo(0x4000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15 || Octo.IsNaN(value) || value >= +twoPow15)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				short result = (short)(((ushort)(bits >> 221) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (short)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator int(Octo value)
		{
			Octo twoPow31 = new Octo(0x4001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31)
			{
				return int.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow31)
			{
				return int.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				int result = (int)((uint)(bits >> 205) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator checked int(Octo value)
		{
			Octo twoPow31 = new Octo(0x4001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31 || Octo.IsNaN(value) || value >= +twoPow31)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				int result = (int)((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator long(Octo value)
		{
			Octo twoPow63 = new Octo(0x4003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63)
			{
				return long.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow63)
			{
				return long.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				long result = (long)((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator checked long(Octo value)
		{
			Octo twoPow63 = new Octo(0x4003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63 || Octo.IsNaN(value) || value >= +twoPow63)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				long result = (long)((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		public static explicit operator Int128(Octo value)
		{
			Octo twoPow127 = new Octo(0x4007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127)
			{
				return Int128.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int128.Zero;
			}
			else if (value >= +twoPow127)
			{
				return Int128.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));

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
		public static explicit operator checked Int128(Octo value)
		{
			Octo twoPow127 = new Octo(0x4007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127 || Octo.IsNaN(value) || value >= +twoPow127)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));

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
		public static explicit operator Int256(Octo value)
		{
			Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255)
			{
				return Int256.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int256.Zero;
			}
			else if (value >= +twoPow255)
			{
				return Int256.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));

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
		public static explicit operator checked Int256(Octo value)
		{
			Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255 || Octo.IsNaN(value) || value >= +twoPow255)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));

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
		public static explicit operator Int512(Octo value)
		{
			Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511)
			{
				return Int512.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int512.Zero;
			}
			else if (value >= +twoPow511)
			{
				return Int512.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				// In order to convert from Quad to Int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits >> 236));

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
		public static explicit operator checked Int512(Octo value)
		{
			Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511 || Octo.IsNaN(value) || value >= +twoPow511)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				// In order to convert from Quad to Int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits >> 236));

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
		public static explicit operator decimal(Octo value)
		{
			return (decimal)(double)value;
		}
		public static explicit operator Quad(Octo value)
		{
			UInt256 octoInt = OctoToUInt256Bits(value);
			bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
			int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
			UInt256 sig = octoInt & Octo.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return BitHelper.CreateQuadNaN(sign, (UInt128)(sig >> 108)); // Shift the significand bits to the x end
				}
				return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			sig <<= 18;
			UInt128 sigOcto = (sig.Upper) | ((UInt128)sig != UInt128.Zero ? UInt128.One : UInt128.Zero);

			if (((uint)exp | sigOcto) == UInt128.Zero)
			{
				return new Quad(sign, 0, UInt128.Zero);
			}

			exp -= 0x3_C001;

			exp = exp < -0x1_0000 ? -0x1_0000 : exp;

			return Quad.UInt128BitsToQuad(BitHelper.RoundPackToQuad(sign, exp, sigOcto | new UInt128(0x4000_0000_0000_0000, 0x0000_0000_0000_0000), 0));
		}
		public static explicit operator double(Octo value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator float(Octo value)
		{
			throw new NotImplementedException();
		}
		public static explicit operator Half(Octo value)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region To Octo
		// Unsigned
		public static implicit operator Octo(byte value)
		{
			return (Octo)(uint)value;
		}
		public static implicit operator Octo(ushort value)
		{
			return (Octo)(uint)value;
		}
		public static implicit operator Octo(uint value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 205;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (ushort)(0x401AD - shiftDist), sig);
		}
		public static implicit operator Octo(ulong value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 173;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (ushort)(0x4016C - shiftDist), sig);
		}
		public static implicit operator Octo(UInt128 value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = ((int)UInt128.LeadingZeroCount(value)) + 109;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (ushort)(0x400EB - shiftDist), sig);
		}
		// Signed
		public static implicit operator Octo(sbyte value)
		{
			if (sbyte.IsNegative(value))
			{
				value = (sbyte)-value;
				return -(Octo)(byte)value;
			}
			return (Octo)(byte)value;
		}
		public static implicit operator Octo(short value)
		{
			if (short.IsNegative(value))
			{
				value = (short)-value;
				return -(Octo)(ushort)value;
			}
			return (Octo)(ushort)value;
		}
		public static implicit operator Octo(int value)
		{
			if (int.IsNegative(value))
			{
				value = -value;
				return -(Octo)(uint)value;
			}
			return (Octo)(uint)value;
		}
		public static implicit operator Octo(long value)
		{
			if (long.IsNegative(value))
			{
				value = -value;
				return -(Octo)(ulong)value;
			}
			return (Octo)(ulong)value;
		}
		public static implicit operator Octo(Int128 value)
		{
			if (Int128.IsNegative(value))
			{
				value = -value;
				return -(Octo)(UInt128)value;
			}
			return (Octo)(UInt128)value;
		}
		// Floating
		public static implicit operator Octo(decimal value)
		{
			return (Octo)(double)value;
		}
		public static implicit operator Octo(double value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator Octo(float value)
		{
			throw new NotImplementedException();
		}
		public static implicit operator Octo(Half value)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
