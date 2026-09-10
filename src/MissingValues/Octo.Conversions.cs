using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Internals;
using MissingValues.Primitives;

namespace MissingValues;

public partial struct Octo
{
	/// <inheritdoc/>
	public static Octo CreateChecked<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Octo result;
		if (value is Octo v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToChecked<Octo>(value, out result))
		{
			Thrower.NotSupported<Octo, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Octo CreateSaturating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Octo result;
		if (value is Octo v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToSaturating<Octo>(value, out result))
		{
			Thrower.NotSupported<Octo, TOther>();
		}

		return result;
	}

	/// <inheritdoc/>
	public static Octo CreateTruncating<TOther>(TOther value)
		where TOther : INumberBase<TOther>
	{
		Octo result;
		if (value is Octo v)
		{
			result = v;
		}
		else if (!TryConvertFrom(value, out result) && !TOther.TryConvertToTruncating<Octo>(value, out result))
		{
			Thrower.NotSupported<Octo, TOther>();
		}

		return result;
	}
	
	static bool INumberBase<Octo>.TryConvertFromChecked<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

	static bool INumberBase<Octo>.TryConvertFromSaturating<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

	static bool INumberBase<Octo>.TryConvertFromTruncating<TOther>(TOther value, out Octo result) => TryConvertFrom(value, out result);

	private static bool TryConvertFrom<TOther>(TOther value, out Octo result)
	{
		bool converted = true;

		result = value switch
		{
			Half actual => (Octo)actual,
			float actual => (Octo)actual,
			double actual => (Octo)actual,
			NFloat actual => (Octo)actual,
			Quad actual => (Octo)actual,
			Octo actual => actual,
			decimal actual => (Octo)actual,
			byte actual => (Octo)actual,
			ushort actual => (Octo)actual,
			uint actual => (Octo)actual,
			ulong actual => (Octo)actual,
			UInt128 actual => (Octo)actual,
			UInt256 actual => (Octo)actual,
			UInt512 actual => (Octo)actual,
			nuint actual => (Octo)actual,
			sbyte actual => (Octo)actual,
			short actual => (Octo)actual,
			int actual => (Octo)actual,
			long actual => (Octo)actual,
			Int128 actual => (Octo)actual,
			Int256 actual => (Octo)actual,
			Int512 actual => (Octo)actual,
			nint actual => (Octo)actual,
			BigInteger actual => (Octo)actual,
			_ => BitHelper.DefaultConvert<Octo>(out converted)
		};

		return converted;
	}

	static bool INumberBase<Octo>.TryConvertToChecked<TOther>(Octo value, out TOther result)
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
				NFloat => (TOther)(object)(NFloat)value,
				Quad => (TOther)(object)(Quad)value,
				Octo => (TOther)(object)value,
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

	static bool INumberBase<Octo>.TryConvertToSaturating<TOther>(Octo value, out TOther result) => TryConvertTo(value, out result);

	static bool INumberBase<Octo>.TryConvertToTruncating<TOther>(Octo value, out TOther result) => TryConvertTo(value, out result);

	private static bool TryConvertTo<TOther>(Octo value, out TOther result)
	{
		bool converted = true;
		result = default;

		result = result switch
		{
			Half => (TOther)(object)(Half)value,
			float => (TOther)(object)(float)value,
			double => (TOther)(object)(double)value,
			NFloat => (TOther)(object)(NFloat)value,
			Quad => (TOther)(object)(Quad)value,
			Octo => (TOther)(object)value,
			decimal => (TOther)(object)(decimal)value,
			byte => (TOther)(object)((value >= byte.MaxValue) ? byte.MaxValue : (value <= Octo.Zero) ? byte.MinValue : (byte)value),
			ushort => (TOther)(object)((value >= ushort.MaxValue) ? ushort.MaxValue : (value <= Octo.Zero) ? ushort.MinValue : (ushort)value),
			uint => (TOther)(object)((value >= uint.MaxValue) ? uint.MaxValue : (value <= Octo.Zero) ? uint.MinValue : (uint)value),
			ulong => (TOther)(object)((value >= ulong.MaxValue) ? ulong.MaxValue : (value <= Octo.Zero) ? ulong.MinValue : (ulong)value),
			UInt128 => (TOther)(object)((value >= UInt128.MaxValue) ? UInt128.MaxValue : (value <= Octo.Zero) ? UInt128.MinValue : (UInt128)value),
			UInt256 => (TOther)(object)((value >= new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt256.MaxValue
			: (value <= Quad.Zero) ? UInt256.MinValue : (UInt256)value),
			UInt512 => (TOther)(object)((value >= new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? UInt512.MaxValue
			: (value <= Quad.Zero) ? UInt512.MinValue : (UInt512)value),
			nuint => (TOther)(object)((value >= nuint.MaxValue) ? nuint.MaxValue : (value <= nuint.MinValue) ? nuint.MinValue : (nuint)value),
			sbyte => (TOther)(object)((value >= sbyte.MaxValue) ? sbyte.MaxValue : (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value),
			short => (TOther)(object)((value >= short.MaxValue) ? short.MaxValue : (value <= short.MinValue) ? short.MinValue : (short)value),
			int => (TOther)(object)((value >= int.MaxValue) ? int.MaxValue : (value <= int.MinValue) ? int.MinValue : (int)value),
			long => (TOther)(object)((value >= long.MaxValue) ? long.MaxValue : (value <= long.MinValue) ? long.MinValue : (long)value),
			Int128 => (TOther)(object)((value >= Int128.MaxValue) ? Int128.MaxValue : (value <= Int128.MinValue) ? Int128.MinValue : (Int128)value),
			Int256 => (TOther)(object)((value >= new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MaxValue
			: (value <= new Octo(0xC00F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int256.MinValue : (Int256)value),
			Int512 => (TOther)(object)((value >= new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MaxValue
			: (value <= new Octo(0xC01F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)) ? Int512.MinValue : (Int512)value),
			nint => (TOther)(object)((value >= nint.MaxValue) ? nint.MaxValue : (value <= nint.MinValue) ? nint.MinValue : (nint)value),
			BigInteger => (TOther)(object)(BigInteger)value,
			_ => BitHelper.DefaultConvert<TOther>(out converted)
		};

		return converted;
	}
	
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator byte(in Octo value)
	{
		Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return byte.MinValue;
		}
		if ((value >= twoPow8))
		{
			return byte.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			byte result = (byte)((byte)(bits.Part3 >> 37) | 0x80);

			result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return byte.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="byte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
	public static explicit operator checked byte(in Octo value)
	{
		Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow8))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			byte result = (byte)((byte)(bits.Part3 >> 37) | 0x80);

			result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return byte.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ushort(in Octo value)
	{
		Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return ushort.MinValue;
		}
		if ((value >= twoPow16))
		{
			return ushort.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			ushort result = (ushort)((ushort)(bits.Part3 >> 29) | 0x8000);

			result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return ushort.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ushort"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
	public static explicit operator checked ushort(in Octo value)
	{
		Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow16))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			ushort result = (ushort)((ushort)(bits.Part3 >> 29) | 0x8000);

			result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return ushort.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator uint(in Octo value)
	{
		Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return uint.MinValue;
		}
		if ((value >= twoPow32))
		{
			return uint.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			uint result = ((uint)(bits.Part3 >> 13) | 0x8000_0000);

			result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return uint.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="uint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
	public static explicit operator checked uint(in Octo value)
	{
		Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow32))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			uint result = ((uint)(bits.Part3 >> 13) | 0x8000_0000);

			result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return uint.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator ulong(in Octo value)
	{
		Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return ulong.MinValue;
		}
		if ((value >= twoPow64))
		{
			return ulong.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			ulong result = ((ulong)(bits.Upper >> 45) | 0x8000_0000_0000_0000);

			result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return ulong.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ulong"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
	public static explicit operator checked ulong(in Octo value)
	{
		Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow64))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			ulong result = ((ulong)(bits.Upper >> 45) | 0x8000_0000_0000_0000);

			result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return ulong.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt128(in Octo value)
	{
		Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return UInt128.MinValue;
		}
		if ((value >= twoPow128))
		{
			return UInt128.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

			result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return UInt128.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
	public static explicit operator checked UInt128(in Octo value)
	{
		Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow128))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

			result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return UInt128.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt256(in Octo value)
	{
		Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return UInt256.MinValue;
		}
		if ((value >= twoPow256))
		{
			return UInt256.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

			result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return UInt256.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
	public static explicit operator checked UInt256(in Octo value)
	{
		Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow256))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

			result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits.Part3 >> 44));
			return result;
		}
		else
		{
			return UInt256.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator UInt512(in Octo value)
	{
		Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative)
		{
			return UInt512.MinValue;
		}
		if ((value >= twoPow512))
		{
			return UInt512.MaxValue;
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

			result >>= Octo.ExponentBias + 512 - 1 - (int)(bits.Part3 >> 44);
			return result;
		}
		else
		{
			return UInt512.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
	public static explicit operator checked UInt512(in Octo value)
	{
		Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		bool isNegative = Octo.IsNegative(value);

		if (Octo.IsNaN(value) || isNegative || (value >= twoPow512))
		{
			Thrower.IntegerOverflow();
		}

		if (value >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(value);
			UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

			result >>= Octo.ExponentBias + 512 - 1 - (int)(bits.Part3 >> 44);
			return result;
		}
		else
		{
			return UInt512.MinValue;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nuint(in Octo value)
	{
		return nuint.Size == 8 ? (nuint)(ulong)value : (nuint)(uint)value;
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="nuint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nuint"/>.</exception>
	public static explicit operator checked nuint(in Octo value)
	{
		return nuint.Size == 8 ? checked((nuint)(ulong)value) : checked((nuint)(uint)value);
	}

	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator sbyte(in Octo value)
	{
		Octo minValue = new Octo(0xC000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4000_5FC0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		
		if (value <= minValue)
		{
			return sbyte.MinValue;
		}
		if (Octo.IsNaN(value))
		{
			return 0;
		}
		if (value >= maxValue)
		{
			return sbyte.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			sbyte result = (sbyte)(((byte)(bits.Part3 >> 37) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits.Part3 >> 44)));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="sbyte"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
	public static explicit operator checked sbyte(in Octo value)
	{
		Octo minValue = new Octo(0xC000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4000_5FC0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Octo.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			sbyte result = (sbyte)(((byte)(bits.Part3 >> 37) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits.Part3 >> 44)));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator short(in Octo value)
	{
		Octo minValue = new Octo(0xC000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4000_DFFF_C000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return short.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return short.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			short result = (short)(((ushort)(bits.Part3 >> 29) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits.Part3 >> 44)));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="short"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
	public static explicit operator checked short(in Octo value)
	{
		Octo minValue = new Octo(0xC000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4000_DFFF_C000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Octo.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			short result = (short)(((ushort)(bits.Part3 >> 29) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits.Part3 >> 44)));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator int(in Octo value)
	{
		Octo minValue = new Octo(0xC001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4001_DFFF_FFFF_C000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return int.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return int.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			int result = (int)((uint)(bits.Part3 >> 13) | 0x8000_0000);

			result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="int"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
	public static explicit operator checked int(in Octo value)
	{
		Octo minValue = new Octo(0xC001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4001_DFFF_FFFF_C000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Octo.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			int result = (int)((uint)(bits.Part3 >> 13) | 0x8000_0000);

			result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator long(in Octo value)
	{
		Octo minValue = new Octo(0xC003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4003_DFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return long.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return 0;
		}
		else if (value >= maxValue)
		{
			return long.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			long result = (long)((ulong)(bits.Upper >> 45) | 0x8000_0000_0000_0000);

			result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="long"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
	public static explicit operator checked long(in Octo value)
	{
		Octo minValue = new Octo(0xC003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4003_DFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Octo.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			long result = (long)((ulong)(bits.Upper >> 45) | 0x8000_0000_0000_0000);

			result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int128(in Octo value)
	{
		Octo minValue = new Octo(0xC007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4007_DFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= minValue)
		{
			return Int128.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return Int128.Zero;
		}
		else if (value >= maxValue)
		{
			return Int128.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

			result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int128"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
	public static explicit operator checked Int128(in Octo value)
	{
		Octo minValue = new Octo(0xC007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		Octo maxValue = new Octo(0x4007_DFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000);

		if (value < minValue || Octo.IsNaN(value) || value > maxValue)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

			result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int256(in Octo value)
	{
		Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow255)
		{
			return Int256.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return Int256.Zero;
		}
		else if (value >= +twoPow255)
		{
			return Int256.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

			result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int256"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
	public static explicit operator checked Int256(in Octo value)
	{
		Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow255 || Octo.IsNaN(value) || value >= +twoPow255)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

			result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Int512(in Octo value)
	{
		Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow511)
		{
			return Int512.MinValue;
		}
		else if (Octo.IsNaN(value))
		{
			return Int512.Zero;
		}
		else if (value >= +twoPow511)
		{
			return Int512.MaxValue;
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			// In order to convert from Quad to Int512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

			result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int512"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
	public static explicit operator checked Int512(in Octo value)
	{
		Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		if (value <= -twoPow511 || Octo.IsNaN(value) || value >= +twoPow511)
		{
			Thrower.IntegerOverflow();
		}

		bool isNegative = Octo.IsNegative(value);

		Octo abs = isNegative ? -value : value;

		if (abs >= Octo.One)
		{
			// In order to convert from Quad to Int512 we first need to extract the signficand,
			// including the implicit leading bit, as a full 512-bit significand. We can then adjust
			// this down to the represented integer by y shifting by the unbiased exponent, taking
			// into account the significand is now represented as 512-bits.

			UInt256 bits = BinaryOperations.OctoToUInt256Bits(abs);
			Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

			result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits.Part3 >> 44));

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
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator nint(in Octo value)
	{
		if (nint.Size == 8)
		{
			return (nint)(long)value;
		}
		else
		{
			return (nint)(int)value;
		}
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="nint"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="nint"/>.</exception>
	public static explicit operator checked nint(in Octo value)
	{
		if (nint.Size == 8)
		{
			return checked((nint)(long)value);
		}
		else
		{
			return checked((nint)(int)value);
		}
	}

	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="BigInteger"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <exception cref="OverflowException"><paramref name="value"/> is not finite.</exception>
	public static explicit operator BigInteger(in Octo value)
	{
		BitHelper.GetOctoParts(in value, out int sign, out int exp, out var man, out bool isFinite);

		if (!isFinite)
		{
			Thrower.IntegerOverflow();
		}

		if (man == UInt256.Zero)
		{
			return BigInteger.Zero;
		}

		BigInteger result;
		if (exp >= 0)
		{
			(int byteShift, int bitShift) = Math.DivRem(exp, 8);
			int bytesNeeded = 32 + byteShift + (bitShift > 0 ? 1 : 0);
			
			byte[]? array = null;
			Span<byte> buffer = bytesNeeded >= Calculator.StackAllocThreshold
				? (array = ArrayPool<byte>.Shared.Rent(bytesNeeded)).AsSpan(0, bytesNeeded)
				: stackalloc byte[bytesNeeded];
			buffer.Clear();

			if (bitShift == 0)
			{
				BinaryOperations.WriteUInt256LittleEndian(buffer[byteShift..], man);
			}
			else
			{
				UInt128 low = man.Lower;
				UInt128 high = man.Upper;

				UInt128 shiftedLow = low << bitShift;
				UInt128 shiftedHigh = (high << bitShift) | (low >> (128 - bitShift));
				UInt128 carry = high >> (128 - bitShift);

				BinaryOperations.WriteUInt256LittleEndian(buffer[byteShift..], new UInt256(shiftedHigh, shiftedLow));
				if (carry > 0)
				{
					buffer[byteShift + 32] = (byte)carry;
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
			
			if (exp >= 256)
			{
				return BigInteger.Zero;
			}
			
			result = (BigInteger)(man >> exp);
		}
		return sign < 0 ? -result : result;
	}

	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="decimal"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator decimal(in Octo value)
	{
		return (decimal)(double)value;
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Quad"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Quad(in Octo value)
	{
		UInt256 octoInt = BinaryOperations.OctoToUInt256Bits(value);
		bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
		int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
		UInt256 sig = octoInt & Octo.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != 0) // NaN
			{
				return BitHelper.CreateQuadNaN(sign, (UInt128)(sig >> 108)); // Shift the significand bits to the x end
			}
			return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
		}

		sig <<= 4;
		UInt128 sigOcto = sig.Upper | (sig.Lower != UInt128.Zero ? UInt128.One : UInt128.Zero);

		if (((uint)exp | sigOcto) == UInt128.Zero)
		{
			return new Quad(sign, 0, UInt128.Zero);
		}

		exp -= 0x3_C000;

		exp = exp < -0x1_0000 ? -0x1_0000 : exp;

		return BinaryOperations.UInt128BitsToQuad(BitHelper.PackToQuad(sign, exp, sigOcto));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="double"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator double(in Octo value)
	{
		UInt256 octoInt = BinaryOperations.OctoToUInt256Bits(value);
		bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
		int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
		UInt256 sig = octoInt & Octo.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != 0) // NaN
			{
				return BitHelper.CreateDoubleNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
			}
			return sign ? double.NegativeInfinity : double.PositiveInfinity;
		}

		sig <<= 18;
		ulong sigOcto = sig.Part3 | (sig.Part2 != 0 && sig.Part1 != 0 && sig.Part0 != 0 ? 1UL : 0UL);

		if (((uint)exp | sigOcto) == 0)
		{
			return BitHelper.CreateDouble(sign, 0, 0);
		}

		exp -= 0x3_FC01;

		exp = exp < -0x1000 ? -0x1000 : exp;

		return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, (short)(exp), (sigOcto | 0x4000_0000_0000_0000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="float"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator float(in Octo value)
	{
		UInt256 octoInt = BinaryOperations.OctoToUInt256Bits(value);
		bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
		int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
		UInt256 sig = octoInt & Octo.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != UInt256.Zero) // NaN
			{
				return BitHelper.CreateSingleNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
			}
			return sign ? float.NegativeInfinity : float.PositiveInfinity;
		}

		uint sigOcto = (uint)BitHelper.ShiftRightJam(sig.Part3 | ((uint)sig.Part3 != 0 && sig.Part2 != 0 && sig.Part1 != 0 && sig.Part0 != 0 ? 1U : 0U), 14);

		if (((uint)exp | sigOcto) == 0)
		{
			return BitHelper.CreateSingle(sign, 0, 0);
		}

		exp -= 0x3_FF81;

		exp = exp < -0x1000 ? -0x1000 : exp;

		return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, (short)(exp), (sigOcto | 0x4000_0000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Half"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Half(in Octo value)
	{
		UInt256 octoInt = BinaryOperations.OctoToUInt256Bits(value);
		bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
		int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
		UInt256 sig = octoInt & Octo.TrailingSignificandMask;

		if (exp == MaxBiasedExponent)
		{
			if (sig != UInt256.Zero) // NaN
			{
				return BitHelper.CreateHalfNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
			}
			return sign ? Half.NegativeInfinity : Half.PositiveInfinity;
		}

		ushort sigHalf = (ushort)BitHelper.ShiftRightJam(sig.Part3 | ((sig.Part2 | sig.Part1 | sig.Part0) != 0 ? 1UL : 0UL), 30);

		if (((uint)exp | sigHalf) == 0)
		{
			return BitHelper.CreateHalf(sign, 0, 0);
		}

		exp -= 0x3FFF1;

		exp = exp < -0x40 ? -0x40 : exp;

		return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, (short)exp, (ushort)(sigHalf | 0x4000)));
	}
	/// <summary>
	/// Explicitly converts a <see cref="Octo" /> value to a <see cref="NFloat"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator NFloat(in Octo value)
	{
		return NFloat.Size == 8 ? (NFloat)(double)value : (NFloat)(float)value;
	}
	
	/// <summary>
	/// Implicitly converts a <see cref="byte" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(byte value)
	{
		return (Octo)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="ushort" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(ushort value)
	{
		return (Octo)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="uint" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(uint value)
	{
		UInt256 sig;
		int shiftDist;

		if (value == UInt128.Zero)
		{
			return Octo.Zero;
		}
		else
		{
			shiftDist = BitOperations.LeadingZeroCount(value) + 205;
			sig = (UInt256)value << shiftDist;
		}

		return new Octo(false, (uint)(0x400EB - shiftDist), sig);
	}
	/// <summary>
	/// Implicitly converts a <see cref="ulong" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(ulong value)
	{
		UInt256 sig;
		int shiftDist;

		if (value == UInt128.Zero)
		{
			return Octo.Zero;
		}
		else
		{
			shiftDist = BitOperations.LeadingZeroCount(value) + 173;
			sig = (UInt256)value << shiftDist;
		}

		return new Octo(false, (uint)(0x400EB - shiftDist), sig);
	}
	/// <summary>
	/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(UInt128 value)
	{
		UInt256 sig;
		int shiftDist;

		if (value == UInt128.Zero)
		{
			return Octo.Zero;
		}
		else
		{
			shiftDist = ((int)UInt128.LeadingZeroCount(value)) + 109;
			sig = (UInt256)value << shiftDist;
		}

		return new Octo(false, (uint)(0x400EB - shiftDist), sig);
	}
	/// <summary>
	/// Implicitly converts a <see cref="nuint" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(nuint value)
	{
		if (value == 0)
		{
			return Octo.Zero;
		}

		return nuint.Size == 8 ? (Octo)(ulong)value : (Octo)(uint)value;
	}
	
	/// <summary>
	/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(sbyte value)
	{
		if (sbyte.IsNegative(value))
		{
			value = (sbyte)-value;
			return -(Octo)(byte)value;
		}
		return (Octo)(byte)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="short" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(short value)
	{
		if (short.IsNegative(value))
		{
			value = (short)-value;
			return -(Octo)(ushort)value;
		}
		return (Octo)(ushort)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="int" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(int value)
	{
		if (int.IsNegative(value))
		{
			value = -value;
			return -(Octo)(uint)value;
		}
		return (Octo)(uint)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="long" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(long value)
	{
		if (long.IsNegative(value))
		{
			value = -value;
			return -(Octo)(ulong)value;
		}
		return (Octo)(ulong)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(Int128 value)
	{
		if (Int128.IsNegative(value))
		{
			value = -value;
			return -(Octo)(UInt128)value;
		}
		return (Octo)(UInt128)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="nint" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(nint value)
	{
		if (Int128.IsNegative(value))
		{
			value = -value;
			return -(Octo)(nuint)value;
		}
		return (Octo)(nuint)value;
	}

	/// <summary>
	/// Explicitly converts a <see cref="BigInteger" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator Octo(BigInteger value)
	{
		int sign = value.Sign;
		if (sign == 0)
		{
			return Octo.Zero;
		}
		
		BigInteger magnitude = sign < 0 ? -value : value;
		
		if (magnitude.CompareTo(ulong.MaxValue) <= 0)
		{
			Octo result = (ulong)magnitude;
			return sign < 0 ? -result : result;
		}
		
		// The maximum exponent for octos is 262143, which corresponds to a UInt128 bit length of 2048.
		// All BigIntegers with bits[] longer than 8192 evaluate to Octo.PositiveInfinity (or NegativeInfinity).
		if (magnitude.GetBitLength() > MaxExponent + 1)
		{
			return sign == 1 ? Octo.PositiveInfinity : Octo.NegativeInfinity;
		}
		
		int byteCount = magnitude.GetByteCount();
		byte[]? array = null;
		Span<byte> bits = byteCount >= Calculator.StackAllocThreshold 
			? (array = ArrayPool<byte>.Shared.Rent(byteCount)).AsSpan(0, byteCount) 
			: stackalloc byte[byteCount];
		bits.Clear();

		magnitude.TryWriteBytes(bits, out int bytesWritten);
		int uintCount = (bytesWritten + 15) / 16;

		UInt256 h = BitHelper.ReadUInt128Chunk(bits, uintCount - 1);
		UInt256 m = BitHelper.ReadUInt128Chunk(bits, uintCount - 2);
		UInt256 l = BitHelper.ReadUInt128Chunk(bits, uintCount - 3);

		int z = (int)UInt128.LeadingZeroCount(h.Lower);
		int exp = (uintCount - 2) * 128 - z;
		UInt256 man = (h << 128 + z) | (m << z) | (l >> 128 - z);
		
		if (array is not null)
		{
			ArrayPool<byte>.Shared.Return(array);
		}

		return BitHelper.GetOctoFromParts(sign, exp, man);
	}

	/// <summary>
	/// Implicitly converts a <see cref="decimal" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(decimal value)
	{
		return (Octo)(double)value;
	}
	/// <summary>
	/// Implicitly converts a <see cref="double" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(double value)
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
				return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 204);
			}
			return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF64Sig(sig);
			exp -= 1;
		}

		return new Octo(sign, (uint)(exp + (ExponentBias - DoubleExponentBias)), (UInt256)sig << 184);
	}
	/// <summary>
	/// Implicitly converts a <see cref="float" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(float value)
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
				return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 233);
			}
			return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
			exp -= 1;
		}

		return new Octo(sign, (uint)(exp + (ExponentBias - SingleExponentBias)), (UInt256)sig << 213);
	}
	/// <summary>
	/// Implicitly converts a <see cref="Half" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(Half value)
	{
		const int MaxBiasedExponentHalf = 0x1F;
		const int HalfExponentBias = 15;

		ushort bits = BitConverter.HalfToUInt16Bits(value);
		bool sign = Half.IsNegative(value);
		int exp = (ushort)((bits >> 10) & MaxBiasedExponentHalf);
		ushort sig = (ushort)(bits & 0x03FF);

		if (exp == MaxBiasedExponentHalf)
		{
			if (sig != 0)
			{
				return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 246);
			}
			return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
		}

		if (exp == 0)
		{
			if (sig == 0)
			{
				return BinaryOperations.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
			}
			(exp, sig) = BitHelper.NormalizeSubnormalF16Sig(sig);
			exp -= 1;
		}

		return new Octo(sign, (uint)(exp + (ExponentBias - HalfExponentBias)), (UInt256)sig << 226);
	}
	/// <summary>
	/// Implicitly converts a <see cref="NFloat" /> value to a <see cref="Octo"/>.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static implicit operator Octo(NFloat value)
	{
		return NFloat.Size == 8 ? (Octo)(double)value : (Octo)(float)value;
	}
}