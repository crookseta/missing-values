using MissingValues.Info;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MissingValues.Internals
{
	internal struct State<TInt>
		where TInt : struct, IBinaryInteger<TInt>
	{
		private TInt _state;

        private State(TInt state)
        {
			_state = state;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(TInt state)
		{
			_state |= state;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(TInt state)
		{
			_state &= ~state;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveAll()
		{
			_state = TInt.Zero;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Contains(TInt state)
		{
			return (_state & state) != TInt.Zero;
		}

		public override readonly string ToString()
		{
			return _state.ToString()!;
		}

		public static implicit operator State<TInt>(TInt @int)
		{
			return new State<TInt>(@int);
		}
		public static explicit operator TInt(State<TInt> state)
		{
			return state._state;
		}
	}

	internal ref struct NumberInfo
	{
		public int DigitsCount;
		public int Scale;
		public bool IsNegative;
		public bool IsFloating;
		public bool HasNonZeroTail;
		public Span<byte> Digits;

        public NumberInfo(Span<byte> digits)
        {
			DigitsCount = 0;
			Scale = 0;
			IsNegative = false;
			IsFloating = false;
			HasNonZeroTail = false;
			Digits = digits;
			Digits[0] = (byte)'\0';
		}
        public NumberInfo(Span<byte> digits, bool isFloating)
        {
			DigitsCount = 0;
			Scale = 0;
			IsNegative = false;
			IsFloating = isFloating;
			HasNonZeroTail = false;
			Digits = digits;
			Digits[0] = (byte)'\0';
		}

		public static bool TryConvertToInteger<TInteger>(ref NumberInfo number, out TInteger value)
			where TInteger : struct, IFormattableInteger<TInteger>, IMinMaxValue<TInteger>
		{
			// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Number.Parsing.cs

			int i = number.Scale;

			if ((i > TInteger.MaxDecimalDigits) || (i < number.DigitsCount) || (TInteger.IsUnsignedInteger && number.IsNegative) || number.HasNonZeroTail)
			{
				value = default;
				return false;
			}

			ref byte p = ref number.GetDigitsReference();

			Debug.Assert(!Unsafe.IsNullRef(ref p));
			TInteger n = TInteger.Zero;
			TInteger ten = TInteger.Ten;
			TInteger maxValueDiv10 = TInteger.MaxValue / ten;

			while (--i >= 0)
			{
				if (TInteger.UnsignedCompare(in n, in maxValueDiv10) > 0)
				{
					value = default;
					return false;
				}

				n *= ten;

				if (p != '\0')
				{
					TInteger newN = n + TInteger.GetDecimalValue((char)p);
					p = ref Unsafe.Add(ref p, 1);

					if (TInteger.IsUnsignedInteger && (newN < n))
					{
						value = default;
						return false;
					}

					n = newN;
				}
			}

			if (!TInteger.IsUnsignedInteger)
			{
				if (number.IsNegative)
				{
					n = -n;

					if (n > TInteger.Zero)
					{
						value = default;
						return false;
					}
				}
				else if (n < TInteger.Zero)
				{
					value = default;
					return false;
				}
			}

			value = n;
			return true;
		}

		public static TFloat ConvertToFloat<TFloat, TBits>(ref NumberInfo number)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
		{
			TFloat result;

			if ((number.DigitsCount == 0) || (number.Scale < TFloat.MinimumDecimalExponent))
			{
				result = TFloat.Zero;
			}
			else if (number.Scale > TFloat.MaximumDecimalExponent)
			{
				result = TFloat.PositiveInfinity;
			}
			else
			{
				TBits bits = GetFloatBits<TFloat, TBits>(ref number);
				result = TFloat.BitsToFloat(bits);
			}

			return number.IsNegative ? -result : result;
		}

		public override string ToString()
		{
			return Encoding.UTF8.GetString(Digits[..DigitsCount]);
		}

		public static unsafe bool TryParse<TChar>(ReadOnlySpan<TChar> s, ref NumberInfo info, NumberFormatInfo formatInfo, NumberStyles styles)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			fixed(TChar* stringPointer = &MemoryMarshal.GetReference(s))
			{
				TChar* p = stringPointer;
				if(!TryParse(ref p, p + s.Length, ref info, formatInfo, styles)
					|| ((int)(p - stringPointer) < s.Length && !TrailingZeros(s, (int)(p - stringPointer))))
				{
					return false;
				}

				return true;
			}
		}
		public static unsafe bool TryParse<TChar>(scoped ref TChar* str, TChar* strEnd, ref NumberInfo number, NumberFormatInfo info, NumberStyles styles)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// Based on: https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/Number.NumberBuffer.cs

			Debug.Assert(str != null);
			Debug.Assert(strEnd != null);
			Debug.Assert(str <= strEnd);
			Debug.Assert((styles & (NumberStyles.AllowHexSpecifier | NumberStyles.AllowBinarySpecifier)) == 0);

			const int StateSign = 0x0001;
			const int StateParens = 0x0002;
			const int StateDigits = 0x0004;
			const int StateNonZero = 0x0008;
			const int StateDecimal = 0x0010;
			const int StateCurrency = 0x0020;

			Debug.Assert(number.DigitsCount == 0);
			Debug.Assert(number.Scale == 0);
			Debug.Assert(!number.IsNegative);
			Debug.Assert(!number.HasNonZeroTail);

			scoped Span<TChar> decSep;
			scoped Span<TChar> groupSep;
			scoped Span<TChar> currSymbol;
			Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(info.PositiveSign)];
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];

			bool parsingCurrency;

			if ((styles & NumberStyles.AllowCurrencySymbol) != 0)
			{
				decSep = stackalloc TChar[TChar.GetLength(info.CurrencyDecimalSeparator)];
				groupSep = stackalloc TChar[TChar.GetLength(info.CurrencyGroupSeparator)];
				currSymbol = stackalloc TChar[TChar.GetLength(info.CurrencySymbol)];

				TChar.Copy(info.CurrencyDecimalSeparator, decSep);
				TChar.Copy(info.CurrencyGroupSeparator, groupSep);
				TChar.Copy(info.CurrencySymbol, currSymbol);

				parsingCurrency = true;
			}
			else
			{
				decSep = stackalloc TChar[TChar.GetLength(info.NumberDecimalSeparator)];
				groupSep = stackalloc TChar[TChar.GetLength(info.NumberGroupSeparator)];
				currSymbol = [];

				TChar.Copy(info.NumberDecimalSeparator, decSep);
				TChar.Copy(info.NumberGroupSeparator, groupSep);

				parsingCurrency = false;
			}
			TChar.Copy(info.PositiveSign, positiveSign);
			TChar.Copy(info.NegativeSign, negativeSign);

			State<int> state = 0;
			TChar* p = str;
			uint ch = (p < strEnd) ? (uint)(*p) : '\0';
			TChar* next;

			while (true)
			{
				// Eat whitespace unless we've found a sign which isn't followed by a currency symbol.
				// "-Kr 1231.47" is legal but "- 1231.47" is not.
				if (!IsWhite(ch) || (styles & NumberStyles.AllowLeadingWhite) == 0 || (state.Contains(StateSign) && !state.Contains(StateCurrency) && info.NumberNegativePattern != 2))
				{
					if (((styles & NumberStyles.AllowLeadingSign) != 0) && !state.Contains(StateSign) && ((next = MatchChars(p, strEnd, positiveSign)) != null || ((next = MatchChars(p, strEnd, negativeSign)) != null && (number.IsNegative = true))))
					{
						state.Add(StateSign);
						p = next - 1;
					}
					else if (ch == '(' && ((styles & NumberStyles.AllowParentheses) != 0) && (!state.Contains(StateSign)))
					{
						state.Add(StateSign | StateParens);
						number.IsNegative = true;
					}
					else if (!currSymbol.IsEmpty && (next = MatchChars(p, strEnd, currSymbol)) != null)
					{
						state.Add(StateCurrency);
						currSymbol = [];
						// We already found the currency symbol. There should not be more currency symbols. Set
						// currSymbol to NULL so that we won't search it again in the later code path.
						p = next - 1;
					}
					else
					{
						break;
					}
				}
				ch = ++p < strEnd ? (uint)(*p) : '\0';
			}

			int digCount = 0;
			int digEnd = 0;
			int maxDigCount = number.Digits.Length - 1;
			int numberOfTrailingZeros = 0;

			while (true)
			{
				if (IsDigit(ch))
				{
					state.Add(StateDigits);

					if (ch != '0' || state.Contains(StateNonZero))
					{
						if (digCount < maxDigCount)
						{
							number.Digits[digCount] = (byte)ch;
							if ((ch != '0') || (number.IsFloating))
							{
								digEnd = digCount + 1;
							}
						}
						else if (ch != '0')
						{
							// For decimal and binary floating-point numbers, we only
							// need to store digits up to maxDigCount. However, we still
							// need to keep track of whether any additional digits past
							// maxDigCount were non-zero, as that can impact rounding
							// for an input that falls evenly between two representable
							// results.

							number.HasNonZeroTail = true;
						}

						if (!state.Contains(StateDecimal))
						{
							number.Scale++;
						}

						if (digCount < maxDigCount)
						{
							// Handle a case like "53.0". We need to ignore trailing zeros in the fractional part for floating point numbers, so we keep a count of the number of trailing zeros and update digCount later
							if (ch == '0')
							{
								numberOfTrailingZeros++;
							}
							else
							{
								numberOfTrailingZeros = 0;
							}
						}
						digCount++;
						state.Add(StateNonZero);
					}
					else if (state.Contains(StateDecimal))
					{
						number.Scale--;
					}
				}
				else if (((styles & NumberStyles.AllowDecimalPoint) != 0) && !state.Contains(StateDecimal) && ((next = MatchChars(p, strEnd, decSep)) != null || (parsingCurrency && !state.Contains(StateCurrency) && (next = MatchChars(p, strEnd, info.NumberDecimalSeparator)) != null)))
				{
					state.Add(StateDecimal);
					p = next - 1;
				}
				else if (((styles & NumberStyles.AllowThousands) != 0) && state.Contains(StateDigits) && !state.Contains(StateDecimal) && ((next = MatchChars(p, strEnd, groupSep)) != null || (parsingCurrency && !state.Contains(StateCurrency) && (next = MatchChars(p, strEnd, info.NumberGroupSeparator)) != null)))
				{
					p = next - 1;
				}
				else
				{
					break;
				}
				ch = ++p < strEnd ? (uint)(*p) : '\0';
			}

			bool negExp = false;
			number.DigitsCount = digEnd;
			number.Digits[digEnd] = (byte)'\0';
			if (state.Contains(StateDigits))
			{
				if ((ch == 'E' || ch == 'e') && ((styles & NumberStyles.AllowExponent) != 0))
				{
					TChar* temp = p;
					ch = ++p < strEnd ? (uint)(*p) : '\0';
					if ((next = MatchChars(p, strEnd, positiveSign)) != null)
					{
						ch = (p = next) < strEnd ? (uint)(*p) : '\0';
					}
					else if ((next = MatchChars(p, strEnd, negativeSign)) != null)
					{
						ch = (p = next) < strEnd ? (uint)(*p) : '\0';
						negExp = true;
					}
					if (IsDigit(ch))
					{
						int exp = 0;
						do
						{
							// Check if we are about to overflow past our limit of 9 digits
							if (exp >= 100_000_000)
							{
								// Set exp to Int.MaxValue to signify the requested exponent is too large. This will lead to an OverflowException later.
								exp = int.MaxValue;
								number.Scale = 0;

								// Finish parsing the number, a FormatException could still occur later on.
								while (IsDigit(ch))
								{
									ch = ++p < strEnd ? (uint)(*p) : '\0';
								}
								break;
							}

							exp = (exp * 10) + (int)(ch - '0');
							ch = ++p < strEnd ? (uint)(*p) : '\0';
						} while (IsDigit(ch));
						if (negExp)
						{
							exp = -exp;
						}
						number.Scale += exp;
					}
					else
					{
						p = temp;
						ch = p < strEnd ? (uint)(*p) : '\0';
					}
				}

				if (number.IsFloating && !number.HasNonZeroTail)
				{
					// Adjust the number buffer for trailing zeros
					int numberOfFractionalDigits = digEnd - number.Scale;
					if (numberOfFractionalDigits > 0)
					{
						numberOfTrailingZeros = Math.Min(numberOfTrailingZeros, numberOfFractionalDigits);
						Debug.Assert(numberOfTrailingZeros >= 0);
						number.DigitsCount = digEnd - numberOfTrailingZeros;
						number.Digits[number.DigitsCount] = (byte)'\0';
					}
				}

				while (true)
				{
					if (!IsWhite(ch) || (styles & NumberStyles.AllowTrailingWhite) == 0)
					{
						if ((styles & NumberStyles.AllowTrailingSign) != 0 && !state.Contains(StateSign) && ((next = MatchChars(p, strEnd, positiveSign)) != null || (((next = MatchChars(p, strEnd, negativeSign)) != null) && (number.IsNegative = true))))
						{
							state.Add(StateSign);
							p = next - 1;
						}
						else if (ch == ')' && state.Contains(StateParens))
						{
							state.Remove(StateParens);
						}
						else if (!currSymbol.IsEmpty && (next = MatchChars(p, strEnd, currSymbol)) != null)
						{
							currSymbol = [];
							p = next - 1;
						}
						else
						{
							break;
						}
					}
					ch = ++p < strEnd ? (uint)(*p) : '\0';
				}
				if (!state.Contains(StateParens))
				{
					if (!state.Contains(StateNonZero))
					{
						number.Scale = 0;
						
						if ((!number.IsFloating) && !state.Contains(StateDecimal))
						{
							number.IsNegative = false;
						}
					}
					str = p;
					return true;
				}
			}
			str = p;
			return false;
		}

		private static TBits GetFloatBits<TFloat, TBits>(ref NumberInfo number)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
		{
			int normalMantissaBits = TFloat.NormalMantissaBits;
			int overflowDecimalExponent = TFloat.OverflowDecimalExponent;

			uint totalDigits = (uint)(number.DigitsCount);
			uint positiveExponent = (uint)(Math.Max(0, number.Scale));

			uint integerDigitsPresent = Math.Min(positiveExponent, totalDigits);
			uint fractionalDigitsPresent = totalDigits - integerDigitsPresent;

			// To generate an N bit mantissa we require N + 1 bits of precision.  The
			// extra bit is used to correctly round the mantissa (if there are fewer bits
			// than this available, then that's totally okay; in that case we use what we
			// have and we don't need to round).
			uint requiredBitsOfPrecision = (uint)(normalMantissaBits + 1);

			uint integerDigitsMissing = positiveExponent - integerDigitsPresent;

			const uint IntegerFirstIndex = 0;
			uint integerLastIndex = integerDigitsPresent;

			uint fractionalFirstIndex = integerLastIndex;
			uint fractionalLastIndex = totalDigits;

			// First, we accumulate the integer part of the mantissa into a BigNumber:
			AccumulateDecimalDigitsIntoBigNumber(ref number, IntegerFirstIndex, integerLastIndex, out BigNumber integerValue);

			if (integerDigitsMissing > 0)
			{
				if (integerDigitsMissing > overflowDecimalExponent)
				{
					return TFloat.PositiveInfinityBits;
				}

				integerValue.MultiplyPow10(integerDigitsMissing);
			}

			// At this point, the integerValue contains the value of the integer part
			// of the mantissa.  If either [1] this number has more than the required
			// number of bits of precision or [2] the mantissa has no fractional part,
			// then we can assemble the result immediately:
			uint integerBitsOfPrecision = BigNumber.CountSignificantBits(ref integerValue);

			if ((integerBitsOfPrecision >= requiredBitsOfPrecision) || (fractionalDigitsPresent == 0))
			{
				return ConvertBigIntegerToFloatingPointBits<TFloat, TBits>(
					ref integerValue,
					integerBitsOfPrecision,
					fractionalDigitsPresent != 0
				);
			}

			// Otherwise, we did not get enough bits of precision from the integer part,
			// and the mantissa has a fractional part.  We parse the fractional part of
			// the mantissa to obtain more bits of precision.  To do this, we convert
			// the fractional part into an actual fraction N/M, where the numerator N is
			// computed from the digits of the fractional part, and the denominator M is
			// computed as the power of 10 such that N/M is equal to the value of the
			// fractional part of the mantissa.

			uint fractionalDenominatorExponent = fractionalDigitsPresent;

			if (number.Scale < 0)
			{
				fractionalDenominatorExponent += (uint)(-number.Scale);
			}

			if ((integerBitsOfPrecision == 0) && (fractionalDenominatorExponent - (int)(totalDigits)) > overflowDecimalExponent)
			{
				// If there were any digits in the integer part, it is impossible to
				// underflow (because the exponent cannot possibly be small enough),
				// so if we underflow here it is a true underflow and we return zero.
				return TFloat.PositiveZeroBits;
			}

			AccumulateDecimalDigitsIntoBigNumber(ref number, fractionalFirstIndex, fractionalLastIndex, out BigNumber fractionalNumerator);

			if (fractionalNumerator.IsZero())
			{
				return ConvertBigIntegerToFloatingPointBits<TFloat, TBits>(
					ref integerValue,
					integerBitsOfPrecision,
					fractionalDigitsPresent != 0
				);
			}

			BigNumber.Pow10(fractionalDenominatorExponent, out BigNumber fractionalDenominator);

			// Because we are using only the fractional part of the mantissa here, the
			// numerator is guaranteed to be smaller than the denominator.  We normalize
			// the fraction such that the most significant bit of the numerator is in
			// the same position as the most significant bit in the denominator.  This
			// ensures that when we later shift the numerator N bits to the left, we
			// will produce N bits of precision.
			uint fractionalNumeratorBits = BigNumber.CountSignificantBits(ref fractionalNumerator);
			uint fractionalDenominatorBits = BigNumber.CountSignificantBits(ref fractionalDenominator);

			uint fractionalShift = 0;

			if (fractionalDenominatorBits > fractionalNumeratorBits)
			{
				fractionalShift = fractionalDenominatorBits - fractionalNumeratorBits;
			}

			if (fractionalShift > 0)
			{
				fractionalNumerator.ShiftLeft(fractionalShift);
			}

			uint requiredFractionalBitsOfPrecision = requiredBitsOfPrecision - integerBitsOfPrecision;
			uint remainingBitsOfPrecisionRequired = requiredFractionalBitsOfPrecision;

			if (integerBitsOfPrecision > 0)
			{
				// If the fractional part of the mantissa provides no bits of precision
				// and cannot affect rounding, we can just take whatever bits we got from
				// the integer part of the mantissa.  This is the case for numbers like
				// 5.0000000000000000000001, where the significant digits of the fractional
				// part start so far to the right that they do not affect the floating
				// point representation.
				//
				// If the fractional shift is exactly equal to the number of bits of
				// precision that we require, then no fractional bits will be part of the
				// result, but the result may affect rounding.  This is e.g. the case for
				// large, odd integers with a fractional part greater than or equal to .5.
				// Thus, we need to do the division to correctly round the result.
				if (fractionalShift > remainingBitsOfPrecisionRequired)
				{
					return ConvertBigIntegerToFloatingPointBits<TFloat, TBits>(
						ref integerValue,
						integerBitsOfPrecision,
						fractionalDigitsPresent != 0
					);
				}

				remainingBitsOfPrecisionRequired -= fractionalShift;
			}

			// If there was no integer part of the mantissa, we will need to compute the
			// exponent from the fractional part.  The fractional exponent is the power
			// of two by which we must multiply the fractional part to move it into the
			// range [1.0, 2.0).  This will either be the same as the shift we computed
			// earlier, or one greater than that shift:
			uint fractionalExponent = fractionalShift;

			if (BigNumber.Compare(ref fractionalNumerator, ref fractionalDenominator) < 0)
			{
				fractionalExponent++;
			}

			fractionalNumerator.ShiftLeft(remainingBitsOfPrecisionRequired);


			BigNumber.DivRem(ref fractionalNumerator, ref fractionalDenominator, out BigNumber bigFractionalMantissa, out BigNumber fractionalRemainder);


			TBits fractionalMantissa = bigFractionalMantissa.ToUInt<TBits>();
			bool hasZeroTail = !number.HasNonZeroTail && fractionalRemainder.IsZero();

			// We may have produced more bits of precision than were required.  Check,
			// and remove any "extra" bits:
			uint fractionalMantissaBits = BigNumber.CountSignificantBits(fractionalMantissa);
			if (fractionalMantissaBits > requiredFractionalBitsOfPrecision)
			{
				int shift = (int)(fractionalMantissaBits - requiredFractionalBitsOfPrecision);
				hasZeroTail = hasZeroTail && (fractionalMantissa & ((TBits.One << shift) - TBits.One)) == TBits.Zero;
				fractionalMantissa >>= shift;
			}


			// Compose the mantissa from the integer and fractional parts:
			TBits integerMantissa = integerValue.ToUInt<TBits>();
			TBits completeMantissa = (integerMantissa << (int)(requiredFractionalBitsOfPrecision)) + fractionalMantissa;

			// Compute the final exponent:
			// * If the mantissa had an integer part, then the exponent is one less than
			//   the number of bits we obtained from the integer part.  (It's one less
			//   because we are converting to the form 1.11111, with one 1 to the left
			//   of the decimal point.)
			// * If the mantissa had no integer part, then the exponent is the fractional
			//   exponent that we computed.
			// Then, in both cases, we subtract an additional one from the exponent, to
			// account for the fact that we've generated an extra bit of precision, for
			// use in rounding.
			int finalExponent = (integerBitsOfPrecision > 0) ? (int)(integerBitsOfPrecision) - 2 : -(int)(fractionalExponent) - 1;

			return AssembleFloatingPointBits<TFloat, TBits>(completeMantissa, finalExponent, hasZeroTail);
		}

		private unsafe static TBits ConvertBigIntegerToFloatingPointBits<TFloat, TBits>(ref BigNumber value, uint integerBitsOfPrecision, bool hasNonZeroFractionalPart)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
		{
			int baseExponent = TFloat.DenormalMantissaBits;
			int size = sizeof(TBits) * 8;

			// When we have N-bits or less of precision, we can just get the mantissa directly
			if (integerBitsOfPrecision <= size)
			{
				return AssembleFloatingPointBits<TFloat, TBits>(value.ToUInt<TBits>(), baseExponent, !hasNonZeroFractionalPart);
			}

			(uint topBlockIndex, uint topBlockBits) = Math.DivRem(integerBitsOfPrecision, 64);
			uint secondTopBlockIndex = topBlockIndex - 1;
			uint middleBlockIndex = secondTopBlockIndex - 1;
			uint bottomBlockIndex = middleBlockIndex - 1;

			TBits mantissa;
			int exponent;
			bool hasZeroTail = !hasNonZeroFractionalPart;

			// When the top N-bits perfectly span two blocks, we can get those blocks directly
			if (topBlockBits == 0)
			{
				exponent = baseExponent + ((int)(bottomBlockIndex) * 64);

				var secondTopBlock = value.GetBlock(secondTopBlockIndex);
				var middleBlock = value.GetBlock(middleBlockIndex);
				if (typeof(TBits) == typeof(UInt128))
				{
					if (secondTopBlock == 0 && middleBlock == 0)
					{
						mantissa = TBits.CreateChecked(new UInt128(value.GetBlock(bottomBlockIndex), value.GetBlock(bottomBlockIndex - 1)));
						goto END;
					}
				}
				else
				{
					mantissa = TBits.CreateChecked(new UInt256(secondTopBlock, middleBlock, value.GetBlock(bottomBlockIndex), value.GetBlock(bottomBlockIndex - 1)));
					goto END;
				}
			}
			if(typeof(TBits) == typeof(UInt128))
			{
				// Otherwise, we need to read three blocks and combine them into a 128-bit mantissa
			
				exponent = baseExponent + ((int)(middleBlockIndex) * 64);
				int bottomBlockShift = (int)(topBlockBits);
				int topBlockShift = size - bottomBlockShift;
				int middleBlockShift = topBlockShift - 64;

				exponent += (int)(topBlockBits);

				ulong bottomBlock = value.GetBlock(middleBlockIndex);
				ulong bottomBits = bottomBlock >> bottomBlockShift;

				TBits middleBits = TBits.CreateChecked(value.GetBlock(secondTopBlockIndex)) << middleBlockShift;
				TBits topBits = TBits.CreateChecked(value.GetBlock(topBlockIndex)) << topBlockShift;

				mantissa = topBits + middleBits + TBits.CreateChecked(bottomBits);

				ulong unusedBottomBlockBitsMask = (1UL << (int)(topBlockBits)) - 1;
				hasZeroTail &= (bottomBlock & unusedBottomBlockBitsMask) == 0;
			}
			else // typeof(TBits) == typeof(UInt256)
			{
				// Otherwise, we need to read five blocks and combine them into a 128-bit mantissa
				exponent = baseExponent + ((int)(bottomBlockIndex) * 64);

				int bottomBlockShift = (int)(topBlockBits);
				int topBlockShift = 256 - bottomBlockShift;
				int secondTopBlockShift = topBlockShift - 64;
				int middleBlockShift = secondTopBlockShift - 64;

				exponent += (int)(topBlockBits);

				ulong bottomBlock = value.GetBlock(bottomBlockIndex);
				ulong bottomBits = bottomBlock >> bottomBlockShift;

				TBits middleBits = TBits.CreateChecked(value.GetBlock(middleBlockIndex)) << middleBlockShift;
				TBits secondTopBits = TBits.CreateChecked(value.GetBlock(secondTopBlockIndex)) << secondTopBlockShift;
				TBits topBits = TBits.CreateChecked(value.GetBlock(topBlockIndex)) << topBlockShift;

				mantissa = topBits + secondTopBits + middleBits + TBits.CreateChecked(bottomBits);

				ulong unusedBottomBlockBitsMask = (1u << (int)(topBlockBits)) - 1;
				hasZeroTail &= (bottomBlock & unusedBottomBlockBitsMask) == 0;
			}
		END:
			for (uint i = 0; i != bottomBlockIndex; i++)
			{
				hasZeroTail &= (value.GetBlock(i) == 0);
			}

			return AssembleFloatingPointBits<TFloat, TBits>(mantissa, exponent, hasZeroTail);
		}

		private static TBits AssembleFloatingPointBits<TFloat, TBits>(TBits initialMantissa, int initialExponent, bool hasZeroTail)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TBits>
			where TBits : unmanaged, IBinaryInteger<TBits>, IUnsignedNumber<TBits>
		{
			int denormalMantissaBits = TFloat.DenormalMantissaBits;
			int normalMantissaBits = TFloat.NormalMantissaBits;
			// number of bits by which we must adjust the mantissa to shift it into the
			// correct position, and compute the resulting base two exponent for the
			// normalized mantissa:
			uint initialMantissaBits = BigNumber.CountSignificantBits(initialMantissa);
			int normalMantissaShift = normalMantissaBits - (int)(initialMantissaBits);
			int normalExponent = initialExponent - normalMantissaShift;

			TBits mantissa = initialMantissa;
			int exponent = normalExponent;

			if (normalExponent > TFloat.MaxBiasedExponent)
			{
				// The exponent is too large to be represented by the floating point
				// type; report the overflow condition:
				return TFloat.PositiveInfinityBits;
			}
			else if (normalExponent < (1 - TFloat.MaxBiasedExponent))
			{
				// The exponent is too small to be represented by the floating point
				// type as a normal value, but it may be representable as a denormal
				// value.  Compute the number of bits by which we need to shift the
				// mantissa in order to form a denormal number.  (The subtraction of
				// an extra 1 is to account for the hidden bit of the mantissa that
				// is not available for use when representing a denormal.)
				int denormalMantissaShift = normalMantissaShift + normalExponent + TFloat.MaxBiasedExponent - 1;

				// Denormal values have an exponent of zero, so the debiased exponent is
				// the negation of the exponent bias:
				exponent = -TFloat.MaxBiasedExponent;

				if (denormalMantissaShift < 0)
				{
					// Use two steps for right shifts:  for a shift of N bits, we first
					// shift by N-1 bits, then shift the last bit and use its value to
					// round the mantissa.
					mantissa = RightShiftWithRounding(mantissa, -denormalMantissaShift, hasZeroTail);

					// If the mantissa is now zero, we have underflowed:
					if (mantissa == TBits.Zero)
					{
						return TFloat.PositiveZeroBits;
					}

					// When we round the mantissa, the result may be so large that the
					// number becomes a normal value.  For example, consider the single
					// precision case where the mantissa is 0x01ffffff and a right shift
					// of 2 is required to shift the value into position. We perform the
					// shift in two steps:  we shift by one bit, then we shift again and
					// round using the dropped bit.  The initial shift yields 0x00ffffff.
					// The rounding shift then yields 0x007fffff and because the least
					// significant bit was 1, we add 1 to this number to round it.  The
					// final result is 0x00800000.
					//
					// 0x00800000 is 24 bits, which is more than the 23 bits available
					// in the mantissa.  Thus, we have rounded our denormal number into
					// a normal number.
					//
					// We detect this case here and re-adjust the mantissa and exponent
					// appropriately, to form a normal number:
					if (mantissa > TFloat.DenormalMantissaMask)
					{
						// We add one to the denormalMantissaShift to account for the
						// hidden mantissa bit (we subtracted one to account for this bit
						// when we computed the denormalMantissaShift above).
						exponent = initialExponent - (denormalMantissaShift + 1) - normalMantissaShift;
					}
				}
				else
				{
					mantissa <<= denormalMantissaShift;
				}
			}
			else
			{
				if (normalMantissaShift < 0)
				{
					// Use two steps for right shifts:  for a shift of N bits, we first
					// shift by N-1 bits, then shift the last bit and use its value to
					// round the mantissa.
					mantissa = RightShiftWithRounding(mantissa, -normalMantissaShift, hasZeroTail);

					// When we round the mantissa, it may produce a result that is too
					// large.  In this case, we divide the mantissa by two and increment
					// the exponent (this does not change the value).
					if (mantissa > TFloat.NormalMantissaMask)
					{
						mantissa >>= 1;
						exponent++;

						// The increment of the exponent may have generated a value too
						// large to be represented.  In this case, report the overflow:
						if (exponent > TFloat.MaxBiasedExponent)
						{
							return TFloat.PositiveInfinityBits;
						}
					}
				}
				else if (normalMantissaShift > 0)
				{
					mantissa <<= normalMantissaShift;
				}
			}

			mantissa &= TFloat.TrailingSignificandMask;

			TBits shiftedExponent = (TBits.CreateChecked(exponent + TFloat.ExponentBias)) << denormalMantissaBits;

			return shiftedExponent | mantissa;
		}

		private unsafe static T RightShiftWithRounding<T>(T value, int shift, bool hasZeroTail)
			where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>
		{
			// If we'd need to shift further than it is possible to shift, the answer
			// is always zero:
			if (shift >= (sizeof(T) * 8))
			{
				return T.Zero;
			}

			T one = T.One;
			T extraBitsMask = (one << (shift - 1)) - one;
			T roundBitMask = (one << (shift - 1));
			T lsbBitMask = one << shift;

			T zero = T.Zero;
			bool lsbBit = (value & lsbBitMask) != zero;
			bool roundBit = (value & roundBitMask) != zero;
			bool hasTailBits = !hasZeroTail || (value & extraBitsMask) != zero;

			return (value >> shift) + (ShouldRoundUp(lsbBit, roundBit, hasTailBits) ? one : zero);
		}

		private static bool ShouldRoundUp(bool lsbBit, bool roundBit, bool hasTailBits)
		{
			// If there are insignificant set bits, we need to round to the
			// nearest; there are two cases:
			// we round up if either [1] the value is slightly greater than the midpoint
			// between two exactly representable values or [2] the value is exactly the
			// midpoint between two exactly representable values and the greater of the
			// two is even (this is "round-to-even").
			return roundBit && (hasTailBits || lsbBit);
		}

		private static unsafe void AccumulateDecimalDigitsIntoBigNumber(scoped ref NumberInfo number, uint firstIndex, uint lastIndex, out BigNumber result)
		{
			BigNumber.SetZero(out result);

			byte* src = number.GetDigitsPointer() + firstIndex;
			uint remaining = lastIndex - firstIndex;

			while (remaining != 0)
			{
				uint count = Math.Min(remaining, 19);
				ulong value = DigitsToUInt64(src, (int)(count));

				result.MultiplyPow10(count);
				result.Add(value);

				src += count;
				remaining -= count;
			}
		}

		// get 64-bit integer from at most 19 digits
		private static unsafe ulong DigitsToUInt64(byte* p, int count)
		{
			Debug.Assert((1 <= count) && (count <= 19));

			byte* end = (p + count);
			ulong res = 0;

			// parse batches of 8 digits with SWAR
			while (end - p >= 8)
			{
				res = (res * 100000000) + ParseEightDigitsUnrolled(p);
				p += 8;
			}

			while (p != end)
			{
				res = (10 * res) + p[0] - '0';
				++p;
			}

			return res;
		}

		private static unsafe uint ParseEightDigitsUnrolled(byte* chars)
		{
			// let's take the following value (byte*) 12345678 and read it unaligned :
			// we get a ulong value of 0x3837363534333231
			// 1. Subtract character '0' 0x30 for each byte to get 0x0807060504030201
			// 2. Consider this sequence as bytes sequence : b8b7b6b5b4b3b2b1
			// we need to transform it to b1b2b3b4b5b6b7b8 by computing :
			// 10000 * (100 * (10*b1+b2) + 10*b3+b4) + 100*(10*b5+b6) + 10*b7+b8
			// this is achieved by masking and shifting values
			ulong val = Unsafe.ReadUnaligned<ulong>(chars);

			// With BigEndian system an endianness swap has to be performed
			// before the following operations as if it has been read with LittleEndian system
			if (!BitConverter.IsLittleEndian)
			{
				val = BinaryPrimitives.ReverseEndianness(val);
			}

			const ulong mask = 0x000000FF000000FF;
			const ulong mul1 = 0x000F424000000064; // 100 + (1000000ULL << 32)
			const ulong mul2 = 0x0000271000000001; // 1 + (10000ULL << 32)
			val -= 0x3030303030303030;
			val = (val * 10) + (val >> 8); // val = (val * 2561) >> 8;
			val = (((val & mask) * mul1) + (((val >> 16) & mask) * mul2)) >> 32;
			return (uint)val;
		}

		internal unsafe byte* GetDigitsPointer()
		{
			return (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(Digits));
		}
		internal ref byte GetDigitsReference()
		{
			return ref MemoryMarshal.GetReference(Digits);
		}

		private static bool IsWhite(uint ch) => (ch == 0x20) || ((ch - 0x09) <= (0x0D - 0x09));

		private static bool IsDigit(uint ch) => (ch - '0') <= 9;

		private static bool IsSpaceReplacingChar(uint c) => (c == '\u00a0') || (c == '\u202f');

		[MethodImpl(MethodImplOptions.NoInlining)] // rare slow path that shouldn't impact perf of the main use case
		private static bool TrailingZeros<TChar>(ReadOnlySpan<TChar> value, int index)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			// For compatibility, we need to allow trailing zeros at the end of a number string
			return !value.Slice(index).ContainsAnyExcept((TChar)('\0'));
		}

		private static unsafe TChar* MatchChars<TChar>(TChar* p, TChar* pEnd, ReadOnlySpan<TChar> value)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Debug.Assert((p != null) && (pEnd != null) && (p <= pEnd));

			fixed (TChar* stringPointer = &MemoryMarshal.GetReference(value))
			{
				TChar* str = stringPointer;

				if ((uint)(*str) != '\0')
				{
					// We only hurt the failure case
					// This fix is for French or Kazakh cultures. Since a user cannot type 0xA0 or 0x202F as a
					// space character we use 0x20 space character instead to mean the same.
					while (true)
					{
						uint cp = (p < pEnd) ? (uint)(*p) : '\0';
						uint val = (uint)(*str);

						if ((cp != val) && !(IsSpaceReplacingChar(val) && (cp == '\u0020')))
						{
							break;
						}

						p++;
						str++;

						if ((uint)(*str) == '\0')
						{
							return p;
						}
					}
				}
			}

			return null;
		}
		private static unsafe TChar* MatchChars<TChar>(TChar* p, TChar* pEnd, ReadOnlySpan<char> value)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Debug.Assert((p != null) && (pEnd != null) && (p <= pEnd));

			fixed (char* stringPointer = &MemoryMarshal.GetReference(value))
			{
				char* str = stringPointer;

				if ((uint)(*str) != '\0')
				{
					// We only hurt the failure case
					// This fix is for French or Kazakh cultures. Since a user cannot type 0xA0 or 0x202F as a
					// space character we use 0x20 space character instead to mean the same.
					while (true)
					{
						uint cp = (p < pEnd) ? (uint)(*p) : '\0';
						uint val = (uint)(*str);

						if ((cp != val) && !(IsSpaceReplacingChar(val) && (cp == '\u0020')))
						{
							break;
						}

						p++;
						str++;

						if ((uint)(*str) == '\0')
						{
							return p;
						}
					}
				}
			}

			return null;
		}
		private static unsafe TChar* MatchChars<TChar>(TChar* p, TChar* pEnd, ReadOnlySpan<byte> value)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			Debug.Assert((p != null) && (pEnd != null) && (p <= pEnd));

			fixed (byte* stringPointer = &MemoryMarshal.GetReference(value))
			{
				byte* str = stringPointer;

				if ((uint)(*str) != '\0')
				{
					// We only hurt the failure case
					// This fix is for French or Kazakh cultures. Since a user cannot type 0xA0 or 0x202F as a
					// space character we use 0x20 space character instead to mean the same.
					while (true)
					{
						uint cp = (p < pEnd) ? (uint)(*p) : '\0';
						uint val = (uint)(*str);

						if ((cp != val) && !(IsSpaceReplacingChar(val) && (cp == '\u0020')))
						{
							break;
						}

						p++;
						str++;

						if ((uint)(*str) == '\0')
						{
							return p;
						}
					}
				}
			}

			return null;
		}
	}
}
