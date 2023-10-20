using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
	internal ref struct Shape
	{
		internal ref Quad f;

		internal ref Word i;
		internal ref Word64 i2;

        public Shape(Quad quad)
        {
			f = quad;

			i = ref Unsafe.As<Quad, Word>(ref f);
			i2 = ref Unsafe.As<Quad, Word64>(ref f);
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

	public static partial class MathQ
	{
		internal const int MaxRoundingDigits = 34;

		private static Quad M_PI_4 => new Quad(0x3FFE_921F_B544_42D1, 0x8469_B288_3370_1F13); // pi / 4

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
		/// Returns the absolute value of a quadruple-precision floating-point number.
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
		/// <param name="x">A number representing a cosine, where <paramref name="x"/> must be greater than or equal to -1, but less than or equal to 1.</param>
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
		/// <param name="x">A number representing a hyperbolic cosine, where <paramref name="x"/> must be greater than or equal to 1, but less than or equal to <seealso cref="Quad.PositiveInfinity"/>.</param>
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
			throw new NotImplementedException();
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
			Shape u = new Shape(x);
			uint n;
			Span<Quad> y = stackalloc Quad[2];
			Quad hi, lo;

			u.i.se &= 0x7FFF;
			if (u.i.se == 0x7FFF)
			{
				return x - x;
			}
			x = u.f;

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
			 * Developed at SunSoft, a Sun Microsystems, Inc. business.
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad Max(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad MaxMagnitude(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static Quad Min(Quad x, Quad y)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Quad MinMagnitude(Quad x, Quad y)
		{
			throw new NotImplementedException();
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
			Quad normalizedValue = new Quad(true, Quad.Bias, bits.matissa);

			x0 = Quad.One;
			Quad two = new Quad(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);

			// 15 iterations should be enough.
			for (int i = 0; i < 15; i++)
			{
				// X1 = X(2 - DX)
				//Quad x1 = x0 * (two - (normalizedValue * x0));
				Quad x1 = x0 * FusedMultiplyAdd(normalizedValue, x0, two);
				// Since we need: two - (normalizedValue * x0)
				// to make use of FusedMultiplyAdd, we can rewrite it to (-normalizedValue * x0) + two
				// which requires normalizedValue to be negative...

				if (Quad.Abs(x1 - x) < Quad.Epsilon)
				{
					x0 = x1;
					break;
				}

				x0 = x1;
			}

			bits = Quad.ExtractFromBits(Quad.QuadToUInt128Bits(x0));

			bits.exponent -= (ushort)(exp - Quad.Bias);

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
			throw new NotImplementedException();
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
			return mode switch
			{
				MidpointRounding.ToEven => Round(x),
				MidpointRounding.AwayFromZero => Truncate(x + CopySign(BitDecrement(Quad.HalfOne), x)),
				_ => Round(x, 0, mode)
			};
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
			Shape u = new Shape(x);
			int n;
			Quad hi, lo;
			Span<Quad> y = stackalloc Quad[2];

			u.i.se &= 0x7fff;
			if (u.i.se == 0x7fff)
				return x - x;
			if (u.f < M_PI_4)
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
			throw new NotImplementedException();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static Quad Tan(Quad x)
		{
			Shape u = new Shape(x);
			Span<Quad> y = stackalloc Quad[2];
			int n;

			u.i.se &= 0x7FFF;
			if (u.i.se == 0x7FFF)
			{
				return x - x;
			}
			if (u.f < M_PI_4)
			{
				if (u.i.se < 0x3FFF - Quad.MantissaDigits / 2)
				{
					return x;
				}
				return __tan(x, Quad.Zero, 0);
			}
			n = __rem_pio2l(x, y);
			return __tan(y[0], y[1], n & 1);

			throw new NotImplementedException();
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
			t = a = -1.0 / w;       /* a = -1.0/w */
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
			throw new NotImplementedException();
		}
	}
}
