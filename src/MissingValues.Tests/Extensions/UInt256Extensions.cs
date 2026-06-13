namespace MissingValues.Tests.Extensions;

public static class UInt256Extensions
{
	extension(UInt256)
	{
		private static UInt256 ByteMaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_00FF);
		private static UInt256 UInt16MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_FFFF);
		private static UInt256 UInt32MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_FFFF_FFFF);
		private static UInt256 UInt64MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF);
		private static UInt256 UInt128MaxValue => new(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		private static UInt256 Two => new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0002);
		private static UInt256 MaxValueMinusOne => new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE);
		private static UInt256 MaxValueMinusTwo => new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFD);
		private static UInt256 HalfMaxValue => new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	}
}