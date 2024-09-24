using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Core
{
	public partial class OctoTest
	{
#pragma warning disable S3263 // Static fields should appear in the order they must be initialized 
		private static (string, bool, Octo)[] _tryParseData =>
		[
			("10,0", true, Ten),
			("3", true, Three),
			("-3", true, NegativeThree),
			("2,0", true, Two),
			("-2", true, NegativeTwo),
			("0", true, Zero),
			("-0", true, NegativeZero),
			(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, true, Octo.PositiveInfinity),
			(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, true, Octo.NegativeInfinity),
			(NumberFormatInfo.CurrentInfo.NaNSymbol, true, Octo.NaN),
		];

		private static (Octo, Octo)[] _unaryNegationOperationData =>
		[
			(Zero, NegativeZero),
			(One, NegativeOne),
			(Two, NegativeTwo),
			(Ten, NegativeTen),
			(Hundred, NegativeHundred),
			(Thousand, NegativeThousand)
		];
		private static (Octo, Octo)[] _incrementOperationData =>
		[
			(NegativeTwo, NegativeOne),
			(NegativeOne, Zero),
			(Zero, One),
			(One, Two),
		];
		private static (Octo, Octo)[] _decrementOperationData =>
		[
			(NegativeOne, NegativeTwo),
			(Zero, NegativeOne),
			(One, Zero),
			(Two, One),
		];

		private static (Octo, Octo, Octo)[] _additionOperationData =>
		[
			(One, One, Two),
			(One, NegativeOne, Zero),
			(One, NegativeTwo, NegativeOne),
			(One, Four, Five),
			(Three, Two, Five),
			(SmallestSubnormal, GreatestSubnormal, Values.CreateFloat<Octo>(0x0000_1000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)),
			(Octo.PositiveInfinity, Octo.One, Octo.PositiveInfinity),
			(Octo.NegativeInfinity, Octo.One, Octo.NegativeInfinity),
			(Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.PositiveInfinity),
			(Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NegativeInfinity),
		];
		private static (Octo, Octo, Octo)[] _subtractionOperationData =>
		[
			(One, One, Zero),
			(One, NegativeOne, Two),
			(One, Two, NegativeOne),
			(SmallestSubnormal, GreatestSubnormal, Values.CreateFloat<Octo>(0x8000_0FFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFE)),
			(Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.NaN),
			(Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NaN),
		];
		private static (Octo, Octo, Octo)[] _multiplicationOperationData =>
		[
			(One, One, One),
			(One, NegativeOne, NegativeOne),
			(Ten, Ten, Hundred),
			(NegativeHundred, Ten, NegativeThousand),
			(NegativeTen, Hundred, NegativeThousand),
			(Zero, NegativeThousand, NegativeZero),
			(Zero, Octo.PositiveInfinity, Octo.NaN),
			(NegativeZero, Octo.NegativeInfinity, Octo.NaN),
			(Octo.PositiveInfinity, Zero, Octo.NaN),
			(Octo.NegativeInfinity, NegativeZero, Octo.NaN),
		];
		private static (Octo, Octo, Octo)[] _divisionOperationData =>
		[
			(One, Two, Half),
			(One, Four, Quarter),
			(Ten, Five, Two),
			(Ten, Ten, One),
			(Hundred, Ten, Ten),
			(NegativeThousand, Ten, NegativeHundred),
			(Zero, Zero, Octo.NaN),
			(One, Zero, Octo.PositiveInfinity),
			(NegativeOne, Zero, Octo.NegativeInfinity),
			(Octo.PositiveInfinity, Octo.PositiveInfinity, Octo.NaN),
			(Octo.NegativeInfinity, Octo.NegativeInfinity, Octo.NaN),
		];

		private static (byte, Octo)[] _castFromByteData =>
		[
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(byte.MaxValue, ByteMaxValue),
		];
		private static (ushort, Octo)[] _castFromUInt16Data =>
		[
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
		];
		private static (uint, Octo)[] _castFromUInt32Data =>
		[
			(0, Zero),
			(1, One),
			(2, Two),
			(10, Ten),
			(100, Hundred),
			(1000, Thousand),
			(byte.MaxValue, ByteMaxValue),
			(ushort.MaxValue, UInt16MaxValue),
			(uint.MaxValue, UInt32MaxValue),
		];
		private static (ulong, Octo)[] _castFromUInt64Data =>
		[
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
		];
		private static (UInt128, Octo)[] _castFromUInt128Data =>
		[
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
			(UInt128.MaxValue, UInt128MaxValue),
		];
		private static (UInt256, Octo)[] _castFromUInt256Data =>
		[
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
			(UInt128.MaxValue, UInt128MaxValue),
		];
		private static (UInt512, Octo)[] _castFromUInt512Data =>
		[
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
			(UInt128.MaxValue, UInt128MaxValue),
		];
		private static (sbyte, Octo)[] _castFromSByteData =>
		[
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
		];
		private static (short, Octo)[] _castFromInt16Data =>
		[
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
		];
		private static (int, Octo)[] _castFromInt32Data =>
		[
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
		];
		private static (long, Octo)[] _castFromInt64Data =>
		[
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
		];
		private static (Int128, Octo)[] _castFromInt128Data =>
		[
			(Int128.MinValue, Int128MinValue),
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
			(Int128.MaxValue, Int128MaxValue),
		];
		private static (Int256, Octo)[] _castFromInt256Data =>
		[
			(Int128.MinValue, Int128MinValue),
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
			(Int128.MaxValue, Int128MaxValue),
		];
		private static (Int512, Octo)[] _castFromInt512Data =>
		[
			(Int128.MinValue, Int128MinValue),
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
			(Int128.MaxValue, Int128MaxValue),
		];
		private static (Half, Octo)[] _castFromHalfData =>
		[
			(System.Half.NegativeOne, NegativeOne),
			(System.Half.NegativeZero, NegativeZero),
			(System.Half.Zero, Zero),
			(System.Half.One, One),
			((Half)10, Ten),
			((Half)100, Hundred),
			((Half)1000, Thousand),
		];
		private static (float, Octo)[] _castFromSingleData =>
		[
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
		];
		private static (double, Octo)[] _castFromDoubleData =>
		[
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
		];

		private static (Octo, byte)[] _castToByteData =>
		[
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(ByteMaxValue, byte.MaxValue),
		];
		private static (Octo, ushort)[] _castToUInt16Data =>
		[
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
		];
		private static (Octo, uint)[] _castToUInt32Data =>
		[
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Hundred, 100),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
		];
		private static (Octo, ulong)[] _castToUInt64Data =>
		[
			(Half, 0),
			(One, 1),
			(Two, 2),
			(Ten, 10),
			(Thousand, 1000),
			(ByteMaxValue, byte.MaxValue),
			(UInt16MaxValue, ushort.MaxValue),
			(UInt32MaxValue, uint.MaxValue),
			(UInt64MaxValue, ulong.MaxValue),
		];
		private static (Octo, UInt128)[] _castToUInt128Data =>
		[
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
			(UInt128MaxValue, UInt128.MaxValue),
		];
		private static (Octo, UInt256)[] _castToUInt256Data =>
		[
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
			(UInt128MaxValue, UInt128.MaxValue),
		];
		private static (Octo, UInt512)[] _castToUInt512Data =>
		[
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
			(UInt128MaxValue, UInt128.MaxValue),
		];
		private static (Octo, sbyte)[] _castToSByteData =>
		[
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
		];
		private static (Octo, short)[] _castToInt16Data =>
		[
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
		];
		private static (Octo, int)[] _castToInt32Data =>
		[
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
		];
		private static (Octo, long)[] _castToInt64Data =>
		[
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
		];
		private static (Octo, Int128)[] _castToInt128Data =>
		[
			(Int128MinValue, Int128.MinValue),
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
			(Int128MaxValue, Int128.MaxValue),
		];
		private static (Octo, Int256)[] _castToInt256Data =>
		[
			(Int128MinValue, Int128.MinValue),
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
			(Int128MaxValue, Int128.MaxValue),
		];
		private static (Octo, Int512)[] _castToInt512Data =>
		[
			(Int128MinValue, Int128.MinValue),
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
			(Int128MaxValue, Int128.MaxValue),
		];
		private static (Octo, Half)[] _castToHalfData =>
		[
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
		];
		private static (Octo, float)[] _castToSingleData =>
		[
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
		];
		private static (Octo, double)[] _castToDoubleData =>
		[
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
		];
		private static (Octo, Quad)[] _castToQuadData =>
		[
			(NegativeThousand, QuadTest.NegativeThousand),
			(NegativeHundred, QuadTest.NegativeHundred),
			(NegativeTen, QuadTest.NegativeTen),
			(NegativeTwo, QuadTest.NegativeTwo),
			(NegativeOne, QuadTest.NegativeOne),
			(NegativeZero, QuadTest.NegativeZero),
			(Zero, QuadTest.Zero),
			(Half, QuadTest.Half),
			(One, QuadTest.One),
			(Two, QuadTest.Two),
			(Ten, QuadTest.Ten),
			(Hundred, QuadTest.Hundred),
			(Thousand, QuadTest.Thousand),
		];

		private static (Octo, int, MidpointRounding, Octo)[] _roundAwayFromZeroData =>
		[
			(Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Four),
			(Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, Three),
			(Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, Three),
			(Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, Two),
			(Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.AwayFromZero, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.AwayFromZero, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.AwayFromZero, NegativeFour),
		];
		private static (Octo, int, MidpointRounding, Octo)[] _roundToEvenData =>
		[
			(Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Four),
			(Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, Three),
			(Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, Two),
			(Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, Two),
			(Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToEven, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToEven, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToEven, NegativeFour),
		];
		private static (Octo, int, MidpointRounding, Octo)[] _roundToNegativeInfinityData =>
		[
			(Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Three),
			(Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, Two),
			(Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToNegativeInfinity, NegativeThree),
			(Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToNegativeInfinity, NegativeFour),
		];
		private static (Octo, int, MidpointRounding, Octo)[] _roundToPositiveInfinityData =>
		[
			(Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Four),
			(Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, Three),
			(Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToPositiveInfinity, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToPositiveInfinity, NegativeThree),
		];
		private static (Octo, int, MidpointRounding, Octo)[] _roundToZeroData =>
		[
			(Values.CreateFloat<Octo>(0x4000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Three),
			(Values.CreateFloat<Octo>(0x4000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Octo>(0x4000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Octo>(0x4000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, Two),
			(Values.CreateFloat<Octo>(0xC000_00CC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCD), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0400_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666, 0x6666_6666_6666_6666), 0, MidpointRounding.ToZero, NegativeTwo),
			(Values.CreateFloat<Octo>(0xC000_0C00_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 0, MidpointRounding.ToZero, NegativeThree),
		];

		private static (Octo, Octo, Octo, Octo)[] _fmaData =>
		[
			(One, One, One, Two),
			(Ten, Ten, Zero, Hundred),
			(Five, Zero, Five, Five),
			(Half, Two, Two, Three),
			(Two, Four, Two, Ten),
			(Ten, Half, Five, Ten),
			(Values.CreateFloat<Octo>(13835044861142630400, 0, 0, 0), One, Two, Values.CreateFloat<Octo>(0x3FFF_E800_0000_0000, 0, 0, 0)),
		];


		public static TryParseTheoryData<Octo> TryParseTheoryData = new(_tryParseData);

		public static UnaryTheoryData<Octo, Octo> UnaryNegationOperationTheoryData = new(_unaryNegationOperationData);
		public static UnaryTheoryData<Octo, Octo> IncrementOperationTheoryData = new(_incrementOperationData);
		public static UnaryTheoryData<Octo, Octo> DecrementOperationTheoryData = new(_decrementOperationData);

		public static OperationTheoryData<Octo, Octo, Octo> AdditionOperationTheoryData = new(_additionOperationData);
		public static OperationTheoryData<Octo, Octo, Octo> SubtractionOperationTheoryData = new(_subtractionOperationData);
		public static OperationTheoryData<Octo, Octo, Octo> MultiplicationOperationTheoryData = new(_multiplicationOperationData);
		public static OperationTheoryData<Octo, Octo, Octo> DivisionOperationTheoryData = new(_divisionOperationData);

		public static CastingTheoryData<byte, Octo> CastFromByteTheoryData = new(_castFromByteData);
		public static CastingTheoryData<ushort, Octo> CastFromUInt16TheoryData = new(_castFromUInt16Data);
		public static CastingTheoryData<uint, Octo> CastFromUInt32TheoryData = new(_castFromUInt32Data);
		public static CastingTheoryData<ulong, Octo> CastFromUInt64TheoryData = new(_castFromUInt64Data);
		public static CastingTheoryData<UInt128, Octo> CastFromUInt128TheoryData = new(_castFromUInt128Data);
		public static CastingTheoryData<UInt256, Octo> CastFromUInt256TheoryData = new(_castFromUInt256Data);
		public static CastingTheoryData<UInt512, Octo> CastFromUInt512TheoryData = new(_castFromUInt512Data);
		public static CastingTheoryData<sbyte, Octo> CastFromSByteTheoryData = new(_castFromSByteData);
		public static CastingTheoryData<short, Octo> CastFromInt16TheoryData = new(_castFromInt16Data);
		public static CastingTheoryData<int, Octo> CastFromInt32TheoryData = new(_castFromInt32Data);
		public static CastingTheoryData<long, Octo> CastFromInt64TheoryData = new(_castFromInt64Data);
		public static CastingTheoryData<Int128, Octo> CastFromInt128TheoryData = new(_castFromInt128Data);
		public static CastingTheoryData<Int256, Octo> CastFromInt256TheoryData = new(_castFromInt256Data);
		public static CastingTheoryData<Int512, Octo> CastFromInt512TheoryData = new(_castFromInt512Data);
		public static CastingTheoryData<Half, Octo> CastFromHalfTheoryData = new(_castFromHalfData);
		public static CastingTheoryData<float, Octo> CastFromSingleTheoryData = new(_castFromSingleData);
		public static CastingTheoryData<double, Octo> CastFromDoubleTheoryData = new(_castFromDoubleData);

		public static CastingTheoryData<Octo, byte> CastToByteTheoryData = new(_castToByteData);
		public static CastingTheoryData<Octo, ushort> CastToUInt16TheoryData = new(_castToUInt16Data);
		public static CastingTheoryData<Octo, uint> CastToUInt32TheoryData = new(_castToUInt32Data);
		public static CastingTheoryData<Octo, ulong> CastToUInt64TheoryData = new(_castToUInt64Data);
		public static CastingTheoryData<Octo, UInt128> CastToUInt128TheoryData = new(_castToUInt128Data);
		public static CastingTheoryData<Octo, UInt256> CastToUInt256TheoryData = new(_castToUInt256Data);
		public static CastingTheoryData<Octo, UInt512> CastToUInt512TheoryData = new(_castToUInt512Data);
		public static CastingTheoryData<Octo, sbyte> CastToSByteTheoryData = new(_castToSByteData);
		public static CastingTheoryData<Octo, short> CastToInt16TheoryData = new(_castToInt16Data);
		public static CastingTheoryData<Octo, int> CastToInt32TheoryData = new(_castToInt32Data);
		public static CastingTheoryData<Octo, long> CastToInt64TheoryData = new(_castToInt64Data);
		public static CastingTheoryData<Octo, Int128> CastToInt128TheoryData = new(_castToInt128Data);
		public static CastingTheoryData<Octo, Int256> CastToInt256TheoryData = new(_castToInt256Data);
		public static CastingTheoryData<Octo, Int512> CastToInt512TheoryData = new(_castToInt512Data);
		public static CastingTheoryData<Octo, Half> CastToHalfTheoryData = new(_castToHalfData);
		public static CastingTheoryData<Octo, float> CastToSingleTheoryData = new(_castToSingleData);
		public static CastingTheoryData<Octo, double> CastToDoubleTheoryData = new(_castToDoubleData);
		public static CastingTheoryData<Octo, Quad> CastToQuadTheoryData = new(_castToQuadData);

		public static RoundTheoryData<Octo> RoundAwayFromZeroTheoryData = new(_roundAwayFromZeroData);
		public static RoundTheoryData<Octo> RoundToEvenTheoryData = new(_roundToEvenData);
		public static RoundTheoryData<Octo> RoundToNegativeInfinityTheoryData = new(_roundToNegativeInfinityData);
		public static RoundTheoryData<Octo> RoundToPositiveInfinityTheoryData = new(_roundToPositiveInfinityData);
		public static RoundTheoryData<Octo> RoundToZeroTheoryData = new(_roundToZeroData);

		public static FusedMultiplyAddTheoryData<Octo> FMATheoryData = new(_fmaData);
#pragma warning restore S3263 // Static fields should appear in the order they must be initialized 
	}
}
