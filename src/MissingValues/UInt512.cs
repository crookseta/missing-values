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
/// Represents a 512-bit unsigned integer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(NumberConverter.UInt512Converter))]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(IntDebugView<UInt512>))]
public readonly partial struct UInt512
{
	internal const int Size = 64;

	/// <summary>
	/// Represents the value <c>1</c> of the type.
	/// </summary>
	public static readonly UInt512 One = new UInt512(0, 0, 0, 0, 0, 0, 0, 1);
	/// <summary>
	/// Represents the largest possible value of the type.
	/// </summary>
	public static readonly UInt512 MaxValue = new UInt512(
		0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
		0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	/// <summary>
	/// Represents the smallest possible value of the type.
	/// </summary>
	public static readonly UInt512 MinValue = default;
	/// <summary>
	/// Represents the value <c>0</c> of the type.
	/// </summary>
	public static readonly UInt512 Zero = default;

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
	internal UInt256 Lower => new(_p3, _p2, _p1, _p0);
	internal UInt256 Upper => new(_p7, _p6, _p5, _p4);
	internal ulong Part0 => _p0;
	internal ulong Part1 => _p1;
	internal ulong Part2 => _p2;
	internal ulong Part3 => _p3;
	internal ulong Part4 => _p4;
	internal ulong Part5 => _p5;
	internal ulong Part6 => _p6;
	internal ulong Part7 => _p7;

	internal UInt512(ulong lower)
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
	/// Initializes a new instance of the <see cref="UInt512" /> struct.
	/// </summary>
	/// <param name="lower">The lower 256-bits of the 512-bit value.</param>
	public UInt512(UInt256 lower)
	{
		_p0 = lower.Part0;
		_p1 = lower.Part1;
		_p2 = lower.Part2;
		_p3 = lower.Part3;
		_p4 = 0;
		_p5 = 0;
		_p6 = 0;
		_p7 = 0;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="UInt512" /> struct.
	/// </summary>
	/// <param name="upper">The upper 256-bits of the 512-bit value.</param>
	/// <param name="lower">The lower 256-bits of the 512-bit value.</param>
	public UInt512(UInt256 upper, UInt256 lower)
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
	/// Initializes a new instance of the <see cref="UInt512" /> struct.
	/// </summary>
	/// <param name="uu">The first 128-bits of the 512-bit value.</param>
	/// <param name="ul">The second 128-bits of the 512-bit value.</param>
	/// <param name="lu">The third 128-bits of the 512-bit value.</param>
	/// <param name="ll">The fourth 128-bits of the 512-bit value.</param>
	public UInt512(UInt128 uu, UInt128 ul, UInt128 lu, UInt128 ll)
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
	/// Initializes a new instance of the <see cref="UInt512" /> struct.
	/// </summary>
	/// <param name="uuu">The first 64-bits of the 512-bit value.</param>
	/// <param name="uul">The second 64-bits of the 512-bit value.</param>
	/// <param name="ulu">The third 64-bits of the 512-bit value.</param>
	/// <param name="ull">The fourth 64-bits of the 512-bit value.</param>
	/// <param name="luu">The fifth 64-bits of the 512-bit value.</param>
	/// <param name="lul">The sixth 64-bits of the 512-bit value.</param>
	/// <param name="llu">The seventh 64-bits of the 512-bit value.</param>
	/// <param name="lll">The eighth 64-bits of the 512-bit value.</param>
	public UInt512(ulong uuu, ulong uul, ulong ulu, ulong ull, ulong luu, ulong lul, ulong llu, ulong lll)
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
	/// Initializes a new instance of the <see cref="UInt512" /> struct.
	/// </summary>
	/// <param name="parts">Span holding the 64-bit parts of the 512-bit value</param>
	/// <exception cref="ArgumentOutOfRangeException">Span is too small for the value</exception>
	internal UInt512(ReadOnlySpan<ulong> parts)
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
		return obj is UInt512 @int && Equals(@int);
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
	/// Produces the full product of two unsigned 512-bit numbers.
	/// </summary>
	/// <param name="left">First number to multiply.</param>
	/// <param name="right">Second number to multiply.</param>
	/// <param name="lower">The low 512-bit of the product of the specified numbers.</param>
	/// <returns>The high 512-bit of the product of the specified numbers.</returns>
	public static UInt512 BigMul(UInt512 left, UInt512 right, out UInt512 lower)
	{
		if (right._p7 == 0 && right._p6 == 0 && right._p5 == 0 && right._p4 == 0)
		{
			if (right._p3 == 0 && right._p2 == 0 && right._p1 == 0)
			{
				if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0 && left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					ulong up = Calculator.BigMul(left._p0, right._p0, out ulong low);
					lower = new UInt512(0, 0, 0, 0, 0, 0, up, low);
					return Zero;
				}

				lower = Calculator.Multiply(in left, right._p0, out ulong carry);
				return carry;
			}
			if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0)
			{
				if (left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
				{
					var temp = Calculator.Multiply(right.Lower, left._p0, out ulong carry);
					lower = new UInt512(0, 0, 0, carry, temp.Part3, temp.Part2, temp.Part1, temp.Part0);
					return Zero;
				}
					
				lower = MathQ.BigMul(left.Lower, right.Lower);
				return Zero;
			}
		}
		else if (left._p7 == 0 && left._p6 == 0 && left._p5 == 0 && left._p4 == 0 && left._p3 == 0 && left._p2 == 0 && left._p1 == 0)
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
		Multiply(in left, right._p4, rawBits[4..]);
		Multiply(in left, right._p5, rawBits[5..]);
		Multiply(in left, right._p6, rawBits[6..]);
		Multiply(in left, right._p7, rawBits[7..]);

		lower = new UInt512(rawBits);

		return new UInt512(rawBits[8..]);

		static void Multiply(in UInt512 left, ulong right, Span<ulong> result)
		{
			Debug.Assert(result.Length >= 9);
				
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

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p4, right, up);
			result[4] = Calculator.AddWithCarry(result[4], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p5, right, up);
			result[5] = Calculator.AddWithCarry(result[5], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p6, right, up);
			result[6] = Calculator.AddWithCarry(result[6], low, out carry);

			up += carry;
			(up, low) = Calculator.BigMulAdd(left._p7, right, up);
			result[7] = Calculator.AddWithCarry(result[7], low, out carry);

			result[8] = up;
		}
	}
		
	/// <summary>
	/// Computes the base-10 logarithm of a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value whose base-10 logarithm is to be computed.</param>
	/// <returns>The base-10 logarithm of <paramref name="value"/></returns>
	public static UInt512 Log10(UInt512 value)
	{
		return (UInt512)BitHelper.Log10(in value);
	}

	/// <summary>
	/// Raises a <see cref="UInt512"/> value to the power of a specified value.
	/// </summary>
	/// <param name="value">The number to raise to the <paramref name="exponent"/> power.</param>
	/// <param name="exponent">The exponent to raise <paramref name="value"/> by.</param>
	/// <returns>The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power.</returns>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="exponent"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// The result of raising <paramref name="value"/> to the <paramref name="exponent"/> power is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
	/// </exception>
	public static UInt512 Pow(UInt512 value, int exponent)
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

		if (value._p7 == 0 && value._p6 == 0 && value._p5 == 0 && value._p4 == 0 && value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
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

		UInt512 result = new UInt512(bits);

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
	/// <exception cref="OverflowException"><paramref name="s" /> is not representable by <see cref="UInt512"/>.</exception>
	public static UInt512 Parse(ReadOnlySpan<char> s)
	{
		return Parse(s, CultureInfo.CurrentCulture);
	}
	/// <summary>Tries to parse a span of characters into a value.</summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
	/// <returns><c>true</c> if <paramref name="s" /> was successfully parsed; otherwise, <c>false</c>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, out UInt512 result)
	{
		return TryParse(s, CultureInfo.CurrentCulture, out result);
	}

	private static UInt512 ToUInt512(double value)
	{
		const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

		Debug.Assert(value >= 0);
		Debug.Assert(double.IsFinite(value));
		Debug.Assert(value < TwoPow512);

		if (value >= 1.0)
		{
			// In order to convert from double to uint512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by right shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			ulong bits = BitConverter.DoubleToUInt64Bits(value);

			var exponent = ((bits >> 52) & 0x7FF) - 1023;
			var matissa = (bits & 0x0F_FFFF_FFFF_FFFF) | 0x10_0000_0000_0000;

			if (exponent <= 52)
			{
				return (matissa >> (int)(52 - exponent));
			}
			else if (exponent >= 512)
			{
				return UInt512.MaxValue;
			}
			else
			{
				return ((UInt512)matissa) << ((int)(exponent - 52));
			}
		}
		else
		{
			return MinValue;
		}
	}
}