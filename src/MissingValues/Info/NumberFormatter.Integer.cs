using MissingValues.Internals;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info;

internal interface IFormattableInteger<TSelf> : IFormattableNumber<TSelf>, IBigInteger<TSelf>
		where TSelf : IFormattableInteger<TSelf>?
{
	/// <summary>
	/// Converts a <typeparamref name="TSelf"/> to a <see cref="char"/>.
	/// </summary>
	/// <returns></returns>
	char ToChar();
	/// <summary>
	/// Converts a <typeparamref name="TSelf"/> to a <see cref="int"/>.
	/// </summary>
	/// <returns></returns>
	int ToInt32();

	/// <summary>
	/// Converts the specified hexadecimal character to a <typeparamref name="TSelf"/>.
	/// </summary>
	/// <param name="value">A hexadecimal character.</param>
	/// <returns>The hexadecimal value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
	abstract static TSelf GetHexValue(char value);

	static bool IFormattableNumber<TSelf>.IsBinaryInteger()
	{
		return true;
	}

	/// <summary>
	/// Gets the value <c>2</c> of the type.
	/// </summary>
	abstract static TSelf Two { get; }
	/// <summary>
	/// Gets the value <c>16</c> of the type.
	/// </summary>
	abstract static TSelf Sixteen { get; }
	/// <summary>
	/// Gets the value <c>10</c> of the type.
	/// </summary>
	abstract static TSelf Ten { get; }

	/// <summary>
	/// Gets the value <c>4</c> of the type.
	/// </summary>
	abstract static TSelf TwoPow2 { get; }
	/// <summary>
	/// Gets the value <c>256</c> of the type.
	/// </summary>
	abstract static TSelf SixteenPow2 { get; }
	/// <summary>
	/// Gets the value <c>100</c> of the type.
	/// </summary>
	abstract static TSelf TenPow2 { get; }

	/// <summary>
	/// Gets the value <c>8</c> of the type.
	/// </summary>
	abstract static TSelf TwoPow3 { get; }
	/// <summary>
	/// Gets the value <c>4096</c> of the type.
	/// </summary>
	abstract static TSelf SixteenPow3 { get; }
	/// <summary>
	/// Gets the value <c>1000</c> of the type.
	/// </summary>
	abstract static TSelf TenPow3 { get; }

	/// <summary>
	/// Gets the left-most digit of the maximum value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static char LastDecimalDigitOfMaxValue { get; }
	/// <summary>
	/// Gets the number of digits of the maximum decimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxDecimalDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum hexadecimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxHexDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum binary value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxBinaryDigits { get; }
}

internal interface IFormattableSignedInteger<TSigned, TUnsigned> : IFormattableInteger<TSigned>, ISignedNumber<TSigned>
	where TSigned : IFormattableSignedInteger<TSigned, TUnsigned>
	where TUnsigned : IUnsignedNumber<TUnsigned>, IFormattableInteger<TUnsigned>
{
	/// <summary>
	/// Returns the unsigned representation of the signed integer.
	/// </summary>
	/// <returns>The unsigned representation of the signed integer.</returns>
	TUnsigned ToUnsigned();
}

internal interface IFormattableUnsignedInteger<TUnsigned, TSigned> : IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
	where TUnsigned : IFormattableUnsignedInteger<TUnsigned, TSigned>
	where TSigned : IFormattableInteger<TSigned>
{
	/// <summary>
	/// Gets the absolute representation of the maximum representable value of <typeparamref name="TSigned"/>(Abs(TSigned.MinValue)).
	/// </summary>
	abstract static TUnsigned SignedMaxMagnitude { get; }

	/// <summary>
	/// Returns the signed representation of the unsigned integer.
	/// </summary>
	/// <returns>The signed representation of the unsigned integer.</returns>
	TSigned ToSigned();
	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	static abstract int CountDigits(in TUnsigned value);
}

internal static partial class NumberFormatter
{
	internal static int CountDigits(UInt128 value)
	{
		ulong upper = ((ulong)(value >> 64));

		// 1e19 is    8AC7_2304_89E8_0000
		// 1e20 is  5_6BC7_5E2D_6310_0000
		// 1e21 is 36_35C9_ADC5_DEA0_0000

		if (upper == 0)
		{
			// We have less than 64-bits, so just return the lower count
			return CountDigits((ulong)value);
		}

		// We have more than 1e19, so we have at least 20 digits
		int digits = 20;

		if (upper > 5)
		{
			// ((2^128) - 1) / 1e20 < 34_02_823_669_209_384_635 which
			// is 18.5318 digits, meaning the result definitely fits
			// into 64-bits and we only need to add the lower digit count

			value /= new UInt128(0x5, 0x6BC7_5E2D_6310_0000); // value /= 1e20

			digits += CountDigits((ulong)value);
		}
		else if ((upper == 5) && ((ulong)value >= 0x6BC75E2D63100000))
		{
			// We're greater than 1e20, but definitely less than 1e21
			// so we have exactly 21 digits

			digits++;
			Debug.Assert(digits == 21);
		}

		return digits;
	}
	internal static int CountDigits(ulong value)
	{
		ReadOnlySpan<byte> log2ToPow10 = stackalloc byte[]
		{
			1,  1,  1,  2,  2,  2,  3,  3,  3,  4,  4,  4,  4,  5,  5,  5,
			6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9,  10, 10, 10,
			10, 11, 11, 11, 12, 12, 12, 13, 13, 13, 13, 14, 14, 14, 15, 15,
			15, 16, 16, 16, 16, 17, 17, 17, 18, 18, 18, 19, 19, 19, 19, 20
		};

		int index = log2ToPow10[BitOperations.Log2(value)];

		ReadOnlySpan<ulong> powersOf10 = stackalloc ulong[]
		{
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
		};

		ulong powerOf10 = powersOf10[index];

		// Return the number of digits based on the power of 10, shifted by 1
		// if it falls below the threshold.
		bool lessThan = value < powerOf10;
		return (index - Unsafe.As<bool, byte>(ref lessThan));
	}
	internal static int CountHexDigits<T>(in T value)
		where T : struct, IFormattableInteger<T>
	{
		return (T.Log2(value) >> 2).ToInt32() + 1;
	}
	internal static int CountBinDigits<T>(in T value)
		where T : struct, IFormattableInteger<T>
	{
		return T.MaxBinaryDigits - T.LeadingZeroCount(value).ToInt32();
	}

	internal static unsafe void UnsignedIntegerToDecChars<TUnsigned, TSigned, TChar>(TUnsigned value, Span<TChar> destination)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		fixed (TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + int.Min(TUnsigned.CountDigits(in value), destination.Length);
			if (value >= TUnsigned.Ten)
			{
				TUnsigned remainder;
				while (value >= TUnsigned.TenPow2)
				{
					bufferEnd -= 2;
					(value, remainder) = TUnsigned.DivRem(value, TUnsigned.TenPow2);
					WriteTwoDigits(in remainder, bufferEnd);
				}
				if (value >= TUnsigned.Ten)
				{
					bufferEnd -= 2;
					WriteTwoDigits(in value, bufferEnd);
					return;
				}
			}

			*(--bufferEnd) = (TChar)(char)(value.ToChar() + '0');
		}

		static void WriteTwoDigits(in TUnsigned value, TChar* ptr)
		{
			Unsafe.CopyBlockUnaligned(
				ref *(byte*)ptr,
				ref Unsafe.Add(ref MemoryMarshal.GetReference(TChar.TwoDigitsAsBytes), sizeof(TChar) * 2 * value.ToInt32()),
				(uint)sizeof(TChar) * 2
				);
		}
	}
	internal static unsafe void UnsignedIntegerToDecChars<TUnsigned, TSigned, TChar>(TUnsigned value, Span<TChar> destination, int digits)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		fixed(TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + int.Min(digits, destination.Length);
			if (value >= TUnsigned.Ten)
			{
				TUnsigned remainder;
				TUnsigned tenPow2 = TUnsigned.TenPow2;
				while (value >= tenPow2)
				{
					bufferEnd -= 2;
					digits -= 2;
					(value, remainder) = TUnsigned.DivRem(value, tenPow2);
					WriteTwoDigits(in remainder, bufferEnd);
				}
				if (value >= TUnsigned.Ten)
				{
					bufferEnd -= 2;
					WriteTwoDigits(in value, bufferEnd);
					return;
				}
			}

			*(--bufferEnd) = (TChar)(char)(value.ToChar() + '0');
		}

		static void WriteTwoDigits(in TUnsigned value, TChar* ptr)
		{
			Unsafe.CopyBlockUnaligned(
				ref *(byte*)ptr,
				ref Unsafe.Add(ref MemoryMarshal.GetReference(TChar.TwoDigitsAsBytes), sizeof(TChar) * 2 * value.ToInt32()),
				(uint)sizeof(TChar) * 2
				);
		}
	}
	public static unsafe void UnsignedIntegerToHexChars<TUnsigned, TSigned, TChar>(in TUnsigned value, char isUpper, Span<TChar> destination, int digits)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		destination[..digits].Fill((TChar)'0');
		int hexBase = (isUpper - ('X' - 'A' + 10));
		var value64 = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TUnsigned, ulong>(ref Unsafe.AsRef(in value)), sizeof(TUnsigned) /  sizeof(ulong));

		fixed (TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + digits;
			const int MaxDigitsPerChunk = 16;
			int digitsLeft = digits;

			for (int i = 0; i < value64.Length && digits > 0; i++)
			{
				ulong v = value64[i];

				while ((digits - digitsLeft) < MaxDigitsPerChunk || v != 0)
				{
					byte digit = (byte)(v & 0xF);
					*(--bufferEnd) = (TChar)(char)(digit + (digit < 10 ? (byte)'0' : hexBase));
					digitsLeft--;
					v >>= 4;
				}

				digits -= MaxDigitsPerChunk;
			}
		}
	}
	public static unsafe void UnsignedIntegerToBinChars<TUnsigned, TSigned, TChar>(in TUnsigned value, Span<TChar> destination, int digits)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var value64 = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TUnsigned, ulong>(ref Unsafe.AsRef(in value)), sizeof(TUnsigned) /  sizeof(ulong));

		fixed (TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + digits;
			const int MaxDigitsPerChunk = 64;
			int digitsLeft = digits;

			for (int i = 0; i < value64.Length && digits > 0; i++)
			{
				ulong v = value64[i];
				while ((digits - digitsLeft) < MaxDigitsPerChunk || v != 0)
				{
					*(--bufferEnd) = (TChar)(char)('0' + (v & 0x1));
					digitsLeft--;
					v >>= 1;
				}

				digits -= MaxDigitsPerChunk;
			}
		}
	}
	

	public static string UnsignedIntegerToString<T>(in T value, T numberBase)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
	{
		int digits = BitHelper.CountDigits(value, numberBase);

		return string.Create(digits, value, (chars, num) =>
		{
			ReadOnlySpan<char> numbers = stackalloc char[]
			{
					'0',
					'1',
					'2',
					'3',
					'4',
					'5',
					'6',
					'7',
					'8',
					'9',
					'A',
					'B',
					'C',
					'D',
					'E',
					'F'
				};
			for (int i = chars.Length - 1; i >= 0; i--)
			{
				(num, var rem) = T.DivRem(num, numberBase);
				chars[i] = numbers[rem.ToInt32()];
			}
		});
	}
	public static string UnsignedIntegerToString<T>(in T value, T numberBase, int digits)
		where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
	{
		digits = int.Max(BitHelper.CountDigits(value, numberBase), digits);

		return string.Create(digits, value, (chars, num) =>
		{
			ReadOnlySpan<char> numbers = stackalloc char[]
			{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F'
			};
			for (int i = chars.Length - 1; i >= 0; i--)
			{
				(num, var rem) = T.DivRem(num, numberBase);
				chars[i] = numbers[rem.ToInt32()];
			}
		});
	}
	public static void UnsignedIntegerToCharSpan<T>(T value, in T numberBase, Span<char> destination)
		where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
	{
		ReadOnlySpan<char> numbers = stackalloc char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		for (int i = destination.Length - 1; i >= 0; i--)
		{
			(value, var rem) = T.DivRem(value, numberBase);
			destination[i] = numbers[rem.ToInt32()];
		}
	}
	public static void UnsignedIntegerToCharSpan<T>(T value, in T numberBase, int digits, Span<char> destination)
		where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
	{
		ReadOnlySpan<char> numbers = stackalloc char[]
		{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F'
			};

		for (int i = digits - 1; i >= 0; i--)
		{
			(value, var rem) = T.DivRem(value, numberBase);
			destination[i] = numbers[rem.ToInt32()];
		}
	}
	public static void UnsignedIntegerToCharSpan<TNumber, TChar>(TNumber value, in TNumber numberBase, int digits, Span<TChar> destination)
		where TNumber : struct, IFormattableInteger<TNumber>, IUnsignedNumber<TNumber>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var numbers = TChar.Digits;

		for (int i = digits - 1; i >= 0; i--)
		{
			(value, var rem) = TNumber.DivRem(value, numberBase);
			destination[i] = numbers[rem.ToInt32()];
		}
	}

	public static string UnsignedIntegerToDecimalString<T>(in T value)
		where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
	{
		int digits = BitHelper.CountDigits<T>(value, T.Ten);

		return string.Create(digits, value, (chars, num) =>
		{
			const string DigitTable =
				"0001020304050607080910111213141516171819" +
				"2021222324252627282930313233343536373839" +
				"4041424344454647484950515253545556575859" +
				"6061626364656667686970717273747576777879" +
				"8081828384858687888990919293949596979899";

			int next = chars.Length - 1;
			while (num >= T.TenPow2)
			{
				(num, var rem) = T.DivRem(num, T.TenPow2);
				int i = rem.ToInt32() * 2;
				chars[next] = DigitTable[i + 1];
				chars[next - 1] = DigitTable[i];
				next -= 2;
			}

			if (num < T.Ten)
			{
				chars[next] = (char)('0' + num.ToChar());
			}
			else
			{
				int i = num.ToInt32() * 2;
				chars[next] = DigitTable[i + 1];
				chars[next - 1] = DigitTable[i];
			}
		});
	}
	public static string FormatUnsignedInteger<TUnsigned, TSigned>(in TUnsigned value, string? format, IFormatProvider? formatProvider)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
	{
		int precision = 0;
		if (!string.IsNullOrWhiteSpace(format) && format.Length != 1 && !int.TryParse(format[1..], out precision))
		{
			Thrower.InvalidFormat(format.ToString());
		}

		char fmt;
		if (string.IsNullOrWhiteSpace(format))
		{
			fmt = 'd';
		}
		else
		{
			fmt = format[0];
		}

		switch (fmt)
		{
			case 'b':
			case 'B':
				precision = int.Max(precision, CountBinDigits(in value));
				return string.Create(precision, value, (destination, number) =>
				{
					UnsignedIntegerToBinChars<TUnsigned, TSigned, Utf16Char>(in number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				precision = int.Max(precision, CountHexDigits(in value));
				return string.Create(precision, (value, fmt), (destination, number) =>
				{
					UnsignedIntegerToHexChars<TUnsigned, TSigned, Utf16Char>(in number.value, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'd':
			case 'D':
				precision = int.Max(precision, TUnsigned.CountDigits(in value));
				return string.Create(precision, value, (destination, number) =>
				{
					UnsignedIntegerToDecChars<TUnsigned, TSigned, Utf16Char>(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			default:
				return FormatNumber(in value, format!, NumberFormatInfo.GetInstance(formatProvider));
		}
	}
	public static bool TryFormatUnsignedInteger<TUnsigned, TSigned, TChar>(in TUnsigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		int precision = 0;
		if (format.Length > 1 && !int.TryParse(format[1..], out precision))
		{
			Thrower.InvalidFormat(format.ToString());
		}

		char fmt;
		if (format.Length < 1)
		{
			fmt = 'd';
		}
		else
		{
			fmt = format[0];
		}

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
				UnsignedIntegerToBinChars<TUnsigned, TSigned, TChar>(in value, destination, charsWritten);
				break;
			case 'x':
			case 'X':
				charsWritten = int.Max(precision, CountHexDigits(in value));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToHexChars<TUnsigned, TSigned, TChar>(in value, fmt, destination, charsWritten);
				break;
			case 'd':
			case 'D':
				charsWritten = int.Max(precision, TUnsigned.CountDigits(in value));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToDecChars<TUnsigned, TSigned, TChar>(value, destination, charsWritten);
				break;
			default:
				return TryFormatNumber(in value, destination, out charsWritten, format, NumberFormatInfo.GetInstance(provider));
		}
		return true;
	}

	public static string FormatSignedInteger<TSigned, TUnsigned>(in TSigned value, string? format, NumberStyles style, IFormatProvider? formatProvider)
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
	{
		int precision = 0;
		if (format is not null && format.Length != 1 && !int.TryParse(format[1..], out precision))
		{
			Thrower.InvalidFormat(format);
		}

		char fmt;

		fmt = string.IsNullOrWhiteSpace(format) ? 'd' : format[0];
		TUnsigned absValue;

		switch (fmt)
		{
			case 'b':
			case 'B':
				absValue = value.ToUnsigned();
				precision = int.Max(precision, CountBinDigits(in absValue));
				return string.Create(precision, absValue, (destination, number) =>
				{
					UnsignedIntegerToBinChars<TUnsigned, TSigned, Utf16Char>(in number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				absValue = value.ToUnsigned();
				precision = int.Max(precision, CountHexDigits(in absValue));
				return string.Create(precision, (absValue, fmt), (destination, number) =>
				{
					UnsignedIntegerToHexChars<TUnsigned, TSigned, Utf16Char>(in number.absValue, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'd':
			case 'D':
				if (TSigned.IsNegative(value))
				{
					var negativeSign = NumberFormatInfo.GetInstance(formatProvider).NegativeSign;
					absValue = value == TSigned.MinValue ? TUnsigned.SignedMaxMagnitude : TSigned.Abs(value).ToUnsigned();
					precision = int.Max(precision, TUnsigned.CountDigits(in absValue));
					return negativeSign + string.Create(precision, absValue, (destination, number) =>
					{
						UnsignedIntegerToDecChars<TUnsigned, TSigned, Utf16Char>(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
					});
				}
				absValue = value.ToUnsigned();
				precision = int.Max(precision, TUnsigned.CountDigits(in absValue));
				return string.Create(precision, absValue, (destination, number) =>
				{
					UnsignedIntegerToDecChars<TUnsigned, TSigned, Utf16Char>(number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			default:
				return FormatNumber(in value, format, NumberFormatInfo.GetInstance(formatProvider));
		}
	}

	public static bool TryFormatSignedInteger<TSigned, TUnsigned, TChar>(in TSigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		int precision = 0;
		if (format.Length > 1 && !int.TryParse(format[1..], out precision))
		{
			Thrower.InvalidFormat(format.ToString());
		}

		char fmt;
		NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

		fmt = format.IsWhiteSpace() || format.IsEmpty ? 'd' : format[0];
		TUnsigned absValue;

		switch (fmt)
		{
			case 'b':
			case 'B':
				absValue = value.ToUnsigned();
				charsWritten = int.Max(precision, CountBinDigits(in absValue));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToBinChars<TUnsigned, TSigned, TChar>(in absValue, destination, charsWritten);
				break;
			case 'x':
			case 'X':
				absValue = value.ToUnsigned();
				charsWritten = int.Max(precision, CountHexDigits(in absValue));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToHexChars<TUnsigned, TSigned, TChar>(in absValue, fmt, destination, charsWritten);
				break;
			case 'd':
			case 'D':
				if (TSigned.IsNegative(value))
				{
					Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
					TChar.Copy(info.NegativeSign, negativeSign);
					absValue = value == TSigned.MinValue ? TUnsigned.SignedMaxMagnitude : TSigned.Abs(value).ToUnsigned();
					charsWritten = int.Max(precision, TUnsigned.CountDigits(in absValue)) + negativeSign.Length;
					if (destination.Length < charsWritten || !negativeSign.TryCopyTo(destination))
					{
						charsWritten = 0;
						return false;
					}
					UnsignedIntegerToDecChars<TUnsigned, TSigned, TChar>(absValue, destination[negativeSign.Length..], charsWritten - negativeSign.Length);
					break;
				}
				absValue = value.ToUnsigned();
				charsWritten = int.Max(precision, TUnsigned.CountDigits(in absValue));
				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}
				UnsignedIntegerToDecChars<TUnsigned, TSigned, TChar>(absValue, destination, charsWritten);
				break;
			default:
				return TryFormatNumber(in value, destination, out charsWritten, format, NumberFormatInfo.GetInstance(provider));
		}

		return true;
	}
}
