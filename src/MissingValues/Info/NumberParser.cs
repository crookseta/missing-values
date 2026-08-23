using MissingValues.Internals;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissingValues.Info
{
	internal static class NumberParser
	{
		internal readonly struct ParsingStatus
		{
			private const int SuccessValue = 0;
			private const int FailedValue = 1;
			private const int OverflowValue = 2;
			private const int UnderflowValue = 3;
			private const int PartialValue = 4;
			
			internal static ParsingStatus Success => new ParsingStatus(SuccessValue);
			internal static ParsingStatus Failed => new ParsingStatus(FailedValue);
			internal static ParsingStatus Overflow => new ParsingStatus(OverflowValue);
			internal static ParsingStatus Underflow => new ParsingStatus(OverflowValue);
			internal static ParsingStatus Partial => new ParsingStatus(PartialValue);

			private readonly int _status;

			private ParsingStatus(int status)
			{
				_status = status;
			}

			internal void Throw<T>(ReadOnlySpan<byte> utf8Input)
				where T : IParsable<T>, IMinMaxValue<T>
			{
				Throw<T>(new string(Encoding.UTF8.GetChars(utf8Input.ToArray())));
			}
			internal void Throw<T>(string input)
				where T : IParsable<T>, IMinMaxValue<T>
			{
				throw _status switch
				{
					OverflowValue => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is bigger than {T.MaxValue}"),
					UnderflowValue => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is smaller than {T.MinValue}"),
					_ => new FormatException($"Could not parse '{input}' as {typeof(T)}.\n"),
				};
			}
			
			internal bool IsSuccessful() => _status == SuccessValue;
			internal bool IsSuccessfulOrPartial() => _status is SuccessValue or PartialValue;
		}
		
		internal static int ConsumeTrailingNulls<TChar>(ReadOnlySpan<TChar> value, int index)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// For compatibility, we need to allow trailing nulls at the end of a number string
			var remainder = value.Slice(index);

			var nullsToConsume = remainder.IndexOfAnyExcept(TChar.NullCharacter);
			return index + ((nullsToConsume >= 0) ? nullsToConsume : remainder.Length);
		}

		#region Integer
		private const int IntBufferLength = 154 + 2;
		private const NumberStyles SPECIAL =
			NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent
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

		public static ParsingStatus ParseDecStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, NumberFormatInfo formatProvider, out T output, out int charsConsumed)
			where T : struct, IFormattableUnsignedInteger<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			const int UInt64MaxSafeCharacterCount = 19;

			T e = BitHelper.GetPow10<T>(UInt64MaxSafeCharacterCount);
			ulong r;

			if (s.Length <= UInt64MaxSafeCharacterCount)
			{
				if (TChar.TryParsePartialInteger(s, style, formatProvider, out r, out charsConsumed))
				{
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
			if (TChar.TryParsePartialInteger(s[..UInt64MaxSafeCharacterCount], style, formatProvider, out r, out charsConsumed))
			{
				output = T.CreateTruncating(r);
				if (charsConsumed < UInt64MaxSafeCharacterCount)
				{
					return ParsingStatus.Partial;
				}
			}
			else
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Failed;
			}

			int length = s.Length - charsConsumed, consumed;

			do
			{
				if (charsConsumed > length)
				{
					break;
				}
				if (TChar.TryParsePartialInteger(s.Slice(charsConsumed, UInt64MaxSafeCharacterCount), style, formatProvider, out r, out consumed))
				{
					if (consumed < UInt64MaxSafeCharacterCount)
					{
						output *= BitHelper.GetPow10<T>(consumed);
						charsConsumed += consumed;
						output += T.CreateTruncating(r);
						return ParsingStatus.Partial;
					}

					output *= e;
					charsConsumed += UInt64MaxSafeCharacterCount;
					output += T.CreateTruncating(r);
				}
				else
				{
					return ParsingStatus.Partial;
				}
			} while (true);

			length = s.Length - charsConsumed;
			if (length != 0)
			{
				if (TChar.TryParsePartialInteger(s[^length..], style, CultureInfo.CurrentCulture, out r, out consumed))
				{
					output *= T.CreateTruncating(E19Table[length]);
					T addon = output + T.CreateTruncating(r);
					if (addon < output)
					{
						charsConsumed = 0;
						output = default;
						return ParsingStatus.Overflow;
					}
					else
					{
						charsConsumed += consumed;
						output = addon;
					}
				}
			}

			if (charsConsumed > T.MaxDecimalDigits || (charsConsumed == T.MaxDecimalDigits && (uint)s[0] > T.LastDecimalDigitOfMaxValue))
			{
				charsConsumed = 0;
				output = default;
				return ParsingStatus.Overflow;
			}

			return s.Length == charsConsumed ? ParsingStatus.Success : ParsingStatus.Partial;
		}
		public static ParsingStatus ParseStringToUnsigned<TInteger, TChar, TConverter>(ReadOnlySpan<TChar> s, out TInteger output, out int charsConsumed)
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

		public static T ParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
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
		public static bool TryParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
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

		public static ParsingStatus TryParseToUnsignedCore<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output, out int charsConsumed)
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
			
			if ((style & SPECIAL) != 0)
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
				status = ParseDecStringToUnsigned(s[index..], style, formatInfo, out output, out charsConsumed);
			}

			charsConsumed += index;
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

		public static TSigned ParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
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
		public static bool TryParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
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
		public static ParsingStatus TryParseToSignedCore<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output, out int charsConsumed)
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
			
			if ((style & SPECIAL) != 0)
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
			
			status = ParseDecStringToUnsigned(s[index..], style, formatInfo, out result, out charsConsumed);
			
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
			
			charsConsumed += style.HasFlag(NumberStyles.AllowTrailingWhite) ? s[charsConsumed..].IndexOfAnyExcept(TChar.WhiteSpaceCharacter, TChar.NullCharacter) : s[charsConsumed..].IndexOfAnyExcept(TChar.NullCharacter);

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
		#endregion
		#region Float
		/*
		 * Max buffer length for floating point numbers:
		 * Quad: 11563
		 * Octo: 183466
		 */
		internal const int QuadBufferLength = 11563 + 1 + 1; // Max buffer length + 1 for rounding 
		internal const int OctoBufferLength = 183466 + 1 + 1; // Max buffer length + 1 for rounding

		internal static bool TryParseFloat<TFloat, TBits, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, out TFloat result)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (TryParseFloatCore<TFloat, TBits, TChar>(s, styles, provider, out result, out _).IsSuccessful()) return true;
			result = TFloat.Zero;
			return false;
		}

		internal static ParsingStatus TryParseFloatCore<TFloat, TBits, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, out TFloat result, out int charsConsumed)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);
			ParsingStatus status;
			if ((styles & NumberStyles.AllowHexSpecifier) != 0)
			{
				if ((status = TryParseHexFloat<TFloat, TBits, TChar>(s, styles, info, out result, out charsConsumed)).IsSuccessfulOrPartial())
				{
					return status;
				}
			}
			else
			{
				byte[] buffer = ArrayPool<byte>.Shared.Rent(OctoBufferLength);
				NumberInfo number = new NumberInfo(buffer, true);

				if ((status = NumberInfo.TryParseCore(s, ref number, info, styles, out charsConsumed)).IsSuccessfulOrPartial())
				{
					result = NumberInfo.ConvertToFloat<TFloat, TBits>(ref number);
					ArrayPool<byte>.Shared.Return(buffer);
					return status;
				}
			}
			

			ReadOnlySpan<TChar> trim = TChar.TrimStart(s);
			charsConsumed = s.Length - trim.Length;

			Span<TChar> positiveInf = stackalloc TChar[TChar.GetLength(info.PositiveInfinitySymbol)];
			TChar.Copy(info.PositiveInfinitySymbol, positiveInf);

			if (StartsWithTrim(trim, positiveInf, ref charsConsumed))
			{
				result = TFloat.PositiveInfinity;
				return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
			}

			Span<TChar> negativeInf = stackalloc TChar[TChar.GetLength(info.NegativeInfinitySymbol)];
			TChar.Copy(info.NegativeInfinitySymbol, negativeInf);

			if (StartsWithTrim(trim, negativeInf, ref charsConsumed))
			{
				result = TFloat.NegativeInfinity;
				return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
			}

			Span<TChar> nan = stackalloc TChar[TChar.GetLength(info.NaNSymbol)];
			TChar.Copy(info.NaNSymbol, nan);

			if (StartsWithTrim(trim, nan, ref charsConsumed))
			{
				result = TFloat.NaN;
				return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
			}

			Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
			TChar.Copy(info.PositiveSign, positiveSign);

			if (TChar.StartsWith(trim, positiveSign, StringComparison.OrdinalIgnoreCase))
			{
				trim = trim[positiveSign.Length..];
				charsConsumed = s.Length - trim.Length;

				if (StartsWithTrim(trim, positiveInf, ref charsConsumed))
				{
					result = TFloat.PositiveInfinity;
					return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
				}
				else if (StartsWithTrim(trim, nan, ref charsConsumed))
				{
					result = TFloat.NaN;
					return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
				}

				result = TFloat.Zero;
				return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
			}
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			if (TChar.StartsWith(trim, negativeSign, StringComparison.OrdinalIgnoreCase))
			{
				var afterSign = trim[negativeSign.Length..];
				charsConsumed = s.Length - afterSign.Length;
				
				if (StartsWithTrim(afterSign, nan, ref charsConsumed))
				{
					result = TFloat.NaN;
					return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
				}

				if (TChar.StartsWith(trim, [(TChar)'-'], StringComparison.OrdinalIgnoreCase))
				{
					var afterHyphen = trim[1..];
					charsConsumed = s.Length - afterHyphen.Length;
					if (StartsWithTrim(afterHyphen, nan, ref charsConsumed))
					{
						result = TFloat.NaN;
						return charsConsumed == s.Length ? ParsingStatus.Success : ParsingStatus.Partial;
					}
				}
			}

			result = TFloat.Zero;
			return ParsingStatus.Failed; // We really failed

			static bool StartsWithTrim(ReadOnlySpan<TChar> v1,  ReadOnlySpan<TChar> v2, ref int charsConsumed)
			{
				if (!v2.IsEmpty && v2.Length <= v1.Length && TChar.StartsWith(v1, v2, StringComparison.OrdinalIgnoreCase))
				{
					ReadOnlySpan<TChar> trailing = TChar.TrimStart(v1[v2.Length..]);
					charsConsumed += v1.Length - trailing.Length;
					return true;
				}

				return false;
			}
		}

		private static ParsingStatus TryParseHexFloat<TFloat, TBits, TChar>(ReadOnlySpan<TChar> value, NumberStyles styles, NumberFormatInfo info, out TFloat result, out int charsConsumed) 
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// Based on CoreLib implementation: https://github.com/dotnet/runtime/blob/ecc5874fbe7c2f2db3cc7e563bc6e81c7a2c17f6/src/libraries/System.Private.CoreLib/src/System/Number.Parsing.cs#L982
			
			result = TFloat.Zero;

			if (value.IsEmpty)
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}

			int index = 0;

			// Skip leading whitespace
			if ((styles & NumberStyles.AllowLeadingWhite) != 0)
			{
				while (index < value.Length && TChar.IsWhiteSpace(value[index]))
				{
					index++;
				}
			}

			if (index >= value.Length)
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}

			bool isNegative = false;
			if ((styles & NumberStyles.AllowLeadingSign) != 0)
			{
				Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
				TChar.Copy(info.NegativeSign, negativeSign);
				if (!negativeSign.IsEmpty && TChar.StartsWith(value.Slice(index), negativeSign, StringComparison.OrdinalIgnoreCase))
				{
					isNegative = true;
					index += negativeSign.Length;
				}
				else
				{
					Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
					TChar.Copy(info.PositiveSign, positiveSign);
					if (!positiveSign.IsEmpty && TChar.StartsWith(value.Slice(index), positiveSign, StringComparison.OrdinalIgnoreCase))
					{
						index += positiveSign.Length;
					}
				}
			}

			if (index >= value.Length)
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}
			
			// Require "0x" or "0X" prefix (consistent with IEEE 754 conventions)
			if ((uint)value[index] != '0' ||
			    index + 1 >= value.Length ||
			    ((uint)value[index + 1] | 0x20) != 'x')
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}
			index += 2;

			if (index >= value.Length)
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}
			
			// Parse hex significand.
			// We accumulate up to 16 significant hex digits into a TBit.
			// We track the exponent adjustment due to digit position.
			//
			// The value is: significand * 2^(binaryExponent - 4 * fractionalDigitsConsumed + 4 * overflowIntegerDigits)

			TBits significand = TBits.Zero;
			int maxSignificandDigits = Unsafe.SizeOf<TBits>() * 2;
			int significandDigits = 0;       // Count of significant (non-leading-zero) digits consumed into significand
			int overflowIntegerDigits = 0;   // Integer digits that didn't fit
			bool hasDiscardedNonZeroDigits = false;  // IEEE 754 "sticky bit": any nonzero digit discarded beyond significand capacity
			
			int integerPartStart = index;
			while (index < value.Length)
			{
				uint ch = (uint)value[index];
				int digit = FromHexDigit((char)ch);
				if (digit >= 16)
				{
					break;
				}
				
				// Accumulate up to 16 significant hex digits. The '|| significand == 0' is
				// a defensive check: significandDigits only increments when a nonzero digit is
				// accumulated, so significandDigits >= 16 implies significand != 0 in practice.
				if (significandDigits < maxSignificandDigits || significand == TBits.Zero)
				{
					if (significand != TBits.Zero || digit != 0)
					{
						significand = (significand << 4) | TBits.CreateTruncating((uint)digit);
						significandDigits++;
					}
				}
				else
				{
					overflowIntegerDigits++;
					hasDiscardedNonZeroDigits |= digit != 0;
				}

				index++;
			}
			bool hasIntegerPart = index > integerPartStart;
			
			// Parse fractional part
			int fractionalDigitsConsumed = 0;
			bool hasFractionalPart = false;

			if ((styles & NumberStyles.AllowDecimalPoint) != 0 && index < value.Length)
			{
				Span<TChar> decimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
				TChar.Copy(info.NumberDecimalSeparator, decimalSeparator);
				if (TChar.StartsWith(value.Slice(index), decimalSeparator, StringComparison.OrdinalIgnoreCase))
				{
					index += decimalSeparator.Length;

					int fractionalPartStart = index;
					while (index < value.Length)
					{
						uint ch = (uint)(value[index]);
						int digit = FromHexDigit((char)ch);
						if (digit >= 16)
						{
							break;
						}

						// Accumulate significant digits (see integer loop comment for '|| significand == 0').
						// Discarded fractional digits intentionally do NOT increment fractionalDigitsConsumed:
						// they are beyond significand precision and only contribute sticky bits for rounding.
						if (significandDigits < maxSignificandDigits || significand == TBits.Zero)
						{
							if (significand != TBits.Zero || digit != 0)
							{
								significand = (significand << 4) | TBits.CreateTruncating(digit);
								significandDigits++;
							}

							// Always increment, even for leading zeros: positional value matters
							// (e.g., 0x0.004p0 = 4 * 2^-12, so all three fractional digits count).
							fractionalDigitsConsumed++;
						}
						else
						{
							hasDiscardedNonZeroDigits |= digit != 0;
						}

						index++;
					}
					hasFractionalPart = index > fractionalPartStart;
				}
			}
			
			if (!hasIntegerPart && !hasFractionalPart)
			{
				charsConsumed = 0;
				return ParsingStatus.Failed;
			}
			
			// Parse the exponent: 'p' or 'P' followed by optional sign and decimal digits.
			// The decimal value specifies an exponent in the radix of the floating-point format
			// (for binary types, the value is multiplied by 2 raised to this power).
			int binaryExponent = 0;
            if (index < value.Length && (((uint)value[index] | 0x20) == 'p'))
            {
                index++;

                if (index >= value.Length)
                {
	                charsConsumed = 0;
	                return ParsingStatus.Failed;
                }

                bool exponentIsNegative = false;
                Span<TChar> negSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
                TChar.Copy(info.NegativeSign, negSign);
                Span<TChar> posSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
                TChar.Copy(info.PositiveSign, posSign);
                if (!negSign.IsEmpty && TChar.StartsWith(value.Slice(index), negSign, StringComparison.OrdinalIgnoreCase))
                {
                    exponentIsNegative = true;
                    index += negSign.Length;
                }
                else if (!posSign.IsEmpty && TChar.StartsWith(value.Slice(index), posSign, StringComparison.OrdinalIgnoreCase))
                {
                    index += posSign.Length;
                }

                if (index >= value.Length)
                {
	                charsConsumed = 0;
	                return ParsingStatus.Failed;
                }

                int exponentStart = index;
                while (index < value.Length)
                {
                    if (!TChar.IsDigit(value[index]))
                    {
                        break;
                    }

                    int digit = (int)((uint)value[index] - '0');

                    // Saturate at int.MaxValue on overflow. Unlike the significand (which tracks
                    // overflow digits and sticky bits for rounding), the exponent just needs to be
                    // large enough to guarantee the result resolves to infinity or zero.
                    binaryExponent = binaryExponent <= (int.MaxValue - digit) / 10 ?
                        binaryExponent * 10 + digit :
                        int.MaxValue;

                    index++;
                }

                if (index == exponentStart)
                {
	                charsConsumed = 0;
	                return ParsingStatus.Failed;
                }

                if (exponentIsNegative)
                {
                    binaryExponent = -binaryExponent;
                }
            }
            else
            {
                // Exponent indicator (p/P) is required
                charsConsumed = 0;
                return ParsingStatus.Failed;
            }

            // Skip trailing whitespace
            if ((styles & NumberStyles.AllowTrailingWhite) != 0)
            {
                while (index < value.Length && TChar.IsWhiteSpace(value[index]))
                {
                    index++;
                }
            }

            // For compatibility, allow trailing null characters (same as other number parsers).
            if (index != value.Length && !value.Slice(index).ContainsAnyExcept((TChar)'\0'))
            {
	            charsConsumed = 0;
	            return ParsingStatus.Failed;
            }

            // We've successfully parsed a number, so now we just need to handle constructing the result
			charsConsumed = index;
			
            if (significand == TBits.Zero)
            {
                result = isNegative ? TFloat.NegativeZero : TFloat.Zero;
                return index == value.Length ? ParsingStatus.Success : ParsingStatus.Partial;
            }

            // Compute the effective binary exponent.
            // value = significand * 2^(-4 * fractionalDigitsConsumed) * 2^(4 * overflowIntegerDigits) * 2^binaryExponent
            long exp = (long)binaryExponent - 4L * fractionalDigitsConsumed + 4L * overflowIntegerDigits;

            // Normalize: shift significand so MSB is at bit 63
            int lz = int.CreateTruncating(TBits.LeadingZeroCount(significand));
            significand <<= lz;
            exp -= lz;

            // significand is now in [2^63, 2^64), so value = significand * 2^exp
            // = (significand / 2^63) * 2^(exp + 63) = 1.xxx * 2^(exp + 63)
            long actualExp = exp + (Unsafe.SizeOf<TBits>() * 8 - 1);

            int mantissaBits = TFloat.DenormalMantissaBits;

            if (actualExp > TFloat.MaximumBinaryExponent)
            {
                result = isNegative ? TFloat.NegativeInfinity : TFloat.PositiveInfinity;
                return index == value.Length ? ParsingStatus.Success : ParsingStatus.Partial;
            }

            int shiftRight = (Unsafe.SizeOf<TBits>() * 8 - 1) - mantissaBits;
            long biasedExp = actualExp + TFloat.ExponentBias;

            if (biasedExp <= 0)
            {
                long denormalShift = 1L - biasedExp;
                if (denormalShift > (Unsafe.SizeOf<TBits>() * 8) - shiftRight)
                {
                    // Value is too small to round to min subnormal
                    result = isNegative ? TFloat.NegativeZero : TFloat.Zero;
                    return index == value.Length ? ParsingStatus.Success : ParsingStatus.Partial;
                }
                shiftRight += (int)denormalShift;
                biasedExp = 0;
            }

            // Round to nearest, ties to even
            TBits mantissa = TBits.Zero;
            if (shiftRight > 0 && shiftRight < (Unsafe.SizeOf<TBits>() * 8))
            {
	            TBits roundBit = TBits.One << (shiftRight - 1);
	            TBits stickyBits = (significand & (roundBit - TBits.One)) | (hasDiscardedNonZeroDigits ? TBits.One : TBits.Zero);
                mantissa = significand >> shiftRight;

                if ((significand & roundBit) != TBits.Zero && (stickyBits != TBits.Zero || (mantissa & TBits.One) != TBits.Zero))
                {
                    mantissa++;

                    if (biasedExp == 0 && mantissa > TFloat.DenormalMantissaMask)
                    {
                        biasedExp = 1;
                        mantissa &= TFloat.DenormalMantissaMask;
                    }
                    else if (mantissa > ((TBits.One << (mantissaBits + 1)) - TBits.One))
                    {
                        mantissa >>= 1;
                        biasedExp++;
                        if (biasedExp >= TFloat.InfinityExponent)
                        {
                            result = isNegative ? TFloat.NegativeInfinity : TFloat.PositiveInfinity;
                            return index == value.Length ? ParsingStatus.Success : ParsingStatus.Partial;
                        }
                    }
                }
            }
            else if (shiftRight == (Unsafe.SizeOf<TBits>() * 8))
            {
                TBits roundBit = TBits.One << (Unsafe.SizeOf<TBits>() * 8 - 1);
                TBits stickyBits = (significand & (roundBit - TBits.One)) | (hasDiscardedNonZeroDigits ? TBits.One : TBits.Zero);
                mantissa = TBits.Zero;

                // mantissa is 0 (even), so ties-to-even rounds up only when sticky bits are nonzero.
                if ((significand & roundBit) != TBits.Zero && stickyBits != TBits.Zero)
                {
                    mantissa = TBits.One;
                    if (mantissa > TFloat.DenormalMantissaMask)
                    {
                        biasedExp = 1;
                        mantissa &= TFloat.DenormalMantissaMask;
                    }
                }
            }

            mantissa &= TFloat.DenormalMantissaMask;

            TBits bits = (TBits.CreateTruncating((ulong)biasedExp) << mantissaBits) | mantissa;
            result = TFloat.BitsToFloat(bits);
            if (isNegative)
            {
                result = -result;
            }

            return index == value.Length ? ParsingStatus.Success : ParsingStatus.Partial;

			static int FromHexDigit(char value)
			{
				if (char.IsDigit(value))
				{
					return (int)CharUnicodeInfo.GetDecimalDigitValue(value);
				}
				else if (char.IsAsciiHexDigit(value))
				{
					return char.ToLowerInvariant(value) - 'W'; // 'W' = 87
				}

				return value;
			}
		}
		#endregion
	}
}
