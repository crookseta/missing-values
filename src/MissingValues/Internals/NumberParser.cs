using MissingValues.Internals;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MissingValues
{
	internal static class NumberParser
	{
		internal readonly struct ParsingStatus
		{
			internal const int Success = 0;
			internal const int Failed = 1;
			internal const int Overflow = 2;
			internal const int Underflow = 3;

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
					Overflow => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is bigger than {T.MaxValue}"),
					Underflow => new OverflowException($"Could not parse '{input}' as {typeof(T)}.\nThe input is smaller than {T.MinValue}"),
					_ => new FormatException($"Could not parse '{input}' as {typeof(T)}.\n"),
				};
			}

			public static implicit operator ParsingStatus(int value) => new(value);
			public static implicit operator bool(ParsingStatus self) => self._status == Success;
        }

		#region Integer
		public static ParsingStatus ParseDecStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxDecimalDigits && ((char)s[0]) > T.LastDecimalDigitOfMaxValue)
			{
				output = default;
				return ParsingStatus.Overflow;
			}

			T acumulator = T.One;
			output = T.GetDecimalValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Ten);
				var addon = T.GetDecimalValue((char)s[^i]) * acumulator;
				if ((T.MaxValue - output) < addon)
				{
					output = default;
					return ParsingStatus.Overflow;
				}
				output += addon;
			}

			return ParsingStatus.Success;
		}
		public static ParsingStatus ParseBinStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxBinaryDigits || s.IndexOfAnyExcept((TChar)'0', (TChar)'1') > -1)
			{
				output = default;
				return ParsingStatus.Overflow;
			}

			T acumulator = T.One;
			output = T.GetDecimalValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Two);
				output += T.GetDecimalValue((char)s[^i]) * acumulator;
			}

			return ParsingStatus.Success;
		}
		public static ParsingStatus ParseHexStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxHexDigits)
			{
				output = default;
				return ParsingStatus.Overflow;
			}

			T acumulator = T.One;
			output = T.GetHexValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Sixteen);
				output += T.GetHexValue((char)s[^i]) * acumulator;
			}

			return ParsingStatus.Success;
		}

		public static ParsingStatus TryParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[0]))
			{
				output = default;
				return ParsingStatus.Failed;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				output = default;
				return ParsingStatus.Failed;
			}
			if (s[0] == (TChar)'-')
			{
				output = default;
				return ParsingStatus.Underflow;
			}

			if (ContainsInvalidCharacter(s, style))
			{
				output = default;
				return ParsingStatus.Failed;
			}

			ParsingStatus status = ParsingStatus.Success;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				status = ParseHexStringToUnsigned(s, out output);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				status = ParseBinStringToUnsigned(s, out output);
			}
#endif
			else
			{
				status = ParseDecStringToUnsigned(s, out output);
			}

			return status;
		}

		public static ParsingStatus TryParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>, IMinMaxValue<TUnsigned>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[0]))
			{
				output = default;
				return ParsingStatus.Failed;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				output = default;
				return ParsingStatus.Failed;
			}

			NumberFormatInfo formatInfo = NumberFormatInfo.GetInstance(formatProvider);
			bool isNegative;
			ReadOnlySpan<TChar> raw;
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
			TChar.Copy(formatInfo.NegativeSign, negativeSign);

			if (style.HasFlag(NumberStyles.AllowParentheses) && s.IndexOf((TChar)'(') > -1 && s.IndexOf((TChar)')') > 1)
			{
				isNegative = true;
				raw = s[1..^1];
			}
			else
			{
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && s.IndexOf(negativeSign) == 0;
				raw = isNegative ? s[1..] : s;
			}

			if (ContainsInvalidCharacter(raw, style))
			{
				output = default;
				return ParsingStatus.Failed;
			}

			ParsingStatus status;
			TUnsigned result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				status = ParseHexStringToUnsigned(raw, out result);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				status = ParseBinStringToUnsigned(raw, out result);
			}
#endif
			else
			{
				status = ParseDecStringToUnsigned(raw, out result);
			}

			output = result.ToSigned();

			if (isNegative)
			{
				output = -output;
			}

			return status;
		}

		private static bool ContainsInvalidCharacter<TChar>(ReadOnlySpan<TChar> s, NumberStyles style)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				foreach (var item in s)
				{
					if (!TChar.IsHexDigit(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (var item in s)
			{
				if (!TChar.IsDigit(item))
				{
					return true;
				}
			}
			return false;
		}
		#endregion
		#region Float
		const int FloatBufferLength = 11563 + 1 + 1;

		public static unsafe bool TryParseFloat<TFloat, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TFloat result)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			byte[] buffer = ArrayPool<byte>.Shared.Rent(FloatBufferLength);
			FloatInfo number = new FloatInfo(buffer);
			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);


			if (!FloatInfo.TryParse(s, ref number, info, styles))
			{
				ReadOnlySpan<TChar> trim = s.Trim(TChar.WhiteSpaceCharacter);

				Span<TChar> positiveInf = stackalloc TChar[TChar.GetLength(info.PositiveInfinitySymbol)];
				TChar.Copy(info.PositiveInfinitySymbol, positiveInf);
				
				if (TChar.Equals(trim, positiveInf, StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.PositiveInfinity;
					return true;
				}

				Span<TChar> negativeInf = stackalloc TChar[TChar.GetLength(info.NegativeInfinitySymbol)];
				TChar.Copy(info.NegativeInfinitySymbol, negativeInf);

				if (TChar.Equals(trim, negativeInf, StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.NegativeInfinity;
					return true;
				}

				Span<TChar> nan = stackalloc TChar[TChar.GetLength(info.NaNSymbol)];
				TChar.Copy(info.NaNSymbol, nan);

				if (TChar.Equals(trim, nan, StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.NaN;
					return true;
				}

				Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
				TChar.Copy(info.PositiveSign, positiveSign);

				if (TChar.StartsWith(trim, positiveSign, StringComparison.OrdinalIgnoreCase))
				{
					trim = trim.Slice(positiveSign.Length);

					if (TChar.Equals(trim, positiveInf, StringComparison.OrdinalIgnoreCase))
					{
						result = TFloat.PositiveInfinity;
						return true;
					}
					else if (TChar.Equals(trim, nan, StringComparison.OrdinalIgnoreCase))
					{
						result = TFloat.NaN;
						return true;
					}

					result = TFloat.Zero;
					return false;
				}
				Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
				TChar.Copy(info.NegativeSign, negativeSign);

				if (TChar.StartsWith(trim, negativeSign, StringComparison.OrdinalIgnoreCase))
				{
					if (TChar.Equals(trim[negativeSign.Length..], nan, StringComparison.OrdinalIgnoreCase))
					{
						result = TFloat.NaN;
						return true;
					}

					if (TChar.StartsWith(trim, negativeSign, StringComparison.OrdinalIgnoreCase))
					{
						result = TFloat.NaN;
						return true;
					}
				}

				result = TFloat.Zero;
				return false; // We really failed
			}

			result = FloatInfo.ConvertToFloat<TFloat>(ref number);
			ArrayPool<byte>.Shared.Return(buffer);
			return true;
		}
		#endregion
	}
}
