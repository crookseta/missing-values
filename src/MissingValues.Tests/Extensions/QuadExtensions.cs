namespace MissingValues.Tests.Extensions;

public static class QuadExtensions
{
	extension(Quad)
	{
		public static Quad NegativeThousand => Values.CreateFloat<Quad>(0xC008_F400_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeHundred => Values.CreateFloat<Quad>(0xC005_9000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeTen => Values.CreateFloat<Quad>(0xC002_4000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeSix => Values.CreateFloat<Quad>(0xC001_8000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeFive => Values.CreateFloat<Quad>(0xC001_4000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeFour => Values.CreateFloat<Quad>(0xC001_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeThree => Values.CreateFloat<Quad>(0xC000_8000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeTwo => Values.CreateFloat<Quad>(0xC000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeHalf => Values.CreateFloat<Quad>(0xBFFE_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad NegativeQuarter => Values.CreateFloat<Quad>(0xBFFD_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Quarter => Values.CreateFloat<Quad>(0x3FFD_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Half => Values.CreateFloat<Quad>(0x3FFE_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Two => Values.CreateFloat<Quad>(0x4000_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Three => Values.CreateFloat<Quad>(0x4000_8000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Four => Values.CreateFloat<Quad>(0x4001_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Five => Values.CreateFloat<Quad>(0x4001_4000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Six => Values.CreateFloat<Quad>(0x4001_8000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Ten => Values.CreateFloat<Quad>(0x4002_4000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Hundred => Values.CreateFloat<Quad>(0x4005_9000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Thousand => Values.CreateFloat<Quad>(0x4008_F400_0000_0000, 0x0000_0000_0000_0000);

		public static Quad GreaterThanOneSmallest => Values.CreateFloat<Quad>(0x3FFF_0000_0000_0000, 0x0000_0000_0000_0001);
		public static Quad LessThanOneLargest => Values.CreateFloat<Quad>(0x3FFE_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static Quad SmallestSubnormal => Values.CreateFloat<Quad>(0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
		public static Quad GreatestSubnormal => Values.CreateFloat<Quad>(0x0000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static Quad ByteMaxValue => Values.CreateFloat<Quad>(0x4006_FE00_0000_0000, 0x0000_0000_0000_0000);
		public static Quad UInt16MaxValue => Values.CreateFloat<Quad>(0x400E_FFFE_0000_0000, 0x0000_0000_0000_0000);
		public static Quad UInt32MaxValue => Values.CreateFloat<Quad>(0x401E_FFFF_FFFE_0000, 0x0000_0000_0000_0000);
		public static Quad UInt64MaxValue => Values.CreateFloat<Quad>(0x403E_FFFF_FFFF_FFFF, 0xFFFE_0000_0000_0000);
		public static Quad TwoOver128 => Values.CreateFloat<Quad>(0x407F_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad TwoOver256 => Values.CreateFloat<Quad>(0x40FF_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad TwoOver512 => Values.CreateFloat<Quad>(0x41FF_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad TwoOver127 => Values.CreateFloat<Quad>(0x407E_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad TwoOver255 => Values.CreateFloat<Quad>(0x40FE_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad TwoOver511 => Values.CreateFloat<Quad>(0x41FE_0000_0000_0000, 0x0000_0000_0000_0000);

		public static Quad SByteMaxValue => Values.CreateFloat<Quad>(0x4005_FC00_0000_0000, 0x0000_0000_0000_0000);
		public static Quad SByteMinValue => Values.CreateFloat<Quad>(0xC006_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Int16MaxValue => Values.CreateFloat<Quad>(0x400D_FFFC_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Int16MinValue => Values.CreateFloat<Quad>(0xC00E_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Int32MaxValue => Values.CreateFloat<Quad>(0x401D_FFFF_FFFC_0000, 0x0000_0000_0000_0000);
		public static Quad Int32MinValue => Values.CreateFloat<Quad>(0xC01E_0000_0000_0000, 0x0000_0000_0000_0000);
		public static Quad Int64MaxValue => Values.CreateFloat<Quad>(0x403D_FFFF_FFFF_FFFF, 0xFFFC_0000_0000_0000);
		public static Quad Int64MinValue => Values.CreateFloat<Quad>(0xC03E_0000_0000_0000, 0x0000_0000_0000_0000);

		public static Quad Delta => Values.CreateFloat<Quad>(0x406F_0000_0000_0000, 0x0000_0000_0000_0000);
	}
}