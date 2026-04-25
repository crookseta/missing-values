using System.Globalization;
using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class Int512DataSources
	: IMathOperatorsDataSource<Int512>,
	IShiftOperatorsDataSource<Int512>,
	IBitwiseOperatorsDataSource<Int512>,
	IEqualityOperatorsDataSource<Int512>,
	IComparisonOperatorsDataSource<Int512>,
	INumberBaseDataSource<Int512>,
	INumberDataSource<Int512>,
	IBinaryNumberDataSource<Int512>,
	IBinaryIntegerDataSource<Int512>
{
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_AdditionTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.One, Int512.Zero, Int512.One);
		yield return () => (Int512.One, Int512.One, new Int512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 1, 1), 
			new Int512(0, 0, 0, 0, 0, 0, 3, 0));
		yield return () => (
			new Int512(0, 0, 0, 1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(0, 0, 0, 1, 1, 1, 1, 1), 
			new Int512(0, 0, 0, 3, 1, 1, 1, 0));
		yield return () => (
			new Int512(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(1, 1, 1, 1, 1, 1, 1, 1), 
			new Int512(3, 1, 1, 1, 1, 1, 1, 0));
		yield return () => (Int512.MaxValue, Int512.One, Int512.MinValue);
		yield return () => (Int512.NegativeOne, Int512.One, Int512.Zero);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedAdditionTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero, false);
		yield return () => (Int512.One, Int512.Zero, Int512.One, false);
		yield return () => (Int512.One, Int512.One, new Int512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 1, 1), 
			new Int512(0, 0, 0, 0, 0, 0, 3, 0), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(0, 0, 0, 1, 1, 1, 1, 1), 
			new Int512(0, 0, 0, 3, 1, 1, 1, 0), 
			false);
		yield return () => (
			new Int512(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(1, 1, 1, 1, 1, 1, 1, 1), 
			new Int512(3, 1, 1, 1, 1, 1, 1, 0), 
			false);
		yield return () => (Int512.MaxValue, Int512.One, Int512.MinValue, true);
		yield return () => (Int512.NegativeOne, Int512.One, Int512.Zero, false);
		yield return () => (Int512.MinValue, Int512.NegativeOne, Int512.MaxValue, true);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedDecrementTestData()
	{
		yield return () => (Int512.Zero, Int512.NegativeOne, false);
		yield return () => (Int512.One, Int512.Zero, false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, 2), 
			new Int512(0, 0, 0, 0, 0, 0, 0, 1), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 0, 1, 0, 0, 0), 
			new Int512(0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 1, 0, 0, 0, 0), 
			new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			false);
		yield return () => (
			new Int512(1, 0, 0, 0, 0, 0, 0, 0), 
			new Int512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			false);
		yield return () => (Int512.MinValue, Int512.MaxValue, true);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedIncrementTestData()
	{
		yield return () => (Int512.Zero, Int512.One, false);
		yield return () => (Int512.One, new Int512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (Int512.MaxValue, Int512.Zero, true);
		yield return () => (Int512.NegativeOne, Int512.Zero, false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			false);
		yield return () => (
			new Int512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(1, 0, 0, 0, 0, 0, 0, 0), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(0, 0, 0, 1, 0, 0, 0, 0), 
			false);
		yield return () => (
			new Int512(unchecked((ulong)-123456789), 987654321, 555555555, 444444444, 333333333, 222222222, 111111111, 999999999), 
			new Int512(unchecked((ulong)-123456789), 987654321, 555555555, 444444444, 333333333, 222222222, 111111111, 1000000000), 
			false);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedMultiplyTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero, false);
		yield return () => (Int512.Zero, Int512.One, Int512.Zero, false);
		yield return () => (Int512.One, Int512.One, Int512.One, false);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne, false);
		yield return () => (Int512.NegativeOne, Int512.NegativeOne, Int512.One, false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), new Int512(0, 0, 0, 0, 0, 0, 0, 3), new Int512(0, 0, 0, 0, 0, 0, 0, 6), false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, 2), new Int256(0, 0, 1, ulong.MaxValue - 1), false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFE, 0x0000_0000_0000_0001),
			false
		);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			false
		);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 1, 0, 0), 
			new Int512(0, 0, 0, 0, 0, 1, 0, 0), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000),
			false
		);
		yield return () => (
			Int512.MaxValue, 
			new Int512(0, 0, 0, 0, 0, 0, 0, 1), 
			default,
			true
		);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedSubtractionTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero, false);
		yield return () => (Int512.One, Int512.Zero, Int512.One, false);
		yield return () => (Int512.One, Int512.One, Int512.Zero, false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, 2), 
			Int512.One, 
			Int512.One, 
			false);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0, 0, 0, 0, 0, 0, 0, 1), 
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			false);
		yield return () => (
			new Int512(0, 0, 0, 1, 0, 0, 0, 0), 
			new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 0, 1), 
			false);
		yield return () => (Int512.MinValue, Int512.One, Int512.MaxValue, true);
		yield return () => (Int512.MaxValue, Int512.NegativeOne, Int512.MinValue, true);
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_DecrementTestData()
	{
		yield return () => (Int512.Zero, Int512.NegativeOne);
		yield return () => (Int512.One, Int512.Zero);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue));
		yield return () => (new Int512(0, 0, 0, 0, 0, 1, 0, 0), new Int512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new Int512(0, 0, 0, 0, 1, 0, 0, 0), new Int512(0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new Int512(1, 0, 0, 0, 0, 0, 0, 0), new Int512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (Int512.MinValue, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_DivisionTestData()
	{
		yield return () => (Int512.Zero, Int512.One, Int512.Zero);
		yield return () => (Int512.One, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.NegativeOne, Int512.One, Int512.NegativeOne);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 4), new Int512(0, 0, 0, 0, 0, 0, 0, 2), new Int512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (Int512.MaxValue, Int512.One, Int512.MaxValue);
		yield return () => (Int512.MinValue, Int512.One, Int512.MinValue);
		yield return () => (Int512.Zero, Int512.MaxValue, Int256.Zero);
		yield return () => (Int512.MaxValue, Int512.MaxValue, Int512.One);
		yield return () => (Int512.MinValue, Int512.MinValue, Int512.One);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), new Int512(0, 0, 0, 0, 0, 1, 0, 0), Int512.Zero);
		yield return () => (
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000)
			);
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_IncrementTestData()
	{
		yield return () => (Int512.Zero, Int512.One);
		yield return () => (Int512.One, new Int512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (Int512.MaxValue, Int512.MinValue);
		yield return () => (Int512.NegativeOne, Int512.Zero);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => (
			new Int512(unchecked((ulong)-123456789), 987654321, 555555555, 444444444, 333333333, 222222222, 111111111, 999999999), 
			new Int512(unchecked((ulong)-123456789), 987654321, 555555555, 444444444, 333333333, 222222222, 111111111, 1000000000));
		yield return () => (new Int512(0, 0, 0, 0, 0x8000000000000000, 0, 0, 0),new Int512(0, 0, 0, 0, 0x8000000000000000, 0, 0, 1));
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_ModulusTestData()
	{
		yield return () => (Int512.Zero, Int512.One, Int512.Zero);
		yield return () => (Int512.One, Int512.One, Int512.Zero);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 123456789), Int512.One, Int512.Zero);
		yield return () => (Int512.MaxValue, Int512.MaxValue, Int512.Zero);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), new Int512(0, 0, 0, 0, 0, 1, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 10), new Int512(0, 0, 0, 0, 0, 0, 0, 3), new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 15), new Int512(0, 0, 0, 0, 0, 0, 0, 5), Int512.Zero);
		yield return () => (Int512.NegativeOne, new Int512(0, 0, 0, 0, 0, 0, 0, 2), Int512.NegativeOne);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 7), Int512.NegativeOne, Int512.Zero);
		yield return () => (Int512.MaxValue, new Int512(0, 0, 0, 0, 0, 0, 0, 123456789), new Int512(0, 0, 0, 0, 0, 0, 0, 77645365));
		yield return () => (new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0040, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_000F, 0xEE50_B702_5C36_A080, 0x2F23_6D04_753D_5B48, 0xE800_0000_0000_0000), new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x46BD_23F6_8F25_7DFF, 0x4372_4BEE_2B0A_92DC, 0x6000_0000_0000_0000));
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_MultiplyTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.Zero, Int512.One, Int512.Zero);
		yield return () => (Int512.One, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.NegativeOne, Int512.NegativeOne, Int512.One);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), new Int512(0, 0, 0, 0, 0, 0, 0, 3), new Int512(0, 0, 0, 0, 0, 0, 0, 6));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), new Int256(0, 0, 0, 2), new Int256(0, 0, 1, ulong.MaxValue - 1));
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			new Int512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFE, 0x0000_0000_0000_0001)
			);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0, 0, 0, 0, 0, 0, 1, 0), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)
			);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 1, 0, 0), 
			new Int512(0, 0, 0, 0, 0, 1, 0, 0), 
			new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000)
			);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_SubtractionTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.One, Int512.Zero, Int512.One);
		yield return () => (Int512.One, Int512.One, Int512.Zero);
		yield return () => (Int512.Zero, Int512.One, Int512.NegativeOne);
		yield return () => (
			Int512.MaxValue, 
			Int512.One, 
			new Int512(long.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue - 1));
		yield return () => (Int512.MinValue, Int512.One, Int512.MaxValue);
		yield return () => (
			Int512.MinValue, 
			Int512.NegativeOne, 
			new Int512(unchecked((ulong)long.MinValue), 0, 0, 0, 0, 0, 0, 1));
		yield return () => (
			new Int512(1, 2, 3, 4, 5, 6, 7, 8),
			new Int512(0, 1, 2, 3, 4, 5, 6, 7),
			new Int512(1, 1, 1, 1, 1, 1, 1, 1));
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, 0),
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(unchecked((ulong)-1), ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> op_ShiftLeftTestData()
	{
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 0, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 1, new Int512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 63, new Int512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 64, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 128, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 256, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 511, new Int512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 512, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 513, new Int512(0, 0, 0, 0, 0, 0, 0, 2));
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> op_ShiftRightTestData()
	{
		yield return () => (Int512.Zero, 100, Int512.Zero);
		yield return () => (Int512.One, 0, Int512.One);
		yield return () => (new Int512(1, 0, 0, 0, 0, 0, 0, 0), 64, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(1, 0, 0, 0, 0, 0, 0, 0), 128, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => (new Int512(1, 0, 0, 0, 0, 0, 0, 0), 192, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => (new Int512(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0, 0, 0, 0, 0), 031, new Int512(0b1111111111111111111111111111111100000000000000000000000000000000, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0, 0, 0, 0, 0), 127, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0, 0, 0, 0, 0), 255, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0, 0, 0));
		yield return () => (new Int512(0b1000000000000000000000000000000000000000000000000000000000000000, 0, 0, 0, 0, 0, 0, 0), 511, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => (Int512.One, 256, Int512.One);
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> op_UnsignedShiftRightTestData()
	{
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 0, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), 1, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000), 63, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), 64, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 1, 0, 0), 128, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 1, 0, 0, 0, 0), 256, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0), 511, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 512, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), 513, new Int512(0, 0, 0, 0, 0, 0, 0, 1));
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseAndTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.Zero, Int512.MaxValue, Int512.Zero);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), Int512.MaxValue, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseOrTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.Zero, Int512.MaxValue, Int512.MaxValue);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), Int512.MaxValue, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseXorTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, Int512.Zero);
		yield return () => (Int512.Zero, Int512.MaxValue, Int512.MaxValue);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), Int512.Zero);
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_OnesComplementTestData()
	{
		yield return () => (Int512.Zero, Int512.MaxValue);
		yield return () => (Int512.MaxValue, Int512.Zero);
		yield return () => (new Int512(0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555), new Int512(0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA));
		yield return () => (new Int512(0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0, 0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new Int512(~0x0123456789ABCDEFU, ~0xFEDCBA9876543210U, ~0x0F0F0F0F0F0F0F0FU, ~0xF0F0F0F0F0F0F0F0U, ~0x0123456789ABCDEFU, ~0xFEDCBA9876543210U, ~0x0F0F0F0F0F0F0F0FU, ~0xF0F0F0F0F0F0F0F0U));
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_EqualityTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_InequalityTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanOrEqualTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanOrEqualTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 6, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 9), new Int512(1, 2, 3, 4, 5, 6, 7, 1), false);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new Int512(1, 2, 2, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(2, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new Int512(1, 1, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(Int512, Int512)>> AbsTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero);
		yield return () => (Int512.One, Int512.One);
		yield return () => (Int512.NegativeOne, Int512.One);
		yield return () => (Int512.MinValue + Int512.One, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsCanonicalTestData()
	{
		yield return () => (Int512.Zero, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsComplexNumberTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsEvenIntegerTestData()
	{
		yield return () => (Int512.Zero, true);
		yield return () => (Int512.One, false);
		yield return () => (Int512.NegativeOne, false);
		yield return () => (new Int512(0, 0, 0, 2), true);
		yield return () => (new Int512(0, 0, 0, 3), false);
		yield return () => (new Int512(0, 0, 0, 4), true);
		yield return () => (new Int512(0, 0, 0, 6), true);
		yield return () => (new Int512(0, 0, 0, 8), true);
		yield return () => (new Int512(0, 0, 0, 16), true);
		yield return () => (-new Int512(0, 0, 0, 2), true);
		yield return () => (-new Int512(0, 0, 0, 3), false);
		yield return () => (-new Int512(0, 0, 0, 4), true);
		yield return () => (-new Int512(0, 0, 0, 6), true);
		yield return () => (-new Int512(0, 0, 0, 8), true);
		yield return () => (-new Int512(0, 0, 0, 16), true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsFiniteTestData()
	{
		yield return () => (Int512.Zero, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsImaginaryNumberTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsInfinityTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsIntegerTestData()
	{
		yield return () => (Int512.Zero, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNaNTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNegativeTestData()
	{
		yield return () => (Int512.Zero, false);
		yield return () => (Int512.One, false);
		yield return () => (Int512.MaxValue, false);
		yield return () => (Int512.NegativeOne, true);
		yield return () => (-Int512.One, true);
		yield return () => (-Int512.MaxValue, true);
		yield return () => (Int512.MinValue, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNegativeInfinityTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNormalTestData()
	{
		yield return () => (Int512.Zero, false);
		yield return () => (Int512.One, true);
		yield return () => (Int512.NegativeOne, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsOddIntegerTestData()
	{
		yield return () => (Int512.Zero, false);
		yield return () => (Int512.One, true);
		yield return () => (Int512.NegativeOne, true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 3), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 4), false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 6), false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 8), false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 16), false);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 3), true);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 4), false);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 6), false);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 8), false);
		yield return () => (-new Int512(0, 0, 0, 0, 0, 0, 0, 16), false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPositiveTestData()
	{
		yield return () => (Int512.Zero, true);
		yield return () => (Int512.One, true);
		yield return () => (Int512.MaxValue, true);
		yield return () => (Int512.NegativeOne, false);
		yield return () => (-Int512.One, false);
		yield return () => (-Int512.MaxValue, false);
		yield return () => (Int512.MinValue, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPositiveInfinityTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsRealNumberTestData()
	{
		yield return () => (Int512.Zero, true);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsSubnormalTestData()
	{
		yield return () => (Int512.Zero, false);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsZeroTestData()
	{
		yield return () => (Int512.Zero, true);
		yield return () => (Int512.One, false);
		yield return () => (Int512.NegativeOne, false);
		yield return () => (Int512.MaxValue, false);
		yield return () => (Int512.MinValue, false);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxMagnitudeTestData()
	{
		yield return () => (Int512.MaxValue, 5, Int512.MaxValue);
		yield return () => (Int512.One, 5, 5);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.One);
		yield return () => (Int512.One, -2, -2);
		yield return () => (Int512.NegativeOne, Int512.MaxValue, Int512.MaxValue);
		yield return () => (Int512.MinValue, -2, Int512.MinValue);
		yield return () => (Int512.MaxValue, Int512.MinValue, Int512.MinValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxMagnitudeNumberTestData()
	{
		yield return () => (Int512.MaxValue, 5, Int512.MaxValue);
		yield return () => (Int512.One, 5, 5);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.One);
		yield return () => (Int512.One, -2, -2);
		yield return () => (Int512.NegativeOne, Int512.MaxValue, Int512.MaxValue);
		yield return () => (Int512.MinValue, -2, Int512.MinValue);
		yield return () => (Int512.MaxValue, Int512.MinValue, Int512.MinValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinMagnitudeTestData()
	{
		yield return () => (Int512.MaxValue, 5, 5);
		yield return () => (Int512.One, 5, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.One, -2, Int512.One);
		yield return () => (Int512.NegativeOne, Int512.MaxValue, Int512.NegativeOne);
		yield return () => (Int512.MinValue, -2, -2);
		yield return () => (Int512.MaxValue, Int512.MinValue, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinMagnitudeNumberTestData()
	{
		yield return () => (Int512.MaxValue, 5, 5);
		yield return () => (Int512.One, 5, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.One, -2, Int512.One);
		yield return () => (Int512.NegativeOne, Int512.MaxValue, Int512.NegativeOne);
		yield return () => (Int512.MinValue, -2, -2);
		yield return () => (Int512.MaxValue, Int512.MinValue, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, Int512)>> MultiplyAddEstimateTestData()
	{
		yield return () => (Int512.One, Int512.One, Int512.One, 2);
		yield return () => (Int512.One, Int512.Zero, Int512.One, Int512.One);
		yield return () => (Int512.MaxValue, Int512.NegativeOne, Int512.NegativeOne, Int512.MinValue);
		yield return () => (200, 100, 500, 20500);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new Int512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new Int512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), Int512.Parse("115792089237316195423570985008687907852929702298719625575994209400481361428480"));
		yield return () => (new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int512(0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Int512)>> ParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Int512)>> ParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Int512)>> ParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int512)>> TryParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int512)>> TryParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int512)>> TryParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, Int512)>> ClampTestData()
	{
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(0, 0, 0, 0, 0, 0, 0, 2),
			new Int512(0, 0, 0, 0, 0, 0, 0, 4),
			new Int512(0, 0, 0, 0, 0, 0, 0, 2)
		);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(0, 0, 0, 0, 0, 0, 0, 4),
			new Int512(0, 0, 0, 0, 0, 0, 0, 1)
		);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 1, 0, 0),
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(0, 0, 0, 0, 1, 0, 0, 0),
			new Int512(0, 0, 0, 0, 0, 1, 0, 0)
		);
		yield return () => (
			new Int512(0, 0, 0, 0, 0, 1, 0, 0),
			new Int512(0, 0, 0, 0, 0, 0, 0, 1),
			new Int512(1, 0, 0, 0, 0, 0, 0, 0),
			new Int512(0, 0, 0, 0, 0, 1, 0, 0)
		);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> CopySignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int)>> SignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPow2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> Log2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Pair<Int512>)>> DivRemTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> LeadingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> PopCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int512)>> ReadBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int512)>> ReadLittleEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> RotateLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> RotateRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> TrailingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int)>> GetByteCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int)>> GetShortestBitLengthTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, byte[], int)>> WriteBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, byte[], int)>> WriteLittleEndianTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int256)>> ConvertToCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int256)>> ConvertToSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Int256)>> ConvertToTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (Int512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}
	
	public static IEnumerable<Func<(Int512, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (Int512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
		yield return () => (Int512.Parse("-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}
	
	public static IEnumerable<Func<(Int512, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (Int512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("-781377183594418599030564404241984000000000000000000"),
			-781377183594418599030564404241984000000000000000000.0d);
		yield return () => (Int512.Parse("693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
		yield return () => (Int512.Parse("-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			-693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}

	public static IEnumerable<Func<(Int512, Quad)>> ConvertToCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertToSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Quad)>> ConvertToTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Octo)>> ConvertToCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Octo)>> ConvertToSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, Octo)>> ConvertToTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, Int512)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Int512)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, Int512)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, Int512)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Int512)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, Int512)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, Int512)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Int512)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, Int512)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, Int512)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Int512)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, Int512)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, Int512)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Int512)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, Int512)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, Int512)>> ConvertFromCheckedUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Int512)>> ConvertFromSaturatingUInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, Int512)>> ConvertFromTruncatingUInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Int512)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertFromTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, Int512)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Int512)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, Int512)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, Int512)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Int512)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, Int512)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, Int512)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Int512)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, Int512)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, Int512)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Int512)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, Int512)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, Int512)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Int512)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, Int512)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int512)>> ConvertFromCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertFromSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, Int512)>> ConvertFromTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, Int512)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Int512)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, Int512)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, Int512)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Int512)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, Int512)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, Int512)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Int512)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, Int512)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, Int512)>> ConvertFromCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertFromSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, Int512)>> ConvertFromTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, Int512)>> ConvertFromCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertFromSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, Int512)>> ConvertFromTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}
}
