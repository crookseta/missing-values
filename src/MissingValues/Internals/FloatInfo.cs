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

	internal ref struct FloatInfo
	{
		public int DigitsCount;
		public int Scale;
		public bool IsNegative;
		public bool HasNonZeroTail;
		public Span<byte> Digits;

        public FloatInfo(Span<byte> digits)
        {
			DigitsCount = 0;
			Scale = 0;
			IsNegative = false;
			HasNonZeroTail = false;
			Digits = digits;
			Digits[0] = (byte)'\0';
		}

		public static TFloat ConvertToFloat<TFloat>(ref FloatInfo number)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
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
				UInt128 bits = GetFloatBits<TFloat>(ref number);
				result = TFloat.BitsToFloat(bits);
			}

			return number.IsNegative ? -result : result;
		}

		public static unsafe bool TryParse<TChar>(ReadOnlySpan<TChar> s, ref FloatInfo info, NumberFormatInfo formatInfo, NumberStyles styles = NumberStyles.Float)
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			const int StateSign = 0x0001;
			const int StateParens = 0x0002;
			const int StateDigits = 0x0004;
			const int StateNonZero = 0x0008;
			const int StateDecimal = 0x0010;


			Span<TChar> decSep = stackalloc TChar[TChar.GetLength(formatInfo.NumberDecimalSeparator)];
			Span<TChar> groupSep = stackalloc TChar[TChar.GetLength(formatInfo.NumberGroupSeparator)];
			Span<TChar> positiveSign = stackalloc TChar[TChar.GetLength(formatInfo.PositiveSign)];
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(formatInfo.NegativeSign)];

			TChar.Copy(formatInfo.NumberDecimalSeparator, decSep);
			TChar.Copy(formatInfo.NumberGroupSeparator, groupSep);
			TChar.Copy(formatInfo.PositiveSign, positiveSign);
			TChar.Copy(formatInfo.NegativeSign, negativeSign);

			State<int> state = 0;

			if ((styles & NumberStyles.AllowLeadingWhite) != 0)
			{
				s = s.TrimStart(TChar.WhiteSpaceCharacter);
			}
			if ((styles & NumberStyles.AllowLeadingWhite) != 0)
			{
				s = s.TrimEnd(TChar.WhiteSpaceCharacter);
			}

			int totalLength = s.Length;
			int curIndex = 0;
			ref TChar ptr = ref MemoryMarshal.GetReference(s);
			TChar ch = curIndex < totalLength ? ptr : TChar.NullCharacter;

			int digCount = 0;
			int digEnd = 0;
			int maxDigCount = info.Digits.Length - 1;
			int numberOfTrailingZeros = 0;

			if ((styles & NumberStyles.AllowLeadingSign) != 0)
			{
				if (s.IndexOf(positiveSign) == 0)
				{
					info.IsNegative = false;
					state.Add(StateSign);

					curIndex++;
					ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
					ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
				}
				else if (s.IndexOf(negativeSign) == 0)
				{
					info.IsNegative = true;
					state.Add(StateSign);

					curIndex++;
					ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
					ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
				}
			}

			while (true)
			{
				if (TChar.IsDigit(ch))
				{
					state.Add(StateDigits);
					if (ch != (TChar)'0' || state.Contains(StateNonZero))
					{
						if (digCount < maxDigCount)
						{
							info.Digits[digCount] = (byte)ch;
							digEnd = digCount + 1;

						}
						else if (ch != (TChar)'0')
						{
							// For decimal and binary floating-point numbers, we only
							// need to store digits up to maxDigCount. However, we still
							// need to keep track of whether any additional digits past
							// maxDigCount were non-zero, as that can impact rounding
							// for an input that falls evenly between two representable
							// results.

							info.HasNonZeroTail = true;
						}

						if (!state.Contains(StateDecimal))
						{
							info.Scale++;
						}

						if (digCount < maxDigCount)
						{
							// Handle a case like "53.0". We need to ignore trailing zeros in the fractional part for floating point numbers,
							// so we keep a count of the number of trailing zeros and update digCount later
							if (ch == (TChar)'0')
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
						info.Scale--;
					}
				}
				else if (((styles & NumberStyles.AllowDecimalPoint) != 0) && (!state.Contains(StateDecimal)) && (TChar.Constains(s[curIndex..], decSep, StringComparison.OrdinalIgnoreCase)))
				{
					state.Add(StateDecimal);
					int decSepIndex = s.IndexOf(decSep);
					ptr = ref MemoryMarshal.GetReference(s[decSepIndex..]);
					curIndex = decSepIndex;
				}
				else if (((styles & NumberStyles.AllowThousands) != 0) && (state.Contains(StateDigits)) && (!state.Contains(StateDecimal)) && (TChar.Constains(s[curIndex..], groupSep, StringComparison.OrdinalIgnoreCase)))
				{
					int groupSepIndex = s[curIndex..].IndexOf(groupSep) + curIndex;
					ptr = ref MemoryMarshal.GetReference(s[groupSepIndex..]);
					curIndex = groupSepIndex;
				}
				else
				{
					break;
				}

				curIndex++;
				ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
				ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
			}

			bool negExp = false;
			info.DigitsCount = digEnd;
			info.Digits[digEnd] = (byte)'\0';
			if (state.Contains(StateDigits))
			{
				if ((ch == (TChar)'E' || ch == (TChar)'e') && ((styles & NumberStyles.AllowExponent) != 0))
				{
					ref TChar temp = ref ptr;
					curIndex++;
					ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
					ch = curIndex < totalLength ? ptr : TChar.NullCharacter;

					if (s[curIndex..].IndexOf(positiveSign) == 0)
					{
						curIndex++;
						ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
						ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
					}
					else if (s[curIndex..].IndexOf(negativeSign) == 0)
					{
						curIndex++;
						ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
						ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
						negExp = true;
					}

					if (TChar.IsDigit(ch))
					{
						int exp = 0;
						do
						{
							exp = (exp * 10) + ((char)ch - '0');
							curIndex++;
							ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
							ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
							if (exp > 5000)
							{
								exp = 9999;
								while (TChar.IsDigit(ch))
								{
									curIndex++;
									ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
									ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
								}
							}
						} while (TChar.IsDigit(ch));

						if (negExp)
						{
							exp = -exp;
						}
						info.Scale += exp;
					}
					else
					{
						ptr = ref temp;
					}
				}

				if (!info.HasNonZeroTail)
				{
					int numberOfFractionalDigits = digEnd - info.Scale;
					if (numberOfFractionalDigits > 0)
					{
						numberOfTrailingZeros = Math.Min(numberOfTrailingZeros, numberOfFractionalDigits);
						Debug.Assert(numberOfTrailingZeros >= 0);
						info.DigitsCount = digEnd - numberOfTrailingZeros;
						info.Digits[info.DigitsCount] = (byte)'\0';
					}
				}

				while (true)
				{
					if (!TChar.IsWhiteSpace(ch) || (styles & NumberStyles.AllowTrailingWhite) == 0)
					{
						int tempIndex = 0;
						if ((styles & NumberStyles.AllowTrailingSign) != 0 && (!state.Contains(StateSign)) && ((tempIndex = s[curIndex..].IndexOf(positiveSign)) >= 0 || (tempIndex = s[curIndex..].IndexOf(negativeSign)) >= 0) && (info.IsNegative = true))
						{
							state.Add(StateSign);
							tempIndex += curIndex;
							curIndex++;
							ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, tempIndex - curIndex) : ref Unsafe.NullRef<TChar>();
							ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
						}
						else if (ch == (TChar)')' && state.Contains(StateParens))
						{
							state.Remove(StateParens);
						}
						else
						{
							break;
						}
					}
					curIndex++;
					ptr = ref curIndex < totalLength ? ref Unsafe.Add(ref ptr, 1) : ref Unsafe.NullRef<TChar>();
					ch = curIndex < totalLength ? ptr : TChar.NullCharacter;
				}

				if (!state.Contains(StateParens))
				{
					if (!state.Contains(StateNonZero))
					{
						info.Scale = 0;
					}

					// Check if we got any value after parsing, if there was any invalid character
					// this will return false
					if (s[curIndex..].IndexOfAnyExcept(TChar.NullCharacter) >= 0)
					{
						return false;
					}

					return true;
				}
			}
			return false;
		}

		private static UInt128 GetFloatBits<TFloat>(ref FloatInfo number)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
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
				return ConvertBigIntegerToFloatingPointBits<TFloat>(
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
				return ConvertBigIntegerToFloatingPointBits<TFloat>(
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
					return ConvertBigIntegerToFloatingPointBits<TFloat>(
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


			UInt128 fractionalMantissa = bigFractionalMantissa.ToUInt128();
			bool hasZeroTail = !number.HasNonZeroTail && fractionalRemainder.IsZero();

			// We may have produced more bits of precision than were required.  Check,
			// and remove any "extra" bits:
			uint fractionalMantissaBits = BigNumber.CountSignificantBits(fractionalMantissa);
			if (fractionalMantissaBits > requiredFractionalBitsOfPrecision)
			{
				int shift = (int)(fractionalMantissaBits - requiredFractionalBitsOfPrecision);
				hasZeroTail = hasZeroTail && (fractionalMantissa & ((UInt128.One << shift) - UInt128.One)) == 0;
				fractionalMantissa >>= shift;
			}


			// Compose the mantissa from the integer and fractional parts:
			UInt128 integerMantissa = integerValue.ToUInt128();
			UInt128 completeMantissa = (integerMantissa << (int)(requiredFractionalBitsOfPrecision)) + fractionalMantissa;

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

			return AssembleFloatingPointBits<TFloat>(completeMantissa, finalExponent, hasZeroTail);
		}

		private static UInt128 ConvertBigIntegerToFloatingPointBits<TFloat>(ref BigNumber value, uint integerBitsOfPrecision, bool hasNonZeroFractionalPart)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
		{
			int baseExponent = TFloat.DenormalMantissaBits;

			// When we have 128-bits or less of precision, we can just get the mantissa directly
			if (integerBitsOfPrecision <= 128)
			{
				return AssembleFloatingPointBits<TFloat>(value.ToUInt128(), baseExponent, !hasNonZeroFractionalPart);
			}

			(uint topBlockIndex, uint topBlockBits) = Math.DivRem(integerBitsOfPrecision, 64);
			uint middleBlockIndex = topBlockIndex - 1;
			uint bottomBlockIndex = middleBlockIndex - 1;

			UInt128 mantissa;
			int exponent = baseExponent + ((int)(bottomBlockIndex) * 64);
			bool hasZeroTail = !hasNonZeroFractionalPart;

			// When the top 128-bits perfectly span two blocks, we can get those blocks directly
			if (topBlockBits == 0)
			{
				mantissa = new UInt128(value.GetBlock(middleBlockIndex), value.GetBlock(bottomBlockIndex));
			}
			else
			{
				// Otherwise, we need to read three blocks and combine them into a 128-bit mantissa

				int bottomBlockShift = (int)(topBlockBits);
				int topBlockShift = 128 - bottomBlockShift;
				int middleBlockShift = topBlockShift - 64;

				exponent += (int)(topBlockBits);

				ulong bottomBlock = value.GetBlock(bottomBlockIndex);
				ulong bottomBits = bottomBlock >> bottomBlockShift;

				UInt128 middleBits = (UInt128)(value.GetBlock(middleBlockIndex)) << middleBlockShift;
				UInt128 topBits = (UInt128)(value.GetBlock(topBlockIndex)) << topBlockShift;

				mantissa = topBits + middleBits + bottomBits;

				ulong unusedBottomBlockBitsMask = (1UL << (int)(topBlockBits)) - 1;
				hasZeroTail &= (bottomBlock & unusedBottomBlockBitsMask) == 0;
			}

			for (uint i = 0; i != bottomBlockIndex; i++)
			{
				hasZeroTail &= (value.GetBlock(i) == 0);
			}

			return AssembleFloatingPointBits<TFloat>(mantissa, exponent, hasZeroTail);
		}

		private static UInt128 AssembleFloatingPointBits<TFloat>(UInt128 initialMantissa, int initialExponent, bool hasZeroTail)
			where TFloat : struct, IFormattableBinaryFloatingPoint<TFloat>
		{
			int denormalMantissaBits = TFloat.DenormalMantissaBits;
			int normalMantissaBits = TFloat.NormalMantissaBits;
			// number of bits by which we must adjust the mantissa to shift it into the
			// correct position, and compute the resulting base two exponent for the
			// normalized mantissa:
			uint initialMantissaBits = BigNumber.CountSignificantBits(initialMantissa);
			int normalMantissaShift = normalMantissaBits - (int)(initialMantissaBits);
			int normalExponent = initialExponent - normalMantissaShift;

			UInt128 mantissa = initialMantissa;
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
					if (mantissa == 0)
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

			UInt128 shiftedExponent = ((UInt128)(exponent + TFloat.ExponentBias)) << denormalMantissaBits;

			return shiftedExponent | mantissa;
		}

		private static UInt128 RightShiftWithRounding(UInt128 value, int shift, bool hasZeroTail)
		{
			// If we'd need to shift further than it is possible to shift, the answer
			// is always zero:
			if (shift >= 128)
			{
				return 0;
			}

			UInt128 extraBitsMask = (UInt128.One << (shift - 1)) - 1;
			UInt128 roundBitMask = (UInt128.One << (shift - 1));
			UInt128 lsbBitMask = UInt128.One << shift;

			bool lsbBit = (value & lsbBitMask) != 0;
			bool roundBit = (value & roundBitMask) != 0;
			bool hasTailBits = !hasZeroTail || (value & extraBitsMask) != 0;

			return (value >> shift) + (ShouldRoundUp(lsbBit, roundBit, hasTailBits) ? 1UL : 0);
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

		private static unsafe void AccumulateDecimalDigitsIntoBigNumber(scoped ref FloatInfo number, uint firstIndex, uint lastIndex, out BigNumber result)
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

		private unsafe byte* GetDigitsPointer()
		{
			return (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(Digits));
		}
	}
}
