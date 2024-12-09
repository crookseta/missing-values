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
using System.Runtime.CompilerServices;
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
	[DebuggerTypeProxy(typeof(FloatDebugView<Quad>))]
	public readonly partial struct Quad
	{
		internal static UInt128 SignMask => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt128 InvertedSignMask => new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal const int SignShift = 127;
		internal const int MantissaDigits = 113;
		internal const int ExponentBias = 16383;
		internal const int BiasedExponentLength = 15;
		internal const int BiasedExponentShift = 112;
		internal const ulong ShiftedBiasedExponentMask = 32767;

		internal const int MinBiasedExponent = 0x0000;
		internal const int MaxBiasedExponent = 0x7FFF;
		internal const int MinExponent = -16382;
		internal const int MaxExponent = 16383;

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

		/// <summary>
		/// Represents the natural logarithmic base, specified by the constant, <c>e</c>.
		/// </summary>
		public static readonly Quad E = new Quad(0x4000_5BF0_A8B1_4576, 0x9535_5FB8_AC40_4E7A);
		/// <summary>
		/// Represents the smallest positive <see cref="Quad"/> value that is greater than zero.
		/// </summary>
		public static readonly Quad Epsilon = new Quad(0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		/// <summary>
		/// Represents the largest possible value of a <see cref="Quad"/>.
		/// </summary>
		public static readonly Quad MaxValue = new Quad(0x7FFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		/// <summary>
		/// Represents the smallest possible value of a <see cref="Quad"/>.
		/// </summary>
		public static readonly Quad MinValue = new Quad(0xFFFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		/// <summary>
		/// Represents a value that is not a number (<c>NaN</c>).
		/// </summary>
		public static readonly Quad NaN = new Quad(0xFFFF_8000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>-1</c> of the type.
		/// </summary>
		public static readonly Quad NegativeOne = new Quad(0xBFFF_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents negative infinity.
		/// </summary>
		public static readonly Quad NegativeInfinity = new Quad(0xFFFF_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>-0</c> of the type.
		/// </summary>
		public static readonly Quad NegativeZero = new Quad(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>1</c> of the type.
		/// </summary>
		public static readonly Quad One = new Quad(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, <c>pi</c>.
		/// </summary>
		public static readonly Quad Pi = new Quad(0x4000_921F_B544_42D1, 0x8469_898C_C517_01B8);
		/// <summary>
		/// Represents positive infinity.
		/// </summary>
		public static readonly Quad PositiveInfinity = new Quad(0x7FFF_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the number of radians in one turn, specified by the constant, <c>tau</c>.
		/// </summary>
		public static readonly Quad Tau = new Quad(0x4001_921F_B544_42D1, 0x8469_898C_C517_01B8);
		/// <summary>
		/// Represents the value <c>0</c> of the type.
		/// </summary>
		public static readonly Quad Zero = new Quad(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

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
		/// <summary>
		/// Initializes a new instance of the <see cref="Quad" /> struct.
		/// </summary>
		/// <param name="sign">A <see cref="bool"/> indicating the sign of the number. <see langword="true"/> represents a negative number, and <see langword="false"/> represents a positive number.</param>
		/// <param name="exp">An <see cref="ushort"/> representing the exponent part of the floating-point number.</param>
		/// <param name="sig">An <see cref="UInt128"/> representing the significand part of the floating-point number.</param>
		public Quad(bool sign, ushort exp, UInt128 sig)
		{
			UInt128 value = (((sign ? UInt128.One : UInt128.Zero) << SignShift) + ((((UInt128)exp) << BiasedExponentShift) & BiasedExponentMask) + (sig & TrailingSignificandMask));
			_lower = unchecked((ulong)value);
			_upper = unchecked((ulong)(value >> 64));
		}

		/// <inheritdoc/>
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is Quad other) && Equals(other);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			if (IsNaNOrZero(this))
			{
				// All NaNs should have the same hash code, as should both Zeros.
				return HashCode.Combine(QuadToUInt128Bits(this) & PositiveInfinityBits);
			}
			return HashCode.Combine(_lower, _upper);
		}

		/// <inheritdoc/>
		public override string? ToString()
		{
			return ToString("G33", NumberFormatInfo.CurrentInfo);
		}

		/// <summary>
		/// Parses a span of characters into a value.
		/// </summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s"/>.</returns>
		public static Quad Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}
		/// <summary>
		/// tries to parse a span of characters into a value.
		/// </summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
		/// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out Quad result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>
		/// Reinterprets the specified 128-bit unsigned integer to a quadruple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A quadruple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Quad UInt128BitsToQuad(UInt128 bits) => System.Runtime.CompilerServices.Unsafe.BitCast<UInt128, Quad>(bits);
		/// <summary>
		/// Reinterprets the specified 128-bit signed integer to a quadruple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A quadruple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Quad Int128BitsToQuad(Int128 bits) => System.Runtime.CompilerServices.Unsafe.BitCast<Int128, Quad>(bits);

		/// <summary>
		/// Converts the specified quadruple-precision floating point number to a 128-bit unsigned integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 128-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe UInt128 QuadToUInt128Bits(Quad value) => new UInt128(value._upper, value._lower);
		/// <summary>
		/// Converts the specified quadruple-precision floating point number to a 128-bit signed integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 128-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Int128 QuadToInt128Bits(Quad value) => new Int128(value._upper, value._lower);

		
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
			UInt128 signInt = (sign ? UInt128.One : UInt128.Zero) << 127;
			UInt128 sigInt = significand >> 16;

			return signInt | (BiasedExponentMask | new UInt128(0x0000_8000_0000_0000, 0x0)) | sigInt;
		}

		#region From Quad
		// Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator sbyte(Quad value)
		{
			Quad twoPow7 = new Quad(0x4006_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7)
			{
				return sbyte.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow7)
			{
				return sbyte.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				// For some reason, sbyte and short dont perform logical shifts correctly, so we have to perform the shifting with byte and ushort.
				sbyte result = (sbyte)(((byte)(bits >> 105) | 0x80) >>> (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112)));

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
		public static explicit operator checked sbyte(Quad value)
		{
			Quad twoPow7 = new Quad(0x4006_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7 || Quad.IsNaN(value) || value >= +twoPow7)
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
				sbyte result = (sbyte)(((byte)(bits >> 105) | 0x80) >>> (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112)));

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator short(Quad value)
		{
			Quad twoPow15 = new Quad(0x400E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15)
			{
				return short.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow15)
			{
				return short.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				// For some reason, sbyte and short dont perform logical shifts correctly, so we have to perform the shifting with byte and ushort.
				short result = (short)(((ushort)(bits >> 97) | 0x8000) >>> (Quad.ExponentBias + 16 - 1 - (int)(bits >> 112)));

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
		public static explicit operator checked short(Quad value)
		{
			Quad twoPow15 = new Quad(0x400E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15 || Quad.IsNaN(value) || value >= +twoPow15)
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
				short result = (short)(((ushort)(bits >> 97) | 0x8000) >>> (Quad.ExponentBias + 16 - 1 - (int)(bits >> 112)));

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator int(Quad value)
		{
			Quad twoPow31 = new Quad(0x401E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31)
			{
				return int.MinValue;
			}
			else if (Quad.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow31)
			{
				return int.MaxValue;
			}

			bool isNegative = Quad.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}
			if (value >= Quad.One)
			{
				UInt128 bits = Quad.QuadToUInt128Bits(value);
				int result = (int)((uint)(bits >> 81) | 0x8000_0000);

				result >>>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
		public static explicit operator checked int(Quad value)
		{
			Quad twoPow31 = new Quad(0x401E_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31 || Quad.IsNaN(value) || value >= +twoPow31)
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
				int result = (int)((uint)(bits >> 81) | 0x8000_0000);

				result >>>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);

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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
				// In order to convert from Quad to Int512 we first need to extract the signficand,
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
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
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

		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="BigInteger"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is not finite.</exception>
		public static explicit operator BigInteger(Quad value)
		{
			const int kcbitUlong = 64;
			const int kcbitUInt128 = 128;

			if (!IsFinite(value))
			{
				Thrower.IntegerOverflow();
			}

			BitHelper.GetQuadParts(value, out int sign, out int exp, out var man, out _);

            if (man == UInt128.Zero)
            {
				return BigInteger.Zero;
            }
			
			if (exp <= 0)
			{
				if (exp <= -kcbitUInt128)
				{
					return BigInteger.Zero;
				}
				return (BigInteger)(sign < 0 ? -(man >> -exp) : (man >> -exp));
			}
			else if (exp <= BiasedExponentLength)
			{
				return (BigInteger)(sign < 0 ? -(man << exp) : (man << exp));
			}
			else
			{
				// Overflow into at least 3 ulongs.
				// Move the leading 1 to the high bit.
				man <<= BiasedExponentLength;
				exp -= BiasedExponentLength;

				// Compute cu and cbit so that exp == 64 * cu - cbit and 0 <= cbit < 64.
				int cu = (exp - 1) / kcbitUlong + 1;
				int cbit = cu * kcbitUlong - exp;
				Debug.Assert(0 <= cbit && cbit < kcbitUlong);
				Debug.Assert(cu >= 1);

				// Populate the uints.
				Span<ulong> bits = stackalloc ulong[cu + 2];
				bits[cu + 1] = (ulong)(man >> (cbit + kcbitUlong));
				bits[cu] = unchecked((ulong)(man >> cbit));
				if (cbit > 0)
					bits[cu - 1] = unchecked((ulong)man) << (kcbitUlong - cbit);

				return sign > 0 ? new BigInteger(MemoryMarshal.Cast<ulong, byte>(bits), true) : -(new BigInteger(MemoryMarshal.Cast<ulong, byte>(bits)));
			}
        }
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="decimal"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator decimal(Quad value)
		{
			return (decimal)(double)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="Quad" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(Quad value)
		{
			bool sign = IsNegative(value);
			int exp = value.BiasedExponent;
			UInt128 sig = value.TrailingSignificand;

            if (exp == MaxBiasedExponent)
            {
				if (sig != UInt128.Zero)
				{
					return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 124);
				}
				return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
            }

			if (exp == 0)
			{
				if (sig == UInt128.Zero)
				{
					return Octo.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF128Sig(sig);
				exp -= 1;
			}

			return new Octo(sign, (uint)(exp + 0x3_C000), (UInt256)sig << 124);
        }
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="double"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
					return BitHelper.CreateDoubleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? double.NegativeInfinity : double.PositiveInfinity;
			}

			sig <<= 14;
			ulong sigQuad = (ulong)(sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL);

			if (((uint)exp | sigQuad) == 0)
			{
				return BitHelper.CreateDouble(sign, 0, 0);
			}

			exp -= 0x3C01;

			exp = exp < -0x1000 ? -0x1000 : exp;

			return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, (short)(exp), (sigQuad | 0x4000_0000_0000_0000)));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="float"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
					return BitHelper.CreateSingleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? float.NegativeInfinity : float.PositiveInfinity;
			}

			uint sigQuad = (uint)BitHelper.ShiftRightJam((ulong)((sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL)), 18);

			if (((uint)exp | sigQuad) == 0)
			{
				return BitHelper.CreateSingle(sign, 0, 0);
			}

			exp -= 0x3F81;

			exp = exp < -0x1000 ? -0x1000 : exp;

			return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, (short)(exp), (sigQuad | 0x4000_0000)));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
					return BitHelper.CreateHalfNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
				}
				return sign ? Half.NegativeInfinity : Half.PositiveInfinity;
			}

			ushort sigHalf = (ushort)BitHelper.ShiftRightJam((ulong)((sig >> 64) | ((ulong)sig != 0 ? 1UL : 0UL)), 34);

			if (((uint)exp | sigHalf) == 0)
			{
				return BitHelper.CreateHalf(sign, 0, 0);
			}

			exp -= 0x3FF1;

			exp = exp < -0x40 ? -0x40 : exp;

			return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, (short)(exp), (ushort)(sigHalf | 0x4000)));
		}
		#endregion
		#region To Quad
		// Unsigned
		/// <summary>
		/// Implicitly converts a <see cref="byte" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(byte value)
		{
			return (Quad)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="ushort" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(ushort value)
		{
			return (Quad)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="uint" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Implicitly converts a <see cref="ulong" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
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
		/// <summary>
		/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(sbyte value)
		{
			if (sbyte.IsNegative(value))
			{
				value = (sbyte)-value;
				return -(Quad)(byte)value;
			}
			return (Quad)(byte)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="short" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(short value)
		{
			if (short.IsNegative(value))
			{
				value = (short)-value;
				return -(Quad)(ushort)value;
			}
			return (Quad)(ushort)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="int" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(int value)
		{
			if (int.IsNegative(value))
			{
				value = -value;
				return -(Quad)(uint)value;
			}
			return (Quad)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="long" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(long value)
		{
			if (long.IsNegative(value))
			{
				value = -value;
				return -(Quad)(ulong)value;
			}
			return (Quad)(ulong)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(Int128 value)
		{
			if (Int128.IsNegative(value))
			{
				value = -value;
				return -(Quad)(UInt128)value;
			}
			return (Quad)(UInt128)value;
		}

		/// <summary>
		/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(BigInteger value)
		{
			Span<byte> bits = stackalloc byte[value.GetByteCount()];

			int sign = value.Sign;
			value.TryWriteBytes(bits, out int length);
			scoped Span<ulong> bits64;
			length /= sizeof(ulong);
			if (!BitOperations.IsPow2(bits.Length) && bits.Length >= 4)
			{
				int pow2Length = length * sizeof(ulong);
				var remainder = bits[pow2Length..];
				bits64 = stackalloc ulong[++length];
				bits[..pow2Length].CopyTo(MemoryMarshal.AsBytes(bits64));
                for (int i = remainder.Length - 1, shift = 64 - 8; i >= 0; i--, shift -= 8)
                {
					bits64[^1] = (ulong)remainder[i] << shift;
				}
			}
			else if (bits.Length < 4)
			{
				bits64 = stackalloc ulong[length = 1];
				bits.CopyTo(MemoryMarshal.AsBytes(bits64));
			}
			else
			{
				bits64 = MemoryMarshal.Cast<byte, ulong>(bits);
			}

			if (length == 1)
			{
				return bits64[0];
			}

			// The maximum exponent for quads is 16383, which corresponds to a uint bit length of 64.
			// All BigIntegers with bits[] longer than 64 evaluate to Quad.Infinity (or NegativeInfinity).
			// Cases where the exponent is between 16384 and 16398 are handled in BitHelper.GetQuadFromParts.
			const int InfinityLength = MaxExponent / 64;

			if (length > InfinityLength)
			{
				if (sign == 1)
					return Quad.PositiveInfinity;
				else
					return Quad.NegativeInfinity;
			}

			UInt128 h = bits64[^1];
			UInt128 m = length > 1 ? bits64[^2] : 0;
			UInt128 l = length > 2 ? bits64[^3] : 0;

			int z = BitOperations.LeadingZeroCount((ulong)h);

			int exp = (length - 2) * 64 - z;
			UInt128 man = (h << 64 + z) | (m << z) | (l >> 64 - z);

			return BitHelper.GetQuadFromParts(sign, exp, man);
		}
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(decimal value)
		{
			return (Quad)(double)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="double" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(double value)
		{
			const int MaxBiasedExponentDouble = 0x07FF;
			const int DoubleExponentBias = 1023;

			ulong bits = BitConverter.DoubleToUInt64Bits(value);
			bool sign = double.IsNegative(value);
			int exp = (ushort)((bits >> 52) & MaxBiasedExponentDouble);
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
		/// <summary>
		/// Implicitly converts a <see cref="float" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(float value)
		{
			const int MaxBiasedExponentSingle = 0xFF;
			const int SingleExponentBias = 127;

			uint bits = BitConverter.SingleToUInt32Bits(value);
			bool sign = float.IsNegative(value);
			int exp = (ushort)((bits >> 23) & MaxBiasedExponentSingle);
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
		/// <summary>
		/// Implicitly converts a <see cref="Half" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Quad(Half value)
		{
			const int MaxBiasedExponentHalf = 0x1F;
			const int HalfExponentBias = 15;

			ushort bits = BitConverter.HalfToUInt16Bits(value);
			bool sign = Half.IsNegative(value);
			int exp = (ushort)((bits >> 10) & MaxBiasedExponentHalf);
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
	}
}
