using MissingValues.Info;
using MissingValues.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MissingValues
{
	/// <summary>
	/// Represents an octuple-precision floating-point number.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[JsonConverter(typeof(NumberConverter.OctoConverter))]
	[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
	public readonly partial struct Octo
	{
		internal static UInt256 SignMask => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 InvertedSignMask => new UInt256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal const int SignShift = 255;
		internal const int MantissaDigits = 237;
		internal const int ExponentBias = 262143;
		internal const int BiasedExponentLength = 19;
		internal const int BiasedExponentShift = 236;
		internal const ulong ShiftedBiasedExponentMask = 524_287;

		internal const int MinBiasedExponent = 0x00000;
		internal const int MaxBiasedExponent = 0x7FFFF;
		internal const int MinExponent = -262142;
		internal const int MaxExponent = 262143;
		internal static UInt256 BiasedExponentMask => new UInt256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		internal static UInt256 TrailingSignificandMask => new UInt256(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 SignificandSignMask => new UInt256(0x0000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 InvertedSignificandMask => ~(SignificandSignMask | TrailingSignificandMask);
		internal static UInt256 MinTrailingSignificand => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 MaxTrailingSignificand => new UInt256(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		internal UInt128 Lower => new UInt128(_bits1, _bits0);
		internal UInt128 Upper => new UInt128(_bits3, _bits2);

		#region Bit representation of constants
		internal static UInt256 EpsilonBits => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		internal static UInt256 PositiveZeroBits => new UInt256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeZeroBits => new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveOneBits => new UInt256(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeOneBits => new UInt256(0xBFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveQNaNBits => new UInt256(0x7FFF_F800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeQNaNBits => new UInt256(0xFFFF_F800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 PositiveInfinityBits => new UInt256(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 NegativeInfinityBits => new UInt256(0xFFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static UInt256 MaxValueBits => new UInt256(0x7FFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 MinValueBits => new UInt256(0xFFFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		internal static UInt256 PiBits => new UInt256(0x4000_0921_FB54_442D, 0x1846_9898_CC51_701B, 0x839A_2520_49C1_114C, 0xF98E_8041_77D4_C762);
		internal static UInt256 TauBits => new UInt256(0x4000_05BF_0A8B_1457, 0x6953_55FB_8AC4_04E7, 0xA79E_3B17_38B0_79C5, 0xA6D2_B53C_26C8_228D);
		internal static UInt256 EBits => new UInt256(0x4000_1921_FB54_442D, 0x1846_9898_CC51_701B, 0x839A_2520_49C1_114C, 0xF98E_8041_77D4_C762);
		#endregion
		#region Constants
		internal static Octo Quarter => new Octo(0x3FFF_D000_0000_0000, 0, 0, 0);
		internal static Octo HalfOne => new Octo(0x3FFF_E000_0000_0000, 0, 0, 0);
		internal static Octo Two => new Octo(0x4000_0000_0000_0000, 0, 0, 0);
		internal static Octo ToInt => new Octo(0x400E_B000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		internal static ReadOnlySpan<Octo> RoundPower10 => new Octo[72]
		{
			new Octo(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4000_2400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4000_5900_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4000_8F40_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4000_C388_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4000_F86A_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4001_2E84_8000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4001_6312_D000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4001_97D7_8400_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4001_CDCD_6500_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4002_02A0_5F20_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4002_3748_76E8_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4002_6D1A_94A2_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4002_A230_9CE5_4000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4002_D6BC_C41E_9000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4003_0C6B_F526_3400, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4003_41C3_7937_E080, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4003_7634_5785_D8A0, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4003_ABC1_6D67_4EC8, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4003_E158_E460_913D, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4004_15AF_1D78_B58C, 0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4004_4B1A_E4D6_E2EF, 0x5000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4004_80F0_CF06_4DD5, 0x9200_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4004_B52D_02C7_E14A, 0xF680_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4004_EA78_4379_D99D, 0xB420_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4005_208B_2A2C_2802, 0x9094_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4005_54AD_F4B7_3203, 0x34B9_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4005_89D9_71E4_FE84, 0x01E7_4000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4005_C027_E72F_1F12, 0x8130_8800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4005_F431_E0FA_E6D7, 0x217C_AA00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4006_293E_5939_A08C, 0xE9DB_D480_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4006_5F8D_EF88_08B0, 0x2452_C9A0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4006_93B8_B5B5_056E, 0x16B3_BE04_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4006_C8A6_E322_46C9, 0x9C60_AD85_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4006_FED0_9BEA_D87C, 0x0378_D8E6_4000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4007_3342_6172_C74D, 0x822B_878F_E800_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4007_6812_F9CF_7920, 0xE2B6_6973_E200_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4007_9E17_B843_5769, 0x1B64_03D0_DA80_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4007_D2CE_D32A_16A1, 0xB11E_8262_8890_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4008_0782_87F4_9C4A, 0x1D66_22FB_2AB4_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4008_3D63_29F1_C35C, 0xA4BF_ABB9_F561_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4008_725D_FA37_1A19, 0xE6F7_CB54_395C_A000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4008_A6F5_78C4_E0A0, 0x60B5_BE29_47B3_C800, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4008_DCB2_D6F6_18C8, 0x78E3_2DB3_99A0_BA00, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4009_11EF_C659_CF7D, 0x4B8D_FC90_4004_7440, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4009_466B_B7F0_435C, 0x9E71_7BB4_5005_9150, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4009_7C06_A5EC_5433, 0xC60D_DAA1_6406_F5A4, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4009_B184_27B3_B4A0, 0x5BC8_A8A4_DE84_5986, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x4009_E5E5_31A0_A1C8, 0x72BA_D2CE_1625_6FE8, 0x2000_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400A_1B5E_7E08_CA3A, 0x8F69_8781_9BAE_CBE2, 0x2800_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400A_511B_0EC5_7E64, 0x99A1_F4B1_014D_3F6D, 0x5900_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400A_8561_D276_DDFD, 0xC00A_71DD_41A0_8F48, 0xAF40_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400A_BABA_4714_957D, 0x300D_0E54_9208_B31A, 0xDB10_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400A_F0B4_6C6C_DD6E, 0x3E08_28F4_DB45_6FF0, 0xC8EA_0000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400B_24E1_8788_14C9, 0xCD8A_3332_1216_CBEC, 0xFB24_8000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400B_5A19_E96A_19FC, 0x40EC_BFFE_969C_7EE8, 0x39ED_A000_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400B_9050_31E2_503D, 0xA893_F7FF_1E21_CF51, 0x2434_8400_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400B_C464_3E5A_E44D, 0x12B8_F5FE_E5AA_4325, 0x6D41_A500_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400B_F97D_4DF1_9D60, 0x5767_337E_9F14_D3EE, 0xC892_0E40_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400C_2FDC_A16E_04B8, 0x6D41_005E_46DA_08EA, 0x7AB6_91D0_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400C_63E9_E4E4_C2F3, 0x4448_A03A_EC48_4592, 0x8CB2_1B22_0000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400C_98E4_5E1D_F3B0, 0x155A_C849_A75A_56F7, 0x2FDE_A1EA_8000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400C_CF1D_75A5_709C, 0x1AB1_7A5C_1130_ECB4, 0xFBD6_4A65_2000_0000, 0x0000_0000_0000_0000),
			new Octo(0x400D_0372_6987_6661, 0x90AE_EC79_8ABE_93F1, 0x1D65_EE7F_3400_0000, 0x0000_0000_0000_0000),
			new Octo(0x400D_384F_03E9_3FF9, 0xF4DA_A797_ED6E_38ED, 0x64BF_6A1F_0100_0000, 0x0000_0000_0000_0000),
			new Octo(0x400D_6E62_C4E3_8FF8, 0x7211_517D_E8C9_C728, 0xBDEF_44A6_C140_0000, 0x0000_0000_0000_0000),
			new Octo(0x400D_A2FD_BB0E_39FB, 0x474A_D2EE_B17E_1C79, 0x76B5_8AE8_38C8_0000, 0x0000_0000_0000_0000),
			new Octo(0x400D_D7BD_29D1_C87A, 0x191D_87AA_5DDD_A397, 0xD462_EDA2_46FA_0000, 0x0000_0000_0000_0000),
			new Octo(0x400E_0DAC_7446_3A98, 0x9F64_E994_F555_0C7D, 0xC97B_A90A_D8B8_8000, 0x0000_0000_0000_0000),
			new Octo(0x400E_428B_C8AB_E49F, 0x639F_11FD_1955_27CE, 0x9DED_49A6_C773_5000, 0x0000_0000_0000_0000),
			new Octo(0x400E_772E_BAD6_DDC7, 0x3C86_D67C_5FAA_71C2, 0x4568_9C10_7950_2400, 0x0000_0000_0000_0000),
			new Octo(0x400E_ACFA_698C_9539, 0x0BA8_8C1B_7795_0E32, 0xD6C2_C314_97A4_2D00, 0x0000_0000_0000_0000),
		};
		#endregion

		/// <summary>
		/// Represents the natural logarithmic base, specified by the constant, <c>e</c>.
		/// </summary>
		public static readonly Octo E = new Octo(0x4000_1921_FB54_442D, 0x1846_9898_CC51_701B, 0x839A_2520_49C1_114C, 0xF98E_8041_77D4_C762);
		/// <summary>
		/// Represents the smallest positive <see cref="Octo"/> value that is greater than zero.
		/// </summary>
		public static readonly Octo Epsilon = new Octo(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		/// <summary>
		/// Represents the largest possible value of a <see cref="Octo"/>.
		/// </summary>
		public static readonly Octo MaxValue = new Octo(0x7FFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		/// <summary>
		/// Represents the smallest possible value of a <see cref="Octo"/>.
		/// </summary>
		public static readonly Octo MinValue = new Octo(0xFFFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		/// <summary>
		/// Represents a value that is not a number (<c>NaN</c>).
		/// </summary>
		public static readonly Octo NaN = new Octo(0x7FFF_F800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>-1</c> of the type.
		/// </summary>
		public static readonly Octo NegativeOne = new Octo(0xBFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents negative infinity.
		/// </summary>
		public static readonly Octo NegativeInfinity = new Octo(0xFFFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>-0</c> of the type.
		/// </summary>
		public static readonly Octo NegativeZero = new Octo(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the value <c>1</c> of the type.
		/// </summary>
		public static readonly Octo One = new Octo(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, <c>pi</c>.
		/// </summary>
		public static readonly Octo Pi = new Octo(0x4000_0921_FB54_442D, 0x1846_9898_CC51_701B, 0x839A_2520_49C1_114C, 0xF98E_8041_77D4_C762);
		/// <summary>
		/// Represents positive infinity.
		/// </summary>
		public static readonly Octo PositiveInfinity = new Octo(0x7FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		/// <summary>
		/// Represents the number of radians in one turn, specified by the constant, <c>tau</c>.
		/// </summary>
		public static readonly Octo Tau = new Octo(0x4000_05BF_0A8B_1457, 0x6953_55FB_8AC4_04E7, 0xA79E_3B17_38B0_79C5, 0xA6D2_B53C_26C8_228D);
		/// <summary>
		/// Represents the value <c>0</c> of the type.
		/// </summary>
		public static readonly Octo Zero = new Octo(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		internal uint BiasedExponent
		{
			get
			{
				UInt256 bits = OctoToUInt256Bits(this);
				return ExtractBiasedExponentFromBits(bits);
			}
		}
		internal int Exponent
		{
			get
			{
				return (int)(BiasedExponent - ExponentBias);
			}
		}
		internal UInt256 Significand
		{
			get
			{
				return (TrailingSignificand | ((BiasedExponent != 0) ? (SignificandSignMask) : UInt256.Zero));
			}
		}
		internal UInt256 TrailingSignificand
		{
			get
			{
				UInt256 bits = OctoToUInt256Bits(this);
				return ExtractTrailingSignificandFromBits(bits);
			}
		}

#if BIGENDIAN
		private readonly ulong _bits3;
		private readonly ulong _bits2;
		private readonly ulong _bits1;
		private readonly ulong _bits0;
#else
		private readonly ulong _bits0;
		private readonly ulong _bits1;
		private readonly ulong _bits2;
		private readonly ulong _bits3;
#endif

		internal Octo(ulong u1, ulong u2, ulong l1, ulong l2)
		{
			_bits0 = l2;
			_bits1 = l1;
			_bits2 = u2;
			_bits3 = u1;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Octo" /> struct.
		/// </summary>
		/// <param name="sign">A <see cref="bool"/> indicating the sign of the number. <see langword="true"/> represents a negative number, and <see langword="false"/> represents a positive number.</param>
		/// <param name="exp">An <see cref="uint"/> representing the exponent part of the floating-point number.</param>
		/// <param name="sig">An <see cref="UInt256"/> representing the significand part of the floating-point number.</param>
		public Octo(bool sign, uint exp, UInt256 sig)
		{
			var bits = (sig & TrailingSignificandMask) | new UInt256((sign ? 0x8000_0000_0000_0000 : 0) | ((ulong)(exp & 0x7FFFF) << 44),0,0,0);
			_bits0 = bits.Part0;
			_bits1 = bits.Part1;
			_bits2 = bits.Part2;
			_bits3 = bits.Part3;
		}

		/// <inheritdoc/>
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return (obj is Octo other) && Equals(other);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <inheritdoc/>
		public override string? ToString()
		{
			return ToString("G71", NumberFormatInfo.CurrentInfo);
		}

		/// <summary>
		/// Parses a span of characters into a value.
		/// </summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <returns>The result of parsing <paramref name="s"/>.</returns>
		public static Octo Parse(ReadOnlySpan<char> s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// tries to parse a span of characters into a value.
		/// </summary>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
		/// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, out Octo result)
		{
			return TryParse(s, CultureInfo.CurrentCulture, out result);
		}

		/// <summary>
		/// Reinterprets the specified 256-bit unsigned integer to a octuple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Octo UInt256BitsToOcto(UInt256 bits) => *((Octo*)&bits);
		/// <summary>
		/// Reinterprets the specified 256-bit signed integer to a octuple-precision floating point number.
		/// </summary>
		/// <param name="bits">The number to convert.</param>
		/// <returns>A octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Octo Int256BitsToOcto(Int256 bits) => *((Octo*)&bits);

		/// <summary>
		/// Converts the specified octuple-precision floating point number to a 256-bit unsigned integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 256-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe UInt256 OctoToUInt256Bits(Octo value) => *((UInt256*)&value);
		/// <summary>
		/// Converts the specified octuple-precision floating point number to a 256-bit signed integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 256-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static unsafe Int256 OctoToInt256Bits(Octo value) => *((Int256*)&value);


		internal static bool AreZero(in Octo x, in Octo y)
		{
			return ((OctoToUInt256Bits(x) | OctoToUInt256Bits(y)) & ~SignMask) == UInt256.Zero;
		}
		internal static UInt256 CreateOctoNaNBits(bool sign, UInt256 significand)
		{
			UInt256 signInt = (sign ? UInt256.One : UInt256.Zero) << 255;
			UInt256 sigInt = significand >> 20;

			return signInt | (BiasedExponentMask | new UInt256(0x0000_0800_0000_0000, 0x0, 0x0, 0x0)) | sigInt;
		}
		internal static uint ExtractBiasedExponentFromBits(in UInt256 bits)
		{
			return (uint)((bits >> BiasedExponentShift) & ShiftedBiasedExponentMask);
		}
		internal static (bool sign, uint exponent, UInt256 matissa) ExtractFromBits(in UInt256 bits)
		{
			return ((bits & SignMask) != 0, (uint)(bits >> BiasedExponentShift), (bits & TrailingSignificandMask));
		}
		internal static UInt256 ExtractTrailingSignificandFromBits(in UInt256 bits)
		{
			return (bits & TrailingSignificandMask);
		}
		internal static UInt256 StripSign(Octo value)
		{
			return OctoToUInt256Bits(value) & ~SignMask;
		}
		#region From Octo
		// Unsigned
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator byte(Octo value)
		{
			Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return byte.MinValue;
			}
			if ((value >= twoPow8))
			{
				return byte.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				byte result = (byte)((byte)(bits >> 229) | 0x80);

				result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="byte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="byte"/>.</exception>
		public static explicit operator checked byte(Octo value)
		{
			Octo twoPow8 = new Octo(0x4007_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow8))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				byte result = (byte)((byte)(bits >> 229) | 0x80);

				result >>>= (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return byte.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ushort(Octo value)
		{
			Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return ushort.MinValue;
			}
			if ((value >= twoPow16))
			{
				return ushort.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ushort result = (ushort)((ushort)(bits >> 221) | 0x8000);

				result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ushort"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ushort"/>.</exception>
		public static explicit operator checked ushort(Octo value)
		{
			Octo twoPow16 = new Octo(0x400F_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow16))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ushort result = (ushort)((ushort)(bits >> 221) | 0x8000);

				result >>>= (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ushort.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator uint(Octo value)
		{
			Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return uint.MinValue;
			}
			if ((value >= twoPow32))
			{
				return uint.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				uint result = ((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="uint"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
		public static explicit operator checked uint(Octo value)
		{
			Octo twoPow32 = new Octo(0x4001_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow32))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				uint result = ((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return uint.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator ulong(Octo value)
		{
			Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return ulong.MinValue;
			}
			if ((value >= twoPow64))
			{
				return ulong.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ulong result = ((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="ulong"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="ulong"/>.</exception>
		public static explicit operator checked ulong(Octo value)
		{
			Octo twoPow64 = new Octo(0x4003_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow64))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				ulong result = ((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return ulong.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt128(Octo value)
		{
			Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt128.MinValue;
			}
			if ((value >= twoPow128))
			{
				return UInt128.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt128"/>.</exception>
		public static explicit operator checked UInt128(Octo value)
		{
			Octo twoPow128 = new Octo(0x4007_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow128))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt128 result = ((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt128.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt256(Octo value)
		{
			Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt256.MinValue;
			}
			if ((value >= twoPow256))
			{
				return UInt256.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt256"/>.</exception>
		public static explicit operator checked UInt256(Octo value)
		{
			Octo twoPow256 = new Octo(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow256))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt256 result = ((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));
				return result;
			}
			else
			{
				return UInt256.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator UInt512(Octo value)
		{
			Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative)
			{
				return UInt512.MinValue;
			}
			if ((value >= twoPow512))
			{
				return UInt512.MaxValue;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>= Octo.ExponentBias + 512 - 1 - (int)(bits >> 236);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="UInt512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="UInt512"/>.</exception>
		public static explicit operator checked UInt512(Octo value)
		{
			Octo twoPow512 = new Octo(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
			bool isNegative = Octo.IsNegative(value);

			if (Octo.IsNaN(value) || isNegative || (value >= twoPow512))
			{
				Thrower.IntegerOverflow();
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				UInt512 result = new UInt512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>= Octo.ExponentBias + 512 - 1 - (int)(bits >> 236);
				return result;
			}
			else
			{
				return UInt512.MinValue;
			}
		}
		// Signed
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator sbyte(Octo value)
		{
			Octo twoPow7 = new Octo(0x4000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7)
			{
				return sbyte.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow7)
			{
				return sbyte.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				sbyte result = (sbyte)(((byte)(bits >> 229) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (sbyte)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="sbyte"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="sbyte"/>.</exception>
		public static explicit operator checked sbyte(Octo value)
		{
			Octo twoPow7 = new Octo(0x4000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow7 || Octo.IsNaN(value) || value >= +twoPow7)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				sbyte result = (sbyte)(((byte)(bits >> 229) | 0x80) >>> (Octo.ExponentBias + 8 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (sbyte)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator short(Octo value)
		{
			Octo twoPow15 = new Octo(0x4000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15)
			{
				return short.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow15)
			{
				return short.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				short result = (short)(((ushort)(bits >> 221) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (short)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="short"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="short"/>.</exception>
		public static explicit operator checked short(Octo value)
		{
			Octo twoPow15 = new Octo(0x4000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow15 || Octo.IsNaN(value) || value >= +twoPow15)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				short result = (short)(((ushort)(bits >> 221) | 0x8000) >>> (Octo.ExponentBias + 16 - 1 - (int)(bits >> 236)));

				if (isNegative)
				{
					result = (short)-result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator int(Octo value)
		{
			Octo twoPow31 = new Octo(0x4001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31)
			{
				return int.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow31)
			{
				return int.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				int result = (int)((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="int"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="int"/>.</exception>
		public static explicit operator checked int(Octo value)
		{
			Octo twoPow31 = new Octo(0x4001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow31 || Octo.IsNaN(value) || value >= +twoPow31)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				int result = (int)((uint)(bits >> 205) | 0x8000_0000);

				result >>>= (Octo.ExponentBias + 32 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator long(Octo value)
		{
			Octo twoPow63 = new Octo(0x4003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63)
			{
				return long.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return 0;
			}
			else if (value >= +twoPow63)
			{
				return long.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				long result = (long)((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="long"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
		public static explicit operator checked long(Octo value)
		{
			Octo twoPow63 = new Octo(0x4003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow63 || Octo.IsNaN(value) || value >= +twoPow63)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				long result = (long)((ulong)(bits >> 173) | 0x8000_0000_0000_0000);

				result >>>= (Octo.ExponentBias + 64 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int128(Octo value)
		{
			Octo twoPow127 = new Octo(0x4007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127)
			{
				return Int128.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int128.Zero;
			}
			else if (value >= +twoPow127)
			{
				return Int128.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int128.Zero;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int128"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int128"/>.</exception>
		public static explicit operator checked Int128(Octo value)
		{
			Octo twoPow127 = new Octo(0x4007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow127 || Octo.IsNaN(value) || value >= +twoPow127)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int128 result = (Int128)((UInt128)(bits >> 109) | new UInt128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));

				result >>>= (Octo.ExponentBias + 128 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int128.Zero;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int256(Octo value)
		{
			Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255)
			{
				return Int256.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int256.Zero;
			}
			else if (value >= +twoPow255)
			{
				return Int256.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int256.Zero;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int256"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int256"/>.</exception>
		public static explicit operator checked Int256(Octo value)
		{
			Octo twoPow255 = new Octo(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow255 || Octo.IsNaN(value) || value >= +twoPow255)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int256 result = (Int256)((bits << 20) >> 1 | Octo.SignMask);

				result >>>= (Octo.ExponentBias + 256 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int256.Zero;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Int512(Octo value)
		{
			Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511)
			{
				return Int512.MinValue;
			}
			else if (Octo.IsNaN(value))
			{
				return Int512.Zero;
			}
			else if (value >= +twoPow511)
			{
				return Int512.MaxValue;
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				// In order to convert from Quad to Int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int512.Zero;
			}
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Int512"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <see cref="Int512"/>.</exception>
		public static explicit operator checked Int512(Octo value)
		{
			Octo twoPow511 = new Octo(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

			if (value <= -twoPow511 || Octo.IsNaN(value) || value >= +twoPow511)
			{
				Thrower.IntegerOverflow();
			}

			bool isNegative = Octo.IsNegative(value);

			if (isNegative)
			{
				value = -value;
			}

			if (value >= Octo.One)
			{
				// In order to convert from Quad to Int512 we first need to extract the signficand,
				// including the implicit leading bit, as a full 512-bit significand. We can then adjust
				// this down to the represented integer by y shifting by the unbiased exponent, taking
				// into account the significand is now represented as 512-bits.

				UInt256 bits = Octo.OctoToUInt256Bits(value);
				Int512 result = new Int512((bits << 20) >> 1 | Octo.SignMask, UInt256.Zero);

				result >>>= (Octo.ExponentBias + 512 - 1 - (int)(bits >> 236));

				if (isNegative)
				{
					result = -result;
				}

				return result;
			}
			else
			{
				return Int512.Zero;
			}
		}
		// Floating
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="decimal"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator decimal(Octo value)
		{
			return (decimal)(double)value;
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Quad"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Quad(Octo value)
		{
			UInt256 octoInt = OctoToUInt256Bits(value);
			bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
			int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
			UInt256 sig = octoInt & Octo.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return BitHelper.CreateQuadNaN(sign, (UInt128)(sig >> 108)); // Shift the significand bits to the x end
				}
				return sign ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			sig <<= 18;
			UInt128 sigOcto = (sig.Upper) | ((UInt128)sig != UInt128.Zero ? UInt128.One : UInt128.Zero);

			if (((uint)exp | sigOcto) == UInt128.Zero)
			{
				return new Quad(sign, 0, UInt128.Zero);
			}

			exp -= 0x3_C001;

			exp = exp < -0x1_0000 ? -0x1_0000 : exp;

			return Quad.UInt128BitsToQuad(BitHelper.RoundPackToQuad(sign, exp, sigOcto | new UInt128(0x4000_0000_0000_0000, 0x0000_0000_0000_0000), 0));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="double"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator double(Octo value)
		{
			UInt256 octoInt = OctoToUInt256Bits(value);
			bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
			int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
			UInt256 sig = octoInt & Octo.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return BitHelper.CreateDoubleNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
				}
				return sign ? double.NegativeInfinity : double.PositiveInfinity;
			}

			sig <<= 18;
			ulong sigOcto = sig.Part3 | (sig.Part2 != 0 && sig.Part1 != 0 && sig.Part0 != 0 ? 1UL : 0UL);

			if (((uint)exp | sigOcto) == 0)
			{
				return BitHelper.CreateDouble(sign, 0, 0);
			}

			exp -= 0x3_EFDD;

			exp = exp < -0x1_0000 ? -0x1_0000 : exp;

			return BitConverter.UInt64BitsToDouble(BitHelper.RoundPackToDouble(sign, (short)(exp), (sigOcto | 0x4000_0000_0000_0000)));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="float"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator float(Octo value)
		{
			UInt256 octoInt = OctoToUInt256Bits(value);
			bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
			int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
			UInt256 sig = octoInt & Octo.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return BitHelper.CreateSingleNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
				}
				return sign ? float.NegativeInfinity : float.PositiveInfinity;
			}

			sig <<= 18;
			uint sigOcto = (uint)(sig.Part3 >> 64) | ((uint)sig.Part3 != 0 && sig.Part2 != 0 && sig.Part1 != 0 && sig.Part0 != 0 ? 1U : 0U);

			if (((uint)exp | sigOcto) == 0)
			{
				return BitHelper.CreateSingle(sign, 0, 0);
			}

			exp -= 0x3_FF81;

			exp = exp < -0x1_0000 ? -0x1_0000 : exp;

			return BitConverter.UInt32BitsToSingle(BitHelper.RoundPackToSingle(sign, (short)(exp), (sigOcto | 0x4000_0000)));
		}
		/// <summary>
		/// Explicitly converts a <see cref="Octo" /> value to a <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static explicit operator Half(Octo value)
		{
			UInt256 octoInt = OctoToUInt256Bits(value);
			bool sign = (octoInt & Octo.SignMask) >> Octo.SignShift != UInt256.Zero;
			int exp = (int)((octoInt & Octo.BiasedExponentMask) >> Octo.BiasedExponentShift);
			UInt256 sig = octoInt & Octo.TrailingSignificandMask;

			if (exp == MaxBiasedExponent)
			{
				if (sig != 0) // NaN
				{
					return BitHelper.CreateHalfNaN(sign, (ulong)(sig >> 172)); // Shift the significand bits to the x end
				}
				return sign ? Half.NegativeInfinity : Half.PositiveInfinity;
			}

			sig <<= 18;
			ushort sigOcto = (ushort)((sig.Part3 >> (64 + 32)) | ((sig.Part3 & 0x0000_FFFF_FFFF_FFFF) != 0 && sig.Part2 != 0 && sig.Part1 != 0 && sig.Part0 != 0 ? 1U : 0U));

			if (((uint)exp | sigOcto) == 0)
			{
				return BitHelper.CreateHalf(sign, 0, 0);
			}

			exp -= 0x3_FFEB;

			exp = exp < -0x1_0000 ? -0x1_0000 : exp;

			return BitConverter.UInt16BitsToHalf(BitHelper.RoundPackToHalf(sign, (short)(exp), (ushort)(sigOcto | 0x4000)));
		}
		#endregion
		#region To Octo
		// Unsigned
		/// <summary>
		/// Implicitly converts a <see cref="byte" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(byte value)
		{
			return (Octo)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="ushort" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(ushort value)
		{
			return (Octo)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="uint" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(uint value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 205;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (uint)(0x400EB - shiftDist), sig);
		}
		/// <summary>
		/// Implicitly converts a <see cref="ulong" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(ulong value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = BitOperations.LeadingZeroCount(value) + 173;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (uint)(0x400EB - shiftDist), sig);
		}
		/// <summary>
		/// Implicitly converts a <see cref="UInt128" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(UInt128 value)
		{
			UInt256 sig;
			int shiftDist;

			if (value == UInt128.Zero)
			{
				return Octo.Zero;
			}
			else
			{
				shiftDist = ((int)UInt128.LeadingZeroCount(value)) + 109;
				sig = (UInt256)value << shiftDist;
			}

			return new Octo(false, (uint)(0x400EB - shiftDist), sig);
		}
		// Signed
		/// <summary>
		/// Implicitly converts a <see cref="sbyte" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(sbyte value)
		{
			if (sbyte.IsNegative(value))
			{
				value = (sbyte)-value;
				return -(Octo)(byte)value;
			}
			return (Octo)(byte)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="short" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(short value)
		{
			if (short.IsNegative(value))
			{
				value = (short)-value;
				return -(Octo)(ushort)value;
			}
			return (Octo)(ushort)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="int" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(int value)
		{
			if (int.IsNegative(value))
			{
				value = -value;
				return -(Octo)(uint)value;
			}
			return (Octo)(uint)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="long" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(long value)
		{
			if (long.IsNegative(value))
			{
				value = -value;
				return -(Octo)(ulong)value;
			}
			return (Octo)(ulong)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="Int128" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(Int128 value)
		{
			if (Int128.IsNegative(value))
			{
				value = -value;
				return -(Octo)(UInt128)value;
			}
			return (Octo)(UInt128)value;
		}
		// Floating
		/// <summary>
		/// Implicitly converts a <see cref="decimal" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(decimal value)
		{
			return (Octo)(double)value;
		}
		/// <summary>
		/// Implicitly converts a <see cref="double" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(double value)
		{
			const int MaxBiasedExponentDouble = 0x07FF;
			const int DoubleExponentBias = 1023;

			ulong bits = BitConverter.DoubleToUInt64Bits(value);
			bool sign = double.IsNegative(value);
			int exp = (ushort)((bits >> 52) & MaxBiasedExponentDouble);
			ulong sig = bits & 0x000F_FFFF_FFFF_FFFF;

			if (exp == MaxBiasedExponentDouble)
			{
				if (sig != 0)
				{
					return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 204);
				}
				return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return Octo.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF64Sig(sig);
				exp -= 1;
			}

			return new Octo(sign, (uint)(exp + (ExponentBias - DoubleExponentBias)), (UInt256)sig << 184);
		}
		/// <summary>
		/// Implicitly converts a <see cref="float" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(float value)
		{
			const int MaxBiasedExponentSingle = 0xFF;
			const int SingleExponentBias = 127;

			uint bits = BitConverter.SingleToUInt32Bits(value);
			bool sign = float.IsNegative(value);
			int exp = (ushort)((bits >> 23) & MaxBiasedExponentSingle);
			uint sig = bits & 0x007F_FFFF;

			if (exp == MaxBiasedExponentSingle)
			{
				if (sig != 0)
				{
					return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 233);
				}
				return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return Octo.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF32Sig(sig);
				exp -= 1;
			}

			return new Octo(sign, (uint)(exp + (ExponentBias - SingleExponentBias)), (UInt256)sig << 213);
		}
		/// <summary>
		/// Implicitly converts a <see cref="Half" /> value to a <see cref="Octo"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		public static implicit operator Octo(Half value)
		{
			const int MaxBiasedExponentHalf = 0x1F;
			const int HalfExponentBias = 15;

			ushort bits = BitConverter.HalfToUInt16Bits(value);
			bool sign = Half.IsNegative(value);
			int exp = (ushort)((bits >> 10) & MaxBiasedExponentHalf);
			ushort sig = (ushort)(bits & 0x03FF);

			if (exp == MaxBiasedExponentHalf)
			{
				if (sig != 0)
				{
					return BitHelper.CreateOctoNaN(sign, (UInt256)sig << 246);
				}
				return sign ? Octo.NegativeInfinity : Octo.PositiveInfinity;
			}

			if (exp == 0)
			{
				if (sig == 0)
				{
					return Octo.UInt256BitsToOcto(sign ? Octo.SignMask : UInt256.Zero);
				}
				(exp, sig) = BitHelper.NormalizeSubnormalF16Sig(sig);
				exp -= 1;
			}

			return new Octo(sign, (uint)(exp + (ExponentBias - HalfExponentBias)), (UInt256)sig << 226);
		}
		#endregion
	}
}
