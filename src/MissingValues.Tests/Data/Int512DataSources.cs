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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> op_ShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, int, Int512)>> op_UnsignedShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseAndTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseOrTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseXorTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_OnesComplementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_EqualityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_InequalityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> AbsTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsCanonicalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsComplexNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsEvenIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsFiniteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsImaginaryNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNaNTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNegativeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNegativeInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsNormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsOddIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPositiveTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsPositiveInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsRealNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsSubnormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, bool)>> IsZeroTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MaxMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> MinMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, Int512)>> MultiplyAddEstimateTestData()
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
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

	public static IEnumerable<Func<(Int512, Int512, (Int512, Int512))>> DivRemTestData()
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
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
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
