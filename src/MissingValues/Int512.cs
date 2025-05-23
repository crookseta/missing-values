using MissingValues.Info;
using MissingValues.Internals;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace MissingValues
{
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
			_p0 = (ulong)ll;
			_p1 = (ulong)(ll >>> 64);
			_p2 = (ulong)lu;
			_p3 = (ulong)(lu >>> 64);
			_p4 = (ulong)ul;
			_p5 = (ulong)(ul >>> 64);
			_p6 = (ulong)uu;
			_p7 = (ulong)(uu >>> 64);
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
		public override string? ToString()
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
				Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(valueSpan)), value);

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

			Int512 result = Unsafe.ReadUnaligned<Int512>(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(bits[..UIntCount])));

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

		#region From Int512
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator char(in Int512 value) => (char)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="char"/>.</exception>
		public static explicit operator checked char(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((char)value._p0);
		}
		// Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator byte(in Int512 value) => (byte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
		public static explicit operator checked byte(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ushort(in Int512 value) => (ushort)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
		public static explicit operator checked ushort(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator uint(in Int512 value) => (uint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
		public static explicit operator checked uint(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ulong(in Int512 value) => value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
		public static explicit operator checked ulong(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value._p0;
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt128(in Int512 value) => new UInt128(value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
		public static explicit operator checked UInt128(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt128(value._p1, value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt256(in Int512 value) => value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
		public static explicit operator checked UInt256(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value.Lower;
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(in Int512 value) => Unsafe.BitCast<Int512, UInt512>(value);
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Thrower.IntegerOverflow();
			}
			return Unsafe.BitCast<Int512, UInt512>(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nuint(in Int512 value) => (nuint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
		public static explicit operator checked nuint(in Int512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value._p0);
		}
		// Signed
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator sbyte(in Int512 value) => (sbyte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
		public static explicit operator checked sbyte(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((sbyte)lower);
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator short(in Int512 value) => (short)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
		public static explicit operator checked short(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((short)lower);
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator int(in Int512 value) => (int)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
		public static explicit operator checked int(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((int)lower);
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator long(in Int512 value) => (long)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
		public static explicit operator checked long(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return lower;
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int128(in Int512 value) => (Int128)value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
		public static explicit operator checked Int128(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2) == 0)
			{
				Int128 lower = new Int128(value._p1, value._p0);
				return lower;
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int128)(new UInt128(value._p1, value._p0)));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(in Int512 value) => (Int256)value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4) == 0)
			{
				return (Int256)value.Lower;
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int256)value.Lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nint(in Int512 value) => (nint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
		public static explicit operator checked nint(in Int512 value)
		{
			if (~(value._p7 | value._p6 | value._p5 | value._p4 | value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((nint)lower);
			}

			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._p0);
		}

		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="BigInteger"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator BigInteger(in Int512 value)
		{
			if (~(value._p7 & value._p6 & value._p5 & value._p4 & value._p3 & value._p2 & value._p1) == 0)
			{
				return new BigInteger((long)value._p0);
			}
			if (value._p7 == 0 && value._p6 == 0 && value._p5 == 0 && value._p4 == 0 && value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
			{
				return new BigInteger(value._p0);
			}

			Span<byte> span = stackalloc byte[Size];
			value.WriteLittleEndianUnsafe(span);
			return new BigInteger(span, (long)value._p7 >= 0);
		}
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="decimal"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator decimal(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(decimal)(UInt512)(abs);
			}
			return (decimal)(UInt512)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Octo(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(Octo)(UInt512)(abs);
			}
			return (Octo)(UInt512)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(Quad)(UInt512)(abs);
			}
			return (Quad)(UInt512)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="double"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator double(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(double)(UInt512)(abs);
			}
			return (double)(UInt512)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="float"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator float(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(float)(UInt512)(abs);
			}
			return (float)(UInt512)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int512" /> value to a <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Half(in Int512 value)
		{
			if ((long)value._p7 < 0)
			{
				Int512 abs = -value;
				return -(Half)(UInt512)(abs);
			}
			return (Half)(UInt512)(value);
		}
		#endregion
		#region To Int512
		//Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="byte" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(byte value) => new Int512(value);
		/// <summary>
		/// Explicitly converts a <see cref="ushort" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(ushort value) => new Int512(value);
		/// <summary>
		/// Explicitly converts a <see cref="uint" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(uint value) => new Int512(value);
		/// <summary>
		/// Explicitly converts a <see cref="nuint" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(nuint value) => new Int512(value);
		/// <summary>
		/// Explicitly converts a <see cref="ulong" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(ulong value) => new Int512(value);
		/// <summary>
		/// Explicitly converts a <see cref="UInt128" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(UInt128 value)
		{
			return new Int512(
				0, 0, 0, 0,
				0, 0, Unsafe.Add(ref Unsafe.As<UInt128, ulong>(ref value), 1), (ulong)value
				);
		}

		//Signed
		/// <summary>
		/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(sbyte value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="short" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(short value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="int" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(int value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="nint" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(nint value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="long" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(long value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(Int128 value)
		{
			ref long v = ref Unsafe.As<Int128, long>(ref value);
			ulong lowerShifted = (ulong)(Unsafe.Add(ref v, 1) >> 63);
			return new(
				lowerShifted, lowerShifted, lowerShifted, lowerShifted,
				lowerShifted, lowerShifted, (ulong)Unsafe.Add(ref v, 1), (ulong)v
				);
		}

		/// <summary>
		/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(BigInteger value)
		{
			bool isUnsigned = BigInteger.IsPositive(value);

			Span<byte> span = stackalloc byte[value.GetByteCount()];
			value.TryWriteBytes(span, out int bytesWritten, isUnsigned);

			ref byte sourceRef = ref MemoryMarshal.GetReference(span);

			if (bytesWritten >= Size)
			{
				return Unsafe.ReadUnaligned<Int512>(ref sourceRef);
			}

			Int512 result = Zero;

			for (int i = 0; i < bytesWritten; i++)
			{
				Int512 part = Unsafe.Add(ref sourceRef, i);
				part <<= (i * 8);
				result |= part;
			}

			result <<= ((Size - bytesWritten) * 8);

			if (!isUnsigned)
			{
				result |= ((One << ((Size * 8) - 1)) >> (((Size - bytesWritten) * 8) - 1));
			}

			return result;
		}
		/// <summary>
		/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(BigInteger value)
		{
			bool isUnsigned = BigInteger.IsPositive(value);

			Span<byte> span = stackalloc byte[isUnsigned ? Size : value.GetByteCount()];
			if (!value.TryWriteBytes(span, out int bytesWritten, isUnsigned))
			{
				Thrower.IntegerOverflow();
			}

			// Propagate the most significant bit so we have `0` or `-1`
			sbyte sign = (sbyte)(span[^1]);
			sign >>= 31;

			// We need to also track if the input data is unsigned
			isUnsigned |= (sign == 0);

			if (isUnsigned && sbyte.IsNegative(sign) && (bytesWritten >= Size))
			{
				// When we are unsigned and the most significant bit is set, we are a large positive
				// and therefore definitely out of range

				Thrower.IntegerOverflow();
			}

			if (bytesWritten > Size)
			{
				if (span[Size..].IndexOfAnyExcept((byte)sign) >= 0)
				{
					// When we are unsigned and have any non-zero leading data or signed with any non-set leading
					// data, we are a large positive/negative, respectively, and therefore definitely out of range

					Thrower.IntegerOverflow();
				}

				if (isUnsigned == sbyte.IsNegative((sbyte)span[Size - 1]))
				{
					// When the most significant bit of the value being set/clear matches whether we are unsigned
					// or signed then we are a large positive/negative and therefore definitely out of range

					Thrower.IntegerOverflow();
				}
			}

			ref byte sourceRef = ref MemoryMarshal.GetReference(span);

			if (bytesWritten >= Size)
			{
				return Unsafe.ReadUnaligned<Int512>(ref sourceRef);
			}

			Int512 result = Zero;

			// We have between 1 and 63 bytes, so construct the relevant value directly
			// since the data is in Little Endian format, we can just read the bytes and
			// shift left by 8-bits for each subsequent part, then reverse endianness to
			// ensure the order is correct. This is more efficient than iterating in reverse
			// due to current JIT limitations

			for (int i = 0; i < bytesWritten; i++)
			{
				Int512 part = Unsafe.Add(ref sourceRef, i);
				part <<= (i * 8);
				result |= part;
			}

			result <<= ((Size - bytesWritten) * 8);
			result = BitHelper.ReverseEndianness(in result);

			if (!isUnsigned)
			{
				result |= ((One << ((Size * 8) - 1)) >> (((Size - bytesWritten) * 8) - 1));
			}

			return result;
		}
		//Floating
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(decimal value) => (Int512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(decimal value) => checked((Int512)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(double value)
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if (value <= -TwoPow255)
			{
				return MinValue;
			}
			else if (double.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +TwoPow255)
			{
				return MaxValue;
			}

			return ToInt512(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(double value)
		{
			const double TwoPow511 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if ((0.0d > value + TwoPow511) || double.IsNaN(value) || (value > +TwoPow511))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow511 - value)
			{
				return MaxValue;
			}

			return ToInt512(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(float value) => (Int512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(float value) => checked((Int512)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(Half value) => (Int512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(Half value) => checked((Int512)(double)value);
		#endregion

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
}
