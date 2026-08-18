using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Int256
{
	/// <inheritdoc/>
	public static Int256 CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int256 result;

		if (value is Int256 v)
		{
			result = v;
		}
		else if (!Int256.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<Int256>(value, out result))
		{
			Thrower.NotSupported<Int256, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Int256 CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int256 result;

		if (value is Int256 v)
		{
			result = v;
		}
		else if (!Int256.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<Int256>(value, out result))
		{
			Thrower.NotSupported<Int256, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Int256 CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Int256 result;

		if (value is Int256 v)
		{
			result = v;
		}
		else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result))
		{
			Thrower.NotSupported<Int256, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<Int256>.TryConvertFromChecked<TOther>(TOther value, out Int256 result) => TryConvertFromChecked(value, out result);
	private static bool TryConvertFromChecked<TOther>(TOther value, out Int256 result)
		where TOther : INumberBase<TOther>
	{
		bool converted = true;

		checked
		{
			result = value switch
			{
				char actual => (Int256)actual,
				Half actual => (Int256)actual,
				float actual => (Int256)actual,
				double actual => (Int256)actual,
				NFloat actual => (Int256)actual,
				decimal actual => (Int256)actual,
				byte actual => (Int256)actual,
				ushort actual => (Int256)actual,
				uint actual => (Int256)actual,
				ulong actual => (Int256)actual,
				UInt128 actual => (Int256)actual,
				UInt256 actual => (Int256)actual,
				UInt512 actual => (Int256)actual,
				nuint actual => (Int256)actual,
				sbyte actual => (Int256)actual,
				short actual => (Int256)actual,
				int actual => (Int256)actual,
				long actual => (Int256)actual,
				Int128 actual => (Int256)actual,
				Int256 actual => actual,
				Int512 actual => (Int256)actual,
				nint actual => (Int256)actual,
				BigInteger actual => (Int256)actual,
				_ => BitHelper.DefaultConvert<Int256>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<Int256>.TryConvertFromSaturating<TOther>(TOther value, out Int256 result) => TryConvertFromSaturating(value, out result);
	private static bool TryConvertFromSaturating<TOther>(TOther value, out Int256 result)
		where TOther : INumberBase<TOther>
	{
		const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
			Half actual => (Int256)actual,
			float actual => (Int256)actual,
			double actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
			NFloat actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
			decimal actual => (Int256)actual,
			byte actual => (Int256)actual,
			ushort actual => (Int256)actual,
			uint actual => (Int256)actual,
			ulong actual => (Int256)actual,
			UInt128 actual => (Int256)actual,
			UInt256 actual => (actual > (UInt256)MaxValue) ? MaxValue : (Int256)actual,
			nuint actual => (Int256)actual,
			sbyte actual => actual,
			short actual => actual,
			int actual => actual,
			long actual => actual,
			Int128 actual => actual,
			Int256 actual => actual,
			Int512 actual => (actual < MinValue) ? MinValue : (actual > MaxValue) ? MaxValue : (Int256)actual,
			nint actual => actual,
			BigInteger actual => (actual < (BigInteger)MinValue) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (Int256)actual,
			_ => BitHelper.DefaultConvert<Int256>(out converted)
		};
		return converted;
	}

	static bool INumberBase<Int256>.TryConvertFromTruncating<TOther>(TOther value, out Int256 result) => TryConvertFromTruncating(value, out result);
	private static bool TryConvertFromTruncating<TOther>(TOther value, out Int256 result)
		where TOther : INumberBase<TOther>
	{
		const double TwoPow255 = 57896044618658097711785492504343953926634992332820282019728792003956564819968.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
			Half actual => (Half.IsPositiveInfinity(actual)) ? MaxValue : (Half.IsNegativeInfinity(actual)) ? MinValue : (Int256)actual,
			float actual => (float.IsPositiveInfinity(actual)) ? MaxValue : (float.IsNegativeInfinity(actual)) ? MinValue : (Int256)actual,
			double actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
			NFloat actual => (actual <= -TwoPow255) ? MinValue : (actual > +TwoPow255) ? MaxValue : (Int256)actual,
			decimal actual => (Int128)actual,
			byte actual => (Int256)actual,
			ushort actual => (Int256)actual,
			uint actual => (Int256)actual,
			ulong actual => (Int256)actual,
			UInt128 actual => (Int256)actual,
			UInt256 actual => (Int256)actual,
			nuint actual => (Int256)actual,
			sbyte actual => actual,
			short actual => actual,
			int actual => actual,
			long actual => actual,
			Int128 actual => actual,
			Int256 actual => actual,
			nint actual => actual,
			BigInteger actual => (Int256)actual,
			_ => BitHelper.DefaultConvert<Int256>(out converted)
		};
		return converted;
	}

	static bool INumberBase<Int256>.TryConvertToChecked<TOther>(Int256 value, out TOther result)
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
				Int256 => (TOther)(object)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)(nint)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<Int256>.TryConvertToSaturating<TOther>(Int256 value, out TOther result)
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
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)((value >= (Int256)byte.MaxValue) ? byte.MaxValue : (value <= (Int256)byte.MinValue) ? byte.MinValue : (byte)value),
			ushort => (TOther)(object)((value >= (Int256)ushort.MaxValue) ? ushort.MaxValue : (value <= (Int256)ushort.MinValue) ? ushort.MinValue : (ushort)value),
			uint => (TOther)(object)((value >= (Int256)uint.MaxValue) ? uint.MaxValue : (value <= (Int256)uint.MinValue) ? uint.MinValue : (uint)value),
			ulong => (TOther)(object)((value >= (Int256)ulong.MaxValue) ? ulong.MaxValue : (value <= (Int256)ulong.MinValue) ? ulong.MinValue : (ulong)value),
			UInt128 => (TOther)(object)((value >= (Int256)UInt128.MaxValue) ? UInt128.MaxValue : (value <= (Int256)UInt128.MinValue) ? UInt128.MinValue : (UInt128)value),
			UInt256 => (TOther)(object)(UInt256)value,
			UInt512 => (TOther)(object)(UInt512)value,
			nuint => (TOther)(object)((value >= (Int256)nuint.MaxValue) ? nuint.MaxValue : (value <= (Int256)nuint.MinValue) ? nuint.MinValue : (nuint)value),
			sbyte => (TOther)(object)((value >= (Int256)sbyte.MaxValue) ? sbyte.MaxValue : (value <= (Int256)sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
			short => (TOther)(object)((value >= (Int256)short.MaxValue) ? short.MaxValue : (value <= (Int256)short.MinValue) ? short.MinValue : (short)value),
			int => (TOther)(object)((value >= (Int256)int.MaxValue) ? int.MaxValue : (value <= (Int256)int.MinValue) ? int.MinValue : (int)value),
			long => (TOther)(object)((value >= (Int256)long.MaxValue) ? long.MaxValue : (value <= (Int256)long.MinValue) ? long.MinValue : (long)value),
			Int128 => (TOther)(object)((value >= (Int256)Int128.MaxValue) ? Int128.MaxValue : (value <= (Int256)Int128.MinValue) ? Int128.MinValue : (Int128)value),
			Int256 => (TOther)(object)value,
			Int512 => (TOther)(object)(Int512)value,
			nint => (TOther)(object)((value >= (Int256)nint.MaxValue) ? nint.MaxValue : (value <= (Int256)nint.MinValue) ? nint.MinValue : (nint)value),
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Int256>.TryConvertToTruncating<TOther>(Int256 value, out TOther result)
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
			Int256 => (TOther)(object)value,
			Int512 => (TOther)(object)(Int512)value,
			nint => (TOther)(object)(nint)value,
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}
	
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
			return (long)value._p0;
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
			return new Int128(value._p1, value._p0);
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
	public static implicit operator Int512(in Int256 value)
	{
		long v = unchecked((long)value._p3);
		ulong lowerShifted = unchecked((ulong)(v >> 63));
		return new Int512(
			lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			value._p3, value._p2, value._p1, value._p0
			);
	}

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
	public static explicit operator UInt256(in Int256 value)
	{
		return Unsafe.BitCast<Int256, UInt256>(value);
	}

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
	public static explicit operator UInt512(in Int256 value)
	{
		ulong shiftedPart = unchecked((ulong)((long)value._p3 >> 63));
		return new UInt512(shiftedPart, shiftedPart, shiftedPart, shiftedPart, value._p3, value._p2, value._p1, value._p0);
	}

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
		BinaryOperations.WriteInt256LittleEndian(span, in value);
		return new BigInteger(span, (long)value._p3 >= 0);
	}

	/// <summary>
	/// Explicitly converts a <see cref="Int256" /> value to a <see cref="decimal"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator decimal(in Int256 value)
	{
		if ((long)value._p3 < 0)
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
		if ((long)value._p3 < 0)
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
		if ((long)value._p3 < 0)
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
		if ((long)value._p3 < 0)
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
		if ((long)value._p3 < 0)
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
		if ((long)value._p3 < 0)
		{
			Int256 abs = -value;
			return -(Half)(UInt256)(abs);
		}
		return (Half)(UInt256)(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Int256" /> value to a <see cref="NFloat"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(in Int256 value)
	{
		if ((long)value._p3 < 0)
		{
			Int256 abs = -value;
			return -(NFloat)(UInt256)(abs);
		}
		return (NFloat)(UInt256)(value);
	}
	
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
		ulong lowerShifted = (ulong)((long)value.Upper >> 63);
		return new(lowerShifted, lowerShifted, value.Upper, value.Lower);
	}

	/// <summary>
	/// Explicitly converts a <see cref="byte" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(byte value) => new Int256(0, 0, 0, value);
	/// <summary>
	/// Explicitly converts a <see cref="ushort" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(ushort value) => new Int256(0, 0, 0, value);
	/// <summary>
	/// Explicitly converts a <see cref="uint" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(uint value) => new Int256(0, 0, 0, value);
	/// <summary>
	/// Explicitly converts a <see cref="nuint" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(nuint value) => new Int256(0, 0, 0, value);
	/// <summary>
	/// Explicitly converts a <see cref="ulong" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(ulong value) => new Int256(0, 0, 0, value);
	/// <summary>
	/// Explicitly converts a <see cref="UInt128" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(UInt128 value)
	{
		return new Int256(0, 0, value.Upper, value.Lower);
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

		if (bytesWritten >= Size)
		{
			return BinaryOperations.ReadInt256LittleEndian(span);
		}

		BitHelper.TryReadLittleEndian(span[..bytesWritten], isUnsigned, out Int256 result);
		
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

		if (!BitHelper.TryReadLittleEndian(span[..bytesWritten], isUnsigned, out Int256 result))
		{
			Thrower.IntegerOverflow();
		}
		
		return result;
	}
	
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
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(NFloat value)
	{
		return NFloat.Size == 8 ? (Int256)(double)value : (Int256)(float)value;
	}
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
	public static explicit operator checked Int256(NFloat value)
	{
		return NFloat.Size == 8 ? checked((Int256)(double)value) : checked((Int256)(float)value);
	}
}