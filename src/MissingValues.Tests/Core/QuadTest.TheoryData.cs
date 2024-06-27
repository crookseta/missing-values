using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Xml;

namespace MissingValues.Tests.Core
{
	public partial class QuadTest
	{
#pragma warning disable S3263 // Static fields should appear in the order they must be initialized 
		private static (string, bool, Quad)[] _tryParseData =
		{
			("2,0", true, Two),
			("-2", true, NegativeTwo),
			("0", true, Zero),
			(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, true, Quad.PositiveInfinity),
			(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, true, Quad.NegativeInfinity),
			(NumberFormatInfo.CurrentInfo.NaNSymbol, true, Quad.NaN),
			("256,4995", true, Values.CreateFloat<Quad>(0x4007_007F_DF3B_645A, 0x1CAC_0831_26E9_78D5)),
			("471581881", true, Values.CreateFloat<Quad>(0x401B_C1BC_4B90_0000, 0x0000_0000_0000_0000)),
			("1,93561113", true, Values.CreateFloat<Quad>(0x3FFF_EF84_3605_1FA4, 0x8B0F_3D34_BECE_8762)),
			("9715574,2", true, Values.CreateFloat<Quad>(0x4016_287E_EC66_6666, 0x6666_6666_6666_6666)),
			("0,51438427732005011792", true, Values.CreateFloat<Quad>(0x3FFE_075D_6041_5519, 0x72D0_0AD3_7DB4_57E9)),
			("0,04201133209656899095", true, Values.CreateFloat<Quad>(0x3FFA_5828_262D_512C, 0x8840_B3B3_D424_5947)),
			("7,7E777", true, Values.CreateFloat<Quad>(0x4A17_0F28_5D1D_4C84, 0xA11F_6899_101B_A9A4)),
			("-7,7E-777", true, Values.CreateFloat<Quad>(0xB5EC_BFCE_3AF6_4E08, 0x42C8_5750_BEBD_A572)),
			("1A", false, default),
		};

		private static (Quad, Quad)[] _unaryNegationOperationData =
		{
			(Zero, NegativeZero),
			(One, NegativeOne),
			(Two, NegativeTwo),
			(Ten, NegativeTen),
			(Hundred, NegativeHundred),
			(Thousand, NegativeThousand)
		};
		private static (Quad, Quad)[] _incrementOperationData =
		{
			(NegativeTwo, NegativeOne),
			(NegativeOne, Zero),
			(Zero, One),
			(One, Two),
		};
		private static (Quad, Quad)[] _decrementOperationData =
		{
			(NegativeOne, NegativeTwo),
			(Zero, NegativeOne),
			(One, Zero),
			(Two, One),
		};

		private static (Quad, Quad, Quad)[] _additionOperationData =
		{
			(One, One, Two),
			(One, NegativeOne, Zero),
			(One, NegativeTwo, NegativeOne),
			(One, Four, Five),
			(Three, Two, Five),
			(SmallestSubnormal, GreatestSubnormal, Values.CreateFloat<Quad>(0x0001_0000_0000_0000, 0x0000_0000_0000_0000)),
			(Quad.PositiveInfinity, Quad.One, Quad.PositiveInfinity),
			(Quad.NegativeInfinity, Quad.One, Quad.NegativeInfinity),
			(Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.PositiveInfinity),
			(Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NegativeInfinity),
		};
		private static (Quad, Quad, Quad)[] _subtractionOperationData =
		{
			(One, One, Zero),
			(One, NegativeOne, Two),
			(One, Two, NegativeOne),
			(SmallestSubnormal, GreatestSubnormal, Values.CreateFloat<Quad>(0x8000_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)),
			(Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.NaN),
			(Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NaN),
		};
		private static (Quad, Quad, Quad)[] _multiplicationOperationData =
		{
			(One, One, One),
			(One, NegativeOne, NegativeOne),
			(Ten, Ten, Hundred),
			(NegativeHundred, Ten, NegativeThousand),
			(NegativeTen, Hundred, NegativeThousand),
			(Zero, NegativeThousand, NegativeZero),
			(Zero, Quad.PositiveInfinity, Quad.NaN),
			(NegativeZero, Quad.NegativeInfinity, Quad.NaN),
			(Quad.PositiveInfinity, Zero, Quad.NaN),
			(Quad.NegativeInfinity, NegativeZero, Quad.NaN),
		};
		private static (Quad, Quad, Quad)[] _divisionOperationData =
		{
			(Ten, Ten, One),
			(Hundred, Ten, Ten),
			(NegativeThousand, Ten, NegativeHundred),
			(Zero, Zero, Quad.NaN),
			(One, Zero, Quad.PositiveInfinity),
			(NegativeOne, Zero, Quad.NegativeInfinity),
			(Quad.PositiveInfinity, Quad.PositiveInfinity, Quad.NaN),
			(Quad.NegativeInfinity, Quad.NegativeInfinity, Quad.NaN),
		};

		private static (byte, Quad)[] _castFromByteData = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(byte.MaxValue, ByteMaxValue),
		};
		private static (ushort, Quad)[] _castFromUInt16Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
		};
		private static (uint, Quad)[] _castFromUInt32Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
		};
		private static (ulong, Quad)[] _castFromUInt64Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
			(ulong.MaxValue, UInt64MaxValue),
		};
		private static (UInt128, Quad)[] _castFromUInt128Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
			(ulong.MaxValue, UInt64MaxValue),
		};
		private static (UInt256, Quad)[] _castFromUInt256Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
			(ulong.MaxValue, UInt64MaxValue),
		};
		private static (UInt512, Quad)[] _castFromUInt512Data = 
		{
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
			(ulong.MaxValue, UInt64MaxValue),
		};
		private static (sbyte, Quad)[] _castFromSByteData = 
		{
			(sbyte.MinValue, SByteMinValue),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(sbyte.MaxValue, SByteMaxValue),
		};
		private static (short, Quad)[] _castFromInt16Data = 
		{
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
		};
		private static (int, Quad)[] _castFromInt32Data = 
		{
			(int.MinValue, Int32MinValue),
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
			(int.MaxValue, Int32MaxValue),
		};
		private static (long, Quad)[] _castFromInt64Data = 
		{
			(long.MinValue, Int64MinValue),
			(int.MinValue, Int32MinValue),
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
			(int.MaxValue, Int32MaxValue),
			(long.MaxValue, Int64MaxValue),
		};
		private static (Int128, Quad)[] _castFromInt128Data = 
		{
			(long.MinValue, Int64MinValue),
			(int.MinValue, Int32MinValue),
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
			(int.MaxValue, Int32MaxValue),
			(long.MaxValue, Int64MaxValue),
		};
		private static (Int256, Quad)[] _castFromInt256Data = 
		{
			(long.MinValue, Int64MinValue),
			(int.MinValue, Int32MinValue),
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
			(int.MaxValue, Int32MaxValue),
			(long.MaxValue, Int64MaxValue),
		};
		private static (Int512, Quad)[] _castFromInt512Data = 
		{
			(long.MinValue, Int64MinValue),
			(int.MinValue, Int32MinValue),
			(short.MinValue, Int16MinValue),
			(sbyte.MinValue, SByteMinValue),
			(-1000, NegativeThousand),
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(sbyte.MaxValue, SByteMaxValue),
			(short.MaxValue, Int16MaxValue),
			(int.MaxValue, Int32MaxValue),
			(long.MaxValue, Int64MaxValue),
		};
		private static (Half, Quad)[] _castFromHalfData = 
		{
			(System.Half.NegativeOne, NegativeOne),
			(System.Half.NegativeZero, NegativeZero),
			(System.Half.Zero, Zero),
			(System.Half.One, One),
			((Half)10, Ten),
			((Half)100, Hundred),
			((Half)1000, Thousand),
		};
		private static (float, Quad)[] _castFromSingleData = 
		{
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
		};
		private static (double, Quad)[] _castFromDoubleData = 
		{
			(-100, NegativeHundred),
			(-10, NegativeTen),
			(-2, NegativeTwo),
			(-1, NegativeOne),
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
		};
		
		private static (Quad, byte)[] _castToByteData = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(ByteMaxValue, byte.MaxValue),
		};
		private static (Quad, ushort)[] _castToUInt16Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
		};
		private static (Quad, uint)[] _castToUInt32Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
		};
		private static (Quad, ulong)[] _castToUInt64Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
			(UInt64MaxValue, ulong.MaxValue),
		};
		private static (Quad, UInt128)[] _castToUInt128Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
			(UInt64MaxValue, ulong.MaxValue),
		};
		private static (Quad, UInt256)[] _castToUInt256Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
			(UInt64MaxValue, ulong.MaxValue),
		};
		private static (Quad, UInt512)[] _castToUInt512Data = 
		{
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
			(UInt64MaxValue, ulong.MaxValue),
		};
		private static (Quad, sbyte)[] _castToSByteData = 
		{
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
		};
		private static (Quad, short)[] _castToInt16Data = 
		{
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
		};
		private static (Quad, int)[] _castToInt32Data = 
		{
			(Int32MinValue, int.MinValue),
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
			(Int32MaxValue, int.MaxValue),
		};
		private static (Quad, long)[] _castToInt64Data = 
		{
			(Int64MinValue, long.MinValue),
			(Int32MinValue, int.MinValue),
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
			(Int32MaxValue, int.MaxValue),
			(Int64MaxValue, long.MaxValue),
		};
		private static (Quad, Int128)[] _castToInt128Data = 
		{
			(Int64MinValue, long.MinValue),
			(Int32MinValue, int.MinValue),
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
			(Int32MaxValue, int.MaxValue),
			(Int64MaxValue, long.MaxValue),
		};
		private static (Quad, Int256)[] _castToInt256Data = 
		{
			(Int64MinValue, long.MinValue),
			(Int32MinValue, int.MinValue),
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
			(Int32MaxValue, int.MaxValue),
			(Int64MaxValue, long.MaxValue),
		};
		private static (Quad, Int512)[] _castToInt512Data = 
		{
			(Int64MinValue, long.MinValue),
			(Int32MinValue, int.MinValue),
			(Int16MinValue, short.MinValue),
			(SByteMinValue, sbyte.MinValue),
			(NegativeHundred, -100),
			(NegativeTen, -10),
			(NegativeTwo, -2),
			(NegativeOne, -1),
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(SByteMaxValue, sbyte.MaxValue),
			(Int16MaxValue, short.MaxValue),
			(Int32MaxValue, int.MaxValue),
			(Int64MaxValue, long.MaxValue),
		};
		private static (Quad, Half)[] _castToHalfData = 
		{
			(NegativeHundred, -(Half)100.0f),
			(NegativeTen, -(Half)10.0f),
			(NegativeTwo, -(Half)2.0f),
			(NegativeOne, -(Half)1.0f),
			(NegativeZero, -(Half)0.0f),
			(Zero, (Half)0.0f),
			(One, System.Half.One),
			(Two, (Half)2.0f),
			(Ten, (Half)10.0f),
			(Hundred, (Half)100.0f),
		};
		private static (Quad, float)[] _castToSingleData = 
		{
			(NegativeThousand, -1000.0f),
			(NegativeHundred, -100.0f),
			(NegativeTen, -10.0f),
			(NegativeTwo, -2.0f),
			(NegativeOne, -1.0f),
			(NegativeZero, -0.0f),
			(Zero, 0.0f),
			(Half, 0.5f),
			(One, 1.0f),
			(Two, 2.0f),
			(Ten, 10.0f),
			(Hundred, 100.0f),
			(Thousand, 1000.0f),
		};
		private static (Quad, double)[] _castToDoubleData = 
		{
			(NegativeThousand, -1000.0d),
			(NegativeHundred, -100.0d),
			(NegativeTen, -10.0d),
			(NegativeTwo, -2.0d),
			(NegativeOne, -1.0d),
			(NegativeZero, -0.0d),
			(Zero, 0.0d),
			(Half, 0.5d),
			(One, 1.0d),
			(Two, 2.0d),
			(Ten, 10.0d),
			(Hundred, 100.0d),
			(Thousand, 1000.0d),
		};

		private static (Quad, int, MidpointRounding, Quad)[] _roundAwayFromZeroData =
		{
			(Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Four),
			(Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Three),
			(Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Three),
			(Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Two),
			(Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, NegativeFour),
		};
		private static (Quad, int, MidpointRounding, Quad)[] _roundToEvenData =
		{
			(Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Four),
			(Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Three),
			(Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Two),
			(Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Two),
			(Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, NegativeFour),
		};
		private static (Quad, int, MidpointRounding, Quad)[] _roundToNegativeInfinityData =
		{
			(Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Three),
			(Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, NegativeFour),
		};
		private static (Quad, int, MidpointRounding, Quad)[] _roundToPositiveInfinityData =
		{
			(Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Four),
			(Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, NegativeThree),
		};
		private static (Quad, int, MidpointRounding, Quad)[] _roundToZeroData =
		{
			(Values.CreateFloat<Quad>(0x4000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Three),
			(Values.CreateFloat<Quad>(0x4000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Quad>(0x4000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Quad>(0x4000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Quad>(0xC000_0CCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_4000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Quad>(0xC000_C000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, NegativeThree),
		};

		private static (Quad, Quad, bool)[] _greaterThanData =
		{
			(Two, One, true),
			(Thousand, NegativeThousand, true),
			(NegativeQuarter, NegativeHalf, true),
			(Quarter, Half, false),
			(Ten, Hundred, false),
			(GreaterThanOneSmallest, One, true),
		};
		private static (Quad, Quad, bool)[] _lessThanData =
		{
			(Zero, One, true),
			(Zero, Quarter, true),
			(NegativeThousand, Thousand, true),
			(NegativeOne, NegativeThree, false),
			(Hundred, Two, false),
			(LessThanOneLargest, One, true),
		};
		private static (Quad, Quad, bool)[] _equalToData =
		{
			(One, One, true), 
			(Two, Two, true),
			(Quad.NaN, Quad.NaN, false),
			(GreatestSubnormal, GreatestSubnormal, true),
		};
		private static (Quad, Quad, bool)[] _notEqualToData =
		{
			(One, One, false),
			(Quad.NaN, Quad.NaN, true),
			(NegativeTwo, Two, true),
			(SmallestSubnormal, GreatestSubnormal, true)
		};

		private static (Quad, Quad, Quad, Quad)[] _fmaData =
		{
			(One, One, One, Two),
			(Ten, Ten, Zero, Hundred),
			(Five, Zero, Five, Five),
		};


		public static TryParseTheoryData<Quad> TryParseTheoryData = new(_tryParseData);

		public static UnaryTheoryData<Quad, Quad> UnaryNegationOperationTheoryData = new(_unaryNegationOperationData);
		public static UnaryTheoryData<Quad, Quad> IncrementOperationTheoryData = new(_incrementOperationData);
		public static UnaryTheoryData<Quad, Quad> DecrementOperationTheoryData = new(_decrementOperationData);

		public static OperationTheoryData<Quad, Quad, Quad> AdditionOperationTheoryData = new(_additionOperationData);
		public static OperationTheoryData<Quad, Quad, Quad> SubtractionOperationTheoryData = new(_subtractionOperationData);
		public static OperationTheoryData<Quad, Quad, Quad> MultiplicationOperationTheoryData = new(_multiplicationOperationData);
		public static OperationTheoryData<Quad, Quad, Quad> DivisionOperationTheoryData = new(_divisionOperationData);

		public static CastingTheoryData<byte, Quad> CastFromByteTheoryData = new(_castFromByteData);
		public static CastingTheoryData<ushort, Quad> CastFromUInt16TheoryData = new(_castFromUInt16Data);
		public static CastingTheoryData<uint, Quad> CastFromUInt32TheoryData = new(_castFromUInt32Data);
		public static CastingTheoryData<ulong, Quad> CastFromUInt64TheoryData = new(_castFromUInt64Data);
		public static CastingTheoryData<UInt128, Quad> CastFromUInt128TheoryData = new(_castFromUInt128Data);
		public static CastingTheoryData<UInt256, Quad> CastFromUInt256TheoryData = new(_castFromUInt256Data);
		public static CastingTheoryData<UInt512, Quad> CastFromUInt512TheoryData = new(_castFromUInt512Data);
		public static CastingTheoryData<sbyte, Quad> CastFromSByteTheoryData = new(_castFromSByteData);
		public static CastingTheoryData<short, Quad> CastFromInt16TheoryData = new(_castFromInt16Data);
		public static CastingTheoryData<int, Quad> CastFromInt32TheoryData = new(_castFromInt32Data);
		public static CastingTheoryData<long, Quad> CastFromInt64TheoryData = new(_castFromInt64Data);
		public static CastingTheoryData<Int128, Quad> CastFromInt128TheoryData = new(_castFromInt128Data);
		public static CastingTheoryData<Int256, Quad> CastFromInt256TheoryData = new(_castFromInt256Data);
		public static CastingTheoryData<Int512, Quad> CastFromInt512TheoryData = new(_castFromInt512Data);
		public static CastingTheoryData<Half, Quad> CastFromHalfTheoryData = new(_castFromHalfData);
		public static CastingTheoryData<float, Quad> CastFromSingleTheoryData = new(_castFromSingleData);
		public static CastingTheoryData<double, Quad> CastFromDoubleTheoryData = new(_castFromDoubleData);

		public static CastingTheoryData<Quad, byte> CastToByteTheoryData = new(_castToByteData);
		public static CastingTheoryData<Quad, ushort> CastToUInt16TheoryData = new(_castToUInt16Data);
		public static CastingTheoryData<Quad, uint> CastToUInt32TheoryData = new(_castToUInt32Data);
		public static CastingTheoryData<Quad, ulong> CastToUInt64TheoryData = new(_castToUInt64Data);
		public static CastingTheoryData<Quad, UInt128> CastToUInt128TheoryData = new(_castToUInt128Data);
		public static CastingTheoryData<Quad, UInt256> CastToUInt256TheoryData = new(_castToUInt256Data);
		public static CastingTheoryData<Quad, UInt512> CastToUInt512TheoryData = new(_castToUInt512Data);
		public static CastingTheoryData<Quad, sbyte> CastToSByteTheoryData = new(_castToSByteData);
		public static CastingTheoryData<Quad, short> CastToInt16TheoryData = new(_castToInt16Data);
		public static CastingTheoryData<Quad, int> CastToInt32TheoryData = new(_castToInt32Data);
		public static CastingTheoryData<Quad, long> CastToInt64TheoryData = new(_castToInt64Data);
		public static CastingTheoryData<Quad, Int128> CastToInt128TheoryData = new(_castToInt128Data);
		public static CastingTheoryData<Quad, Int256> CastToInt256TheoryData = new(_castToInt256Data);
		public static CastingTheoryData<Quad, Int512> CastToInt512TheoryData = new(_castToInt512Data);
		public static CastingTheoryData<Quad, Half> CastToHalfTheoryData = new(_castToHalfData);
		public static CastingTheoryData<Quad, float> CastToSingleTheoryData = new(_castToSingleData);
		public static CastingTheoryData<Quad, double> CastToDoubleTheoryData = new(_castToDoubleData);

		public static RoundTheoryData<Quad> RoundAwayFromZeroTheoryData = new(_roundAwayFromZeroData);
		public static RoundTheoryData<Quad> RoundToEvenTheoryData = new(_roundToEvenData);
		public static RoundTheoryData<Quad> RoundToNegativeInfinityTheoryData = new(_roundToNegativeInfinityData);
		public static RoundTheoryData<Quad> RoundToPositiveInfinityTheoryData = new(_roundToPositiveInfinityData);
		public static RoundTheoryData<Quad> RoundToZeroTheoryData = new(_roundToZeroData);

		public static ComparisonOperatorsTheoryData<Quad, Quad> GreaterThanTheoryData = new(_greaterThanData);
		public static ComparisonOperatorsTheoryData<Quad, Quad> LessThanTheoryData = new(_lessThanData);
		public static ComparisonOperatorsTheoryData<Quad, Quad> EqualToTheoryData = new(_equalToData);
		public static ComparisonOperatorsTheoryData<Quad, Quad> NotEqualToTheoryData = new(_notEqualToData);

		public static FusedMultiplyAddTheoryData<Quad> FMATheoryData = new(_fmaData);
#pragma warning restore S3263 // Static fields should appear in the order they must be initialized 
	}
}
