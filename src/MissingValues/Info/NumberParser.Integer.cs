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

	private static ParsingStatus ParseDecStringToInteger<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, NumberFormatInfo formatProvider, out T output, out int charsConsumed)
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
		
		if (!TChar.IsDigit(s[charsConsumed]))
		{
			if ((allowTrailingWhite && TChar.IsWhiteSpace(s[charsConsumed])) || s[charsConsumed] == TChar.NullCharacter)
			{
				if (allowTrailingWhite)
				{
					int consumedWhites = s[charsConsumed..].IndexOfAnyExcept(TChar.WhiteSpaceCharacter, TChar.NullCharacter);
					if (consumedWhites < 0)
					{
						charsConsumed += s.Length - charsConsumed;
					}
					else
					{
						charsConsumed += consumedWhites;
						return ParsingStatus.Partial;
					}
				}
				else
				{
					charsConsumed = ConsumeTrailingNulls(s, charsConsumed);
				}
				return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
			}
			else
			{
				return ParsingStatus.Partial;
			}
		}

		if (!T.TryCheckedMultiplyAdd(output, 10, (uint)s[charsConsumed++] - '0', out output) || (charsConsumed < s.Length && TChar.IsDigit(s[charsConsumed])))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Overflow;
		}

		return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
	}

	private static ParsingStatus ParseStringToUnsigned<TInteger, TChar, TConverter>(ReadOnlySpan<TChar> s, out TInteger output, out int charsConsumed)
		where TInteger : struct, IFormattableUnsignedInteger<TInteger>
		where TChar : unmanaged, IUtfCharacter<TChar>
		where TConverter : struct, IIntegerRadixConverter<TInteger>
	{
		if (s.Length > TConverter.MaxDigitCount)
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Overflow;
		}
		ulong temp;
		int count = TConverter.MaxUInt64DigitCount;
		if (s.Length <= count)
		{
			if (!TChar.TryParsePartialInteger(s, TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp, out charsConsumed))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
			output = TInteger.CreateTruncating(temp);
			return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		}

		if (!TChar.TryParsePartialInteger(s[..count], TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp, out charsConsumed))
		{
			output = default;
			return ParsingStatus.Failed;
		}
		Debug.Assert(charsConsumed <= count);
		output = TInteger.CreateTruncating(temp);
		if (charsConsumed < count)
		{
			// We got trailing invalid characters.
			return ParsingStatus.Partial;
		}
		ReadOnlySpan<TChar> slice = s[charsConsumed..];
		int consumed;

		while (count <= slice.Length)
		{
			if (!TChar.TryParsePartialInteger(slice[..count], TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp, out consumed))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
			Debug.Assert(consumed <= count);
			if (consumed < count)
			{
				// We got trailing invalid characters. That means the block has been interrupted and we got partial.
				int shiftAmount = consumed * TConverter.BitsPerCharacter;
				charsConsumed += consumed;
				output <<= shiftAmount;
				output |= TInteger.CreateTruncating(temp);
				
				return ParsingStatus.Partial;
			}
			output <<= 64;
			charsConsumed += consumed;
			output |= TInteger.CreateTruncating(temp);
			slice = slice[consumed..];
		}

		if (slice.Length != 0)
		{
			if (!TChar.TryParsePartialInteger(slice, TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp, out consumed))
			{
				output = default;
				return ParsingStatus.Failed;
			}
			int shiftAmount = consumed * TConverter.BitsPerCharacter;
			charsConsumed += consumed;
			output <<= shiftAmount;
			output |= TInteger.CreateTruncating(temp);
		}
		return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
	}

	internal static T ParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
		where T : struct, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var status = TryParseToUnsignedCore(s, style, formatProvider, out T output, out _);
		if (!status.IsSuccessful())
		{
			if (typeof(TChar) == typeof(Utf16Char))
			{
				status.Throw<T>(TChar.CastToCharSpan(s).ToString());
			}
			else
			{
				status.Throw<T>(TChar.CastToByteSpan(s));
			}
		}

		return output;
	}
	internal static bool TryParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
		where T : struct, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (TryParseToUnsignedCore(s, style, formatProvider, out output, out _).IsSuccessful())
		{
			return true;
		}
		output = default;
		return false;
	}
	internal static bool TryParsePartialToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output, out int elementsConsumed)
		where T : struct, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (TryParseToUnsignedCore(s, style, formatProvider, out output, out elementsConsumed).IsSuccessfulOrPartial())
		{
			return true;
		}
		output = default;
		return false;
	}

	private static ParsingStatus TryParseToUnsignedCore<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output, out int charsConsumed)
		where T : struct, IFormattableUnsignedInteger<T>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		int index = 0;
		if (style.HasFlag(NumberStyles.AllowLeadingWhite))
		{
			index = s.IndexOfAnyExcept(TChar.WhiteSpaceCharacter);
			if (index < 0)
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
		}
		else if (TChar.IsWhiteSpace(s[index]))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Failed;
		}
		NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(formatProvider);
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
		TChar.Copy(formatInfo.NegativeSign, negativeSign);
		if (TChar.StartsWith(s[index..], negativeSign, StringComparison.OrdinalIgnoreCase))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Underflow;
		}

		ParsingStatus status;
		
		if ((style & Special) != 0)
		{
			NumberInfo number = new NumberInfo(stackalloc byte[IntBufferLength]);
			NumberFormatInfo info = NumberFormatInfo.GetInstance(formatProvider);
			if (!NumberInfo.TryParseCore(s[index..], ref number, info, style, out charsConsumed).IsSuccessfulOrPartial()
				|| !NumberInfo.TryConvertToInteger(ref number, out output))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}

			charsConsumed += index;
			return ParsingStatus.Success;
		}

		if (style.HasFlag(NumberStyles.AllowHexSpecifier))
		{
			status = ParseStringToUnsigned<T, TChar, HexConverter<T>>(s[index..], out output, out charsConsumed);
		}
		else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
		{
			status = ParseStringToUnsigned<T, TChar, BinConverter<T>>(s[index..], out output, out charsConsumed);
		}
		else
		{
			status = ParseDecStringToInteger(s[index..], style, formatInfo, out output, out charsConsumed);
		}

		charsConsumed += index;
		if (charsConsumed == s.Length) return status;
		
		var remaining = s[charsConsumed..];
		if (style.HasFlag(NumberStyles.AllowTrailingWhite))
		{
			int trailingSpaces = remaining.IndexOfAnyExcept(TChar.WhiteSpaceCharacter, TChar.NullCharacter);
			charsConsumed += (trailingSpaces >= 0 ? trailingSpaces : remaining.Length);
		}
		else if (TChar.StartsWith(remaining, [TChar.WhiteSpaceCharacter], StringComparison.OrdinalIgnoreCase))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Failed;
		}
		else
		{
			int trailingSpaces = remaining.IndexOfAnyExcept(TChar.NullCharacter);
			charsConsumed += (trailingSpaces >= 0 ? trailingSpaces : remaining.Length);
		}

		return status;
	}

	internal static TSigned ParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
		where TSigned : struct, IFormattableSignedInteger<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		var status = TryParseToSignedCore<TSigned, TUnsigned, TChar>(s, style, formatProvider, out TSigned output, out _);
		if (!status.IsSuccessful())
		{
			if (typeof(TChar) == typeof(Utf16Char))
			{
				status.Throw<TSigned>(TChar.CastToCharSpan(s).ToString());
			}
			else
			{
				status.Throw<TSigned>(TChar.CastToByteSpan(s));
			}
		}

		return output;
	}
	internal static bool TryParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
		where TSigned : struct, IFormattableSignedInteger<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (TryParseToSignedCore<TSigned, TUnsigned, TChar>(s, style, formatProvider, out output, out _).IsSuccessful())
		{
			return true;
		}
		output = default;
		return false;
	}
	internal static bool TryParsePartialToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output, out int elementsConsumed)
		where TSigned : struct, IFormattableSignedInteger<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		if (TryParseToSignedCore<TSigned, TUnsigned, TChar>(s, style, formatProvider, out output, out elementsConsumed).IsSuccessfulOrPartial())
		{
			return true;
		}
		output = default;
		return false;
	}
	private static ParsingStatus TryParseToSignedCore<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output, out int charsConsumed)
		where TSigned : struct, IFormattableSignedInteger<TSigned>
		where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned>
		where TChar : unmanaged, IUtfCharacter<TChar>
	{
		Debug.Assert(Unsafe.SizeOf<TUnsigned>() == Unsafe.SizeOf<TSigned>());
		int index = 0;
		if (style.HasFlag(NumberStyles.AllowLeadingWhite))
		{
			index = s.IndexOfAnyExcept(TChar.WhiteSpaceCharacter);
			if (index < 0)
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}
		}
		else if (TChar.IsWhiteSpace(s[index]))
		{
			charsConsumed = 0;
			output = default;
			return ParsingStatus.Failed;
		}

		NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(formatProvider);
		bool isNegative, openParentheses = false;
		Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
		TChar.Copy(formatInfo.NegativeSign, negativeSign);
		ParsingStatus status;
		
		if ((style & Special) != 0)
		{
			NumberInfo number = new NumberInfo(stackalloc byte[IntBufferLength]);
			if (!NumberInfo.TryParseCore(s[index..], ref number, formatInfo, style, out charsConsumed).IsSuccessfulOrPartial()
			    || !NumberInfo.TryConvertToInteger(ref number, out output))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}

			charsConsumed += index;
			return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		}
		
		TUnsigned result;
		
		if (style.HasFlag(NumberStyles.AllowHexSpecifier))
		{
			status = ParseStringToUnsigned<TUnsigned, TChar, HexConverter<TUnsigned>>(s[index..], out result, out charsConsumed);
			output = Unsafe.BitCast<TUnsigned, TSigned>(result);
			charsConsumed += index;
			return status;
		}
		if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
		{
			status = ParseStringToUnsigned<TUnsigned, TChar, BinConverter<TUnsigned>>(s[index..], out result, out charsConsumed);
			output = Unsafe.BitCast<TUnsigned, TSigned>(result);
			charsConsumed += index;
			return status;
		}

		if (style.HasFlag(NumberStyles.AllowParentheses) && TChar.StartsWith(s[index..], [(TChar)'('], StringComparison.OrdinalIgnoreCase))
		{
			isNegative = true;
			openParentheses = true;
			index++;
		}
		else
		{
			isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && TChar.StartsWith(s[index..], negativeSign, StringComparison.OrdinalIgnoreCase);
			if (isNegative)
			{
				index += negativeSign.Length;
			}
		}
		
		status = ParseDecStringToInteger(s[index..], style, formatInfo, out result, out charsConsumed);
		
		charsConsumed += index;
		if (style.HasFlag(NumberStyles.AllowParentheses) && openParentheses && TChar.StartsWith(s[charsConsumed..], [(TChar)')'], StringComparison.OrdinalIgnoreCase))
		{
			charsConsumed++;
		}

		if (!status.IsSuccessfulOrPartial())
		{
			charsConsumed = 0;
			output = default;
			return status;
		}

		if (s.Length > charsConsumed)
		{
			int trailingWhites = style.HasFlag(NumberStyles.AllowTrailingWhite)
				? s[charsConsumed..].IndexOfAnyExcept(TChar.WhiteSpaceCharacter, TChar.NullCharacter)
				: s[charsConsumed..].IndexOfAnyExcept(TChar.NullCharacter);
			charsConsumed += trailingWhites < 0 ? 0 : trailingWhites;
		}

		if (result == TUnsigned.SignedMaxMagnitude)
		{
			if (!isNegative)
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Overflow;
			}
			output = TSigned.MinValue;
		}
		else
		{
			output = Unsafe.BitCast<TUnsigned, TSigned>(result);

			if (output < TSigned.Zero)
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Overflow;
			}
			if (isNegative)
			{
				output = -output;
			}
		}

		return status;
	}
}