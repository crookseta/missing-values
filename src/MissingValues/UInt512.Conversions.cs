using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct UInt512
{
	/// <inheritdoc/>
	public static UInt512 CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt512 result;

		if (value is UInt512 v)
		{
			result = v;
		}
		else if (!UInt512.TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked<UInt512>(value, out result))
		{
			Thrower.NotSupported<UInt512, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static UInt512 CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt512 result;

		if (value is UInt512 v)
		{
			result = v;
		}
		else if (!UInt512.TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating<UInt512>(value, out result))
		{
			Thrower.NotSupported<UInt512, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static UInt512 CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		UInt512 result;

		if (value is UInt512 v)
		{
			result = v;
		}
		else if (!UInt512.TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating<UInt512>(value, out result))
		{
			Thrower.NotSupported<UInt512, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<UInt512>.TryConvertFromChecked<TOther>(TOther value, out UInt512 result) => TryConvertFromChecked(value, out result);
	private static bool TryConvertFromChecked<TOther>(TOther value, out UInt512 result)
	{
		bool converted = true;
		checked
		{
			result = value switch
			{
				char actual => (UInt512)actual,
				NFloat actual => (UInt512)actual,
				Half actual => (UInt512)actual,
				float actual => (UInt512)actual,
				double actual => (UInt512)actual,
				Quad actual => (UInt512)actual,
				decimal actual => (UInt512)actual,
				byte actual => (UInt512)actual,
				ushort actual => (UInt512)actual,
				uint actual => (UInt512)actual,
				ulong actual => (UInt512)actual,
				UInt128 actual => (UInt512)actual,
				UInt256 actual => (UInt512)actual,
				UInt512 actual => actual,
				nuint actual => (UInt512)actual,
				sbyte actual => (UInt512)actual,
				short actual => (UInt512)actual,
				int actual => (UInt512)actual,
				long actual => (UInt512)actual,
				Int128 actual => (UInt512)actual,
				Int256 actual => (UInt512)actual,
				Int512 actual => (UInt512)actual,
				BigInteger actual => (UInt512)actual,
				nint actual => (UInt512)actual,
				_ => BitHelper.DefaultConvert<UInt512>(out converted)
			};
		}
		return converted;
	}

	static bool INumberBase<UInt512>.TryConvertFromSaturating<TOther>(TOther value, out UInt512 result) => TryConvertFromSaturating(value, out result);
	private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt512 result)
	{
		const double TwoPow512 = 13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096.0;

		bool converted = true;
		result = value switch
		{
			char actual => actual,
#if TARGET_32BIT
			NFloat actual => (actual < 0) ? MinValue : (UInt512)actual,
#else
			NFloat actual => (actual < 0) ? MinValue : (actual > TwoPow512) ? MaxValue : (UInt512)actual,
#endif
			Half actual => (actual < Half.Zero) ? MinValue : (UInt512)actual,
			float actual => (actual < 0) ? MinValue : (UInt512)actual,
			double actual => (actual < 0) ? MinValue : (actual > TwoPow512) ? MaxValue : (UInt512)actual,
			Quad actual => (actual >= new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue : (actual <= Quad.Zero) ? UInt512.MinValue : (UInt512)actual,
			decimal actual => (actual < 0) ? MinValue : (UInt512)actual,
			byte actual => actual,
			ushort actual => actual,
			uint actual => actual,
			ulong actual => actual,
			UInt128 actual => actual,
			UInt256 actual => actual,
			UInt512 actual => actual,
			nuint actual => actual,
			sbyte actual => (actual < 0) ? MinValue : (UInt512)actual,
			short actual => (actual < 0) ? MinValue : (UInt512)actual,
			int actual => (actual < 0) ? MinValue : (UInt512)actual,
			long actual => (actual < 0) ? MinValue : (UInt512)actual,
			Int128 actual => (actual < 0) ? MinValue : (UInt512)actual,
			Int256 actual => (actual < 0) ? MinValue : (UInt512)actual,
			Int512 actual => (actual < 0) ? MinValue : (UInt512)actual,
			nint actual => (actual < 0) ? MinValue : (UInt512)actual,
			BigInteger actual => (BigInteger.IsNegative(actual)) ? MinValue : (actual > (BigInteger)MaxValue) ? MaxValue : (UInt512)actual,
			_ => BitHelper.DefaultConvert<UInt512>(out converted)
		};
		return converted;
	}

	static bool INumberBase<UInt512>.TryConvertFromTruncating<TOther>(TOther value, out UInt512 result) => TryConvertFromTruncating(value, out result);
	private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt512 result)
	{
		bool converted = true;
		unchecked
		{
			result = value switch
			{
				char actual => actual,
				Half actual => (actual < Half.Zero) ? MinValue : (UInt512)actual,
				float actual => (actual < 0) ? MinValue : (UInt512)actual,
				double actual => (actual < 0) ? MinValue : (UInt512)actual,
				decimal actual => (actual < 0) ? MinValue : (UInt512)actual,
				byte actual => actual,
				ushort actual => actual,
				uint actual => actual,
				ulong actual => actual,
				UInt128 actual => actual,
				UInt256 actual => actual,
				UInt512 actual => actual,
				nuint actual => actual,
				sbyte actual => (UInt512)actual,
				short actual => (UInt512)actual,
				int actual => (UInt512)actual,
				long actual => (UInt512)actual,
				Int128 actual => (UInt512)actual,
				Int256 actual => (UInt512)actual,
				Int512 actual => (UInt512)actual,
				nint actual => (UInt512)actual,
				BigInteger actual => (UInt512)actual,
				_ => BitHelper.DefaultConvert<UInt512>(out converted)
			};
		}
		return converted;
	}

	static bool INumberBase<UInt512>.TryConvertToChecked<TOther>(UInt512 value, out TOther result)
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
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)value,
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

	static bool INumberBase<UInt512>.TryConvertToSaturating<TOther>(UInt512 value, out TOther result)
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
			UInt256 => (TOther)(object)((value >= new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? UInt256.MaxValue : (UInt256)value),
			UInt512 => (TOther)(object)value,
#if TARGET_32BIT
			nuint => (TOther)(object)((value >= 0x0000_0000_FFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#else
			nuint => (TOther)(object)((value >= 0xFFFF_FFFF_FFFF_FFFF) ? nuint.MaxValue : (nuint)value),
#endif
			sbyte => (TOther)(object)((value >= 0x0000_0000_0000_007F) ? sbyte.MaxValue : (sbyte)value),
			short => (TOther)(object)((value >= 0x0000_0000_0000_7FFF) ? short.MaxValue : (short)value),
			int => (TOther)(object)((value >= 0x0000_0000_7FFF_FFFF) ? int.MaxValue : (int)value),
			long => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? long.MaxValue : (long)value),
			Int128 => (TOther)(object)((value >= new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int128.MaxValue : (Int128)value),
			Int256 => (TOther)(object)((value >= new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int256.MaxValue : (Int256)value),
			Int512 => (TOther)(object)((value >= new UInt512(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int512.MaxValue : (Int512)value),
#if TARGET_32BIT
			nint => (TOther)(object)((value >= 0x0000_0000_7FFF_FFFF) ? nint.MaxValue : (nint)value),
#else
			nint => (TOther)(object)((value >= 0x7FFF_FFFF_FFFF_FFFF) ? nint.MaxValue : (nint)value),
#endif
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}

	static bool INumberBase<UInt512>.TryConvertToTruncating<TOther>(UInt512 value, out TOther result)
	{
		bool converted = true;
		result = TOther.Zero;
		unchecked
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
				UInt256 => (TOther)(object)(UInt256)value,
				UInt512 => (TOther)(object)value,
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
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="char"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator char(in UInt512 value) => (char)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="char"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="char"/>.</exception>
	public static explicit operator checked char(in UInt512 value)
	{
		if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((char)value._p0);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator byte(in UInt512 value) => (byte)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
	public static explicit operator checked byte(in UInt512 value)
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
	public static explicit operator ushort(in UInt512 value) => (ushort)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
	public static explicit operator checked ushort(in UInt512 value)
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
	public static explicit operator uint(in UInt512 value) => (uint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
	public static explicit operator checked uint(in UInt512 value)
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
	public static explicit operator ulong(in UInt512 value) => value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
	public static explicit operator checked ulong(in UInt512 value)
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
	public static explicit operator UInt128(in UInt512 value) => new UInt128(value._p1, value._p0);
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
	public static explicit operator checked UInt128(in UInt512 value)
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
	public static explicit operator UInt256(in UInt512 value) => value.Lower;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(in UInt512 value)
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
	public static explicit operator nuint(in UInt512 value) => (nuint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
	public static explicit operator checked nuint(in UInt512 value)
	{
		if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((nuint)value.Lower);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator sbyte(in UInt512 value) => (sbyte)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
	public static explicit operator checked sbyte(in UInt512 value)
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
	public static explicit operator short(in UInt512 value) => (short)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
	public static explicit operator checked short(in UInt512 value)
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
	public static explicit operator int(in UInt512 value) => (int)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
	public static explicit operator checked int(in UInt512 value)
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
	public static explicit operator long(in UInt512 value) => (long)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
	public static explicit operator checked long(in UInt512 value)
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
	public static explicit operator Int128(in UInt512 value) => new Int128(value._p1, value._p0);
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
	public static explicit operator checked Int128(in UInt512 value)
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
	public static explicit operator Int256(in UInt512 value) => (Int256)value.Lower;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
	public static explicit operator checked Int256(in UInt512 value)
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
	public static explicit operator Int512(in UInt512 value) => Unsafe.BitCast<UInt512, Int512>(value);
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
	public static explicit operator checked Int512(in UInt512 value)
	{
		if ((long)value._p7 < 0)
		{
			Thrower.IntegerOverflow();
		}
		return Unsafe.BitCast<UInt512, Int512>(value);
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nint(in UInt512 value) => (nint)value._p0;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
	public static explicit operator checked nint(in UInt512 value)
	{
		if (value._p7 != 0 || value._p6 != 0 || value._p5 != 0 || value._p4 != 0 || value._p3 != 0 || value._p2 != 0 || value._p1 != 0)
		{
			Thrower.IntegerOverflow();
		}
		return checked((nint)value._p0);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="BigInteger"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator BigInteger(in UInt512 value)
	{
		if (value._p7 == 0 && value._p6 == 0 && value._p5 == 0 && value._p4 == 0 && value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
		{
			return new BigInteger(value._p0);
		}
		Span<byte> span = stackalloc byte[Size];
		BinaryOperations.WriteUInt512LittleEndian(span, in value);
		return new BigInteger(span, true);
	}

	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="decimal"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="decimal"/>.</exception>
	public static explicit operator decimal(in UInt512 value)
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
	public static explicit operator Octo(in UInt512 value)
	{
		if (value.Upper == 0)
		{
			return (value._p3 | value._p2 | value._p1) != 0 ? (Octo)value.Lower : value._p0;
		}
		else if ((value.Upper >> 32) == UInt128.Zero) // value < (2^472)
		{
			// For values greater than MaxValue but less than 2^472 this takes advantage
			// that we can represent both "halves" of the uint256 within the 236-bit mantissa of
			// a pair of octos.
			Octo twoPow236 = new Octo(0x400E_B000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			Octo twoPow472 = new Octo(0x401D_7000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			UInt256 twoPow236Bits = BinaryOperations.OctoToUInt256Bits(twoPow236);
			UInt256 twoPow472Bits = BinaryOperations.OctoToUInt256Bits(twoPow472);

			Octo lower = BinaryOperations.UInt256BitsToOcto(twoPow236Bits | ((value.Lower << 20) >> 20)) - twoPow236;
			Octo upper = BinaryOperations.UInt256BitsToOcto(twoPow472Bits | (UInt256)(value >> 236)) - twoPow472;

			return lower + upper;
		}
		else
		{
			// For values greater than 2^472 we basically do the same as before but we need to account
			// for the precision loss that octo will have. As such, the lower value effectively drops the
			// lowest 40 bits and then or's them back to ensure rounding stays correct.

			Octo twoPow276 = new Octo(0x4011_3000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			UInt256 twoPow276Bits = BinaryOperations.OctoToUInt256Bits(twoPow276);
			UInt256 twoPow512Bits = BinaryOperations.OctoToUInt256Bits(twoPow512);

			Octo lower = BinaryOperations.UInt256BitsToOcto(twoPow276Bits | ((UInt256)(value >> 20) >> 20) | (value._p0 & 0xFF_FFFF_FFFF)) - twoPow276;
			Octo upper = BinaryOperations.UInt256BitsToOcto(twoPow512Bits | (UInt256)(value >> 276)) - twoPow512;

			return lower + upper;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(in UInt512 value)
	{
		if (value.Upper == UInt256.Zero)
		{
			return (value._p3 | value._p2 | value._p1) != 0 ? (Quad)value.Lower : (Quad)value._p0;
		}
		else
		{
			// For values greater than 2^224 we basically do the same as before but we need to account
			// for the precision loss that quad will have. As such, the lower value effectively drops the
			// lowest 288 bits.
			Quad twoPow400 = new Quad(0x418F_0000_0000_0000, 0x0000_0000_0000_0000);
			Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);

			UInt128 twoPow400Bits = BinaryOperations.QuadToUInt128Bits(twoPow400);
			UInt128 twoPow512Bits = BinaryOperations.QuadToUInt128Bits(twoPow512);

			Quad lower = BinaryOperations.UInt128BitsToQuad(twoPow400Bits | (UInt128)((UInt256)(value >> 144) >> 144) | (value.Upper.Lower & 0xFFFF_FFFF)) - twoPow400;
			Quad upper = BinaryOperations.UInt128BitsToQuad(twoPow512Bits | (UInt128)(value >> 400)) - twoPow512;

			return lower + upper;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="double"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator double(in UInt512 value)
	{
		const double TwoPow0 = 1.0d;
		const double TwoPow64 = 18446744073709551616.0d;
		const double TwoPow128 = 340282366920938463463374607431768211456.0d;
		const double TwoPow192 = 6277101735386680763835789423207666416102355444464034512896.0d;
		const double TwoPow256 = 115792089237316195423570985008687907853269984665640564039457584007913129639936.0d;
		const double TwoPow320 = 2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576.0d;
		const double TwoPow384 = 39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816.0d;
		const double TwoPow448 = 726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656.0d;

		if (Vector512.IsHardwareAccelerated)
		{
			Vector512<double> vValue = Vector512.ConvertToDouble(Unsafe.BitCast<UInt512, Vector512<ulong>>(value));
			
			return Vector512.Sum(vValue * Vector512.Create(TwoPow0, TwoPow64, TwoPow128, TwoPow192, TwoPow256, TwoPow320, TwoPow384, TwoPow448));
		}
		if (Vector256.IsHardwareAccelerated)
		{
			Vector256<double> vUpper = Vector256.ConvertToDouble(Vector256.Create(value._p4, value._p5, value._p6, value._p7));
			Vector256<double> vLower = Vector256.ConvertToDouble(Vector256.Create(value._p0, value._p1, value._p2, value._p3));

			double upper = Vector256.Sum(vUpper * Vector256.Create(TwoPow256, TwoPow320, TwoPow384, TwoPow448));
			double lower = Vector256.Sum(vLower * Vector256.Create(TwoPow0, TwoPow64, TwoPow128, TwoPow192));
			
			return upper + lower;
		}

		if (value._p7 == 0 && value._p6 == 0 && value._p5 == 0 && value._p4 == 0)
		{
			if (value._p3 == 0 && value._p2 == 0 && value._p1 == 0)
			{
				return (double)value._p0;
			}
			return (double)value.Lower;
		}
		
		return value._p7 * TwoPow448
		       + value._p6 * TwoPow384
		       + value._p5 * TwoPow320
		       + value._p4 * TwoPow256
		       + value._p3 * TwoPow192
		       + value._p2 * TwoPow128
		       + value._p1 * TwoPow64
		       + value._p0 * TwoPow0;
	}
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="float"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator float(in UInt512 value) => (float)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="Half"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Half(in UInt512 value) => (Half)(double)value;
	/// <summary>
	/// Explicitly converts a <see cref="UInt512" /> value to a <see cref="NFloat"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(in UInt512 value)
	{
		return NFloat.Size == 8 ? (NFloat)(double)value : (NFloat)(float)value;
	}

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
	public static implicit operator UInt512(UInt128 value)
	{
		return new UInt512(
			0, 0, 0, 0,
			0, 0, value.Upper, value.Lower
			);
	}

	/// <summary>
	/// Implicitly converts a <see cref="nuint" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator UInt512(nuint value) => new UInt512(value);

	/// <summary>
	/// Explicitly converts a <see cref="sbyte" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(sbyte value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new UInt512(lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			lowerShifted, lowerShifted, lowerShifted, (ulong)value);
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
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new UInt512(lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			lowerShifted, lowerShifted, lowerShifted, (ulong)value);
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
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new UInt512(lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			lowerShifted, lowerShifted, lowerShifted, (ulong)value);
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
		ulong lowerShifted = (ulong)(value >> 63);
		return new UInt512(lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			lowerShifted, lowerShifted, lowerShifted, (ulong)value);
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
		ulong lowerShifted = (ulong)((long)value.Upper >> 63);
		return new(
			lowerShifted, lowerShifted, lowerShifted, lowerShifted,
			lowerShifted, lowerShifted, value.Upper, value.Lower
			);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Int128" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt512(Int128 value)
	{
		if (value < Int128.Zero)
		{
			Thrower.IntegerOverflow();
		}
		return new UInt512(
			0, 0, 0, 0,
			0, 0, value.Upper, value.Lower);
	}
	/// <summary>
	/// Explicitly converts a <see cref="nint" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(nint value)
	{
		ulong lowerShifted = (ulong)((long)value >> 63);
		return new UInt512(lowerShifted, lowerShifted, lowerShifted, lowerShifted, 
			lowerShifted, lowerShifted, lowerShifted, (ulong)value);
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

	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(BigInteger value)
	{
		Span<byte> span = stackalloc byte[value.GetByteCount()];
		value.TryWriteBytes(span, out int bytesWritten, true);

		if (bytesWritten >= Size)
		{
			return BinaryOperations.ReadUInt512LittleEndian(span);
		}

		UInt512 result = Zero;

		for (int i = 0; i < bytesWritten; i++)
		{
			UInt512 part = span[i];
			part <<= (i * 8);
			result |= part;
		}

		return result;
	}
	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt512(BigInteger value)
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
			return BinaryOperations.ReadUInt512LittleEndian(span);
		}
		else if (bytesWritten > Size)
		{
			Thrower.IntegerOverflow();
		}

		UInt512 result = Zero;

		for (int i = 0; i < bytesWritten; i++)
		{
			UInt512 part = span[i];
			part <<= (i * 8);
			result |= part;
		}

		return result;
	}

	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(NFloat value)
	{
		if (NFloat.Size == 8)
		{
			return (UInt512)(double)value;
		}
		else
		{
			return (UInt512)(float)value;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="NFloat" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt512(NFloat value)
	{
		if (NFloat.Size == 8)
		{
			return checked((UInt512)(double)value);
		}
		else
		{
			return checked((UInt512)(float)value);
		}
	}
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

		if ((value < 0.0) || double.IsNaN(value) || (value >= TwoPow512))
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
}