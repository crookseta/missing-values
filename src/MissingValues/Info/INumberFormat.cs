using MissingValues.Internals;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MissingValues.Info;

internal interface INumberFormat
{
	abstract static bool CanRound { get; }
	abstract static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>;
	abstract static int GetDefaultDecimalDigits(NumberFormatInfo info);
	abstract static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits);
	abstract static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number,
	int nMaxDigits, bool isUpper, NumberFormatInfo info) where TChar : unmanaged, IUtfCharacter<TChar>;
}

internal readonly struct CurrencyFormat : INumberFormat
{
	internal static string[] PosCurrencyFormats =>
	[
		"$#", "#$", "$ #", "# $"
	];

	internal static string[] NegCurrencyFormats =>
	[
		"($#)", "-$#", "$-#", "$#-",
		"(#$)", "-#$", "#-$", "#$-",
		"-# $", "-$ #", "# $-", "$ #-",
		"$ -#", "#- $", "($ #)", "(# $)",
		"$- #"
	];

	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.CurrencyDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		ReadOnlySpan<char> fmt = number.IsNegative ?
			NegCurrencyFormats[(info.CurrencyNegativePattern)] :
			PosCurrencyFormats[(info.CurrencyPositivePattern)];

		Span<TChar> currencyDecimalSeparator = stackalloc TChar[TChar.GetLength(info.CurrencyDecimalSeparator)];
		Span<TChar> currencyGroupSeparator = stackalloc TChar[TChar.GetLength(info.CurrencyGroupSeparator)];
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
		Span<TChar> currencySymbol = stackalloc TChar[TChar.GetLength(info.CurrencySymbol)];

		TChar.Copy(info.CurrencyDecimalSeparator, currencyDecimalSeparator);
		TChar.Copy(info.CurrencyGroupSeparator, currencyGroupSeparator);
		TChar.Copy(info.NegativeSign, negativeSign);
		TChar.Copy(info.CurrencySymbol, currencySymbol);

		foreach (var ch in fmt)
		{
			switch (ch)
			{
				case '#':
					NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, info.CurrencyGroupSizes, currencyDecimalSeparator, currencyGroupSeparator);
					break;
				case '-':
					vlb.Append(negativeSign);
					break;
				case '$':
					vlb.Append(currencySymbol);
					break;
				default:
					vlb.Append((TChar)ch);
					break;
			}
		}
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct EngineeringFormat : INumberFormat
{
	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return 6;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return TNumber.IsBinaryInteger();
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);

		if (number.IsNegative)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			vlb.Append(negativeSign);
		}

		ref byte dig = ref number.GetDigitsReference();

		vlb.Append((TChar)((dig != 0) ? (char)(dig) : '0'));
		dig = ref Unsafe.Add(ref dig, 1);

		if (nMaxDigits != 1)
		{
			vlb.Append(numberDecimalSeparator);
		}

		while (--nMaxDigits > 0)
		{
			vlb.Append((TChar)((dig != 0) ? (char)(dig) : '0'));
			dig = ref Unsafe.Add(ref dig, 1);
		}

		int e = number.Digits[0] == 0 ? 0 : number.Scale - 1;
		NumberFormatter.FormatExponent(ref vlb, info, e, isUpper ? 'E' : 'e', 3, true);
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return ++nMaxDigits;
	}
}
internal readonly struct FixedFormat : INumberFormat
{
	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		if (number.IsNegative)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			vlb.Append(negativeSign);
		}

		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		Span<TChar> numberGroupSeparator = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		TChar.Copy(info.NumberGroupSeparator, numberGroupSeparator);

		NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, null, numberDecimalSeparator, numberGroupSeparator);
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct NumericFormat : INumberFormat
{
	internal static readonly string[] NegNumberFormats =
	[
		"(#)", "-#", "- #", "#-", "# -",
	];

	static bool INumberFormat.CanRound => true;

	static int INumberFormat.GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	static bool INumberFormat.IsSupported<TNumber>()
	{
		return true;
	}

	static void INumberFormat.Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
	{
		ReadOnlySpan<char> fmt = number.IsNegative ?
			NegNumberFormats[(info.NumberNegativePattern)] :
			['#'];


		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		Span<TChar> numberGroupSeparator = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		TChar.Copy(info.NumberGroupSeparator, numberGroupSeparator);
		TChar.Copy(info.NegativeSign, negativeSign);

		foreach (var ch in fmt)
		{
			switch (ch)
			{
				case '#':
					NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, info.NumberGroupSizes, numberDecimalSeparator, numberGroupSeparator);
					break;
				case '-':
					vlb.Append(negativeSign);
					break;
				default:
					vlb.Append((TChar)ch);
					break;
			}
		}
	}

	static int INumberFormat.GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}

internal interface IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static abstract NumberStyles AllowedStyles { get; }
	static abstract bool IsValidChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
	static abstract TInteger FromChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
	static abstract uint MaxDigitValue { get; }
	static abstract int MaxDigitCount { get; }
	static abstract int MaxUInt64DigitCount { get; }
	static abstract int BitsPerCharacter { get; }
	static abstract TInteger ShiftLeftForNextDigit(in TInteger value);
}
internal readonly struct HexConverter<TInteger> : IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static NumberStyles IIntegerRadixConverter<TInteger>.AllowedStyles => NumberStyles.HexNumber;

	static uint IIntegerRadixConverter<TInteger>.MaxDigitValue => 0xF;

	static int IIntegerRadixConverter<TInteger>.MaxDigitCount => TInteger.MaxHexDigits;

	static int IIntegerRadixConverter<TInteger>.MaxUInt64DigitCount => 16;

	static int IIntegerRadixConverter<TInteger>.BitsPerCharacter => 4;

	static TInteger IIntegerRadixConverter<TInteger>.FromChar<TChar>(TChar ch)
	{
		return TInteger.GetHexValue((char)ch);
	}

	static bool IIntegerRadixConverter<TInteger>.IsValidChar<TChar>(TChar ch)
	{
		return TChar.IsHexDigit(ch);
	}

	static TInteger IIntegerRadixConverter<TInteger>.ShiftLeftForNextDigit(in TInteger value)
	{
		return value << 4;
	}
}
internal readonly struct BinConverter<TInteger> : IIntegerRadixConverter<TInteger>
	where TInteger : struct, IFormattableInteger<TInteger>
{
	static NumberStyles IIntegerRadixConverter<TInteger>.AllowedStyles => NumberStyles.BinaryNumber;

	static uint IIntegerRadixConverter<TInteger>.MaxDigitValue => 0b1;

	static int IIntegerRadixConverter<TInteger>.MaxDigitCount => TInteger.MaxBinaryDigits;

	static int IIntegerRadixConverter<TInteger>.MaxUInt64DigitCount => 64;

	static int IIntegerRadixConverter<TInteger>.BitsPerCharacter => 1;

	static TInteger IIntegerRadixConverter<TInteger>.FromChar<TChar>(TChar ch)
	{
		return TInteger.GetDecimalValue((char)ch);
	}

	static bool IIntegerRadixConverter<TInteger>.IsValidChar<TChar>(TChar ch)
	{
		return ch == (TChar)'1' || ch == (TChar)'0';
	}

	static TInteger IIntegerRadixConverter<TInteger>.ShiftLeftForNextDigit(in TInteger value)
	{
		return value << 1;
	}
}