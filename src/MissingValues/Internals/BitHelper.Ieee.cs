using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal static partial class BitHelper
	{
		public static Quad GetQuadFromParts(int sign, int exp, UInt128 man)
		{
			const int Bias = Quad.ExponentBias + Quad.BiasedExponentShift;
			UInt128 bits;

			if (man == 0)
			{
				bits = 0;
			}
			else
			{
				// Normalize so that 0x0010 0000 0000 0000 is the highest bit set.
				int cbitShift = ((int)UInt128.LeadingZeroCount(man)) - Quad.BiasedExponentLength;
				if (cbitShift < 0)
					man >>= -cbitShift;
				else
					man <<= cbitShift;
				exp -= cbitShift;

				// Move the point to just behind the leading 1: 0x001.0 0000 0000 0000
				// (112 bits) and skew the exponent (by 0x3FF == 1023).
				exp += Bias;

				if (exp >= Quad.MaxBiasedExponent)
				{
					// Infinity.
					bits = Quad.PositiveInfinityBits;
				}
				else if (exp <= 0)
				{
					// Denormalized.
					exp--;
					if (exp < -Quad.BiasedExponentShift)
					{
						// Underflow to zero.
						bits = 0;
					}
					else
					{
						bits = man >> -exp;
						Debug.Assert(bits != 0);
					}
				}
				else
				{
					// Mask off the implicit high bit.
					bits = (man & Quad.TrailingSignificandMask) | ((UInt128)exp << Quad.BiasedExponentShift);
				}
			}

			if (sign < 0)
				bits |= Quad.SignMask;

			return Quad.UInt128BitsToQuad(bits);
		}
		public static Octo GetOctoFromParts(int sign, int exp, UInt256 man)
		{
			const int Bias = Octo.ExponentBias + Octo.BiasedExponentShift;
			UInt256 bits;

			if (man == 0)
			{
				bits = 0;
			}
			else
			{
				// Normalize so that 0x0010 0000 0000 0000 is the highest bit set.
				int cbitShift = (LeadingZeroCount(in man)) - Octo.BiasedExponentLength;
				if (cbitShift < 0)
					man >>= -cbitShift;
				else
					man <<= cbitShift;
				exp -= cbitShift;

				// Move the point to just behind the leading 1: 0x001.0 0000 0000 0000
				// (112 bits) and skew the exponent (by 0x3FF == 1023).
				exp += Bias;

				if (exp >= Octo.MaxBiasedExponent)
				{
					// Infinity.
					bits = Octo.PositiveInfinityBits;
				}
				else if (exp <= 0)
				{
					// Denormalized.
					exp--;
					if (exp < -Octo.BiasedExponentShift)
					{
						// Underflow to zero.
						bits = 0;
					}
					else
					{
						bits = man >> -exp;
						Debug.Assert(bits != 0);
					}
				}
				else
				{
					// Mask off the implicit high bit.
					bits = (man & Octo.TrailingSignificandMask) | ((UInt256)exp << Octo.BiasedExponentShift);
				}
			}

			if (sign < 0)
				bits |= Octo.SignMask;

			return Octo.UInt256BitsToOcto(bits);
		}

		public static void GetQuadParts(Quad dbl, out int sign, out int exp, out UInt128 man, out bool fFinite)
		{
			const int Bias = Quad.ExponentBias + Quad.BiasedExponentShift;
			UInt128 bits = Quad.QuadToUInt128Bits(dbl);

			sign = 1 - ((int)(bits >> 126) & 2);
			man = bits & Quad.TrailingSignificandMask;
			exp = (int)(bits >> Quad.BiasedExponentShift) & Quad.MaxBiasedExponent;
			if (exp == 0)
			{
				// Denormalized number.
				fFinite = true;
				if (man != UInt128.Zero)
					exp = -(Bias - 1);
			}
			else if (exp == Quad.MaxBiasedExponent)
			{
				// NaN or Infinite.
				fFinite = false;
				exp = int.MaxValue;
			}
			else
			{
				fFinite = true;
				man |= Quad.SignificandSignMask;
				exp -= Bias;
			}
		}
		public static void GetOctoParts(in Octo dbl, out int sign, out int exp, out UInt256 man, out bool fFinite)
		{
			const int Bias = Octo.ExponentBias + Octo.BiasedExponentShift;
			UInt256 bits = Octo.OctoToUInt256Bits(dbl);

			sign = 1 - ((int)(bits.Part3 >> 62) & 2);
			man = bits & Octo.TrailingSignificandMask;
			exp = (int)(bits.Part3 >> 44) & Octo.MaxBiasedExponent;
			if (exp == 0)
			{
				// Denormalized number.
				fFinite = true;
				if (man != UInt256.Zero)
					exp = -(Bias - 1);
			}
			else if (exp == Octo.MaxBiasedExponent)
			{
				// NaN or Infinite.
				fFinite = false;
				exp = int.MaxValue;
			}
			else
			{
				fFinite = true;
				man |= Octo.SignificandSignMask;
				exp -= Bias;
			}
		}

		private static ReadOnlySpan<ushort> ApproxRecip_1k0s =>
		[
			0xFFC4, 0xF0BE, 0xE363, 0xD76F, 0xCCAD, 0xC2F0, 0xBA16, 0xB201,
			0xAA97, 0xA3C6, 0x9D7A, 0x97A6, 0x923C, 0x8D32, 0x887E, 0x8417
		];
		private static ReadOnlySpan<ushort> ApproxRecip_1k1s =>
		[
			0xF0F1, 0xD62C, 0xBFA1, 0xAC77, 0x9C0A, 0x8DDB, 0x8185, 0x76BA,
			0x6D3B, 0x64D4, 0x5D5C, 0x56B1, 0x50B6, 0x4B55, 0x4679, 0x4211
		];
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

		private static ReadOnlySpan<ushort> ApproxRecipSqrt_1k0s =>
		[
			0xB4C9, 0xFFAB, 0xAA7D, 0xF11C, 0xA1C5, 0xE4C7, 0x9A43, 0xDA29,
			0x93B5, 0xD0E5, 0x8DED, 0xC8B7, 0x88C6, 0xC16D, 0x8424, 0xBAE1
		];
		private static ReadOnlySpan<ushort> ApproxRecipSqrt_1k1s =>
		[
			0xA5A5, 0xEA42, 0x8C21, 0xC62D, 0x788F, 0xAA7F, 0x6928, 0x94B6,
			0x5CC7, 0x8335, 0x52A6, 0x74E2, 0x4A3E, 0x68FE, 0x432B, 0x5EFD
		];
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
		internal static (ushort Exp, UInt128 Sig) NormalizeSubnormalF128Sig(UInt128 sig)
		{
			int shiftDist;

			shiftDist = (int)UInt128.LeadingZeroCount(sig) - 15;

			return ((ushort)(1 - shiftDist), sig << shiftDist);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static (uint Exp, UInt256 Sig) NormalizeSubnormalF256Sig(UInt256 sig)
		{
			int shiftDist;

			shiftDist = LeadingZeroCount(in sig) - 19;

			return ((uint)(1 - shiftDist), sig << shiftDist);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong PackToQuadUI64(bool sign, int exp, ulong sig64) => ((Convert.ToUInt64(sign) << 63) + ((ulong)(exp) << 48) + (sig64));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 PackToQuad(bool sign, int exp, UInt128 sig) => ((new UInt128(sign ? 1UL << 63 : 0, 0)) + ((((UInt128)exp) << Quad.BiasedExponentShift) & Quad.BiasedExponentMask) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 PackToOcto(bool sign, int exp, UInt256 sig) => ((new UInt256(sign ? 1UL << 63 : 0, 0, 0, 0)) + ((((UInt256)exp) << Octo.BiasedExponentShift) & Octo.BiasedExponentMask) + (sig));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt128 ShortShiftRightJamExtra(UInt128 a, ulong extra, int dist, out ulong ext)
		{
			int negDist = -dist;

			ulong a64 = a.GetUpperBits(), a0 = a.GetLowerBits();

			ulong z64 = a64 >> dist, z0 = a64 << (negDist & 63) | a0 >> dist;
			ext = a0 << (negDist & 63) | ((extra != 0) ? 1UL : 0UL);

			return new UInt128(z64, z0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 ShortShiftRightJamExtra(UInt256 a, UInt128 extra, int dist, out UInt128 ext)
		{
			int negDist = -dist;

			UInt128 a64 = a.Upper, a0 = a.Lower;

			UInt128 z64 = a64 >> dist, z0 = a64 << (negDist & 127) | a0 >> dist;
			ext = a0 << (negDist & 127) | ((extra != UInt128.Zero) ? UInt128.One : UInt128.Zero);

			return new UInt256(z64, z0);
		}


		// If any bits are lost by shifting, "jam" them into the LSB.
		// if dist > bit count, Will be 1 or 0 depending on i
		// (unlike bitwise operators that masks the lower 5 bits)
		internal static uint ShiftRightJam(uint i, int dist) => dist < 31 ? (i >> dist) | (i << (-dist & 31) != 0 ? 1U : 0U) : (i != 0 ? 1U : 0U);
		internal static ulong ShiftRightJam(ulong l, int dist) => dist < 63 ? (l >> dist) | (l << (-dist & 63) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		internal static UInt128 ShiftRightJam(UInt128 l, int dist) => dist < 127 ? (l >> dist) | (l << (-dist & 127) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		internal static UInt256 ShiftRightJam(UInt256 l, int dist) => dist < 255 ? (l >> dist) | (l << (-dist & 255) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
		internal static UInt512 ShiftRightJam(UInt512 l, int dist) => dist < 511 ? (l >> dist) | (l << (-dist & 511) != 0 ? 1UL : 0UL) : (l != 0 ? 1UL : 0UL);
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
		private static UInt256 ShiftRightJamExtra(UInt256 a, UInt128 extra, int dist, out UInt128 ext)
		{
			ushort u8NegDist;
			UInt256 z;
			UInt128 a64 = a.Upper, a0 = a.Lower;

			u8NegDist = (ushort)-dist;
			if (dist < 128)
			{
				z = new UInt256(a64 >> dist, a64 << (u8NegDist & 127) | a0 >> dist);
				ext = a0 << (u8NegDist & 127);
			}
			else
			{
				UInt128 z0, z64 = 0;

				if (dist == 128)
				{
					z0 = a64;
					ext = a0;
				}
				else
				{
					extra |= a0;

					if (dist < 256)
					{
						z0 = a64 >> (dist & 127);
						ext = a64 << (u8NegDist & 127);
					}
					else
					{
						z0 = 0;
						ext = (dist == 256) ? a64 : Convert.ToUInt64(a64 != UInt128.Zero);
					}
				}

				z = new UInt256(z64, z0);
			}

			ext |= Convert.ToUInt64(extra != UInt128.Zero);
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

		internal static UInt256 RoundPackToOcto(bool sign, int exp, UInt256 sig, UInt128 sigExtra)
		{
			UInt128 signBit = new(0x8000_0000_0000_0000, 0x0);
			bool doIncrement = signBit <= sigExtra;

			if (0x7FFFD <= unchecked((uint)exp))
			{
				UInt256 sigMask = new UInt256(0x0000_1FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
				if (exp < 0)
				{
					bool isTiny = (exp < -1) || !doIncrement || sig < sigMask;
					sig = ShiftRightJamExtra(sig, sigExtra, -exp, out sigExtra);
					exp = 0;

					doIncrement = signBit <= sigExtra;
				}
				else if (0x7FFFD < exp || ((exp == 0x7FFFD) && sig == sigMask && doIncrement))
				{
					return PackToOcto(sign, 0x7FFFF, UInt256.Zero);
				}
			}

			if (doIncrement)
			{
				UInt256 sig256 = sig + UInt256.One;
				sig = sig256 & new UInt256(UInt128.MaxValue, (sig256.Lower & ~(UInt128)Convert.ToUInt64((sigExtra & new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) == UInt128.Zero)));
			}
			else
			{
				if (sig == UInt256.Zero)
				{
					exp = 0;
				}
			}

			return PackToOcto(sign, exp, sig);
		}
		internal static double CreateDouble(bool sign, ushort exp, ulong sig)
		{
			return BitConverter.UInt64BitsToDouble(((sign ? 1UL : 0UL) << 63) + ((ulong)exp << 52) + sig);
		}
		internal static float CreateSingle(bool sign, byte exp, uint sig)
		{
			return BitConverter.UInt32BitsToSingle(((sign ? 1U : 0U) << 31) + ((uint)exp << 23) + sig);
		}
		internal static Half CreateHalf(bool sign, ushort exp, ushort sig)
		{
			return BitConverter.UInt16BitsToHalf(((ushort)(((sign ? 1 : 0) << 15) + (exp << 10) + sig)));
		}
		internal static Octo CreateOctoNaN(bool sign, UInt256 significand)
		{
			UInt256 signInt = (sign ? UInt256.One : UInt256.Zero) << 255;
			UInt256 sigInt = significand >> 20;

			return Octo.UInt256BitsToOcto(
				   signInt
				   | (new UInt256(0x7FFF_F000_0000_0000, 0x0, 0x0, 0x0) | new UInt256(0x0000_0800_0000_0000, 0x0, 0x0, 0x0))
				   | sigInt
				   );
		}
		internal static Quad CreateQuadNaN(bool sign, UInt128 significand)
		{
			UInt128 signInt = (sign ? UInt128.One : UInt128.Zero) << 127;
			UInt128 sigInt = significand >> 16;

			return Quad.UInt128BitsToQuad(
				   signInt
				   | (new UInt128(0x7FFF_0000_0000_0000, 0x0) | new UInt128(0x0000_8000_0000_0000, 0x0))
				   | sigInt
				   );
		}
		internal static double CreateDoubleNaN(bool sign, ulong significand)
		{
			const ulong NaNBits = 0x7FF0_0000_0000_0000 | 0x80000_00000000; // Most significant significand bit

			ulong signInt = (sign ? 1UL : 0UL) << 63;
			ulong sigInt = significand >> 12;

			return BitConverter.UInt64BitsToDouble(signInt | NaNBits | sigInt);
		}
		internal static float CreateSingleNaN(bool sign, ulong significand)
		{
			const uint NaNBits = 0x7F80_0000 | 0x400000; // Most significant significand bit

			uint signInt = (sign ? 1U : 0U) << 31;
			uint sigInt = (uint)(significand >> 41);

			return BitConverter.UInt32BitsToSingle(signInt | NaNBits | sigInt);
		}
		internal static Half CreateHalfNaN(bool sign, ulong significand)
		{
			const uint NaNBits = 0x7C00 | 0x200; // Most significant significand bit

			uint signInt = (sign ? 1U : 0U) << 15;
			uint sigInt = (uint)(significand >> 54);

			return BitConverter.UInt16BitsToHalf((ushort)(signInt | NaNBits | sigInt));
		}
		internal static UInt128 NormalizeRoundPackQuad(bool sign, int exp, UInt128 sig)
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
		internal static UInt256 NormalizeRoundPackOcto(bool sign, int exp, UInt256 sig)
		{
			UInt128 sigExtra;

			int shiftDist = (LeadingZeroCount(in sig) - 19);
			exp -= shiftDist;

			if (0 <= shiftDist)
			{
				if (shiftDist != 0)
				{
					sig <<= shiftDist;
				}

				if ((uint)exp < 0x7FFFD)
				{
					return PackToOcto(sign, sig != UInt256.Zero ? exp : 0, sig);
				}

				sigExtra = 0;
			}
			else
			{
				sig = ShortShiftRightJamExtra(sig, 0, -shiftDist, out sigExtra);
			}

			return RoundPackToOcto(sign, exp, sig, sigExtra);
		}

		internal static void FastFloor(in Quad x, out Quad ai, out Quad ar)
		{
			ulong m;
			int e;
			e = x.Exponent;
			if (e < 48)
			{
				UInt128 man = x.TrailingSignificand;
				m = ((1LU << 49) - 1) >> (e + 1);
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
			return NormalizeRoundPackQuad(signZ, expZ - 5, sigZ);

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
						sigZExtra = sig256Z.Part1 | sig256Z.Part2;
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

					if (sigZ == UInt128.Zero && (sig256Z.Part1 == 0 && sig256Z.Part0 == 0))
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
				sigZExtra = sig256Z.Part1;
				ulong sig256Z0 = sig256Z.Part0;
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
			sigZExtra = sig256Z.Part1 | sig256Z.Part0;
		shiftRightRoundPack:
			sigZExtra = ((sigZ.GetLowerBits() << (64 - shiftDist)) | (sigZExtra != 0 ? 1UL : 0UL));
			sigZ = sigZ >> shiftDist;
		roundPack:
			return RoundPackToQuad(signZ, expZ - 1, sigZ, sigZExtra);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 AddOctoBits(UInt256 uiA, UInt256 uiB, bool signZ)
		{
			const int MaxExp = 0x7FFFF;
			
			int expA;
			UInt256 sigA;
			int expB;
			UInt256 sigB;
			int expDiff;
			UInt256 uiZ, sigZ;
			int expZ;
			UInt128 sigZExtra;

			expA = (int)Octo.ExtractBiasedExponentFromBits(in uiA);
			sigA = Octo.ExtractTrailingSignificandFromBits(in uiA);

			expB = (int)Octo.ExtractBiasedExponentFromBits(in uiB);
			sigB = Octo.ExtractTrailingSignificandFromBits(in uiB);

			expDiff = expA - expB;

			UInt256 sigMask = new UInt256(0x0000_2000_0000_0000, 0x0, 0x0, 0x0);

			if (expDiff == 0)
			{
				if (expA == MaxExp)
				{
					if ((sigA | sigB) != UInt256.Zero) return Octo.PositiveQNaNBits;
					return uiA;
				}
				sigZ = sigA + sigB;

				if (expA == 0)
				{
					return PackToOcto(signZ, 0, sigZ);
				}
				expZ = expA;
				sigZ |= sigMask;
				sigZExtra = UInt128.Zero;
				goto shiftRight1;
			}
			if (expDiff < 0)
			{
				if (expB == MaxExp)
				{
					if (sigB != UInt256.Zero)
					{
						goto propagateNaN;
					}
					uiZ = PackToOcto(signZ, MaxExp, UInt256.Zero);
					goto uiZ;
				}

				expZ = expB;

				if (expA != 0)
				{
					sigA |= Octo.SignificandSignMask;
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
				if (expA == MaxExp)
				{
					if (sigA != UInt256.Zero)
					{
						goto propagateNaN;
					}
					uiZ = uiA;
					goto uiZ;
				}

				expZ = expA;

				if (expB != 0)
				{
					sigB |= Octo.SignificandSignMask;
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
			sigZ = (sigA | Octo.SignificandSignMask) + sigB;
			--expZ;
			if (sigZ < sigMask)
			{
				goto roundAndPack;
			}
			++expZ;
		shiftRight1:
			sigZ = ShortShiftRightJamExtra(sigZ, sigZExtra, 1, out sigZExtra);
		roundAndPack:
			return RoundPackToOcto(signZ, expZ, sigZ, sigZExtra);
		propagateNaN:
			uiZ = Octo.PositiveQNaNBits;
		uiZ:
			return uiZ;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 SubOctoBits(UInt256 uiA, UInt256 uiB, bool signZ)
		{
			const int MaxExp = 0x7FFFF;

			int expA;
			UInt256 sigA;
			int expB;
			UInt256 sigB;
			int expDiff;
			UInt256 uiZ, sigZ;
			int expZ;

			expA = (int)Octo.ExtractBiasedExponentFromBits(in uiA);
			sigA = Octo.ExtractTrailingSignificandFromBits(in uiA);
			sigA <<= 4;

			expB = (int)Octo.ExtractBiasedExponentFromBits(in uiB);
			sigB = Octo.ExtractTrailingSignificandFromBits(in uiB);
			sigB <<= 4;

			expDiff = expA - expB;

			UInt256 lastBit = new UInt256(0x0001_0000_0000_0000, 0, 0, 0);

			if (0 < expDiff)
			{
				goto expABigger;
			}
			if (expDiff < 0)
			{
				goto expBBigger;
			}
			if (expA == MaxExp)
			{
				return Octo.PositiveQNaNBits;
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
			uiZ = PackToOcto(false, 0, UInt256.Zero);
			goto uiZ;

		expBBigger:
			if (expB == MaxExp)
			{
				if (sigB != UInt256.Zero)
				{
					return Octo.PositiveQNaNBits;
				}

				uiZ = PackToOcto(signZ ^ true, MaxExp, UInt256.Zero);
				goto uiZ;
			}
			
			if (expA != 0)
			{
				sigA |= lastBit;
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
			sigB |= lastBit;

		bBigger:
			signZ = !signZ;
			sigZ = sigB - sigA;
			goto normRoundPack;

		expABigger:
			if (expA == MaxExp)
			{
				if (sigA != UInt256.Zero)
				{
					return Octo.PositiveQNaNBits;
				}

				uiZ = uiA;
				goto uiZ;
			}

			if (expB != 0)
			{
				sigB |= lastBit;
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
			sigA |= lastBit;

		aBigger:
			sigZ = sigA - sigB;

		normRoundPack:
			return NormalizeRoundPackOcto(signZ, expZ - 5, sigZ);

		uiZ:
			return uiZ;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static UInt256 MulAddOctoBits(UInt256 uiA, UInt256 uiB, UInt256 uiC)
		{
			const int MaxExp = 0x7FFFF;
			UInt256 signBit = new UInt256(0x8000_0000_0000_0000, 0x0, 0x0, 0x0);

			bool signA = (uiA & signBit) != UInt256.Zero;
			uint expA = Octo.ExtractBiasedExponentFromBits(in uiA);
			UInt256 sigA = Octo.ExtractTrailingSignificandFromBits(in uiA);

			bool signB = (uiB & signBit) != UInt256.Zero;
			uint expB = Octo.ExtractBiasedExponentFromBits(in uiB);
			UInt256 sigB = Octo.ExtractTrailingSignificandFromBits(in uiB);

			bool signC = (uiC & signBit) != UInt256.Zero;
			uint expC = Octo.ExtractBiasedExponentFromBits(in uiC);
			UInt256 sigC = Octo.ExtractTrailingSignificandFromBits(in uiC);

			UInt256 uiZ;
			uint expZ;
			bool signZ = signA ^ signB;


			if (expA == MaxExp)
			{
				if (sigA != UInt256.Zero || ((expB == MaxExp) && (sigB != UInt256.Zero)))
				{
					return Octo.CreateOctoNaNBits(signZ, sigA | sigB);
				}

				if ((expB | sigB) != UInt256.Zero)
				{
					uiZ = PackToOcto(signZ, MaxExp, UInt256.Zero);
					if (expC != MaxExp)
					{
						return uiZ;
					}
					if (sigC != UInt256.Zero)
					{
						return Octo.CreateOctoNaNBits(signZ, sigC);
					}
					if (signZ == signC)
					{
						return uiZ;
					}
				}

				return Octo.PositiveQNaNBits;
			}
			if (expB == MaxExp)
			{
				if (sigB != UInt256.Zero)
				{
					return Octo.CreateOctoNaNBits(signZ, sigA | sigB);
				}

				if ((expA | sigA) != UInt256.Zero)
				{
					uiZ = PackToOcto(signZ, MaxExp, UInt256.Zero);
					if (expC != MaxExp)
					{
						return uiZ;
					}
					if (sigC != UInt256.Zero)
					{
						return Octo.CreateOctoNaNBits(signZ, sigC);
					}
					if (signZ == signC)
					{
						return uiZ;
					}
				}

				return Octo.PositiveQNaNBits;
			}
			if (expC == MaxExp)
			{
				if (sigC != UInt256.Zero)
				{
					return Octo.CreateOctoNaNBits(signZ, sigC);
				}
				return uiC;
			}

			if (expA == 0)
			{
				if (sigA == UInt256.Zero)
				{
					return uiC;
				}
				(expA, sigA) = NormalizeSubnormalF256Sig(sigA);
			}
			if (expB == 0)
			{
				if (sigB == UInt256.Zero)
				{
					return uiC;
				}
				(expB, sigB) = NormalizeSubnormalF256Sig(sigB);
			}

			const int MinShiftDistance = 10;
			const int MaxShiftDistance = 19;

			expZ = expA + expB - 0x3FFFE;
			sigA |= Octo.SignificandSignMask;
			sigB |= Octo.SignificandSignMask;
			sigA <<= MinShiftDistance;
			sigB <<= MaxShiftDistance;
			UInt512 sig512Z = MathQ.BigMul(sigA, sigB);
			UInt256 sigZ = sig512Z.Upper;
			int shiftDist = 0;
			if ((sigZ.Part3 & 0x0100_0000_0000_0000) == 0)
			{
				--expZ;
				shiftDist = -1;
			}
			if (expC == 0)
			{
				if (sigC == UInt256.Zero)
				{
					shiftDist += MinShiftDistance;
					goto sigZ;
				}
				(expC, sigC) = NormalizeSubnormalF256Sig(sigC);
			}
			sigC = (sigC | Octo.SignificandSignMask) << MinShiftDistance;

			int expDiff = (int)expZ - (int)expC;
			UInt512 sig512C;
			if (expDiff < 0)
			{
				expZ = expC;
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
						UInt256 x256 = sig512Z.Lower >> 1;
						x256 = (sigZ << 255) | x256;
						sigZ >>= 1;
						sig512Z = new UInt512(sigZ, x256);
					}
				}
				sig512C = default;
			}
			else
			{
				if (shiftDist != 0)
				{
					sig512Z += sig512Z;
				}
				if (expDiff == 0)
				{
					sigZ = sig512Z.Upper;
					sig512C = default;
				}
				else
				{
					sig512C = ShiftRightJam(new UInt512(sigC, UInt256.Zero), expDiff);
				}
			}
			shiftDist = MinShiftDistance;
			UInt128 sigZExtra;

			if (signZ == signC)
			{
				if (expDiff <= 0)
				{
					sigZ = sigC + sigZ;
				}
				else
				{
					sig512Z += sig512C;
					sigZ = sig512Z.Upper;
				}
				if ((sigZ.Part3 & 0x0080_0000_0000_0000) != 0)
				{
					++expZ;
					shiftDist = MinShiftDistance + 1;
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
						sigZExtra = sig512Z.Lower.Upper | sig512Z.Upper.Lower;
						if (sigZExtra != UInt128.Zero)
						{
							--sigZ;
						}
						if ((sigZ.Part3 & 0x0100_0000_0000_0000) == 0)
						{
							--expZ;
							shiftDist = MinShiftDistance - 1;
						}
						goto shiftRightRoundPack;
					}
					else
					{
						sig512C = new UInt512(sigC, UInt256.Zero);
						sig512Z = sig512C - sig512Z;
					}
				}
				else if (expDiff != 0)
				{
					sigZ -= sigC;

					if (sigZ == UInt256.Zero && sig512Z.Part1 == 0 && sig512Z.Part0 == 0)
					{
						return Octo.PositiveZeroBits;
					}

					sig512Z = new UInt512(sigZ, sig512Z.Lower);
					if ((sigZ.Part3 & 0x8000_0000_0000_0000) != 0)
					{
						signZ = !signZ;
						sig512Z = -sig512Z;
					}
				}
				else
				{
					sig512Z -= sig512C;

					if (1 < expDiff)
					{
						sigZ = sig512Z.Upper;
						if ((sigZ.Part3 & 0x0100_0000_0000_0000) == 0)
						{
							--expZ;
							shiftDist = MinShiftDistance - 1;
						}
						goto sigZ;
					}
				}

				sigZ = sig512Z.Upper;
				sigZExtra = sig512Z.Lower.Upper;
				UInt128 sig512Z0 = sig512Z.Lower.Lower;
				if (sigZ.Upper != UInt128.Zero)
				{
					if (sig512Z0 != UInt128.Zero)
					{
						sigZExtra |= UInt128.One;
					}
				}
				else
				{
					expZ -= 128;
					sigZ = new UInt256(sigZ.Lower, sigZExtra);
					sigZExtra = sig512Z0;
					if (sigZ.Upper == UInt128.Zero)
					{
						expZ -= 128;
						sigZ = new UInt256(sigZ.Lower, sigZExtra);
						sigZExtra = sig512Z0;
						if (sigZ.Upper == UInt128.Zero)
						{
							expZ -= 128;
							sigZ = new UInt256(sigZ.Lower, UInt128.Zero);
						}
					}
				}
				shiftDist = (int)UInt128.LeadingZeroCount(sigZ.Upper);
				expZ += (uint)((MinShiftDistance - 1) - shiftDist);
				shiftDist = MaxShiftDistance - shiftDist;
				if (0 < shiftDist)
				{
					goto shiftRightRoundPack;
				}
				if (shiftDist != 0)
				{
					shiftDist = -shiftDist;
					sigZ <<= shiftDist;
					UInt256 temp = ((UInt256)sigZExtra << shiftDist);
					sigZ |= temp.Upper;
					sigZExtra = temp.Lower;
				}
				goto roundPack;
			}
		sigZ:
			sigZExtra = sig512Z.Lower.Upper | sig512Z.Lower.Lower;
		shiftRightRoundPack:
			sigZExtra = ((sigZ.Lower << (128 - shiftDist)) | (sigZExtra != UInt128.Zero ? 1UL : 0UL));
			sigZ = sigZ >> shiftDist;
		roundPack:
			return RoundPackToOcto(signZ, (int)expZ - 1, sigZ, sigZExtra);
		}
	}
}
