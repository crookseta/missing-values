using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Int512
{
	/// <inheritdoc/>
	public static Int512 CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int512 result;

		if (value is Int512 v)
		{
			result = v;
		}
		else if (!Int512.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<Int512>(value, out result))
		{
			Thrower.NotSupported<Int512, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Int512 CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int512 result;

		if (value is Int512 v)
		{
			result = v;
		}
		else if (!Int512.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<Int512>(value, out result))
		{
			Thrower.NotSupported<Int512, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Int512 CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int512 result;

		if (value is Int512 v)
		{
			result = v;
		}
		else if (!Int512.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<Int512>(value, out result))
		{
			Thrower.NotSupported<Int512, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<Int512>.TryConvertFromChecked<TOther>(TOther value, out Int512 result) => TryConvertFromChecked(value, out result);
	private static bool TryConvertFromChecked<TOther>(TOther value, out Int512 result)
		where TOther : INumberBase<TOther>
	{
		bool converted = true;

		checked
		{
			result = value switch
			{
				char actual => (Int512)actual,
				Half actual => (Int512)actual,
				float actual => (Int512)actual,
				double actual => (Int512)actual,
				NFloat actual => (Int512)actual,
				Quad actual => (Int512)actual,
				decimal actual => (Int512)actual,
				byte actual => (Int512)actual,
				ushort actual => (Int512)actual,
				uint actual => (Int512)actual,
				ulong actual => (Int512)actual,
				UInt128 actual => (Int512)actual,
				UInt256 actual => (Int512)actual,
				UInt512 actual => (Int512)actual,
				nuint actual => (Int512)actual,
				sbyte actual => (Int512)actual,
				short actual => (Int512)actual,
				int actual => (Int512)actual,
				long actual => (Int512)actual,
				Int128 actual => (Int512)actual,
				Int256 actual => (Int512)actual,
				Int512 actual => actual,
				nint actual => (Int512)actual,
				BigInteger actual => (Int512)actual,
				_ => BitHelper.DefaultConvert<Int512>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<Int512>.TryConvertFromSaturating<TOther>(TOther value, out Int512 result) => TryConvertFromSaturating(value, out result);
	private static bool TryConvertFromSaturating<TOther>(TOther value, out Int512 result)
		where TOther : INumberBase<TOther>
	{
		const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
			Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
			float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
			double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
			NFloat actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
			Quad actual => (actual <= (new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MinValue : (actual > (new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MaxValue : (Int512)actual,
			decimal actual => (Int512)actual,
			byte actual => (Int512)actual,
			ushort actual => (Int512)actual,
			uint actual => (Int512)actual,
			ulong actual => (Int512)actual,
			UInt128 actual => (Int512)actual,
			UInt256 actual => (Int512)actual,
			UInt512 actual => (actual > (UInt512)MaxValue) ? MaxValue : (Int512)actual,
			nuint actual => (Int512)actual,
			sbyte actual => actual,
			short actual => actual,
			int actual => actual,
			long actual => actual,
			Int128 actual => actual,
			Int256 actual => actual,
			Int512 actual => actual,
			nint actual => actual,
			BigInteger actual => (actual < (BigInteger)MinValue) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (Int512)actual,
			_ => BitHelper.DefaultConvert<Int512>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Int512>.TryConvertFromTruncating<TOther>(TOther value, out Int512 result) => TryConvertFromTruncating(value, out result);
	private static bool TryConvertFromTruncating<TOther>(TOther value, out Int512 result)
		where TOther : INumberBase<TOther>
	{
		const double TwoPow511 = 6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
			Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
			float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int512)actual,
			double actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
			NFloat actual => (actual <= -TwoPow511) ? MinValue : (actual > +TwoPow511) ? MaxValue : (Int512)actual,
			Quad actual => (actual <= (new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MinValue : (actual > (new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000))) ? MaxValue : (Int512)actual,
			decimal actual => (Int512)actual,
			byte actual => (Int512)actual,
			ushort actual => (Int512)actual,
			uint actual => (Int512)actual,
			ulong actual => (Int512)actual,
			UInt128 actual => (Int512)actual,
			UInt256 actual => (Int512)actual,
			UInt512 actual => (Int512)actual,
			nuint actual => (Int512)actual,
			sbyte actual => actual,
			short actual => actual,
			int actual => actual,
			long actual => actual,
			Int128 actual => actual,
			Int256 actual => actual,
			Int512 actual => actual,
			nint actual => actual,
			BigInteger actual => (Int512)actual,
			_ => BitHelper.DefaultConvert<Int512>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Int512>.TryConvertToChecked<TOther>(Int512 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		checked
		{
			result = result switch
			{
				char => (TOther)(object)(char)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				NFloat => (TOther)(object)(NFloat)value,
				Quad => (TOther)(object)(Quad)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)(byte)value,
				ushort => (TOther)(object)(ushort)value,
				uint => (TOther)(object)(uint)value,
				ulong => (TOther)(object)(ulong)value,
				UInt128 => (TOther)(object)(UInt128)value,
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)(nuint)value,
				sbyte => (TOther)(object)(sbyte)value,
				short => (TOther)(object)(short)value,
				int => (TOther)(object)(int)value,
				long => (TOther)(object)(long)value,
				Int128 => (TOther)(object)(Int128)value,
				Int256 => (TOther)(object)(Int256)value,
				Int512 => (TOther)(object)value,
				nint => (TOther)(object)(nint)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<Int512>.TryConvertToSaturating<TOther>(Int512 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;

		result = result switch
		{
			char => (TOther)(object)(char)value,
			Half => (TOther)(object)(Half)value,
			float => (TOther)(object)(float)value,
			double => (TOther)(object)(double)value,
			NFloat => (TOther)(object)(NFloat)value,
			Quad => (TOther)(object)(Quad)value,
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)((value >= (Int512)byte.MaxValue) ? byte.MaxValue : (value <= (Int512)byte.MinValue) ? byte.MinValue : (byte)value),
			ushort => (TOther)(object)((value >= (Int512)ushort.MaxValue) ? ushort.MaxValue : (value <= (Int512)ushort.MinValue) ? ushort.MinValue : (ushort)value),
			uint => (TOther)(object)((value >= (Int512)uint.MaxValue) ? uint.MaxValue : (value <= (Int512)uint.MinValue) ? uint.MinValue : (uint)value),
			ulong => (TOther)(object)((value >= (Int512)ulong.MaxValue) ? ulong.MaxValue : (value <= (Int512)ulong.MinValue) ? ulong.MinValue : (ulong)value),
			UInt128 => (TOther)(object)((value >= (Int512)UInt128.MaxValue) ? UInt128.MaxValue : (value <= (Int512)UInt128.MinValue) ? UInt128.MinValue : (UInt128)value),
			UInt256 => (TOther)(object)((value >= (Int512)UInt256.MaxValue) ? UInt256.MaxValue : (value <= (Int512)UInt256.MinValue) ? UInt256.MinValue : (UInt256)value),
			UInt512 => (TOther)(object)(UInt512)value,
			nuint => (TOther)(object)((value >= (Int512)nuint.MaxValue) ? nuint.MaxValue : (value <= (Int512)nuint.MinValue) ? nuint.MinValue : (nuint)value),
			sbyte => (TOther)(object)((value >= (Int512)sbyte.MaxValue) ? sbyte.MaxValue : (value <= (Int512)sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
			short => (TOther)(object)((value >= (Int512)short.MaxValue) ? short.MaxValue : (value <= (Int512)short.MinValue) ? short.MinValue : (short)value),
			int => (TOther)(object)((value >= (Int512)int.MaxValue) ? int.MaxValue : (value <= (Int512)int.MinValue) ? int.MinValue : (int)value),
			long => (TOther)(object)((value >= (Int512)long.MaxValue) ? long.MaxValue : (value <= (Int512)long.MinValue) ? long.MinValue : (long)value),
			Int128 => (TOther)(object)((value >= (Int512)Int128.MaxValue) ? Int128.MaxValue : (value <= (Int512)Int128.MinValue) ? Int128.MinValue : (Int128)value),
			Int256 => (TOther)(object)((value >= (Int512)Int256.MaxValue) ? Int256.MaxValue : (value <= (Int512)Int256.MinValue) ? Int128.MinValue : (Int256)value),
			Int512 => (TOther)(object)value,
			nint => (TOther)(object)((value >= (Int512)nint.MaxValue) ? nint.MaxValue : (value <= (Int512)nint.MinValue) ? nint.MinValue : (nint)value),
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Int512>.TryConvertToTruncating<TOther>(Int512 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		result = result switch
		{
			char => (TOther)(object)(char)value,
			Half => (TOther)(object)(Half)value,
			float => (TOther)(object)(float)value,
			double => (TOther)(object)(double)value,
			NFloat => (TOther)(object)(NFloat)value,
			Quad => (TOther)(object)(Quad)value,
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)(byte)value,
			ushort => (TOther)(object)(ushort)value,
			uint => (TOther)(object)(uint)value,
			ulong => (TOther)(object)(ulong)value,
			UInt128 => (TOther)(object)(UInt128)value,
			UInt256 => (TOther)(object)(UInt256)value,
			UInt512 => (TOther)(object)(UInt512)value,
			nuint => (TOther)(object)(nuint)value,
			sbyte => (TOther)(object)(sbyte)value,
			short => (TOther)(object)(short)value,
			int => (TOther)(object)(int)value,
			long => (TOther)(object)(long)value,
			Int128 => (TOther)(object)(Int128)value,
			Int256 => (TOther)(object)(Int256)value,
			Int512 => (TOther)(object)value,
			nint => (TOther)(object)(nint)value,
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}
	
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
		BinaryOperations.WriteInt512LittleEndian(span, in value);
		return new BigInteger(span, (long)value._p7 >= 0);
	}

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
	/// <summary>
	/// Explicitly converts a <see cref="Int512" /> value to a <see cref="NFloat"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(in Int512 value)
	{
		if ((long)value._p7 < 0)
		{
			Int512 abs = -value;
			return -(NFloat)(UInt512)(abs);
		}
		return (NFloat)(UInt512)(value);
	}
	
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
			0, 0, value.Upper, value.Lower
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
		long v = unchecked((long)value.Upper);
		ulong lowerShifted = unchecked((ulong)(v >> 63));
		return new(
			lowerShifted, lowerShifted, lowerShifted, lowerShifted,
			lowerShifted, lowerShifted, value.Upper, value.Lower
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

		if (bytesWritten >= Size)
		{
			return BinaryOperations.ReadInt512LittleEndian(span);
		}

		BitHelper.TryReadLittleEndian(span[..bytesWritten], isUnsigned, out Int512 result);

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

		if (!BitHelper.TryReadLittleEndian(span[..bytesWritten], isUnsigned, out Int512 result))
		{
			Thrower.IntegerOverflow();
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
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int512(NFloat value)
	{
		return NFloat.Size == 8 ? (Int512)(double)value : (Int512)(float)value;
	}
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
	public static explicit operator checked Int512(NFloat value)
	{
		return NFloat.Size == 8 ? checked((Int512)(double)value) : checked((Int512)(float)value);
	}
}