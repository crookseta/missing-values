using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Internals;

internal static class MathQConstants
{
	public static class Log2
	{
		public static ReadOnlySpan<Quad> P => new Quad[]
		{
			new Quad(0x3FFD_FF9D_AF73_9C45, 0xFCE3_1EE4_1413_1D0D),
			new Quad(0x4002_588E_58E2_1C69, 0x6EF0_0A73_39AE_0610),
			new Quad(0x4005_36AF_2DF1_8EA3, 0xD516_0C78_3EA6_226D),
			new Quad(0x4007_0034_CFF6_D405, 0xFD5E_C01B_D3DA_813F),
			new Quad(0x4007_A804_A3FF_E4A4, 0xD6B3_5557_1FC3_0212),
			new Quad(0x4007_5695_0E09_3EEC, 0x739B_3BB0_FC71_842F),
			new Quad(0x4005_ADE6_A65C_E816, 0x3671_1264_809C_1A51),
		};
		public static ReadOnlySpan<Quad> Q => new Quad[]
		{
			new Quad(0x4003_77AD_27AB_7E4C, 0x5E73_6E43_7E1C_10F9),
			new Quad(0x4006_84E2_5AF6_0634, 0x2790_3FC0_21D6_0B66),
			new Quad(0x4008_85C3_B266_3326, 0x8939_3359_4E7A_758C),
			new Quad(0x4009_A6CB_0602_AE9C, 0xB6CA_5014_A341_1CD4),
			new Quad(0x4009_FBB1_8086_77A5, 0x4CB9_9C4C_20D1_3582),
			new Quad(0x4009_3D64_39EB_FFD4, 0x7641_E6F4_FC85_DD40),
			new Quad(0x4007_426C_FCC5_AE10, 0xA8B8_4CA1_A4BB_F30D),
		};
		public static ReadOnlySpan<Quad> R => new Quad[]
		{
			new Quad(0x3FF6_02F6_EEC7_F244, 0xDDE7_A1FB_CCFD_1E69),
			new Quad(0xBFFE_7097_BD1E_35F2, 0x2BFA_1CD8_825E_6EBD),
			new Quad(0x4002_58DF_4A78_9F1B, 0x172B_5D58_EE1E_58D7),
			new Quad(0xC004_1DBD_D15D_69C7, 0x1263_54D8_B67F_A0C6),
		};
		public static ReadOnlySpan<Quad> S => new Quad[]
		{
			new Quad(0xC003_A337_7B8A_3F92, 0xF9C8_2D7B_2676_FC64),
			new Quad(0x4006_833C_E2DE_1A20, 0x15E5_B3BB_2A34_5BBD),
			new Quad(0xC007_AC9C_BA0C_1EAA, 0x9AF9_B63D_BDAA_4949),
		};
	}
	public static class Asin
	{
		public static Quad pS0 => new Quad(0x3FFC_5555_5555_5555, 0x5555_5555_5555_5ACB);
		public static Quad pS1 => new Quad(0xBFFE_7733_C865_9C3C, 0x29EA_3614_B02F_2E41);
		public static Quad pS2 => new Quad(0x3FFF_5797_9B5B_674E, 0x91B5_0237_574C_7D1E);
		public static Quad pS3 => new Quad(0xBFFF_5328_2885_3E02, 0x7A76_AE0D_EC75_A3DC);
		public static Quad pS4 => new Quad(0x3FFE_85BC_D120_445D, 0x1E68_6012_F127_DCD9);
		public static Quad pS5 => new Quad(0xBFFD_0650_52B7_026F, 0x590C_744B_6978_9B90);
		public static Quad pS6 => new Quad(0x3FFA_89CD_FE96_9597, 0xD61A_8F51_A8DD_0C37);
		public static Quad pS7 => new Quad(0xBFF7_2203_159A_1D9D, 0x9944_1478_415C_791C);
		public static Quad pS8 => new Quad(0x3FF2_2F25_83B7_EC31, 0x8DDA_1ADA_B639_2121);
		public static Quad pS9 => new Quad(0xBFE8_C42C_02E3_4D40, 0x972F_241C_19F8_A186);
		public static Quad qS1 => new Quad(0xC001_3633_A319_01F9, 0xEC3C_755C_50ED_E994);
		public static Quad qS2 => new Quad(0x4002_3EEB_09AC_020A, 0x0443_12EC_E6E7_A13E);
		public static Quad qS3 => new Quad(0xC002_6A2B_54F9_B826, 0x9216_B712_3B22_1EAB);
		public static Quad qS4 => new Quad(0x4001_EF5C_E06F_68D3, 0xB151_4081_08B0_3471);
		public static Quad qS5 => new Quad(0xC000_A11D_BB7D_E998, 0x9DDC_9D89_0C3A_63C4);
		public static Quad qS6 => new Quad(0x3FFE_A7D9_6024_76DB, 0x8849_72B3_5236_11EE);
		public static Quad qS7 => new Quad(0xBFFB_E679_5462_B32C, 0xA615_34B1_42EE_9992);
		public static Quad qS8 => new Quad(0x3FF8_10D3_9D46_3279, 0x7749_8241_5549_DFE3);
		public static Quad qS9 => new Quad(0xBFF2_A230_064A_F874, 0x87AA_66DC_CC58_B501);

		public static Quad R(Quad z)
		{
			Quad p, q;
			p = z * (pS0 + z * (pS1 + z * (pS2 + z * (pS3 + z * (pS4 + z * (pS5 + z * (pS6 + z * (pS7 + z * (pS8 + z * pS9)))))))));
			q = Quad.One + z * (qS1 + z * (qS2 + z * (qS3 + z * (qS4 + z * (qS5 + z * (qS6 + z * (qS7 + z * (qS8 + z * qS9))))))));
			return p / q;
		}
	}
	public static class Atan
	{
		public static ReadOnlySpan<Quad> AtanHi => new Quad[4]
		{
			new Quad(0x3FFD_DAC6_7056_1BB4, 0xF68A_DFC8_8BD9_7875),
			new Quad(0x3FFE_921F_B544_42D1, 0x8469_898C_C517_01B8),
			new Quad(0x3FFE_F730_BD28_1F69, 0xB200_F10F_5E19_7794),
			new Quad(0x3FFF_921F_B544_42D1, 0x8469_898C_C517_01B8),
		};
		public static ReadOnlySpan<Quad> AtanLo => new Quad[4]
		{
			new Quad(0x3F89_A06D_C282_B0E4, 0xC39B_E01C_59E2_DCDD),
			new Quad(0x3F8B_CD12_9024_E088, 0xA67C_C740_20BB_EA64),
			new Quad(0xBF8B_EBE5_66C9_9ADA, 0x9F23_1BCC_AE27_916C),
			new Quad(0x3F8C_CD12_9024_E088, 0xA67C_C740_20BB_EA64),
		};
		public static ReadOnlySpan<Quad> AT => new Quad[24]
		{
			new Quad(0x3FFD_5555_5555_5555, 0x5555_5555_5555_5551),
			new Quad(0xBFFC_9999_9999_9999, 0x9999_9999_9999_149E),
			new Quad(0x3FFC_2492_4924_9249, 0x2492_4924_9079_4362),
			new Quad(0xBFFB_C71C_71C7_1C71, 0xC71C_71C1_C2AF_E323),
			new Quad(0x3FFB_745D_1745_D174, 0x5D17_4185_B65D_7596),
			new Quad(0xBFFB_3B13_B13B_13B1, 0x3B11_8C19_75E2_A610),
			new Quad(0x3FFB_1111_1111_1111, 0x1057_D0A5_38CA_D4F9),
			new Quad(0xBFFA_E1E1_E1E1_E1E1, 0x889E_87FC_4C05_8330),
			new Quad(0x3FFA_AF28_6BCA_1AE2, 0x8E03_7B91_71F0_F80C),
			new Quad(0xBFFA_8618_6186_1632, 0x788A_5CC7_2D12_DD95),
			new Quad(0x3FFA_642C_8590_766C, 0xAC82_36DD_0429_4B9B),
			new Quad(0xBFFA_47AE_1475_D4D4, 0x58F3_E5A9_9BC9_E94B),
			new Quad(0x3FFA_2F68_4B82_58F7, 0x459A_07ED_D01D_F529),
			new Quad(0xBFFA_1A7B_9140_F096, 0x5161_0294_D698_0DCE),
			new Quad(0x3FFA_0841_D983_C8EF, 0xF246_7423_A0A8_6E23),
			new Quad(0xBFF9_F078_1F2D_5643, 0x79FD_ACD9_A939_4763),
			new Quad(0x3FF9_D3FE_EE0C_8B8D, 0xBEED_3888_2F4E_9337),
			new Quad(0xBFF9_BA14_C1DA_11FB, 0x7C8B_AE68_A03D_216F),
			new Quad(0x3FF9_A078_FBEB_F61B, 0xE4A8_F661_AE4E_EFE2),
			new Quad(0xBFF9_8129_5361_0BD4, 0x4D6F_FEC7_D9A2_3D57),
			new Quad(0x3FF9_4F98_EB13_94EE, 0xE119_CF2D_75D7_C25B),
			new Quad(0xBFF8_FA3B_3B6B_7AE1, 0xB137_A049_9210_FD65),
			new Quad(0x3FF8_1B46_E017_27FC, 0x7DCF_338D_5EAA_9BA0),
			new Quad(0xBFF6_52D9_4B40_71FF, 0x85CE_62C9_80D9_F92C),
		};

		public static Quad Even(Quad x)
		{
			return (AT[0] + x * (AT[2] + x * (AT[4] + x * (AT[6] + x * (AT[8] +
				x * (AT[10] + x * (AT[12] + x * (AT[14] + x * (AT[16] +
				x * (AT[18] + x * (AT[20] + x * AT[22])))))))))));
		}
		public static Quad Odd(Quad x)
		{
			return (AT[1] + x * (AT[3] + x * (AT[5] + x * (AT[7] + x * (AT[9] +
				x * (AT[11] + x * (AT[13] + x * (AT[15] + x * (AT[17] +
				x * (AT[19] + x * (AT[21] + x * AT[23])))))))))));
		}
	}

	public static class Exp
	{
		public static Quad LN2HI => new Quad(0x3FFE_62E4_2FEF_A39E, 0xF357_93C7_6730_07E5);
		public static Quad LN2LO => new Quad(0x3F19_2CB2_8EF7_5125, 0x44DD_5BD7_A1CD_3EB9);
		public static Quad LOG2E => new Quad();
	}
}
