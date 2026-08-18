namespace MissingValues.Tests.Extensions;

public static class UInt256Extensions
{
	extension(UInt256)
	{
		public static UInt256 SByteMaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_007F);
		public static UInt256 ByteMaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_00FF);
		public static UInt256 Int16MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_7FFF);
		public static UInt256 UInt16MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_0000_FFFF);
		public static UInt256 Int32MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF);
		public static UInt256 UInt32MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x0000_0000_FFFF_FFFF);
		public static UInt256 Int64MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF);
		public static UInt256 UInt64MaxValue => new(0, 0, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF);
		public static UInt256 IntPtrMaxValue => nint.Size == 8 ? UInt256.Int64MaxValue : UInt256.Int32MaxValue;
		public static UInt256 UIntPtrMaxValue => nuint.Size == 8 ? UInt256.UInt64MaxValue : UInt256.UInt32MaxValue;
		public static UInt256 Int128MaxValue => new(0, 0, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		public static UInt256 UInt128MaxValue => new(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
		public static UInt256 Int256MaxValue => new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);

		public static UInt256 Two => new(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0002);
		public static UInt256 MaxValueMinusOne => new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE);
		public static UInt256 MaxValueMinusTwo => new(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFD);
		public static UInt256 HalfMaxValue => new(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF);
	}
}