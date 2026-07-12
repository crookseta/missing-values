using System.Globalization;
using System.Runtime.CompilerServices;
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
			Int512.MaxValue,
			false
		);
		yield return () => (
			Int512.MaxValue, 
			new Int512(0, 0, 0, 0, 0, 0, 0, 2), 
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
		yield return () => (Int512.MaxValue, new Int512(0, 0, 0, 0, 0, 0, 0, 123456789), new Int512(0, 0, 0, 0, 0, 0, 0, 29802988));
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

	public static IEnumerable<Func<(Int512, Int512)>> op_UnaryNegationTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero);
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedUnaryNegationTestData()
	{
		yield return () => (Int512.Zero, Int512.Zero, false);
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
		yield return () => (Int512.One, 512, Int512.One);
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
		yield return () => (Int512.Zero, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (Int512.MaxValue, new Int512(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
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
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048", NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.NegativeOne);
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656", NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MaxValue);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000", NumberStyles.Number, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("1,000.0", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00", NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000", NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048", NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00", NumberStyles.Currency, Helper.CustomInfo, Int512.MinValue);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Int512)>> ParseSpanTestData()
	{
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.NegativeOne);
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MaxValue);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("1,000.0".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000".ToCharArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048".ToCharArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, Int512.MinValue);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Int512)>> ParseUtf8TestData()
	{
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int128.MinValue);
		yield return () => ("-9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, long.MinValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.NegativeOne);
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, Int512.MaxValue);
		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		
		yield return () => ("2.5E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 25_000_000_000);
		yield return () => ("1E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, 10_000_000_000);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, Int512.One);
		yield return () => ("1,000.0"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000);
		yield return () => ("1,000,000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000);
		yield return () => ("1,000,000,000.00"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000"u8.ToArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048"u8.ToArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, Int512.MinValue);
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int512)>> TryParseTestData()
	{
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042049", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808", NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.NegativeOne);
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047", NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MaxValue);
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10", NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000", NumberStyles.Number, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("1,000.0", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00", NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000", NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048", NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00", NumberStyles.Currency, Helper.CustomInfo, true, Int512.MinValue);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int512)>> TryParseSpanTestData()
	{
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042049".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.NegativeOne);
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MaxValue);
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10".ToCharArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("1,000.0".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00".ToCharArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000".ToCharArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048".ToCharArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00".ToCharArray(), NumberStyles.Currency, Helper.CustomInfo, true, Int512.MinValue);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int512)>> TryParseUtf8TestData()
	{
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042049"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MinValue);
		yield return () => ("-57896044618658097711785492504343953926634992332820282019728792003956564819968"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int256.MinValue);
		yield return () => ("-170141183460469231731687303715884105728"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int128.MinValue);
		yield return () => ("-9223372036854775808"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, long.MinValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.NegativeOne);
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new Int512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042047"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, Int512.MaxValue);
		yield return () => ("6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, true, new Int512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, true, new Int512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, false, default);
		
		yield return () => ("1E200"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.5E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 25_000_000_000);
		yield return () => ("1E10"u8.ToArray(), NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, true, 10_000_000_000);
		yield return () => ("1.000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, Int512.One);
		yield return () => ("1,000.0"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000);
		yield return () => ("1,000,000"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000);
		yield return () => ("1,000,000,000.00"u8.ToArray(), NumberStyles.Number, CultureInfo.InvariantCulture, true, 1_000_000_000);
		yield return () => ("-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000"u8.ToArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048"u8.ToArray(), NumberStyles.Number, NumberFormatInfo.InvariantInfo, true, Int512.MinValue);
		yield return () => ("$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00"u8.ToArray(), NumberStyles.Currency, Helper.CustomInfo, true, Int512.MinValue);
	}

	public static IEnumerable<Func<(Int512, string, IFormatProvider?, string)>> ToStringTestData()
	{
		yield return () => (Int256.MaxValue, "e25", CultureInfo.InvariantCulture, "5.7896044618658097711785493e+76");
		yield return () => (Int256.MinValue, "e25", CultureInfo.InvariantCulture, "-5.7896044618658097711785493e+76");
		yield return () => (Int512.MaxValue, "e25", CultureInfo.InvariantCulture, "6.7039039649712985497870125e+153");
		yield return () => (Int512.MinValue, "e25", CultureInfo.InvariantCulture, "-6.7039039649712985497870125e+153");
		yield return () => (Int512.MinValue, "F3", CultureInfo.InvariantCulture, "-6703903964971298549787012499102923063739682910296196688861780721860882015036773488400937149083451713845015929093243025426876941405973284973216824503042048.000");
		yield return () => (Int512.MinValue, "N", CultureInfo.InvariantCulture, "-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00");
		yield return () => (Int512.MinValue, "C", Helper.CustomInfo, "$-6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048.00");
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
		yield return () => (Int512.One, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.NegativeOne, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.NegativeOne, Int512.One, Int512.One);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxTestData()
	{
		yield return () => (Int512.One, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.One);
		yield return () => (Int512.MinValue, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.Zero, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.MaxValue, Int512.MaxValue);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinTestData()
	{
		yield return () => (Int512.One, Int512.One, Int512.One);
		yield return () => (Int512.One, Int512.NegativeOne, Int512.NegativeOne);
		yield return () => (Int512.MinValue, Int512.NegativeOne, Int512.MinValue);
		yield return () => (Int512.Zero, Int512.One, Int512.Zero);
		yield return () => (Int512.One, Int512.MaxValue, Int512.One);
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(Int512, int)>> SignTestData()
	{
		yield return () => (Int512.Zero, 0);
		yield return () => (Int512.MaxValue, 1);
		yield return () => (Int512.One, 1);
		yield return () => (Int512.MinValue, -1);
		yield return () => (Int512.NegativeOne, -1);
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPow2TestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 4), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 8), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 16), true);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1UL << 63), true);
		yield return () => (new Int512(0, 0, 0, 0, 1UL << 63, 0, 0, 0), true);
		yield return () => (new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), false);
		yield return () => (Int512.Zero, false);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 3), false);
		yield return () => (Int512.MaxValue, false);
	}

	public static IEnumerable<Func<(Int512, Int512)>> Log2TestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 2), new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 4), new Int512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 8), new Int512(0, 0, 0, 0, 0, 0, 0, 3));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1UL << 63), new Int512(0, 0, 0, 0, 0, 0, 0, 63));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1UL << 5, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 69));
		yield return () => (new Int512(0, 0, 0, 0, 0, 1UL << 42, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 170));
		yield return () => (new Int512(0, 0, 0, 0, 1UL << 13, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 205));
		yield return () => (new Int512(0, 0, 0, 0, 1UL << 63, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 255));
		yield return () => (new Int512(1UL << 62, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 510));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(Int512, Int512, Pair<Int512>)>> DivRemTestData()
	{
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(0, 0, 0, 0, 0, 0, 0, 1), (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 8), (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), new Int512(1, 2, 3, 4, 5, 6, 7, 9), (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new Int512(1, 2, 3, 4, 5, 6, 7, 8)));
		yield return () => (Int512.MaxValue, new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), (new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000), new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
		yield return () => (Int512.MaxValue, new Int512(0, 0, 0, 1, 0, 0, 0, 0), (new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new Int512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
	}

	public static IEnumerable<Func<(Int512, Int512)>> LeadingZeroCountTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 512));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 511));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 447));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1UL << 36, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 411));
		yield return () => (new Int512(0, 0, 0, 1, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 255));
		yield return () => (new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(Int512, Int512)>> PopCountTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (Int512.MaxValue, new Int512(0, 0, 0, 0, 0, 0, 0, 511));
		yield return () => (new Int512(ulong.MaxValue, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 64));
		yield return () => (new Int512(0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA), new Int512(0, 0, 0, 0, 0, 0, 0, 256));
		yield return () => (new Int512(0, 0, 0, 0, 1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60), new Int512(0, 0, 0, 0, 0, 0, 0, 4));
		yield return () => (new Int512(1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60, 1UL << 59, 1UL << 58, 1UL << 57, 1UL << 56), new Int512(0, 0, 0, 0, 0, 0, 0, 8));
	}

	public static IEnumerable<Func<(byte[], bool, Int512)>> ReadBigEndianTestData()
	{
		yield return () => ([], true, Int512.Zero);
		yield return () => ([0x01], true, Int512.One);
		yield return () =>
		{
			byte[] array = new byte[64];
			Array.Fill(array, byte.MaxValue);
			return (array, false, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () =>
		{
			byte[] array = new byte[67];
			for (int i = 0; i < 67; i++)
				array[i] = byte.MaxValue;
			return (array, false, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () => ([0x12, 0x34], true, new Int512(0, 0, 0, 0, 0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[64];
			array[0] = 0x80;
			return (array, false, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(byte[], bool, Int512)>> ReadLittleEndianTestData()
	{
		yield return () => ([], true, Int512.Zero);
		yield return () => ([0x01], true, Int512.One);
		yield return () =>
		{
			byte[] array = new byte[64];
			Array.Fill(array, byte.MaxValue);
			return (array, false, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () =>
		{
			byte[] array = new byte[67];
			for (int i = 0; i < 67; i++)
				array[i] = byte.MaxValue;
			return (array, false, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		};
		yield return () => ([0x34, 0x12], true, new Int512(0, 0, 0, 0, 0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[64];
			array[63] = 0x80;
			return (array, false, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> RotateLeftTestData()
	{
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 0, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 512, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0), 64, new Int512(0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0));
		yield return () => (new Int512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0), 64, new Int512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new Int512(0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0, 0), 128, new Int512(0, 0, 0x8000_0000_0000_0000, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> RotateRightTestData()
	{
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 0, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(1, 2, 3, 4, 5, 6, 7, 8), 512, new Int512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0), 64, new Int512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000), 64, new Int512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0, 0), 128, new Int512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(Int512, Int512)>> TrailingZeroCountTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 512));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), new Int512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 64));
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 1UL << 36, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 100));
		yield return () => (new Int512(0, 0, 0, 1, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 256));
		yield return () => (new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), new Int512(0, 0, 0, 0, 0, 0, 0, 511));
	}

	public static IEnumerable<Func<(Int512, int)>> GetByteCountTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), Unsafe.SizeOf<Int512>());
	}

	public static IEnumerable<Func<(Int512, int)>> GetShortestBitLengthTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), 0);
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 1), 1);
		yield return () => (new Int512(0, 0, 0, 0, 1, 0, 0, 0), 193);
		yield return () => (new Int512(1, 0, 0, 0, 0, 0, 0, 0), 449);
		yield return () => (Int512.MaxValue, 511);
		yield return () => (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), 1);
	}

	public static IEnumerable<Func<(Int512, byte[], int)>> WriteBigEndianTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], Unsafe.SizeOf<Int512>());
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 63; i++)
				buffer[i] = 0;

			buffer[63] = 1;
			
			return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer, Unsafe.SizeOf<Int512>());
		};
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 64; i++)
				buffer[i] = 0xFF;
			
			return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, Unsafe.SizeOf<Int512>());
		};
	}

	public static IEnumerable<Func<(Int512, byte[], int)>> WriteLittleEndianTestData()
	{
		yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], Unsafe.SizeOf<Int512>());
		yield return () =>
		{
			var buffer = new byte[64];
			
			buffer[0] = 1;
			for (int i = 1; i < 64; i++)
				buffer[i] = 0;
			
			return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer, Unsafe.SizeOf<Int512>());
		};
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 64; i++)
				buffer[i] = 0xFF;
			
			return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, Unsafe.SizeOf<UInt512>());
		};
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
