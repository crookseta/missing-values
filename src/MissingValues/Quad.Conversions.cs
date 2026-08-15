using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Quad
{
	/// <inheritdoc/>
	public static Quad CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Quad result;
		if (value is Quad v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToChecked<Quad>(value, out result))
		{
			Thrower.NotSupported<Quad, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Quad CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Quad result;

		if (value is Quad v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToSaturating<Quad>(value, out result))
		{
			Thrower.NotSupported<Quad, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Quad CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Quad result;

		if (value is Quad v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToTruncating<Quad>(value, out result))
		{
			Thrower.NotSupported<Quad, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<Quad>.TryConvertFromChecked<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

	static bool INumberBase<Quad>.TryConvertFromSaturating<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

	static bool INumberBase<Quad>.TryConvertFromTruncating<TOther>(TOther value, out Quad result) => TryConvertFrom(value, out result);

	private static bool TryConvertFrom<TOther>(TOther value, out Quad result)
	{
		bool converted = true;

		result = value switch
		{
			Half actual => (Quad)actual,
			float actual => (Quad)actual,
			double actual => (Quad)actual,
			Quad actual => actual,
			decimal actual => (Quad)actual,
			byte actual => (Quad)actual,
			ushort actual => (Quad)actual,
			uint actual => (Quad)actual,
			ulong actual => (Quad)actual,
			UInt128 actual => (Quad)actual,
			UInt256 actual => (Quad)actual,
			UInt512 actual => (Quad)actual,
			nuint actual => (Quad)actual,
			sbyte actual => (Quad)actual,
			short actual => (Quad)actual,
			int actual => (Quad)actual,
			long actual => (Quad)actual,
			Int128 actual => (Quad)actual,
			Int256 actual => (Quad)actual,
			Int512 actual => (Quad)actual,
			nint actual => (Quad)actual,
			BigInteger actual => (Quad)actual,
			_ => BitHelper.DefaultConvert<Quad>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Quad>.TryConvertToChecked<TOther>(Quad value, out TOther result)
	{
		bool converted = true;
		result = default;
		checked
		{
			result = result switch
			{
				Half => (TOther)(object)(Half)value,
				float => (TOther)(object)(float)value,
				double => (TOther)(object)(double)value,
				Quad => (TOther)(object)value,
				Octo => (TOther)(object)(Octo)value,
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
				Int512 => (TOther)(object)(Int512)value,
				BigInteger => (TOther)(object)(BigInteger)value,
				nint => (TOther)(object)(nint)value,
				_ => BitHelper.DefaultConvert<TOther>(out converted)
			};
		}

		return converted;
	}

	static bool INumberBase<Quad>.TryConvertToSaturating<TOther>(Quad value, out TOther result) => TryConvertTo(value, out result);

	static bool INumberBase<Quad>.TryConvertToTruncating<TOther>(Quad value, out TOther result) => TryConvertTo(value, out result);

	private static bool TryConvertTo<TOther>(Quad value, out TOther result)
	{
		bool converted = true;
		result = default;

		result = result switch
		{
			Half => (TOther)(object)(Half)value,
			float => (TOther)(object)(float)value,
			double => (TOther)(object)(double)value,
			Quad => (TOther)(object)value,
			Octo => (TOther)(object)(Octo)value,
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)((value >= byte.MaxValue) ? byte.MaxValue : (value <= Quad.Zero) ? byte.MinValue : (byte)value),
			ushort => (TOther)(object)((value >= ushort.MaxValue) ? ushort.MaxValue : (value <= Quad.Zero) ? ushort.MinValue : (ushort)value),
			uint => (TOther)(object)((value >= uint.MaxValue) ? uint.MaxValue : (value <= Quad.Zero) ? uint.MinValue : (uint)value),
			ulong => (TOther)(object)((value >= ulong.MaxValue) ? ulong.MaxValue : (value <= Quad.Zero) ? ulong.MinValue : (ulong)value),
			UInt128 => (TOther)(object)((value >= new Quad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt128.MaxValue : (value <= Quad.Zero) ? UInt128.MinValue : (UInt128)value),
			UInt256 => (TOther)(object)((value >= new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt256.MaxValue : (value <= Quad.Zero) ? UInt256.MinValue : (UInt256)value),
			UInt512 => (TOther)(object)((value >= new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue : (value <= Quad.Zero) ? UInt512.MinValue : (UInt512)value),
			nuint => (TOther)(object)((value >= nuint.MaxValue) ? nuint.MaxValue : (value <= nuint.MinValue) ? nuint.MinValue : (nuint)value),
			sbyte => (TOther)(object)((value >= sbyte.MaxValue) ? sbyte.MaxValue : (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
			short => (TOther)(object)((value >= short.MaxValue) ? short.MaxValue : (value <= short.MinValue) ? short.MinValue : (short)value),
			int => (TOther)(object)((value >= int.MaxValue) ? int.MaxValue : (value <= int.MinValue) ? int.MinValue : (int)value),
			long => (TOther)(object)((value >= long.MaxValue) ? long.MaxValue : (value <= long.MinValue) ? long.MinValue : (long)value),
			Int128 => (TOther)(object)((value >= new Quad(0x407E_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int128.MaxValue : (value <= new Quad(0xC07E_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int128.MinValue : (Int128)value),
			Int256 => (TOther)(object)((value >= new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MaxValue : (value <= new Quad(0xC0FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MinValue : (Int256)value),
			Int512 => (TOther)(object)((value >= new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MaxValue : (value <= new Quad(0xC1FE_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MinValue : (Int512)value),
			nint => (TOther)(object)((value >= nint.MaxValue) ? nint.MaxValue : (value <= nint.MinValue) ? nint.MinValue : (nint)value),
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}
	
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator byte(Quad value)
	{
		Quad twoPow8 = new Quad(0x4007_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return byte.MinValue;
		}
		if (value >= twoPow8)
		{
			return byte.MaxValue;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			byte result = (byte)((uint)(bits >> 105) | 0x80);

			result >>= (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112));
			return result;
		}
		else
		{
			return byte.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
	public static explicit operator checked byte(Quad value)
	{
		Quad twoPow8 = new Quad(0x4007_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || value >= twoPow8)
		{
			Thrower.IntegerOverflow();
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			byte result = (byte)((uint)(bits >> 105) | 0x80);

			result >>= (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112));
			return result;
		}
		else
		{
			return byte.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ushort(Quad value)
	{
		Quad twoPow16 = new Quad(0x400F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return ushort.MinValue;
		}
		if (value >= twoPow16)
		{
			return ushort.MaxValue;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			ushort result = (ushort)((uint)(bits >> 97) | 0x8000);

			result >>= Quad.ExponentBias + 16 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return ushort.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
	public static explicit operator checked ushort(Quad value)
	{
		Quad twoPow16 = new Quad(0x400F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || value >= twoPow16)
		{
			Thrower.IntegerOverflow();
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			ushort result = (ushort)((uint)(bits >> 97) | 0x8000);

			result >>= Quad.ExponentBias + 16 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return ushort.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator uint(Quad value)
	{
		Quad twoPow32 = new Quad(0x401F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return uint.MinValue;
		}
		if (value >= twoPow32)
		{
			return uint.MaxValue;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			uint result = (uint)(bits >> 81) | 0x8000_0000;

			result >>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return uint.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
	public static explicit operator checked uint(Quad value)
	{
		Quad twoPow32 = new Quad(0x401F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || value >= twoPow32)
		{
			Thrower.IntegerOverflow();
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			uint result = (uint)(bits >> 81) | 0x8000_0000;

			result >>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return uint.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ulong(Quad value)
	{
		Quad twoPow64 = new Quad(0x403F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return ulong.MinValue;
		}
		if (value >= twoPow64)
		{
			return ulong.MaxValue;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			ulong result = (ulong)(bits >> 49) | 0x8000_0000_0000_0000;

			result >>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return ulong.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
	public static explicit operator checked ulong(Quad value)
	{
		Quad twoPow64 = new Quad(0x403F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || value >= twoPow64)
		{
			Thrower.IntegerOverflow();
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			ulong result = (ulong)(bits >> 49) | 0x8000_0000_0000_0000;

			result >>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return ulong.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt128(Quad value)
	{
		Quad twoPow128 = new Quad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return UInt128.MinValue;
		}
		if (value >= twoPow128)
		{
			return UInt128.MaxValue;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt128 result = (bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0);

			result >>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt128.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
	public static explicit operator checked UInt128(Quad value)
	{
		Quad twoPow128 = new Quad(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || (value >= twoPow128))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt128 result = (bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0);

			result >>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt128.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(Quad value)
	{
		Quad twoPow256 = new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return UInt256.MinValue;
		}
		else if ((value >= twoPow256))
		{
			return UInt256.MaxValue;
		}

		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt256 result = new UInt256((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero);

			result >>= Quad.ExponentBias + 256 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt256.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(Quad value)
	{
		Quad twoPow256 = new Quad(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || (value >= twoPow256))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt256 result = new UInt256((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero);

			result >>= Quad.ExponentBias + 256 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt256.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(Quad value)
	{
		Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative)
		{
			return UInt512.MinValue;
		}
		else if ((value >= twoPow512))
		{
			return UInt512.MaxValue;
		}

		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt512 result = new UInt512((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero, UInt128.Zero, UInt128.Zero);

			result >>= Quad.ExponentBias + 512 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt512.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt512(Quad value)
	{
		Quad twoPow512 = new Quad(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Quad.IsNegative(value);

		if (Quad.IsNaN(value) || isNegative || (value >= twoPow512))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			UInt512 result = new UInt512((bits << 16) >> 1 | new UInt128(0x8000_0000_0000_0000, 0x0), UInt128.Zero, UInt128.Zero, UInt128.Zero);

			result >>= Quad.ExponentBias + 512 - 1 - (int)(bits >> 112);
			return result;
		}
		else
		{
			return UInt512.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nuint(Quad value)
	{
		return nuint.Size == 8 ? (nuint)(ulong)value : (nuint)(uint)value;
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
	public static explicit operator checked nuint(Quad value)
	{
		return nuint.Size == 8 ? checked((nuint)(ulong)value) : checked((nuint)(uint)value);
	}

	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator sbyte(Quad value)
	{
		Quad minValue = new Quad(0xC006_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x4005_FC00_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return sbyte.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return sbyte.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			// For some reason, sbyte and short don't perform logical shifts correctly, so we have to perform the shifting with byte and ushort.
			sbyte result = (sbyte)(((byte)(bits >> 105) | 0x80) >>> (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112)));

			if (isNegative)
			{
				result = (sbyte)-result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
	public static explicit operator checked sbyte(Quad value)
	{
		Quad minValue = new Quad(0xC006_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x4005_FC00_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Quad.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			sbyte result = (sbyte)(((byte)(bits >> 105) | 0x80) >>> (Quad.ExponentBias + 8 - 1 - (int)(bits >> 112)));

			if (isNegative)
			{
				result = (sbyte)-result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator short(Quad value)
	{
		Quad minValue = new Quad(0xC00E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x400D_FFFC_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return short.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return short.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			// For some reason, sbyte and short dont perform logical shifts correctly, so we have to perform the shifting with byte and ushort.
			short result = (short)(((ushort)(bits >> 97) | 0x8000) >>> (Quad.ExponentBias + 16 - 1 - (int)(bits >> 112)));

			if (isNegative)
			{
				result = (short)-result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
	public static explicit operator checked short(Quad value)
	{
		Quad minValue = new Quad(0xC00E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x400D_FFFC_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Quad.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			short result = (short)(((ushort)(bits >> 97) | 0x8000) >>> (Quad.ExponentBias + 16 - 1 - (int)(bits >> 112)));

			if (isNegative)
			{
				result = (short)-result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator int(Quad value)
	{
		Quad minValue = new Quad(0xC01E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x401D_FFFF_FFFC_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return int.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return int.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			int result = (int)((uint)(bits >> 81) | 0x8000_0000);

			result >>>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
	public static explicit operator checked int(Quad value)
	{
		Quad minValue = new Quad(0xC01E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x401D_FFFF_FFFC_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Quad.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			int result = (int)((uint)(bits >> 81) | 0x8000_0000);

			result >>>= Quad.ExponentBias + 32 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator long(Quad value)
	{
		Quad minValue = new Quad(0xC03E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x403D_FFFF_FFFF_FFFF, 0xFFFC_0000_0000_0000);

		if (value <= minValue)
		{
			return long.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return long.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			long result = (long)((ulong)(bits >> 49) | 0x8000_0000_0000_0000);

			result >>>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return 0L;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
	public static explicit operator checked long(Quad value)
	{
		Quad minValue = new Quad(0xC03E_0000_0000_0000, 0x0000_0000_0000_0000);
		Quad maxValue = new Quad(0x403D_FFFF_FFFF_FFFF, 0xFFFC_0000_0000_0000);

		if (value < minValue || Quad.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			long result = (long)((ulong)(bits >> 49) | 0x8000_0000_0000_0000);

			result >>>= Quad.ExponentBias + 64 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return 0L;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int128(Quad value)
	{
		Quad twoPow127 = new Quad(0x407E_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow127)
		{
			return Int128.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return Int128.Zero;
		}
		else if (value >= +twoPow127)
		{
			return Int128.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int128 result = (Int128)(((bits << 16) >> 1) | Quad.SignMask);

			result >>>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return Int128.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
	public static explicit operator checked Int128(Quad value)
	{
		Quad twoPow127 = new Quad(0x407E_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow127 || Quad.IsNaN(value) || value >= +twoPow127)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}
		if (value >= Quad.One)
		{
			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int128 result = (Int128)(((bits << 16) >> 1) | Quad.SignMask);

			result >>>= Quad.ExponentBias + 128 - 1 - (int)(bits >> 112);

			if (isNegative)
			{
				result = -result;
			}
			return result;
		}
		else
		{
			return Int128.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(Quad value)
	{
		Quad twoPow255 = new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow255)
		{
			return Int256.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return Int256.Zero;
		}
		else if (value >= +twoPow255)
		{
			return Int256.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}

		if (value >= Quad.One)
		{
			// In order to convert from Quad to int256 we first need to extract the signficand,
			// including the implicit leading bit, as a full 256-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 256-bits.

			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int256 result = new Int256((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero);

			result >>>= (Quad.ExponentBias + 256 - 1 - (int)(bits >> 112));

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}
		else
		{
			return Int256.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
	public static explicit operator checked Int256(Quad value)
	{
		Quad twoPow255 = new Quad(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow255 || Quad.IsNaN(value) || value >= +twoPow255)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}

		if (value >= Quad.One)
		{
			// In order to convert from Quad to int256 we first need to extract the signficand,
			// including the implicit leading bit, as a full 256-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 256-bits.

			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int256 result = new Int256((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero);

			result >>>= (Quad.ExponentBias + 256 - 1 - (int)(bits >> 112));

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}
		else
		{
			return Int256.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int512(Quad value)
	{
		Quad twoPow511 = new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow511)
		{
			return Int512.MinValue;
		}
		else if (Quad.IsNaN(value))
		{
			return Int512.Zero;
		}
		else if (value >= +twoPow511)
		{
			return Int512.MaxValue;
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}

		if (value >= Quad.One)
		{
			// In order to convert from Quad to Int512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int512 result = new Int512((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero, UInt128.Zero, UInt128.Zero);

			result >>>= (Quad.ExponentBias + 512 - 1 - (int)(bits >> 112));

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}
		else
		{
			return Int512.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
	public static explicit operator checked Int512(Quad value)
	{
		Quad twoPow511 = new Quad(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow511 || Quad.IsNaN(value) || value >= +twoPow511)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Quad.IsNegative(value);

		if (isNegative)
		{
			value = -value;
		}

		if (value >= Quad.One)
		{
			// In order to convert from Quad to int512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			UInt128 bits = BinaryOperations.QuadToUInt128Bits(value);
			Int512 result = new Int512((bits << 16) >> 1 | Quad.SignMask, UInt128.Zero, UInt128.Zero, UInt128.Zero);

			result >>>= (Quad.ExponentBias + 512 - 1 - (int)(bits >> 112));

			if (isNegative)
			{
				result = -result;
			}

			return result;
		}
		else
		{
			return Int512.Zero;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nint(Quad value)
	{
		if (nint.Size == 8)
		{
			return (nint)(int)value;
		}
		else
		{
			return (nint)(long)value;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
	public static explicit operator checked nint(Quad value)
	{
		if (nint.Size == 8)
		{
			return checked((nint)(int)value);
		}
		else
		{
			return checked((nint)(long)value);
		}
	}

	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="BigInteger"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is not finite.</exception>
	public static explicit operator BigInteger(Quad value)
	{
		BitHelper.GetQuadParts(value, out int sign, out int exp, out var man, out bool isFinite);
		
		if (!isFinite)
		{
			Thrower.IntegerOverflow();
		}

		if (man == UInt128.Zero)
		{
			return BigInteger.Zero;
		}

		BigInteger result;
		if (exp >= 0)
		{
			(int byteShift, int bitShift) = Math.DivRem(exp, 8);
			int bytesNeeded = 16 + byteShift + (bitShift > 0 ? 1 : 0);
			
			byte[]? array = null;
			Span<byte> buffer = bytesNeeded >= Calculator.StackAllocThreshold
				? (array = ArrayPool<byte>.Shared.Rent(bytesNeeded)).AsSpan(0, bytesNeeded)
				: stackalloc byte[bytesNeeded];
			buffer.Clear();

			if (bitShift == 0)
			{
				BinaryPrimitives.WriteUInt128LittleEndian(buffer[byteShift..], man);
			}
			else
			{
				ulong low = (ulong)man;
				ulong high = (ulong)(man >> 64);

				ulong shiftedLow = low << bitShift;
				ulong shiftedHigh = (high << bitShift) | (low >> (64 - bitShift));
				ulong carry = high >> (64 - bitShift);

				BinaryPrimitives.WriteUInt128LittleEndian(buffer[byteShift..], new UInt128(shiftedHigh, shiftedLow));
				if (carry > 0)
				{
					buffer[byteShift + 16] = (byte)carry;
				}
			}
			
			result = new BigInteger(buffer, isUnsigned: true, isBigEndian: false);
			
			if (array is not null)
			{
				ArrayPool<byte>.Shared.Return(array);
			}
		}
		else
		{
			exp = -exp;
			
			if (exp >= 128)
			{
				return BigInteger.Zero;
			}
			
			result = (BigInteger)(man >> exp);
		}
		return sign < 0 ? -result : result;
	}

	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="decimal"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator decimal(Quad value)
	{
		return (decimal)(double)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="Quad" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(Quad value)
	{
		bool sign = IsNegative(value);
		int exp = value.BiasedExponent;
		UInt128 sig = value.TrailingSignificand;

		if (exp == MaxBiasedExponent)
		{
			if (sig != UInt128.Zero)
			{
				return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 124);
			}
			return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == UInt128.Zero)
			{
				return BinaryOperations.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF128Sig(sig);
			exp -= 1;
		}

		return new Octo(sign, (uint)(exp + 0x3_C000), (UInt256)sig << 124);
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="double"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator double(Quad value)
	{
		UInt128 quadInt = BinaryOperations.QuadToUInt128Bits(value);
		bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
		int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
		UInt128 sig = quadInt & Quad.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != 0) // NaN
			{
				return BitHelper.CreateDoubleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
			}
			return sign ? double.NegativeInfinity : double.PositiveInfinity;
		}

		sig <<= 14;
		ulong sigQuad = sig.Upper | ((ulong)sig != 0 ? 1UL : 0UL);

		if (((uint)exp | sigQuad) == 0)
		{
			return BitHelper.CreateDouble(sign, 0, 0);
		}

		exp -= 0x3C01;

		exp = exp < -0x1000 ? -0x1000 : exp;

		return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, (short)(exp), (sigQuad | 0x4000_0000_0000_0000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="float"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator float(Quad value)
	{
		UInt128 quadInt = BinaryOperations.QuadToUInt128Bits(value);
		bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
		int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
		UInt128 sig = quadInt & Quad.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != 0) // NaN
			{
				return BitHelper.CreateSingleNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
			}
			return sign ? float.NegativeInfinity : float.PositiveInfinity;
		}

		uint sigQuad = (uint)BitHelper.ShiftRightJam((sig.Upper | ((ulong)sig != 0 ? 1UL : 0UL)), 18);

		if (((uint)exp | sigQuad) == 0)
		{
			return BitHelper.CreateSingle(sign, 0, 0);
		}

		exp -= 0x3F81;

		exp = exp < -0x1000 ? -0x1000 : exp;

		return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, (short)(exp), (sigQuad | 0x4000_0000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="Half"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Half(Quad value)
	{
		UInt128 quadInt = BinaryOperations.QuadToUInt128Bits(value);
		bool sign = (quadInt & Quad.SignMask) >> Quad.SignShift != 0;
		int exp = (int)((quadInt & Quad.BiasedExponentMask) >> Quad.BiasedExponentShift);
		UInt128 sig = quadInt & Quad.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != 0) // NaN
			{
				return BitHelper.CreateHalfNaN(sign, (ulong)(sig >> 48)); // Shift the significand bits to the x end
			}
			return sign ? Half.NegativeInfinity : Half.PositiveInfinity;
		}

		ushort sigHalf = (ushort)BitHelper.ShiftRightJam(((sig.Upper) | ((ulong)sig != 0 ? 1UL : 0UL)), 34);

		if (((uint)exp | sigHalf) == 0)
		{
			return BitHelper.CreateHalf(sign, 0, 0);
		}

		exp -= 0x3FF1;

		exp = exp < -0x40 ? -0x40 : exp;

		return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, (short)(exp), (ushort)(sigHalf | 0x4000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Quad" /> value to a <see cref="NFloat"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(Quad value)
	{
		return NFloat.Size == 8 ? (NFloat)(double)value : (NFloat)(float)value;
	}
	
	/// <summary>
	/// Implicitly converts a <see cref="byte" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(byte value)
	{
		return (Quad)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="ushort" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(ushort value)
	{
		return (Quad)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="uint" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(uint value)
	{
		int shiftDist;
		ulong z = 0;

		if (value != 0)
		{
			shiftDist = BitOperations.LeadingZeroCount(value) + 17;
			z = BitHelper.PackToQuadUI64(false, 0x402E - shiftDist, (ulong)value << shiftDist);
		}

		return new Quad(z, 0);
	}
	/// <summary>
	/// Implicitly converts a <see cref="ulong" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(ulong value)
	{
		UInt128 sig;
		int shiftDist;

		if ((value) == 0)
		{
			return Quad.Zero;
		}
		else
		{
			shiftDist = BitOperations.LeadingZeroCount(value) + 49;
			if (shiftDist >= 64)
			{
				sig = new UInt128(value << (shiftDist - 64), 0);
			}
			else
			{
				sig = (UInt128)value << shiftDist;
			}
		}

		return new Quad(false, (ushort)(0x406F - shiftDist), sig);
	}
	/// <summary>
	/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(UInt128 value)
	{
		if (value == UInt128.Zero)
		{
			return Quad.Zero;
		}
		int shiftDist = (int)UInt128.LeadingZeroCount(value);
		UInt128 a = (value << shiftDist >> 15); // Significant bits, with bit 113 still intact
		UInt128 b = (value << shiftDist << 113); // Insignificant bits, only relevant for rounding.
		UInt128 m = a + ((b - (b >> 127 & (a == UInt128.Zero ? UInt128.One : UInt128.Zero))) >> 127); // Add one when we need to round up. Break ties to even.
		UInt128 e = (UInt128)(0x407D - shiftDist); // Exponent plus 16383, minus one, except for zero.
		return BinaryOperations.UInt128BitsToQuad((e << 112) + m);
	}
	/// <summary>
	/// Implicitly converts a <see cref="nuint" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(nuint value)
	{
		if (value == 0)
		{
			return Quad.Zero;
		}

		return nuint.Size == 8 ? (Quad)(ulong)value : (Quad)(uint)value;
	}

	/// <summary>
	/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(sbyte value)
	{
		if (sbyte.IsNegative(value))
		{
			value = (sbyte)-value;
			return -(Quad)(byte)value;
		}
		return (Quad)(byte)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="short" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(short value)
	{
		if (short.IsNegative(value))
		{
			value = (short)-value;
			return -(Quad)(ushort)value;
		}
		return (Quad)(ushort)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="int" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(int value)
	{
		if (int.IsNegative(value))
		{
			value = -value;
			return -(Quad)(uint)value;
		}
		return (Quad)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="long" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(long value)
	{
		if (long.IsNegative(value))
		{
			value = -value;
			return -(Quad)(ulong)value;
		}
		return (Quad)(ulong)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(Int128 value)
	{
		if (Int128.IsNegative(value))
		{
			value = -value;
			return -(Quad)(UInt128)value;
		}
		return (Quad)(UInt128)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="nint" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(nint value)
	{
		if (Int128.IsNegative(value))
		{
			value = -value;
			return -(Quad)(nuint)value;
		}
		return (Quad)(nuint)value;
	}

	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(BigInteger value)
	{
		int sign = value.Sign;
		if (sign == 0)
		{
			return Quad.Zero;
		}
		
		BigInteger magnitude = sign < 0 ? -value : value;
		
		if (magnitude.CompareTo(ulong.MaxValue) <= 0)
		{
			Quad result = (ulong)magnitude;
			return sign < 0 ? -result : result;
		}
		
		// The maximum exponent for quads is 16383, which corresponds to ulong bit length of 256.
		// All BigIntegers with bits[] longer than 512 evaluate to Quad.PositiveInfinity (or NegativeInfinity).
		if (magnitude.GetBitLength() > MaxExponent + 1)
		{
			return sign == 1 ? Quad.PositiveInfinity : Quad.NegativeInfinity;
		}

		int byteCount = magnitude.GetByteCount();
		byte[]? array = null;
		Span<byte> bits = byteCount >= Calculator.StackAllocThreshold 
			? (array = ArrayPool<byte>.Shared.Rent(byteCount)).AsSpan(0, byteCount) 
			: stackalloc byte[byteCount];
		
		bits.Clear();

		magnitude.TryWriteBytes(bits, out int bytesWritten);
		int ulongCount = (bytesWritten + 7) / 8;

		UInt128 h = BitHelper.ReadUInt64Chunk(bits, ulongCount - 1);
		UInt128 m = BitHelper.ReadUInt64Chunk(bits, ulongCount - 2);
		UInt128 l = BitHelper.ReadUInt64Chunk(bits, ulongCount - 3);

		int z = BitOperations.LeadingZeroCount((ulong)h);
		int exp = (ulongCount - 2) * 64 - z;
		UInt128 man = (h << (64 + z)) | (m << z) | (l >> (64 - z));
	    
		if (array is not null)
		{
			ArrayPool<byte>.Shared.Return(array);
		}

		return BitHelper.GetQuadFromParts(sign, exp, man);
	}

	/// <summary>
	/// Explicitly converts a <see cref="decimal" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(decimal value)
	{
		return (Quad)(double)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="double" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(double value)
	{
		const int MaxBiasedExponentDouble = 0x07FF;
		const int DoubleExponentBias = 1023;

		ulong bits = BitConverter.DoubleToUInt64Bits(value);
		bool sign = double.IsNegative(value);
		int exp = (ushort)((bits >> 52) & MaxBiasedExponentDouble);
		ulong sig = bits & 0x000F_FFFF_FFFF_FFFF;

		if (exp == MaxBiasedExponentDouble)
		{
			if (sig != 0)
			{
				return CreateQuadNaN(sign, (UInt128)sig << 76);
			}
			return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF64Sig(sig);
			exp -= 1;
		}

		return new Quad(sign, (ushort)(exp + (ExponentBias - DoubleExponentBias)), (UInt128)sig << 60);
	}
	/// <summary>
	/// Implicitly converts a <see cref="float" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(float value)
	{
		const int MaxBiasedExponentSingle = 0xFF;
		const int SingleExponentBias = 127;

		uint bits = BitConverter.SingleToUInt32Bits(value);
		bool sign = float.IsNegative(value);
		int exp = (ushort)((bits >> 23) & MaxBiasedExponentSingle);
		uint sig = bits & 0x007F_FFFF;

		if (exp == MaxBiasedExponentSingle)
		{
			if (sig != 0)
			{
				return CreateQuadNaN(sign, (UInt128)sig << 105);
			}
			return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
			exp -= 1;
		}

		return new Quad(sign, (ushort)(exp + (ExponentBias - SingleExponentBias)), (UInt128)sig << 89);
	}
	/// <summary>
	/// Implicitly converts a <see cref="Half" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Quad(Half value)
	{
		const int MaxBiasedExponentHalf = 0x1F;
		const int HalfExponentBias = 15;

		ushort bits = BitConverter.HalfToUInt16Bits(value);
		bool sign = Half.IsNegative(value);
		int exp = (ushort)((bits >> 10) & MaxBiasedExponentHalf);
		uint sig = (uint)(bits & 0x03FF);

		if (exp == MaxBiasedExponentHalf)
		{
			if (sig != 0)
			{
				return CreateQuadNaN(sign, (UInt128)sig << 118);
			}
			return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt128BitsToQuad(sign ? SignMask : 0); // Positive / Negative zero
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
			exp -= 1;
		}

		return new Quad(sign, (ushort)(exp + (ExponentBias - HalfExponentBias)), (UInt128)sig << 102);
	}
	/// <summary>
	/// Implicitly converts a <see cref="NFloat" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(NFloat value)
	{
		return NFloat.Size == 8 ? (Quad)(double)value : (Quad)(float)value;
	}
}