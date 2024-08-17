using MissingValues.Internals;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info
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
			scoped NumberInfo number;
			scoped Span<byte> digits;
			ValueListBuilder<TChar> builder = new ValueListBuilder<TChar>(stackalloc TChar[256]);

			if (TNumber.IsBinaryInteger())
			{
				if (value is UInt256 uInt256)
				{
					digits = stackalloc byte[UInt256Precision];
					number = new(digits);
					UIntToNumber<UInt256, Int256>(in uInt256, ref number);
				}
				else if (value is Int256 int256)
				{
					digits = stackalloc byte[Int256Precision];
					number = new(digits);
					IntToNumber<Int256, UInt256>(ref int256, ref number);
				}
				else if (value is UInt512 uInt512)
				{
					digits = stackalloc byte[UInt512Precision];
					number = new(digits);
					UIntToNumber<UInt512, Int512>(in uInt512, ref number);
				}
				else if (value is Int512 int512)
				{
					digits = stackalloc byte[Int512Precision];
					number = new(digits);
					IntToNumber<Int512, UInt512>(ref int512, ref number);
				}
				else
				{
					throw new FormatException();
				}
			}
			else
			{
				bool exceptional;

				if (value is Quad quad)
				{
					digits = stackalloc byte[11563 + 1 + 1];

					number = new(digits);
					Ryu.Format<Quad, UInt128>(in quad, ref number, out exceptional);
				}
				else if (value is Octo octo)
				{
					digits = stackalloc byte[183466 + 1 + 1];

					number = new(digits);
					Ryu.Format<Octo, UInt256>(in octo, ref number, out exceptional);
				}
				else
				{
					throw new FormatException();
				}

				if (exceptional)
				{
					scoped Span<TChar> sign;
					bool result;
					if (number.DigitsCount > 1) // NaN
					{
						sign = stackalloc TChar[TChar.GetLength(provider.NaNSymbol)];
						TChar.Copy(provider.NaNSymbol, sign);
					}
					else if (number.IsNegative) // Negative Infinity
					{
						sign = stackalloc TChar[TChar.GetLength(provider.NegativeInfinitySymbol)];
						TChar.Copy(provider.NegativeInfinitySymbol, sign);
					}
					else // Positive Infinity
					{
						sign = stackalloc TChar[TChar.GetLength(provider.PositiveInfinitySymbol)];
						TChar.Copy(provider.PositiveInfinitySymbol, sign);
					}
					if(result = sign.TryCopyTo(destination))
					{
						charsWritten = sign.Length;
					}
					else
					{
						charsWritten = 0;
					}
					return result;
					
				}
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
					Format<CurrencyFormat>(ref builder, ref number, precision, false, provider);
					return builder.TryCopyTo(destination, out charsWritten);
				case 'e':
					Debug.Assert(TNumber.IsBinaryInteger());
					Format<EngineeringFormat>(ref builder, ref number, precision, false, provider);
					return builder.TryCopyTo(destination, out charsWritten);
				case 'E':
					Debug.Assert(TNumber.IsBinaryInteger());
					Format<EngineeringFormat>(ref builder, ref number, precision, true, provider);
					return builder.TryCopyTo(destination, out charsWritten);
				case 'f':
				case 'F':
					Format<FixedFormat>(ref builder, ref number, precision, false, provider);
					return builder.TryCopyTo(destination, out charsWritten);
				case 'n':
				case 'N':
					Format<NumericFormat>(ref builder, ref number, precision, false, provider);
					return builder.TryCopyTo(destination, out charsWritten);
				default:
					throw new FormatException();
			}
			static void Format<TFormat>(ref ValueListBuilder<TChar> vlb, ref NumberInfo number, int nMaxDigits, bool isUpper, NumberFormatInfo info)
				where TFormat : struct, INumberFormat
			{
				if (nMaxDigits < 0)
				{
					nMaxDigits = TFormat.GetDefaultDecimalDigits(info);
				}
				if (TFormat.CanRound)
				{
					RoundNumber(ref number, TFormat.GetRoundingPosition(ref number, ref nMaxDigits));
				}
				TFormat.Format(ref vlb, ref number, nMaxDigits, isUpper, info);
			}
		}

		private static void UIntToNumber<TUnsigned, TSigned>(in TUnsigned value, ref NumberInfo number)
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
		{
			number.DigitsCount = TUnsigned.MaxDecimalDigits;

			TryFormatUnsignedInteger<TUnsigned, TSigned, Utf8Char>(in value, MemoryMarshal.Cast<byte, Utf8Char>(number.Digits), out int charsWritten, [], null);

			number.DigitsCount = charsWritten;
			number.Scale = charsWritten;
			number.Digits[charsWritten] = (byte)'\0';

		}
		private static void IntToNumber<TSigned, TUnsigned>(ref TSigned value, ref NumberInfo number)
			where TSigned : struct, IFormattableSignedInteger<TSigned, TUnsigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned, TSigned>
		{
			if (TSigned.IsPositive(value))
			{
				number.IsNegative = false;
			}
			else
			{
				number.IsNegative = true;
				value = -value;
			}

			UIntToNumber<TUnsigned, TSigned>(value.ToUnsigned(), ref number);
		}

		/// <summary>
		/// Groups numbers in a similar way to the numeric format (ex. #,###,###.##).
		/// </summary>
		/// <typeparam name="TChar"></typeparam>
		/// <param name="vlb"></param>
		/// <param name="number"></param>
		/// <param name="nMaxDigits"></param>
		/// <param name="groupDigits"></param>
		/// <param name="sDecimal"></param>
		/// <param name="sGroup"></param>
		internal static void FormatGroupedNumeric<TChar>(
			ref ValueListBuilder<TChar> vlb, ref NumberInfo number,
			int nMaxDigits, int[]? groupDigits,
			scoped ReadOnlySpan<TChar> sDecimal, scoped ReadOnlySpan<TChar> sGroup)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			int digPos = number.Scale;
			ref byte dig = ref number.GetDigitsReference();

			if (digPos > 0)
			{
				if (groupDigits is not null)
				{
					int groupSizeIndex = 0;                             // Index into the groupDigits array.
					int bufferSize = digPos;                            // The length of the result buffer string.
					int groupSize = 0;                                  // The current group size.

					// Find out the size of the string buffer for the result.
					if (groupDigits.Length != 0) // You can pass in 0 length arrays
					{
						int groupSizeCount = groupDigits[groupSizeIndex];   // The current total of group size.

						while (digPos > groupSizeCount)
						{
							groupSize = groupDigits[groupSizeIndex];
							if (groupSize == 0)
							{
								break;
							}

							bufferSize += sGroup.Length;
							if (groupSizeIndex < groupDigits.Length - 1)
							{
								groupSizeIndex++;
							}

							groupSizeCount += groupDigits[groupSizeIndex];
							ArgumentOutOfRangeException.ThrowIfNegative(groupSizeCount | bufferSize);
						}
						groupSize = groupSizeCount == 0 ? 0 : groupDigits[0]; // If you passed in an array with one entry as 0, groupSizeCount == 0
					}

					groupSizeIndex = 0;
					int digitCount = 0;
					int digLength = number.DigitsCount;
					int digStart = (digPos < digLength) ? digPos : digLength;

					ref TChar spanPtr = ref MemoryMarshal.GetReference(vlb.AppendSpan(bufferSize));
					ref TChar p = ref Unsafe.Add(ref spanPtr, bufferSize - 1);

					for (int i = digPos - 1; i >= 0; i--)
					{
						p = ((i < digStart) ? (TChar)Unsafe.Add(ref dig, i) : (TChar)'0');
						p = ref Unsafe.Subtract(ref p, 1);

						if (groupSize > 0)
						{
							digitCount++;
							if ((digitCount == groupSize) && (i != 0))
							{
								for (int j = sGroup.Length - 1; j >= 0; j--)
								{
									p = sGroup[j];
									p = ref Unsafe.Subtract(ref p, 1);
								}

								if (groupSizeIndex < groupDigits.Length - 1)
								{
									groupSizeIndex++;
									groupSize = groupDigits[groupSizeIndex];
								}
								digitCount = 0;
							}
						}
					}

					dig = ref Unsafe.Add(ref dig, digStart);
				}
				else
				{
					do
					{
						vlb.Append((TChar)(dig != 0 ? (char)dig : '0'));
						dig = ref Unsafe.Add(ref dig, 1);
					} while (--digPos > 0);
				}
			}
			else
			{
				vlb.Append((TChar)'0');
			}

			if (nMaxDigits > 0)
			{
				vlb.Append(sDecimal);
				if ((digPos < 0) && (nMaxDigits > 0))
				{
					int zeroes = Math.Min(-digPos, nMaxDigits);
					for (int i = 0; i < zeroes; i++)
					{
						vlb.Append((TChar)('0'));
					}
					digPos += zeroes;
					nMaxDigits -= zeroes;
				}

				while (nMaxDigits > 0)
				{
					vlb.Append((TChar)((dig != 0) ? (char)(dig) : '0'));
					dig = ref Unsafe.Add(ref dig, 1);
					nMaxDigits--;
				}
			}
		}
		internal static void FormatExponent<TChar>(ref ValueListBuilder<TChar> vlb, NumberFormatInfo info, int value, char expChar, int minDigits, bool positiveSign)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{

			vlb.Append((TChar)expChar);

			if (value < 0)
			{
				Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
				TChar.Copy(info.NegativeSign, negativeSign);
				vlb.Append(negativeSign);
				value = -value;
			}
			else
			{
				if (positiveSign)
				{
					Span<TChar> posSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
					TChar.Copy(info.PositiveSign, posSign);
					vlb.Append(posSign);
				}
			}

			Span<TChar> digits = stackalloc TChar[10];
			int charsWritten = 0;
			TryCopyInt32(value, digits, ref charsWritten, info);
			vlb.Append(digits[..charsWritten]);
		}

		private static void RoundNumber(ref NumberInfo number, int pos)
		{
			ref byte dig = ref number.GetDigitsReference();

			int i = 0;
			while (i < pos && Unsafe.Add(ref dig, i) != '\0')
			{
				i++;
			}

			if ((i == pos) && ShouldRoundUp(ref dig, i))
			{
				while (i > 0 && Unsafe.Add(ref dig, i - 1) == '9')
				{
					i--;
				}

				if (i > 0)
				{
					Unsafe.Add(ref dig, i - 1) = (byte)(Unsafe.Add(ref dig, i - 1) + 1);
				}
				else
				{
					number.Scale++;
					dig = (byte)('1');
					i = 1;
				}
			}
			else
			{
				while (i > 0 && Unsafe.Add(ref dig, i - 1) == '0')
				{
					i--;
				}
			}

			if (i == 0)
			{
				number.Scale = 0; // Decimals with scale ('0.00') should be rounded.
			}

			Unsafe.Add(ref dig, i) = (byte)'\0';
			number.DigitsCount = i;

			static bool ShouldRoundUp(ref byte dig, int i)
			{
				byte digit = Unsafe.Add(ref dig, i);

				if (digit == '\0')
				{
					return false;
				}

				return digit >= '5';
			}
		}

		private static bool TryCopyInt32<TChar>(int value, Span<TChar> destination, ref int charsWritten, NumberFormatInfo info)
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

			//if (int.IsNegative(value))
			//{
			//	var length = TChar.GetLength(info.NegativeSign);
			//	charsWritten += length;
			//	TChar.Copy(info.NegativeSign, digits[(i - length)..]);
			//}
			//else
			//{
			//	var length = TChar.GetLength(info.PositiveSign);
			//	charsWritten += length;
			//	TChar.Copy(info.PositiveSign, digits[(i - length)..]);
			//}

			return digits.TrimStart(TChar.NullCharacter).TryCopyTo(destination);
		}
	}
}