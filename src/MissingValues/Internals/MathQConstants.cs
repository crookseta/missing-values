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
		public static ReadOnlySpan<Quad> AtanTBL => new Quad[]
		{

		};



		public static Quad Huge => new Quad(0x7FF8_136C_69CE_8ADF, 0xF439_7B05_0CAE_44C6);
	}
}
