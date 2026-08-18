using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct UInt256
{
	/// <inheritdoc/>
	public static UInt256 CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt256 result;

		if (value is UInt256 v)
		{
			result = v;
		}
		else if (!UInt256.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<UInt256>(value, out result))
		{
			Thrower.NotSupported<UInt256, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static UInt256 CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt256 result;

		if (value is UInt256 v)
		{
			result = v;
		}
		else if (!UInt256.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<UInt256>(value, out result))
		{
			Thrower.NotSupported<UInt256, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static UInt256 CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt256 result;

		if (value is UInt256 v)
		{
			result = v;
		}
		else if (!UInt256.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<UInt256>(value, out result))
		{
			Thrower.NotSupported<UInt256, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<UInt256>.TryConvertFromChecked<TOther>(TOther value, out UInt256 result) => TryConvertFromChecked(value, out result);
	private static bool TryConvertFromChecked<TOther>(TOther value, out UInt256 result)
		where TOther : INumberBase<TOther>
	{
		bool converted = true;
		checked
		{
			result = value switch
			{
				char actual => (UInt256)actual,
				NFloat actual => (UInt256)actual,
				Half actual => (UInt256)actual,
				float actual => (UInt256)actual,
				double actual => (UInt256)actual,
				decimal actual => (UInt256)actual,
				byte actual => (UInt256)actual,
				ushort actual => (UInt256)actual,
				uint actual => (UInt256)actual,
				ulong actual => (UInt256)actual,
				UInt128 actual => (UInt256)actual,
				UInt256 actual => actual,
				UInt512 actual => (UInt256)actual,
				nuint actual => (UInt256)actual,
				sbyte actual => (UInt256)actual,
				short actual => (UInt256)actual,
				int actual => (UInt256)actual,
				long actual => (UInt256)actual,
				Int128 actual => (UInt256)actual,
				Int256 actual => (UInt256)actual,
				Int512 actual => (UInt256)actual,
				nint actual => (UInt256)actual,
				BigInteger actual => (UInt256)actual,
				_ => BitHelper.DefaultConvert<UInt256>(out converted)
			};
		}
		return converted;
	}

	static bool INumberBase<UInt256>.TryConvertFromSaturating<TOther>(TOther value, out UInt256 result) => TryConvertFromSaturating(value, out result);
	private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt256 result)
		where TOther : INumberBase<TOther>
	{
		const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
#if TARGET_32BIT
			NFloat actual => (actual < 0) ? MinValue : (UInt256)actual,
#else
			NFloat actual => (actual < 0) ? MinValue : (actual > TwoPow256) ? MaxValue : (UInt256)actual,
#endif
			Half actual => (actual < Half.Zero) ? MinValue : (UInt256)actual,
			float actual => (actual < 0) ? MinValue : (UInt256)actual,
			double actual => (actual < 0) ? MinValue : (actual > TwoPow256) ? MaxValue : (UInt256)actual,
			decimal actual => (actual < 0) ? MinValue : (UInt128)actual,
			byte actual => actual,
			ushort actual => actual,
			uint actual => actual,
			ulong actual => actual,
			UInt128 actual => actual,
			UInt256 actual => actual,
			UInt512 actual => (actual > MaxValue) ? MaxValue : (UInt256)actual,
			nuint actual => actual,
			sbyte actual => (actual < 0) ? MinValue : (UInt256)actual,
			short actual => (actual < 0) ? MinValue : (UInt256)actual,
			int actual => (actual < 0) ? MinValue : (UInt256)actual,
			long actual => (actual < 0) ? MinValue : (UInt256)actual,
			Int128 actual => (actual < 0) ? MinValue : (UInt256)actual,
			Int256 actual => (actual < 0) ? MinValue : (UInt256)actual,
			Int512 actual => (actual < 0) ? MinValue : (actual > (Int512)MaxValue) ? MaxValue : (UInt256)actual,
			nint actual => (actual < 0) ? MinValue : (UInt256)actual,
			BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (UInt256)actual,
			_ => BitHelper.DefaultConvert<UInt256>(out converted)
		};
		return converted;
	}

	static bool INumberBase<UInt256>.TryConvertFromTruncating<TOther>(TOther value, out UInt256 result) => TryConvertFromTruncating(value, out result);
	private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt256 result)
		where TOther : INumberBase<TOther>
	{
		bool converted = true;
		result = value switch
		{
			char actual => actual,
			NFloat actual => (actual < 0) ? MinValue : (UInt256)actual,
			Half actual => (actual < Half.Zero) ? MinValue : (UInt256)actual,
			float actual => (actual < 0) ? MinValue : (UInt256)actual,
			double actual => (actual < 0) ? MinValue : (UInt256)actual,
			decimal actual => (actual < 0) ? MinValue : (UInt256)actual,
			byte actual => actual,
			ushort actual => actual,
			uint actual => actual,
			ulong actual => actual,
			UInt128 actual => actual,
			UInt256 actual => actual,
			UInt512 actual => (UInt256)actual,
			nuint actual => actual,
			sbyte actual => (UInt256)actual,
			short actual => (UInt256)actual,
			int actual => (UInt256)actual,
			long actual => (UInt256)actual,
			Int128 actual => (UInt256)actual,
			Int256 actual => (UInt256)actual,
			Int512 actual => (UInt256)actual,
			nint actual => (UInt256)actual,
			BigInteger actual => (UInt256)actual,
			_ => BitHelper.DefaultConvert<UInt256>(out converted)
		};
		return converted;
	}

	static bool INumberBase<UInt256>.TryConvertToChecked<TOther>(UInt256 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		checked
		{
			result = result switch
			{
				char => (TOther)(object)(char)value,
				NFloat => (TOther)(object)(NFloat)value,
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				decimal => (TOther)(object)(decimal)value,
				byte => (TOther)(object)(byte)value,
				ushort => (TOther)(object)(ushort)value,
				uint => (TOther)(object)(uint)value,
				ulong => (TOther)(object)(ulong)value,
				UInt128 => (TOther)(object)(UInt128)value,
				UInt256 => (TOther)(object)value,
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)(nuint)value,
				sbyte => (TOther)(object)(sbyte)value,
				short => (TOther)(object)(short)value,
				int => (TOther)(object)(int)value,
				long => (TOther)(object)(long)value,
				Int128 => (TOther)(object)(Int128)value,
				Int256 => (TOther)(object)(Int256)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)(nint)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<UInt256>.TryConvertToSaturating<TOther>(UInt256 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		result = result switch
		{
			char => (TOther)(object)(char)value,
			NFloat => (TOther)(object)(NFloat)value,
			Half => (TOther)(object)(Half)value,
			float => (TOther)(object)(float)value,
			double => (TOther)(object)(double)value,
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)((value >= 0xFF) ? byte.MaxValue : (byte)value),
			ushort => (TOther)(object)((value >= 0xFFFF) ? ushort.MaxValue : (ushort)value),
			uint => (TOther)(object)((value >= 0xFFFF_FFFF) ? uint.MaxValue : (uint)value),
			ulong => (TOther)(object)((value >= 0xFFFF_FFFF_FFFF_FFFF) ? ulong.MaxValue : (ulong)value),
			UInt128 => (TOther)(object)((value >= new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? UInt128.MaxValue : (UInt128)value),
			UInt256 => (TOther)(object)value,
			UInt512 => (TOther)(object)(UInt512)value,
#if TARGET_32BIT
			nuint => (TOther)(object)((value >= 0xFFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#else
			nuint => (TOther)(object)((value >= 0xFFFF_FFFF_FFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#endif
			sbyte => (TOther)(object)((value >= 0x7F) ? sbyte.MaxValue : (sbyte)value),
			short => (TOther)(object)((value >= 0x7FFF) ? short.MaxValue : (short)value),
			int => (TOther)(object)((value >= 0x7FFF_FFFF) ? int.MaxValue : (int)value),
			long => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? long.MaxValue : (long)value),
			Int128 => (TOther)(object)((value >= new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int128.MaxValue : (Int128)value),
			Int256 => (TOther)(object)((value >= new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int256.MaxValue : (Int256)value),
			Int512 => (TOther)(object)(Int512)value,
#if TARGET_32BIT
			nint => (TOther)(object)((value >= 0x7FFF_FFFF) ? nint.MaxValue : (nint)value),
#else
			nint => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? nint.MaxValue : (nint)value),
#endif
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}

	static bool INumberBase<UInt256>.TryConvertToTruncating<TOther>(UInt256 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		unchecked
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
				UInt256 => (TOther)(object)value,
				UInt512 => (TOther)(object)(UInt512)value,
				nuint => (TOther)(object)(nuint)value,
				sbyte => (TOther)(object)(sbyte)value,
				short => (TOther)(object)(short)value,
				int => (TOther)(object)(int)value,
				long => (TOther)(object)(long)value,
				Int128 => (TOther)(object)(Int128)value,
				Int256 => (TOther)(object)(Int256)value,
				Int512 => (TOther)(object)(Int512)value,
				nint => (TOther)(object)(nint)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
		}
		return converted;
	}
	
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="char"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator char(in UInt256 value) => (char)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="char"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="char"/>.</exception>
	public static explicit operator checked char(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((char)value._p0);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator byte(in UInt256 value) => (byte)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
	public static explicit operator checked byte(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((byte)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ushort(in UInt256 value) => (ushort)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
	public static explicit operator checked ushort(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((ushort)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator uint(in UInt256 value) => (uint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
	public static explicit operator checked uint(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((uint)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ulong(in UInt256 value) => value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
	public static explicit operator checked ulong(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return value._p0;
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt128(in UInt256 value) => value.Lower;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
	public static explicit operator checked UInt128(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return value.Lower;
	}
	/// <summary>
	/// Implicitly converts a <see cref="UInt256" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt512(in UInt256 value) => new(value);
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nuint(in UInt256 value) => (nuint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
	public static explicit operator checked nuint(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return (nuint)value._p0;
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator sbyte(in UInt256 value) => (sbyte)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
	public static explicit operator checked sbyte(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((sbyte)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator short(in UInt256 value) => (short)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
	public static explicit operator checked short(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((short)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator int(in UInt256 value) => (int)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
	public static explicit operator checked int(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((int)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator long(in UInt256 value) => (long)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
	public static explicit operator checked long(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((long)value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int128(in UInt256 value) => (Int128)value.Lower;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
	public static explicit operator checked Int128(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return (Int128)value.Lower;
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(in UInt256 value) => Unsafe.BitCast<UInt256, Int256>(value);
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
	public static explicit operator checked Int256(in UInt256 value)
	{
		if ((long)value._p3 < 0)
		{
			Thrower.IntegerOverflow();
		}
		return Unsafe.BitCast<UInt256, Int256>(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int512(in UInt256 value)
	{
		return new Int512(value);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
	public static explicit operator checked Int512(in UInt256 value)
	{
		return new Int512(0, 0, 0, 0, value._p3, value._p2, value._p1, value._p0);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nint(in UInt256 value) => (nint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
	public static explicit operator checked nint(in UInt256 value)
	{
		if (value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return (nint)value._p0;
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="BigInteger"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator BigInteger(in UInt256 value)
	{
		if (value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
		{
			return new BigInteger(value._p0);
		}
		Span<byte> span = stackalloc byte[Size];
		BinaryOperations.WriteUInt256LittleEndian(span, in value);
		return new BigInteger(span, true);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="decimal"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="decimal"/>.</exception>
	public static explicit operator decimal(in UInt256 value)
	{

		if (value.Upper != 0)
		{
			// The default behavior of decimal conversions is to always throw on overflow
			Thrower.IntegerOverflow();
		}

		return (decimal)value.Lower;
	}

#if NET11_0_OR_GREATER
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Decimal32"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Decimal32(in UInt256 value)
	{
		return BitHelper.ConvertToDecimalN<Decimal32, UInt256>(in value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Decimal64"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Decimal64(in UInt256 value)
	{
		return BitHelper.ConvertToDecimalN<Decimal64, UInt256>(in value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Decimal128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Decimal128(in UInt256 value)
	{
		return BitHelper.ConvertToDecimalN<Decimal128, UInt256>(in value);
	}
#endif
	
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Octo(in UInt256 value)
	{
		if (value == UInt256.Zero)
		{
			return Octo.Zero;
		}
		int shiftDist = BitHelper.LeadingZeroCount(in value);
		UInt256 a = (value << shiftDist >> 19); // Significant bits, with bit 237 still intact
		UInt256 b = (value << shiftDist << 237); // Insignificant bits, only relevant for rounding.
		UInt256 m = a + ((b - (b >> 255 & (a == UInt256.Zero ? UInt256.One : UInt256.Zero))) >> 255); // Add one when we need to round up. Break ties to even.
		UInt256 e = (UInt256)(0x400FD - shiftDist); // Exponent plus 262143, minus one, except for zero.
		return BinaryOperations.UInt256BitsToOcto((e << 236) + m);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(in UInt256 value)
	{
		if (value.Upper == 0)
		{
			return value._p1 != 0 ? (Quad)value.Lower : (Quad)value._p0;
		}
		else if ((value.Part3 == 0) && ((value.Part2 >> 32) == UInt128.Zero)) // value < (2^224)
		{
			// For values greater than MaxValue but less than 2^224 this takes advantage
			// that we can represent both "halves" of the uint256 within the 112-bit mantissa of
			// a pair of quads.
			Quad twoPow112 = new Quad(0x406F_0000_0000_0000, 0x0000_0000_0000_0000);
			Quad twoPow224 = new Quad(0x40DF_0000_0000_0000, 0x0000_0000_0000_0000);

			UInt128 twoPow112bits = BinaryOperations.QuadToUInt128Bits(twoPow112);
			UInt128 twoPow224bits = BinaryOperations.QuadToUInt128Bits(twoPow224);

			Quad lower = BinaryOperations.UInt128BitsToQuad(twoPow112bits | ((value.Lower << 16) >> 16)) - twoPow112;
			Quad upper = BinaryOperations.UInt128BitsToQuad(twoPow224bits | (UInt128)(value >> 112)) - twoPow224;

			return lower + upper;
		}
		else
		{
			// For values greater than 2^224 we basically do the same as before but we need to account
			// for the precision loss that quad will have. As such, the lower value effectively drops the
			// lowest 32 bits and then or's them back to ensure rounding stays correct.

			Quad twoPow144 = new Quad(0x408F_0000_0000_0000, 0x0000_0000_0000_0000);
			Quad twoPow256 = new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);

			UInt128 twoPow144bits = BinaryOperations.QuadToUInt128Bits(twoPow144);
			UInt128 twoPow256bits = BinaryOperations.QuadToUInt128Bits(twoPow256);

			Quad lower = BinaryOperations.UInt128BitsToQuad(twoPow144bits | ((UInt128)(value >> 16) >> 16) | (value.Part0 & 0xFFFF_FFFF)) - twoPow144;
			Quad upper = BinaryOperations.UInt128BitsToQuad(twoPow256bits | (UInt128)(value >> 144)) - twoPow256;

			return lower + upper;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="double"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator double(in UInt256 value)
	{
		const double TwoPow0 = 1.0d;
		const double TwoPow64 = 18446744073709551616.0d;
		const double TwoPow128 = 340282366920938463463374607431768211456.0d;
		const double TwoPow192 = 6277101735386680763835789423207666416102355444464034512896.0d;

		if (Vector256.IsHardwareAccelerated)
		{
			Vector256<double> vValue = Vector256.ConvertToDouble(Unsafe.BitCast<UInt256, Vector256<ulong>>(value));
			return Vector256.Sum(vValue * Vector256.Create(TwoPow0, TwoPow64, TwoPow128, TwoPow192));
		}

		if (value.Upper == 0)
		{
			return value._p1 != 0 ? (double)value.Lower : value._p0;
		}

		return value._p3 * TwoPow192
		       + value._p2 * TwoPow128
		       + value._p1 * TwoPow64
		       + value._p0 * TwoPow0;
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="float"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator float(in UInt256 value) => (float)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Half"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Half(in UInt256 value) => (Half)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="UInt256" /> value to a <see cref="Half"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(in UInt256 value)
	{
		return NFloat.Size == 8 ? (NFloat)(double)value : (NFloat)(float)value;
	}

	/// <summary>
	/// Implicitly converts a <see cref="char" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(char value) => new UInt256(0, 0, 0, value);

	/// <summary>
	/// Explicitly converts a <see cref="Half" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Half value) => (UInt256)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="Half" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Half value) => checked((UInt256)(double)value);
	/// <summary>
	/// Explicitly converts a <see cref="float" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(float value) => (UInt256)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="float" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(float value) => checked((UInt256)(double)value);
	/// <summary>
	/// Explicitly converts a <see cref="double" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(double value)
	{
		const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

		if (double.IsNegative(value) || double.IsNaN(value))
		{
			return MinValue;
		}
		else if (value >= TwoPow256)
		{
			return MaxValue;
		}

		return ToUInt256(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="double" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(double value)
	{
		const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0;

		// We need to convert -0.0 to 0 and not throw, so we compare
		// value against 0 rather than checking IsNegative

		if ((value < 0.0) || double.IsNaN(value) || (value >= TwoPow256))
		{
			Thrower.IntegerOverflow();
		}
		if (0.0 == TwoPow256 - value)
		{
			return MaxValue;
		}

		return ToUInt256(value);
	}
	
#if NET11_0_OR_GREATER
	/// <summary>
	/// Explicitly converts a <see cref="Decimal32" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Decimal32 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal32>(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Decimal32" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Decimal32 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal32>(value, true);
	}
	
	/// <summary>
	/// Explicitly converts a <see cref="Decimal64" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Decimal64 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal64>(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Decimal64" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Decimal64 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal64>(value, true);
	}
	
	/// <summary>
	/// Explicitly converts a <see cref="Decimal128" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Decimal128 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal128>(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Decimal128" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Decimal128 value)
	{
		return BitHelper.ConvertFromDecimalN<UInt256, Decimal128>(value, true);
	}
#endif
	
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(NFloat value)
	{
		if (NFloat.Size == 8)
		{
			return (UInt256)(double)value;
		}
		else
		{
			return (UInt256)(float)value;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt256(NFloat value)
	{
		if (NFloat.Size == 8)
		{
			return checked((UInt256)(double)value);
		}
		else
		{
			return checked((UInt256)(float)value);
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="decimal" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(decimal value) => (UInt256)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="decimal" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(decimal value) => checked((UInt256)(double)value);
	
	/// <summary>
	/// Implicitly converts a <see cref="byte" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(byte value) => new UInt256(0, 0, 0, value);
	/// <summary>
	/// Implicitly converts a <see cref="ushort" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(ushort value) => new UInt256(0, 0, 0, value);
	/// <summary>
	/// Implicitly converts a <see cref="uint" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(uint value) => new UInt256(0, 0, 0, value);
	/// <summary>
	/// Implicitly converts a <see cref="ulong" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(ulong value) => new UInt256(0, 0, 0, value);
	/// <summary>
	/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(UInt128 value)
	{
		return new UInt256(0, 0, value.Upper, value.Lower);
	}

	/// <summary>
	/// Implicitly converts a <see cref="nuint" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt256(nuint value) => new UInt256(0, 0, 0, value);
	// Signed
	/// <summary>
	/// Explicitly converts a <see cref="sbyte" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(sbyte value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new(lowerShifted, lowerShifted, lowerShifted, (ulong)(long)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="sbyte" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(sbyte value)
	{
		if (value < 0)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, 0, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="short" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(short value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new(lowerShifted, lowerShifted, lowerShifted, (ulong)(long)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="short" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(short value)
	{
		if (value < 0)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, 0, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="int" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(int value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new(lowerShifted, lowerShifted, lowerShifted, (ulong)(long)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="int" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(int value)
	{
		if (value < 0)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, 0, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="long" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(long value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new(lowerShifted, lowerShifted, lowerShifted, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="long" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(long value)
	{
		if (value < 0)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, 0, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Int128" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Int128 value)
	{
		ulong lowerShifted = (ulong)((long)value.Upper >> 63);
		return new(lowerShifted, lowerShifted, value.Upper, value.Lower);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Int128" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Int128 value)
	{
		if (value < Int128.Zero)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, value.Upper, value.Lower);
	}
	/// <summary>
	/// Explicitly converts a <see cref="nint" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(nint value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new(lowerShifted, lowerShifted, lowerShifted, (ulong)value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="nint" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(nint value)
	{
		if (value < 0)
		{
			Thrower.IntegerOverflow();
		}
		return new(0, 0, 0, (ulong)value);
	}

	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(BigInteger value)
	{
		Span<byte> span = stackalloc byte[value.GetByteCount()];
		value.TryWriteBytes(span, out int bytesWritten, true);

		if (bytesWritten >= Size)
		{
			return BinaryOperations.ReadUInt256LittleEndian(span);
		}

		UInt256 result = Zero;

		for (int i = 0; i < bytesWritten; i++)
		{
			UInt256 part = span[i];
			part <<= (i * 8);
			result |= part;
		}

		return result;
	}
	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(BigInteger value)
	{
		if (BigInteger.IsNegative(value))
		{
			Thrower.IntegerOverflow();
		}

		Span<byte> span = stackalloc byte[Size];

		if (!value.TryWriteBytes(span, out int bytesWritten, true))
		{
			Thrower.IntegerOverflow();
		}

		if (bytesWritten == Size)
		{
			return BinaryOperations.ReadUInt256LittleEndian(span);
		}
		else if (bytesWritten > Size)
		{
			Thrower.IntegerOverflow();
		}

		UInt256 result = Zero;

		for (int i = 0; i < bytesWritten; i++)
		{
			UInt256 part = span[i];
			part <<= (i * 8);
			result |= part;
		}

		return result;
	}
}