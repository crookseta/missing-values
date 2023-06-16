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

		public static bool TryParseToUnsigned<T>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
			where T : struct, IFormattableInteger<T>, IMinMaxValue<T>, IUnsignedNumber<T>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
			}
			if (style.HasFlag(NumberStyles.AllowParentheses))
			{
				s = s[1..^1];
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

		public static bool TryParseToSigned<TSigned, TUnsigned>(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>, IMinMaxValue<TUnsigned>
		{
			if (style.HasFlag(NumberStyles.AllowLeadingWhite))
			{
				s = s.TrimStart();
			}
			if (style.HasFlag(NumberStyles.AllowTrailingWhite))
			{
				s = s.TrimEnd();
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
