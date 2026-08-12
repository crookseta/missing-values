using MissingValues.Internals;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info;

internal interface IFormattableInteger<TSelf> : IFormattableNumber<TSelf>, IBigInteger<TSelf>, IMinMaxValue<TSelf>
		where TSelf : IFormattableInteger<TSelf>?
{
	/// <summary>
	/// Converts the specified hexadecimal character to a <typeparamref name="TSelf"/>.
	/// </summary>
	/// <param name="value">A hexadecimal character.</param>
	/// <returns>The hexadecimal value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
	static abstract TSelf GetHexValue(char value);

	static abstract int UnsignedCompare(in TSelf value1, in TSelf value2);

	static abstract int Log2Int32(in TSelf value);

	static abstract int LeadingZeroCountInt32(in TSelf value);

	static bool IFormattableNumber<TSelf>.IsBinaryInteger() => true;

	/// <summary>
	/// Gets the value <c>2</c> of the type.
	/// </summary>
	static abstract TSelf Two { get; }
	/// <summary>
	/// Gets the value <c>16</c> of the type.
	/// </summary>
	static abstract TSelf Sixteen { get; }
	/// <summary>
	/// Gets the value <c>10</c> of the type.
	/// </summary>
	static abstract TSelf Ten { get; }

	/// <summary>
	/// Gets the value <c>4</c> of the type.
	/// </summary>
	static abstract TSelf TwoPow2 { get; }
	/// <summary>
	/// Gets the value <c>256</c> of the type.
	/// </summary>
	static abstract TSelf SixteenPow2 { get; }
	/// <summary>
	/// Gets the value <c>100</c> of the type.
	/// </summary>
	static abstract TSelf TenPow2 { get; }

	/// <summary>
	/// Gets the value <c>8</c> of the type.
	/// </summary>
	static abstract TSelf TwoPow3 { get; }
	/// <summary>
	/// Gets the value <c>4096</c> of the type.
	/// </summary>
	static abstract TSelf SixteenPow3 { get; }
	/// <summary>
	/// Gets the value <c>1000</c> of the type.
	/// </summary>
	static abstract TSelf TenPow3 { get; }

	static virtual TSelf E19
	{
		get => TSelf.CreateTruncating(10000000000000000000UL);
	}

	/// <summary>
	/// Gets the left-most digit of the maximum value of <typeparamref name="TSelf"/>.
	/// </summary>
	static abstract char LastDecimalDigitOfMaxValue { get; }
	/// <summary>
	/// Gets the number of digits of the maximum decimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	static abstract int MaxDecimalDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum hexadecimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	static abstract int MaxHexDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum binary value of <typeparamref name="TSelf"/>.
	/// </summary>
	static abstract int MaxBinaryDigits { get; }
	static abstract bool IsUnsignedInteger { get; }
}

internal interface IFormattableSignedInteger<TSigned> : IFormattableInteger<TSigned>, ISignedNumber<TSigned>
	where TSigned : IFormattableSignedInteger<TSigned>
{
	static bool IFormattableInteger<TSigned>.IsUnsignedInteger => false;
}

internal interface IFormattableUnsignedInteger<TUnsigned> : IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
	where TUnsigned : IFormattableUnsignedInteger<TUnsigned>
{
	/// <summary>
	/// Gets the absolute representation of the maximum representable value of <typeparamref name="TSigned"/>(Abs(TSigned.MinValue)).
	/// </summary>
	static abstract TUnsigned SignedMaxMagnitude { get; }

	static abstract int CountDigits(in TUnsigned value);

	static abstract void ToDecChars<TChar>(in TUnsigned number, Span<TChar> destination, int digits) where TChar : unmanaged, IUtfCharacter<TChar>;

	static bool IFormattableInteger<TUnsigned>.IsUnsignedInteger => true;
}

internal static partial class NumberFormatter
{
	internal const ulong E19 = 10_000_000_000_000_000_000UL;
	internal const int E19Digits = 19;
	
	internal static int CountDigits(ulong value)
	{
		ReadOnlySpan<byte> log2ToPow10 =
		[
			1,  1,  1,  2,  2,  2,  3,  3,  3,  4,  4,  4,  4,  5,  5,  5,
			6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9,  10, 10, 10,
			10, 11, 11, 11, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 15, 15,
			15, 16, 16, 16, 16, 17, 17, 17, 18, 18, 18, 19, 19, 19, 19, 20
		];

		int index = log2ToPow10[BitOperations.Log2(value)];

		ReadOnlySpan<ulong> powersOf10 =
		[
			0, // unused entry to avoid needing to subtract
            0,
			10,
			100,
			1000,
			10000,
			100000,
			1000000,
			10000000,
			100000000,
			1000000000,
			10000000000,
			100000000000,
			1000000000000,
			10000000000000,
			100000000000000,
			1000000000000000,
			10000000000000000,
			100000000000000000,
			1000000000000000000,
			10000000000000000000,
		];

		ulong powerOf10 = powersOf10[index];

		// Return the number of digits based on the power of 10, shifted by 1
		// if it falls below the threshold.
		bool lessThan = value < powerOf10;
		return (index - Unsafe.As<bool, byte>(ref lessThan));
	}
	internal static int CountHexDigits<T>(in T value)
		where T : struct, IFormattableInteger<T>
	{
		return (T.Log2Int32(in value) >>> 2) + 1;
	}
	internal static int CountBinDigits<T>(in T value)
		where T : struct, IFormattableInteger<T>
	{
		return T.MaxBinaryDigits - T.LeadingZeroCountInt32(in value);
	}

	private static ref TChar UInt64ToDecChars<TChar>(ulong value, ref TChar bufferEnd, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		// Borrowed from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Number.Formatting.cs
		uint remainder;
		while (value >= 100)
		{
			bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 2);
			digits -= 2;
			(value, remainder) = Calculator.DivRemByUInt32(value, 100);
			WriteTwoDigits(remainder, ref bufferEnd);
		}
		while (value != 0 || digits > 0)
		{
			digits--;
			(value, remainder) = Calculator.DivRemByUInt32(value, 10);
			bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 1);
			bufferEnd = (TChar)(remainder + '0');
		}

		return ref bufferEnd;
	}

	private static void WriteTwoDigits<TChar>(uint value, ref TChar ptr)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		Unsafe.CopyBlockUnaligned(
			ref Unsafe.As<TChar, byte>(ref ptr),
			ref Unsafe.Add(ref MemoryMarshal.GetReference(TChar.TwoDigitsAsBytes), (uint)Unsafe.SizeOf<TChar>() * 2 * value),
			(uint)Unsafe.SizeOf<TChar>() * 2
			);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong UInt256DivMod1E19(ref UInt256 value)
	{
		Calculator.DivRem(in value, E19, out value, out ulong remainder);
		return remainder;
	}
	internal static void UInt256ToDecChars<TChar>(UInt256 value, Span<TChar> destination, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), digits);

		while (value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0)
		{
			bufferEnd = ref UInt64ToDecChars(UInt256DivMod1E19(ref value), ref bufferEnd, E19Digits);
			digits -= E19Digits;
		}
		UInt64ToDecChars(value.Part0, ref bufferEnd, digits);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong UInt512DivMod1E19(ref UInt512 value)
	{
		Calculator.DivRem(in value, E19, out value, out ulong remainder);
		return remainder;
	}
	internal static void UInt512ToDecChars<TChar>(UInt512 value, Span<TChar> destination, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), digits);

		while (value.Part7 != 0 || value.Part6 != 0 || value.Part5 != 0 || value.Part4 != 0 || value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0)
		{
			bufferEnd = ref UInt64ToDecChars(UInt512DivMod1E19(ref value), ref bufferEnd, E19Digits);
			digits -= E19Digits;
		}
		UInt64ToDecChars(value.Part0, ref bufferEnd, digits);
	}

	internal static void UnsignedIntegerToRadixChars<T, TChar, TConverter>(in T value, char isUpper, Span<TChar> destination, int digits)
		where T : unmanaged, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
		where TConverter : struct, IIntegerRadixConverter<T>
	{
		destination[..digits].Fill((TChar)'0');
		int hexBase = (isUpper - ('X' - 'A' + 10));
		Span<ulong> value64 = stackalloc ulong[Unsafe.SizeOf<T>() / sizeof(ulong)];
		value.TryCopyTo(value64);
		value64 = value64[..^(T.LeadingZeroCountInt32(in value) / 64)];
		
		ulong v;
		if (value64.Length == 1)
		{
			v = value64[0];
			
			while (--digits >= 0 || v != 0)
			{
				byte digit = (byte)(v & TConverter.MaxDigitValue);
				destination[digits] = (TChar)(char)(digit + (digit < 10 ? (byte)'0' : hexBase));
				v >>>= TConverter.BitsPerCharacter;
			}
			
			return;
		}

		for (int i = 0; i < value64.Length && digits > 0; i++)
		{
			int digitsLeft = Math.Min(digits, TConverter.MaxUInt64DigitCount);
			v = value64[i];

			while (digitsLeft > 0 || v != 0)
			{
				byte digit = (byte)(v & TConverter.MaxDigitValue);
				destination[--digits] = (TChar)(char)(digit + (digit < 10 ? (byte)'0' : hexBase));
				digitsLeft--;
				v >>>= TConverter.BitsPerCharacter;
			}
		}
	}

	internal static string FormatInt<TSigned, TUnsigned>(in TSigned value, string? format, IFormatProvider? provider)
		where TSigned : unmanaged, IFormattableSignedInteger<TSigned>
		where TUnsigned : unmanaged, IFormattableUnsignedInteger<TUnsigned>
	{
		if (string.IsNullOrEmpty(format))
		{
			if (value >= TSigned.Zero)
			{
				TUnsigned ui = TUnsigned.CreateTruncating(value);
				return string.Create(TUnsigned.CountDigits(in ui), ui, (destination, number) =>
				{
					TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			}
			else
			{
				TSigned abs = -value;
				TUnsigned ui = abs >= TSigned.Zero ? TUnsigned.CreateTruncating(abs) : TUnsigned.SignedMaxMagnitude;
				return NumberFormatInfo.GetInstance(provider).NegativeSign + string.Create(TUnsigned.CountDigits(in ui), ui, (destination, number) =>
				{
					TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			}
		}

		ReadOnlySpan<char> formatSpan = format;
		char fmt = GetFormat(formatSpan, out int precision);
		TUnsigned u;
		switch (fmt)
		{
			case 'b':
			case 'B':
				u = Unsafe.BitCast<TSigned, TUnsigned>(value);
				precision = int.Max(precision, CountBinDigits(in u));
				return string.Create(precision, (u, fmt), (destination, number) =>
				{
					UnsignedIntegerToRadixChars<TUnsigned, Utf16Char, BinConverter<TUnsigned>>(in number.u, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				u = Unsafe.BitCast<TSigned, TUnsigned>(value);
				precision = int.Max(precision, CountHexDigits(in u));
				return string.Create(precision, (u, fmt), (destination, number) =>
				{
					UnsignedIntegerToRadixChars<TUnsigned, Utf16Char, HexConverter<TUnsigned>>(in number.u, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'd':
			case 'D':
			case 'g':
			case 'G':
				bool isNegative = value < TSigned.Zero;
				if (!isNegative)
				{
					u = TUnsigned.CreateTruncating(value);
				}
				else
				{
					TSigned abs = -value;
					u = abs >= TSigned.Zero ? TUnsigned.CreateTruncating(abs) : TUnsigned.SignedMaxMagnitude;
				}
				precision = int.Max(precision, TUnsigned.CountDigits(in u));
				if (isNegative)
				{
					return NumberFormatInfo.GetInstance(provider).NegativeSign + string.Create(precision, u, (destination, number) =>
					{
						TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
					});
				}
				return string.Create(precision, u, (destination, number) =>
				{
					TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			default:
				return FormatNumber(in value, format!, NumberFormatInfo.GetInstance(provider));
		}
	}
	internal static bool TryFormatInt<TSigned, TUnsigned, TChar>(in TSigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where TSigned : unmanaged, IFormattableSignedInteger<TSigned>
		where TUnsigned : unmanaged, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		int digits;
		scoped Span<TChar> negativeSign;
		NumberFormatInfo info;

		if (format.IsEmpty)
		{
			if (value >= TSigned.Zero)
			{
				TUnsigned ui = TUnsigned.CreateTruncating(value);
				charsWritten = TUnsigned.CountDigits(in ui);
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				TUnsigned.ToDecChars(ui, destination, charsWritten);
				return true;
			}
			else
			{
				info = NumberFormatInfo.GetInstance(provider);
				negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
				TChar.Copy(info.NegativeSign, negativeSign);

				TSigned abs = -value;
				TUnsigned ui = abs >= TSigned.Zero ? TUnsigned.CreateTruncating(abs) : TUnsigned.SignedMaxMagnitude;
				digits = TUnsigned.CountDigits(in ui);
				charsWritten = digits + negativeSign.Length;
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				negativeSign.CopyTo(destination);
				TUnsigned.ToDecChars(ui, destination[negativeSign.Length..], digits);
				return true;
			}
		}

		char fmt = GetFormat(format, out int precision);
		TUnsigned u;
		switch (fmt)
		{
			case 'b':
			case 'B':
				u = Unsafe.BitCast<TSigned, TUnsigned>(value);
				charsWritten = int.Max(precision, CountBinDigits(in u));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToRadixChars<TUnsigned, TChar, BinConverter<TUnsigned>>(in u, fmt, destination, charsWritten);
				return true;
			case 'x':
			case 'X':
				u = Unsafe.BitCast<TSigned, TUnsigned>(value);
				charsWritten = int.Max(precision, CountHexDigits(in u));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToRadixChars<TUnsigned, TChar, HexConverter<TUnsigned>>(in u, fmt, destination, charsWritten);
				return true;
			case 'd':
			case 'D':
			case 'g':
			case 'G':
				bool isNegative = value < TSigned.Zero;
				if (!isNegative)
				{
					u = TUnsigned.CreateTruncating(value);
				}
				else
				{
					TSigned abs = -value;
					u = abs >= TSigned.Zero ? TUnsigned.CreateTruncating(abs)
						: TUnsigned.SignedMaxMagnitude;
				}
				precision = int.Max(precision, TUnsigned.CountDigits(in u));
				if (isNegative)
				{
					info = NumberFormatInfo.GetInstance(provider);
					negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
					TChar.Copy(info.NegativeSign, negativeSign);

					charsWritten = precision + negativeSign.Length;
					if (destination.Length < charsWritten)
					{
						charsWritten = 0;
						return false;
					}
					negativeSign.CopyTo(destination);
					TUnsigned.ToDecChars(u, destination[negativeSign.Length..], precision);
					return true;
				}
				charsWritten = precision;
				TUnsigned.ToDecChars(u, destination, precision);
				return true;
			default:
				return TryFormatNumber(in value, destination, out charsWritten, format, NumberFormatInfo.GetInstance(provider));
		}
	}
	internal static string FormatUInt<TUnsigned>(in TUnsigned value, string? format, IFormatProvider? provider)
		where TUnsigned : unmanaged, IFormattableUnsignedInteger<TUnsigned>
	{
		if (string.IsNullOrEmpty(format))
		{
			return string.Create(TUnsigned.CountDigits(in value), value, (destination, number) =>
			{
				TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
			});
		}

		ReadOnlySpan<char> formatSpan = format;
		char fmt = GetFormat(formatSpan, out int precision);
		switch (fmt)
		{
			case 'b':
			case 'B':
				precision = int.Max(precision, CountBinDigits(in value));
				return string.Create(precision, (value, fmt), (destination, number) =>
				{
					UnsignedIntegerToRadixChars<TUnsigned, Utf16Char, BinConverter<TUnsigned>>(in number.value, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				precision = int.Max(precision, CountHexDigits(in value));
				return string.Create(precision, (value, fmt), (destination, number) =>
				{
					UnsignedIntegerToRadixChars<TUnsigned, Utf16Char, HexConverter<TUnsigned>>(in number.value, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'd':
			case 'D':
			case 'g':
			case 'G':
				precision = int.Max(precision, TUnsigned.CountDigits(in value));
				return string.Create(precision, value, (destination, number) =>
				{
					TUnsigned.ToDecChars(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			default:
				return FormatNumber(in value, format!, NumberFormatInfo.GetInstance(provider));
		}
	}
	internal static bool TryFormatUInt<TUnsigned, TChar>(in TUnsigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where TUnsigned : unmanaged, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (format.IsEmpty)
		{
			charsWritten = TUnsigned.CountDigits(in value);

			if (destination.Length < charsWritten)
			{
				charsWritten = 0;
				return false;
			}

			TUnsigned.ToDecChars(value, destination, charsWritten);
			return true;
		}

		char fmt = GetFormat(format, out int precision);
		switch (fmt)
		{
			case 'b':
			case 'B':
				charsWritten = int.Max(precision, CountBinDigits(in value));

				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}

				UnsignedIntegerToRadixChars<TUnsigned, TChar, BinConverter<TUnsigned>>(in value, fmt, destination, charsWritten);
				return true;
			case 'x':
			case 'X':
				charsWritten = int.Max(precision, CountHexDigits(in value));

				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}

				UnsignedIntegerToRadixChars<TUnsigned, TChar, HexConverter<TUnsigned>>(in value, fmt, destination, charsWritten);
				return true;
			case 'd':
			case 'D':
			case 'g':
			case 'G':
				charsWritten = int.Max(precision, TUnsigned.CountDigits(in value));

				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}

				TUnsigned.ToDecChars(value, destination, charsWritten);
				return true;
			default:
				return TryFormatNumber(in value, destination, out charsWritten, format, NumberFormatInfo.GetInstance(provider));
		}
	}
}
