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
}
