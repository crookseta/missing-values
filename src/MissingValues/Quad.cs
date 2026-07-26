using MissingValues.Info;
using MissingValues.Internals;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MissingValues.Primitives;

namespace MissingValues;

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
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(this);
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
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(this);
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
		_lower = value.Lower;
		_upper = value.Upper;
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
			return HashCode.Combine(BinaryOperations.QuadToUInt128Bits(this) & PositiveInfinityBits);
		}
		return HashCode.Combine(_lower, _upper);
	}

	/// <inheritdoc/>
	public override string ToString()
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
	public static Quad UInt128BitsToQuad(UInt128 bits) => BinaryOperations.UInt128BitsToQuad(bits);
	/// <summary>
	/// Reinterprets the specified 128-bit signed integer to a quadruple-precision floating point number.
	/// </summary>
	/// <param name="bits">The number to convert.</param>
	/// <returns>A quadruple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Quad Int128BitsToQuad(Int128 bits) => BinaryOperations.Int128BitsToQuad(bits);

	/// <summary>
	/// Converts the specified quadruple-precision floating point number to a 128-bit unsigned integer.
	/// </summary>
	/// <param name="value">The number to convert.</param>
	/// <returns>A 128-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static UInt128 QuadToUInt128Bits(Quad value) => BinaryOperations.QuadToUInt128Bits(value);
	/// <summary>
	/// Converts the specified quadruple-precision floating point number to a 128-bit signed integer.
	/// </summary>
	/// <param name="value">The number to convert.</param>
	/// <returns>A 128-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Int128 QuadToInt128Bits(Quad value) => BinaryOperations.QuadToInt128Bits(value);


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
		return ((BinaryOperations.QuadToUInt128Bits(x) | BinaryOperations.QuadToUInt128Bits(y)) & ~SignMask) == UInt128.Zero;
	}

	internal static bool IsNaNOrZero(Quad value)
	{
		return ((BinaryOperations.QuadToUInt128Bits(value) - 1) & ~SignMask) >= PositiveInfinityBits;
	}

	internal static UInt128 StripSign(Quad value)
	{
		return BinaryOperations.QuadToUInt128Bits(value) & ~SignMask;
	}

	internal static Quad CreateQuadNaN(bool sign, UInt128 significand)
	{
		return BinaryOperations.UInt128BitsToQuad(CreateQuadNaNBits(sign, significand));
	}
	internal static UInt128 CreateQuadNaNBits(bool sign, UInt128 significand)
	{
		UInt128 signInt = (sign ? UInt128.One : UInt128.Zero) << 127;
		UInt128 sigInt = significand >> 16;

		return signInt | (BiasedExponentMask | new UInt128(0x0000_8000_0000_0000, 0x0)) | sigInt;
	}
}