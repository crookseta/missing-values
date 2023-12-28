using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static partial class NumberFormatter
	{
		#region IFormattableNumber
		public static string UnsignedNumberToString<T>(in T value, T numberBase)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int digits = BitHelper.CountDigits(value, numberBase);

			return string.Create(digits, value, (chars, num) =>
			{
				ReadOnlySpan<char> numbers = stackalloc char[]
				{
					'0',
					'1',
					'2',
					'3',
					'4',
					'5',
					'6',
					'7',
					'8',
					'9',
					'A',
					'B',
					'C',
					'D',
					'E',
					'F'
				};
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					(num, var rem) = T.DivRem(num, numberBase);
					chars[i] = numbers[rem.ToInt32()];
				}
			});
		}
		public static string UnsignedNumberToString<T>(in T value, T numberBase, int digits)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			digits = int.Max(BitHelper.CountDigits(value, numberBase), digits);

			return string.Create(digits, value, (chars, num) =>
			{
				ReadOnlySpan<char> numbers = stackalloc char[]
				{
					'0',
					'1',
					'2',
					'3',
					'4',
					'5',
					'6',
					'7',
					'8',
					'9',
					'A',
					'B',
					'C',
					'D',
					'E',
					'F'
				};
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					(num, var rem) = T.DivRem(num, numberBase);
					chars[i] = numbers[rem.ToInt32()];
				}
			});
		}
		public static void UnsignedNumberToCharSpan<T>(T value, in T numberBase, Span<char> destination)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			ReadOnlySpan<char> numbers = stackalloc char[]
			{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F'
			};

			for (int i = destination.Length - 1; i >= 0; i--)
			{
				(value, var rem) = T.DivRem(value, numberBase);
				destination[i] = numbers[rem.ToInt32()];
			}
		}
		public static void UnsignedNumberToCharSpan<T>(T value, in T numberBase, int digits, Span<char> destination)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			ReadOnlySpan<char> numbers = stackalloc char[]
			{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F'
			};

			for (int i = digits - 1; i >= 0; i--)
			{
				(value, var rem) = T.DivRem(value, numberBase);
				destination[i] = numbers[rem.ToInt32()];
			}
		}
		public static void UnsignedNumberToCharSpan<TNumber, TChar>(TNumber value, in TNumber numberBase, int digits, Span<TChar> destination)
			where TNumber : struct, IFormattableInteger<TNumber>, IUnsignedNumber<TNumber>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			var numbers = TChar.Digits;

            for (int i = digits - 1; i >= 0; i--)
            {
				(value, var rem) = TNumber.DivRem(value, numberBase);
				destination[i] = numbers[rem.ToInt32()];
			}
        }

		public static string UnsignedNumberToDecimalString<T>(in T value)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int digits = BitHelper.CountDigits<T>(value, T.Ten);

			return string.Create(digits, value, (chars, num) =>
			{
				const string DigitTable = 
					"0001020304050607080910111213141516171819" +
					"2021222324252627282930313233343536373839" +
					"4041424344454647484950515253545556575859" +
					"6061626364656667686970717273747576777879" +
					"8081828384858687888990919293949596979899";

				int next = chars.Length - 1;
				while (num >= T.TenPow2)
				{
					(num, var rem) = T.DivRem(num, T.TenPow2);
					int i = rem.ToInt32() * 2;
					chars[next] = DigitTable[i + 1];
					chars[next - 1] = DigitTable[i];
					next -= 2;
				}

				if (num < T.Ten)
				{
					chars[next] = (char)('0' + num.ToChar());
				}
				else
				{
					int i = num.ToInt32() * 2;
					chars[next] = DigitTable[i + 1];
					chars[next - 1] = DigitTable[i];
				}
			});
		}
		public static string FormatUnsignedNumber<T>(in T value, string? format, NumberStyles style, IFormatProvider? formatProvider)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int precision = 0;
			if (format is not null && format.Length != 1 && !int.TryParse(format[1..], out precision))
			{
				Thrower.InvalidFormat(format);
			}

			char fmt;
			bool isUpper = false;

			if (format is null)
			{
				fmt = 'd';
			}
			else
			{
				fmt = char.ToLowerInvariant(format[0]);
				isUpper = char.IsUpper(format[0]);
			}

			T fmtBase = fmt switch
			{
				'b' => T.Two,
				'x' => T.Sixteen,
				_ => T.Ten,
			};

			if (fmt != 'd')
			{
				precision = int.Max(precision, BitHelper.CountDigits(value, fmtBase));
			}

			if (isUpper)
			{
				return UnsignedNumberToString(in value, fmtBase, precision).ToUpper();
			}

			return UnsignedNumberToString(in value, fmtBase, precision);
		}
		public static bool TryFormatUnsignedNumber<TNumber, TChar>(in TNumber value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			where TNumber : struct, IFormattableInteger<TNumber>, IUnsignedNumber<TNumber>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			int precision = 0;
			if (format.Length > 1 && !int.TryParse(format[1..], out precision))
			{
				Thrower.InvalidFormat(format.ToString());
			}

			bool isUpper = false;
			char fmt;
			if (format.Length < 1)
			{
				fmt = 'd';
			}
			else
			{
				fmt = char.ToLowerInvariant(format[0]);
				isUpper = char.IsUpper(format[0]);
			}

			TNumber fmtBase = fmt switch
			{
				'b' => TNumber.Two,
				'x' => TNumber.Sixteen,
				_ => TNumber.Ten,
			};

			if (fmtBase != TNumber.Ten)
			{
				precision = int.Max(precision, BitHelper.CountDigits(value, fmtBase));
			}
			else
			{
				precision = BitHelper.CountDigits(value, fmtBase);
			}

			Span<TChar> output = stackalloc TChar[precision];
			UnsignedNumberToCharSpan(value, in fmtBase, precision, output);

			if (isUpper)
			{
				for (int i = 0; i < output.Length; i++)
				{
					output[i] = TChar.ToUpper(output[i]);
				}
			}

			bool success = output.TryCopyTo(destination);

			charsWritten = success ? precision : 0;
			return success;
		}

		public static string SignedNumberToDecimalString<TSigned, TUnsigned>(in TSigned value)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
		{
			if (TSigned.IsNegative(value))
			{
				if (value == TSigned.MinValue)
				{
					return "-" + UnsignedNumberToString(TSigned.MaxValue.ToUnsigned() + TUnsigned.One, TUnsigned.Ten);
				}
				return "-" + UnsignedNumberToDecimalString(TSigned.Abs(value).ToUnsigned());
			}
			else
			{
				return UnsignedNumberToDecimalString(value.ToUnsigned());
			}
		}

		public static string FormatSignedNumber<TSigned, TUnsigned>(in TSigned value, string? format, NumberStyles style, IFormatProvider? formatProvider)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		{
			int precision = 0;
			if (format is not null && format.Length != 1 && !int.TryParse(format[1..], out precision))
			{
				Thrower.InvalidFormat(format);
			}

			char fmt;
			bool isUpper = false;

			if (format is null)
			{
				fmt = 'd';
			}
			else
			{
				isUpper = char.IsUpper(format[0]);
				fmt = char.ToLowerInvariant(format[0]);
			}

			string output = fmt switch
			{
				'b' => UnsignedNumberToString(value.ToUnsigned(), TUnsigned.Two, precision),
				'x' => UnsignedNumberToString(value.ToUnsigned(), TUnsigned.Sixteen, precision),
				_ => ToDecimal(value),
			};

			if (isUpper)
			{
				output = output.ToUpper();
			}

			return output;

			string ToDecimal(TSigned v)
			{
				return SignedNumberToDecimalString<TSigned, TUnsigned>(in v);
			}
		}

		public static bool TryFormatSignedNumber<TSigned, TUnsigned, TChar>(in TSigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			int precision = 0;
			if (format.Length > 1 && !int.TryParse(format[1..], out precision))
			{
				Thrower.InvalidFormat(format.ToString());
			}

			bool isUpper = false;
			char fmt;
			if (format.Length < 1)
			{
				fmt = 'd';
			}
			else
			{
				fmt = char.ToLowerInvariant(format[0]);
				isUpper = char.IsUpper(format[0]);
			}

			TSigned fmtBase = fmt switch
			{
				'b' => TSigned.Two,
				'x' => TSigned.Sixteen,
				_ => TSigned.Ten,
			};


			precision = int.Max(precision, BitHelper.CountDigits(value, fmtBase));
			bool isNegative = TSigned.IsNegative(value);
			var v = value;

			if (fmtBase != TSigned.Ten)
			{
				isUpper = char.IsUpper(format[0]);
			}
			else if (isNegative)
			{
				++precision;
				v = value == TSigned.MinValue ? (TSigned.MaxValue + TSigned.One) : TSigned.Abs(value);
			}

			Span<TChar> output = stackalloc TChar[precision];
			UnsignedNumberToCharSpan(v.ToUnsigned(), fmtBase.ToUnsigned(), precision, output);

			if (isUpper)
			{
				for (int i = 0; i < output.Length; i++)
				{
					output[i] = TChar.ToUpper(output[i]);
				}
			}

			if (isNegative && fmtBase == TSigned.Ten)
			{
				TChar.NegativeSign(NumberFormatInfo.GetInstance(provider)).CopyTo(output[..1]);
			}

			bool success = output.TryCopyTo(destination);


			charsWritten = success ? precision : 0;
			return success;
		}
		#endregion
		#region Float
		public static string FloatToString<TFloat>(
			in TFloat value, 
			ReadOnlySpan<char> format, 
			IFormatProvider? provider)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
		{
			int maxSignificandPrecision = TFloat.MaxSignificandPrecision;
			int maxBufferAlloc = maxSignificandPrecision + 4 + 4 + 4; // N significant decimal digits precision, 4 possible special symbols, 4 exponent decimal digits

			int precision;

			if (format.IsEmpty)
			{
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

				if (!(format.Contains("F", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("G", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("N", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("E", StringComparison.OrdinalIgnoreCase)))
				{
					Thrower.NotSupported();
				}
			}

			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

			Span<Utf16Char> buffer = stackalloc Utf16Char[maxBufferAlloc]; 
			Ryu.Format(in value, buffer, out _, out bool isExceptional, info, precision);

			if (isExceptional || format.Contains("E", StringComparison.OrdinalIgnoreCase))
			{
				return new string(Utf16Char.CastToCharSpan(buffer).TrimEnd('\0'));
			}

			return new string(Utf16Char.CastToCharSpan(GetGeneralFromScientificFloatChars(buffer, info, precision)));
		}

		public static bool TryFormatFloat<TFloat, TChar>(
			in TFloat value,
			Span<TChar> destination,
			out int charsWritten,
			ReadOnlySpan<char> format,
			IFormatProvider? provider)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			int maxSignificandPrecision = TFloat.MaxSignificandPrecision;
			int MaxBufferAlloc = maxSignificandPrecision + 4 + 4 + 4; // 33 significant decimal digits precision, 4 possible special symbols, 4 exponent decimal digits

			int precision;

			if (format.IsEmpty)
			{
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

				if (!(format.Contains("F", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("G", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("N", StringComparison.OrdinalIgnoreCase)
				|| format.Contains("E", StringComparison.OrdinalIgnoreCase)))
				{
					Thrower.NotSupported();
				}
			}

			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

			Span<TChar> buffer = stackalloc TChar[MaxBufferAlloc];
			Ryu.Format(in value, buffer, out charsWritten, out bool isExceptional, info, precision);

			if (isExceptional || format.Contains("E", StringComparison.OrdinalIgnoreCase))
			{
				return buffer.TrimEnd(TChar.NullCharacter).TryCopyTo(destination);
			}

			ReadOnlySpan<TChar> general = GetGeneralFromScientificFloatChars(buffer, info, precision);
			charsWritten = general.Length;
			return general.TryCopyTo(destination);
		}
		private static ReadOnlySpan<TChar> GetGeneralFromScientificFloatChars<TChar>(Span<TChar> buffer, NumberFormatInfo info, int precision)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			const int MaxSignificandPrecision = 33;

			Span<TChar> actualValue = buffer.TrimEnd(TChar.NullCharacter);

			int eIndex = actualValue.IndexOf((TChar)'E');
			if (eIndex <= 0 || !TChar.TryParseInteger(actualValue[(eIndex + 1)..], out int exponent))
			{
				exponent = 0;
			}

			ReadOnlySpan<TChar> numberDecimalSeparator = TChar.NumberDecimalSeparator(info);
			ReadOnlySpan<TChar> negativeSign = TChar.NegativeSign(info);

			bool isNegativeExponent = exponent < 0;
			bool isNegative = buffer.IndexOf(negativeSign) == 0;
			int dotIndex = buffer.IndexOf(numberDecimalSeparator);
			bool containsDecimalSeparator = dotIndex >= 0;

			// If buffer cannot be represented with precision.
			if ((!isNegativeExponent && (containsDecimalSeparator && exponent >= actualValue[(dotIndex + 1)..eIndex].Length && MaxSignificandPrecision < actualValue.Length)) ||
				(isNegativeExponent && (containsDecimalSeparator && (-exponent) >= 1 && MaxSignificandPrecision < actualValue.Length)))
			{
				return actualValue;
			}
			if (!containsDecimalSeparator && ((isNegativeExponent && (-exponent) >= MaxSignificandPrecision) || (!isNegativeExponent && exponent >= MaxSignificandPrecision)))
			{
				return actualValue;
			}
			if (int.Abs(exponent) >= MaxSignificandPrecision)
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
		#endregion
	}
}