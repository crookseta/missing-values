using MissingValues.Internals;
using System.Globalization;
using System.Numerics;

namespace MissingValues.Info;

internal interface IFormattableFloatingPoint<TSelf> : IFloatingPoint<TSelf>, IFormattableNumber<TSelf>
		where TSelf : IFormattableFloatingPoint<TSelf>
{
	abstract static ReadOnlySpan<TSelf> PowersOfTen { get; }

	static bool IFormattableNumber<TSelf>.IsBinaryInteger()
	{
		return false;
	}
}
internal interface IBinaryFloatingPointInfo<TFloat, TSignificand> : IBinaryFloatingPointIeee754<TFloat>, IFormattableFloatingPoint<TFloat>
	where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
	where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
{
	abstract static bool ExplicitLeadingBit { get; }
	abstract static int NormalMantissaBits { get; }
	abstract static int DenormalMantissaBits { get; }
	abstract static int MinimumDecimalExponent { get; }
	abstract static int MaximumDecimalExponent { get; }
	abstract static int MinBiasedExponent { get; }
	abstract static int MaxBiasedExponent { get; }
	abstract static int MaxSignificandPrecision { get; }
	abstract static int ExponentBits { get; }
	abstract static int ExponentBias { get; }
	abstract static int OverflowDecimalExponent { get; }
	abstract static TSignificand DenormalMantissaMask { get; }
	abstract static TSignificand NormalMantissaMask { get; }
	abstract static TSignificand TrailingSignificandMask { get; }
	abstract static TSignificand PositiveZeroBits { get; }
	abstract static TSignificand PositiveInfinityBits { get; }
	abstract static TSignificand NegativeInfinityBits { get; }

	static abstract TFloat BitsToFloat(TSignificand bits);
	static abstract TSignificand FloatToBits(TFloat value);
}

internal static partial class NumberFormatter
{
	public static string FloatToString<TFloat, TSignificand>(
		in TFloat value,
		string? format,
		IFormatProvider? provider)
		where TFloat : unmanaged, IBinaryFloatingPointInfo<TFloat, TSignificand>
		where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
	{
		int maxSignificandPrecision = TFloat.MaxSignificandPrecision;
		int maxBufferAlloc = maxSignificandPrecision + 4 + 4 + 4; // N significant decimal digits precision, 4 possible special symbols, 4 exponent decimal digits

		int precision;
		bool noFormat = string.IsNullOrWhiteSpace(format);

		if (noFormat)
		{
			precision = maxSignificandPrecision;
		}
		else
		{
			if (format!.Length > 1 && int.TryParse(format![1..].TrimEnd(), out int p))
			{
				precision = p > maxSignificandPrecision ? maxSignificandPrecision : p;
			}
			else
			{
				precision = maxSignificandPrecision;
			}
		}

		ReadOnlySpan<char> fmt = noFormat ? ['G'] : format;

		NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

		Span<Utf16Char> buffer = stackalloc Utf16Char[maxBufferAlloc];
		if (!(fmt.Contains("G", StringComparison.OrdinalIgnoreCase)
			|| fmt.Contains("E", StringComparison.OrdinalIgnoreCase)))
		{
			return FormatNumber(in value, format!, info);
		}

		Ryu.Format<TFloat, TSignificand, Utf16Char>(in value, buffer, out _, fmt, out bool isExceptional, info, precision);

		if (isExceptional || fmt.Contains("E", StringComparison.OrdinalIgnoreCase))
		{
			return new string(Utf16Char.CastToCharSpan(buffer).TrimEnd('\0'));
		}

		return new string(Utf16Char.CastToCharSpan(GetGeneralFromScientificFloatChars(buffer, info, precision, maxSignificandPrecision)));
	}

	public static bool TryFormatFloat<TFloat, TSignificand, TChar>(
		in TFloat value,
		Span<TChar> destination,
		out int charsWritten,
		ReadOnlySpan<char> format,
		IFormatProvider? provider)
		where TFloat : unmanaged, IBinaryFloatingPointInfo<TFloat, TSignificand>
		where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		int maxSignificandPrecision = TFloat.MaxSignificandPrecision;
		int maxBufferAlloc = maxSignificandPrecision + 4 + 4 + 4; // N significant decimal digits precision, 4 possible special symbols, 4 exponent decimal digits

		int precision;

		NumberFormatInfo info = provider is null ? NumberFormatInfo.CurrentInfo : NumberFormatInfo.GetInstance(provider);
		if (format.IsEmpty)
		{
			format = "G";
			precision = maxSignificandPrecision;
		}
		else
		{
			if (int.TryParse(format.Trim()[1..], out int p))
			{
				precision = p > maxSignificandPrecision ? maxSignificandPrecision : p;
			}
			else
			{
				precision = maxSignificandPrecision;
			}
		}


		if (!(format.Contains("G", StringComparison.OrdinalIgnoreCase)
			|| format.Contains("E", StringComparison.OrdinalIgnoreCase)))
		{
			return TryFormatNumber(in value, destination, out charsWritten, format, info);
		}

		Span<TChar> buffer = stackalloc TChar[maxBufferAlloc];
		Ryu.Format<TFloat, TSignificand, TChar>(in value, buffer, out charsWritten, format, out bool isExceptional, info, precision);

		if (isExceptional || format.Contains("E", StringComparison.OrdinalIgnoreCase))
		{
			return buffer.TrimEnd(TChar.NullCharacter).TryCopyTo(destination);
		}

		ReadOnlySpan<TChar> general = GetGeneralFromScientificFloatChars(buffer, info, precision, maxSignificandPrecision);
		charsWritten = general.Length;
		return general.TryCopyTo(destination);
	}

	private static ReadOnlySpan<TChar> GetGeneralFromScientificFloatChars<TChar>(Span<TChar> buffer, NumberFormatInfo info, int precision, int maxSignificandPrecision)
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		Span<TChar> actualValue = buffer.TrimEnd(TChar.NullCharacter);

		int eIndex = actualValue.IndexOf((TChar)'E');
		if (eIndex <= 0 || !TChar.TryParseInteger(actualValue[(eIndex + 1)..], NumberStyles.Integer, CultureInfo.CurrentCulture, out int exponent))
		{
			exponent = 0;
		}

		Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
		TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
		TChar.Copy(info.NegativeSign, negativeSign);

		bool isNegativeExponent = exponent < 0;
		bool isNegative = buffer.IndexOf(negativeSign) == 0;
		int dotIndex = buffer.IndexOf(numberDecimalSeparator);
		bool containsDecimalSeparator = dotIndex >= 0;

		// If buffer cannot be represented with precision.
		if ((!isNegativeExponent && (containsDecimalSeparator && exponent >= actualValue[(dotIndex + 1)..eIndex].Length && maxSignificandPrecision < actualValue.Length)) ||
			(isNegativeExponent && (containsDecimalSeparator && (-exponent) >= 1 && maxSignificandPrecision < actualValue.Length)))
		{
			return actualValue;
		}
		if (!containsDecimalSeparator && ((isNegativeExponent && (-exponent) >= maxSignificandPrecision) || (!isNegativeExponent && exponent >= maxSignificandPrecision)))
		{
			return actualValue;
		}
		if (int.Abs(exponent) >= maxSignificandPrecision)
		{
			return actualValue;
		}

		// Get rid of the scientific notation
		actualValue[eIndex..].Fill(TChar.NullCharacter);
		actualValue = actualValue[..eIndex];


		int temp;

		if (!containsDecimalSeparator)
		{
			if (isNegativeExponent) // ie: 5E-1 = 0.5
			{
				/*
				 * Since we got rid of E.. actualValue only has 5, now we have to add the leading zeroes
				 * we know we have to add the first zero as 0.N, so lets do that first
				 */

				int i;

				if (isNegative)
				{
					i = 4;
					buffer[2 + numberDecimalSeparator.Length] = buffer[1];
					buffer[1] = (TChar)'0';
					numberDecimalSeparator.CopyTo(buffer[2..]);
				}
				else
				{
					i = 3;
					buffer[1 + numberDecimalSeparator.Length] = buffer[0];
					buffer[0] = (TChar)'0';
					numberDecimalSeparator.CopyTo(buffer[1..]);
				}

				for (int leadingZeroes = (-exponent) - 1; leadingZeroes > 0 && i < buffer.Length; leadingZeroes--, i++)
				{
					(buffer[i - 1], buffer[i]) = ((TChar)'0', buffer[i - 1]);
				}
			}
			else if (exponent != 0) // ie: 5E1 = 50
			{
				/*
				 * This one is easier, we just add trailing zeroes
				 */
				for (int i = isNegative ? 2 : 1, trailingZeroes = exponent; trailingZeroes > 0 && i < buffer.Length; i++, trailingZeroes--)
				{
					buffer[i] = (TChar)'0';
				}
			}


		}
		else if (isNegativeExponent) // ie: 1.1E-5 = 0.000011
		{
			Span<TChar> digits = stackalloc TChar[actualValue.Length - 1];
			digits[0] = isNegative ? actualValue[1] : actualValue[0];
			actualValue[(dotIndex + 1)..].CopyTo(digits[1..]);

			buffer[isNegative ? 1 : 0] = (TChar)'0';
			int i;

			for (i = isNegative ? 3 : 2, temp = exponent; temp > 0 && i < buffer.Length - digits.Length; i++, temp--)
			{
				buffer[i] = (TChar)'0';
			}

			digits.CopyTo(buffer[i..]);
		}
		else if (exponent != 0) // ie: 1.1E5 = 110000
		{
			int i;
			int decimalDigits = actualValue[(dotIndex + 1)..].Length;
			if (decimalDigits < exponent && exponent < buffer.Length)
			{
				i = actualValue.Length;
				buffer.Slice(i, exponent - decimalDigits).Fill((TChar)'0');
			}
			i = isNegative ? 3 : 2;
			for (temp = exponent; temp > 0 && i < buffer.Length; i++, temp--)
			{
				(buffer[i - 1], buffer[i]) = (buffer[i], buffer[i - 1]);
			}
		}

		actualValue = buffer.TrimEnd(TChar.NullCharacter);

		if (actualValue.EndsWith(numberDecimalSeparator))
		{
			actualValue = actualValue[..^numberDecimalSeparator.Length];
			containsDecimalSeparator = false;
			dotIndex = -1;
		}
		else
		{
			containsDecimalSeparator = (dotIndex = actualValue.IndexOf(numberDecimalSeparator)) > -1;
		}

		if (containsDecimalSeparator && actualValue[dotIndex..].Length > precision)
		{
			actualValue = actualValue[..precision];
		}

		return actualValue;
	}
}
