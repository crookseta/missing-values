using MissingValues.Internals;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info;

internal static partial class NumberFormatter
{
	/*
	 * Using Ryu algorith for Binary-to-decimal conversion
	 * Based on this implementation:
	 * https://github.com/ulfjack/ryu/tree/master
	 */
	internal static partial class Ryu
	{
		[InlineArray(4)]
		internal struct CachePower
		{
			private ulong _s0;

			internal CachePower(ulong S0, ulong S1)
			{
				this[0] = S0;
				this[1] = S1;
				this[2] = default;
				this[3] = default;
			}
			internal CachePower(ulong S0, ulong S1, ulong S2, ulong S3)
			{
				this[0] = S0;
				this[1] = S1;
				this[2] = S2;
				this[3] = S3;
			}
		}
		[InlineArray(8)]
		internal struct CachePowerSplit
		{
			private ulong _s0;

			internal CachePowerSplit(ulong S0, ulong S1, ulong S2, ulong S3)
			{
				this[0] = S0;
				this[1] = S1;
				this[2] = S2;
				this[3] = S3;
				this[4] = default;
				this[5] = default;
				this[6] = default;
				this[7] = default;
			}
			internal CachePowerSplit(ulong S0, ulong S1, ulong S2, ulong S3, ulong S4, ulong S5, ulong S6, ulong S7)
			{
				this[0] = S0;
				this[1] = S1;
				this[2] = S2;
				this[3] = S3;
				this[4] = S4;
				this[5] = S5;
				this[6] = S6;
				this[7] = S7;
			}
		}

		private record struct FloatingDecimal<TSignificand>(TSignificand Mantissa, int Exponent, bool Sign)
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			internal static ReadOnlySpan<CachePower> GenericPow5Table => typeof(TSignificand) == typeof(UInt128) ? CacheArraysForQuad.QuadPow5Table : CacheArraysForOcto.OctoPow5Table;

			internal static ReadOnlySpan<CachePowerSplit> GenericPow5Split => typeof(TSignificand) == typeof(UInt128) ? CacheArraysForQuad.QuadPow5Split : CacheArraysForOcto.OctoPow5Split;

			internal static ReadOnlySpan<CachePowerSplit> GenericPow5InvSplit => typeof(TSignificand) == typeof(UInt128) ? CacheArraysForQuad.QuadPow5InvSplit : CacheArraysForOcto.OctoPow5InvSplit;

			internal static ReadOnlySpan<ulong> Pow5Errors => typeof(TSignificand) == typeof(UInt128) ? CacheArraysForQuad.QuadPow5Errors : CacheArraysForOcto.OctoPow5Errors;

			internal static ReadOnlySpan<ulong> Pow5InvErrors => typeof(TSignificand) == typeof(UInt128) ? CacheArraysForQuad.QuadPow5InvErrors : CacheArraysForOcto.OctoPow5InvErrors;

			internal static uint Pow5InvBitCount => typeof(TSignificand) == typeof(UInt128) ? 249U : 501U;

			internal static uint Pow5BitCount => typeof(TSignificand) == typeof(UInt128) ? 249U : 501U;

			internal static uint Pow5TableSize => typeof(TSignificand) == typeof(UInt128) ? 56U : 111U;

			internal static TSignificand MulShift(TSignificand m, ReadOnlySpan<ulong> mul, int j)
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					Debug.Assert(j > 128);
					UInt128 mm = (UInt128)(object)m;
					ReadOnlySpan<ulong> a =
					[
						(ulong)mm,
						mm.Upper
					];
					Span<ulong> result = stackalloc ulong[4];
					Mul128To256Shift(a, mul, j, 0, result);
					return (TSignificand)(object)(new UInt128(result[1], result[0]));
				}
				else // typeof(TSignificand) == typeof(UInt256)
				{
					Debug.Assert(j > 256);
					UInt256 mm = (UInt256)(object)m;
					ReadOnlySpan<ulong> a =
					[
						mm.Part0,
						mm.Part1,
						mm.Part2,
						mm.Part3
					];
					Span<ulong> result = stackalloc ulong[8];
					Mul256To512Shift(a, mul, j, 0, result);
					return (TSignificand)(object)(new UInt256(result[3], result[2], result[1], result[0]));
				}
			}

			internal static uint Pow5Factor(TSignificand value)
			{
				var five = TSignificand.CreateTruncating(5);
				for (uint i = 0; value > TSignificand.Zero; ++i)
				{
					(value, TSignificand rem) = TSignificand.DivRem(value, five);
					if (rem != TSignificand.Zero)
					{
						return i;
					}
				}

				return 0;
			}
		}

		private const int FDExceptionalExponent = 0x7FFF_FFFF;

		internal static void Format<TFloat, TSignificand, TChar>(in TFloat value, scoped Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, out bool isExceptional, NumberFormatInfo? info)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			int digits = TFloat.MaxSignificandPrecision;
			if (!format.IsEmpty)
			{
				GetFormat(format, out int formatDigits);
				if (formatDigits >= 0)
				{
					digits = formatDigits;
				}
			}
			var fd = FloatToDecimal<TFloat, TSignificand>(in value, format.StartsWith("e", StringComparison.InvariantCultureIgnoreCase) ? digits : TFloat.MaxSignificandPrecision);
			charsWritten = ToCharSpan(in fd, destination, format, info is null ? NumberFormatInfo.CurrentInfo : info!, out isExceptional, TFloat.MaxSignificandPrecision);
		}
		internal static void Format<TFloat, TSignificand>(in TFloat value, scoped ref NumberInfo number, out bool isExceptional)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			var fd = FloatToDecimal<TFloat, TSignificand>(in value, TFloat.MaxSignificandPrecision);
			UInt256 significand = UInt256.CreateTruncating(fd.Mantissa);
			UIntToNumber(in significand, ref number);
			number.Scale += fd.Exponent;
			number.IsNegative = fd.Sign;
			isExceptional = fd.Exponent == FDExceptionalExponent;
		}

		private static int ToCharSpan<TSignificand, TChar>(in FloatingDecimal<TSignificand> v, Span<TChar> destination, ReadOnlySpan<char> format, NumberFormatInfo info, out bool isExceptional, int maxSignificandPrecision)
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (v.Exponent == FDExceptionalExponent)
			{
				isExceptional = true;
				if (v.Mantissa != TSignificand.Zero)
				{
					TChar.Copy(info.NaNSymbol, destination);
					return TChar.GetLength(info.NaNSymbol);
				}
				if (v.Sign)
				{
					TChar.Copy(info.NegativeInfinitySymbol, destination);
					return TChar.GetLength(info.NegativeInfinitySymbol);
				}
				else
				{
					TChar.Copy(info.PositiveInfinitySymbol, destination);
					return TChar.GetLength(info.PositiveInfinitySymbol);
				}
			}

			isExceptional = false;

			// Step 5: Print the decimal representation.
			ReadOnlySpan<TChar> negativeSign = info.NegativeSignTChar(stackalloc TChar[16]);

			char fmt = GetFormat(format, out int digits);
			if (digits < 0)
			{
				digits = maxSignificandPrecision;
			}

			int index = 0;
			if (v.Sign)
			{
				negativeSign.CopyTo(destination[index..]);
				index += negativeSign.Length;
			}

			TSignificand output = v.Mantissa;
			uint olength = (uint)(output switch
			{
				UInt128 temp => UInt128.CountDigits(temp),
				UInt256 temp => UInt256.CountDigits(in temp)
			});
			if (format.StartsWith("e", StringComparison.InvariantCultureIgnoreCase))
			{
				index = AsScientificNotation(in v, destination, index, olength, digits + 1, char.IsUpper(fmt), info, negativeSign);
			}
			else
			{
				int exp = v.Exponent;
				var ten = TSignificand.CreateTruncating(10);
				if (exp < 0)
				{
					int absExp = -exp;
					if ((uint)absExp >= maxSignificandPrecision)
					{
						index = AsScientificNotation(in v, destination, index, olength, digits + 1, char.IsUpper(fmt), info, negativeSign);
					}
					else
					{
						int i;
						int wholeLength = (int)olength - absExp;
						var numberDecimalSeparator = info.NumberDecimalSeparatorTChar(stackalloc TChar[16]);
						if (wholeLength <= 0)
						{
							int decLength = -wholeLength;
							
							destination[index++] = (TChar)'0';
							numberDecimalSeparator.CopyTo(destination.Slice(index));
							index += numberDecimalSeparator.Length;

							if (decLength > 1)
							{
								destination.Slice(index, decLength).Fill((TChar)'0');
								index += decLength;
							}
							
							for (i = (int)olength - 1; i >= 0; i--)
							{
								(output, TSignificand c) = TSignificand.DivRem(output, ten);
								if ((i + decLength) >= digits)
								{
									if ((i + decLength) == digits)
									{
										var five = TSignificand.CreateTruncating(5);
										if (c == five)
										{
											output += TSignificand.IsEvenInteger(output)
												? TSignificand.Zero
												: TSignificand.One;
										}
										else if (c > five)
										{
											output++;
										}
									}
									continue;
								}
								destination[i + index] = (TChar)(char)('0' + uint.CreateTruncating(c));
							}
							index += (int)olength;
						}
						else
						{
							int decLength = (int)olength - wholeLength;
							i = (int)olength - 1 + numberDecimalSeparator.Length;
							for (int j = decLength; j > 0; i--, j--)
							{
								(output, TSignificand c) = TSignificand.DivRem(output, ten);
								if (j > digits)
								{
									if (j == digits + 1)
									{
										var five = TSignificand.CreateTruncating(5);
										if (c == five)
										{
											output += TSignificand.IsEvenInteger(output)
												? TSignificand.Zero
												: TSignificand.One;
										}
										else if (c > five)
										{
											output++;
										}
									}
									continue;
								}
								destination[i + index] = (TChar)(char)('0' + uint.CreateTruncating(c));
							}
							
							numberDecimalSeparator.CopyTo(destination.Slice(i -= (numberDecimalSeparator.Length - 1)));
							i--;
							
							for (; i >= 0; i--)
							{
								(output, TSignificand c) = TSignificand.DivRem(output, ten);
								destination[i + index] = (TChar)(char)('0' + uint.CreateTruncating(c));
							}

							index += decLength;
							index += numberDecimalSeparator.Length;
							index += wholeLength;
						}
					}
				}
				else
				{
					if (exp + olength > maxSignificandPrecision)
					{
						index = AsScientificNotation(in v, destination, index, olength, digits + 1, char.IsUpper(fmt), info, negativeSign);
					}
					else
					{
						int i;
						for (i = (int)olength - 1; i >= 0; i--)
						{
							(output, TSignificand c) = TSignificand.DivRem(output, ten);
							destination[i + index] = (TChar)(char)('0' + uint.CreateTruncating(c));
						}

						index += (int)olength;
						i = exp;
						while (i > 0)
						{
							destination[(int)(olength + --i)] = (TChar)'0';
						}

						index += exp;
					}
				}
			}
			return index;

			static int AsScientificNotation(in FloatingDecimal<TSignificand> v, Span<TChar> destination, int index, uint digits, int precision, bool upper, NumberFormatInfo info, ReadOnlySpan<TChar> negativeSign)
			{
				var ten = TSignificand.CreateTruncating(10);
				precision = int.Min((int)digits, precision);
				TSignificand output = v.Mantissa;
				for (uint i = 0; i < precision - 1; i++)
				{
					(output, TSignificand c) = TSignificand.DivRem(output, ten);
					destination[(int)(index + precision - i)] = (TChar)(char)('0' + uint.CreateTruncating(c));
				}
				destination[index] = (TChar)(char)('0' + uint.CreateTruncating(output % ten));

				// Print decimal point if needed
				if (precision > 1)
				{
					int sepLength = TChar.GetLength(info.NumberDecimalSeparator);
					TChar.Copy(info.NumberDecimalSeparator, destination[(index + 1)..]);

					index += (int)precision + sepLength;
				}
				else
				{
					++index;
				}

				// Print the exponent.
				destination[index++] = upper ? (TChar)'E' : (TChar)'e';
				int exp = (int)(v.Exponent + digits - 1);

				if (exp < 0)
				{
					negativeSign.CopyTo(destination[index..]);
					index += negativeSign.Length;
					exp = -exp;
				}
				else
				{
					TChar.Copy(info.PositiveSign, destination[index..]);
					index += TChar.GetLength(info.PositiveSign);
				}

				uint elength = (uint)CountDigits((ulong)exp);

				for (int i = 0; i < elength; ++i)
				{
					(exp, int c) = int.DivRem(exp, 10);
					destination[(int)(index + elength - 1 - i)] = (TChar)(char)('0' + c);
				}
				index += (int)elength;
				return index;
			}
		}

		private static FloatingDecimal<TSignificand> FloatToDecimal<TFloat, TSignificand>(in TFloat value, int digits)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			int mantissaBits = TFloat.DenormalMantissaBits;
			int exponentBits = TFloat.ExponentBits;

			TSignificand bits = TFloat.FloatToBits(value);

			uint bias = (1U << (exponentBits - 1)) - 1;
			bool ieeeSign = ((bits >> (mantissaBits + exponentBits)) & TSignificand.One) != TSignificand.Zero;
			TSignificand ieeeMantissa = bits & ((TSignificand.One << mantissaBits) - TSignificand.One);
			uint ieeeExponent = uint.CreateTruncating((bits >> mantissaBits) & ((TSignificand.One << exponentBits) - TSignificand.One));

			if (ieeeExponent == 0 && ieeeMantissa == TSignificand.Zero)
			{
				return new FloatingDecimal<TSignificand>(ieeeMantissa, 0, ieeeSign);
			}
			if (ieeeExponent == ((1U << exponentBits) - 1U))
			{
				return new FloatingDecimal<TSignificand>(
					TFloat.ExplicitLeadingBit ? ieeeMantissa & ((TSignificand.One << (mantissaBits - 1)) - TSignificand.One) : ieeeMantissa,
					FDExceptionalExponent,
					ieeeSign
					);
			}

			int e2;
			TSignificand m2;

			// We subtract 2 in all cases so that the bounds computation has 2 additional bits.
			if (TFloat.ExplicitLeadingBit)
			{
				if (ieeeExponent == 0)
				{
					e2 = (int)(1 - bias - mantissaBits + 1 - 2);
				}
				else
				{
					e2 = (int)(ieeeExponent - bias - mantissaBits + 1 - 2);
				}
				m2 = ieeeMantissa;
			}
			else
			{
				if (ieeeExponent == 0)
				{
					e2 = (int)(1 - bias - mantissaBits - 2);
					m2 = ieeeMantissa;
				}
				else
				{
					e2 = (int)(ieeeExponent - (int)bias - mantissaBits - 2);
					m2 = (TSignificand.One << mantissaBits) | ieeeMantissa;
				}
			}


			bool even = (m2 & TSignificand.One) == TSignificand.Zero;
			bool acceptBounds = even;

			// Step 2: Determine the interval of legal decimal representations.
			TSignificand four = TSignificand.CreateTruncating(4);
			TSignificand mv = four * m2;
			// bool -> int conversion. True is 1, false is 0.
			uint mmShift = ((ieeeMantissa != (TFloat.ExplicitLeadingBit ? TSignificand.One << (mantissaBits - 1) : TSignificand.Zero))
				|| (ieeeExponent == 0)) ? 1U : 0U;

			// Step 3: Convert to a decimal power base using 128-bit arithmetic.
			TSignificand vr, vp, vm;
			int e10;
			bool vmIsTrailingZeros = false;
			bool vrIsTrailingZeros = false;

			if (e2 >= 0)
			{
				uint q = Log10Pow2(e2) - ((e2 > 3) ? 1U : 0U);
				e10 = (int)q;
				int k = (int)(FloatingDecimal<TSignificand>.Pow5InvBitCount + Pow5Bits((int)q) - 1);
				int i = -e2 + (int)q + k;
				Span<ulong> pow5 = stackalloc ulong[typeof(TSignificand) == typeof(UInt128) ? 4 : 8];
				GenericComputeInvPow5<TSignificand>(q, pow5);
				TSignificand m = four * m2;
				vr = FloatingDecimal<TSignificand>.MulShift(m, pow5, i);
				vp = FloatingDecimal<TSignificand>.MulShift(m + TSignificand.CreateTruncating(2), pow5, i);
				vm = FloatingDecimal<TSignificand>.MulShift(m - TSignificand.One - TSignificand.CreateTruncating(mmShift), pow5, i);

				// floor(log5(2^128)) = 55, this is very conservative
				if (q <= (typeof(TSignificand) == typeof(UInt128) ? 55 : 110))
				{
					// Only one of mp, mv, and mm can be a multiple of 5, if any.
					if (mv % TSignificand.CreateTruncating(5) == TSignificand.Zero)
					{
						vrIsTrailingZeros = FloatingDecimal<TSignificand>.Pow5Factor(mv) >= q - 1;
					}
					else if (acceptBounds)
					{
						// Same as min(e2 + (~mm & 1), pow5Factor(mm)) >= q
						// <=> e2 + (~mm & 1) >= q && pow5Factor(mm) >= q
						// <=> true && pow5Factor(mm) >= q, since e2 >= q.
						vmIsTrailingZeros = FloatingDecimal<TSignificand>.Pow5Factor(mv - TSignificand.One - TSignificand.CreateTruncating(mmShift)) >= q;
					}
					else
					{
						// Same as min(e2 + 1, pow5Factor(mp)) >= q.
						vp -= FloatingDecimal<TSignificand>.Pow5Factor(mv + TSignificand.CreateTruncating(2)) >= q ? TSignificand.One : TSignificand.Zero;
					}
				}
			}
			else
			{
				uint q = Log10Pow5(-e2) - (-2 > 1 ? 1 : 0);
				e10 = (int)(q + e2);
				int i = (int)(-e2 - q);
				int k = (int)(Pow5Bits(i) - FloatingDecimal<TSignificand>.Pow5BitCount);
				int j = (int)(q - k);
				Span<ulong> pow5 = stackalloc ulong[typeof(TSignificand) == typeof(UInt128) ? 4 : 8];
				GenericComputePow5<TSignificand>((uint)i, pow5);
				TSignificand m = four * m2;
				vr = FloatingDecimal<TSignificand>.MulShift(m, pow5, j);
				vp = FloatingDecimal<TSignificand>.MulShift(m + TSignificand.CreateTruncating(2), pow5, j);
				vm = FloatingDecimal<TSignificand>.MulShift(m - TSignificand.One - TSignificand.CreateTruncating(mmShift), pow5, j);

				if (q <= 1)
				{
					// {vr,vp,vm} is trailing zeros if {mv,mp,mm} has at least q trailing 0 bits.
					// mv = 4 m2, so it always has at least two trailing 0 bits.
					vrIsTrailingZeros = true;
					if (acceptBounds)
					{
						// mm = mv - 1 - mmShift, so it has 1 trailing 0 bit iff mmShift == 1.
						vrIsTrailingZeros = mmShift == 1;
					}
					else
					{
						--vp;
					}
				}
				else if (q < (typeof(TSignificand) == typeof(UInt128) ? 127 : 255))
				{
					vrIsTrailingZeros = (mv & ((TSignificand.One << (int)(q - 1)) - TSignificand.One)) == TSignificand.Zero;
				}
			}

			// Step 4: Find the shortest decimal representation in the interval of legal representations.
			uint removed = 0;
			byte lastRemovedDigit = 0;
			TSignificand output, ten = TSignificand.CreateTruncating(10);

			while ((vp / ten) > (vm / ten))
			{
				vmIsTrailingZeros &= vm % ten == TSignificand.Zero;
				vrIsTrailingZeros &= lastRemovedDigit == 0;
				lastRemovedDigit = byte.CreateTruncating(vr % ten);
				vr /= ten;
				vp /= ten;
				vm /= ten;
				++removed;
			}

			if (vmIsTrailingZeros)
			{
				while (vm % ten == TSignificand.Zero)
				{
					vrIsTrailingZeros &= lastRemovedDigit == 0;
					lastRemovedDigit = byte.CreateTruncating(vr % ten);
					vr /= ten;
					vp /= ten;
					vm /= ten;
					++removed;
				}
			}

			if (digits < TFloat.MaxSignificandPrecision)
			{
				int currDigits = (vr switch
				{
					UInt128 temp => UInt128.CountDigits(temp),
					UInt256 temp => UInt256.CountDigits(in temp)
				});

				while (currDigits-- > digits)
				{
					vmIsTrailingZeros &= vm % ten == TSignificand.Zero;
					vrIsTrailingZeros &= lastRemovedDigit == 0;
					lastRemovedDigit = byte.CreateTruncating(vr % ten);
					vr /= ten;
					vp /= ten;
					vm /= ten;
					++removed;
				}
				if (vmIsTrailingZeros)
				{
					while (vm % ten == TSignificand.Zero)
					{
						vrIsTrailingZeros &= lastRemovedDigit == 0;
						lastRemovedDigit = byte.CreateTruncating(vr % ten);
						vr /= ten;
						vp /= ten;
						vm /= ten;
						++removed;
					}
				}
			}

			if (vrIsTrailingZeros && (lastRemovedDigit == 5) && (vr % TSignificand.CreateTruncating(2) == TSignificand.Zero))
			{
				lastRemovedDigit = 4;
			}

			output = vr
			+ (((vr == vm && (!acceptBounds || !vmIsTrailingZeros)) || (lastRemovedDigit >= 5)) ? TSignificand.One : TSignificand.Zero);
			int exp = e10 + (int)removed;

			return new FloatingDecimal<TSignificand>(output, exp, ieeeSign);
		}
		internal static (TSignificand Mantissa, int Exponent, bool Sign) FloatToDecimalExtract<TFloat, TSignificand>(in TFloat value)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			var fd = FloatToDecimal<TFloat, TSignificand>(in value, TFloat.MaxSignificandPrecision);
			return (fd.Mantissa, fd.Exponent, fd.Sign);
		}

		// Returns floor(log_10(2^e)).
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Log10Pow2(int e)
		{
			Debug.Assert(e >= 0);
			if (e > 1 << 15)
			{
				return (uint)double.Floor(BigInteger.Log10(BigInteger.Pow(2, e)));
			}

			// The first value this approximation fails for is 2^1651 which is just greater than 10^297.
			return (uint)((((ulong)e) * 169464822037455UL) >> 49);
		}
		// Returns floor(log_10(5^e)).
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Log10Pow5(int e)
		{
			Debug.Assert(e >= 0);
			if (e > 1 << 15)
			{
				return (uint)double.Floor(BigInteger.Log10(BigInteger.Pow(5, e)));
			}

			// The first value this approximation fails for is 5^2621 which is just greater than 10^1832.
			return (uint)((((ulong)e) * 196742565691928UL) >> 48);
		}
		// Returns e == 0 ? 1 : ceil(log_2(5^e)); requires 0 <= e <= 32768.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Pow5Bits(int e)
		{
			Debug.Assert(e >= 0);
			if (e > 1 << 15)
			{
				return ((uint)BigInteger.Log2(BigInteger.Pow(5, e))) + 1;
			}

			return (uint)(((((ulong)e) * 163391164108059UL) >> 46) + 1);
		}
		private static void GenericComputePow5<TSignificand>(uint i, Span<ulong> result)
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			uint ibase = i / FloatingDecimal<TSignificand>.Pow5TableSize;
			uint ibase2 = ibase * FloatingDecimal<TSignificand>.Pow5TableSize;
			var mul = FloatingDecimal<TSignificand>.GenericPow5Split[(int)ibase];

			if (i == ibase2)
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					result[0] = mul[0];
					result[1] = mul[1];
					result[2] = mul[2];
					result[3] = mul[3];
				}
				else
				{
					result[0] = mul[0];
					result[1] = mul[1];
					result[2] = mul[2];
					result[3] = mul[3];
					result[4] = mul[4];
					result[5] = mul[5];
					result[6] = mul[6];
					result[7] = mul[7];
				}
			}
			else
			{
				uint offset = i - ibase2;
				var m = FloatingDecimal<TSignificand>.GenericPow5Table[(int)offset];
				uint delta = Pow5Bits((int)i) - Pow5Bits((int)ibase2);
				uint corr = (uint)((FloatingDecimal<TSignificand>.Pow5Errors[(int)i / 32] >> (2 * ((int)i % 32))) & 3);
				if (typeof(TSignificand) == typeof(UInt128))
				{
					Mul128To256Shift(m, mul, (int)delta, corr, result);
				}
				else
				{
					Mul256To512Shift(m, mul, (int)delta, corr, result);
				}
			}
		}
		private static void GenericComputeInvPow5<TSignificand>(uint i, Span<ulong> result)
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			uint ibase = (i + FloatingDecimal<TSignificand>.Pow5TableSize - 1) / FloatingDecimal<TSignificand>.Pow5TableSize;
			uint ibase2 = ibase * FloatingDecimal<TSignificand>.Pow5TableSize;
			var mul = FloatingDecimal<TSignificand>.GenericPow5InvSplit[(int)ibase];
			if (i == ibase2)
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					result[0] = mul[0] + 1;
					result[1] = mul[1];
					result[2] = mul[2];
					result[3] = mul[3];
				}
				else
				{
					result[0] = mul[0] + 1;
					result[1] = mul[1];
					result[2] = mul[2];
					result[3] = mul[3];
					result[4] = mul[4];
					result[5] = mul[5];
					result[6] = mul[6];
					result[7] = mul[7];
				}
			}
			else
			{
				uint offset = ibase2 - i;
				var m = FloatingDecimal<TSignificand>.GenericPow5Table[(int)offset];
				ulong delta = Pow5Bits((int)ibase2) - Pow5Bits((int)i);
				uint corr = (uint)((FloatingDecimal<TSignificand>.Pow5InvErrors[(int)i / 32] >> (2 * ((int)i % 32))) & 3) + 1;
				if (typeof(TSignificand) == typeof(UInt128))
				{
					Mul128To256Shift(m, mul, (int)delta, corr, result);
				}
				else
				{
					Mul256To512Shift(m, mul, (int)delta, corr, result);
				}
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Mul128To256Shift(ReadOnlySpan<ulong> a, ReadOnlySpan<ulong> b, int shift, uint corr, Span<ulong> result)
		{
			Debug.Assert(a.Length == 2 || (a[2] == default && a[3] == default));
			Debug.Assert(b.Length == 4 || (b[4] == default && b[5] == default && b[6] == default && b[7] == default));
			Debug.Assert(result.Length == 4);

			UInt128 b00 = Math.BigMul(a[0], b[0]);           // 0
			UInt128 b01 = Math.BigMul(a[0], b[1]);           // 64
			UInt128 b02 = Math.BigMul(a[0], b[2]);           // 128
			UInt128 b03 = Math.BigMul(a[0], b[3]);           // 196
			UInt128 b10 = Math.BigMul(a[1], b[0]);           // 64
			UInt128 b11 = Math.BigMul(a[1], b[1]);           // 128
			UInt128 b12 = Math.BigMul(a[1], b[2]);           // 196
			UInt128 b13 = Math.BigMul(a[1], b[3]);           // 256

			UInt128 s0 = b00;                                           // 0
			UInt128 s1 = b01 + b10;                                     // 64
			UInt128 c1 = s1 < b01 ? UInt128.One : UInt128.Zero;         // 196
			UInt128 s2 = b02 + b11;                                     // 128
			UInt128 c2 = s2 < b02 ? UInt128.One : UInt128.Zero;         // 256
			UInt128 s3 = b03 + b12;                                     // 196
			UInt128 c3 = s3 < b03 ? UInt128.One : UInt128.Zero;         // 324

			UInt128 p0 = s0 + (s1 << 64);                               // 0
			UInt128 d0 = p0 < b00 ? UInt128.One : UInt128.Zero;         // 128
			UInt128 q1 = s2 + (s1 >> 64) + (s3 << 64);                  // 128
			UInt128 d1 = q1 < s2 ? UInt128.One : UInt128.Zero;          // 256
			UInt128 p1 = q1 + (c1 << 64) + d0;                          // 128
			UInt128 d2 = p1 < q1 ? UInt128.One : UInt128.Zero;          // 256
			UInt128 p2 = b13 + (s3 >> 64) + c2 + (c3 << 64) + d1 + d2;  // 256

			UInt128 r0, r1;
			if (shift < 128)
			{
				r0 = corr + ((p0 >> shift) | (p1 << (128 - shift)));
				r1 = ((p1 >> shift) | (p2 << (128 - shift))) + (r0 < corr ? UInt128.One : UInt128.Zero);
			}
			else if (shift == 128)
			{
				r0 = corr + p1;
				r1 = p2 + (r0 < corr ? UInt128.One : UInt128.Zero);
			}
			else
			{
				r0 = corr + ((p1 >> shift - 128) | (p2 << (256 - shift)));
				r1 = (p2 >> (shift - 128)) + (r0 < corr ? UInt128.One : UInt128.Zero);
			}

			result[0] = (ulong)r0;
			result[1] = (ulong)r0.Upper;
			result[2] = (ulong)r1;
			result[3] = (ulong)r1.Upper;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Mul256To512Shift(ReadOnlySpan<ulong> a, ReadOnlySpan<ulong> b, int shift, uint corr, Span<ulong> result)
		{
			Debug.Assert(a.Length == 4);
			Debug.Assert(b.Length == 8);
			Debug.Assert(result.Length == 8);

			ReadOnlySpan<UInt128> a128 = MemoryMarshal.Cast<ulong, UInt128>(a);
			ReadOnlySpan<UInt128> b128 = MemoryMarshal.Cast<ulong, UInt128>(b);

			UInt256 b00 = MathQ.BigMul(a128[0], b128[0]);           // 0
			UInt256 b01 = MathQ.BigMul(a128[0], b128[1]);           // 128
			UInt256 b02 = MathQ.BigMul(a128[0], b128[2]);           // 256
			UInt256 b03 = MathQ.BigMul(a128[0], b128[3]);           // 384
			UInt256 b10 = MathQ.BigMul(a128[1], b128[0]);           // 128
			UInt256 b11 = MathQ.BigMul(a128[1], b128[1]);           // 256
			UInt256 b12 = MathQ.BigMul(a128[1], b128[2]);           // 384
			UInt256 b13 = MathQ.BigMul(a128[1], b128[3]);           // 512

			UInt256 s0 = b00;                                               // 0
			UInt256 s1 = b01 + b10;                                         // 64
			UInt256 c1 = s1 < b01 ? UInt256.One : UInt256.Zero;             // 196
			UInt256 s2 = b02 + b11;                                         // 128
			UInt256 c2 = s2 < b02 ? UInt256.One : UInt256.Zero;             // 256
			UInt256 s3 = b03 + b12;                                         // 196
			UInt256 c3 = s3 < b03 ? UInt256.One : UInt256.Zero;             // 324

			UInt256 p0 = s0 + (s1 << 128);                                  // 0
			UInt256 d0 = p0 < b00 ? UInt256.One : UInt256.Zero;             // 128
			UInt256 q1 = s2 + (s1 >> 128) + (s3 << 128);                    // 128
			UInt256 d1 = q1 < s2 ? UInt256.One : UInt256.Zero;              // 256
			UInt256 p1 = q1 + (c1 << 128) + d0;                             // 128
			UInt256 d2 = p1 < q1 ? UInt256.One : UInt256.Zero;              // 256
			UInt256 p2 = b13 + (s3 >> 128) + c2 + (c3 << 128) + d1 + d2;    // 256

			UInt256 r0, r1;
			if (shift < 256)
			{
				r0 = corr + ((p0 >> shift) | (p1 << (256 - shift)));
				r1 = ((p1 >> shift) | (p2 << (256 - shift))) + (r0 < corr ? UInt256.One : UInt256.Zero);
			}
			else if (shift == 256)
			{
				r0 = corr + p1;
				r1 = p2 + (r0 < corr ? UInt256.One : UInt256.Zero);
			}
			else
			{
				r0 = corr + ((p1 >> shift - 256) | (p2 << (512 - shift)));
				r1 = (p2 >> (shift - 256)) + (r0 < corr ? UInt256.One : UInt256.Zero);
			}

			Span<UInt128> r = MemoryMarshal.Cast<ulong, UInt128>(result);
			r[0] = r0.Lower;
			r[1] = r0.Upper;
			r[2] = r1.Lower;
			r[3] = r1.Upper;
		}
	}
}
