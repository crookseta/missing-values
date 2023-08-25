using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static class NumberParser
	{
		#region IFormattableNumber
		public static T? ParseDecStringToUnsigned<T>(ReadOnlySpan<char> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (s.Length > T.MaxDecimalDigits && s[0] > T.LastDecimalDigitOfMaxValue)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetDecimalValue(s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Ten);
				var addon = T.GetDecimalValue(s[^i]) * acumulator;
				if ((T.MaxValue - output) < addon)
				{
					return null;
				}
				output += addon;
			}

			return output;
		}
		public static T? ParseBinStringToUnsigned<T>(ReadOnlySpan<char> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (s.Length > T.MaxBinaryDigits)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetDecimalValue(s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Two);
				output += T.GetDecimalValue(s[^i]) * acumulator;
			}

			return output;
		}
		public static T? ParseHexStringToUnsigned<T>(ReadOnlySpan<char> s)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (s.Length > T.MaxHexDigits)
			{
				return null;
			}

			T acumulator = T.One;
			T output = T.GetHexValue(s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Sixteen);
				output += T.GetHexValue(s[^i]) * acumulator;
			}

			return output;
		}

		public static T ParseToUnsigned<T>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			else if (char.IsWhiteSpace(s[0]))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidLeadingWhiteSpace);
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
			}
			else if (char.IsWhiteSpace(s[^1]))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidTrailingWhiteSpace);
			}

			if (ContainsInvalidCharacter(s, style))
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.InvalidCharacter);
			}

			T acumulator = T.One;
			T result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				if (s.Length > T.MaxHexDigits)
				{
					Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				result = T.GetHexValue(s[^1]);

				for (int i = 2; i <= s.Length; i++)
				{
					acumulator = unchecked(acumulator * T.Sixteen);
					result += T.GetHexValue(s[^i]) * acumulator;
				}

				return result;
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				if (s.Length > T.MaxBinaryDigits)
				{
					Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				acumulator = T.One;
				result = T.GetDecimalValue(s[^1]);

				for (int i = 2; i <= s.Length; i++)
				{
					acumulator = unchecked(acumulator * T.Two);
					result += T.GetDecimalValue(s[^i]) * acumulator;
				}
			} 
#endif

			if (s.Length > T.MaxDecimalDigits && s[0] > T.LastDecimalDigitOfMaxValue)
			{
				Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
			}

			result = T.GetDecimalValue(s[^1]);

			for (int i = 2; i <= s.Length; i++)
			{
				acumulator = unchecked(acumulator * T.Ten);
				var addon = T.GetDecimalValue(s[^i]) * acumulator;
				if ((T.MaxValue - result) < addon)
				{
					Thrower.ParsingError<T>(s.ToString(), Thrower.ParsingErrorType.ValueTooBig);
				}
				result += addon;
			}

			return result;
		}

		public static bool TryParseToUnsigned<T>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			else if (char.IsWhiteSpace(s[0]))
			{
				output = default;
				return false;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
			}
			else if (char.IsWhiteSpace(s[^1]))
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
				result = ParseHexStringToUnsigned<T>(s);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				result = ParseBinStringToUnsigned<T>(s);
			} 
#endif
			else
			{
				result = ParseDecStringToUnsigned<T>(s);
			}

			if (result is null)
			{
				output = default;
				return false;
			}
			output = (T)result;
			return true;
		}

		public static TSigned ParseToSigned<TSigned, TUnsigned>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>, IMinMaxValue<TUnsigned>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			else if (char.IsWhiteSpace(s[0]))
			{
				Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.InvalidLeadingWhiteSpace);
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
			}
			else if (char.IsWhiteSpace(s[^1]))
			{
				Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.InvalidTrailingWhiteSpace);
			}

			bool isNegative;
			ReadOnlySpan<char> raw;
			if (style.HasFlag(NumberStyles.AllowParentheses) && s.IndexOf('(') > -1 && s.IndexOf(')') > 1)
			{
				isNegative = true;
				raw = s[1..^1];
			}
			else
			{
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && s.IndexOf(NumberFormatInfo.CurrentInfo.NegativeSign) == 0;
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
				result = TUnsigned.GetHexValue(raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Sixteen);
					result += TUnsigned.GetHexValue(raw[^i]) * acumulator;
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
				result = TUnsigned.GetDecimalValue(raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Two);
					result += TUnsigned.GetDecimalValue(raw[^i]) * acumulator;
				}
			} 
#endif
			else
			{
				if (s.Length > TUnsigned.MaxDecimalDigits && raw[0] > TUnsigned.LastDecimalDigitOfMaxValue)
				{
					Thrower.ParsingError<TSigned>(s.ToString(), Thrower.ParsingErrorType.StringTooBig);
				}

				acumulator = TUnsigned.One;
				result = TUnsigned.GetDecimalValue(raw[^1]);

				for (int i = 2; i <= raw.Length; i++)
				{
					acumulator = unchecked(acumulator * TUnsigned.Ten);
					var addon = TUnsigned.GetDecimalValue(raw[^i]) * acumulator;
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

		public static bool TryParseToSigned<TSigned, TUnsigned>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>, IMinMaxValue<TUnsigned>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			else if (char.IsWhiteSpace(s[0]))
			{
				output = default;
				return false;
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
			}
			else if (char.IsWhiteSpace(s[^1]))
			{
				output = default;
				return false;
			}

			bool isNegative;
			ReadOnlySpan<char> raw;
			if (style.HasFlag(NumberStyles.AllowParentheses) && s.IndexOf('(') > -1 && s.IndexOf(')') > 1)
			{
				isNegative = true;
				raw = s[1..^1];
			}
			else
			{
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && s.IndexOf(NumberFormatInfo.CurrentInfo.NegativeSign) == 0;
				raw = isNegative ? s[1..] : s;
			}

			if(ContainsInvalidCharacter(raw, style))
			{
				output = default;
				return false;
			}

			TUnsigned? result;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				result = ParseHexStringToUnsigned<TUnsigned>(raw);
			}
#if NET8_0_OR_GREATER
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				result = ParseBinStringToUnsigned<TUnsigned>(raw);
			} 
#endif
			else
			{
				result = ParseDecStringToUnsigned<TUnsigned>(raw);
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

		private static bool ContainsInvalidCharacter(ReadOnlySpan<char> s, NumberStyles style)
		{
			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				foreach (var item in s)
				{
					if (!char.IsAsciiHexDigit(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (var item in s)
			{
				if (!char.IsDigit(item))
				{
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}
