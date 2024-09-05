using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MissingValues.Info;

internal static partial class NumberFormatter
{
	internal interface IBinaryFloatingPointDecimalFormat<TSelf, TSignificand>
		where TSelf : struct, IBinaryFloatingPointDecimalFormat<TSelf, TSignificand>
		where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
	{
		abstract static ReadOnlySpan<Ryu.CachePower> GenericPow5Table { get; }
		abstract static ReadOnlySpan<Ryu.CachePowerSplit> GenericPow5Split { get; }
		abstract static ReadOnlySpan<Ryu.CachePowerSplit> GenericPow5InvSplit { get; }
		abstract static ReadOnlySpan<ulong> Pow5Errors { get; }
		abstract static ReadOnlySpan<ulong> Pow5InvErrors { get; }
		abstract static uint Pow5InvBitCount { get; }
		abstract static uint Pow5BitCount { get; }
		abstract static uint Pow5TableSize { get; }
		abstract static TSignificand Two { get; }
		abstract static TSignificand Four { get; }
		abstract static TSignificand Five { get; }
		abstract static TSignificand Ten { get; }

		abstract static TSelf Build(in TSignificand mantissa, int exponent, bool sign);
		abstract static uint Pow5Factor(TSignificand value);
		abstract static uint ToUInt32(in TSignificand value);
		abstract static TSignificand FromUInt32(uint value);
		virtual static bool MultipleOfPowerOf2(in TSignificand value, uint p)
		{
			return (value & (((TSignificand.One) << (int)p) - TSignificand.One)) == TSignificand.Zero;
		}
		virtual static bool MultipleOfPowerOf5(in TSignificand value, uint p)
		{
			return TSelf.Pow5Factor(value) >= p;
		}
		abstract static TSignificand MulShift(TSignificand m, ReadOnlySpan<ulong> mul, int j);

		bool Sign { get; }
		int Exponent { get; }
		TSignificand Mantissa { get; }

	}

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

			public CachePower(ulong S0, ulong S1)
			{
				this[0] = S0;
				this[1] = S1;
				this[2] = default;
				this[3] = default;
			}
			public CachePower(ulong S0, ulong S1, ulong S2, ulong S3)
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

			public CachePowerSplit(ulong S0, ulong S1, ulong S2, ulong S3)
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
			public CachePowerSplit(ulong S0, ulong S1, ulong S2, ulong S3, ulong S4, ulong S5, ulong S6, ulong S7)
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
		
		private record struct FloatingDecimal<TSignificand>(TSignificand Mantissa, int Exponent, bool Sign) : IBinaryFloatingPointDecimalFormat<FloatingDecimal<TSignificand>, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			public static ReadOnlySpan<CachePower> GenericPow5Table
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return CacheArraysForQuad.QuadPow5Table;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return CacheArraysForOcto.OctoPow5Table;
					}
				}
			}

			public static ReadOnlySpan<CachePowerSplit> GenericPow5Split
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return CacheArraysForQuad.QuadPow5Split;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return CacheArraysForOcto.OctoPow5Split;
					}
				}
			}

			public static ReadOnlySpan<CachePowerSplit> GenericPow5InvSplit
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return CacheArraysForQuad.QuadPow5InvSplit;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return CacheArraysForOcto.OctoPow5InvSplit;
					}
				}
			}

			public static ReadOnlySpan<ulong> Pow5Errors
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return CacheArraysForQuad.QuadPow5Errors;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return CacheArraysForOcto.OctoPow5Errors;
					}
				}
			}

			public static ReadOnlySpan<ulong> Pow5InvErrors
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return CacheArraysForQuad.QuadPow5InvErrors;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return CacheArraysForOcto.OctoPow5InvErrors;
					}
				}
			}

			public static uint Pow5InvBitCount
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return 249;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return 501;
					}
				}
			}

			public static uint Pow5BitCount
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return 249;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return 501;
					}
				}
			}

			public static uint Pow5TableSize
			{
				get
				{
					if (typeof(TSignificand) == typeof(UInt128))
					{
						return 56;
					}
					else // if (typeof(TSignificand) == typeof(UInt256))
					{
						return 111;
					}
				}
			}

			public static TSignificand Two => TSignificand.CreateTruncating(2);

			public static TSignificand Four => TSignificand.CreateTruncating(4);

			public static TSignificand Five => TSignificand.CreateTruncating(5);

			public static TSignificand Ten => TSignificand.CreateTruncating(10);

			public static FloatingDecimal<TSignificand> Build(in TSignificand mantissa, int exponent, bool sign)
			{
				return new(mantissa, exponent, sign);
			}

			public static TSignificand FromUInt32(uint value)
			{
				return TSignificand.CreateChecked(value);
			}

			public static TSignificand MulShift(TSignificand m, ReadOnlySpan<ulong> mul, int j)
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					Debug.Assert(j > 128);
					UInt128 mm = (UInt128)(object)m;
					ReadOnlySpan<ulong> a =
					[
						(ulong)mm,
						(ulong)(mm >> 64)
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

			public static uint Pow5Factor(TSignificand value)
			{
				var five = Five;
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

			public static uint ToUInt32(in TSignificand value)
			{
				return uint.CreateTruncating(value);
			}
		}

		private const int FD128ExceptionalExponent = 0x7FFF_FFFF;

		public static void Format<TFloat, TSignificand, TChar>(in TFloat value, scoped Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, out bool isExceptional, NumberFormatInfo? info, int? precision = null)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			var fd = FloatToDecimal<TFloat, FloatingDecimal<TSignificand>, TSignificand>(in value);
			charsWritten = ToCharSpan<FloatingDecimal<TSignificand>, TSignificand, TChar>(in fd, destination, format, info is null ? NumberFormatInfo.CurrentInfo : info!, out isExceptional);
		}
		public static void Format<TFloat, TSignificand>(in TFloat value, scoped ref NumberInfo number, out bool isExceptional)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			var fd = FloatToDecimal<TFloat, FloatingDecimal<TSignificand>, TSignificand>(in value);
			UInt256 significand = UInt256.CreateTruncating(fd.Mantissa);
			UIntToNumber<UInt256, Int256>(in significand, ref number);
			number.Scale += fd.Exponent;
			number.IsNegative = fd.Sign;
			isExceptional = fd.Exponent == FD128ExceptionalExponent;
		}

		private static int ToCharSpan<TDecimal, TSignificand, TChar>(in TDecimal v, Span<TChar> destination, ReadOnlySpan<char> format, NumberFormatInfo info, out bool isExceptional)
			where TDecimal : struct, IBinaryFloatingPointDecimalFormat<TDecimal, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
			where TChar : unmanaged, IUtfCharacter<TChar>
		{
			if (v.Exponent == FD128ExceptionalExponent)
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
			Span<TChar> negativeSign = stackalloc TChar[TChar.GetLength(info.NegativeSign)];
			TChar.Copy(info.NegativeSign, negativeSign);

			int index = 0;
			if (v.Sign)
			{
				negativeSign.CopyTo(destination[index..]);
				index += negativeSign.Length;
			}

			TSignificand output = v.Mantissa;
			uint olength = (uint)(output switch
			{
				UInt128 temp => BitHelper.CountDigits(temp),
				UInt256 temp => UInt256.CountDigits(in temp)
			});

			for (uint i = 0; i < olength - 1; ++i)
			{
				(output, TSignificand c) = TSignificand.DivRem(output, TDecimal.Ten);
				destination[(int)(index + olength - i)] = (TChar)(char)('0' + TDecimal.ToUInt32(in c));
			}
			destination[index] = (TChar)(char)('0' + TDecimal.ToUInt32(output % TDecimal.Ten));

			// Print decimal point if needed
			if (olength > 1)
			{
				int sepLength;
				switch (format.IsEmpty ? '\0' : format[0])
				{
					case 'p':
					case 'P':
						sepLength = TChar.GetLength(info.PercentDecimalSeparator);
						TChar.Copy(info.PercentDecimalSeparator, destination[(index + 1)..]);
						break;
					case 'c':
					case 'C':
						sepLength = TChar.GetLength(info.CurrencyDecimalSeparator);
						TChar.Copy(info.CurrencyDecimalSeparator, destination[(index + 1)..]);
						break;
					default:
						sepLength = TChar.GetLength(info.NumberDecimalSeparator);
						TChar.Copy(info.NumberDecimalSeparator, destination[(index + 1)..]);
						break;
				}

				index += (int)olength + sepLength;
			}
			else
			{
				++index;
			}

			// Print the exponent.
			destination[index++] = (TChar)'E';
			int exp = (int)(v.Exponent + olength - 1);

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

			uint elength = (uint)BitHelper.CountDigits((UInt128)exp);

			for (int i = 0; i < elength; ++i)
			{
				(exp, int c) = int.DivRem(exp, 10);
				destination[(int)(index + elength - 1 - i)] = (TChar)(char)('0' + c);
			}
			index += (int)elength;
			return index;
		}

		public static TDecimal FloatToDecimal<TFloat, TDecimal, TSignificand>(in TFloat value)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TDecimal : struct, IBinaryFloatingPointDecimalFormat<TDecimal, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			int mantissaBits = TFloat.DenormalMantissaBits;
			int exponentBits = TFloat.ExponentBits;

			TSignificand bits = TFloat.FloatToBits(value);


			uint bias = (1U << (exponentBits - 1)) - 1;
			bool ieeeSign = ((bits >> (mantissaBits + exponentBits)) & TSignificand.One) != TSignificand.Zero;
			TSignificand ieeeMantissa = bits & ((TSignificand.One << mantissaBits) - TSignificand.One);
			uint ieeeExponent = TDecimal.ToUInt32(((bits >> mantissaBits) & ((TSignificand.One << exponentBits) - TSignificand.One)));

			if (ieeeExponent == 0 && ieeeMantissa == TSignificand.Zero)
			{
				return TDecimal.Build(ieeeMantissa, 0, ieeeSign);
			}
			if (ieeeExponent == ((1U << exponentBits) - 1U))
			{
				return TDecimal.Build(
					   TFloat.ExplicitLeadingBit ? ieeeMantissa & ((TSignificand.One << (mantissaBits - 1)) - TSignificand.One) : ieeeMantissa,
					   FD128ExceptionalExponent,
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
			TSignificand four = TDecimal.Four;
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
				int k = (int)(TDecimal.Pow5InvBitCount + Pow5Bits((int)q) - 1);
				int i = -e2 + (int)q + k;
				Span<ulong> pow5 = stackalloc ulong[typeof(TSignificand) == typeof(UInt128) ? 4 : 8];
				GenericComputeInvPow5<TDecimal, TSignificand>(q, pow5);
				vr = TDecimal.MulShift(four * m2, pow5, i);
				vp = TDecimal.MulShift(four * m2 + TDecimal.Two, pow5, i);
				vm = TDecimal.MulShift(four * m2 - TSignificand.One - TDecimal.FromUInt32(mmShift), pow5, i);

				// floor(log5(2^128)) = 55, this is very conservative
				if (q <= 55)
				{
					// Only one of mp, mv, and mm can be a multiple of 5, if any.
					if (mv % TDecimal.Five == TSignificand.Zero)
					{
						vrIsTrailingZeros = TDecimal.MultipleOfPowerOf5(in mv, q - 1);
					}
					else if (acceptBounds)
					{
						// Same as min(e2 + (~mm & 1), pow5Factor(mm)) >= q
						// <=> e2 + (~mm & 1) >= q && pow5Factor(mm) >= q
						// <=> true && pow5Factor(mm) >= q, since e2 >= q.
						vmIsTrailingZeros = TDecimal.MultipleOfPowerOf5(mv - TSignificand.One - TDecimal.FromUInt32(mmShift), q);
					}
					else
					{
						// Same as min(e2 + 1, pow5Factor(mp)) >= q.
						vp -= TDecimal.MultipleOfPowerOf5(mv + TDecimal.Two, q) ? TSignificand.One : TSignificand.Zero;
					}
				}
			}
			else
			{
				uint q = Log10Pow5(-e2) - (-2 > 1 ? 1 : 0);
				e10 = (int)(q + e2);
				int i = (int)(-e2 - q);
				int k = (int)(Pow5Bits(i) - TDecimal.Pow5BitCount);
				int j = (int)(q - k);
				Span<ulong> pow5 = stackalloc ulong[typeof(TSignificand) == typeof(UInt128) ? 4 : 8];
				GenericComputePow5<TDecimal, TSignificand>((uint)i, pow5);
				vr = TDecimal.MulShift(four * m2, pow5, j);
				vp = TDecimal.MulShift(four * m2 + TDecimal.Two, pow5, j);
				vm = TDecimal.MulShift(four * m2 - TSignificand.One - TDecimal.FromUInt32(mmShift), pow5, j);

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
				else if (q < 127)
				{
					vrIsTrailingZeros = TDecimal.MultipleOfPowerOf2(in mv, q - 1);
				}
			}

			// Step 4: Find the shortest decimal representation in the interval of legal representations.
			uint removed = 0;
			byte lastRemovedDigit = 0;
			TSignificand output, ten = TDecimal.Ten;

			while ((vp / ten) > (vm / ten))
			{
				vmIsTrailingZeros &= vm % ten == TSignificand.Zero;
				vrIsTrailingZeros &= lastRemovedDigit == 0;
				lastRemovedDigit = (byte)TDecimal.ToUInt32(vr % ten);
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
					lastRemovedDigit = (byte)TDecimal.ToUInt32(vr % ten);
					vr /= ten;
					vp /= ten;
					vm /= ten;
					++removed;
				}
			}

			if (vrIsTrailingZeros && (lastRemovedDigit == 5) && (vr % TDecimal.Two == TSignificand.Zero))
			{
				lastRemovedDigit = 4;
			}

			output = vr
			+ (((vr == vm && (!acceptBounds || !vmIsTrailingZeros)) || (lastRemovedDigit >= 5)) ? TSignificand.One : TSignificand.Zero);
			int exp = e10 + (int)removed;

			return TDecimal.Build(output, exp, ieeeSign);
		}
		public static (TSignificand Mantissa, int Exponent, bool Sign) FloatToDecimalExtract<TFloat, TSignificand>(in TFloat value)
			where TFloat : struct, IBinaryFloatingPointInfo<TFloat, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			var fd = FloatToDecimal<TFloat, FloatingDecimal<TSignificand>, TSignificand>(in value);
			return (fd.Mantissa, fd.Exponent, fd.Sign);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Log10Pow2(int e)
		{
			// The first value this approximation fails for is 2^1651 which is just greater than 10^297.
			Debug.Assert(e >= 0);
			Debug.Assert(e <= 1 << 15);

			return (uint)((((ulong)e) * 169464822037455UL) >> 49);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Log10Pow5(int e)
		{
			// The first value this approximation fails for is 2^1651 which is just greater than 10^297.
			Debug.Assert(e >= 0);
			Debug.Assert(e <= 1 << 15);

			return (uint)((((ulong)e) * 196742565691928UL) >> 48);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Pow5Bits(int e)
		{
			Debug.Assert(e >= 0);
			Debug.Assert(e <= 1 << 15);

			return (uint)(((((ulong)e * 163391164108059UL)) >> 46) + 1);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void GenericComputePow5<TDecimal, TSignificand>(uint i, Span<ulong> result)
			where TDecimal : struct, IBinaryFloatingPointDecimalFormat<TDecimal, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			uint ibase = i / TDecimal.Pow5TableSize;
			uint ibase2 = ibase * TDecimal.Pow5TableSize;
			var mul = TDecimal.GenericPow5Split[(int)ibase];

			if (i == ibase2)
			{
				result[0] = mul[0];
				result[1] = mul[1];
				result[2] = mul[2];
				result[3] = mul[3];
			}
			else
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					uint offset = i - ibase2;
					var m = TDecimal.GenericPow5Table[(int)offset];
					uint delta = Pow5Bits((int)i) - Pow5Bits((int)ibase2);
					uint corr = (uint)((TDecimal.Pow5Errors[(int)i / 32] >> (2 * ((int)i % 32))) & 3);
					Mul128To256Shift(m, mul, (int)delta, corr, result);
				}
				else
				{
					uint offset = i - ibase2;
					var m = TDecimal.GenericPow5Table[(int)offset];
					uint delta = Pow5Bits((int)i) - Pow5Bits((int)ibase2);
					uint corr = (uint)((TDecimal.Pow5Errors[(int)i / 32] >> (2 * ((int)i % 32))) & 3);
					Mul256To512Shift(m, mul, (int)delta, corr, result);
				}
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void GenericComputeInvPow5<TDecimal, TSignificand>(uint i, Span<ulong> result)
			where TDecimal : struct, IBinaryFloatingPointDecimalFormat<TDecimal, TSignificand>
			where TSignificand : unmanaged, IBinaryInteger<TSignificand>, IUnsignedNumber<TSignificand>
		{
			uint ibase = (i + TDecimal.Pow5TableSize - 1) / TDecimal.Pow5TableSize;
			uint ibase2 = ibase * TDecimal.Pow5TableSize;
			var mul = TDecimal.GenericPow5InvSplit[(int)ibase];
			if (i == ibase2)
			{
				result[0] = mul[0] + 1;
				result[1] = mul[1];
				result[2] = mul[2];
				result[3] = mul[3];
			}
			else
			{
				if (typeof(TSignificand) == typeof(UInt128))
				{
					uint offset = ibase2 - i;
					var m = TDecimal.GenericPow5Table[(int)offset];
					ulong delta = Pow5Bits((int)ibase2) - Pow5Bits((int)i);
					uint corr = (uint)((TDecimal.Pow5InvErrors[(int)i / 32] >> (2 * ((int)i % 32))) & 3) + 1;
					Mul128To256Shift(m, mul, (int)delta, corr, result);
				}
				else
				{
					uint offset = ibase2 - i;
					var m = TDecimal.GenericPow5Table[(int)offset];
					ulong delta = Pow5Bits((int)ibase2) - Pow5Bits((int)i);
					uint corr = (uint)((TDecimal.Pow5InvErrors[(int)i / 32] >> (2 * ((int)i % 32))) & 3) + 1;
					Mul256To512Shift(m, mul, (int)delta, corr, result);
				}
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Mul128To256Shift(ReadOnlySpan<ulong> a, ReadOnlySpan<ulong> b, int shift, uint corr, Span<ulong> result)
		{
			Debug.Assert(result.Length == 4);

			UInt128 b00 = Calculator.BigMul(a[0], b[0]);           // 0
			UInt128 b01 = Calculator.BigMul(a[0], b[1]);           // 64
			UInt128 b02 = Calculator.BigMul(a[0], b[2]);           // 128
			UInt128 b03 = Calculator.BigMul(a[0], b[3]);           // 196
			UInt128 b10 = Calculator.BigMul(a[1], b[0]);           // 64
			UInt128 b11 = Calculator.BigMul(a[1], b[1]);           // 128
			UInt128 b12 = Calculator.BigMul(a[1], b[2]);           // 196
			UInt128 b13 = Calculator.BigMul(a[1], b[3]);           // 256

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
			result[1] = (ulong)(r0 >> 64);
			result[2] = (ulong)r1;
			result[3] = (ulong)(r1 >> 64);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Mul256To512Shift(ReadOnlySpan<ulong> a, ReadOnlySpan<ulong> b, int shift, uint corr, Span<ulong> result)
		{
			Debug.Assert(result.Length == 8);

			// Multiply each combination of 64-bit segments of a and b
			UInt128 b00 = Calculator.BigMul(a[0], b[0]);    // 0
			UInt128 b01 = Calculator.BigMul(a[0], b[1]);    // 64
			UInt128 b02 = Calculator.BigMul(a[0], b[2]);    // 128
			UInt128 b03 = Calculator.BigMul(a[0], b[3]);    // 192
			UInt128 b10 = Calculator.BigMul(a[1], b[0]);    // 64
			UInt128 b11 = Calculator.BigMul(a[1], b[1]);    // 128
			UInt128 b12 = Calculator.BigMul(a[1], b[2]);    // 192
			UInt128 b13 = Calculator.BigMul(a[1], b[3]);    // 256
			UInt128 b20 = Calculator.BigMul(a[2], b[0]);    // 128
			UInt128 b21 = Calculator.BigMul(a[2], b[1]);    // 192
			UInt128 b22 = Calculator.BigMul(a[2], b[2]);    // 256
			UInt128 b23 = Calculator.BigMul(a[2], b[3]);    // 320
			UInt128 b30 = Calculator.BigMul(a[3], b[0]);    // 192
			UInt128 b31 = Calculator.BigMul(a[3], b[1]);    // 256
			UInt128 b32 = Calculator.BigMul(a[3], b[2]);    // 320
			UInt128 b33 = Calculator.BigMul(a[3], b[3]);    // 384

			// Sum up the intermediate products
			UInt128 s0 = b00;
			UInt128 s1 = b01 + b10;
			UInt128 c1 = s1 < b01 ? UInt128.One : UInt128.Zero;
			UInt128 s2 = b02 + b11 + b20;
			UInt128 c2 = s2 < b02 ? UInt128.One : UInt128.Zero;
			UInt128 s3 = b03 + b12 + b21 + b30;
			UInt128 c3 = s3 < b03 ? UInt128.One : UInt128.Zero;
			UInt128 s4 = b13 + b22 + b31 + c1;
			UInt128 c4 = s4 < b13 ? UInt128.One : UInt128.Zero;
			UInt128 s5 = b23 + b32 + c2 + c3;
			UInt128 c5 = s5 < b23 ? UInt128.One : UInt128.Zero;
			UInt128 s6 = b33 + c4 + c5;

			// Combine sums into 512-bit result
			UInt128 p0 = s0 + (s1 << 64);
			UInt128 d0 = p0 < s0 ? UInt128.One : UInt128.Zero;
			UInt128 q1 = s2 + (s1 >> 64) + (s3 << 64);
			UInt128 d1 = q1 < s2 ? UInt128.One : UInt128.Zero;
			UInt128 p1 = q1 + (d0 << 64);
			UInt128 d2 = p1 < q1 ? UInt128.One : UInt128.Zero;
			UInt128 q2 = s4 + (s3 >> 64) + (s5 << 64);
			UInt128 d3 = q2 < s4 ? UInt128.One : UInt128.Zero;
			UInt128 p2 = q2 + d1;
			UInt128 d4 = p2 < q2 ? UInt128.One : UInt128.Zero;
			UInt128 p3 = s6 + (s5 >> 64) + d2 + d3 + d4;

			// Apply shift and correction
			UInt128 r0, r1, r2, r3;
			if (shift < 128)
			{
				r0 = corr + ((p0 >> shift) | (p1 << (128 - shift)));
				r1 = (p1 >> shift) | (p2 << (128 - shift)) + (r0 < corr ? UInt128.One : UInt128.Zero);
				r2 = (p2 >> shift) | (p3 << (128 - shift)) + (r1 < corr ? UInt128.One : UInt128.Zero);
				r3 = p3 >> shift;
			}
			else if (shift < 256)
			{
				r0 = corr + ((p1 >> (shift - 128)) | (p2 << (256 - shift)));
				r1 = (p2 >> (shift - 128)) | (p3 << (256 - shift)) + (r0 < corr ? UInt128.One : UInt128.Zero);
				r2 = p3 >> (shift - 128);
				r3 = UInt128.Zero;
			}
			else if (shift < 384)
			{
				r0 = corr + (p2 >> (shift - 256)) | (p3 << (384 - shift));
				r1 = p3 >> (shift - 256) + (r0 < corr ? 1 : 0);
				r2 = UInt128.Zero;
				r3 = UInt128.Zero;
			}
			else
			{
				r0 = corr + (p3 >> (shift - 384));
				r1 = UInt128.Zero;
				r2 = UInt128.Zero;
				r3 = UInt128.Zero;
			}

			// Store the 512-bit result in the result span
			result[0] = (ulong)r0;
			result[1] = (ulong)(r0 >> 64);
			result[2] = (ulong)r1;
			result[3] = (ulong)(r1 >> 64);
			result[4] = (ulong)r2;
			result[5] = (ulong)(r2 >> 64);
			result[6] = (ulong)r3;
			result[7] = (ulong)(r3 >> 64);
		}
	}
}
