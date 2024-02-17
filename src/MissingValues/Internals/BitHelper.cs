using MissingValues.Internals;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static class BitHelper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(UInt128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetUpperAndLowerBits(Int128 value, out ulong upper, out ulong lower)
		{
			lower = value.GetLowerBits();
			upper = value.GetUpperBits();
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetUpperBits(this in UInt128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetUpperBits(this in Int128 value)
		{
			return unchecked((ulong)(value >> 64));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetLowerBits(this in UInt128 value)
		{
			return unchecked((ulong)(value));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong GetLowerBits(this in Int128 value)
		{
			return unchecked((ulong)(value));
		}

		public static void GetDoubleParts(double dbl, out int sign, out int exp, out ulong man, out bool fFinite)
		{
			ulong bits = BitConverter.DoubleToUInt64Bits(dbl);

			sign = 1 - ((int)(bits >> 62) & 2);
			man = bits & 0x000FFFFFFFFFFFFF;
			exp = (int)(bits >> 52) & 0x7FF;
			if (exp == 0)
			{
				// Denormalized number.
				fFinite = true;
				if (man != 0)
					exp = -1074;
			}
			else if (exp == 0x7FF)
			{
				// NaN or Infinite.
				fFinite = false;
				exp = int.MaxValue;
			}
			else
			{
				fFinite = true;
				man |= 0x0010000000000000;
				exp -= 1075;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);

			if (upper == 0)
			{
				return 64 + BitOperations.LeadingZeroCount(lower);
			}
			return BitOperations.LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt256 value)
		{
			UInt128 upper = value.Upper;

			if (upper == 0)
			{
				return 128 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(Int256 value)
		{
			UInt128 upper = value.Upper;

			if (upper == 0)
			{
				return 128 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(UInt512 value)
		{
			UInt256 upper = value.Upper;

			if (upper == 0)
			{
				return 256 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int LeadingZeroCount(Int512 value)
		{
			UInt256 upper = value.Upper;

			if (upper == 0)
			{
				return 256 + LeadingZeroCount(value.Lower);
			}
			return LeadingZeroCount(upper);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 ReverseEndianness(UInt128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);


			return new UInt128(BinaryPrimitives.ReverseEndianness(lower), BinaryPrimitives.ReverseEndianness(upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Int128 ReverseEndianness(Int128 value)
		{
			GetUpperAndLowerBits(value, out var upper, out var lower);

			return new Int128(BinaryPrimitives.ReverseEndianness(lower), BinaryPrimitives.ReverseEndianness(upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 ReverseEndianness(UInt256 value)
		{
			return new UInt256(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		internal static Int256 ReverseEndianness(Int256 value)
		{
			return new(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt512 ReverseEndianness(UInt512 value)
		{
			return new UInt512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		internal static Int512 ReverseEndianness(Int512 value)
		{
			return new Int512(ReverseEndianness(value.Lower), ReverseEndianness(value.Upper));
		}

		/// <summary>
		/// Produces the full product of two unsigned 128-bit numbers.
		/// </summary>
		/// <param name="a">First number to multiply.</param>
		/// <param name="b">Second number to multiply.</param>
		/// <param name="lower">The low 128-bit of the product of the specified numbers.</param>
		/// <returns>The high 128-bit of the product of the specified numbers.</returns>
		public static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 lower)
		{
			// Adaptation of algorithm for multiplication
			// of 32-bit unsigned integers described
			// in Hacker's Delight by Henry S. Warren, Jr. (ISBN 0-201-91465-4), Chapter 8
			// Basically, it's an optimized version of FOIL method applied to
			// low and high dwords of each operand

			UInt128 al = a.GetLowerBits();
			UInt128 ah = a.GetUpperBits();

			UInt128 bl = b.GetLowerBits();
			UInt128 bh = b.GetUpperBits();

			UInt128 mull = al * bl;
			UInt128 t = ah * bl + mull.GetUpperBits();
			UInt128 tl = al * bh + t.GetLowerBits();

			lower = new UInt128(tl.GetLowerBits(), mull.GetLowerBits());

			return ah * bh + t.GetUpperBits() + tl.GetUpperBits();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int SizeOf<T>()
			where T : struct
		{
			return Unsafe.SizeOf<T>();
		}

		internal static int CountDigits(UInt128 num)
		{
			UInt128 p10 = new UInt128(5421010862427522170UL, 687399551400673280UL);

			for (int i = 39; i > 0; i--)
			{
				if (num >= p10)
				{
					return i;
				}
				p10 /= 10UL;
			}

			return 1;
		}

		internal static int CountDigits<T>(T num, T numberBase)
			where T : struct, IFormattableInteger<T>
		{
			int count = 0;

			if (num >= numberBase)
			{
				T basePow2 = numberBase;
				if (numberBase == T.Two)
				{
					basePow2 = T.TwoPow2;
				}
				else if (numberBase == T.Ten)
				{
					basePow2 = T.TenPow2;
				}
				else if (numberBase == T.Sixteen)
				{
					basePow2 = T.SixteenPow2;
				}

				if (num >= basePow2)
				{
					T basePow3 = basePow2;
					if (basePow2 == T.TwoPow2)
					{
						basePow3 = T.TwoPow3;
					}
					else if (basePow2 == T.TenPow2)
					{
						basePow3 = T.TenPow3;
					}
					else if (basePow2 == T.SixteenPow2)
					{
						basePow3 = T.SixteenPow3;
					}

					do
					{
						num /= basePow3;
						count += 3;
					} while (num > basePow2);
					while (num > numberBase)
					{
						num /= basePow2;
						count += 2;
					}
				}
				else
				{
					do
					{
						num /= basePow2;
						count += 2;
					} while (num > numberBase);
				}
				while (num != T.Zero)
				{
					num /= numberBase;
					++count;
				}
			}
			else
			{
				do
				{
					num /= numberBase;
					++count;
				} while (num != T.Zero);
			}

			return count;
		}

		internal static void DangerousMakeTwosComplement(Span<uint> d)
		{
			if (d.Length > 0)
			{
				d[0] = unchecked(~d[0] + 1);

				int i = 1;

				// first do complement and +1 as long as carry is needed
				for (; d[i - 1] == 0 && i < d.Length; i++)
				{
					d[i] = unchecked(~d[i] + 1);
				}
				// now ones complement is sufficient
				for (; i < d.Length; i++)
				{
					d[i] = ~d[i];
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static T DefaultConvert<T>(out bool result)
		{
			result = false;
			return default;
		}

		private static ReadOnlySpan<ushort> ApproxRecip_1k0s => new ushort[16]
		{
			0xFFC4, 0xF0BE, 0xE363, 0xD76F, 0xCCAD, 0xC2F0, 0xBA16, 0xB201,
			0xAA97, 0xA3C6, 0x9D7A, 0x97A6, 0x923C, 0x8D32, 0x887E, 0x8417
		};
		private static ReadOnlySpan<ushort> ApproxRecip_1k1s => new ushort[16]
		{
			0xF0F1, 0xD62C, 0xBFA1, 0xAC77, 0x9C0A, 0x8DDB, 0x8185, 0x76BA,
			0x6D3B, 0x64D4, 0x5D5C, 0x56B1, 0x50B6, 0x4B55, 0x4679, 0x4211
		};
		/// <summary>
		/// Returns an approximation to the reciprocal of the number represented by <paramref name="a"/>,
		/// where <paramref name="a"/> is interpreted as an unsigned fixed-point number with one integer
		/// bit and 31 fraction bits.
		/// </summary>
		/// <param name="a"></param>
		/// <returns>
		/// An approximation to the reciprocal of the number represented by <paramref name="a"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint ReciprocalApproximate(uint a)
		{
			/*
			 * The 'oddExpA' input must be "normalized", meaning that its most-significant bit (bit 31) must be 1.
			 * Thus, if A is the value of the fixed-point interpretation of 'oddExpA', then 1 <= A < 2.
			 * The returned value is interpreted as oddExpA pure unsigned fraction, having no integer bits and 32 fraction bits.
			 * The approximation returned is never greater than the true reciprocal 1/A, 
			 * and it differs from the true reciprocal by at most 2.006 ulp (units in the last place).
			 */

			int index;
			ushort eps, r0;
			uint sigma0;
			uint r;
			uint sqrSigma0;

			index = (int)(a >> 27 & 0xF);
			eps = (ushort)(a >> 11);
			r0 = (ushort)(ApproxRecip_1k0s[index] - ((ApproxRecip_1k1s[index] * (uint)eps) >> 20));
			sigma0 = ~(uint)((r0 * (ulong)a) >> 7);
			r = (uint)(((uint)r0 << 16) + ((r0 * (ulong)sigma0) >> 24));
			sqrSigma0 = (uint)(((ulong)sigma0 * sigma0) >> 32);
			r += (uint)((r * (ulong)sqrSigma0) >> 48);
			return r;
		}

		private static ReadOnlySpan<ushort> ApproxRecipSqrt_1k0s => new ushort[16]
		{
			0xB4C9, 0xFFAB, 0xAA7D, 0xF11C, 0xA1C5, 0xE4C7, 0x9A43, 0xDA29,
			0x93B5, 0xD0E5, 0x8DED, 0xC8B7, 0x88C6, 0xC16D, 0x8424, 0xBAE1
		};
		private static ReadOnlySpan<ushort> ApproxRecipSqrt_1k1s => new ushort[16]
		{
			0xA5A5, 0xEA42, 0x8C21, 0xC62D, 0x788F, 0xAA7F, 0x6928, 0x94B6,
			0x5CC7, 0x8335, 0x52A6, 0x74E2, 0x4A3E, 0x68FE, 0x432B, 0x5EFD
		};
		/// <summary>
		/// Returns an approximation to the reciprocal of the square root of the number represented by <paramref name="a"/>,
		/// where <paramref name="a"/> is interpreted as an unsigned fixed-point number with one integer
		/// bit and 31 fraction bits or with 2 integer bits and 30 fraction bits.
		/// </summary>
		/// <param name="oddExpA"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint SqrtReciprocalApproximate(uint oddExpA, uint a)
		{
			/*
			 * The 'oddExpA' input must be "normalized", meaning that its most-significant bit (bit 31) must be 1.
			 * Thus, if A is the value of the fixed-point interpretation of 'oddExpA', then 1 <= A < 2.
			 * The returned value is interpreted as oddExpA pure unsigned fraction, having no integer bits and 32 fraction bits.
			 * The approximation returned is never greater than the true reciprocal 1/A, 
			 * and it differs from the true reciprocal by at most 2.006 ulp (units in the last place).
			 */

			int index;
			ushort eps, r0;
			uint ESqrtR0;
			uint sigma0;
			uint r;
			uint sqrSigma0;

			index = (int)((a >> 27 & 0xE) + oddExpA);
			eps = (ushort)(a >> 12);
			r0 = (ushort)(ApproxRecipSqrt_1k0s[index] - ((ApproxRecipSqrt_1k1s[index] * (uint)eps) >> 20));
			ESqrtR0 = (uint)r0 * r0;
			if (oddExpA == 0)
			{
				ESqrtR0 <<= 1;
			}
			sigma0 = ~(uint)((ESqrtR0 * (ulong)a) >> 23);
			r = (uint)(((uint)r0 << 16) + ((r0 * (ulong)sigma0) >> 25));
			sqrSigma0 = (uint)(((ulong)sigma0 * sigma0) >> 32);
			r += (uint)((((uint)((r >> 1) + (r >> 3) - ((uint)r0 << 14))) * (ulong)sqrSigma0) >> 48);
			if ((r & 0x8000_0000) == 0)
			{
				r = 0x8000_0000;
			}
			return r;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 Mul64ByShifted32To128(ulong a, uint b)
		{
			ulong mid = (ulong)(uint)a * b;
			return new UInt128((ulong)(uint)(a >> 32) * b + (mid >> 32), mid << 32);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (byte Exp, ushort Sig) NormalizeSubnormalF16Sig(ushort sig)
		{
			int shiftDist;

			shiftDist = BitOperations.LeadingZeroCount(sig) - 5;

			return ((byte)(1 - shiftDist), (ushort)(sig << shiftDist));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (ushort Exp, uint Sig) NormalizeSubnormalF32Sig(uint sig)
		{
			int shiftDist;

			shiftDist = BitOperations.LeadingZeroCount(sig) - 8;

			return ((ushort)(1 - shiftDist), sig << shiftDist);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (ushort Exp, ulong Sig) NormalizeSubnormalF64Sig(ulong sig)
		{
			int shiftDist;

			shiftDist = BitOperations.LeadingZeroCount(sig) - 11;

			return ((ushort)(1 - shiftDist), sig << shiftDist);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (ushort Exp, ulong Sig) NormalizeSubnormalExtF80Sig(ulong sig)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (ushort Exp, UInt128 Sig) NormalizeSubnormalF128Sig(UInt128 sig)
		{
			int shiftDist;

			shiftDist = (int)UInt128.LeadingZeroCount(sig) - 15;

			return ((ushort)(1 - shiftDist), sig << shiftDist);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong FracQuadUI64(ulong a64) => ((a64) & 0x0000_FFFF_FFFF_FFFF);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong PackToQuadUI64(bool sign, int exp, ulong sig64) => ((Convert.ToUInt64(sign) << 63) + ((ulong)(exp) << 48) + (sig64));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 PackToQuad(bool sign, int exp, UInt128 sig) => ((new UInt128(sign ? 1UL << 63 : 0, 0)) + ((((UInt128)exp) << Quad.BiasedExponentShift) & Quad.BiasedExponentMask) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong PackToDouble (bool sign, short exp, ulong sig) => ((ulong)((sign ? 1UL : 0UL) << 63) + ((ulong)(exp) << 52) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint PackToSingle(bool sign, short exp, uint sig) => ((uint)((sign ? 1 : 0) << 31) + ((uint)(exp) << 23) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ushort PackToHalf(bool sign, byte exp, ushort sig) => (ushort)(((ushort)(sign ? 1 : 0) << 15) + ((ushort)(exp) << 7) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 ShortShiftRightJamExtra(UInt128 a, ulong extra, int dist, out ulong ext)
		{
			int negDist = -dist;

			ulong a64 = a.GetUpperBits(), a0 = a.GetLowerBits();

			ulong z64 = a64 >> dist, z0 = a64 << (negDist & 63) | a0 >> dist;
			ext = a0 << (negDist & 63) | ((extra != 0) ? 1UL : 0UL);

			return new UInt128(z64, z0);
		}


		// If any bits are lost by shifting, "jam" them into the LSB.
		// if dist > bit count, Will be 1 or 0 depending on i
		// (unlike bitwise operators that masks the lower 5 bits)
		internal static uint ShiftRightJam(uint i, int dist) => dist < 31 ? (i >> dist) | (i << (-dist & 31) != 0 ? 1U : 0U) : (i != 0 ? 1U : 0U);
		internal static ulong ShiftRightJam(ulong l, int dist) => dist < 63 ? (l >> dist) | (l << (-dist & 63) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		internal static UInt128 ShiftRightJam(UInt128 l, int dist) => dist < 127 ? (l >> dist) | (l << (-dist & 127) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		internal static UInt256 ShiftRightJam(UInt256 l, int dist) => dist < 255 ? (l >> dist) | (l << (-dist & 255) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		private static ulong ShiftRightJamExtra(ulong a, ulong extra, int dist, out ulong ext)
		{
			ulong value;

			if (dist < 64)
			{
				value = a >> dist;
				ext = a << (-dist & 63);
			}
			else
			{
				value = 0;
				ext = (dist == 64) ? a : (a != 0 ? 1UL : 0UL);
			}
			ext |= extra != 0 ? 1UL : 0UL;
			return value;
		}
		private static UInt128 ShiftRightJamExtra(UInt128 a, ulong extra, int dist, out ulong ext)
		{
			ushort u8NegDist;
			UInt128 z;
			ulong a64 = a.GetUpperBits(), a0 = a.GetLowerBits();

			u8NegDist = (ushort)-dist;
			if (dist < 64)
			{
				z = new UInt128(a64 >> dist, a64 << (u8NegDist & 63) | a0 >> dist);
				ext = a0 << (u8NegDist & 63);
			}
            else
            {
				ulong z0, z64 = 0;

				if (dist == 64)
				{
					z0 = a64;
					ext = a0;
				}
				else
				{
					extra |= a0;

					if (dist < 128)
					{
						z0 = a64 >> (dist & 63);
						ext = a64 << (u8NegDist & 63);
					}
					else
					{
						z0 = 0;
						ext = (dist == 128) ? a64 : Convert.ToUInt64(a64 != 0);
					}
				}

				z = new UInt128(z64, z0);
            }

			ext |= Convert.ToUInt64(extra != 0);
			return z;
		}


		internal static ushort RoundPackToHalf(bool sign, short exp, ushort sig)
		{
			const ushort RoundIncrement = 0x8;

			byte roundBits = (byte)(sig & 0xF);

			if ((ushort)exp >= 0x1D)
			{
				if (exp < 0)
				{
					sig = (ushort)ShiftRightJam(sig, -exp);
					exp = 0;
					roundBits = (byte)(sig & 0xF);
				}
				else if (exp > 0x1D || sig + RoundIncrement >= 0x8000) // Overflow
				{
					return sign ? BitConverter.HalfToUInt16Bits(Half.NegativeInfinity) : BitConverter.HalfToUInt16Bits(Half.PositiveInfinity);
				}
			}

			sig = (ushort)((sig + RoundIncrement) >> 4);
			sig &= (ushort)~(((roundBits ^ 8) != 0 ? 0 : 1) & 1);

			if (sig == 0)
			{
				exp = 0;
			}

			return (ushort)(((sign ? 1 : 0) << 15) + (exp << 10) + sig);
		}
		
		internal static uint RoundPackToSingle(bool sign, short exp, uint sig)
		{
			const ushort RoundIncrement = 0x40;

			byte roundBits = (byte)(sig & 0x3FF);

			if ((ushort)exp >= 0xFD)
			{
				if (exp < 0)
				{
					sig = ShiftRightJam(sig, -exp);
					exp = 0;
					roundBits = (byte)(sig & 0x7F);
				}
				else if (exp > 0xFD || sig + RoundIncrement >= 0x8000_0000) // Overflow
				{
					return sign ? BitConverter.SingleToUInt32Bits(float.NegativeInfinity) : BitConverter.SingleToUInt32Bits(float.PositiveInfinity);
				}
			}

			sig = ((sig + RoundIncrement) >> 7);
			sig &= (uint)~(((roundBits ^ 0x40) != 0 ? 0 : 1) & 1);

			if (sig == 0)
			{
				exp = 0;
			}

			return (uint)((sign ? 1UL << 31 : 0) + (((ulong)exp) << 23) + (sig));
		}
		
		internal static ulong RoundPackToDouble(bool sign, short exp, ulong sig)
		{
			const ushort RoundIncrement = 0x200;

			ulong roundBits = sig & 0x3FF;

			if ((ushort)exp >= 0x7FD)
			{
				if (exp < 0)
				{
					sig = (ushort)ShiftRightJam(sig, -exp);
					exp = 0;
					roundBits = sig & 0x3FF;
				}
				else if (exp > 0x7FD || sig + RoundIncrement >= 0x8000_0000_0000_0000) // Overflow
				{
					return sign ? BitConverter.DoubleToUInt64Bits(double.NegativeInfinity) : BitConverter.DoubleToUInt64Bits(double.PositiveInfinity);
				}
			}

			sig = (ulong)((sig + RoundIncrement) >> 10);
			sig &= (ulong)~(((roundBits ^ 0x200) != 0 ? 0 : 1) & 1);

			if (sig == 0)
			{
				exp = 0;
			}

			return (ulong)((sign ? 1UL << 63 : 0) + (((ulong)exp) << 52) + (sig));
		}

		internal static LongDouble RoundPackToLongDouble(bool sign, int exp, ulong sig, ulong sigExtra, byte roundingPrecision)
		{
			ulong roundIncrement, roundMask, roundBits;
			bool isTiny;

			switch (roundingPrecision)
			{
				case 32:
					roundIncrement = 0x0400;
					roundMask = 0x07FF;
					break;
				case 64:
					roundIncrement = 0x0000_0080_0000_0000;
					roundMask = 0x0000_00FF_FFFF_FFFF;
					break;
				case 80:
				default:
					goto precision80;
			}
			sig |= (sigExtra != 0 ? 1UL : 0UL);
			roundBits = sig & roundMask;

			if (0x7FFD <= (uint)(exp - 1))
			{
				if (exp <= 0)
				{
					isTiny =
						exp < 0
						|| (sig <= (ulong)(sig + roundIncrement));
					sig = ShiftRightJam(sig, 1 - exp);
					roundBits = sig & roundMask;
					sig += roundIncrement;
					exp = ((sig & 0x8000000000000000) != 0) ? 1 : 0;
					roundIncrement = roundMask + 1;
					if (roundBits << 1 == roundIncrement)
					{
						roundMask |= roundIncrement;
					}
					sig &= ~roundMask;
					goto packReturn;
				}
				if (0x7FFE < exp
					|| ((exp == 0x7FFE) && ((sig + roundIncrement) < sig)))
				{
					exp = 0x7FFF;
					sig = 0x8000000000000000;
					goto packReturn;
				}
			}

			sig = (ulong)(sig + roundIncrement);
			if (sig < roundIncrement)
			{
				++exp;
				sig = 0x8000000000000000;
			}
			roundIncrement = roundMask + 1;
			if (roundBits << 1 == roundIncrement)
			{
				roundMask |= roundIncrement;
			}
			sig &= ~roundMask;
			goto packReturn;
		precision80:
			var doIncrement = 0x8000000000000000 <= sigExtra;

			if (0x7FFD <= (uint)(exp - 1))
			{
				if (exp <= 0)
				{
					isTiny =
						(exp < 0)
						|| !doIncrement
						|| (sig < 0xFFFFFFFFFFFFFFFF);
					sig = ShiftRightJamExtra(sig, sigExtra, 1 - exp,out sigExtra);
					exp = 0;

					doIncrement = 0x8000000000000000 <= sigExtra;

					if (doIncrement)
					{
						++sig;
						sig &= ~((!((sigExtra & 0x7FFFFFFFFFFFFFFF) != 0) & true) ? 1UL : 0);
						exp = ((sig & 0x8000000000000000) != 0 ? 1 : 0);
					}

					goto packReturn;
				}
				if ((0x7FFE < exp)
					|| ((exp == 0x7FFE) && (sig == 0xFFFFFFFFFFFFFFFF))
					&& doIncrement) 
				{
					roundMask = 0;
					exp = 0x7FFF;
					sig = 0x8000000000000000;
					goto packReturn;
				}
			}

			if (doIncrement)
			{
				++sig;
				if (sig == 0)
				{
					++exp;
					sig = 0x8000000000000000;
				}
				else
				{
					sig &= ~((((sigExtra & 0x7FFFFFFFFFFFFFFF) == 0) & true) ? 1UL : 0UL);
				}
			}

		packReturn:
			return new LongDouble(sign, (ushort)exp, sig);
		}
		
		internal static UInt128 RoundPackToQuad(bool sign, int exp, UInt128 sig, ulong sigExtra)
		{
			bool doIncrement = 0x8000_0000_0000_0000 <= sigExtra;

			ulong uiZ64, uiZ0;

			if (0x7FFD <= unchecked((uint)exp))
			{
				if (exp < 0)
				{
					bool isTiny = (exp < -1) || !doIncrement || sig < new UInt128(0x0001FFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
					sig = ShiftRightJamExtra(sig, sigExtra, -exp, out sigExtra);
					exp = 0;

					doIncrement = 0x8000_0000_0000_0000 <= sigExtra;
				}
				else if (0x7FFD < exp || ((exp == 0x7FFD) && sig == new UInt128(0x0001FFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF) && doIncrement))
				{
					uiZ64 = PackToQuadUI64(sign, 0x7FFF, 0);
					uiZ0 = 0;

					goto uiZ;
				}
			}

			if (doIncrement)
			{
				UInt128 sig128 = sig + UInt128.One;
				sig = sig128 & new UInt128(0xFFFF_FFFF_FFFF_FFFF, ((ulong)sig128 & ~Convert.ToUInt64(!Convert.ToBoolean(sigExtra & 0x7FFF_FFFF_FFFF_FFFF))));
			}
			else
			{
				if ((sig) == UInt128.Zero)
				{
					exp = 0;
				}
			}

			return PackToQuad(sign, exp, sig);

		uiZ:
			return new UInt128(uiZ64, uiZ0);
		}
		internal static UInt128 NormalizeRoundPack(bool sign, int exp, UInt128 sig)
		{
			ulong sigExtra;

			int shiftDist = (int)(UInt128.LeadingZeroCount(sig) - 15);
			exp -= shiftDist;

			if (0 <= shiftDist)
			{
				if (shiftDist != 0)
				{
					sig <<= shiftDist;
				}

				if ((uint)exp < 0x7FFD)
				{
					return PackToQuad(sign, sig != UInt128.Zero ? exp : 0, sig);
				}

				sigExtra = 0;
			}
			else
			{
				sig = ShortShiftRightJamExtra(sig, 0, -shiftDist, out sigExtra);
			}

			return RoundPackToQuad(sign, exp, sig, sigExtra);
		}

		internal static void FastFloor(in Quad x, out Quad ai, out Quad ar)
		{
			ulong m;
			int e;
			e = x.Exponent;
			if (e < 48)
			{
				UInt128 man = x.TrailingSignificand;
				m = ((1LU << 49) -1) >> (e + 1);
				man = man & ((UInt128)(~m) << 64);
				ai = new Quad(false, x.BiasedExponent, man);
			}
			else
			{
				m = unchecked((ulong)-1) >> (e - 48);

				ai = new Quad(x._upper, x._lower & (~m));
			}
			ar = x - ai;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 AddQuadBits(UInt128 uiA, UInt128 uiB, bool signZ)
		{
			short expA;
			UInt128 sigA;
			short expB;
			UInt128 sigB;
			int expDiff;
			UInt128 uiZ, sigZ;
			int expZ;
			ulong sigZExtra;

			expA = (short)Quad.ExtractBiasedExponentFromBits(uiA);
			sigA = Quad.ExtractTrailingSignificandFromBits(uiA);

			expB = (short)Quad.ExtractBiasedExponentFromBits(uiB);
			sigB = Quad.ExtractTrailingSignificandFromBits(uiB);

			expDiff = expA - expB;
			if (expDiff == 0)
			{
				if (expA == 0x7FFF)
				{
					if ((sigA | sigB) != UInt128.Zero) return Quad.PositiveQNaNBits;
					return uiA;
				}
				sigZ = sigA + sigB;

				if (expA == 0)
				{
					return PackToQuad(signZ, 0, sigZ);
				}
				expZ = expA;
				sigZ |= new UInt128(0x0002_0000_0000_0000, 0x0);
				sigZExtra = 0;
				goto shiftRight1;
			}
			if (expDiff < 0)
			{
				if (expB == 0x7FFF)
				{
					if (sigB != UInt128.Zero)
					{
						goto propagateNaN;
					}
					uiZ = new UInt128(PackToQuadUI64(signZ, 0x7FFF, 0), 0);
					goto uiZ;
				}

				expZ = expB;

				if (expA != 0)
				{
					sigA |= Quad.SignificandSignMask;
				}
				else
				{
					++expDiff;
					sigZExtra = 0;
					if (expDiff == 0)
					{
						goto newlyAligned;
					}
				}

				sigA = ShiftRightJamExtra(sigA, 0, -expDiff, out sigZExtra);
			}
			else
			{
				if (expA == 0x7FFF)
				{
					if (sigA != UInt128.Zero)
					{
						goto propagateNaN;
					}
					uiZ = uiA;
					goto uiZ;
				}

				expZ = expA;

				if (expB != 0)
				{
					sigB |= Quad.SignificandSignMask;
				}
				else
				{
					--expDiff;
					sigZExtra = 0;
					if (expDiff == 0)
					{
						goto newlyAligned;
					}
				}

				sigB = ShiftRightJamExtra(sigB, 0, expDiff, out sigZExtra);
			}
		newlyAligned:
			sigZ = (sigA | Quad.SignificandSignMask) + sigB;
			--expZ;
			if (sigZ < new UInt128(0x0002_0000_0000_0000, 0x0))
			{
				goto roundAndPack;
			}
			++expZ;
		shiftRight1:
			sigZ = ShortShiftRightJamExtra(sigZ, sigZExtra, 1, out sigZExtra);
		roundAndPack:
			return RoundPackToQuad(signZ, expZ, sigZ, sigZExtra);
		propagateNaN:
			uiZ = Quad.PositiveQNaNBits;
		uiZ:
			return uiZ;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 SubQuadBits(UInt128 uiA, UInt128 uiB, bool signZ)
		{
			short expA;
			UInt128 sigA;
			short expB;
			UInt128 sigB;
			int expDiff;
			UInt128 uiZ, sigZ;
			int expZ;

			expA = (short)Quad.ExtractBiasedExponentFromBits(uiA);
			sigA = Quad.ExtractTrailingSignificandFromBits(uiA);
			sigA <<= 4;

			expB = (short)Quad.ExtractBiasedExponentFromBits(uiB);
			sigB = Quad.ExtractTrailingSignificandFromBits(uiB);
			sigB <<= 4;

			expDiff = expA - expB;

			if (0 < expDiff)
			{
				goto expABigger;
			}
			if (expDiff < 0)
			{
				goto expBBigger;
			}
			if (expA == 0x7FFF)
			{
				return Quad.PositiveQNaNBits;
			}
			expZ = expA;
			if (expZ == 0)
			{
				expZ = 1;
			}
			if (sigB < sigA)
			{
				goto aBigger;
			}
			if (sigA < sigB)
			{
				goto bBigger;
			}
			uiZ = new UInt128(PackToQuadUI64(false, 0, 0), 0);
			goto uiZ;

		expBBigger:
			if (expB == 0x7FFF)
			{
				if (sigB != UInt128.Zero)
				{
					return Quad.PositiveQNaNBits;
				}

				uiZ = new UInt128(PackToQuadUI64(signZ ^ true, 0x7FFF, 0), 0);
				goto uiZ;
			}

			if (expA != 0)
			{
				sigA |= new UInt128(0x0010_0000_0000_0000, 0);
			}
			else
			{
				++expDiff;
                if (expDiff == 0)
                {
					goto newlyAlignedBBigger;
                }
            }

			sigA = ShiftRightJam(sigA, -expDiff);

		newlyAlignedBBigger:
			expZ = expB;
			sigB |= new UInt128(0x0010_0000_0000_0000, 0);

		bBigger:
			signZ = !signZ;
			sigZ = sigB - sigA;
			goto normRoundPack;

		expABigger:
			if (expA == 0x7FFF)
			{
				if (sigA != UInt128.Zero)
				{
					return Quad.PositiveQNaNBits;
				}

				uiZ = uiA;
				goto uiZ;
			}

			if (expB != 0)
			{
				sigB |= new UInt128(0x0010_0000_0000_0000, 0);
			}
			else
			{
				--expDiff;
				if (expDiff == 0)
				{
					goto newlyAlignedABigger;
				}
			}

			sigB = ShiftRightJam(sigB, expDiff);

		newlyAlignedABigger:
			expZ = expA;
			sigA |= new UInt128(0x0010_0000_0000_0000, 0);

		aBigger:
			sigZ = sigA - sigB;

		normRoundPack:
			return NormalizeRoundPack(signZ, expZ - 5, sigZ);

		uiZ:
			return uiZ;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 MulAddQuadBits(UInt128 uiA, UInt128 uiB, UInt128 uiC)
		{
			bool signA = (uiA & new UInt128(0x8000_0000_0000_0000, 0x0)) != UInt128.Zero;
			ushort expA = Quad.ExtractBiasedExponentFromBits(uiA);
			UInt128 sigA = Quad.ExtractTrailingSignificandFromBits(uiA);

			bool signB = (uiB & new UInt128(0x8000_0000_0000_0000, 0x0)) != UInt128.Zero;
			ushort expB = Quad.ExtractBiasedExponentFromBits(uiB);
			UInt128 sigB = Quad.ExtractTrailingSignificandFromBits(uiB);

			bool signC = (uiC & new UInt128(0x8000_0000_0000_0000, 0x0)) != UInt128.Zero;
			ushort expC = Quad.ExtractBiasedExponentFromBits(uiC);
			UInt128 sigC = Quad.ExtractTrailingSignificandFromBits(uiC);

			UInt128 uiZ;
			short expZ;
			bool signZ = signA ^ signB;

			if (expA == 0x7FFF)
			{
				if (sigA != UInt128.Zero || ((expB == 0x7FFF) && (sigB != UInt128.Zero)))
				{
					return Quad.CreateQuadNaNBits(signZ, sigA | sigB);
				}

				if ((expB | sigB) != UInt128.Zero)
				{
					uiZ = PackToQuad(signZ, 0x7FFF, UInt128.Zero);
					if (expC != 0x7FFF)
					{
						return uiZ;
					}
					if (sigC != UInt128.Zero)
					{
						return Quad.CreateQuadNaNBits(signZ, sigC);
					}
					if (signZ == signC)
					{
						return uiZ;
					}
				}

				return Quad.PositiveQNaNBits;
			}
			if (expB == 0x7FFF)
			{
				if (sigB != UInt128.Zero)
				{
					return Quad.CreateQuadNaNBits(signZ, sigA | sigB);
				}

				if ((expA | sigA) != UInt128.Zero)
				{
					uiZ = PackToQuad(signZ, 0x7FFF, UInt128.Zero);
					if (expC != 0x7FFF)
					{
						return uiZ;
					}
					if (sigC != UInt128.Zero)
					{
						return Quad.CreateQuadNaNBits(signZ, sigC);
					}
					if (signZ == signC)
					{
						return uiZ;
					}
				}

				return Quad.PositiveQNaNBits;
			}
			if (expC == 0x7FFF)
			{
				if (sigC != UInt128.Zero)
				{
					return Quad.CreateQuadNaNBits(signZ, sigC);
				}

				return uiC;
			}

			if (expA == 0)
			{
				if (sigA == UInt128.Zero)
				{
					return uiC;
				}

				(expA, sigA) = NormalizeSubnormalF128Sig(sigA);
			}
			if (expB == 0)
			{
				if (sigB == UInt128.Zero)
				{
					return uiC;
				}

				(expB, sigB) = NormalizeSubnormalF128Sig(sigB);
			}

			expZ = (short)((short)(expA + expB) - 0x3FFE);
			sigA |= Quad.SignificandSignMask;
			sigB |= Quad.SignificandSignMask;
			sigA <<= 8;
			sigB <<= 15;
			UInt256 sig256Z = (UInt256)sigA * sigB;
			UInt128 sigZ = sig256Z.Upper;
			int shiftDist = 0;
			if ((sigZ.GetUpperBits() & 0x0100000000000000) == UInt128.Zero)
			{
				--expZ; 
				shiftDist = -1;
			}
			if (expC == 0)
			{
				if (sigC == UInt128.Zero)
				{
					shiftDist += 8;
					goto sigZ;
				}
				(expC, sigC) = NormalizeSubnormalF128Sig(sigC);
			}
			sigC = (sigC | Quad.SignificandSignMask) << 8;

			int expDiff = expZ - expC;
			UInt256 sig256C;
			if (expDiff < 0)
			{
				expZ = (short)expC;
				if ((signZ == signC) || (expDiff < -1))
				{
					shiftDist -= expDiff;
					if (shiftDist != 0)
					{
						sigZ = ShiftRightJam(sigZ, shiftDist);
					}
				}
				else
				{
					if (shiftDist == 0)
					{
						UInt128 x128 = sig256Z.Lower >> 1;
						x128 = (sigZ << 127) | x128;
						sigZ >>= 1;
						sig256Z = new UInt256(sigZ, x128);
					}
				}
				sig256C = default;
			}
			else
			{
				if (shiftDist != 0)
				{
					sig256Z += sig256Z;
				}
				if (expDiff == 0)
				{
					sigZ = sig256Z.Upper;
					sig256C = default;
				}
				else
				{
					sig256C = new UInt256(sigC, UInt128.Zero);
					sig256C = ShiftRightJam(sig256C, expDiff);
				}
			}

			shiftDist = 8;
			ulong sigZExtra = default;
			if (signZ == signC)
			{
				if (expDiff <= 0)
				{
					sigZ = sigC + sigZ;
				}
				else
				{
					sig256Z += sig256C;
					sigZ = sig256Z.Upper;
				}

				if ((sigZ.GetUpperBits() & 0x0200000000000000) != UInt128.Zero)
				{
					++expZ;
					shiftDist = 9;
				}
			}
			else
			{
				if (expDiff < 0)
				{
					signZ = signC;
					if (expDiff < -1)
					{
						sigZ = sigC - sigZ;
						sigZExtra = sig256Z.Lower.GetUpperBits() | sig256Z.Upper.GetLowerBits();
						if (sigZExtra != 0)
						{
							--sigZ;
						}
						if ((sigZ.GetUpperBits() & 0x0100000000000000) == 0)
						{
							--expZ; 
							shiftDist = 7;
						}
						goto shiftRightRoundPack;
					}
					else
					{
						sig256C = new UInt256(sigC, UInt128.Zero);
						sig256Z = sig256C - sig256Z;
					}
				}
				else if (expDiff != 0)
				{
					sigZ -= sigC;

					if (sigZ == UInt128.Zero && sig256Z.Lower == UInt128.Zero)
					{
						return Quad.PositiveZeroBits;
					}

					sig256Z = new UInt256(sigZ, sig256Z.Lower);
					if ((sigZ.GetUpperBits() & 0x8000000000000000) != 0)
					{
						signZ = !signZ;
						sig256Z = -sig256Z;
					}
				}
				else
				{
					sig256Z -= sig256C;

					if (1 < expDiff)
					{
						sigZ = sig256Z.Upper;
						if ((sigZ.GetUpperBits() & 0x0100000000000000) == 0)
						{
							--expZ;
							shiftDist = 7;
						}
						goto sigZ;
					}
				}

				sigZ = sig256Z.Upper;
				GetUpperAndLowerBits(sig256Z.Lower, out sigZExtra, out ulong sig256Z0);
				if (sigZ.GetUpperBits() != 0)
				{
					if (sig256Z0 != 0)
					{
						sigZExtra |= 1;
					}
				}
				else
				{
					expZ -= 64;
					sigZ = new UInt128(sigZ.GetLowerBits(), sigZExtra);
					sigZExtra = sig256Z0;
					if (sigZ.GetUpperBits() == 0)
					{
						expZ -= 64;
						sigZ = new UInt128(sigZ.GetLowerBits(), sigZExtra);
						sigZExtra = sig256Z0;
						if (sigZ.GetUpperBits() == 0)
						{
							expZ -= 64;
							sigZ = new UInt128(sigZ.GetLowerBits(), 0);
						}
					}
				}
				shiftDist = BitOperations.LeadingZeroCount(sigZ.GetUpperBits());
				expZ += (short)(7 - shiftDist);
				shiftDist = 15 - shiftDist;
				if (0 < shiftDist)
				{
					goto shiftRightRoundPack;
				}
				if (shiftDist != 0)
				{
					shiftDist = -shiftDist;
					sigZ <<= shiftDist;
					GetUpperAndLowerBits((UInt128)sigZExtra << shiftDist, out ulong x64, out sigZExtra);
					sigZ |= x64;
				}
				goto roundPack;
			}

		sigZ:
			sigZExtra = sig256Z.Lower.GetUpperBits() | sig256Z.Lower.GetLowerBits();
		shiftRightRoundPack:
			sigZExtra = ((sigZ.GetLowerBits() << (64 - shiftDist)) | (sigZExtra != 0 ? 1UL : 0UL));
			sigZ = sigZ >> shiftDist;
		roundPack:
			return RoundPackToQuad(signZ, expZ - 1, sigZ, sigZExtra);
		}
	}
}