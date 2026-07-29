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
/// Represents a 512-bit signed integer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(NumberConverter.Int512Converter))]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(IntDebugView<Int512>))]
public readonly partial struct Int512
{
	internal const int Size = 64;

	/// <summary>
	/// Represents the value <c>1</c> of the type.
	/// </summary>
	public static readonly Int512 One = new Int512(0, 0, 0, 0, 0, 0, 0, 1);
	/// <summary>
	/// Represents the largest possible value of the type.
	/// </summary>
	public static readonly Int512 MaxValue = new Int512(_upperMax, _lowerMax);
	/// <summary>
	/// Represents the smallest possible value of the type.
	/// </summary>
	public static readonly Int512 MinValue = new Int512(_upperMin, _lowerMin);
	/// <summary>
	/// Represents the value <c>-1</c> of the type.
	/// </summary>
	public static readonly Int512 NegativeOne = new Int512(_lowerMax, _lowerMax);
	/// <summary>
	/// Represents the value <c>0</c> of the type.
	/// </summary>
	public static readonly Int512 Zero = default;

#if BIGENDIAN
	private readonly ulong _p7;
	private readonly ulong _p6;
	private readonly ulong _p5;
	private readonly ulong _p4;
	private readonly ulong _p3;
	private readonly ulong _p2;
	private readonly ulong _p1;
	private readonly ulong _p0;
#else
	private readonly ulong _p0;
	private readonly ulong _p1;
	private readonly ulong _p2;
	private readonly ulong _p3;
	private readonly ulong _p4;
	private readonly ulong _p5;
	private readonly ulong _p6;
	private readonly ulong _p7;
#endif

	internal UInt256 Lower => new UInt256(_p3, _p2, _p1, _p0);
	internal UInt256 Upper => new UInt256(_p7, _p6, _p5, _p4);
	internal ulong Part0 => _p0;
	internal ulong Part1 => _p1;
	internal ulong Part2 => _p2;
	internal ulong Part3 => _p3;
	internal ulong Part4 => _p4;
	internal ulong Part5 => _p5;
	internal ulong Part6 => _p6;
	internal ulong Part7 => _p7;

	internal Int512(ulong lower)
	{
		_p0 = lower;
		_p1 = 0;
		_p2 = 0;
		_p3 = 0;
		_p4 = 0;
		_p5 = 0;
		_p6 = 0;
		_p7 = 0;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int512"/> struct.
	/// </summary>
	/// <param name="lower">The lower 256-bits of the 512-bit value.</param>
	public Int512(UInt256 lower) : this(UInt256.Zero, lower)
	{
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int512"/> struct.
	/// </summary>
	/// <param name="upper">The upper 256-bits of the 512-bit value.</param>
	/// <param name="lower">The lower 256-bits of the 512-bit value.</param>
	public Int512(UInt256 upper, UInt256 lower)
	{
		_p0 = lower.Part0;
		_p1 = lower.Part1;
		_p2 = lower.Part2;
		_p3 = lower.Part3;
		_p4 = upper.Part0;
		_p5 = upper.Part1;
		_p6 = upper.Part2;
		_p7 = upper.Part3;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int512"/> struct.
	/// </summary>
	/// <param name="uu">The first 128-bits of the 512-bit value.</param>
	/// <param name="ul">The second 128-bits of the 512-bit value.</param>
	/// <param name="lu">The third 128-bits of the 512-bit value.</param>
	/// <param name="ll">The fourth 128-bits of the 512-bit value.</param>
	public Int512(UInt128 uu, UInt128 ul, UInt128 lu, UInt128 ll)
	{
		_p0 = ll.Lower;
		_p1 = ll.Upper;
		_p2 = lu.Lower;
		_p3 = lu.Upper;
		_p4 = ul.Lower;
		_p5 = ul.Upper;
		_p6 = uu.Lower;
		_p7 = uu.Upper;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int512"/> struct.
	/// </summary>
	/// <param name="uuu">The first 64-bits of the 512-bit value.</param>
	/// <param name="uul">The second 64-bits of the 512-bit value.</param>
	/// <param name="ulu">The third 64-bits of the 512-bit value.</param>
	/// <param name="ull">The fourth 64-bits of the 512-bit value.</param>
	/// <param name="luu">The fifth 64-bits of the 512-bit value.</param>
	/// <param name="lul">The sixth 64-bits of the 512-bit value.</param>
	/// <param name="llu">The seventh 64-bits of the 512-bit value.</param>
	/// <param name="lll">The eighth 64-bits of the 512-bit value.</param>
	public Int512(ulong uuu, ulong uul, ulong ulu, ulong ull, ulong luu, ulong lul, ulong llu, ulong lll)
	{
		_p0 = lll;
		_p1 = llu;
		_p2 = lul;
		_p3 = luu;
		_p4 = ull;
		_p5 = ulu;
		_p6 = uul;
		_p7 = uuu;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Int512" /> struct.
	/// </summary>
	/// <param name="parts">Span holding the 64-bit parts of the 512-bit value</param>
	/// <exception cref="ArgumentOutOfRangeException">Span is too small for the value</exception>
	public Int512(ReadOnlySpan<ulong> parts)
	{
		if (Vector512.IsHardwareAccelerated && BitConverter.IsLittleEndian)
		{
			Unsafe.SkipInit(out this);
			Unsafe.As<ulong, Vector512<ulong>>(ref _p0) = Vector512.Create(parts);
		}
		if (Vector256.IsHardwareAccelerated && BitConverter.IsLittleEndian)
		{
			Unsafe.SkipInit(out this);
			Unsafe.As<ulong, Vector256<ulong>>(ref _p0) = Vector256.Create(parts);
			Unsafe.As<ulong, Vector256<ulong>>(ref _p4) = Vector256.Create(parts[4..]);
		}
		else
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(parts.Length, Size / 8);
			_p0 = parts[0];
			_p1 = parts[1];
			_p2 = parts[2];
			_p3 = parts[3];
			_p4 = parts[4];
			_p5 = parts[5];
			_p6 = parts[6];
			_p7 = parts[7];
		}
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is Int512 @int && Equals(@int);
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return HashCode.Combine(Upper, Lower);
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return ToString("D", CultureInfo.CurrentCulture);
	}

	/// <summary>
	/// Produces the full product of two signed 512-bit numbers.
	/// </summary>
	/// <param name="left">First number to multiply.</param>
	/// <param name="right">Second number to multiply.</param>
	/// <param name="low">The low 512-bit of the product of the specified numbers.</param>
	/// <returns>The high 512-bit of the product of the specified numbers.</returns>
	public static Int512 BigMul(Int512 left, Int512 right, out Int512 low)
	{
		// This follows the same logic as is used in `long Math.BigMul(long, long, out long)`

		UInt512 upper = UInt512.BigMul((UInt512)left, (UInt512)right, out UInt512 ulower);
		low = (Int512)ulower;
		return (Int512)(upper) - ((left >> 511) & right) - ((right >> 511) & left);
	}

	/// <summary>
	/// Computes the base-10 logarithm of a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value whose base-10 logarithm is to be computed.</param>
	/// <returns>The base-10 logarithm of <paramref name="value"/></returns>
	public static Int512 Log10(Int512 value)
	{
		return BitHelper.Log10(in value);
	}

	/// <summary>
	/// Raises a <see cref="Int512"/> value to the power of a specified value.
	/// </summary>
	/// <param name="value">The number to raise to the <paramref name="exponent"/> power.</param>
	/// <param name="exponent">The exponent to raise <paramref name="value"/> by.</param>
	/// <returns>The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="exponent"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
	/// </exception>
	public static Int512 Pow(Int512 value, int exponent)
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
			long sign = (long)value;
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

		Int512 result = new Int512(bits);

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
	/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="Int512"/>.</exception>
	public static Int512 Parse(ReadOnlySpan<char> s)
	{
		return Parse(s, CultureInfo.CurrentCulture);
	}
	/// <summary>Tries to parse a span of characters into a value.</summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
	/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, out Int512 result)
	{
		return TryParse(s, CultureInfo.CurrentCulture, out result);
	}

	private static Int512 ToInt512(double value)
	{
		const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

		Debug.Assert(value >= -TwoPow511);
		Debug.Assert(double.IsFinite(value));
		Debug.Assert(TwoPow511 > value);

		bool isNegative = double.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}

		if (value >= 1.0)
		{
			// In order to convert from double to int512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by right shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			ulong bits = BitConverter.DoubleToUInt64Bits(value);

			Int512 result = new Int512((bits << 12) >> 1 | 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
				0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			result >>>= (1023 + 512 - 1 - (int)(bits >> 52));

			if (isNegative)
			{
				return -result;
			}
			return result;
		}
		else
		{
			return Int512.Zero;
		}
	}
}