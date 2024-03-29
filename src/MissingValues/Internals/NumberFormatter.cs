using MissingValues.Internals;
using System;
using System.Buffers;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MissingValues.Internals
{
	internal static partial class NumberFormatter
	{
		private const int UInt256Precision = 78;
		private const int Int256Precision = UInt256Precision;
		private const int UInt512Precision = 155;
		private const int Int512Precision = UInt512Precision;

		private static string FormatNumber<TNumber>(
			in TNumber value,
			string format,
			NumberFormatInfo provider)
			where TNumber : struct, IFormattableNumber<TNumber>
		{
			Span<char> buffer = stackalloc char[128];

			if (!TryFormatNumber(
				in value,
				  Utf16Char.CastFromCharSpan(buffer),
					out int charsWritten,
					format,
					provider))
			{
				int minimumLength = 256;
				char[] bufferArray = ArrayPool<char>.Shared.Rent(minimumLength);
				buffer = bufferArray;
				while (!TryFormatNumber(
					in value,
				  Utf16Char.CastFromCharSpan(buffer),
					out charsWritten,
					format,
					provider))
				{
					ArrayPool<char>.Shared.Return(bufferArray);
					minimumLength *= 2;
					bufferArray = ArrayPool<char>.Shared.Rent(minimumLength);
					buffer = bufferArray;
				}
				string output = new string(buffer[..charsWritten]);
				ArrayPool<char>.Shared.Return(bufferArray);
				return output;
			}
			return new string(buffer[..charsWritten]);
		}
		private static bool TryFormatNumber<TNumber, TChar>(
			in TNumber value,
			Span<TChar> destination, 
			out int charsWritten, 
			ReadOnlySpan<char> format,
			NumberFormatInfo provider)
			where TNumber : struct, IFormattableNumber<TNumber>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			const int DefaultPrecisionExponentialFormat = 6;

			int digitCount;
			bool isNegative;
			int exponent = 0;
			scoped Span<TChar> digits;

			if (TNumber.IsBinaryInteger())
			{
				digits = stackalloc TChar[
					value switch
					{
						UInt256 => UInt256Precision,
						Int256 => Int256Precision,
						UInt512 => UInt512Precision,
						Int512 => Int512Precision,
						_ => throw new FormatException()
					}];
				IntegerToDigits(in value, digits, out digitCount, out isNegative);
				exponent = digitCount - 1;
			}
			else
			{
				UInt256 mantissa;

				if (value is Quad quad)
				{
					digits = stackalloc TChar[39]; // UInt128Precision

					var temp = Ryu.FloatToFloatingDecimal128(in quad);

					mantissa = temp.Mantissa;
					exponent = temp.Exponent;
					isNegative = temp.Sign;
				}
				else
				{
					throw new FormatException();
				}

				IntegerToDigits(in mantissa, digits, out digitCount, out _);
			}

			digits = digits[..digitCount];
			if (digits.Length > destination.Length)
			{
				charsWritten = 0;
				return false;
			}

			int precision = -1;
			if (format.Length > 1)
			{
				precision = int.Parse(format[1..]);
			}

			switch (format[0])
			{
				case 'c':
				case 'C':
					throw new NotImplementedException();
					break;
				case 'e':
					Debug.Assert(TNumber.IsBinaryInteger());
					if (precision < 0)
					{
						precision = DefaultPrecisionExponentialFormat;
					}
					RoundNumber(digits, ref exponent, ref digitCount, precision);
					return TryFormatScientific(destination, digits, exponent, isNegative, digitCount, out charsWritten, false, provider);
				case 'E':
					Debug.Assert(TNumber.IsBinaryInteger());
					if (precision < 0)
					{
						precision = DefaultPrecisionExponentialFormat;
					}
					RoundNumber(digits, ref exponent, ref digitCount, precision);
					return TryFormatScientific(destination, digits, exponent, isNegative, digitCount, out charsWritten, true, provider);
				case 'f':
				case 'F':
					throw new NotImplementedException();
					break;
				case 'n':
				case 'N':
					throw new NotImplementedException();
					break;
				case 'g':
				case 'G':
					throw new NotImplementedException();
					break;
			}



			if (TNumber.IsBinaryInteger())
			{
				return value switch
				{
					UInt256 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					UInt512 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					Int256 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					Int512 n => TryFormatIntegerSlow(in n, destination, out charsWritten, format, provider),
					_ => throw new FormatException()
				};
				
			}
			else
			{
				double dValue = double.CreateChecked(value);

				if (typeof(TChar) == typeof(Utf16Char))
				{
					return dValue.TryFormat(TChar.CastToCharSpan(destination), out charsWritten, format, provider);
				}
#if NET8_0_OR_GREATER
				else if (typeof(TChar) == typeof(Utf8Char))
				{
					return dValue.TryFormat(TChar.CastToByteSpan(destination), out charsWritten, format, provider);
				}
#endif
				else
				{
					charsWritten = 0;
					return false;
				}
			}
		}

		private static void IntegerToDigits<TInt, TChar>(in TInt value, Span<TChar> digits, out int digitCount, out bool isNegative)
			where TInt : struct, IFormattableNumber<TInt>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			isNegative = false;
			if (value is UInt256 uInt256)
			{
				UnsignedIntegerToDecChars<UInt256, Int256, TChar>(uInt256, digits);
			}
			else if (value is Int256 int256)
			{
				UInt256 absValue;
				if (Int256.IsNegative(int256))
				{
					isNegative = true;
					absValue = int256 == Int256.MinValue ? new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000) : (UInt256)Int256.Abs(int256);
					
					UnsignedIntegerToDecChars<UInt256, Int256, TChar>(absValue, digits);
				}
				else
				{
					absValue = (UInt256)int256;
					UnsignedIntegerToDecChars<UInt256, Int256, TChar>(absValue, digits);
				}
			}
			else if (value is UInt512 uInt512)
			{
				UnsignedIntegerToDecChars<UInt512, Int512, TChar>(uInt512, digits);
			}
			else if (value is Int512 int512)
			{
				UInt512 absValue;
				if (Int512.IsNegative(int512))
				{
					isNegative = true;
					absValue = int512 == Int512.MinValue 
						? new UInt512(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000,
			   0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000) : (UInt512)Int512.Abs(int512);

					UnsignedIntegerToDecChars<UInt512, Int512, TChar>(absValue, digits);
				}
				else
				{
					absValue = (UInt512)int512;
					UnsignedIntegerToDecChars<UInt512, Int512, TChar>(absValue, digits);
				}
			}

			digitCount = digits.TrimEnd(TChar.NullCharacter).Length;
		}

		private static bool TryFormatIntegerSlow<TInteger, TChar>(
			in TInteger value,
			Span<TChar> destination,
			out int charsWritten,
			ReadOnlySpan<char> format,
			NumberFormatInfo provider)
			where TInteger : struct, IFormattableInteger<TInteger>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Span<byte> integerBits = stackalloc byte[value.GetByteCount()];
			BigInteger integer = new BigInteger(integerBits, TInteger.IsNegative(value));
			charsWritten = 0;

			if (typeof(TChar) == typeof(Utf16Char))
			{
				return integer.TryFormat(TChar.CastToCharSpan(destination), out charsWritten, format, provider);
			}
#if NET8_0_OR_GREATER
			else if (typeof(TChar) == typeof(Utf8Char))
			{
				return ((IUtf8SpanFormattable)integer).TryFormat(TChar.CastToByteSpan(destination), out charsWritten, format, provider);
			}
#endif
			return false;
		}


		private static bool TryFormatScientific<TChar>(
			Span<TChar> destination, ReadOnlySpan<TChar> digits, 
			int exponent, bool isNegative, int precision, out int charsWritten,
			bool isUpper, NumberFormatInfo info)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);
			Span<TChar> numberDecimalSeparator = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
			TChar.Copy(info.NumberDecimalSeparator, numberDecimalSeparator);

			if (isNegative)
			{
				negativeSign.CopyTo(destination);
				charsWritten = negativeSign.Length;
				destination[charsWritten++] = digits[0];
			}
			else
			{
				destination[0] = digits[0];
				charsWritten = 1;
			}
			
			if (digits.Length == 1)
			{
				if (digits[0] != (TChar)'0')
				{
					destination[charsWritten++] = (TChar)(isUpper ? 'E' : 'e');
					CopyInt32(exponent, destination[charsWritten..], ref charsWritten, info);
				}

				return true;
			}
			numberDecimalSeparator.CopyTo(destination[charsWritten..]);
			charsWritten += numberDecimalSeparator.Length;

			for (int j = 1; charsWritten < digits.Length && j < precision; charsWritten++, j++)
			{
				destination[charsWritten] = digits[j];
			}

			destination[charsWritten++] = (TChar)(isUpper ? 'E' : 'e');
			CopyInt32(exponent, destination[charsWritten..], ref charsWritten, info);

			return true;
		}

		private static void RoundNumber<TChar>(Span<TChar> digits, ref int exponent, ref int digitCount, int pos)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			ref TChar dig = ref MemoryMarshal.GetReference(digits);

			int i = 0;
			while (i <= pos && Unsafe.Add(ref dig, i) != TChar.NullCharacter)
			{
				i++;
			}

			if ((i == (pos + 1)) && ShouldRoundUp(ref dig, i))
			{
				while (i > 0 && Unsafe.Add(ref dig, i - 1) == (TChar)'9')
				{
					i--;
				}

				if (i > 0)
				{
					Unsafe.Add(ref dig, i - 1) = (TChar)(byte)((byte)Unsafe.Add(ref dig, i - 1) + 1);
				}
				else
				{
					exponent++;
					Unsafe.Add(ref dig, 0) = (TChar)('1');
					i = 1;
				}
			}
			else
			{
				while (i > 0 && Unsafe.Add(ref dig, i - 1) == (TChar)'0')
				{
					i--;
				}
			}

			if (i == 0)
			{
				exponent = 0; // Decimals with scale ('0.00') should be rounded.
			}

			Unsafe.Add(ref dig, i) = TChar.NullCharacter;
			digitCount = i;

			static bool ShouldRoundUp(ref TChar dig, int i)
			{
				TChar digit = Unsafe.Add(ref dig, i);

				if (digit == TChar.NullCharacter)
				{
					return false;
				}

				return (char)digit >= '5';
			}
		}

		private static void CopyInt32<TChar>(int value, Span<TChar> destination, ref int charsWritten, NumberFormatInfo info)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Span<TChar> digits = stackalloc TChar[12];
			int quo = int.Abs(value);
			int i = 12;

			while (quo >= 10)
			{
				(quo, int rem) = int.DivRem(quo, 10);
				digits[(--i)] = (TChar)(byte)(rem + '0');
				charsWritten++;
			}
			if (quo != 0)
			{
				digits[(--i)] = (TChar)(byte)(quo + '0');
				charsWritten++;
			}

			if (int.IsNegative(value))
			{
				var length = TChar.GetLength(info.NegativeSign);
				charsWritten += length;
				TChar.Copy(info.NegativeSign, digits[(i - length)..]);
			}
			else
			{
				var length = TChar.GetLength(info.PositiveSign);
				charsWritten += length;
				TChar.Copy(info.PositiveSign, digits[(i - length)..]);
			}

			digits.TrimStart(TChar.NullCharacter).CopyTo(destination);
		}
	}
}