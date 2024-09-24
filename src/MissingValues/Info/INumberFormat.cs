using MissingValues.Internals;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

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
	internal static readonly byte[][] Utf8PosCurrencyFormats =
	[
		"$#"u8.ToArray(), "#$"u8.ToArray(), "$ #"u8.ToArray(), "# $"u8.ToArray()
	];
	internal static readonly byte[][] Utf8NegCurrencyFormats =
	[
		"($#)"u8.ToArray(), "-$#"u8.ToArray(), "$-#"u8.ToArray(), "$#-"u8.ToArray(),
		"(#$)"u8.ToArray(), "-#$"u8.ToArray(), "#-$"u8.ToArray(), "#$-"u8.ToArray(),
		"-# $"u8.ToArray(), "-$ #"u8.ToArray(), "# $-"u8.ToArray(), "$ #-"u8.ToArray(),
		"$ -#"u8.ToArray(), "#- $"u8.ToArray(), "($ #)"u8.ToArray(), "(# $)"u8.ToArray(),
		"$- #"u8.ToArray()
	];
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


	public static bool CanRound => true;

	public static int GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.CurrencyDecimalDigits;
	}

	public static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>
	{
		return true;
	}

	public static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info) 
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var fmt = number.IsNegative ?
			TChar.GetNegativeCurrencyFormat(info.CurrencyNegativePattern) :
			TChar.GetPositiveCurrencyFormat(info.CurrencyPositivePattern);

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
			switch ((char)ch)
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
					vlb.Append(ch);
					break;
			}
		}
	}

	public static bool TryParse<TNumber, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TNumber result)
		where TNumber : struct, IFormattableNumber<TNumber>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		throw new NotImplementedException();
	}

	public static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct EngineeringFormat : INumberFormat
{
	public static bool CanRound => true;

	public static int GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return 6;
	}

	public static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>
	{
		return TNumber.IsBinaryInteger();
	}

	public static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info) where TChar : unmanaged, IUtfCharacter<TChar>
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

		while(--nMaxDigits > 0)
		{
			vlb.Append((TChar)((dig != 0) ? (char)(dig) : '0'));
			dig = ref Unsafe.Add(ref dig, 1);
		}

		int e = number.Digits[0] == 0 ? 0 : number.Scale - 1;
		NumberFormatter.FormatExponent(ref vlb, info, e, isUpper ? 'E' : 'e', 3, true);
	}

	public static bool TryParse<TNumber, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TNumber result)
		where TNumber : struct, IFormattableNumber<TNumber>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		throw new NotImplementedException();
	}

	public static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return ++nMaxDigits;
	}
}
internal readonly struct FixedFormat : INumberFormat
{
	public static bool CanRound => true;

	public static int GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	public static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>
	{
		return true;
	}

	public static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info) where TChar : unmanaged, IUtfCharacter<TChar>
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

	public static bool TryParse<TNumber, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TNumber result)
		where TNumber : struct, IFormattableNumber<TNumber>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		throw new NotImplementedException();
	}

	public static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}
internal readonly struct NumericFormat : INumberFormat
{
	internal static readonly byte[][] Utf8NegNumberFormats =
	[
		"(#)"u8.ToArray(), "-#"u8.ToArray(), "- #"u8.ToArray(), "#-"u8.ToArray(), "# -"u8.ToArray(),
	];
	internal static readonly string[] NegNumberFormats =
	[
		"(#)", "-#", "- #", "#-", "# -",
	];

	public static bool CanRound => true;

	public static int GetDefaultDecimalDigits(NumberFormatInfo info)
	{
		return info.NumberDecimalDigits;
	}

	public static bool IsSupported<TNumber>() where TNumber : struct, IFormattableNumber<TNumber>
	{
		return true;
	}

	public static void Format<TChar>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info) where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var fmt = number.IsNegative ?
			TChar.GetNegativeNumberFormat(info.NumberNegativePattern) :
			TChar.GetPositiveNumberFormat(0);

		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		Span<TChar> numberGroupSeparator = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];

		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		TChar.Copy(info.NumberGroupSeparator, numberGroupSeparator);
		TChar.Copy(info.NegativeSign, negativeSign);

		foreach (var ch in fmt)
		{
			switch ((char)ch)
			{
				case '#':
					NumberFormatter.FormatGroupedNumeric(ref vlb, ref number, nMaxDigits, info.NumberGroupSizes, numberDecimalSeparator, numberGroupSeparator);
					break;
				case '-':
					vlb.Append(negativeSign);
					break;
				default:
					vlb.Append(ch);
					break;
			}
		}
	}

	public static bool TryParse<TNumber, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TNumber result)
		where TNumber : struct, IFormattableNumber<TNumber>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		throw new NotImplementedException();
	}

	public static int GetRoundingPosition(ref NumberInfo number, ref int nMaxDigits)
	{
		return number.Scale + nMaxDigits;
	}
}