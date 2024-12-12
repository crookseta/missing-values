using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Buffers;
using System.Collections.Generic;
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
	/// Represents a 256-bit signed integer.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.Int256Converter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	[DebuggerTypeProxy(typeof(IntDebugView<Int256>))]
	public readonly partial struct Int256
	{
		internal const int Size = 32;

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
		public Int256(UInt128 lower) : this(UInt128.Zero, lower)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Int256" /> struct.
		/// </summary>
		/// <param name="upper">The upper 128-bits of the 256-bit value.</param>
		/// <param name="lower">The lower 128-bits of the 256-bit value.</param>
		public Int256(UInt128 upper, UInt128 lower)
		{
			_p0 = (ulong)lower;
			_p1 = (ulong)(lower >> 64);
			_p2 = (ulong)upper;
			_p3 = (ulong)(upper >> 64);
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
			const int UIntCount = Size / sizeof(uint);

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
			uint[]? bitsArray = null;
			scoped Span<uint> bits;

			if (value <= int.MaxValue && value >= int.MinValue)
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
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Exponentiation);
				}

				size = Calculator.PowBound(power, 1);

				bits = (size <= Calculator.StackAllocThreshold 
					? stackalloc uint[Calculator.StackAllocThreshold]
					: bitsArray = ArrayPool<uint>.Shared.Rent(size));
				bits.Clear();

				Calculator.Pow(unchecked((uint)sign), power, bits[..size]);
			}
			else
			{
				if (power >= ((Size * 8) - 1))
				{
					Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Exponentiation);
				}

				int valueLength = (UIntCount - (BitHelper.LeadingZeroCount(in value) / 32));
				size = Calculator.PowBound(power, valueLength);

				Span<uint> valueSpan = stackalloc uint[UIntCount];
				valueSpan.Clear();
				Unsafe.WriteUnaligned(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(valueSpan)), value);

				bits = (size <= Calculator.StackAllocThreshold
					? stackalloc uint[Calculator.StackAllocThreshold]
					: bitsArray = ArrayPool<uint>.Shared.Rent(size));
				bits.Clear();

				Calculator.Pow(valueSpan[..valueLength], power, bits[..size]);
			}

			if (size > UIntCount)
			{
				Span<uint> overflow = bits[UIntCount..];

				for (int i = 0; i < overflow.Length; i++)
				{
					if (overflow[i] != 0)
					{
						Thrower.ArithmethicOverflow(Thrower.ArithmethicOperation.Exponentiation);
					}
				}
			}

			Int256 result = Unsafe.ReadUnaligned<Int256>(ref Unsafe.As<uint, byte>(ref MemoryMarshal.GetReference(bits[..UIntCount])));

            if (bitsArray is not null)
            {
                ArrayPool<uint>.Shared.Return(bitsArray);
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

		#region From Int256
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator char(in Int256 value) => (char)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="char"/>.</exception>
		public static explicit operator checked char(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((char)value._p0);
		}
		// Signed
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator sbyte(in Int256 value) => (sbyte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
		public static explicit operator checked sbyte(in Int256 value)
		{
			if (~(value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((sbyte)lower);
			}

			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator short(in Int256 value) => (short)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
		public static explicit operator checked short(in Int256 value)
		{
			if (~(value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((short)lower);
			}

			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator int(in Int256 value) => (int)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
		public static explicit operator checked int(in Int256 value)
		{
			if (~(value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((int)lower);
			}

			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator long(in Int256 value) => (long)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
		public static explicit operator checked long(in Int256 value)
		{
			if (~(value._p3 | value._p2 | value._p1) == 0)
			{
				return checked((long)value._p0);
			}

			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int128(in Int256 value) => (Int128)value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
		public static explicit operator checked Int128(in Int256 value)
		{
			if (~(value._p3 | value._p2) == 0)
			{
				return checked((Int128)value.Lower);
			}

			if (value._p3 != 0 || value._p2 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((Int128)value.Lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="Int256" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int512(in Int256 value) => new Int512(0, 0, 0, 0, value._p3, value._p2, value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nint(in Int256 value) => (nint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
		public static explicit operator checked nint(in Int256 value)
		{
			if (~(value._p3 | value._p2 | value._p1) == 0)
			{
				long lower = (long)value._p0;
				return checked((nint)lower);
			}

			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._p0);
		}
		// Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator byte(in Int256 value) => (byte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
		public static explicit operator checked byte(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ushort(in Int256 value) => (ushort)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
		public static explicit operator checked ushort(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator uint(in Int256 value) => (uint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
		public static explicit operator checked uint(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ulong(in Int256 value) => value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
		public static explicit operator checked ulong(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value._p0;
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt128(in Int256 value) => value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
		public static explicit operator checked UInt128(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value.Lower;
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt256(in Int256 value) => Unsafe.BitCast<Int256, UInt256>(value);
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
		public static explicit operator checked UInt256(in Int256 value)
		{
			if ((long)value._p3 < 0)
			{
				Thrower.IntegerOverflow();
			}
			return Unsafe.BitCast<Int256, UInt256>(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(in Int256 value) => new(0, 0, 0, 0, value._p3, value._p2, value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(in Int256 value)
		{
			if ((long)value._p3 < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new(0, 0, 0, 0, value._p3, value._p2, value._p1, value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nuint(in Int256 value) => (nuint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
		public static explicit operator checked nuint(in Int256 value)
		{
			if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value._p0);
		}

		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="BigInteger"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator BigInteger(in Int256 value)
		{
			if (~(value._p3 & value._p2 & value._p1) == 0)
			{
				return new BigInteger((long)value._p0);
			}
			if (value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
			{
				return new BigInteger(value._p0);
			}

			Span<byte> span = stackalloc byte[Size];
			value.WriteLittleEndianUnsafe(span);
			return new BigInteger(span, (long)value._p3 >= 0);
		}
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="decimal"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator decimal(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(decimal)(double)(UInt256)(abs);
			}
			return (decimal)(double)(UInt256)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Octo(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(Octo)(UInt256)(abs);
			}
			return (Octo)(UInt256)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(Quad)(UInt256)(abs);
			}
			return (Quad)(UInt256)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="double"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator double(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(double)(UInt256)(abs);
			}
			return (double)(UInt256)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="float"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator float(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(float)(UInt256)(abs);
			}
			return (float)(UInt256)(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int256" /> value to a <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Half(in Int256 value)
		{
			if (IsNegative(value))
			{
				Int256 abs = -value;
				return -(Half)(UInt256)(abs);
			}
			return (Half)(UInt256)(value);
		}
		#endregion
		#region To Int256
		// Signed
		/// <summary>
		/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(sbyte value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)(lowerShifted), (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="short" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(short value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)(lowerShifted), (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="int" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(int value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)(lowerShifted), (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="nint" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(nint value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)(lowerShifted), (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="long" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(long value)
		{
			long lower = value;
			long lowerShifted = lower >> 63;
			return new((ulong)(lowerShifted), (ulong)lowerShifted, (ulong)lowerShifted, (ulong)lower);
		}
		/// <summary>
		/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Int256(Int128 value)
		{
			ref long v = ref Unsafe.As<Int128, long>(ref value);
			ulong lowerShifted = (ulong)(Unsafe.Add(ref v, 1) >> 63);
			return new(lowerShifted, lowerShifted, (ulong)Unsafe.Add(ref v, 1), (ulong)v);
		}
		//Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="byte" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(byte value) => new Int256(0,0,0, value);
		/// <summary>
		/// Explicitly converts a <see cref="ushort" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(ushort value) => new Int256(0,0,0, value);
		/// <summary>
		/// Explicitly converts a <see cref="uint" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(uint value) => new Int256(0,0,0, value);
		/// <summary>
		/// Explicitly converts a <see cref="nuint" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(nuint value) => new Int256(0,0,0, value);
		/// <summary>
		/// Explicitly converts a <see cref="ulong" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(ulong value) => new Int256(0,0,0, value);
		/// <summary>
		/// Explicitly converts a <see cref="UInt128" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(UInt128 value)
		{
			return new Int256(0, 0, Unsafe.Add(ref Unsafe.As<UInt128, ulong>(ref value), 1), (ulong)value);
		}

		/// <summary>
		/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(BigInteger value)
		{
			bool isUnsigned = BigInteger.IsPositive(value);

			Span<byte> span = stackalloc byte[value.GetByteCount()];
			value.TryWriteBytes(span, out int bytesWritten, isUnsigned);

			ref byte sourceRef = ref MemoryMarshal.GetReference(span);

			if (bytesWritten >= Size)
			{
				return Unsafe.ReadUnaligned<Int256>(ref sourceRef);
			}

			Int256 result = Zero;

			for (int i = 0; i < bytesWritten; i++)
			{
				Int256 part = Unsafe.Add(ref sourceRef, i);
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
		/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(BigInteger value)
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
				return Unsafe.ReadUnaligned<Int256>(ref sourceRef);
			}

			Int256 result = Zero;

			// We have between 1 and 63 bytes, so construct the relevant value directly
			// since the data is in Little Endian format, we can just read the bytes and
			// shift left by 8-bits for each subsequent part, then reverse endianness to
			// ensure the order is correct. This is more efficient than iterating in reverse
			// due to current JIT limitations

			for (int i = 0; i < bytesWritten; i++)
			{
				Int256 part = Unsafe.Add(ref sourceRef, i);
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
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(decimal value) => (Int256)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(decimal value) => checked((Int256)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(double value)
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

			return ToInt256(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(double value)
		{
			const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

			if ((0.0d > value + TwoPow255) || double.IsNaN(value) || (value > +TwoPow255))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow255 - value)
			{
				return MaxValue;
			}

			return ToInt256(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(float value) => (Int256)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(float value) => checked((Int256)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(Half value) => (Int256)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(Half value) => checked((Int256)(double)value);
		#endregion

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

				Int256 result = new Int256(new UInt128((bits << 12) >> 1 | 0x8000_0000_0000_0000, 0x0000_0000_0000_0000), UInt128.Zero);
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
}
