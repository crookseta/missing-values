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
		internal interface IIntegerRadixConverter<TInteger>
			where TInteger : struct, IFormattableInteger<TInteger>
		{
			static abstract NumberStyles AllowedStyles { get; }
			static abstract bool IsValidChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
			static abstract TInteger FromChar<TChar>(TChar ch) where TChar : unmanaged, IUtfCharacter<TChar>;
			static abstract uint MaxDigitValue { get; }
			static abstract int MaxDigitCount { get; }
			static abstract int MaxUInt64DigitCount { get; }
			static abstract int BitsPerCharacter { get; }
			static abstract TInteger ShiftLeftForNextDigit(in TInteger value);
		}
		private readonly struct HexConverter<TInteger> : IIntegerRadixConverter<TInteger>
			where TInteger : struct, IFormattableInteger<TInteger>
		{
			public static NumberStyles AllowedStyles => NumberStyles.HexNumber;

			public static uint MaxDigitValue => 0xF;

			public static int MaxDigitCount => TInteger.MaxHexDigits;

			public static int MaxUInt64DigitCount => 16;

			public static int BitsPerCharacter => 4;

			public static TInteger FromChar<TChar>(TChar ch)
				where TChar : unmanaged, IUtfCharacter<TChar>
			{
				return TInteger.GetHexValue((char)ch);
			}

			public static bool IsValidChar<TChar>(TChar ch)
				where TChar : unmanaged, IUtfCharacter<TChar>
			{
				return TChar.IsHexDigit(ch);
			}

			public static TInteger ShiftLeftForNextDigit(in TInteger value)
			{
				return value << 4;
			}
		}
		private readonly struct BinConverter<TInteger> : IIntegerRadixConverter<TInteger>
			where TInteger : struct, IFormattableInteger<TInteger>
		{
			public static NumberStyles AllowedStyles => NumberStyles.BinaryNumber;

			public static uint MaxDigitValue => 0b1;

			public static int MaxDigitCount => TInteger.MaxBinaryDigits;

			public static int MaxUInt64DigitCount => 64;

			public static int BitsPerCharacter => 1;

			public static TInteger FromChar<TChar>(TChar ch)
				where TChar : unmanaged, IUtfCharacter<TChar>
			{
				return TInteger.GetDecimalValue((char)ch);
			}

			public static bool IsValidChar<TChar>(TChar ch)
				where TChar : unmanaged, IUtfCharacter<TChar>
			{
				return ch == (TChar)'1' || ch == (TChar)'0';
			}

			public static TInteger ShiftLeftForNextDigit(in TInteger value)
			{
				return value << 1;
			}
		}

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

		public static ParsingStatus ParseDecStringToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, out T output)
			where T : struct, IFormattableUnsignedInteger<T>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (s.Length > T.MaxDecimalDigits || (s.Length == T.MaxDecimalDigits && (char)s[0] > T.LastDecimalDigitOfMaxValue))
			{
				output = default;
				return ParsingStatus.Overflow;
			}

			T e19 = T.E19;
			ulong r;

			if (s.Length < 19 && TChar.TryParseInteger(s, NumberStyles.Integer, CultureInfo.CurrentCulture, out r))
			{
				output = T.CreateTruncating(r);
				return ParsingStatus.Success;
			}
			else if (TChar.TryParseInteger(s[..19], NumberStyles.Integer, CultureInfo.CurrentCulture, out r))
			{
				output = T.CreateTruncating(r);
			}
			else
			{
				output = default;
				return ParsingStatus.Failed;
			}

			int i = 19, length = s.Length - 19;

			do
			{
				if (i > length)
				{
					break;
				}
				output *= e19;
				ReadOnlySpan<TChar> slice = s[i..];
				i += 19;
				if (TChar.TryParseInteger(slice[..19], NumberStyles.Integer, CultureInfo.CurrentCulture, out r))
				{
					output += T.CreateTruncating(r);
				}
				else
				{
					output = default;
					return ParsingStatus.Failed;
				}
			} while (true);

			length = s.Length - i;
			if (length != 0)
			{
				if (TChar.TryParseInteger(s[^length..], NumberStyles.Integer, CultureInfo.CurrentCulture, out r))
				{
					output *= T.CreateTruncating(E19Table[length]);
					T addon = output + T.CreateTruncating(r);
					if (addon < output)
					{
						output = default;
						return ParsingStatus.Overflow;
					}
					else
					{
						output = addon;
					}
				}
				else
				{
					output = default;
					return ParsingStatus.Failed;
				}
			}

			return ParsingStatus.Success;
		}
		public static ParsingStatus ParseStringToUnsigned<TInteger, TChar, TConverter>(ReadOnlySpan<TChar> s, out TInteger output)
			where TInteger : struct, IFormattableUnsignedInteger<TInteger>
			where TChar : unmanaged, IUtfCharacter<TChar>
			where TConverter : struct, IIntegerRadixConverter<TInteger>
		{
			if (s.Length > TConverter.MaxDigitCount)
			{
				output = default;
				return ParsingStatus.Overflow;
			}
			ulong temp;
			int count = TConverter.MaxUInt64DigitCount;
			if (s.Length <= count)
			{
				if (!TChar.TryParseInteger(s, TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp))
				{
					output = default;
					return ParsingStatus.Failed;
				}
				output = TInteger.CreateTruncating(temp);
				return ParsingStatus.Success;
			}

			if (!TChar.TryParseInteger(s[..count], TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp))
			{
				output = default;
				return ParsingStatus.Failed;
			}
			output = TInteger.CreateTruncating(temp);
			ReadOnlySpan<TChar> slice = s[count..];

			while (count <= slice.Length)
			{
				output <<= 64;
				if (!TChar.TryParseInteger(slice[..count], TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp))
				{
					output = default;
					return ParsingStatus.Failed;
				}
				output |= TInteger.CreateTruncating(temp);
				slice = slice[count..];
			}

			if (slice.Length != 0)
			{
				if (!TChar.TryParseInteger(slice, TConverter.AllowedStyles, CultureInfo.CurrentCulture, out temp))
				{
					output = default;
					return ParsingStatus.Failed;
				}
				int shiftAmount = slice.Length * TConverter.BitsPerCharacter;
				output <<= shiftAmount;
				output |= TInteger.CreateTruncating(temp);
			}
			return ParsingStatus.Success;
		}

		public static ParsingStatus TryParseToUnsigned<T, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out T output)
			where T : struct, IFormattableUnsignedInteger<T>
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
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];
			TChar.Copy(formatInfo.NegativeSign, negativeSign);
			if (TChar.StartsWith(s, negativeSign, StringComparison.OrdinalIgnoreCase))
			{
				output = default;
				return ParsingStatus.Underflow;
			}

			if ((style & SPECIAL) != 0)
			{
				NumberInfo number = new NumberInfo(stackalloc byte[IntBufferLength]);
				NumberFormatInfo info = NumberFormatInfo.GetInstance(formatProvider);
				if (!NumberInfo.TryParse(s, ref number, info, style)
					|| !NumberInfo.TryConvertToInteger(ref number, out output))
				{
					output = default;
					return ParsingStatus.Failed;
				}

				return ParsingStatus.Success;
			}

			if (ContainsInvalidCharacter(s, style))
			{
				output = default;
				return ParsingStatus.Failed;
			}

			ParsingStatus status;

			if (style.HasFlag(NumberStyles.AllowHexSpecifier))
			{
				status = ParseStringToUnsigned<T, TChar, HexConverter<T>>(s, out output);
			}
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				status = ParseStringToUnsigned<T, TChar, BinConverter<T>>(s, out output);
			}
			else
			{
				status = ParseDecStringToUnsigned(s, out output);
			}

			return status;
		}

		public static ParsingStatus TryParseToSigned<TSigned, TUnsigned, TChar>(ReadOnlySpan<TChar> s, NumberStyles style, IFormatProvider? formatProvider, out TSigned output)
			where TSigned : struct, IFormattableSignedInteger<TSigned>
			where TUnsigned : struct, IFormattableUnsignedInteger<TUnsigned>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Debug.Assert(Unsafe.SizeOf<TUnsigned>() == Unsafe.SizeOf<TSigned>());
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
				isNegative = style.HasFlag(NumberStyles.AllowLeadingSign) && TChar.StartsWith(s, negativeSign, StringComparison.OrdinalIgnoreCase);
				raw = isNegative ? s[(negativeSign.Length)..] : s;
			}

			if ((style & SPECIAL) != 0)
			{
				NumberInfo number = new NumberInfo(stackalloc byte[IntBufferLength]);
				NumberFormatInfo info = NumberFormatInfo.GetInstance(formatProvider);
				if (!NumberInfo.TryParse(s, ref number, info, style)
					|| !NumberInfo.TryConvertToInteger(ref number, out output))
				{
					output = default;
					return ParsingStatus.Failed;
				}

				return ParsingStatus.Success;
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
				status = ParseStringToUnsigned<TUnsigned, TChar, HexConverter<TUnsigned>>(raw, out result);
				output = Unsafe.BitCast<TUnsigned, TSigned>(result);
				return status;
			}
			else if (style.HasFlag(NumberStyles.AllowBinarySpecifier))
			{
				status = ParseStringToUnsigned<TUnsigned, TChar, BinConverter<TUnsigned>>(raw, out result);
				output = Unsafe.BitCast<TUnsigned, TSigned>(result);
				return status;
			}
			else
			{
				status = ParseDecStringToUnsigned(raw, out result);
			}

			if (!status)
			{
				output = default;
				return status;
			}

			if (result == TUnsigned.SignedMaxMagnitude)
			{
				if (!isNegative)
				{
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
		/*
		 * Max buffer length for floating point numbers:
		 * Quad: 11563
		 * Octo: 183466
		 */
		internal const int QuadBufferLength = 11563 + 1 + 1; // Max buffer length + 1 for rounding 
		internal const int OctoBufferLength = 183466 + 1 + 1; // Max buffer length + 1 for rounding 

		public static unsafe bool TryParseFloat<TFloat, TBits, TChar>(ReadOnlySpan<TChar> s, NumberStyles styles, IFormatProvider? provider, [MaybeNullWhen(false)] out TFloat result)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);
			// TODO: Expose Hex parsing for v3.0
			if (false /*(styles & NumberStyles.AllowHexSpecifier) != 0*/)
			{
				return TryParseHexFloat<TFloat, TBits, TChar>(s, styles, info, out result);
			}
			
			byte[] buffer = ArrayPool<byte>.Shared.Rent(OctoBufferLength);
			NumberInfo number = new NumberInfo(buffer, true);

			if (!NumberInfo.TryParse(s, ref number, info, styles))
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

			result = NumberInfo.ConvertToFloat<TFloat, TBits>(ref number);
			ArrayPool<byte>.Shared.Return(buffer);
			return true;
		}

		private static bool TryParseHexFloat<TFloat, TBits, TChar>(ReadOnlySpan<TChar> value, NumberStyles styles, NumberFormatInfo info, out TFloat result) 
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// Based on CoreLib implementation: https://github.com/dotnet/runtime/blob/ecc5874fbe7c2f2db3cc7e563bc6e81c7a2c17f6/src/libraries/System.Private.CoreLib/src/System/Number.Parsing.cs#L982
			
			result = TFloat.Zero;

			if (value.IsEmpty)
			{
				return false;
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
				return false;
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
				return false;
			}
			
			// Require "0x" or "0X" prefix (consistent with IEEE 754 conventions)
			if ((uint)value[index] != '0' ||
			    index + 1 >= value.Length ||
			    ((uint)value[index + 1] | 0x20) != 'x')
			{
				return false;
			}
			index += 2;

			if (index >= value.Length)
			{
				return false;
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
				return false;
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
                    return false;
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
                    return false;
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
                    return false;
                }

                if (exponentIsNegative)
                {
                    binaryExponent = -binaryExponent;
                }
            }
            else
            {
                // Exponent indicator (p/P) is required
                return false;
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
                return false;
            }

            if (significand == TBits.Zero)
            {
                result = isNegative ? TFloat.NegativeZero : TFloat.Zero;
                return true;
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
                return true;
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
                    return true;
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
                            return true;
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

            return true;

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
