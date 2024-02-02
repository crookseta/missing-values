using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals;

internal interface IFormattableInteger<TSelf> : IFormattableNumber<TSelf>, IBinaryInteger<TSelf>
		where TSelf : IFormattableInteger<TSelf>?
{
	/// <summary>
	/// Converts a <typeparamref name="TSelf"/> to a <see cref="char"/>.
	/// </summary>
	/// <returns></returns>
	char ToChar();
	/// <summary>
	/// Converts a <typeparamref name="TSelf"/> to a <see cref="int"/>.
	/// </summary>
	/// <returns></returns>
	int ToInt32();

	/// <summary>
	/// Converts the specified hexadecimal character to a <typeparamref name="TSelf"/>.
	/// </summary>
	/// <param name="value">A hexadecimal character.</param>
	/// <returns>The hexadecimal value of <paramref name="value"/> if it represents a number; otherwise, 0</returns>
	abstract static TSelf GetHexValue(char value);

	static bool IFormattableNumber<TSelf>.IsBinaryInteger()
	{
		return true;
	}

	/// <summary>
	/// Gets the value <c>2</c> of the type.
	/// </summary>
	abstract static TSelf Two { get; }
	/// <summary>
	/// Gets the value <c>16</c> of the type.
	/// </summary>
	abstract static TSelf Sixteen { get; }
	/// <summary>
	/// Gets the value <c>10</c> of the type.
	/// </summary>
	abstract static TSelf Ten { get; }

	/// <summary>
	/// Gets the value <c>4</c> of the type.
	/// </summary>
	abstract static TSelf TwoPow2 { get; }
	/// <summary>
	/// Gets the value <c>256</c> of the type.
	/// </summary>
	abstract static TSelf SixteenPow2 { get; }
	/// <summary>
	/// Gets the value <c>100</c> of the type.
	/// </summary>
	abstract static TSelf TenPow2 { get; }

	/// <summary>
	/// Gets the value <c>8</c> of the type.
	/// </summary>
	abstract static TSelf TwoPow3 { get; }
	/// <summary>
	/// Gets the value <c>4096</c> of the type.
	/// </summary>
	abstract static TSelf SixteenPow3 { get; }
	/// <summary>
	/// Gets the value <c>1000</c> of the type.
	/// </summary>
	abstract static TSelf TenPow3 { get; }

	/// <summary>
	/// Gets the left-most digit of the maximum value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static char LastDecimalDigitOfMaxValue { get; }
	/// <summary>
	/// Gets the number of digits of the maximum decimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxDecimalDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum hexadecimal value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxHexDigits { get; }
	/// <summary>
	/// Gets the number of digits of the maximum binary value of <typeparamref name="TSelf"/>.
	/// </summary>
	abstract static int MaxBinaryDigits { get; }
}

internal interface IFormattableSignedInteger<TSigned, TUnsigned> : IFormattableInteger<TSigned>, ISignedNumber<TSigned>
	where TSigned : IFormattableSignedInteger<TSigned, TUnsigned>
	where TUnsigned : IUnsignedNumber<TUnsigned>, IFormattableInteger<TUnsigned>
{
	/// <summary>
	/// Returns the unsigned representation of the signed integer.
	/// </summary>
	/// <returns>The unsigned representation of the signed integer.</returns>
	TUnsigned ToUnsigned();
}

internal interface IFormattableUnsignedInteger<TUnsigned, TSigned> : IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
	where TUnsigned : IFormattableUnsignedInteger<TUnsigned, TSigned>
	where TSigned : IFormattableInteger<TSigned>
{
	/// <summary>
	/// Gets the absolute representation of the maximum representable value of <typeparamref name="TSigned"/>.
	/// </summary>
	abstract static TUnsigned SignedMaxMagnitude { get; }

	/// <summary>
	/// Returns the signed representation of the unsigned integer.
	/// </summary>
	/// <returns>The signed representation of the unsigned integer.</returns>
	TSigned ToSigned();
}

internal static partial class NumberFormatter
{
	public static string UnsignedIntegerToString<T>(in T value, T numberBase)
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
	public static string UnsignedIntegerToString<T>(in T value, T numberBase, int digits)
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
	public static void UnsignedIntegerToCharSpan<T>(T value, in T numberBase, Span<char> destination)
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
	public static void UnsignedIntegerToCharSpan<T>(T value, in T numberBase, int digits, Span<char> destination)
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
	public static void UnsignedIntegerToCharSpan<TNumber, TChar>(TNumber value, in TNumber numberBase, int digits, Span<TChar> destination)
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

	public static string UnsignedIntegerToDecimalString<T>(in T value)
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
			isUpper = char.IsUpper(format[0]);
			fmt = char.ToLowerInvariant(format[0]);
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
			return UnsignedIntegerToString(in value, fmtBase, precision).ToUpper();
		}

		return UnsignedIntegerToString(in value, fmtBase, precision);
	}
	public static bool TryFormatUnsignedInteger<TNumber, TChar>(in TNumber value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
			isUpper = char.IsUpper(format[0]);
			fmt = char.ToLowerInvariant(format[0]);
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
		UnsignedIntegerToCharSpan(value, in fmtBase, precision, output);

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

	public static string SignedIntegerToDecimalString<TSigned, TUnsigned>(in TSigned value)
		where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>, IMinMaxValue<TSigned>
		where TUnsigned : struct, IFormattableInteger<TUnsigned>, IUnsignedNumber<TUnsigned>
	{
		if (TSigned.IsNegative(value))
		{
			if (value == TSigned.MinValue)
			{
				return "-" + UnsignedIntegerToString(TSigned.MaxValue.ToUnsigned() + TUnsigned.One, TUnsigned.Ten);
			}
			return "-" + UnsignedIntegerToDecimalString(TSigned.Abs(value).ToUnsigned());
		}
		else
		{
			return UnsignedIntegerToDecimalString(value.ToUnsigned());
		}
	}

	public static string FormatSignedInteger<TSigned, TUnsigned>(in TSigned value, string? format, NumberStyles style, IFormatProvider? formatProvider)
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
			'b' => UnsignedIntegerToString(value.ToUnsigned(), TUnsigned.Two, precision),
			'x' => UnsignedIntegerToString(value.ToUnsigned(), TUnsigned.Sixteen, precision),
			_ => SignedIntegerToDecimalString<TSigned, TUnsigned>(in value),
		};

		if (isUpper)
		{
			output = output.ToUpper();
		}

		return output;
	}

	public static bool TryFormatSignedInteger<TSigned, TUnsigned, TChar>(in TSigned value, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
			isUpper = char.IsUpper(format[0]);
			fmt = char.ToLowerInvariant(format[0]);
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
		UnsignedIntegerToCharSpan(v.ToUnsigned(), fmtBase.ToUnsigned(), precision, output);

		if (isUpper)
		{
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = TChar.ToUpper(output[i]);
			}
		}

		if (isNegative && fmtBase == TSigned.Ten)
		{
			TChar.Copy(NumberFormatInfo.GetInstance(provider).NegativeSign, output[..1]);
		}

		bool success = output.TryCopyTo(destination);


		charsWritten = success ? precision : 0;
		return success;
	}
}
