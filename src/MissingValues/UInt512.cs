using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Represents a 512-bit unsigned integer.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.UInt512Converter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
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
			lower.GetLowerParts(out _p1, out _p0);
			lower.GetUpperParts(out _p3, out _p2);
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
			lower.GetLowerParts(out _p1, out _p0);
			lower.GetUpperParts(out _p3, out _p2);
			upper.GetLowerParts(out _p5, out _p4);
			upper.GetUpperParts(out _p7, out _p6);
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
		public override string? ToString()
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
			ReadOnlySpan<uint> leftSpan = MemoryMarshal.CreateReadOnlySpan(in Unsafe.As<UInt512, uint>(ref left), (Size / sizeof(uint)) - (BitHelper.LeadingZeroCount(in left) / 32));

			ReadOnlySpan<uint> rightSpan = MemoryMarshal.CreateReadOnlySpan(in Unsafe.As<UInt512, uint>(ref right), (Size / sizeof(uint)) - (BitHelper.LeadingZeroCount(in right) / 32));

			Span<uint> rawBits = stackalloc uint[(Size / sizeof(uint)) * 2];
			rawBits.Clear();

			Calculator.Multiply(leftSpan, rightSpan, rawBits);

			lower = new UInt512(
				(((ulong)rawBits[15] << 32) | rawBits[14]), (((ulong)rawBits[13] << 32) | rawBits[12]),
				(((ulong)rawBits[11] << 32) | rawBits[10]), (((ulong)rawBits[09] << 32) | rawBits[08]),
				(((ulong)rawBits[07] << 32) | rawBits[06]), (((ulong)rawBits[05] << 32) | rawBits[04]),
				(((ulong)rawBits[03] << 32) | rawBits[02]), (((ulong)rawBits[01] << 32) | rawBits[00])
				);
			return new UInt512(
				(((ulong)rawBits[31] << 32) | rawBits[30]), (((ulong)rawBits[29] << 32) | rawBits[28]),
				(((ulong)rawBits[27] << 32) | rawBits[26]), (((ulong)rawBits[25] << 32) | rawBits[24]),
				(((ulong)rawBits[23] << 32) | rawBits[22]), (((ulong)rawBits[21] << 32) | rawBits[20]),
				(((ulong)rawBits[19] << 32) | rawBits[18]), (((ulong)rawBits[17] << 32) | rawBits[16])
				);
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

		#region From UInt512
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator char(UInt512 value) => (char)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="char"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="char"/>.</exception>
		public static explicit operator checked char(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((char)value._p0);
		}
		// Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator byte(UInt512 value) => (byte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
		public static explicit operator checked byte(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((byte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ushort(UInt512 value) => (ushort)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
		public static explicit operator checked ushort(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((ushort)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator uint(UInt512 value) => (uint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
		public static explicit operator checked uint(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((uint)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ulong(UInt512 value) => value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
		public static explicit operator checked ulong(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value._p0;
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt128(UInt512 value) => new UInt128(value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
		public static explicit operator checked UInt128(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt128(value._p1, value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt256(UInt512 value) => value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
		public static explicit operator checked UInt256(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return value.Lower;
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nuint(UInt512 value) => (nuint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nuint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
		public static explicit operator checked nuint(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nuint)value.Lower);
		}
		// Signed
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator sbyte(UInt512 value) => (sbyte)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
		public static explicit operator checked sbyte(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((sbyte)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator short(UInt512 value) => (short)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
		public static explicit operator checked short(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((short)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator int(UInt512 value) => (int)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
		public static explicit operator checked int(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((int)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator long(UInt512 value) => (long)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
		public static explicit operator checked long(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((long)value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int128(UInt512 value) => new Int128(value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
		public static explicit operator checked Int128(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || (long)value._p1 < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new Int128(value._p1, value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(UInt512 value) => (Int256)value.Lower;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return (Int256)value.Lower;
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(UInt512 value) => new Int512(value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0);
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(UInt512 value)
		{
			if ((long)value._p7 < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new Int512(value._p7, value._p6, value._p5, value._p4, value._p3, value._p2, value._p1, value._p0);
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator nint(UInt512 value) => (nint)value._p0;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
		public static explicit operator checked nint(UInt512 value)
		{
			if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
			{
				Thrower.IntegerOverflow();
			}
			return checked((nint)value._p0);
		}
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="decimal"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="decimal"/>.</exception>
		public static explicit operator decimal(UInt512 value)
		{
			if (value.Upper != 0)
			{
				// The default behavior of decimal conversions is to always throw on overflow
				Thrower.IntegerOverflow();
			}

			return (decimal)value.Lower;
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Octo(UInt512 value)
		{
			if (value.Upper == 0)
			{
				return value._p1 != 0 ? (Octo)value.Lower : (Octo)value._p0;
			}
			else if ((value.Upper >> 32) == UInt128.Zero) // value < (2^472)
			{
				// For values greater than MaxValue but less than 2^472 this takes advantage
				// that we can represent both "halves" of the uint256 within the 236-bit mantissa of
				// a pair of octos.
				Octo twoPow236 = new Octo(0x400E_B000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
				Octo twoPow472 = new Octo(0x401D_7000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

				UInt256 twoPow236bits = Octo.OctoToUInt256Bits(twoPow236);
				UInt256 twoPow472bits = Octo.OctoToUInt256Bits(twoPow472);

				Octo lower = Octo.UInt256BitsToOcto(twoPow236bits | ((value.Lower << 20) >> 20)) - twoPow236;
				Octo upper = Octo.UInt256BitsToOcto(twoPow472bits | (UInt256)(value >> 236)) - twoPow472;

				return lower + upper;
			}
			else
			{
				// For values greater than 2^472 we basically do the same as before but we need to account
				// for the precision loss that octo will have. As such, the lower value effectively drops the
				// lowest 40 bits and then or's them back to ensure rounding stays correct.

				Octo twoPow276 = new Octo(0x4011_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
				Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

				UInt256 twoPow276bits = Octo.OctoToUInt256Bits(twoPow276);
				UInt256 twoPow512bits = Octo.OctoToUInt256Bits(twoPow512);

				Octo lower = Octo.UInt256BitsToOcto(twoPow276bits | ((UInt256)(value >> 20) >> 20) | (value._p0 & 0xFF_FFFF_FFFF)) - twoPow276;
				Octo upper = Octo.UInt256BitsToOcto(twoPow512bits | (UInt256)(value >> 276)) - twoPow512;

				return lower + upper;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(UInt512 value)
		{
			if (value.Upper == UInt256.Zero)
			{
				return (Quad)value.Lower;
			}
			else
			{
				// For values greater than 2^224 we basically do the same as before but we need to account
				// for the precision loss that quad will have. As such, the lower value effectively drops the
				// lowest 288 bits.
				Quad twoPow400 = new Quad(0x418F_0000_0000_0000, 0x0000_0000_0000_0000);
				Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);

				UInt128 twoPow400bits = Quad.QuadToUInt128Bits(twoPow400);
				UInt128 twoPow512bits = Quad.QuadToUInt128Bits(twoPow512);

				Quad lower = Quad.UInt128BitsToQuad(twoPow400bits | (UInt128)((UInt256)(value >> 144) >> 144) | (value.Upper.Lower & 0xFFFF_FFFF)) - twoPow400;
				Quad upper = Quad.UInt128BitsToQuad(twoPow512bits | (UInt128)(value >> 400)) - twoPow512;

				return lower + upper;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="double"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator double(UInt512 value)
		{
			const double TwoPow460 = 2977131414714805823690030317109266572712515013375254774912983855843898524112477893944078543723575564536883288499266264815757728270805630976.0d;
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			const ulong TwoPow460bits = 0x5CB0_0000_0000_0000;
			const ulong TwoPow512bits = 0x5FF0_0000_0000_0000;

			if (value.Upper == 0)
			{
				return (double)value.Lower;
			}

			double lower = BitConverter.UInt64BitsToDouble(TwoPow460bits | ((ulong)(value >> 12) >> 12) | ((ulong)(value.Lower) & 0xFFFFFF)) - TwoPow460;
			double upper = BitConverter.UInt64BitsToDouble(TwoPow512bits | (ulong)(value >> 460)) - TwoPow512;

			return lower + upper;
		}
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="float"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator float(UInt512 value) => (float)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Half(UInt512 value) => (Half)(double)value;
		#endregion
		#region To UInt512
		/// <summary>
		/// Implicitly converts a <see cref="char" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(char value) => new UInt512(value);
		//Unsigned
		/// <summary>
		/// Implicitly converts a <see cref="byte" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(byte value) => new UInt512(value);
		/// <summary>
		/// Implicitly converts a <see cref="ushort" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(ushort value) => new UInt512(value);
		/// <summary>
		/// Implicitly converts a <see cref="uint" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(uint value) => new UInt512(value);
		/// <summary>
		/// Implicitly converts a <see cref="ulong" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(ulong value) => new UInt512(value);
		/// <summary>
		/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(UInt128 value) => new UInt512(value);
		/// <summary>
		/// Implicitly converts a <see cref="nuint" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator UInt512(nuint value) => new UInt512(value);
		//Signed
		/// <summary>
		/// Explicitly converts a <see cref="sbyte" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(sbyte value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="sbyte" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(sbyte value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((byte)value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="short" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(short value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="short" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(short value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((ushort)value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="int" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(int value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="int" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(int value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((uint)value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="long" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(long value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="long" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(long value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((ulong)value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int128" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(Int128 value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="Int128" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(Int128 value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((UInt128)value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="nint" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(nint value)
		{
			Int256 lower = value;
			return new UInt512((UInt256)(lower >> 255), (UInt256)lower);
		}
		/// <summary>
		/// Explicitly converts a <see cref="nint" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(nint value)
		{
			if (value < 0)
			{
				Thrower.IntegerOverflow();
			}
			return new UInt512((nuint)value);
		}
		//Floating
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(Half value) => (UInt512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="Half" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(Half value) => checked((UInt512)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(float value) => (UInt512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="float" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(float value) => checked((UInt512)(double)value);
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			if (double.IsNegative(value) || double.IsNaN(value))
			{
				return MinValue;
			}
			else if (value >= TwoPow512)
			{
				return MaxValue;
			}

			return ToUInt512(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="double" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			// value against 0 rather than checking IsNegative

			if ((value < 0.0) || double.IsNaN(value) || (0.0 < TwoPow512 - value))
			{
				Thrower.IntegerOverflow();
			}
			if (0.0 == TwoPow512 - value)
			{
				return MaxValue;
			}

			return ToUInt512(value);
		}
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(decimal value) => (UInt512)(double)value;
		/// <summary>
		/// Explicitly converts a <see cref="decimal" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(decimal value) => checked((UInt512)(double)value);
		#endregion

		private static UInt512 ToUInt512(double value)
		{
			const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0d;

			Debug.Assert(value >= 0);
			Debug.Assert(double.IsFinite(value));
			Debug.Assert(0.0 >= TwoPow512 - value);

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
					return (UInt512)(matissa >> (int)(52 - exponent));
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
}
