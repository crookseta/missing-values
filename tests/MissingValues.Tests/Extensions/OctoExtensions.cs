namespace MissingValues.Tests.Extensions;

public static class OctoExtensions
{
	extension(Octo)
	{
		public static Octo NegativeThousand => Values.CreateFloat<Octo>(0xC000_8F40_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeHundred => Values.CreateFloat<Octo>(0xC000_5900_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeTen => Values.CreateFloat<Octo>(0xC000_2400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeFive => Values.CreateFloat<Octo>(0xC000_1400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeFour => Values.CreateFloat<Octo>(0xC000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeThree => Values.CreateFloat<Octo>(0xC000_0800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeTwo => Values.CreateFloat<Octo>(0xC000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeHalf => Values.CreateFloat<Octo>(0xBFFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo NegativeQuarter => Values.CreateFloat<Octo>(0xBFFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Quarter => Values.CreateFloat<Octo>(0x3FFF_D000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Half => Values.CreateFloat<Octo>(0x3FFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Two => Values.CreateFloat<Octo>(0x4000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Three => Values.CreateFloat<Octo>(0x4000_0800_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Four => Values.CreateFloat<Octo>(0x4000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Five => Values.CreateFloat<Octo>(0x4000_1400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Ten => Values.CreateFloat<Octo>(0x4000_2400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Hundred => Values.CreateFloat<Octo>(0x4000_5900_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Thousand => Values.CreateFloat<Octo>(0x4000_8F40_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static Octo GreaterThanOneSmallest => Values.CreateFloat<Octo>(0x3FFF_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static Octo LessThanOneLargest => Values.CreateFloat<Octo>(0x3FFF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static Octo SmallestSubnormal => Values.CreateFloat<Octo>(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static Octo GreatestSubnormal => Values.CreateFloat<Octo>(0x0000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static Octo ByteMaxValue => Values.CreateFloat<Octo>(0x4000_6FE0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo UInt16MaxValue => Values.CreateFloat<Octo>(0x4000_EFFF_E000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo UInt32MaxValue => Values.CreateFloat<Octo>(0x4001_EFFF_FFFF_E000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo UInt64MaxValue => Values.CreateFloat<Octo>(0x4003_EFFF_FFFF_FFFF, 0xFFFF_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo UInt128MaxValue => Values.CreateFloat<Octo>(0x4007_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_E000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo TwoOver255 => Values.CreateFloat<Octo>(0x400F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo TwoOver256 => Values.CreateFloat<Octo>(0x400F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo TwoOver511 => Values.CreateFloat<Octo>(0x401F_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo TwoOver512 => Values.CreateFloat<Octo>(0x401F_F000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static Octo SByteMaxValue => Values.CreateFloat<Octo>(0x4000_5FC0_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo SByteMinValue => Values.CreateFloat<Octo>(0xC000_6000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int16MaxValue => Values.CreateFloat<Octo>(0x4000_DFFF_C000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int16MinValue => Values.CreateFloat<Octo>(0xC000_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int32MaxValue => Values.CreateFloat<Octo>(0x4001_DFFF_FFFF_C000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int32MinValue => Values.CreateFloat<Octo>(0xC001_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int64MaxValue => Values.CreateFloat<Octo>(0x4003_DFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int64MinValue => Values.CreateFloat<Octo>(0xC003_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int128MaxValue => Values.CreateFloat<Octo>(0x4007_DFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_C000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo Int128MinValue => Values.CreateFloat<Octo>(0xC007_E000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		
		public static Octo HalfMaxValue => Values.CreateFloat<Octo>(0x4000_EFFC_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo HalfMinValue => Values.CreateFloat<Octo>(0xC000_EFFC_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo SingleMaxValue => Values.CreateFloat<Octo>(0x4007_EFFF_FFE0_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo SingleMinValue => Values.CreateFloat<Octo>(0xC007_EFFF_FFE0_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo DoubleMaxValue => Values.CreateFloat<Octo>(0x403F_EFFF_FFFF_FFFF, 0xFF00000000000000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo DoubleMinValue => Values.CreateFloat<Octo>(0xC03F_EFFF_FFFF_FFFF, 0xFF00000000000000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo QuadMaxValue => Values.CreateFloat<Octo>(0x43FF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xF000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Octo QuadMinValue => Values.CreateFloat<Octo>(0xC3FF_EFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xF000_0000_0000_0000, 0x0000_0000_0000_0000);

		public static Octo Delta => Values.CreateFloat<Octo>(0x400E_B000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000);
	}
}