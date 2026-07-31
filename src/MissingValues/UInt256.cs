using MissingValues.Info;
using MissingValues.Internals;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.Json.Serialization;

namespace MissingValues;

/// <summary>
/// Represents a 256-bit unsigned integer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(NumberConverter.UInt256Converter))]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(IntDebugView<UInt256>))]
public readonly partial struct UInt256
{
	internal const int Size = 32;

	/// <summary>
	/// Represents the value <c>1</c> of the type.
	/// </summary>
	public static readonly UInt256 One = new UInt256(0, 0, 0, 1);
	/// <summary>
	/// Represents the largest possible value of the type.
	/// </summary>
	public static readonly UInt256 MaxValue = new UInt256(
		0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	/// <summary>
	/// Represents the smallest possible value of the type.
	/// </summary>
	public static readonly UInt256 MinValue = default;
	/// <summary>
	/// Represents the value <c>0</c> of the type.
	/// </summary>
	public static readonly UInt256 Zero = default;

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
	/// Initializes a new instance of the <see cref="UInt256" /> struct.
	/// </summary>
	/// <param name="u1">The first 64-bits of the 256-bit value.</param>
	/// <param name="u2">The second 64-bits of the 256-bit value.</param>
	/// <param name="l1">The third 64-bits of the 256-bit value.</param>
	/// <param name="l2">The fourth 64-bits of the 256-bit value.</param>
	public UInt256(ulong u1, ulong u2, ulong l1, ulong l2)
	{
		_p3 = u1;
		_p2 = u2;
		_p1 = l1;
		_p0 = l2;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="UInt256" /> struct.
	/// </summary>
	/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
	public UInt256(UInt128 lower)
	{
		_p0 = lower.Lower;
		_p1 = lower.Upper;
		_p2 = 0;
		_p3 = 0;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="UInt256" /> struct.
	/// </summary>
	/// <param name="upper">The upper 128-bits of the 256-bit value.</param>
	/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
	public UInt256(UInt128 upper, UInt128 lower)
	{
		_p0 = lower.Lower;
		_p1 = lower.Upper;
		_p2 = upper.Lower;
		_p3 = upper.Upper;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="UInt256" /> struct.
	/// </summary>
	/// <param name="parts">Span holding the 64-bit parts of the 256-bit value</param>
	/// <exception cref="ArgumentOutOfRangeException">Span is too small for the value</exception>
	internal UInt256(ReadOnlySpan<ulong> parts)
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
	public override string ToString()
	{
		return ToString("D", CultureInfo.CurrentCulture);
	}

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		return obj is UInt256 @int && Equals(@int);
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return HashCode.Combine(_p3, _p2, _p1, _p0);
	}

	/// <summary>
	/// Produces the full product of two unsigned 256-bit numbers.
	/// </summary>
	/// <param name="left">First number to multiply.</param>
	/// <param name="right">Second number to multiply.</param>
	/// <param name="lower">The low 256-bit of the product of the specified numbers.</param>
	/// <returns>The high 256-bit of the product of the specified numbers.</returns>
	public static UInt256 BigMul(UInt256 left, UInt256 right, out UInt256 lower)
	{
		if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
		{
			if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
			{
				ulong up = Calculator.BigMul(left._p0, right._p0, out ulong low);
				lower = new UInt256(0, 0, up, low);
				return Zero;
			}

			lower = Calculator.Multiply(in left, right._p0, out ulong carry);
			return carry;
		}
		else if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
		{
			lower = Calculator.Multiply(in right, left._p0, out ulong carry);
			return carry;
		}

		const int UIntCount = Size / sizeof(ulong);

		Span<ulong> rawBits = stackalloc ulong[UIntCount * 2];
		rawBits.Clear();

		Multiply(in left, right._p0, rawBits);
		Multiply(in left, right._p1, rawBits[1..]);
		Multiply(in left, right._p2, rawBits[2..]);
		Multiply(in left, right._p3, rawBits[3..]);

		lower = new UInt256(rawBits);

		return new UInt256(rawBits[4..]);

		static void Multiply(in UInt256 left, ulong right, Span<ulong> result)
		{
			Debug.Assert(result.Length >= 5);
				
			ulong up, low, carry;
			(up, low) = Calculator.BigMulAdd(left._p0, right, 0);
			result[0] = Calculator.AddWithCarry(result[0], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p1, right, up);
			result[1] = Calculator.AddWithCarry(result[1], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p2, right, up);
			result[2] = Calculator.AddWithCarry(result[2], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p3, right, up);
			result[3] = Calculator.AddWithCarry(result[3], low, out carry);

			result[4] = up;
		}
	}
		
	/// <summary>
	/// Computes the base-10 logarithm of a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value whose base-10 logarithm is to be computed.</param>
	/// <returns>The base-10 logarithm of <paramref name="value"/></returns>
	public static UInt256 Log10(UInt256 value)
	{
		return (UInt256)BitHelper.Log10(in value);
	}

	/// <summary>
	/// Raises a <see cref="UInt256"/> value to the power of a specified value.
	/// </summary>
	/// <param name="value">The number to raise to the <paramref name="exponent"/> power.</param>
	/// <param name="exponent">The exponent to raise <paramref name="value"/> by.</param>
	/// <returns>The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="exponent"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
	/// </exception>
	public static UInt256 Pow(UInt256 value, int exponent)
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

		if (value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
		{
			if (value._p0 == 1)
				return value;
			if (value._p0 == 0)
				return value;

			if (power >= (Size * 8))
			{
				Thrower.ArithmeticOverflow(Thrower.ArithmeticOperation.Exponentiation);
			}

			size = Calculator.PowBound(power, 1);

			bits = (size <= Calculator.StackAllocThreshold
				? stackalloc ulong[Calculator.StackAllocThreshold]
				: bitsArray = ArrayPool<ulong>.Shared.Rent(size));
			bits.Clear();

			Calculator.Pow(value._p0, power, bits[..size]);
		}
		else
		{
			if (power >= (Size * 8))
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

		UInt256 result = new UInt256(bits);

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

	private static UInt256 ToUInt256(double value)
	{
		const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

		Debug.Assert(value >= 0);
		Debug.Assert(double.IsFinite(value));
		Debug.Assert(value < TwoPow256);

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