using MissingValues.Info;
using MissingValues.Internals;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MissingValues.Primitives;

namespace MissingValues;

/// <summary>
/// Represents an octuple-precision floating-point number.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(NumberConverter.OctoConverter))]
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[DebuggerTypeProxy(typeof(FloatDebugView<Octo>))]
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
	public static readonly Octo NaN = new Octo(0xFFFF_F800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
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
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(this);
			return ExtractBiasedExponentFromBits(in bits);
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
			UInt256 bits = BinaryOperations.OctoToUInt256Bits(this);
			return ExtractTrailingSignificandFromBits(in bits);
		}
	}

#if BIGENDIAN
		internal readonly ulong _bits3;
		internal readonly ulong _bits2;
		internal readonly ulong _bits1;
		internal readonly ulong _bits0;
#else
	internal readonly ulong _bits0;
	internal readonly ulong _bits1;
	internal readonly ulong _bits2;
	internal readonly ulong _bits3;
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
		var bits = (sig & TrailingSignificandMask) | new UInt256((sign ? 0x8000_0000_0000_0000 : 0) | ((ulong)(exp & 0x7FFFF) << 44), 0, 0, 0);
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
		if (IsNaNOrZero(this))
		{
			// All NaNs should have the same hash code, as should both Zeros.
			return HashCode.Combine(BinaryOperations.OctoToUInt256Bits(this) & PositiveInfinityBits);
		}
		return HashCode.Combine(_bits0, _bits1, _bits2, _bits3);
	}

	/// <inheritdoc/>
	public override string ToString()
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
	/// Reinterprets the specified 256-bit unsigned integer to an octuple-precision floating point number.
	/// </summary>
	/// <param name="bits">The number to convert.</param>
	/// <returns>An octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Octo UInt256BitsToOcto(UInt256 bits) => BinaryOperations.UInt256BitsToOcto(bits);
	/// <summary>
	/// Reinterprets the specified 256-bit signed integer to an octuple-precision floating point number.
	/// </summary>
	/// <param name="bits">The number to convert.</param>
	/// <returns>An octuple-precision floating point number whose bits are identical to <paramref name="bits"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Octo Int256BitsToOcto(Int256 bits) => BinaryOperations.Int256BitsToOcto(bits);

	/// <summary>
	/// Converts the specified octuple-precision floating point number to a 256-bit unsigned integer.
	/// </summary>
	/// <param name="value">The number to convert.</param>
	/// <returns>A 256-bit unsigned integer whose value is equivalent to <paramref name="value"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static UInt256 OctoToUInt256Bits(Octo value) => BinaryOperations.OctoToUInt256Bits(value);
	/// <summary>
	/// Converts the specified octuple-precision floating point number to a 256-bit signed integer.
	/// </summary>
	/// <param name="value">The number to convert.</param>
	/// <returns>A 256-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Int256 OctoToInt256Bits(Octo value) => BinaryOperations.OctoToInt256Bits(value);


	internal static bool AreZero(in Octo x, in Octo y)
	{
		return ((BinaryOperations.OctoToUInt256Bits(x) | BinaryOperations.OctoToUInt256Bits(y)) & ~SignMask) == UInt256.Zero;
	}
	internal static bool IsNaNOrZero(Octo value)
	{
		return ((BinaryOperations.OctoToUInt256Bits(value) - UInt256.One) & ~SignMask) >= PositiveInfinityBits;
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
		return BinaryOperations.OctoToUInt256Bits(value) & ~SignMask;
	}
}