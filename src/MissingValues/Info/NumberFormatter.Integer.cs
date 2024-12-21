using MissingValues.Internals;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info;

internal interface IFormattableInteger<TSelf> : IFormattableNumber<TSelf>, IBigInteger<TSelf>, IMinMaxValue<TSelf>
		where TSelf : IFormattableInteger<TSelf>?
{
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

	abstract static int UnsignedCompare(in TSelf value1, in TSelf value2);

	abstract static int Log2Int32(in TSelf value);

	abstract static int LeadingZeroCountInt32(in TSelf value);

	static bool IFormattableNumber<TSelf>.IsBinaryInteger() => true;

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

	virtual static TSelf E19 
	{
		get => TSelf.CreateTruncating(10000000000000000000UL);
	} 

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
	abstract static bool IsUnsignedInteger { get; }
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
	abstract static TUnsigned SignedMaxMagnitude { get; }

	static abstract int CountDigits(in TUnsigned value);

	static abstract void ToDecChars<TChar>(in TUnsigned number, Span<TChar> destination, int digits) where TChar : unmanaged, IUtfCharacter<TChar>;

	static bool IFormattableInteger<TUnsigned>.IsUnsignedInteger => true;
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

	private static ref TChar UInt32ToDecChars<TChar>(uint value, ref TChar bufferEnd)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		// Borrowed from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Number.Formatting.cs
		if (value >= 10)
		{
			// Handle all values >= 100 two-digits at a time so as to avoid expensive integer division operations.
			while (value >= 100)
			{
				bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 2);
				(value, uint remainder) = Math.DivRem(value, 100);
				WriteTwoDigits(remainder, ref bufferEnd);
			}

			// If there are two digits remaining, store them.
			if (value >= 10)
			{
				bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 2);
				WriteTwoDigits(value, ref bufferEnd);
				return ref bufferEnd;
			}
		}

		bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 1);
		bufferEnd = (TChar)(value + '0');
		return ref bufferEnd;
	}
	private static ref TChar UInt32ToDecChars<TChar>(uint value, ref TChar bufferEnd, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		// Borrowed from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Number.Formatting.cs
		uint remainder;
		while (value >= 100)
		{
			bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 2);
			digits -= 2;
			(value, remainder) = Math.DivRem(value, 100);
			WriteTwoDigits(remainder, ref bufferEnd);
		}
		while (value != 0 || digits > 0)
		{
			digits--;
			(value, remainder) = Math.DivRem(value, 10);
			bufferEnd = ref Unsafe.Subtract(ref bufferEnd, 1);
			bufferEnd = (TChar)(remainder + '0');
		}

		return ref bufferEnd;
	}

	private static unsafe void WriteTwoDigits<TChar>(uint value, ref TChar ptr)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		Unsafe.CopyBlockUnaligned(
			ref Unsafe.As<TChar, byte>(ref ptr),
			ref Unsafe.Add(ref MemoryMarshal.GetReference(TChar.TwoDigitsAsBytes), (uint)sizeof(TChar) * 2 * value),
			(uint)sizeof(TChar) * 2
			);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint UInt256DivMod1E9(ref UInt256 value)
	{
		Calculator.DivRem(value, 1_000_000_000U, out value, out uint remainder);
		return remainder;
	}
	private static void UInt256ToDecChars<TChar>(UInt256 value, Span<TChar> destination)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), UInt256.CountDigits(in value));

		while (value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0 || value.Part0 > uint.MaxValue)
		{
			bufferEnd = ref UInt32ToDecChars(UInt256DivMod1E9(ref value), ref bufferEnd, 9);
		}
		UInt32ToDecChars((uint)value.Part0, ref bufferEnd);
	}
	internal static void UInt256ToDecChars<TChar>(UInt256 value, Span<TChar> destination, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), digits);

		while (value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0 || value.Part0 > uint.MaxValue)
		{
			bufferEnd = ref UInt32ToDecChars(UInt256DivMod1E9(ref value), ref bufferEnd, 9);
			digits -= 9;
		}
		UInt32ToDecChars((uint)value.Part0, ref bufferEnd, digits);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint UInt512DivMod1E9(ref UInt512 value)
	{
		Calculator.DivRem(value, 1_000_000_000U, out value, out uint remainder);
		return remainder;
	}
	private static void UInt512ToDecChars<TChar>(UInt512 value, Span<TChar> destination)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), UInt512.CountDigits(in value));

		while (value.Part7 != 0 || value.Part6 != 0 || value.Part5 != 0 || value.Part4 != 0 || value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0 || value.Part0 > uint.MaxValue)
		{
			bufferEnd = ref UInt32ToDecChars(UInt512DivMod1E9(ref value), ref bufferEnd, 9);
		}
		UInt32ToDecChars((uint)value.Part0, ref bufferEnd);
	}
	internal static void UInt512ToDecChars<TChar>(UInt512 value, Span<TChar> destination, int digits)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ref TChar bufferEnd = ref Unsafe.Add(ref MemoryMarshal.GetReference(destination), digits);

		while (value.Part7 != 0 || value.Part6 != 0 || value.Part5 != 0 || value.Part4 != 0 || value.Part3 != 0 || value.Part2 != 0 || value.Part1 != 0 || value.Part0 > uint.MaxValue)
		{
			bufferEnd = ref UInt32ToDecChars(UInt512DivMod1E9(ref value), ref bufferEnd, 9);
			digits -= 9;
		}
		UInt32ToDecChars((uint)value.Part0, ref bufferEnd, digits);
	}
	public static unsafe void UnsignedIntegerToHexChars<T, TChar>(in T value, char isUpper, Span<TChar> destination, int digits)
		where T : unmanaged, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		destination[..digits].Fill((TChar)'0');
		int hexBase = (isUpper - ('X' - 'A' + 10));
		Span<ulong> value64 = stackalloc ulong[sizeof(T) / sizeof(ulong)];
		Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(value64)), value);
		value64 = value64[..(value64.Length - (T.LeadingZeroCountInt32(in value) / 64))];

		fixed (TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + digits;
			int maxDigitsPerChunk = 16;
			int digitsLeft = digits;

			for (int i = 0; i < value64.Length && digits > 0; i++)
			{
				ulong v = value64[i];

				while ((digits - digitsLeft) < maxDigitsPerChunk || v != 0)
				{
					byte digit = (byte)(v & 0xF);
					*(--bufferEnd) = (TChar)(char)(digit + (digit < 10 ? (byte)'0' : hexBase));
					digitsLeft--;
					v >>= 4;
				}

				digits -= maxDigitsPerChunk;
				maxDigitsPerChunk = Math.Min(digits, maxDigitsPerChunk);
			}
		}
	}
	public static unsafe void UnsignedIntegerToBinChars<T, TChar>(in T value, Span<TChar> destination, int digits)
		where T : unmanaged, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		destination[..digits].Fill((TChar)'0');
		Span<ulong> value64 = stackalloc ulong[sizeof(T) / sizeof(ulong)];
		Unsafe.WriteUnaligned(ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(value64)), value);
		value64 = value64[..(value64.Length - (T.LeadingZeroCountInt32(in value) / 64))];

		fixed (TChar* ptr = &MemoryMarshal.GetReference(destination))
		{
			TChar* bufferEnd = ptr + digits;
			int maxDigitsPerChunk = 64;
			int digitsLeft = digits;

			for (int i = 0; i < value64.Length && digits > 0; i++)
			{
				ulong v = value64[i];
				while ((digits - digitsLeft) < maxDigitsPerChunk || v != 0)
				{
					*(--bufferEnd) = (TChar)(char)('0' + (v & 0x1));
					digitsLeft--;
					v >>= 1;
				}

				digits -= maxDigitsPerChunk;
				maxDigitsPerChunk = Math.Min(digits, maxDigitsPerChunk);
			}
		}
	}
	
	public static string FormatInt<TSigned, TUnsigned>(in TSigned value, string? format, IFormatProvider? provider)
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
				return string.Create(precision, u, (destination, number) =>
				{
					UnsignedIntegerToBinChars(in number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				u = Unsafe.BitCast<TSigned, TUnsigned>(value);
				precision = int.Max(precision, CountHexDigits(in u));
				return string.Create(precision, (u, fmt), (destination, number) =>
				{
					UnsignedIntegerToHexChars(in number.u, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
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
	public static bool TryFormatInt<TSigned, TUnsigned, TChar>(in TSigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
				UnsignedIntegerToBinChars(in u, destination, charsWritten);
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
				UnsignedIntegerToHexChars(in u, fmt, destination, charsWritten);
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
	public static string FormatUInt<TUnsigned>(in TUnsigned value, string? format, IFormatProvider? provider)
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
				return string.Create(precision, value, (destination, number) =>
				{
					UnsignedIntegerToBinChars(in number, Utf16Char.CastFromCharSpan(destination), destination.Length);
				});
			case 'x':
			case 'X':
				precision = int.Max(precision, CountHexDigits(in value));
				return string.Create(precision, (value, fmt), (destination, number) =>
				{
					UnsignedIntegerToHexChars(in number.value, number.fmt, Utf16Char.CastFromCharSpan(destination), destination.Length);
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
	public static bool TryFormatUInt<TUnsigned, TChar>(in TUnsigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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

				UnsignedIntegerToBinChars(in value, destination, charsWritten);
				return true;
			case 'x':
			case 'X':
				charsWritten = int.Max(precision, CountHexDigits(in value));

				if (destination.Length < charsWritten)
				{
					charsWritten = 0;
					return false;
				}

				UnsignedIntegerToHexChars(in value, fmt, destination, charsWritten);
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
