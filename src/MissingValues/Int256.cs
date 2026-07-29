using MissingValues.Info;
using MissingValues.Internals;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.Json.Serialization;

namespace MissingValues;

/// <summary>
/// Represents a 256-bit signed integer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(NumberConverter.Int256Converter))]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(IntDebugView<Int256>))]
public readonly partial struct Int256
{
	internal const int Size = 32;
		
	private static UInt128 _upperMin => new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000);
	private static UInt128 _lowerMin => new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

	private static UInt128 _upperMax => new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	private static UInt128 _lowerMax => new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

	/// <summary>
	/// Represents the value <c>1</c> of the type.
	/// </summary>
	public static readonly Int256 One = new Int256(0, 0, 0, 1);
	/// <summary>
	/// Represents the largest possible value of the type.
	/// </summary>
	public static readonly Int256 MaxValue = new Int256(_upperMax, _lowerMax);
	/// <summary>
	/// Represents the smallest possible value of the type.
	/// </summary>
	public static readonly Int256 MinValue = new Int256(_upperMin, _lowerMin);
	/// <summary>
	/// Represents the value <c>-1</c> of the type.
	/// </summary>
	public static readonly Int256 NegativeOne = new Int256(_lowerMax, _lowerMax);
	/// <summary>
	/// Represents the value <c>0</c> of the type.
	/// </summary>
	public static readonly Int256 Zero = default;

#if BIGENDIAN
	private readonly ulong _p3;
	private readonly ulong _p2;
	private readonly ulong _p1;
	private readonly ulong _p0;
#else
	private readonly ulong _p0;
	private readonly ulong _p1;
	private readonly ulong _p2;
	private readonly ulong _p3;
#endif
	internal UInt128 Lower => new UInt128(_p1, _p0);
	internal UInt128 Upper => new UInt128(_p3, _p2);

	internal ulong Part0 => _p0;
	internal ulong Part1 => _p1;
	internal ulong Part2 => _p2;
	internal ulong Part3 => _p3;

	/// <summary>
	/// Initializes a new instance of the <see cref="Int256" /> struct.
	/// </summary>
	/// <param name="u1">The first 64-bits of the 256-bit value.</param>
	/// <param name="u2">The second 64-bits of the 256-bit value.</param>
	/// <param name="l1">The third 64-bits of the 256-bit value.</param>
	/// <param name="l2">The fourth 64-bits of the 256-bit value.</param>
	public Int256(ulong u1, ulong u2, ulong l1, ulong l2)
	{
		_p0 = l2;
		_p1 = l1;
		_p2 = u2;
		_p3 = u1;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int256" /> struct.
	/// </summary>
	/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
	public Int256(UInt128 lower)
	{
		_p0 = lower.Lower;
		_p1 = lower.Upper;
		_p2 = 0;
		_p3 = 0;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int256" /> struct.
	/// </summary>
	/// <param name="upper">The upper 128-bits of the 256-bit value.</param>
	/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
	public Int256(UInt128 upper, UInt128 lower)
	{
		_p0 = lower.Lower;
		_p1 = lower.Upper;
		_p2 = upper.Lower;
		_p3 = upper.Upper;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int256" /> struct.
	/// </summary>
	/// <param name="parts">Span holding the 64-bit parts of the 256-bit value</param>
	/// <exception cref="ArgumentOutOfRangeException">Span is too small for the value</exception>
	public Int256(ReadOnlySpan<ulong> parts)
	{
		if (Vector256.IsHardwareAccelerated && BitConverter.IsLittleEndian)
		{
			Unsafe.SkipInit(out this);
			Unsafe.As<ulong, Vector256<ulong>>(ref _p0) = Vector256.Create(parts);
		}
		else
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(parts.Length, Size / 8);
			_p0 = parts[0];
			_p1 = parts[1];
			_p2 = parts[2];
			_p3 = parts[3];
		}
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return (obj is Int256 other) && Equals(other);
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return HashCode.Combine(_p3, _p2, _p1, _p0);
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return ToString("D", CultureInfo.CurrentCulture);
	}

	/// <summary>
	/// Produces the full product of two signed 256-bit numbers.
	/// </summary>
	/// <param name="left">First number to multiply.</param>
	/// <param name="right">Second number to multiply.</param>
	/// <param name="low">The low 256-bit of the product of the specified numbers.</param>
	/// <returns>The high 256-bit of the product of the specified numbers.</returns>
	public static Int256 BigMul(Int256 left, Int256 right, out Int256 low)
	{
		// This follows the same logic as is used in `long Math.BigMul(long, long, out long)`

		UInt256 upper = UInt256.BigMul((UInt256)left, (UInt256)right, out UInt256 ulower);
		low = (Int256)ulower;
		return (Int256)(upper) - ((left >> 255) & right) - ((right >> 255) & left);
	}
		
	/// <summary>
	/// Computes the base-10 logarithm of a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value whose base-10 logarithm is to be computed.</param>
	/// <returns>The base-10 logarithm of <paramref name="value"/></returns>
	public static Int256 Log10(Int256 value)
	{
		return BitHelper.Log10(in value);
	}

	/// <summary>
	/// Raises a <see cref="Int256"/> value to the power of a specified value.
	/// </summary>
	/// <param name="value">The number to raise to the <paramref name="exponent"/> power.</param>
	/// <param name="exponent">The exponent to raise <paramref name="value"/> by.</param>
	/// <returns>The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="exponent"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
	/// </exception>
	public static Int256 Pow(Int256 value, int exponent)
	{
		const int UIntCount = Size / sizeof(ulong);

		ArgumentOutOfRangeException.ThrowIfNegative(exponent);

		if (exponent == 0)
		{
			return One;
		}
		if (exponent == 1)
		{
			return value;
		}

		uint power = checked((uint)exponent);
		int size;
		ulong[]? bitsArray = null;
		scoped Span<ulong> bits;

		if (value <= long.MaxValue && value >= long.MinValue)
		{
			int sign = (int)value;
			if (sign == 1)
				return value;
			if (sign == -1)
				return (exponent & 1) != 0 ? value : One;
			if (sign == 0)
				return value;

			if (power >= ((Size * 8) - 1))
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Exponentiation);
			}

			size = Calculator.PowBound(power, 1);

			bits = (size <= Calculator.StackAllocThreshold
				? stackalloc ulong[Calculator.StackAllocThreshold]
				: bitsArray = ArrayPool<ulong>.Shared.Rent(size));
			bits.Clear();

			Calculator.Pow(unchecked((ulong)sign), power, bits[..size]);
		}
		else
		{
			if (power >= ((Size * 8) - 1))
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Exponentiation);
			}

			int valueLength = BitHelper.GetTrimLength(in value);
			size = Calculator.PowBound(power, valueLength);

			Span<ulong> valueSpan = stackalloc ulong[UIntCount];
			valueSpan.Clear();
			BitHelper.Write(valueSpan, in value);

			bits = (size <= Calculator.StackAllocThreshold
				? stackalloc ulong[Calculator.StackAllocThreshold]
				: bitsArray = ArrayPool<ulong>.Shared.Rent(size));
			bits.Clear();

			Calculator.Pow(valueSpan[..valueLength], power, bits[..size]);
		}

		if (size > UIntCount)
		{
			Span<ulong> overflow = bits[UIntCount..];

			for (int i = 0; i < overflow.Length; i++)
			{
				if (overflow[i] != 0)
				{
					Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Exponentiation);
				}
			}
		}

		Int256 result = new Int256(bits);

		if (bitsArray is not null)
		{
			ArrayPool<ulong>.Shared.Return(bitsArray);
		}

		return result;
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

			Int256 result = new Int256((bits << 12) >> 1 | 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
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