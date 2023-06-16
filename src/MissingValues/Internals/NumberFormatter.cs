using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static class NumberFormatter
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

		public static string UnsignedNumberToDecimalString<T>(in T value)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int digits = BitHelper.CountDigits<T>(value, T.Ten);

			return string.Create(digits, value, (chars, num) =>
			{
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					(num, var rem) = T.DivRem(num, T.Ten);
					chars[i] = (char)(rem.ToChar() + 48U);
				}
			});
		}
		public static string UnsignedNumberToBinaryString<T>(in T value)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int digits = BitHelper.CountDigits<T>(value, T.Two);

			return string.Create(digits, value, (chars, num) =>
			{
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					(num, var rem) = T.DivRem(num, T.Two);
					chars[i] = (char)(rem.ToChar() + 48U);
				}
			});
		}
		public static string UnsignedNumberToHexString<T>(in T value)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int digits = BitHelper.CountDigits<T>(value, T.Sixteen);

			return string.Create(digits, value, (chars, num) =>
			{
				for (int i = chars.Length - 1; i >= 0; i--)
				{
					(num, var rem) = T.DivRem(num, T.Sixteen);
					chars[i] = (char)(rem.ToChar() + 48U);
					chars[i] = chars[i] <= '9' ? chars[i] : (char)(chars[i] + 7U);
				}
			});
		}
		public static string FormatUnsignedNumber<T>(in T value, string? format, NumberStyles style, IFormatProvider? formatProvider)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int precision = 0;
			if (format is not null && format.Length != 1 && !int.TryParse(format[1..], out precision))
			{
				throw new ArgumentException("Invalid format.", nameof(format));
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
		public static bool TryFormatUnsignedNumber<T>(in T value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			where T : struct, IFormattableInteger<T>, IUnsignedNumber<T>
		{
			int precision = 0;
			if (format.Length > 1 && !int.TryParse(format[1..], out precision))
			{
				throw new ArgumentException("Invalid format.", nameof(format));
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

			T fmtBase = fmt switch
			{
				'b' => T.Two,
				'x' => T.Sixteen,
				_ => T.Ten,
			};

			if (fmtBase != T.Ten)
			{
				precision = int.Max(precision, BitHelper.CountDigits(value, fmtBase));
			}
			else
			{
				precision = BitHelper.CountDigits(value, fmtBase);
			}

			Span<char> output = stackalloc char[precision];
			UnsignedNumberToCharSpan(value, in fmtBase, precision, output);

			if (isUpper)
			{
				for (int i = 0; i < output.Length; i++)
				{
					output[i] = char.ToUpper(output[i]);
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
				throw new ArgumentException("Invalid format.", nameof(format));
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

		public static bool TryFormatSignedNumber<TSigned, TUnsigned>(in TSigned value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		{
			int precision = 0;
			if (format.Length > 1 && !int.TryParse(format[1..], out precision))
			{
				throw new ArgumentException("Invalid format.", nameof(format));
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

			Span<char> output = stackalloc char[precision];
			UnsignedNumberToCharSpan(v.ToUnsigned(), fmtBase.ToUnsigned(), precision, output);

			if (isUpper)
			{
				for (int i = 0; i < output.Length; i++)
				{
					output[i] = char.ToUpper(output[i]);
				}
			}

			if (isNegative && fmtBase == TSigned.Ten)
			{
				output[0] = '-';
			}

			bool success = output.TryCopyTo(destination);


			charsWritten = success ? precision : 0;
			return success;
		}
		#endregion
	}
}