using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	// TODO: Finish implementing math functions so that Quad can implemente the IBinaryFloatingPointIEEE interface...

	internal ref struct Shape
	{
		internal ref Word i;
		internal ref Word64 i2;

        public Shape(ref Quad quad)
        {
			i = ref Unsafe.As<Quad, Word>(ref quad);
			i2 = ref Unsafe.As<Quad, Word64>(ref quad);
        }

		[StructLayout(LayoutKind.Sequential)]
		public struct Word
		{
#if BIGENDIAN
			internal ushort se;
			internal ushort top;
			internal uint mid;
			internal ulong lo;
#else
			internal ulong lo;
			internal uint mid;
			internal ushort top;
			internal ushort se;
#endif
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct Word64
		{
#if BIGENDIAN
			internal ulong hi;
			internal ulong lo;
#else
			internal ulong lo;
			internal ulong hi;
#endif
		}
    }
	internal readonly ref struct ReadOnlyShape
	{
		internal readonly ref Word i;
		internal readonly ref Word64 i2;

        public ReadOnlyShape(ref Quad quad)
        {
			i = ref Unsafe.As<Quad, Word>(ref quad);
			i2 = ref Unsafe.As<Quad, Word64>(ref quad);
        }

		[StructLayout(LayoutKind.Sequential)]
		public readonly struct Word
		{
#if BIGENDIAN
			internal readonly ushort se;
			internal readonly ushort top;
			internal readonly uint mid;
			internal readonly ulong lo;
#else
			internal readonly ulong lo;
			internal readonly uint mid;
			internal readonly ushort top;
			internal readonly ushort se;
#endif
		}
		[StructLayout(LayoutKind.Sequential)]
		public readonly struct Word64
		{
#if BIGENDIAN
			internal readonly ulong hi;
			internal readonly ulong lo;
#else
			internal readonly ulong lo;
			internal readonly ulong hi;
#endif
		}
    }

	internal static partial class MathQ
	{
		internal const int MaxRoundingDigits = 34;

		private static Quad M_PI_4 => new Quad(0x3FFE_921F_B544_42D1, 0x8469_B288_3370_1F13); // pi / 4
		private static Quad LOG2EA => new Quad();
		private static Quad SQRTH => new Quad();

		private static Quad RoundLimit => new Quad(0x4073_3426_172C_74D8, 0x22B8_78FE_8000_0000); // 1E35
		internal static ReadOnlySpan<Quad> RoundPower10 => new Quad[MaxRoundingDigits + 1] 
		{ 
			new Quad(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4002_4000_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4005_9000_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4008_F400_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x400C_3880_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x400F_86A0_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4012_E848_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4016_312D_0000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4019_7D78_4000_0000, 0x0000_0000_0000_0000),
			new Quad(0x401C_DCD6_5000_0000, 0x0000_0000_0000_0000),
			new Quad(0x4020_2A05_F200_0000, 0x0000_0000_0000_0000),
			new Quad(0x4023_7487_6E80_0000, 0x0000_0000_0000_0000),
			new Quad(0x4026_D1A9_4A20_0000, 0x0000_0000_0000_0000),
			new Quad(0x402A_2309_CE54_0000, 0x0000_0000_0000_0000),
			new Quad(0x402D_6BCC_41E9_0000, 0x0000_0000_0000_0000),
			new Quad(0x4030_C6BF_5263_4000, 0x0000_0000_0000_0000),
			new Quad(0x4034_1C37_937E_0800, 0x0000_0000_0000_0000),
			new Quad(0x4037_6345_785D_8A00, 0x0000_0000_0000_0000),
			new Quad(0x403A_BC16_D674_EC80, 0x0000_0000_0000_0000),
			new Quad(0x403E_158E_4609_13D0, 0x0000_0000_0000_0000),
			new Quad(0x4041_5AF1_D78B_58C4, 0x0000_0000_0000_0000),
			new Quad(0x4044_B1AE_4D6E_2EF5, 0x0000_0000_0000_0000),
			new Quad(0x4048_0F0C_F064_DD59, 0x2000_0000_0000_0000),
			new Quad(0x404B_52D0_2C7E_14AF, 0x6800_0000_0000_0000),
			new Quad(0x404E_A784_379D_99DB, 0x4200_0000_0000_0000),
			new Quad(0x4052_08B2_A2C2_8029, 0x0940_0000_0000_0000),
			new Quad(0x4055_4ADF_4B73_2033, 0x4B90_0000_0000_0000),
			new Quad(0x4058_9D97_1E4F_E840, 0x1E74_0000_0000_0000),
			new Quad(0x405C_027E_72F1_F128, 0x1308_8000_0000_0000),
			new Quad(0x405F_431E_0FAE_6D72, 0x17CA_A000_0000_0000),
			new Quad(0x4062_93E5_939A_08CE, 0x9DBD_4800_0000_0000),
			new Quad(0x4065_F8DE_F880_8B02, 0x452C_9A00_0000_0000),
			new Quad(0x4069_3B8B_5B50_56E1, 0x6B3B_E040_0000_0000),
			new Quad(0x406C_8A6E_3224_6C99, 0xC60A_D850_0000_0000),
			new Quad(0x406F_ED09_BEAD_87C0, 0x378D_8E64_0000_0000),
		};

		// Domain [-0.7854, 0.7854], range ~[-1.80e-37, 1.79e-37]:
		// |cos(x) - c(x))| < 2**-122.0
		private static Quad C1 => new Quad(0x3FFA_5555_5555_5555, 0x5555_5555_5555_5548);
		private static Quad C2 => new Quad(0xBFF5_6C16_C16C_16C1, 0x6C16_C16C_16BF_5C98);
		private static Quad C3 => new Quad(0x3FEF_A01A_01A0_1A01, 0xA01A_019F_FFC4_B13D);
		private static Quad C4 => new Quad(0xBFE9_27E4_FB77_89F5, 0xC72E_EF94_869C_AC2A);
		private static Quad C5 => new Quad(0x3FE2_1EED_8EFF_8D89, 0x7B51_B5F6_2EA9_599A);
		private static Quad C6 => new Quad(0xBFDA_9397_4A8C_07C9, 0xC2A3_8FC4_4BBC_8DF5);
		private static Quad C7 => new Quad(0x3FD2_AE7F_3E73_3B48, 0xDDA7_3725_A8CB_76C2);
		private static Quad C8 => new Quad(0xBFCA_6827_863B_100E, 0xC1D2_05BD_6344_4584);
		private static Quad C9 => new Quad(0x3FC1_E542_B8A1_08C0, 0x71EB_27E7_68BA_79E3);
		private static Quad C10 => new Quad(0xBFB9_0CE2_0CD8_68A2, 0x04B8_FF44_E6BF_56E0);
		private static Quad C11 => new Quad(0x3FAF_EF81_27D7_65B0, 0x90B7_B2A6_9D9B_4DA3);

		// Domain [-0.7854, 0.7854], range ~[-1.53e-37, 1.659e-37]
		// |sin(x)/x - s(x)| < 2**-122.1
		private static Quad S1 => new Quad(0xBFFC_5555_5555_5555, 0x5555_5555_5555_5555);
		private static Quad S2 => new Quad(0x3FF8_1111_1111_1111, 0x1111_1111_1111_107F);
		private static Quad S3 => new Quad(0xBFF2_A01A_01A0_1A01, 0xA01A_01A0_19F8_F785);
		private static Quad S4 => new Quad(0x3FEC_71DE_3A55_6C73, 0x38FA_AC1C_5571_67FE);
		private static Quad S5 => new Quad(0xBFE5_AE64_567F_544E, 0x38FE_7342_6974_AE93);
		private static Quad S6 => new Quad(0x3FDE_6124_613A_86D0, 0x97C5_C00C_FA3D_6509);
		private static Quad S7 => new Quad(0xBFD6_AE7F_3E73_3B81, 0xDC97_972A_DED6_8D8D);
		private static Quad S8 => new Quad(0x3FCE_952C_7703_0A96, 0x9D8A_B423_F5C4_7870);
		private static Quad S9 => new Quad(0xBFC6_2F49_B467_96FE, 0x2939_DA87_05A1_8D0A);
		private static Quad S10 => new Quad(0x3FBD_71B8_EE20_94BA, 0xE4FA_A8EE_0F63_1EF8);
		private static Quad S11 => new Quad(0xBFB4_7619_0E26_27FC, 0xD447_C8CE_C732_8FCB);
		private static Quad S12 => new Quad(0x3FAB_3D19_FFD7_AD8B, 0xF1DD_C6F8_CBDE_24B6);
		// Domain [-0.67434, 0.67434], range ~[-3.37e-36, 1.982e-37]
		// |tan(x)/x - t(x)| < 2**-117.8 (XXX should be ~1e-37)
		private static Quad T3 => new Quad(0x3FFD_5555_5555_5555, 0x5555_5555_5555_5555);
		private static Quad T5 => new Quad(0x3FFB_1111_1111_1111, 0x1111_1111_1111_1111);
		private static Quad T7 => new Quad(0x3FF9_4AFD_6A05_2BF5, 0xA814_AFD6_A052_BF5B);
		private static Quad T9 => new Quad(0x3FF8_BACF_914C_1BAC, 0xF914_C1BA_CF91_4C1C);
		private static Quad T11 => new Quad(0x3FF8_3991_C2C1_87F6, 0x3371_E9F3_C04E_6471);
		private static Quad T13 => new Quad(0x3FF7_C46E_2EDF_04B7, 0x7CF3_29D6_48F7_37F3);
		private static Quad T15 => new Quad(0x3FF7_50FF_D3D7_CBD5, 0xE03D_BA3D_CD6C_A9DE);
		private static Quad T17 => new Quad(0x3FF7_035D_CFDB_8799, 0x4DF9_3F49_6D43_A78E);
		private static Quad T19 => new Quad(0x3FF6_3CED_5147_D527, 0xDE68_E31D_1D4D_A384);
		private static Quad T21 => new Quad(0x3FF5_69F0_F52C_180C, 0xD4F8_26F2_B404_E630);
		private static Quad T23 => new Quad(0x3FF4_9D3B_1169_5AC7, 0x1678_4F31_194D_1DA6);
		private static Quad T25 => new Quad(0x3FF3_CDE3_E0F7_FB55, 0x386E_CE7F_1EE1_C803);
		private static Quad T27 => new Quad(0x3FF3_0100_7209_8FB6, 0x724C_7800_92C5_9B12);
		private static Quad T29 => new Quad(0x3FF2_20DF_545B_CF0B, 0x653C_F18A_0CF1_3671);
		private static Quad T31 => new Quad(0x3FF1_4335_BB9F_BC81, 0x8BCF_005A_4BF6_DB2C);
		private static Quad T33 => new Quad(0x3FF0_6966_313F_170F, 0xED67_CE16_ADD6_8D4C);
		private static Quad T35 => new Quad(0x3FEF_920B_2709_36E0, 0x6EF5_8910_3109_E713);
		private static Quad T37 => new Quad(0x3FEE_C4C4_612C_6A34, 0x2B75_272F_80AC_827B);
		private static Quad Pio4 => new Quad(0x3FFE_921F_B544_42D1, 0x8469_898C_C517_01B8);
		private static Quad Pio4lo => new Quad(0x3F8C_CD12_90C7_B259, 0x07DD_492F_6840_C751);
		private static Quad T39 => new Quad(0x3FE5_E8A7_5929_7793, 0x7F54_CCE1_AFCF_5393);
		private static Quad T41 => new Quad(0x3FE4_9BAA_1B12_2321, 0x8F8C_A1EF_55EF_5447);
		private static Quad T43 => new Quad(0x3FE3_0738_5DFB_2452, 0x9040_8081_0F2D_A186);
		private static Quad T45 => new Quad(0x3FE2_DC6C_702A_0526, 0x20B3_B94C_D647_048A);
		private static Quad T47 => new Quad(0xBFE1_9ECE_F356_9EBB, 0x5EDD_03A8_74BE_875B);
		private static Quad T49 => new Quad(0x3FE2_94C0_668D_A786, 0x9F57_234D_6237_8C55);
		private static Quad T51 => new Quad(0xBFE2_2E76_3B88_4526, 0x808D_F7F5_A784_CC40);
		private static Quad T53 => new Quad(0x3FE1_A92F_C98C_2955, 0x3DA4_66BA_7143_E38D);
		private static Quad T55 => new Quad(0xBFE0_5110_6CBC_779A, 0x8FC0_7E96_AD48_C11E);
		private static Quad T57 => new Quad(0x3FDE_47ED_BDBA_6F43, 0xA141_91C3_09F7_7315);


		/// <summary>
		/// Returns the absolute value of x quadruple-precision floating-point number.
		/// </summary>
		/// <param name="x">A number that is greater than or equal to <seealso cref="Quad.MinValue"/>, but less than or equal to <seealso cref="Quad.MaxValue"/>.</param>
		/// <returns>A quadruple-precision floating-point number, x, such that 0 ≤ x ≤ <seealso cref="Quad.MaxValue"/>.</returns>
		public static Quad Abs(Quad x)
		{
			return Quad.UInt128BitsToQuad(Quad.QuadToUInt128Bits(x) & Quad.InvertedSignMask);
		}
		/// <summary>
		/// Returns the angle whose cosine is the specified number.
		/// </summary>
		/// <param name="x">A number representing x cosine, where <paramref name="x"/> must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>
		/// An angle, θ, measured in radians, such that 0 ≤ θ ≤ π.
		/// </returns>
		public static Quad Acos(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Returns the angle whose hyperbolic cosine is the specified number.
		/// </summary>
		/// <param name="x">A number representing x hyperbolic cosine, where <paramref name="x"/> must be greater than or equal to 1, but less than or equal to <seealso cref="Quad.PositiveInfinity"/>.</param>
		/// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ ∞.</returns>
		public static Quad Acosh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Asin(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Asinh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Atan(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad Atan2(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Atanh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad BitDecrement(Quad x)
		{
			UInt128 bits = Quad.QuadToUInt128Bits(x);

			if ((bits & Quad.PositiveInfinityBits) >= Quad.PositiveInfinityBits)
			{
				// NaN returns NaN
				// -Infinity returns -Infinity
				// +Infinity returns MaxValue
				return (bits == Quad.PositiveInfinityBits) ? Quad.MaxValue : x;
			}

			if (bits == Quad.PositiveZeroBits)
			{
				// +0.0 returns -Epsilon
				return -Quad.Epsilon;
			}

			// Negative values need to be incremented
			// Positive values need to be decremented

			bits += (UInt128)(((Int128)bits < 0) ? Int128.One : Int128.NegativeOne);
			return Quad.UInt128BitsToQuad(bits);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad BitIncrement(Quad x)
		{
			UInt128 bits = Quad.QuadToUInt128Bits(x);

			if ((bits & Quad.PositiveInfinityBits) >= Quad.PositiveInfinityBits)
			{
				// NaN returns NaN
				// -Infinity returns MinValue
				// +Infinity returns +Infinity
				return (bits == Quad.NegativeInfinityBits) ? Quad.MinValue : x;
			}

			if (bits == Quad.NegativeZeroBits)
			{
				// -0.0 returns Epsilon
				return Quad.Epsilon;
			}

			// Negative values need to be decremented
			// Positive values need to be incremented

			bits += (UInt128)(((Int128)bits < 0) ? Int128.NegativeOne : Int128.One);
			return Quad.UInt128BitsToQuad(bits);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Cbrt(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Ceiling(Quad x)
		{
			if (Quad.IsNaN(x) || Quad.IsInfinity(x))
			{
				return x;
			}

			int unbiasedExponent = x.Exponent;
			if (unbiasedExponent < 0)
			{
				if (Quad.IsNegative(x))
				{
					return Quad.Zero;
				}
				else
				{
					return Quad.One;
				}
			}
			else
			{
				if (unbiasedExponent >= 112)
				{
					return x;
				}

				UInt128 resultBits = Quad.QuadToUInt128Bits(x);
				int bitsToErase = 112 - unbiasedExponent;
				resultBits = (resultBits >> bitsToErase) << bitsToErase;
				Quad result = Quad.UInt128BitsToQuad(resultBits);

				if (result != x && !Quad.IsNegative(result))
				{
					result++;
				}

				return result;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad CopySign(Quad x, Quad y)
		{
			// This method is required to work for all inputs,
			// including NaN, so we operate on the raw bits.
			UInt128 xbits = Quad.QuadToUInt128Bits(x);
			UInt128 ybits = Quad.QuadToUInt128Bits(y);

			// Remove the sign from x, and remove everything but the sign from y
			xbits &= Quad.InvertedSignMask;
			ybits &= Quad.SignMask;

			// Simply OR them to get the correct sign
			return Quad.UInt128BitsToQuad(xbits | ybits);
		}

		private static int __rem_pio2l(Quad x, Span<Quad> y)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Cos(Quad x)
		{
			Quad x0 = x;
			Shape u = new Shape(ref x0);
			uint n;
			Span<Quad> y = stackalloc Quad[2];
			Quad hi, lo;

			u.i.se &= 0x7FFF;
			if (u.i.se == 0x7FFF)
			{
				return x - x;
			}
			x = x0;

			if (x < M_PI_4)
			{
				if (u.i.se < 0x3FF - 113)
				{
					return Quad.One + x;
				}
				return __cos(x, Quad.One);
			}

			n = (uint)__rem_pio2l(x, y);

			hi = y[0];
			lo = y[1];

			return (n & 3) switch
			{
				0 => __cos(hi, lo),
				1 => -__sin(hi, lo, 1),
				2 => -__cos(hi, lo),
				_ => __sin(hi, lo, 1),
			};
		}
		private static Quad __cos(in Quad x, in Quad y)
		{
			/* origin: FreeBSD /usr/src/lib/msun/ld128/k_cosl.c */
			/*
			 * ====================================================
			 * Copyright (C) 1993 by Sun Microsystems, Inc. All rights reserved.
			 * Copyright (c) 2008 Steven G. Kargl, David Schultz, Bruce D. Evans.
			 *
			 * Developed at SunSoft, x Sun Microsystems, Inc. business.
			 * Permission to use, copy, modify, and distribute this
			 * software is freely granted, provided that this notice
			 * is preserved.
			 * ====================================================
			 */

			Quad hz, z, r, w;

			z = x * x;
			r = Poly(in z);
			hz = Quad.HalfOne * z;
			w = Quad.One - hz;

			return w + (((Quad.One - w) - hz) + (z * r - x * y));

			static Quad Poly(in Quad z) => (z * (C1 + z * (C2 + z * (C3 + z * (C4 + z * (C5 + z * (C6 + z * (C7 + z * (C8 + z * (C9 + z * (C10 + z * C11)))))))))));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Cosh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Exp(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Floor(Quad x)
		{
			if (Quad.IsNaN(x) || Quad.IsInfinity(x))
			{
				return x;
			}

			int unbiasedExponent = x.Exponent;
			if (unbiasedExponent < 0)
			{
				if (Quad.IsNegative(x))
				{
					return Quad.NegativeOne;
				}
				else
				{
					return Quad.Zero;
				}
			}
			else
			{
				if (unbiasedExponent >= 112)
				{
					return x;
				}

				UInt128 resultBits = Quad.QuadToUInt128Bits(x);
				int bitsToErase = 112 - unbiasedExponent;
				resultBits = (resultBits >> bitsToErase) << bitsToErase;
				Quad result = Quad.UInt128BitsToQuad(resultBits);

				if (result != x && Quad.IsNegative(result))
				{
					result--;
				}

				return result;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Quad FusedMultiplyAdd(Quad x, Quad y, Quad z)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad IEEERemainder(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int ILogB(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Log(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad Log(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Log10(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Log2(Quad x)
		{
			Quad y, z;
			short e = 0;

			if (Quad.IsNaN(x))
			{
				return x;
			}
			if (Quad.IsInfinity(x))
			{
				return x;
			}
			if (x <= Quad.Zero)
			{
				if (x == Quad.Zero)
				{
					return Quad.NegativeInfinity;
				}
				return Quad.NaN;
			}

			x = Quad.SeparateExponent(x, ref e);

			/* logarithm using log(x) = z + z**3 P(z)/Q(z),
			 * where z = 2(x-1)/x+1)
			 */

			if (e > 2 || e < -2)
			{
				if (x < SQRTH) // 2(2x-1)/(2x+1)
				{
					e -= 1;
					z = x - Quad.HalfOne;
					y = Quad.HalfOne * z + Quad.HalfOne;
				}
				else // 2 (x-1)/(x+1)
				{
					z = x - Quad.HalfOne;
					z -= Quad.HalfOne;
					y = Quad.HalfOne * x + Quad.HalfOne;
				}
				x = z / y;
				z = x * x;
				y = x * (z * PolynomialEvaluator(z, ref MemoryMarshal.GetReference(MathQConstants.Log2.R), 3) / PolynomialEvaluator1(z, ref MemoryMarshal.GetReference(MathQConstants.Log2.S), 3));
				goto done;
			}

			// logarithm using log(1+x) = x - .5x**2 + x**3 P(x)/Q(x)
			if (x < SQRTH)
			{
				e -= 1;
				x = Quad.Two * x - Quad.One;
			}
			else
			{
				x -= Quad.One;
			}

			z = x * x;
			y = x * (z * PolynomialEvaluator(x, ref MemoryMarshal.GetReference(MathQConstants.Log2.P), 6) / PolynomialEvaluator1(x, ref MemoryMarshal.GetReference(MathQConstants.Log2.Q), 7));
			y -= Quad.HalfOne * z;

		done:
			z = y * LOG2EA;
			z += x * LOG2EA;
			z += y;
			z += x;
			z += e;
			return z;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quad Max(Quad val1, Quad val2)
		{
			// This matches the IEEE 754:2019 `maximum` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the greater of the inputs. It
			// treats +0 as greater than -0 as per the specification.

			if (val1 != val2)
			{
				if (!Quad.IsNaN(val1))
				{
					return val2 < val1 ? val1 : val2;
				}

				return val1;
			}

			return Quad.IsNegative(val2) ? val1 : val2;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad MaxMagnitude(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `maximumMagnitude` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the input with a greater magnitude.
			// It treats +0 as greater than -0 as per the specification.

			Quad ax = Abs(x);
			Quad ay = Abs(y);

			if ((ax > ay) || Quad.IsNaN(ax))
			{
				return x;
			}

			if (ax == ay)
			{
				return Quad.IsNegative(x) ? y : x;
			}

			return y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quad Min(Quad val1, Quad val2)
		{
			// This matches the IEEE 754:2019 `minimum` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the lesser of the inputs. It
			// treats +0 as greater than -0 as per the specification.

			if (val1 != val2)
			{
				if (!Quad.IsNaN(val1))
				{
					return val1 < val2 ? val1 : val2;
				}

				return val1;
			}

			return Quad.IsNegative(val1) ? val1 : val2;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad MinMagnitude(Quad x, Quad y)
		{
			// This matches the IEEE 754:2019 `minimumMagnitude` function
			//
			// It propagates NaN inputs back to the caller and
			// otherwise returns the input with a lesser magnitude.
			// It treats +0 as greater than -0 as per the specification.

			Quad ax = Abs(x);
			Quad ay = Abs(y);

			if ((ax < ay) || Quad.IsNaN(ax))
			{
				return x;
			}

			if (ax == ay)
			{
				return Quad.IsNegative(x) ? x : y;
			}

			return y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad Pow(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad ReciprocalEstimate(Quad x)
		{
			// Uses Newton Raphton Series to find 1/x
			Quad x0;
			var bits = Quad.ExtractFromBits(Quad.QuadToUInt128Bits(x));

			// we save the original sign and exponent for later
			bool sign = bits.sign;
			ushort exp = bits.exponent;

			// Expresses D as M × 2e where 1 ≤ M < 2
			// we also get the absolute value while we are at it.
			Quad normalizedValue = new Quad(true, Quad.ExponentBias, bits.matissa);

			x0 = Quad.One;
			Quad two = new Quad(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);

			// 15 iterations should be enough.
			for (int i = 0; i < 15; i++)
			{
				// X1 = X(2 - DX)
				//Quad x1 = f * (two - (normalizedValue * f));
				Quad x1 = x0 * FusedMultiplyAdd(normalizedValue, x0, two);
				// Since we need: two - (normalizedValue * f)
				// to make use of FusedMultiplyAdd, we can rewrite it to (-normalizedValue * f) + two
				// which requires normalizedValue to be negative...

				if (Quad.Abs(x1 - x) < Quad.Epsilon)
				{
					x0 = x1;
					break;
				}

				x0 = x1;
			}

			bits = Quad.ExtractFromBits(Quad.QuadToUInt128Bits(x0));

			bits.exponent -= (ushort)(exp - Quad.ExponentBias);

			var output = new Quad(sign, bits.exponent, bits.matissa);
			return output;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad ReciprocalSqrtEstimate(Quad x) => ReciprocalEstimate(Sqrt(x));
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Round(Quad x)
		{
			// This is based on the 'Berkeley SoftFloat Release 3e' algorithm
			// source: berkeley-softfloat-3/source/f128_roundToInt.c

			UInt128 uiZ, lastBitMask;
			UInt128 bits = Quad.QuadToUInt128Bits(x);
			ulong uiZ64 = x._upper, uiZ0 = x._lower, roundBitsMask;
			ulong lastBitMask64, lastBitMask0;
			ushort biasedExponent = Quad.ExtractBiasedExponentFromBits(bits);

			if (biasedExponent >= 0x402F)
			{
				if (biasedExponent >= 0x406F)
				{
					if (biasedExponent == 0x7FFF && (Quad.ExtractTrailingSignificandFromBits(bits) != UInt128.Zero))
					{
						return Quad.NaN;
					}
					return x;
				}

				lastBitMask0 = (ulong)(2 << (0x406E - biasedExponent));
				roundBitsMask = lastBitMask0 - 1;
				uiZ = bits;

				if (biasedExponent == 0x402F)
				{
					if (uiZ0 >= 0x8000_0000_0000_0000)
					{
						uiZ64++;
						if (uiZ0 == 0x8000_0000_0000_0000)
						{
							uiZ64 &= ~1UL;
						}
					}
				}
				else
				{
					uiZ = new UInt128(uiZ64, uiZ0) + new UInt128(0, lastBitMask0 >> 1);
					if (((ulong)uiZ & roundBitsMask) == 0)
					{
						uiZ &= new UInt128(0xFFFF_FFFF_FFFF_FFFF, ~lastBitMask0);
						//uiZ0 = ((ulong)uiZ);
						//uiZ0 &= ~lastBitMask0;
						//uiZ = new UInt128((ulong)(uiZ >> 64), uiZ0);
					}
				}

				//uiZ0 = ((ulong)uiZ);
				//uiZ0 &= ~roundBitsMask;

				uiZ &= new UInt128(0xFFFF_FFFF_FFFF_FFFF, ~roundBitsMask);

				lastBitMask64 = (lastBitMask0 == 0) ? 0UL : 1UL;
			}
			else
			{
				if (biasedExponent < 0x3FFF)
				{
					if ((bits & new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) == UInt128.Zero)
					{
						return x;
					}
					uiZ = bits & new UInt128(BitHelper.PackToQuadUI64(true, 0, 0), 0);

					return Quad.UInt128BitsToQuad(uiZ);
				}

				uiZ = bits & new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0x0);
				lastBitMask64 = (ulong)1 << (0x402F - biasedExponent);
				roundBitsMask = lastBitMask64 - 1;
				uiZ += new UInt128(lastBitMask64 >> 1, 0);
				if ((((ulong)(uiZ >> 64) & roundBitsMask) | (ulong)bits) == 0)
				{
					uiZ &= new UInt128(~lastBitMask64, 0xFFFF_FFFF_FFFF_FFFF);
				}
				uiZ &= new UInt128(~roundBitsMask, 0xFFFF_FFFF_FFFF_FFFF);
				lastBitMask0 = 0;
			}

			return Quad.UInt128BitsToQuad(uiZ);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="digits"></param>
		/// <returns></returns>
		public static Quad Round(Quad x, int digits)
		{
			return Round(x, digits, MidpointRounding.ToEven);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static Quad Round(Quad x, MidpointRounding mode)
		{
			return Round(x, 0, mode);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="digits"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Quad Round(Quad x, int digits, MidpointRounding mode)
		{
			if ((uint)digits > MaxRoundingDigits)
			{
				Thrower.OutOfRange(nameof(digits));
			}

			if (Abs(x) < RoundLimit)
			{
				Quad power10 = RoundPower10[digits];

				x *= power10;

				x = mode switch
				{
					MidpointRounding.ToEven => Round(x),
					MidpointRounding.AwayFromZero => Truncate(x + CopySign(BitDecrement(Quad.HalfOne), x)),
					MidpointRounding.ToZero => Truncate(x),
					MidpointRounding.ToNegativeInfinity => Floor(x),
					MidpointRounding.ToPositiveInfinity => Ceiling(x),
					_ => throw new ArgumentException("Invalid enum value.", nameof(mode)),
				};

				x /= power10;
			}

			return x;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static Quad ScaleB(Quad x, int n)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		/// <exception cref="ArithmeticException"></exception>
		public static int Sign(Quad x)
		{
			if (x < Quad.Zero)
			{
				return -1;
			}
			else if (x > Quad.Zero)
			{
				return 1;
			}
			else if (x == Quad.Zero)
			{
				return 0;
			}

			throw new ArithmeticException($"{nameof(x)} cannot be {NumberFormatInfo.CurrentInfo.NaNSymbol}");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Sin(Quad x)
		{
			Quad x0 = x;
			Shape u = new Shape(ref x0);
			int n;
			Quad hi, lo;
			Span<Quad> y = stackalloc Quad[2];

			u.i.se &= 0x7fff;
			if (u.i.se == 0x7fff)
				return x - x;
			if (x0 < M_PI_4)
			{
				if (u.i.se < 0x3fff - Quad.MantissaDigits / 2)
				{
					return x;
				}

				return __sin(x, Quad.Zero, 0);
			}
			n = __rem_pio2l(x, y);
			hi = y[0];
			lo = y[1];

			return (n & 3) switch
			{
				0 => __sin(hi, lo, 1),
				1 => __cos(in hi, in lo),
				2 => -__sin(hi, lo, 1),
				_ => -__cos(in hi, in lo),
			};
		}
		private static Quad __sin(Quad x, Quad y, int iy)
		{
			Quad z, r, v;

			z = x * x;
			v = z * x;
			r = Poly(in z);

			if (iy == 0)
			{
				return x + v * (S1 + z * r);
			}

			return x - ((z * (Quad.HalfOne * y - v * r) - y) - v * S1);

			static Quad Poly(in Quad z)
			{
				return (S2 + z * (S3 + z * (S4 + z * (S5 + z * (S6 + z * (S7 + z * (S8 + z * (S9 + z * (S10 + z * (S11 + z * S12))))))))));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static (Quad Sin, Quad Cos) SinCos(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static Quad Sinh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Sqrt(Quad x)
		{
			UInt128 bits = Quad.QuadToUInt128Bits(x);
			bool signA = Quad.IsNegative(x);
			int exp = Quad.ExtractBiasedExponentFromBits(bits);
			UInt128 sig = Quad.ExtractTrailingSignificandFromBits(bits);

			// Is x NaN?
			if (exp == 0x7FFF)
			{
				if (sig != UInt128.Zero)
				{
					return Quad.NaN;
				}
				if (!signA)
				{
					return x;
				}
				return Quad.NaN;
			}
			if (signA)
			{
				if (((UInt128)exp | sig) != UInt128.Zero)
				{
					return x;
				}
				return Quad.NaN;
			}

			if (exp == 0)
			{
				if (sig == UInt128.Zero)
				{
					return x;
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF128Sig(sig);
			}

			/*
			 * `sig32Z' is guaranteed to be a lower bound on the square root of
			 * `sig32A', which makes `sig32Z' also a lower bound on the square root of
			 * `sigA'.
			 */

			int expZ = ((exp - 0x3FFF) >> 1) + 0x3FFE;
			exp &= 1;
			sig |= new UInt128(0x0001000000000000, 0x0);
			uint sig32 = (uint)(sig >> 81);
			uint recipSqrt32 = BitHelper.SqrtReciprocalApproximate((uint)exp, sig32);
			uint sig32Z = (uint)(((ulong) sig32 * recipSqrt32) >> 32);
			UInt128 rem;
			if (exp != 0)
			{
				sig32Z >>= 1;
				rem = sig << 12;
			}
			else
			{
				rem = sig << 13;
			}
			Span<uint> qs = stackalloc uint[3] { 0, 0, sig32Z };
			rem -= new UInt128((ulong)sig32Z * sig32Z, 0x0);

			uint q = (uint)(((uint)(rem >> 66) * (ulong)recipSqrt32) >> 32);
			ulong x64 = (ulong)sig32Z << 32;
			ulong sig64Z = x64 + ((ulong)q << 3);
			UInt128 y = rem << 29;

			UInt128 term;
			do
			{
				term = BitHelper.Mul64ByShifted32To128(x64 + sig64Z, q);
				rem = y - term;
				if (((ulong)(rem >> 64) & 0x8000_0000_0000_0000) == 0)
				{
					break;
				}
				--q;
				sig64Z -= 1 << 3;
			} while (true);
			qs[1] = q;

			q = (uint)(((ulong)(rem >> 66) * recipSqrt32) >> 32);
			y = rem << 29;
			sig64Z <<= 1;

			do
			{
				term = (UInt128)sig64Z << 32;
				term += (ulong)q << 6;
				term += q;
				rem = y - term;
				if (((ulong)(rem >> 64) & 0x8000_0000_0000_0000) == 0)
				{
					break;
				}
				--q;
			} while (true);
			qs[0] = q;

			q = (uint)((((ulong)(rem >> 66) * recipSqrt32) >> 32) + 2);
			ulong sigZExtra = q << 59;
			term = (UInt128)qs[1] << 53;
			UInt128 sigZ = new UInt128((ulong)qs[2] << 18, ((ulong)qs[0] << 24) + (q >> 5)) + term;

			if ((q & 0xF) <= 2)
			{
				q &= ~3U;
				sigZExtra = q << 59;
				y = sigZ << 6;
				y |= sigZExtra >> 58;
				term = y - q;
				y = BitHelper.Mul64ByShifted32To128((ulong)term, q);
				term = BitHelper.Mul64ByShifted32To128((ulong)(term >> 64), q);
				term += (y >> 64);
				rem <<= 20;
				term -= rem;
				/*
				 * The concatenation of `term' and `y.v0' is now the negative remainder
				 * (3 words altogether).
				 */
				if (((ulong)(term >> 64) & 0x8000_0000_0000_0000) != 0)
				{
					sigZExtra |= 1;
				}
				else
				{
					if ((term | (ulong)y) != UInt128.Zero)
					{
						if (sigZExtra != 0)
						{
							--sigZExtra;
						}
						else
						{
							sigZ -= UInt128.One;
							sigZExtra = ~0UL;
						}
					}
				}
			}

			return Quad.UInt128BitsToQuad(BitHelper.RoundPackToQuad(false, expZ, sigZ, sigZExtra));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Tan(Quad x)
		{
			Quad x0 = x;
			Shape u = new Shape(ref x0);
			Span<Quad> y = stackalloc Quad[2];
			int n;

			u.i.se &= 0x7FFF;
			if (u.i.se == 0x7FFF)
			{
				return x - x;
			}
			if (x0 < M_PI_4)
			{
				if (u.i.se < 0x3FFF - Quad.MantissaDigits / 2)
				{
					return x;
				}
				return __tan(x, Quad.Zero, 0);
			}
			n = __rem_pio2l(x, y);
			return __tan(y[0], y[1], n & 1);
		}
		private static Quad __tan(Quad x, Quad y, int odd)
		{
			Quad z, r, v, w, s, a, t;
			bool big, sign = false;

			big = Abs(x) >= new Quad(0x3FFE_5943_17AC_C4EF, 0x88B9_7785_729B_280F); // |x| >= 0.67434
			if (big)
			{
				if (x < Quad.Zero)
				{
					sign = true;
					x = -x;
					y = -y;
				}
				x = (Pio4 - x) + (Pio4lo - y);
				y = Quad.Zero;
			}

			z = x * x;
			w = z * z;
			r = RPoly(w);
			v = z * VPoly(w);
			s = z * x;
			r = y + z * (s * (r + v) + y) + T3 * s;
			w = x + r;

			if (big)
			{
				s = 1 - 2 * odd;
				v = s - Quad.Two * (x + (r - w * w / (w + s)));
				return sign ? -v : v;
			}
			if (odd == 0)
			{
				return w;
			}

			/*
			 * if allow error up to 2 ulp, simply return
			 * -1.0 / (x+r) here
			 *
			 * 
			 * compute -1.0 / (x+r) accurately 
			 */

			Quad temp = new Quad(0x401F_0000_0000_0000, 0x0000_0000_0000_0000);
			z = w;
			z = z + temp - temp;
			v = r - (z - x);        /* z+v = r+x */
			t = a = -1.0 / w;       /* x = -1.0/w */
			t = t + temp - temp;
			s = 1.0 + t * z;
			return t + a * (s + t * v);

			static Quad RPoly(Quad w)
			{
				return (T5 + w * (T9 + w * (T13 + w * (T17 + w * (T21 +
					w * (T25 + w * (T29 + w * (T33 + w * (T37 + w * (T41 +
					w * (T45 + w * (T49 + w * (T53 + w * T57)))))))))))));
			}
            static Quad VPoly(Quad w)
			{
				return (T7 + w * (T11 + w * (T15 + w * (T19 + w * (T23 +
					w * (T27 + w * (T31 + w * (T35 + w * (T39 + w * (T43 +
					w * (T47 + w * (T51 + w * T55))))))))))));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Tanh(Quad x)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Truncate(Quad x)
		{
			return (Quad)(Int128)x;
		}

		private unsafe static Quad PolynomialEvaluator(Quad x, ref Quad p, int n)
		{
			Quad y = p;

			do
			{
				p = ref Unsafe.Add(ref p, 1);
				y = y * x + p;
			} while (--n != 0);

			return y;
		}
		private unsafe static Quad PolynomialEvaluator1(Quad x, ref Quad p, int n)
		{
			--n;
			Quad y = x + p;

			do
			{
				p = ref Unsafe.Add(ref p, 1);
				y = y * x + p;
			} while (--n != 0);

			return y;
		}
	}
}
