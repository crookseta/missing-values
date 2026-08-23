using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MissingValues.Tests.Data.Sources;
using MissingValues.Tests.Extensions;

namespace MissingValues.Tests.Data;

public class Int256DataSources
	: IMathOperatorsDataSource<Int256>,
	IShiftOperatorsDataSource<Int256>,
	IBitwiseOperatorsDataSource<Int256>,
	IEqualityOperatorsDataSource<Int256>,
	IComparisonOperatorsDataSource<Int256>,
	INumberBaseDataSource<Int256>,
	INumberDataSource<Int256>,
	IBinaryNumberDataSource<Int256>,
	IBinaryIntegerDataSource<Int256>
{
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_AdditionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.Zero, Int256.One);
		yield return () => (Int256.One, Int256.One, new Int256(0, 0, 0, 2));
		yield return () => (new Int256(0, 0, 1, ulong.MaxValue), new Int256(0, 0, 1, 1), new Int256(0, 0, 3, 0));
		yield return () => (new Int256(0, 1, ulong.MaxValue, ulong.MaxValue), new Int256(0, 1, 1, 1), new Int256(0, 3, 1, 0));
		yield return () => (new Int256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 1, 1, 1), new Int256(3, 1, 1, 0));
		yield return () => (Int256.MaxValue, Int256.One, Int256.MinValue);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.Zero);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedAdditionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, Int256.One, new Int256(0, 0, 0, 2), false);
		yield return () => (new Int256(0, 0, 1, ulong.MaxValue), new Int256(0, 0, 1, 1), new Int256(0, 0, 3, 0), false);
		yield return () => (new Int256(0, 1, ulong.MaxValue, ulong.MaxValue), new Int256(0, 1, 1, 1), new Int256(0, 3, 1, 0), false);
		yield return () => (new Int256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 1, 1, 1), new Int256(3, 1, 1, 0), false);
		yield return () => (Int256.MaxValue, Int256.One, Int256.MinValue, true);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.Zero, false);
		yield return () => (Int256.MinValue, Int256.NegativeOne, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedDecrementTestData()
	{
		yield return () => (Int256.Zero, Int256.NegativeOne, false);
		yield return () => (Int256.One, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 1), false);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, ulong.MaxValue), false);
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (new Int256(1, 0, 0, 0), new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (Int256.MinValue, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedIncrementTestData()
	{
		yield return () => (Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, new Int256(0, 0, 0, 2), false);
		yield return () => (Int256.MaxValue, Int256.Zero, true);
		yield return () => (Int256.NegativeOne, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 1, 0), false);
		yield return () => (new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 0, 0, 0), false);
		yield return () => (new Int256(unchecked((ulong)-123456789), 987654321, 555555555, 999999999), new Int256(unchecked((ulong)-123456789), 987654321, 555555555, 1000000000), false);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedMultiplyTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.One, Int256.One, false);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne, false);
		yield return () => (Int256.NegativeOne, Int256.NegativeOne, Int256.One, false);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 6), false);
		yield return () => (Int256.MaxValue, Int256.One, Int256.MaxValue, false);
		yield return () => (Int256.MaxValue, new Int256(0, 0, 0, 2), default, true);
		yield return () => (Int256.MinValue, Int256.NegativeOne, default, true);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, ulong.MaxValue), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFE, 0x0000_0000_0000_0001), false);
		yield return () => (new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(2, 0, 0, 0), new Int256(0xFFFF_FFFF_FFFF_FFFE, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), false);
		yield return () => (new Int256(long.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(2, 0, 0, 0), default, true);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedSubtractionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.One, Int256.Zero, Int256.One, false);
		yield return () => (Int256.One, Int256.One, Int256.Zero, false);
		yield return () => (new Int256(0, 0, 0, 2), Int256.One, Int256.One, false);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, 1), new Int256(0, 0, 0, ulong.MaxValue), false);
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue), new Int256(0, 0, 0, 1), false);
		yield return () => (Int256.MinValue, Int256.One, Int256.MaxValue, true);
		yield return () => (Int256.MaxValue, Int256.NegativeOne, Int256.MinValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_DecrementTestData()
	{
		yield return () => (Int256.Zero, Int256.NegativeOne);
		yield return () => (Int256.One, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, ulong.MaxValue));
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new Int256(1, 0, 0, 0), new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (Int256.MinValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_DivisionTestData()
	{
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.NegativeOne);
		yield return () => (new Int256(0, 0, 0, 4), new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 2));
		yield return () => (Int256.MaxValue, Int256.One, Int256.MaxValue);
		yield return () => (Int256.MinValue, Int256.One, Int256.MinValue);
		yield return () => (Int256.Zero, Int256.MaxValue, Int256.Zero);
		yield return () => (Int256.MaxValue, Int256.MaxValue, Int256.One);
		yield return () => (Int256.MinValue, Int256.MinValue, Int256.One);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 1, 0, 0), Int256.Zero);
		yield return () => (new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000));
		yield return () => (new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000));
		yield return () => (new Int256(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 0, 10), new Int256(0x0CCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC));
		yield return () => (new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0, 0, 0, 10), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0CCC_CCCC_CCCC_CCCC, 0xCCCC_CCCC_CCCC_CCCC));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0, 0, 0, 10), new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xF333_3333_3333_3333, 0x3333_3333_3333_3334));
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_IncrementTestData()
	{
		yield return () => (Int256.Zero, Int256.One);
		yield return () => (Int256.One, new Int256(0, 0, 0, 2));
		yield return () => (Int256.MaxValue, Int256.MinValue);
		yield return () => (Int256.NegativeOne, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 1, 0));
		yield return () => (new Int256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(1, 0, 0, 0));
		yield return () => (new Int256(123456789, 987654321, 555555555, 999999999), new Int256(123456789, 987654321, 555555555, 1000000000));
		yield return () => (new Int256(0x8000000000000000, 0, 0, 0),new Int256(0x8000000000000000, 0, 0, 1));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_ModulusTestData()
	{
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.Zero);
		yield return () => (new Int256(0, 0, 0, 123456789), Int256.One, Int256.Zero);
		yield return () => (Int256.MaxValue, Int256.MaxValue, Int256.Zero);
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 1, 0, 0), new Int256(0, 0, 1, 0));
		yield return () => (new Int256(0, 0, 0, 10), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0, 0, 0, 15), new Int256(0, 0, 0, 5), Int256.Zero);
		yield return () => (Int256.NegativeOne, new Int256(0, 0, 0, 2), Int256.NegativeOne);
		yield return () => (new Int256(0, 0, 0, 7), Int256.NegativeOne, Int256.Zero);
		yield return () => (Int256.MaxValue, new Int256(0, 0, 0, 123456789), new Int256(0, 0, 0, 77645365));
		yield return () => (new Int256(0x0000_0000_0000_0040, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int256(0x0000_0000_0000_000F, 0xEE50_B702_5C36_A080, 0x2F23_6D04_753D_5B48, 0xE800_0000_0000_0000), new Int256(0x0000_0000_0000_0000, 0x46BD_23F6_8F25_7DFF, 0x4372_4BEE_2B0A_92DC, 0x6000_0000_0000_0000));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_MultiplyTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.NegativeOne, Int256.One);
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 3), new Int256(0, 0, 0, 6));
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, 2), new Int256(0, 0, 1, ulong.MaxValue - 1));
		yield return () => (new Int256(0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, ulong.MaxValue), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFE, 0x0000_0000_0000_0001));
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 1, 0), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_SubtractionTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.Zero, Int256.One);
		yield return () => (Int256.One, Int256.One, Int256.Zero);
		yield return () => (Int256.Zero, Int256.One, Int256.NegativeOne);
		yield return () => (Int256.MaxValue,Int256.One, new Int256(0x7FFF_FFFF_FFFF_FFFF, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue - 1));
		yield return () => (Int256.MinValue,Int256.One,Int256.MaxValue);
		yield return () => (Int256.MinValue,Int256.NegativeOne, new Int256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001));
		yield return () => (new Int256(1, 2, 3, 4),new Int256(0, 1, 2, 3),new Int256(1, 1, 1, 1));
		yield return () => (new Int256(0, 0, 0, 0),new Int256(0, 0, 0, 1),new Int256(unchecked((ulong)-1), ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_UnaryNegationTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedUnaryNegationTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftLeftTestData()
	{
		yield return () => (Int256.Zero, 100, Int256.Zero);
		yield return () => (Int256.One, 0, Int256.One);
		yield return () => (Int256.One, 1, new Int256(0, 0, 0, 2));
		yield return () => (Int256.One, 2, new Int256(0, 0, 0, 4));
		yield return () => (Int256.One, 64, new Int256(0, 0, 1, 0));
		yield return () => (Int256.One, 128, new Int256(0, 1, 0, 0));
		yield return () => (Int256.One, 192, new Int256(1, 0, 0, 0));
		yield return () => (new Int256(0, 0, 0, 0xFFFF_FFFF_FFFF_FFFF), 32, new Int256(0, 0, 0xFFFF_FFFF, 0xFFFF_FFFF_0000_0000));
		yield return () => (new Int256(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), 96, new Int256(0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_0000_0000, 0));
		yield return () => (Int256.One, 256, Int256.One);
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftRightTestData()
	{
		yield return () => (Int256.Zero, 100, Int256.Zero);
		yield return () => (Int256.One, 0, Int256.One);
		yield return () => (new Int256(1, 0, 0, 0), 64, new Int256(0, 1, 0, 0));
		yield return () => (new Int256(1, 0, 0, 0), 128, new Int256(0, 0, 1, 0));
		yield return () => (new Int256(1, 0, 0, 0), 192, new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0), 031, new Int256(0b1111111111111111111111111111111100000000000000000000000000000000, 0, 0, 0));
		yield return () => (new Int256(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0), 127, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0));
		yield return () => (new Int256(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0), 255, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => (Int256.One, 256, Int256.One);
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_UnsignedShiftRightTestData()
	{
		yield return () => (Int256.Zero, 100, Int256.Zero);
		yield return () => (Int256.One, 0, Int256.One);
		yield return () => (new Int256(1, 0, 0, 0), 64, new Int256(0, 1, 0, 0));
		yield return () => (new Int256(1, 0, 0, 0), 128, new Int256(0, 0, 1, 0));
		yield return () => (new Int256(1, 0, 0, 0), 192, new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0, 0, 0), 64, new Int256(0, 0xFFFF_FFFF_FFFF_FFFF, 0, 0));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0, 0, 0), 128, new Int256(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0));
		yield return () => (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0, 0, 0), 192, new Int256(0, 0, 0, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (Int256.One, 256, Int256.One);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseAndTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.Zero, Int256.MaxValue, Int256.Zero);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4));
		yield return () => (new Int256(1, 2, 3, 4), Int256.MaxValue, new Int256(1, 2, 3, 4));
		yield return () => (new Int256(0xFFFFFFFF00000000, 0xAAAAAAAA55555555, 0x123456789ABCDEF0, 0x0F0F0F0F0F0F0F0F), new Int256(0x00000000FFFFFFFF, 0x55555555AAAAAAAA, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new Int256(0x0000000000000000, 0x0000000000000000, 0x020406080A0C0E00, 0x0000000000000000));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseOrTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.Zero, Int256.MaxValue, Int256.MaxValue);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4));
		yield return () => (new Int256(1, 2, 3, 4), Int256.MaxValue, new Int256(long.MaxValue | 1, ulong.MaxValue | 2, ulong.MaxValue | 3, ulong.MaxValue | 4));
		yield return () => (new Int256(0x00000000FFFFFFFF, 0xAAAAAAAA00000000, 0x00000000AAAAAAAA, 0x1234567890ABCDEF), new Int256(0xFFFFFFFF00000000, 0x0000000055555555, 0x5555555500000000, 0xFEDCBA9876543210), new Int256(0xFFFFFFFFFFFFFFFF, 0xAAAAAAAA55555555, 0x55555555AAAAAAAA, 0x1234567890ABCDEF | 0xFEDCBA9876543210));
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseXorTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, Int256.Zero);
		yield return () => (Int256.Zero, Int256.MaxValue, Int256.MaxValue);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4), Int256.Zero);
		yield return () => (new Int256(0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x1234567890ABCDEF, 0xFEDCBA9876543210), new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(ulong.MaxValue, 0x0, 0xEDCBA9876F543210, 0x0123456789ABCDEF));
		yield return () => (new Int256(0x1234567890ABCDEF, 0xAAAAAAAA00000000, 0x00000000AAAAAAAA, 0xFFFFFFFFFFFFFFFF), new Int256(0xFFFFFFFFFFFFFFFF, 0x00000000AAAAAAAA, 0xAAAAAAAA00000000, 0x0000000000000000), new Int256(0xEDCBA9876F543210, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xFFFFFFFFFFFFFFFF));
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_OnesComplementTestData()
	{
		yield return () => (Int256.Zero, new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), Int256.Zero);
		yield return () => (new Int256(0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555), new Int256(0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA));
		yield return () => (new Int256(0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new Int256(~(ulong)0x0123456789ABCDEF, ~0xFEDCBA9876543210, ~(ulong)0x0F0F0F0F0F0F0F0F, ~0xF0F0F0F0F0F0F0F0));
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_EqualityTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, true);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4), true);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 5), false);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 4, 4), false);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 3, 3, 4), false);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(2, 2, 3, 4), false);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_InequalityTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, false);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 4), false);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 3, 5), true);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 2, 4, 4), true);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(1, 3, 3, 4), true);
		yield return () => (new Int256(1, 2, 3, 4), new Int256(2, 2, 3, 4), true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanOrEqualTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, true);
		yield return () => (Int256.NegativeOne, Int256.One, false);
		yield return () => (Int256.One, Int256.NegativeOne, true);
		yield return () => (Int256.Zero, Int256.MinValue, true);
		yield return () => (Int256.Zero, Int256.NegativeOne, true);
		yield return () => (Int256.Zero, Int256.One, false);
		yield return () => (Int256.Zero, Int256.MaxValue, false);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.NegativeOne, Int256.One, false);
		yield return () => (Int256.One, Int256.NegativeOne, true);
		yield return () => (Int256.Zero, Int256.MinValue, true);
		yield return () => (Int256.Zero, Int256.NegativeOne, true);
		yield return () => (Int256.Zero, Int256.One, false);
		yield return () => (Int256.Zero, Int256.MaxValue, false);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanOrEqualTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, true);
		yield return () => (Int256.NegativeOne, Int256.One, true);
		yield return () => (Int256.One, Int256.NegativeOne, false);
		yield return () => (Int256.Zero, Int256.MinValue, false);
		yield return () => (Int256.Zero, Int256.NegativeOne, false);
		yield return () => (Int256.Zero, Int256.One, true);
		yield return () => (Int256.Zero, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero, false);
		yield return () => (Int256.NegativeOne, Int256.One, true);
		yield return () => (Int256.One, Int256.NegativeOne, false);
		yield return () => (Int256.Zero, Int256.MinValue, false);
		yield return () => (Int256.Zero, Int256.NegativeOne, false);
		yield return () => (Int256.Zero, Int256.One, true);
		yield return () => (Int256.Zero, Int256.MaxValue, true);
	}

	public static IEnumerable<Func<(Int256, Int256)>> AbsTestData()
	{
		yield return () => (Int256.Zero, Int256.Zero);
		yield return () => (Int256.One, Int256.One);
		yield return () => (Int256.NegativeOne, Int256.One);
		yield return () => (Int256.MinValue + Int256.One, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsCanonicalTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsComplexNumberTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsEvenIntegerTestData()
	{
		yield return () => (Int256.Zero, true);
		yield return () => (Int256.One, false);
		yield return () => (Int256.NegativeOne, false);
		yield return () => (new Int256(0, 0, 0, 2), true);
		yield return () => (new Int256(0, 0, 0, 3), false);
		yield return () => (new Int256(0, 0, 0, 4), true);
		yield return () => (new Int256(0, 0, 0, 6), true);
		yield return () => (new Int256(0, 0, 0, 8), true);
		yield return () => (new Int256(0, 0, 0, 16), true);
		yield return () => (-new Int256(0, 0, 0, 2), true);
		yield return () => (-new Int256(0, 0, 0, 3), false);
		yield return () => (-new Int256(0, 0, 0, 4), true);
		yield return () => (-new Int256(0, 0, 0, 6), true);
		yield return () => (-new Int256(0, 0, 0, 8), true);
		yield return () => (-new Int256(0, 0, 0, 16), true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsFiniteTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsImaginaryNumberTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsInfinityTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsIntegerTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNaNTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeTestData()
	{
		yield return () => (Int256.Zero, false);
		yield return () => (Int256.One, false);
		yield return () => (Int256.MaxValue, false);
		yield return () => (Int256.NegativeOne, true);
		yield return () => (-Int256.One, true);
		yield return () => (-Int256.MaxValue, true);
		yield return () => (Int256.MinValue, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeInfinityTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNormalTestData()
	{
		yield return () => (Int256.Zero, false);
		yield return () => (Int256.One, true);
		yield return () => (Int256.NegativeOne, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsOddIntegerTestData()
	{
		yield return () => (Int256.Zero, false);
		yield return () => (Int256.One, true);
		yield return () => (Int256.NegativeOne, true);
		yield return () => (new Int256(0, 0, 0, 2), false);
		yield return () => (new Int256(0, 0, 0, 3), true);
		yield return () => (new Int256(0, 0, 0, 4), false);
		yield return () => (new Int256(0, 0, 0, 6), false);
		yield return () => (new Int256(0, 0, 0, 8), false);
		yield return () => (new Int256(0, 0, 0, 16), false);
		yield return () => (-new Int256(0, 0, 0, 2), false);
		yield return () => (-new Int256(0, 0, 0, 3), true);
		yield return () => (-new Int256(0, 0, 0, 4), false);
		yield return () => (-new Int256(0, 0, 0, 6), false);
		yield return () => (-new Int256(0, 0, 0, 8), false);
		yield return () => (-new Int256(0, 0, 0, 16), false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveTestData()
	{
		yield return () => (Int256.Zero, true);
		yield return () => (Int256.One, true);
		yield return () => (Int256.MaxValue, true);
		yield return () => (Int256.NegativeOne, false);
		yield return () => (-Int256.One, false);
		yield return () => (-Int256.MaxValue, false);
		yield return () => (Int256.MinValue, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveInfinityTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsRealNumberTestData()
	{
		yield return () => (Int256.Zero, true);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsSubnormalTestData()
	{
		yield return () => (Int256.Zero, false);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsZeroTestData()
	{
		yield return () => (Int256.Zero, true);
		yield return () => (Int256.One, false);
		yield return () => (Int256.NegativeOne, false);
		yield return () => (Int256.MaxValue, false);
		yield return () => (Int256.MinValue, false);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeTestData()
	{
		yield return () => (Int256.MaxValue, 5, Int256.MaxValue);
		yield return () => (Int256.One, 5, 5);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.One);
		yield return () => (Int256.One, -2, -2);
		yield return () => (Int256.NegativeOne, Int256.MaxValue, Int256.MaxValue);
		yield return () => (Int256.MinValue, -2, Int256.MinValue);
		yield return () => (Int256.MaxValue, Int256.MinValue, Int256.MinValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeNumberTestData()
	{
		yield return () => (Int256.MaxValue, 5, Int256.MaxValue);
		yield return () => (Int256.One, 5, 5);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.One);
		yield return () => (Int256.One, -2, -2);
		yield return () => (Int256.NegativeOne, Int256.MaxValue, Int256.MaxValue);
		yield return () => (Int256.MinValue, -2, Int256.MinValue);
		yield return () => (Int256.MaxValue, Int256.MinValue, Int256.MinValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeTestData()
	{
		yield return () => (Int256.MaxValue, 5, 5);
		yield return () => (Int256.One, 5, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.One, -2, Int256.One);
		yield return () => (Int256.NegativeOne, Int256.MaxValue, Int256.NegativeOne);
		yield return () => (Int256.MinValue, -2, -2);
		yield return () => (Int256.MaxValue, Int256.MinValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeNumberTestData()
	{
		yield return () => (Int256.MaxValue, 5, 5);
		yield return () => (Int256.One, 5, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.One, -2, Int256.One);
		yield return () => (Int256.NegativeOne, Int256.MaxValue, Int256.NegativeOne);
		yield return () => (Int256.MinValue, -2, -2);
		yield return () => (Int256.MaxValue, Int256.MinValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> MultiplyAddEstimateTestData()
	{
		yield return () => (Int256.One, Int256.One, Int256.One, 2);
		yield return () => (Int256.One, Int256.Zero, Int256.One, Int256.One);
		yield return () => (Int256.MaxValue, Int256.NegativeOne, Int256.NegativeOne, Int256.MinValue);
		yield return () => (200, 100, 500, 20500);
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Int256)>> ParseTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.NegativeOne);
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MaxValue);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000", NumberStyles.Number, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("1,000.0", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000", NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int256.MinValue);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Int256)>> ParseSpanTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.NegativeOne);
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MaxValue);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("1,000.0".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000".ToCharArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int256.MinValue);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Int256)>> ParseUtf8TestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.NegativeOne);
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MaxValue);
		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, Int256.One);
		yield return () => ("1,000.0"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, Int256.MinValue);
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int256)>> TryParseTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne);
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000", NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("1,000.0", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000", NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseSpanTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne);
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("1,000.0".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseUtf8TestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne);
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => ("170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
        		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
        		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One);
		yield return () => ("1,000.0"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue);
	}
	
	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int256, int)>> TryParsePartialTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue, 78);
		yield return () => ("-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue, 40);
		yield return () => ("-9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue, 20);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne, 2);
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero, 1);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One, 1);
		yield return () => ("9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000), 19);
		yield return () => ("170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0), 39);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue, 77);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		
		yield return () => ("1E200", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("2.5E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000, 6);
		yield return () => ("1E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000, 4);
		yield return () => ("1.000", NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One, 5);
		yield return () => ("1,000.0", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000, 7);
		yield return () => ("1,000,000", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000, 9);
		yield return () => ("1,000,000,000.00", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000, 16);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000", NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue, 82);
	}
	
	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int256, int)>> TryParsePartialSpanTestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue, 78);
		yield return () => ("-170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue, 40);
		yield return () => ("-9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue, 20);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne, 2);
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero, 1);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One, 1);
		yield return () => ("9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000), 19);
		yield return () => ("170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0), 39);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue, 77);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		
		yield return () => ("1E200".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("2.5E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000, 6);
		yield return () => ("1E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000, 4);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One, 5);
		yield return () => ("1,000.0".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000, 7);
		yield return () => ("1,000,000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000, 9);
		yield return () => ("1,000,000,000.00".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000, 16);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue, 82);
	}
	
	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int256, int)>> TryParsePartialUtf8TestData()
	{
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819969"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue, 78);
		yield return () => ("-170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue, 40);
		yield return () => ("-9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue, 20);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.NegativeOne, 2);
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.Zero, 1);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.One, 1);
		yield return () => ("9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0, 0x8000_0000_0000_0000), 19);
		yield return () => ("170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int256(0, 0, 0x8000_0000_0000_0000, 0), 39);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819967"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MaxValue, 77);
		yield return () => ("57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default, 0);
		
		yield return () => ("1E200"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default, 0);
		yield return () => ("2.5E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000, 6);
		yield return () => ("1E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000, 4);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.One, 5);
		yield return () => ("1,000.0"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000, 7);
		yield return () => ("1,000,000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000, 9);
		yield return () => ("1,000,000,000.00"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000, 16);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int256.MinValue, 82);
	}

	public static IEnumerable<Func<(Int256, string, IFormatProvider?, string)>> ToStringTestData()
	{
		yield return () => (Int256.Int32MaxValue, "x", CultureInfo.InvariantCulture, "7fffffff");
		
		yield return () => (Int256.Int32MaxValue, "X", CultureInfo.InvariantCulture, "7FFFFFFF");
		yield return () => (Int256.Int64MaxValue, "X", CultureInfo.InvariantCulture,  "7FFFFFFFFFFFFFFF");
		yield return () => (Int256.Int128MaxValue, "X", CultureInfo.InvariantCulture, "7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		yield return () => (Int256.MaxValue, "X", CultureInfo.InvariantCulture, "7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		
		yield return () => (Int256.Int32MaxValue, "B", CultureInfo.InvariantCulture, "1111111111111111111111111111111");
		yield return () => (Int256.Int64MaxValue, "B", CultureInfo.InvariantCulture,  "111111111111111111111111111111111111111111111111111111111111111");
		yield return () => (Int256.Int128MaxValue, "B", CultureInfo.InvariantCulture, "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		yield return () => (Int256.MaxValue, "b", CultureInfo.InvariantCulture, "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		
		yield return () => (Int256.MaxValue, "e25", CultureInfo.InvariantCulture, "5.7896044618658097711785493e+76");
		yield return () => (Int256.MinValue, "e25", CultureInfo.InvariantCulture, "-5.7896044618658097711785493e+76");
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> ClampTestData()
	{
		yield return () => (
			new Int256(0, 0, 0, 1),
			new Int256(0, 0, 0, 2),
			new Int256(0, 0, 0, 4),
			new Int256(0, 0, 0, 2)
		);
		yield return () => (
			new Int256(0, 0, 0, 1),
			new Int256(0, 0, 0, 1),
			new Int256(0, 0, 0, 4),
			new Int256(0, 0, 0, 1)
		);
		yield return () => (
			new Int256(0, 1, 0, 0),
			new Int256(0, 0, 0, 1),
			new Int256(1, 0, 0, 0),
			new Int256(0, 1, 0, 0)
		);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> CopySignTestData()
	{
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.NegativeOne, Int256.One, Int256.One);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxTestData()
	{
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.One);
		yield return () => (Int256.MinValue, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.Zero, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.MaxValue, Int256.MaxValue);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinTestData()
	{
		yield return () => (Int256.One, Int256.One, Int256.One);
		yield return () => (Int256.One, Int256.NegativeOne, Int256.NegativeOne);
		yield return () => (Int256.MinValue, Int256.NegativeOne, Int256.MinValue);
		yield return () => (Int256.Zero, Int256.One, Int256.Zero);
		yield return () => (Int256.One, Int256.MaxValue, Int256.One);
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(Int256, int)>> SignTestData()
	{
		yield return () => (Int256.Zero, 0);
		yield return () => (Int256.MaxValue, 1);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.MinValue, -1);
		yield return () => (Int256.NegativeOne, -1);
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPow2TestData()
	{
		yield return () => (Int256.Zero, false);
		yield return () => (Int256.One, true);
		yield return () => (new Int256(0, 0, 0, 3), false);
		yield return () => (new Int256(0, 0, 0, 4), true);
		yield return () => (new Int256(0, 0, 0, 6), false);
		yield return () => (new Int256(0, 0, 0, 8), true);
		yield return () => (new Int256(1UL << 62, 0, 0, 0), true);
		yield return () => (Int256.NegativeOne, false);
		yield return () => (-new Int256(0, 0, 0, 3), false);
		yield return () => (-new Int256(0, 0, 0, 4), false);
		yield return () => (-new Int256(0, 0, 0, 6), false);
		yield return () => (-new Int256(0, 0, 0, 8), false);
	}

	public static IEnumerable<Func<(Int256, Int256)>> Log2TestData()
	{
		yield return () => (new Int256(0, 0, 0, 1), new Int256(0, 0, 0, 0));
		yield return () => (new Int256(0, 0, 0, 2), new Int256(0, 0, 0, 1));
		yield return () => (new Int256(0, 0, 0, 4), new Int256(0, 0, 0, 2));
		yield return () => (new Int256(0, 0, 0, 8), new Int256(0, 0, 0, 3));
		yield return () => (new Int256(0, 0, 0, 1UL << 63), new Int256(0, 0, 0, 63));
		yield return () => (new Int256(0, 0, 1UL << 5, 0), new Int256(0, 0, 0, 69));
		yield return () => (new Int256(0, 1UL << 42, 0, 0), new Int256(0, 0, 0, 170));
		yield return () => (new Int256(1UL << 13, 0, 0, 0), new Int256(0, 0, 0, 205));
		yield return () => (new Int256(0, 0, 0, 0), new Int256(0, 0, 0, 0));
	}

	public static IEnumerable<Func<(Int256, Int256, Pair<Int256>)>> DivRemTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 0, 10), (new Int256(0, 0, 0, 0xFFFF_FFFF_FFFF_FFFF / 10), new Int256(0, 0, 0, 0xFFFF_FFFF_FFFF_FFFF % 10)));
		yield return () => (new Int256(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 0, 10), (new Int256(0, new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF) / 10), new Int256(0, new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF) % 10)));
		yield return () => (new Int256(0, 0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 1, 0), (new Int256(0, new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF) / new UInt128(1, 0)), new Int256(0, new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF) % new UInt128(1, 0))));
		yield return () => (new Int256(0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 0, 10), (new Int256(0x0000_0000_0000_0000, 0x1999_9999_9999_9999, 0x9999_9999_9999_9999, 0x9999_9999_9999_9999), new Int256(0, 0, 0, 5)));
		yield return () => (new Int256(0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 0, 1, 0), (new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF)));
		yield return () => (new Int256(0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0, 1, 0, 0), (new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF), new Int256(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
	}

	public static IEnumerable<Func<(Int256, Int256)>> LeadingZeroCountTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), new Int256(0, 0, 0, 256));
		yield return () => (new Int256(0, 0, 0, 1), new Int256(0, 0, 0, 255));
		yield return () => (new Int256(0, 0, 1, 0), new Int256(0, 0, 0, 191));
		yield return () => (new Int256(0, 1, 0, 0), new Int256(0, 0, 0, 127));
		yield return () => (new Int256(1, 0, 0, 0), new Int256(0, 0, 0, 63));
		yield return () => (new Int256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int256(0, 0, 0, 63));
		yield return () => (new Int256(0, 0, 0, 1UL << 63), new Int256(0, 0, 0, 192));
		yield return () => (new Int256(0, 0, 1UL << 63, 0), new Int256(0, 0, 0, 128));
		yield return () => (new Int256(0, 1UL << 63, 0, 0), new Int256(0, 0, 0, 64));
		yield return () => (new Int256(1UL << 63, 0, 0, 0), new Int256(0, 0, 0, 0));
		yield return () => (new Int256(1UL << 62, 0, 0, 0), new Int256(0, 0, 0, 1));
	}

	public static IEnumerable<Func<(Int256, Int256)>> PopCountTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), new Int256(0, 0, 0, 0));
		yield return () => (new Int256(0, 0, 0, 1), new Int256(0, 0, 0, 1));
		yield return () => (Int256.MaxValue, new Int256(0, 0, 0, 255));
		yield return () => (new Int256(ulong.MaxValue, 0, 0, 0), new Int256(0, 0, 0, 64));
		yield return () => (new Int256(0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA), new Int256(0, 0, 0, 128));
		yield return () => (new Int256(1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60), new Int256(0, 0, 0, 4));
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadBigEndianTestData()
	{
		yield return () => ([], true, Int256.Zero);
		yield return () => ([0x01], true, Int256.One);
		yield return () =>
		{
			byte[] array = new byte[32];
			Array.Fill(array, byte.MaxValue);
			return (array, false, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () =>
		{
			byte[] array = new byte[35];
			for (int i = 0; i < 35; i++)
				array[i] = byte.MaxValue;
			return (array, false, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () => ([0x12, 0x34], true, new Int256(0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[32];
			array[0] = 0x80;
			return (array, false, new Int256(1UL << 63, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadLittleEndianTestData()
	{
		yield return () => ([], true, Int256.Zero);
		yield return () => ([0x01], true, Int256.One);
		yield return () =>
		{
			byte[] array = new byte[32];
			Array.Fill(array, byte.MaxValue);
			return (array, false, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () =>
		{
			byte[] array = new byte[35];
			for (int i = 0; i < 35; i++)
				array[i] = byte.MaxValue;
			return (array, false, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () => ([0x34, 0x12], true, new Int256(0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[32];
			array[31] = 0x80;
			return (array, false, new Int256(1UL << 63, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateLeftTestData()
	{
		yield return () => (new Int256(1, 2, 3, 4), 0, new Int256(1, 2, 3, 4));
		yield return () => (new Int256(1, 2, 3, 4), 256, new Int256(1, 2, 3, 4));
		yield return () => (new Int256(0, 0, 0x8000_0000_0000_0000, 0), 64, new Int256(0, 0x8000_0000_0000_0000, 0, 0));
		yield return () => (new Int256(0x8000_0000_0000_0000, 0, 0, 0), 64, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new Int256(0, 0, 0x8000_0000_0000_0000, 0), 128, new Int256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new Int256(0x8000_0000_0000_0000, 0, 0, 0), 128, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateRightTestData()
	{
		yield return () => (new Int256(1, 2, 3, 4), 0, new Int256(1, 2, 3, 4));
		yield return () => (new Int256(1, 2, 3, 4), 256, new Int256(1, 2, 3, 4));
		yield return () => (new Int256(0, 0, 0x8000_0000_0000_0000, 0), 64, new Int256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new Int256(0, 0, 0, 0x8000_0000_0000_0000), 64, new Int256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new Int256(0, 0, 0x8000_0000_0000_0000, 0), 128, new Int256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new Int256(0x8000_0000_0000_0000, 0, 0, 0), 128, new Int256(0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(Int256, Int256)>> TrailingZeroCountTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), new Int256(0, 0, 0, 256));
		yield return () => (new Int256(0, 0, 0, 1), new Int256(0, 0, 0, 0));
		yield return () => (new Int256(0, 0, 8, 0), new Int256(0, 0, 0, 67));
		yield return () => (new Int256(0, 0x10, 0, 0), new Int256(0, 0, 0, 132));
		yield return () => (new Int256(0x200, 0, 0, 0), new Int256(0, 0, 0, 201));
	}

	public static IEnumerable<Func<(Int256, int)>> GetByteCountTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), Unsafe.SizeOf<Int256>());
	}

	public static IEnumerable<Func<(Int256, int)>> GetShortestBitLengthTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), 0);
		yield return () => (new Int256(0, 0, 0, 1), 1);
		yield return () => (new Int256(1, 0, 0, 0), 193);
		yield return () => (Int256.MaxValue, 255);
		yield return () => (Int256.MinValue, 256);
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteBigEndianTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), new byte[32], Unsafe.SizeOf<Int256>());
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 31; i++)
				buffer[i] = 0;

			buffer[31] = 1;
			
			return (new Int256(0, 0, 0, 1), buffer, Unsafe.SizeOf<Int256>());
		};
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 32; i++)
				buffer[i] = 0xFF;
			
			return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, Unsafe.SizeOf<Int256>());
		};
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteLittleEndianTestData()
	{
		yield return () => (new Int256(0, 0, 0, 0), new byte[32], Unsafe.SizeOf<Int256>());
		yield return () =>
		{
			var buffer = new byte[32];
			
			buffer[0] = 1;
			for (int i = 1; i < 32; i++)
				buffer[i] = 0;
			
			return (new Int256(0, 0, 0, 1), buffer, Unsafe.SizeOf<Int256>());
		};
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 32; i++)
				buffer[i] = 0xFF;
			
			return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, Unsafe.SizeOf<Int256>());
		};
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToCheckedByteTestData()
	{
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToSaturatingByteTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.MaxValue, byte.MaxValue);
		yield return () => (Int256.ByteMaxValue + Int256.One, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, byte)>> ConvertToTruncatingByteTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.NegativeOne, 0xFF);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.MaxValue, byte.MaxValue);
		yield return () => (Int256.ByteMaxValue + Int256.One, 0);
	}

	public static IEnumerable<Func<(Int256, ushort)>> ConvertToCheckedUInt16TestData()
	{
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt16MaxValue + Int256.One, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.NegativeOne, 0xFFFF);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt16MaxValue + Int256.One, 0);
	}

	public static IEnumerable<Func<(Int256, uint)>> ConvertToCheckedUInt32TestData()
	{
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, uint)>> ConvertToSaturatingUInt32TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt32MaxValue + Int256.One, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, uint)>> ConvertToTruncatingUInt32TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.NegativeOne, 0xFFFF_FFFF);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt32MaxValue + Int256.One, 0);
	}

	public static IEnumerable<Func<(Int256, ulong)>> ConvertToCheckedUInt64TestData()
	{
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Int256.MaxValue, ulong.MaxValue);
		yield return () => (Int256.UInt64MaxValue + Int256.One, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.NegativeOne, 0xFFFF_FFFF_FFFF_FFFF);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Int256.MaxValue, ulong.MaxValue);
		yield return () => (Int256.UInt64MaxValue + Int256.One, 0);
	}

	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		yield return () => (Int256.One, UInt128.One);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		yield return () => (Int256.MinValue, UInt128.Zero);
		yield return () => (Int256.One, UInt128.One);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (Int256.MaxValue, UInt128.MaxValue);
		yield return () => (Int256.UInt128MaxValue + Int256.One, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		yield return () => (Int256.MinValue, UInt128.Zero);
		yield return () => (Int256.NegativeOne, new UInt128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (Int256.One, UInt128.One);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (Int256.MaxValue, UInt128.MaxValue);
		yield return () => (Int256.UInt128MaxValue + Int256.One, UInt128.Zero);
	}

	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		yield return () => (Int256.One, UInt256.One);
		yield return () => (Int256.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt256.Int256MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		yield return () => (Int256.MinValue, UInt256.Zero);
		yield return () => (Int256.One, UInt256.One);
		yield return () => (Int256.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt256.Int256MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		yield return () => (Int256.MinValue, new UInt256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
		yield return () => (Int256.NegativeOne, UInt256.MaxValue);
		yield return () => (Int256.One, UInt256.One);
		yield return () => (Int256.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt256.Int256MaxValue);
	}

	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		yield return () => (Int256.One, UInt512.One);
		yield return () => (Int256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt512.Int256MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		yield return () => (Int256.MinValue, UInt512.Zero);
		yield return () => (Int256.One, UInt512.One);
		yield return () => (Int256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt512.Int256MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		yield return () => (Int256.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
		yield return () => (Int256.NegativeOne, UInt512.MaxValue);
		yield return () => (Int256.One, UInt512.One);
		yield return () => (Int256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (Int256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (Int256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (Int256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (Int256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (Int256.MaxValue, UInt512.Int256MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, nuint)>> ConvertToCheckedUIntPtrTestData()
	{
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UIntPtrMaxValue, nuint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, nuint)>> ConvertToSaturatingUIntPtrTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UIntPtrMaxValue, nuint.MaxValue);
		yield return () => (Int256.MaxValue, nuint.MaxValue);
		yield return () => (Int256.UIntPtrMaxValue + Int256.One, nuint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, nuint)>> ConvertToTruncatingUIntPtrTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.NegativeOne, nuint.MaxValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.ByteMaxValue, byte.MaxValue);
		yield return () => (Int256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (Int256.UInt32MaxValue, uint.MaxValue);
		yield return () => (Int256.UIntPtrMaxValue, nuint.MaxValue);
		yield return () => (Int256.MaxValue, nuint.MaxValue);
		yield return () => (Int256.UIntPtrMaxValue + Int256.One, 0);
	}

	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToCheckedSByteTestData()
	{
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		yield return () => (Int256.MinValue, sbyte.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.MaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.MaxValue, -1);
	}

	public static IEnumerable<Func<(Int256, short)>> ConvertToCheckedInt16TestData()
	{
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, short)>> ConvertToSaturatingInt16TestData()
	{
		yield return () => (Int256.MinValue, short.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, short)>> ConvertToTruncatingInt16TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.MaxValue, -1);
	}

	public static IEnumerable<Func<(Int256, int)>> ConvertToCheckedInt32TestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, int)>> ConvertToSaturatingInt32TestData()
	{
		yield return () => (Int256.MinValue, int.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, int)>> ConvertToTruncatingInt32TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.MaxValue, -1);
	}

	public static IEnumerable<Func<(Int256, long)>> ConvertToCheckedInt64TestData()
	{
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, long)>> ConvertToSaturatingInt64TestData()
	{
		yield return () => (Int256.MinValue, long.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, long)>> ConvertToTruncatingInt64TestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.MaxValue, -1);
	}

	public static IEnumerable<Func<(Int256, Int128)>> ConvertToCheckedInt128TestData()
	{
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, Int128.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Int128)>> ConvertToSaturatingInt128TestData()
	{
		yield return () => (Int256.MinValue, Int128.MinValue);
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, Int128.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
		yield return () => (Int256.MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Int128)>> ConvertToTruncatingInt128TestData()
	{
		yield return () => (Int256.MinValue, Int128.Zero);
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, Int128.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
		yield return () => (Int256.MaxValue, Int128.NegativeOne);
	}

	public static IEnumerable<Func<(Int256, Int512)>> ConvertToCheckedInt512TestData()
	{
		yield return () => (Int256.MinValue, Int512.Int256MinValue);
		yield return () => (Int256.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Int256.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Int256.One, Int512.One);
		yield return () => (Int256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Int256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (Int256.MaxValue, Int512.Int256MaxValue);
		
		yield return () => (Int256.Parse("-465182250000"), Int512.Parse("-465182250000"));
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertToSaturatingInt512TestData()
	{
		yield return () => (Int256.MinValue, Int512.Int256MinValue);
		yield return () => (Int256.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Int256.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Int256.One, Int512.One);
		yield return () => (Int256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Int256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (Int256.MaxValue, Int512.Int256MaxValue);
		
		yield return () => (Int256.Parse("-465182250000"), Int512.Parse("-465182250000"));
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertToTruncatingInt512TestData()
	{
		yield return () => (Int256.MinValue, Int512.Int256MinValue);
		yield return () => (Int256.Int128MinValue, Int512.Int128MinValue);
		yield return () => (Int256.Int64MinValue, Int512.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Int512.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Int512.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Int512.SByteMinValue);
		yield return () => (Int256.One, Int512.One);
		yield return () => (Int256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (Int256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (Int256.MaxValue, Int512.Int256MaxValue);
		
		yield return () => (Int256.Parse("-465182250000"), Int512.Parse("-465182250000"));
	}
	
	public static IEnumerable<Func<(Int256, nint)>> ConvertToCheckedIntPtrTestData()
	{
		yield return () => (Int256.IntPtrMinValue, nint.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.IntPtrMaxValue, nint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, nint)>> ConvertToSaturatingIntPtrTestData()
	{
		yield return () => (Int256.MinValue, nint.MinValue);
		yield return () => (Int256.IntPtrMinValue, nint.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.IntPtrMaxValue, nint.MaxValue);
		yield return () => (Int256.MaxValue, nint.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, nint)>> ConvertToTruncatingIntPtrTestData()
	{
		yield return () => (Int256.MinValue, 0);
		yield return () => (Int256.IntPtrMinValue, nint.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.IntPtrMaxValue, nint.MaxValue);
		yield return () => (Int256.MaxValue, -1);
	}
	
	public static IEnumerable<Func<(Int256, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		yield return () => (Int256.MinValue, BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"));
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, BigInteger.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
		yield return () => (Int256.MaxValue, BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"));
	}

	public static IEnumerable<Func<(Int256, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		yield return () => (Int256.MinValue, BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"));
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, BigInteger.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
		yield return () => (Int256.MaxValue, BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"));
	}

	public static IEnumerable<Func<(Int256, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		yield return () => (Int256.MinValue, BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"));
		yield return () => (Int256.Int128MinValue, Int128.MinValue);
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, BigInteger.One);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		yield return () => (Int256.Int128MaxValue, Int128.MaxValue);
		yield return () => (Int256.MaxValue, BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"));
	}

	public static IEnumerable<Func<(Int256, Half)>> ConvertToCheckedHalfTestData()
	{
		yield return () => (Int256.Int16MinValue, (Half)short.MinValue);
		yield return () => (Int256.SByteMinValue, (Half)sbyte.MinValue);
		yield return () => (Int256.One, Half.One);
		yield return () => (Int256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, (Half)short.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Half)>> ConvertToSaturatingHalfTestData()
	{
		yield return () => (Int256.MinValue, Half.NegativeInfinity);
		yield return () => (Int256.Int16MinValue, (Half)short.MinValue);
		yield return () => (Int256.SByteMinValue, (Half)sbyte.MinValue);
		yield return () => (Int256.One, Half.One);
		yield return () => (Int256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (Int256.MaxValue, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(Int256, Half)>> ConvertToTruncatingHalfTestData()
	{
		yield return () => (Int256.MinValue, Half.NegativeInfinity);
		yield return () => (Int256.Int16MinValue, (Half)short.MinValue);
		yield return () => (Int256.SByteMinValue, (Half)sbyte.MinValue);
		yield return () => (Int256.One, Half.One);
		yield return () => (Int256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (Int256.MaxValue, Half.PositiveInfinity);
	}

	public static IEnumerable<Func<(Int256, float)>> ConvertToCheckedSingleTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, float)>> ConvertToSaturatingSingleTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, float)>> ConvertToTruncatingSingleTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}

	public static IEnumerable<Func<(Int256, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1d);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		
		yield return () => (Int256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int256.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
	}
	
	public static IEnumerable<Func<(Int256, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1d);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		
		yield return () => (Int256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int256.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
	}
	
	public static IEnumerable<Func<(Int256, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (Int256.Int64MinValue, long.MinValue);
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1d);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
		yield return () => (Int256.Int64MaxValue, long.MaxValue);
		
		yield return () => (Int256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int256.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
	}

	public static IEnumerable<Func<(Int256, Quad)>> ConvertToCheckedQuadTestData()
	{
		yield return () => (Int256.Int64MinValue, Quad.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Quad.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Quad.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Quad.SByteMinValue);
		yield return () => (Int256.One, Quad.One);
		yield return () => (Int256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Quad.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertToSaturatingQuadTestData()
	{
		yield return () => (Int256.Int64MinValue, Quad.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Quad.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Quad.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Quad.SByteMinValue);
		yield return () => (Int256.One, Quad.One);
		yield return () => (Int256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Quad.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Quad)>> ConvertToTruncatingQuadTestData()
	{
		yield return () => (Int256.Int64MinValue, Quad.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Quad.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Quad.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Quad.SByteMinValue);
		yield return () => (Int256.One, Quad.One);
		yield return () => (Int256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Quad.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int256, Octo)>> ConvertToCheckedOctoTestData()
	{
		yield return () => (Int256.Int64MinValue, Octo.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Octo.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Octo.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Octo.SByteMinValue);
		yield return () => (Int256.One, Octo.One);
		yield return () => (Int256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Octo.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertToSaturatingOctoTestData()
	{
		yield return () => (Int256.Int64MinValue, Octo.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Octo.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Octo.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Octo.SByteMinValue);
		yield return () => (Int256.One, Octo.One);
		yield return () => (Int256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Octo.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, Octo)>> ConvertToTruncatingOctoTestData()
	{
		yield return () => (Int256.Int64MinValue, Octo.Int64MinValue);
		yield return () => (Int256.Int32MinValue, Octo.Int32MinValue);
		yield return () => (Int256.Int16MinValue, Octo.Int16MinValue);
		yield return () => (Int256.SByteMinValue, Octo.SByteMinValue);
		yield return () => (Int256.One, Octo.One);
		yield return () => (Int256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (Int256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (Int256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (Int256.Int64MaxValue, Octo.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int256, NFloat)>> ConvertToCheckedNFloatTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, NFloat)>> ConvertToSaturatingNFloatTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(Int256, NFloat)>> ConvertToTruncatingNFloatTestData()
	{
		yield return () => (Int256.Int32MinValue, int.MinValue);
		yield return () => (Int256.Int16MinValue, short.MinValue);
		yield return () => (Int256.SByteMinValue, sbyte.MinValue);
		yield return () => (Int256.One, 1f);
		yield return () => (Int256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (Int256.Int16MaxValue, short.MaxValue);
		yield return () => (Int256.Int32MaxValue, int.MaxValue);
	}

	public static IEnumerable<Func<(byte, Int256)>> ConvertFromCheckedByteTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Int256)>> ConvertFromSaturatingByteTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, Int256)>> ConvertFromTruncatingByteTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
	}

	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromCheckedUInt16TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromSaturatingUInt16TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, Int256)>> ConvertFromTruncatingUInt16TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}

	public static IEnumerable<Func<(uint, Int256)>> ConvertFromCheckedUInt32TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Int256)>> ConvertFromSaturatingUInt32TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, Int256)>> ConvertFromTruncatingUInt32TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}

	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromCheckedUInt64TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromSaturatingUInt64TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, Int256)>> ConvertFromTruncatingUInt64TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromCheckedUInt128TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Int256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromSaturatingUInt128TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Int256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, Int256)>> ConvertFromTruncatingUInt128TestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, Int256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, Int256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(nuint, Int256)>> ConvertFromCheckedUIntPtrTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (nuint.MaxValue, Int256.UIntPtrMaxValue);
	}
	
	public static IEnumerable<Func<(nuint, Int256)>> ConvertFromSaturatingUIntPtrTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (nuint.MaxValue, Int256.UIntPtrMaxValue);
	}
	
	public static IEnumerable<Func<(nuint, Int256)>> ConvertFromTruncatingUIntPtrTestData()
	{
		yield return () => (1, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
		yield return () => (nuint.MaxValue, Int256.UIntPtrMaxValue);
	}

	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromCheckedSByteTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromSaturatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, Int256)>> ConvertFromTruncatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}

	public static IEnumerable<Func<(short, Int256)>> ConvertFromCheckedInt16TestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Int256)>> ConvertFromSaturatingInt16TestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, Int256)>> ConvertFromTruncatingInt16TestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
	}

	public static IEnumerable<Func<(int, Int256)>> ConvertFromCheckedInt32TestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Int256)>> ConvertFromSaturatingInt32TestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, Int256)>> ConvertFromTruncatingInt32TestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
	}

	public static IEnumerable<Func<(long, Int256)>> ConvertFromCheckedInt64TestData()
	{
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Int256)>> ConvertFromSaturatingInt64TestData()
	{
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, Int256)>> ConvertFromTruncatingInt64TestData()
	{
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromCheckedInt128TestData()
	{
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (Int128.One, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromSaturatingInt128TestData()
	{
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, Int256)>> ConvertFromTruncatingInt128TestData()
	{
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(nint, Int256)>> ConvertFromCheckedIntPtrTestData()
	{
		yield return () => (nint.MinValue, Int256.IntPtrMinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (nint.MaxValue, Int256.IntPtrMaxValue);
	}
	
	public static IEnumerable<Func<(nint, Int256)>> ConvertFromSaturatingIntPtrTestData()
	{
		yield return () => (nint.MinValue, Int256.IntPtrMinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (nint.MaxValue, Int256.IntPtrMaxValue);
	}
	
	public static IEnumerable<Func<(nint, Int256)>> ConvertFromTruncatingIntPtrTestData()
	{
		yield return () => (nint.MinValue, Int256.IntPtrMinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (nint.MaxValue, Int256.IntPtrMaxValue);
	}
	
	public static IEnumerable<Func<(BigInteger, Int256)>> ConvertFromCheckedBigIntegerTestData()
	{
		yield return () => (BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"), Int256.MinValue);
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (BigInteger.One, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
		yield return () => (BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"), Int256.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Int256)>> ConvertFromSaturatingBigIntegerTestData()
	{
		yield return () => (BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819969"), Int256.MinValue);
		yield return () => (BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"), Int256.MinValue);
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (BigInteger.One, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
		yield return () => (BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"), Int256.MaxValue);
		yield return () => (BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"), Int256.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, Int256)>> ConvertFromTruncatingBigIntegerTestData()
	{
		yield return () => (BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819969"), Int256.MaxValue);
		yield return () => (BigInteger.Parse("-57896044618658097711785492504343953926634992332820282019728792003956564819968"), Int256.MinValue);
		yield return () => (Int128.MinValue, Int256.Int128MinValue);
		yield return () => (long.MinValue, Int256.Int64MinValue);
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (BigInteger.One, Int256.One);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (long.MaxValue, Int256.Int64MaxValue);
		yield return () => (Int128.MaxValue, Int256.Int128MaxValue);
		yield return () => (BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967"), Int256.MaxValue);
		yield return () => (BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819968"), Int256.MinValue);
	}

	public static IEnumerable<Func<(Half, Int256)>> ConvertFromCheckedHalfTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (Half.One, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, Int256)>> ConvertFromSaturatingHalfTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (Half.One, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, Int256)>> ConvertFromTruncatingHalfTestData()
	{
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (Half.One, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
	}

	public static IEnumerable<Func<(float, Int256)>> ConvertFromCheckedSingleTestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, Int256)>> ConvertFromSaturatingSingleTestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, Int256)>> ConvertFromTruncatingSingleTestData()
	{
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
	}

	public static IEnumerable<Func<(double, Int256)>> ConvertFromCheckedDoubleTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1d, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, Int256)>> ConvertFromSaturatingDoubleTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1d, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, Int256)>> ConvertFromTruncatingDoubleTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1d, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(NFloat, Int256)>> ConvertFromCheckedNFloatTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(NFloat, Int256)>> ConvertFromSaturatingNFloatTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(NFloat, Int256)>> ConvertFromTruncatingNFloatTestData()
	{
		yield return () => (int.MinValue, Int256.Int32MinValue);
		yield return () => (short.MinValue, Int256.Int16MinValue);
		yield return () => (sbyte.MinValue, Int256.SByteMinValue);
		yield return () => (1f, Int256.One);
		yield return () => (byte.MaxValue, Int256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, Int256.SByteMaxValue);
		yield return () => (short.MaxValue, Int256.Int16MaxValue);
		yield return () => (ushort.MaxValue, Int256.UInt16MaxValue);
		yield return () => (int.MaxValue, Int256.Int32MaxValue);
		yield return () => (uint.MaxValue, Int256.UInt32MaxValue);
	}
}
