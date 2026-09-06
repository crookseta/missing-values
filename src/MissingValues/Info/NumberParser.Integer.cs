using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MissingValues.Internals;

namespace MissingValues.Info;

internal static partial class NumberParser
{
	private const int IntBufferLength = 154 + 2;
	private const NumberStyles Special = NumberStyles.AllowTrailingSign 
	                                     | NumberStyles.AllowDecimalPoint 
	                                     | NumberStyles.AllowThousands 
	                                     | NumberStyles.AllowExponent 
	                                     | NumberStyles.AllowCurrencySymbol;

	public static ReadOnlySpan<ulong> E19Table => [
		1,
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

	internal static T ParseToInteger<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
		where T : struct, IFormattableInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ParsingStatus status = TryParseStringToInteger(s, style, formatProvider, false, out T result, out _);
		if (status.IsSuccessful())
		{
			return result;
		}

		if (typeof(T) == typeof(Utf8Char))
		{
			status.Throw<T>(TChar.CastToByteSpan(s));
		}
		else
		{
			status.Throw<T>(TChar.CastToCharSpan(s).ToString());
		}

		return default;
	}
	internal static bool TryParseToInteger<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, bool allowPartial, out T output, out int elementsConsumed)
		where T : struct, IFormattableInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		ParsingStatus status = TryParseStringToInteger(s, style, formatProvider, allowPartial, out output, out elementsConsumed);
		if ((allowPartial && status.IsSuccessfulOrPartial()) || (!allowPartial && status.IsSuccessful()))
		{
			return true;
		}
		output = default;
		return false;
	}
	private static ParsingStatus TryParseStringToInteger<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? info, bool allowPartial, out T output, out int charsConsumed)
		where T : struct, IFormattableInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(info);
		
		// Consume leading signs, whitespaces and invalid characters
		charsConsumed = ConsumeChars(s, (styles & NumberStyles.AllowLeadingWhite) != 0);
		if (charsConsumed < 0)
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Failed;
		}

		bool isNegative;
		if ((styles & NumberStyles.AllowLeadingSign) != 0)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
			TChar.Copy(formatInfo.NegativeSign, negativeSign);
			if (TChar.StartsWith(s[charsConsumed..], negativeSign, StringComparison.OrdinalIgnoreCase))
			{
				if (T.IsUnsignedInteger)
				{
					charsConsumed = 0;
					output = default;
					return ParsingStatus.Underflow;
				}

				isNegative = true;
				charsConsumed += negativeSign.Length;
			}
			else
			{
				Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(formatInfo.PositiveSign)];
				TChar.Copy(formatInfo.PositiveSign, positiveSign);
				if (TChar.StartsWith(s[charsConsumed..], positiveSign, StringComparison.OrdinalIgnoreCase))
				{
					charsConsumed += positiveSign.Length;
				}
				isNegative = false;
			}
		}
		else
		{
			isNegative = false;
		}

		// Consume the actual digits
		ParsingStatus status;
		if ((styles & ~NumberStyles.Integer) == 0)
		{
			int elementsConsumed;
			if (Unsafe.SizeOf<T>() == 32)
			{
				status = ParseStringToDecInteger(s[charsConsumed..], styles, formatInfo, out UInt256 absResult, out elementsConsumed);
				output = Unsafe.BitCast<UInt256, T>(absResult);
			}
			else if (Unsafe.SizeOf<T>() == 64)
			{
				status = ParseStringToDecInteger(s[charsConsumed..], styles, formatInfo, out UInt512 absResult, out elementsConsumed);
				output = Unsafe.BitCast<UInt512, T>(absResult);
			}
			else
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
			charsConsumed += elementsConsumed;
		}
		else if ((styles & NumberStyles.AllowHexSpecifier) != 0)
		{
			status = ParseStringToInteger<T, TChar, HexConverter<T>>(s[charsConsumed..], out output, out int elementsConsumed);
			charsConsumed += elementsConsumed;
		}
		else if ((styles & NumberStyles.AllowBinarySpecifier) != 0)
		{
			status = ParseStringToInteger<T, TChar, BinConverter<T>>(s[charsConsumed..], out output, out int elementsConsumed);
			charsConsumed += elementsConsumed;
		}
		else
		{
			NumberInfo number = new NumberInfo(stackalloc byte[IntBufferLength]);
			if (!(status = NumberInfo.TryParseCore(s, ref number, formatInfo, styles, out charsConsumed)).IsSuccessfulOrPartial()
			    || !NumberInfo.TryConvertToInteger(ref number, out output))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}

			if (!allowPartial && !status.IsSuccessful())
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}

			return status;
		}
		
		// Consume trailing signs, whitespaces and nulls
		if (status.IsSuccessfulOrPartial() && (styles & NumberStyles.AllowTrailingSign) != 0)
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
			TChar.Copy(formatInfo.NegativeSign, negativeSign);
			if (TChar.StartsWith(s[charsConsumed..], negativeSign, StringComparison.OrdinalIgnoreCase))
			{
				if (T.IsUnsignedInteger)
				{
					charsConsumed = 0;
					output = default;
					return ParsingStatus.Underflow;
				}

				charsConsumed += negativeSign.Length;
				isNegative = true;
			}
			else
			{
				Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(formatInfo.PositiveSign)];
				TChar.Copy(formatInfo.PositiveSign, positiveSign);
				if (TChar.StartsWith(s[charsConsumed..], positiveSign, StringComparison.OrdinalIgnoreCase))
				{
					charsConsumed += positiveSign.Length;
				}
			}
			status = s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		}
		// Negative correction
		if ((styles & ~NumberStyles.Integer) == 0)
		{
			if (isNegative)
			{
				if (T.IsUnsignedInteger)
				{
					charsConsumed = 0;
					output = default;
					return ParsingStatus.Underflow;
				}

				if (output != T.MinValue && T.IsNegative(output))
				{
					charsConsumed = 0;
					output = default;
					return ParsingStatus.Overflow;
				}

				output = -output;
			}
			else if (!isNegative && T.IsNegative(output))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Overflow;
			}
		}

		if (status.IsSuccessful())
		{
			return status;
		}

		if (!status.IsSuccessfulOrPartial())
		{
			// Either we overflowed or reach some sort of error.
			return status;
		}
		
		if ((styles & NumberStyles.AllowTrailingWhite) != 0)
		{
			int elementsConsumed = ConsumeChars(s[charsConsumed..], true);
			charsConsumed += elementsConsumed;
		}
		else
		{
			int elementsConsumed = ConsumeChars(s[charsConsumed..], false);
			if (elementsConsumed < 0)
			{
				// By this point we should have all the characters consumed.
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
			charsConsumed += elementsConsumed;
		}

		if (!allowPartial && s.Length != charsConsumed)
		{
			// By this point we should have all the characters consumed.
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Failed;
		}

		return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		
		static int ConsumeChars(ReadOnlySpan<TChar> s, bool allowWhite)
		{
			int consumed;
			if (typeof(TChar) == typeof(Utf8Char))
			{
				if (allowWhite)
				{
					consumed = s.IndexOfAnyExcept(TChar.CastFromByteSpan([(byte)' ', (byte)'\0']));
				}
				else
				{
					consumed = s.IndexOfAnyExcept(TChar.NullCharacter);
					int index = consumed < 0 ? 0 : consumed;
					if (index < s.Length && TChar.IsWhiteSpace(s[index]))
					{
						return -1;
					}
				}
			}
			else
			{
				if (allowWhite)
				{
					consumed = s.Length - TChar.CastToCharSpan(s).TrimStart().Length;
				}
				else
				{
					consumed = s.IndexOfAnyExcept(TChar.NullCharacter);
					int index = consumed < 0 ? 0 : consumed;
					if (index < s.Length && TChar.IsWhiteSpace(s[index]))
					{
						return -1;
					}
				}
			}
			
			return consumed < 0 ? s.Length : consumed;
		}
	}
	private static ParsingStatus ParseStringToDecInteger<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, NumberFormatInfo formatProvider, out T output, out int charsConsumed)
		where T : struct, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		// By this point there should not be leading whitespaces.
		styles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite);
		const int UInt64MaxSafeCharacterCount = 19;
		
		bool allowTrailingWhite = styles.HasFlag(NumberStyles.AllowTrailingWhite);
		int leadingZeroes = s.IndexOfAnyExcept((TChar)'0');
		if (leadingZeroes < 0)
		{
			charsConsumed = s.Length;
			output = T.Zero;
			return ParsingStatus.Success;
		}
		charsConsumed = leadingZeroes;

		// Fast path for when it can surely be parsed as ulong.
		if (s.Length - charsConsumed <= UInt64MaxSafeCharacterCount)
		{
			if (TChar.TryParsePartialInteger(s[charsConsumed..], styles, formatProvider, out ulong r, out int consumed))
			{
				charsConsumed += consumed;
				output = T.CreateTruncating(r);
				return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
			}
			else
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
		}

		output = T.Zero;
		
		// Explanation for the Vector128 version of the algorithm here: https://kholdstare.github.io/technical/2020/05/26/faster-integer-parsing.html
		while (Avx512BW.IsSupported && Vector512.IsHardwareAccelerated && s.Length - charsConsumed >= 64 && (charsConsumed - leadingZeroes) + 64 < T.MaxDecimalDigits - 2)
		{
			Vector512<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector512.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar512(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TryParse64Chars(v, out ulong high, out ulong mid, out ulong midLow, out ulong low)) break;
			
			output *= T.E64;
			output += (T.MultiplyByUInt64(T.CreateTruncating(high), 10_000_000_000_000_000UL) + T.CreateTruncating(mid)) * T.E32;
			output += T.MultiplyByUInt64(T.CreateTruncating(midLow), 10_000_000_000_000_000UL) + T.CreateTruncating(low);
			charsConsumed += 64;
		}
		
		while (Avx2.IsSupported && Vector256.IsHardwareAccelerated && s.Length - charsConsumed >= 32 && (charsConsumed - leadingZeroes) + 32 < T.MaxDecimalDigits - 2)
		{
			Vector256<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector256.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar256(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TryParse32Chars(v, out ulong high, out ulong low)) break;
			
			output *= T.E32;
			output += T.MultiplyByUInt64(T.CreateTruncating(high), 10_000_000_000_000_000UL) + T.CreateTruncating(low);
			charsConsumed += 32;
		}
		
		while (Sse41.IsSupported && Vector128.IsHardwareAccelerated && s.Length - charsConsumed >= 16 && (charsConsumed - leadingZeroes) + 16 < T.MaxDecimalDigits - 2)
		{
			Vector128<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector128.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar128(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TryParse16Chars(v, out ulong low)) break;
			
			output = T.MultiplyByUInt64(in output, 10_000_000_000_000_000UL) + T.CreateTruncating(low);
			charsConsumed += 16;
		}
		
		while (s.Length - charsConsumed >= 8 && (charsConsumed - leadingZeroes) + 8 < T.MaxDecimalDigits - 2)
		{
			ulong chunk;
			if (typeof(TChar) == typeof(Utf8Char))
			{
				chunk = BitConverter.ToUInt64(TChar.CastToByteSpan(s[charsConsumed..]));
			}
			else
			{
				var slice = s.Slice(charsConsumed, 8);
				chunk = 0;

				for (int i = 7; i >= 0; i--)
				{
					chunk <<= 8;
					chunk |= (byte)slice[i];
				}
			}
			if (!TryParse8Chars(chunk, out ulong low)) break;
			
			output = T.MultiplyByUInt64(in output, 100_000_000UL) + T.CreateTruncating(low);
			charsConsumed += 8;
		}
		
		int maxDigitsLeft = T.MaxDecimalDigits - (charsConsumed -  leadingZeroes);
		if (maxDigitsLeft < 0) 
		{
			// We've already overflowed.
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Overflow;
		}

		ulong remaining = 0;
		int index = 0;
		for (; index < maxDigitsLeft - 1; index++)
		{
			if (charsConsumed >= s.Length)
			{
				break;
			}

			if (!TChar.IsDigit(s[charsConsumed]))
			{
				if (allowTrailingWhite && TChar.IsWhiteSpace(s[charsConsumed]))
				{
					break;
				}
				if (s[charsConsumed] == TChar.NullCharacter)
				{
					break;
				}
				output = T.MultiplyByUInt64(in output, E19Table[index]);
				output += T.CreateTruncating(remaining);
				return ParsingStatus.Partial;
			}

			remaining *= 10;
			remaining += (uint)s[charsConsumed++] - '0';
		}
		
		output = T.MultiplyByUInt64(in output, E19Table[index]);
		output += T.CreateTruncating(remaining);
		
		if (charsConsumed >= s.Length)
		{
			return ParsingStatus.Success;
		}

		if (TChar.IsDigit(s[charsConsumed]) && !T.TryCheckedMultiplyAdd(output, 10, (uint)s[charsConsumed++] - '0', out output) || (charsConsumed < s.Length && TChar.IsDigit(s[charsConsumed])))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Overflow;
		}

		return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
	}

	private static ParsingStatus ParseStringToInteger<TInteger, TChar, TConverter>(ReadOnlySpan<TChar> s, out TInteger output, out int charsConsumed)
		where TInteger : struct, IFormattableInteger<TInteger>
		where TChar : unmanaged, IUtfCharacter<TChar>
		where TConverter : struct, IIntegerRadixConverter<TInteger>
	{
		int count = TConverter.MaxUInt64DigitCount;
		if (s.Length <= count)
		{
			if (!TChar.TryParsePartialInteger(s, TConverter.AllowedStyles, CultureInfo.CurrentCulture, out ulong temp, out charsConsumed))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
			output = TInteger.CreateTruncating(temp);
			return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		}

		int leadingZeroes = s.IndexOfAnyExcept((TChar)'0');
		if (leadingZeroes < 0)
		{
			charsConsumed = s.Length;
			output = TInteger.Zero;
			return ParsingStatus.Success;
		}
		
		charsConsumed = leadingZeroes;
		output = TInteger.Zero;
		while (Vector512.IsHardwareAccelerated && Avx512BW.IsSupported && (s.Length - charsConsumed) >= 64 && (charsConsumed + 64) <= TConverter.MaxDigitCount)
		{
			Vector512<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector512.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar512(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TConverter.TryParse64Chars(v, ref output)) break;
			
			charsConsumed += 64;
		}
		while (Vector256.IsHardwareAccelerated && Avx2.IsSupported && (s.Length - charsConsumed) >= 32 && (charsConsumed + 32) <= TConverter.MaxDigitCount)
		{
			Vector256<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector256.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar256(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TConverter.TryParse32Chars(v, ref output)) break;
			
			charsConsumed += 32;
		}
		while (Vector128.IsHardwareAccelerated && Ssse3.IsSupported && (s.Length - charsConsumed) >= 16 && (charsConsumed + 16) <= TConverter.MaxDigitCount)
		{
			Vector128<byte> v = typeof(TChar) == typeof(Utf8Char) 
				? Vector128.Create(TChar.CastToByteSpan(s[charsConsumed..])) 
				: FromChar128(TChar.CastToCharSpan(s[charsConsumed..]));
			
			if (!TConverter.TryParse16Chars(v, ref output)) break;
			
			charsConsumed += 16;
		}

		for (int length = int.Min(s.Length, TConverter.MaxDigitCount + leadingZeroes); charsConsumed < length; charsConsumed++)
		{
			if (!TConverter.IsValidChar(s[charsConsumed]))
			{
				break;
			}
			output <<= TConverter.BitsPerCharacter;
			output |= TConverter.FromChar(s[charsConsumed]);
		}
		if (charsConsumed < s.Length && TConverter.IsValidChar(s[charsConsumed]))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Overflow;
		}
		return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
	}
}