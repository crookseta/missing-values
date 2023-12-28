using MissingValues.Internals;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace MissingValues
{
	internal static class NumberParser
	{
		#region IFormattableNumber
		public static T? ParseDecStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxDecimalDigits && ((char)s[0]) > T.LastDecimalDigitOfMaxValue)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetDecimalValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Ten);
				var addon = T.GetDecimalValue((char)s[^i]) * acumulator;
				if ((T.MaxValue - output) < addon)
				{
					return null;
				}
				output += addon;
			}

			return output;
		}
		public static T? ParseBinStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxBinaryDigits)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetDecimalValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Two);
				output += T.GetDecimalValue((char)s[^i]) * acumulator;
			}

			return output;
		}
		public static T? ParseHexStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxHexDigits)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetHexValue((char)s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Sixteen);
				output += T.GetHexValue((char)s[^i]) * acumulator;
			}

			return output;
		}

		public static T ParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[0]))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidLeadingWhiteSpace);
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidTrailingWhiteSpace);
			}

			if (ContainsInvalidCharacter(s, style))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidCharacter);
			}

			T acumulator = T.One;
			T? result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				result = ParseHexStringToUnsigned<T, TChar>(s);

				if (result is null)
				{
					Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				return (T)result;
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				result = ParseBinStringToUnsigned<T, TChar>(s);

				if (result is null)
				{
					Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				return (T)result;
			}
#endif

			result = ParseDecStringToUnsigned<T, TChar>(s);

			if (result is null)
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
			}

			return (T)result;
		}

		public static bool TryParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
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
				return false;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				output = default;
				return false;
			}

			if (ContainsInvalidCharacter(s, style))
			{
				output = default;
				return false;
			}
			T? result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				result = ParseHexStringToUnsigned<T, TChar>(s);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				result = ParseBinStringToUnsigned<T, TChar>(s);
			}
#endif
			else
			{
				result = ParseDecStringToUnsigned<T, TChar>(s);
			}

			if (result is null)
			{
				output = default;
				return false;
			}
			output = (T)result;
			return true;
		}

		public static TSigned ParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider)
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
				Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.InvalidLeadingWhiteSpace);
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.InvalidTrailingWhiteSpace);
			}

			bool isNegative;
			ReadOnlySpan<TChar> raw;
			if (style.HasFlag(NumberStyles.AllowParentheses) && s.IndexOf((TChar)'(') > -1 && s.IndexOf((TChar)')') > 1)
			{
				isNegative = true;
				raw = s[1..^1];
			}
			else
			{
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && s.IndexOf(TChar.NegativeSign(NumberFormatInfo.CurrentInfo)) == 0;
				raw = isNegative ? s[1..] : s;
			}

			if (ContainsInvalidCharacter(raw, style))
			{
				Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.InvalidCharacter);
			}

			TUnsigned result, acumulator;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				if (raw.Length > TUnsigned.MaxHexDigits)
				{
					Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				acumulator = TUnsigned.One;
				result = TUnsigned.GetHexValue((char)raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Sixteen);
					result += TUnsigned.GetHexValue((char)raw[^i]) * acumulator;
				}
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				if (raw.Length > TUnsigned.MaxBinaryDigits)
				{
					Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				acumulator = TUnsigned.One;
				result = TUnsigned.GetDecimalValue((char)raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Two);
					result += TUnsigned.GetDecimalValue((char)raw[^i]) * acumulator;
				}
			}
#endif
			else
			{
				if (s.Length > TUnsigned.MaxDecimalDigits && (char)raw[0] > TUnsigned.LastDecimalDigitOfMaxValue)
				{
					Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				acumulator = TUnsigned.One;
				result = TUnsigned.GetDecimalValue((char)raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Ten);
					var addon = TUnsigned.GetDecimalValue((char)raw[^i]) * acumulator;
					if ((TUnsigned.MaxValue - result) < addon)
					{
						Thrower.ParsingError<TSigned>(raw.ToString(), Thrower.ParsingErrorType.ValueTooBig);
					}
					result += addon;
				}
			}

			TSigned output = result.ToSigned();

			if (isNegative)
			{
				output = -output;
			}

			return output;
		}

		public static bool TryParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
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
				return false;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}
			else if (TChar.IsWhiteSpace(s[^1]))
			{
				output = default;
				return false;
			}

			bool isNegative;
			ReadOnlySpan<TChar> raw;
			if (style.HasFlag(NumberStyles.AllowParentheses) && s.IndexOf((TChar)'(') > -1 && s.IndexOf((TChar)')') > 1)
			{
				isNegative = true;
				raw = s[1..^1];
			}
			else
			{
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && s.IndexOf(TChar.NegativeSign(NumberFormatInfo.CurrentInfo)) == 0;
				raw = isNegative ? s[1..] : s;
			}

			if (ContainsInvalidCharacter(raw, style))
			{
				output = default;
				return false;
			}

			TUnsigned? result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				result = ParseHexStringToUnsigned<TUnsigned, TChar>(raw);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				result = ParseBinStringToUnsigned<TUnsigned, TChar>(raw);
			}
#endif
			else
			{
				result = ParseDecStringToUnsigned<TUnsigned, TChar>(raw);
			}
			if (result is null)
			{
				output = default;
				return false;
			}
			output = result.Value.ToSigned();

			if (isNegative)
			{
				output = -output;
			}

			return true;
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

				ReadOnlySpan<TChar> infinity = TChar.PositiveInfinitySymbol(info);

				if (TChar.Equals(trim, infinity, StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.PositiveInfinity;
					return true;
				}

				if (TChar.Equals(trim, TChar.NegativeInfinitySymbol(info), StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.NegativeInfinity;
					return true;
				}

				ReadOnlySpan<TChar> nan = TChar.NaNSymbol(info);

				if (TChar.Equals(trim, nan, StringComparison.OrdinalIgnoreCase))
				{
					result = TFloat.NaN;
					return true;
				}

				ReadOnlySpan<TChar> positiveSign = TChar.PositiveSign(info);

				if (TChar.StartsWith(trim, positiveSign, StringComparison.OrdinalIgnoreCase))
				{
					trim = trim.Slice(positiveSign.Length);

					if (TChar.Equals(trim, infinity, StringComparison.OrdinalIgnoreCase))
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
				ReadOnlySpan<TChar> negativeSign = TChar.NegativeSign(info);

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
