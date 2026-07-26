using System.Diagnostics;
using MissingValues.Internals;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

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
	abstract static int MinimumBinaryExponent { get; }
	abstract static int MaximumBinaryExponent { get; }
	abstract static int MinimumDecimalExponent { get; }
	abstract static int MaximumDecimalExponent { get; }
	abstract static int MinBiasedExponent { get; }
	abstract static int MaxBiasedExponent { get; }
	abstract static int MaxSignificandPrecision { get; }
	abstract static int ExponentBits { get; }
	abstract static int ExponentBias { get; }
	abstract static int OverflowDecimalExponent { get; }
	abstract static int InfinityExponent { get; }
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
			if (format!.Length > 1 && int.TryParse(format!.AsSpan()[1..].TrimEnd(), out int p))
			{
				precision = p > maxSignificandPrecision ? maxSignificandPrecision : p;
			}
			else
			{
				precision = maxSignificandPrecision;
			}
		}

		ReadOnlySpan<char> fmt = noFormat ? ['G'] : format.AsSpan().TrimStart();

		NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

		Span<Utf16Char> buffer = stackalloc Utf16Char[maxBufferAlloc];
		int charsWritten;
		// TODO: Expose Hex format for v3.0
		if (false /*fmt.StartsWith("X", StringComparison.OrdinalIgnoreCase) 
		    && TryFormatFloatToHex<TFloat, TSignificand, Utf16Char>(in value, buffer, out charsWritten, fmt[0], precision, info)*/)
		{
			return new string(Utf16Char.CastToCharSpan(buffer).Slice(0, charsWritten));
		}
		if (!(fmt.StartsWith("G", StringComparison.OrdinalIgnoreCase)
			|| fmt.StartsWith("E", StringComparison.OrdinalIgnoreCase)))
		{
			return FormatNumber(in value, format!, info);
		}

		Ryu.Format<TFloat, TSignificand, Utf16Char>(in value, buffer, out charsWritten, fmt, out bool isExceptional, info, precision);

		if (isExceptional || fmt.StartsWith("E", StringComparison.OrdinalIgnoreCase))
		{
			return new string(Utf16Char.CastToCharSpan(buffer).Slice(0, charsWritten));
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

		// TODO: Expose Hex format for v3.0
		if (false /*format.Contains("X", StringComparison.OrdinalIgnoreCase)*/)
		{
			return TryFormatFloatToHex<TFloat, TSignificand, TChar>(in value, destination, out charsWritten, format.TrimStart()[0], precision, info);
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
	
	private static bool TryFormatFloatToHex<TFloat, TSignificand, TChar>(in TFloat value, Span<TChar> destination, out int charsWritten, char format, int precision, NumberFormatInfo info)
		where TFloat : unmanaged, IBinaryFloatingPointInfo<TFloat, TSignificand>
		where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		// Based on CoreLib implementation: https://github.com/dotnet/runtime/blob/1cf40e98f541b9bbcb614f86caf3a2504459c919/src/libraries/System.Private.CoreLib/src/System/Number.Formatting.cs#L544
		
		Debug.Assert((format | 0x20) == 'x');
		Debug.Assert(TFloat.IsFinite(value));

		ValueListBuilder<TChar> builder = new ValueListBuilder<TChar>(stackalloc TChar[256]);
		
		bool isNegative = TFloat.IsNegative(value);

		if (isNegative)
		{
			builder.AppendUtf16(info.NegativeSign);
		}
		
		builder.Append((TChar)'0');
		builder.Append((TChar)format);

		TSignificand fraction = ExtractFractionAndBiasedExponent(in value, out int exponent);

		if (fraction == TSignificand.Zero)
		{
			// +/- 0
			builder.Append((TChar)'0');

			if (precision > 0)
			{
				builder.AppendUtf16(info.NumberDecimalSeparator);
				builder.AppendSpan(precision).Fill((TChar)'0');
			}
			
			// Exponent sign is always emitted ('+' or '-'), consistent with the 'E' format.
			builder.Append((TChar)(format == 'X' ? 'P' : 'p'));
			builder.Append((TChar)'+');
			builder.Append((TChar)'0');

			return builder.TryCopyTo(destination, out charsWritten);
		}
		
		// ExtractFractionAndBiasedExponent returns (note: despite the name, the exponent is unbiased):
		//   For normal:   fraction = (1 << DenormalMantissaBits) | mantissa, exponent = biasedExp - ExponentBias - DenormalMantissaBits
		//   For denormal: fraction = mantissa, exponent = MinBinaryExponent - DenormalMantissaBits
		//
		// We want the form: 1.xxxxx * 2^e
		// So we need to normalize so that the leading 1 bit is at bit DenormalMantissaBits.
		// For normal numbers, this is already the case.
		// For denormal numbers, we need to shift left until the leading 1 is there.

		int mantissaBits = TFloat.DenormalMantissaBits;

		if (fraction < (TSignificand.One << mantissaBits))
		{
			// Denormal: shift the leading 1 up to the implicit bit position
			int lz = int.CreateTruncating(TSignificand.LeadingZeroCount(fraction)) - ((Unsafe.SizeOf<TSignificand>() * 8 - 1) - mantissaBits);
			fraction <<= lz;
			exponent -= lz;
		}
		
		// Now fraction has the leading 1 at bit [mantissaBits], and the remaining bits below.
		// The unbiased exponent for the value is: exponent + mantissaBits (since fraction is
		// really fraction * 2^exponent, and we want 1.xxx * 2^actualExponent).
		int actualExponent = exponent + mantissaBits;

		// Strip the implicit leading 1 to get the fractional bits
		TSignificand significandBits = fraction & ((TSignificand.One << mantissaBits) - TSignificand.One);

		// Leading digit is normally '1' for non-zero (the implicit bit)
		int leadingDigit = 1;

		// Determine how many hex digits to emit for the fractional part
		int defaultHexDigits = (mantissaBits + 3) / 4;
		
		if (precision == 0)
		{
			// Round significandBits into the leading digit
			TSignificand half = (mantissaBits > 0) ? (TSignificand.One << (mantissaBits - 1)) : TSignificand.Zero;
			if (significandBits > half || (significandBits == half && (leadingDigit & 1) != 0))
			{
				leadingDigit++;
				// leadingDigit can't exceed 2 since it started at 1
			}

			significandBits = TSignificand.Zero;
		}
		
		builder.Append((TChar)(char)('0' + leadingDigit));

		if (precision > 0)
		{
			TSignificand shifted;

			if (precision < defaultHexDigits)
			{
				// Need to round
				int bitsToKeep = precision * 4;
				int bitsToDiscard = mantissaBits - bitsToKeep;

				// bitsToDiscard is always in (0, mantissaBits) here because precision >= 1
				// (we're in the precision > 0 branch) and precision < defaultHexDigits
				// (checked above), so bitsToKeep < mantissaBits and bitsToDiscard > 0.
				// For all IEEE types mantissaBits <= 52, so bitsToDiscard < 64.
				Debug.Assert(bitsToDiscard > 0 && bitsToDiscard < (Unsafe.SizeOf<TSignificand>() * 8));
				if (bitsToDiscard > 0 && bitsToDiscard < (Unsafe.SizeOf<TSignificand>() * 8))
				{
					TSignificand roundBit = TSignificand.One << (bitsToDiscard - 1);
					TSignificand discardedBits = significandBits & ((TSignificand.One << bitsToDiscard) - TSignificand.One);
					bool roundUp = discardedBits > roundBit || (discardedBits == roundBit && ((significandBits >> bitsToDiscard) & TSignificand.One) != TSignificand.Zero);

					if (roundUp)
					{
						significandBits = (significandBits >> bitsToDiscard) + TSignificand.One;

						// Check if rounding overflowed into leading digit
						if (significandBits >= (TSignificand.One << bitsToKeep))
						{
							significandBits = TSignificand.Zero;
							actualExponent++;
						}
					}
					else
					{
						significandBits >>= bitsToDiscard;
					}

					shifted = significandBits << ((Unsafe.SizeOf<TSignificand>() * 8) - bitsToKeep);
				}
				else
				{
					shifted = significandBits << ((Unsafe.SizeOf<TSignificand>() * 8) - mantissaBits);
				}
			}
			else
			{
				shifted = significandBits << ((Unsafe.SizeOf<TSignificand>() * 8) - mantissaBits);
			}
			
			builder.AppendUtf16(info.NumberDecimalSeparator);
			
			// Emit real nibbles
			int realDigits = Math.Min(precision, defaultHexDigits);
			for (int i = 0; i < realDigits; i++)
			{
				builder.Append(format == 'X' 
					? TChar.ToCharUpper(uint.CreateTruncating(shifted >> (Unsafe.SizeOf<TSignificand>() * 8 - 4)))
					: TChar.ToCharLower(uint.CreateTruncating(shifted >> (Unsafe.SizeOf<TSignificand>() * 8 - 4))));
				shifted <<= 4;
			}
			
			// Emit padding zeros (when precision > defaultHexDigits)
			int padCount = precision - realDigits;
			if (padCount > 0)
			{
				builder.AppendSpan(padCount).Fill((TChar)'0');
			}
		}
		else if (precision < 0)
		{
			// Default precision: emit significant hex digits, trimming trailing zeros.
			// Compute trailing zero nibbles from the nibble-aligned representation.
			int trimmedDigits = 0;
			if (significandBits != TSignificand.Zero)
			{
				// Align significand to nibble boundary (pad LSB so total bits = defaultHexDigits * 4),
				// then count trailing zero nibbles via trailing zero bits.
				int paddingBits = defaultHexDigits * 4 - mantissaBits;
				TSignificand nibbleAligned = significandBits << paddingBits;
				int trailingZeroBits = int.CreateTruncating(TSignificand.TrailingZeroCount(nibbleAligned));
				trimmedDigits = defaultHexDigits - (trailingZeroBits / 4);
				
				if (trimmedDigits > 0)
				{
					builder.AppendUtf16(info.NumberDecimalSeparator);

					TSignificand shifted = significandBits << ((Unsafe.SizeOf<TSignificand>() * 8) - mantissaBits);
					for (int i = 0; i < trimmedDigits; i++)
					{
						builder.Append(format == 'X' 
							? TChar.ToUpper((TChar)uint.CreateTruncating(shifted >> (Unsafe.SizeOf<TSignificand>() * 8 - 4)))
							: TChar.ToLower((TChar)uint.CreateTruncating(shifted >> (Unsafe.SizeOf<TSignificand>() * 8 - 4))));
						shifted <<= 4;
					}
				}
			}
		}
		
		// Emit exponent: p+NNN or p-NNN
		// The exponent sign is always ASCII '+'/'-' per IEEE 754 §5.12.3,
		// independent of NumberFormatInfo (which only governs the leading value sign).
		builder.Append((TChar)(format == 'X' ? 'P' : 'p'));
		
		if (actualExponent >= 0)
		{
			builder.Append((TChar)'+');
		}
		else
		{
			builder.Append((TChar)'-');
			actualExponent = -actualExponent;
		}
		
		// Write exponent digits
		Debug.Assert(actualExponent >= 0);
		int digitCount = CountDigits((ulong)actualExponent);
		Span<TChar> digits = stackalloc TChar[digitCount + 1];
		UInt64ToDecChars((ulong)actualExponent, ref digits[digitCount], digitCount);
		builder.Append(digits[..digitCount]);
		
		return builder.TryCopyTo(destination, out charsWritten);

		static TSignificand ExtractFractionAndBiasedExponent(in TFloat value, out int exponent)
		{
			TSignificand bits = TFloat.FloatToBits(value);
			TSignificand fraction = (bits & TFloat.DenormalMantissaMask);
			exponent = int.CreateTruncating(bits >> TFloat.DenormalMantissaBits) & TFloat.InfinityExponent;

			if (exponent != 0)
			{
				// For normalized value,
				// value = 1.fraction * 2^(exp - ExponentBias)
				//       = (1 + mantissa / 2^TrailingSignificandLength) * 2^(exp - ExponentBias)
				//       = (2^TrailingSignificandLength + mantissa) * 2^(exp - ExponentBias - TrailingSignificandLength)
				//
				// So f = (2^TrailingSignificandLength + mantissa), e = exp - ExponentBias - TrailingSignificandLength;
				fraction |= (TSignificand.One << TFloat.DenormalMantissaBits);
				exponent -= TFloat.ExponentBias + TFloat.DenormalMantissaBits;
			}
			else
			{
				// For denormalized value,
				// value = 0.fraction * 2^(MinBinaryExponent)
				//       = (mantissa / 2^TrailingSignificandLength) * 2^(MinBinaryExponent)
				//       = mantissa * 2^(MinBinaryExponent - TrailingSignificandLength)
				//       = mantissa * 2^(MinBinaryExponent - TrailingSignificandLength)
				// So f = mantissa, e = MinBinaryExponent - TrailingSignificandLength
				exponent = TFloat.MinimumBinaryExponent - TFloat.DenormalMantissaBits;
			}
			return fraction;
		}
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
		}

		return actualValue;
	}
}
