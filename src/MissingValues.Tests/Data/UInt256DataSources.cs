using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class UInt256DataSources
	: IMathOperatorsDataSource<UInt256>,
	IShiftOperatorsDataSource<UInt256>,
	IBitwiseOperatorsDataSource<UInt256>,
	IEqualityOperatorsDataSource<UInt256>, 
	IComparisonOperatorsDataSource<UInt256>
{
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_AdditionTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, UInt256.One, UInt256.One);
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 2), new UInt256(0, 0, 0, 3));
		yield return () => (new UInt256(0, 0, 1, ulong.MaxValue), new UInt256(0, 0, 1, 1), new UInt256(0, 0, 3, 0));
		yield return () => (new UInt256(0, 1, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 1, 1, 1), new UInt256(0, 3, 1, 0));
		yield return () => (new UInt256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, 1, 1, 1), new UInt256(3, 1, 1, 0));
		yield return () => (UInt256.MaxValue, UInt256.One, UInt256.Zero);
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256, bool)>> op_CheckedAdditionTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero, false);
		yield return () => (UInt256.Zero, UInt256.One, UInt256.One, false);
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 2), new UInt256(0, 0, 0, 3), false);
		yield return () => (new UInt256(0, 0, 1, ulong.MaxValue), new UInt256(0, 0, 1, 1), new UInt256(0, 0, 3, 0), false);
		yield return () => (new UInt256(0, 1, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 1, 1, 1), new UInt256(0, 3, 1, 0), false);
		yield return () => (new UInt256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, 1, 1, 1), new UInt256(3, 1, 1, 0), false);
		yield return () => (UInt256.MaxValue, UInt256.One, UInt256.Zero, true);
		yield return () => (new UInt256(ulong.MaxValue, 0, 0, 0), new UInt256(1, 0, 0, 0), UInt256.Zero, true);
	}
	public static IEnumerable<Func<(UInt256, UInt256)>> op_IncrementTestData()
	{
		yield return () => (UInt256.Zero, UInt256.One);
		yield return () => (new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, 0, 0, 0));
		yield return () => (UInt256.MaxValue, UInt256.Zero);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_CheckedIncrementTestData()
	{
		yield return () => (UInt256.Zero, UInt256.One, false);
		yield return () => (new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, 0, 0, 0), false);
		yield return () => (UInt256.MaxValue, UInt256.Zero, true);
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_SubtractionTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.Zero, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), UInt256.Zero);
		yield return () => (new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 3), new UInt256(0, 0, 0, 2));
		yield return () => (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, ulong.MaxValue));
		yield return () => (new UInt256(1, 0, 0, 0), UInt256.One, new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (UInt256.Zero, UInt256.One, UInt256.MaxValue);
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256, bool)>> op_CheckedSubtractionTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero, false);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.Zero, new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), UInt256.Zero, false);
		yield return () => (new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 3), new UInt256(0, 0, 0, 2), false);
		yield return () => (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, ulong.MaxValue), false);
		yield return () => (new UInt256(1, 0, 0, 0), UInt256.One, new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (UInt256.Zero, UInt256.One, UInt256.MaxValue, true);
	}
	public static IEnumerable<Func<(UInt256, UInt256)>> op_DecrementTestData()
	{
		yield return () => (UInt256.One, UInt256.Zero);
		yield return () => (new UInt256(1, 0, 0, 0), new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (UInt256.Zero, UInt256.MaxValue);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_CheckedDecrementTestData()
	{
		yield return () => (UInt256.One, UInt256.Zero, false);
		yield return () => (new UInt256(1, 0, 0, 0), new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), false);
		yield return () => (UInt256.Zero, UInt256.MaxValue, true);
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_MultiplyTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, new UInt256(1, 2, 3, 4), UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.One, new UInt256(1, 2, 3, 4));
		yield return () => (UInt256.One, new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0, 3), new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 15));
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, ulong.MaxValue - 1, 1));
		yield return () => (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 10, 0));
		yield return () => (new UInt256(1, 1, 1, 1), new UInt256(2, 2, 2, 2), new UInt256(8, 6, 4, 2));
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256, bool)>> op_CheckedMultiplyTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero, false);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.Zero, UInt256.Zero, false);
		yield return () => (UInt256.Zero, new UInt256(1, 2, 3, 4), UInt256.Zero, false);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.One, new UInt256(1, 2, 3, 4), false);
		yield return () => (UInt256.One, new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(0, 0, 0, 3), new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 15), false);
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, ulong.MaxValue - 1, 1), false);
		yield return () => (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 10, 0), false);
		yield return () => (new UInt256(1, 1, 1, 1), new UInt256(2, 2, 2, 2), new UInt256(8, 6, 4, 2), true);
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_DivisionTestData()
	{
		yield return () => (UInt256.One, UInt256.Zero, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.One, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), UInt256.One);
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(1, 0, 0, 0), UInt256.Zero);
		yield return () => (new UInt256(0, 0, 0, 1000), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 100));
		yield return () => (new UInt256(0x0, 0x1, 0x0, 0x0), new UInt256(0x0, 0x0, 0x0, 0x2), new UInt256(0x0, 0x0, 0x8000_0000_0000_0000, 0x0));
		yield return () => (new UInt256(0x0, 0x1, 0x2, 0x3), new UInt256(0x0, 0x0, 0x0, 0x10), new UInt256(0x0, 0x0, 0x1000_0000_0000_0000, 0x2000_0000_0000_0000));
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_ModulusTestData()
	{
		yield return () => (UInt256.One, UInt256.Zero, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.One, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), UInt256.Zero);
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(1, 0, 0, 0), new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 1000), new UInt256(0, 0, 0, 10), UInt256.Zero);
		yield return () => (new UInt256(0, 0, 0, 1234), new UInt256(0, 0, 0, 1000), new UInt256(0, 0, 0, 234));
		yield return () => (new UInt256(0x2, 0x0, 0x0, 0x0), new UInt256(0x0, 0x1, 0x0, 0x0), UInt256.Zero);
		yield return () => (new UInt256(0x0, 0x1, 0x2, 0x3), new UInt256(0x0, 0x0, 0x0, 0x10), new UInt256(0x0, 0x0, 0x0, 0x3));
	}
	public static IEnumerable<Func<(UInt256, UInt256)>> op_OnesComplementTestData()
	{
		yield return () => (UInt256.Zero, UInt256.MaxValue);
		yield return () => (UInt256.MaxValue, UInt256.Zero);
		yield return () => (new UInt256(0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555), new UInt256(0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA));
		yield return () => (new UInt256(0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new UInt256(~0x0123456789ABCDEFU, ~0xFEDCBA9876543210U, ~0x0F0F0F0F0F0F0F0FU, ~0xF0F0F0F0F0F0F0F0U));
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_BitwiseAndTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, UInt256.MaxValue, UInt256.Zero);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.MaxValue, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0xFFFFFFFF00000000, 0xAAAAAAAA55555555, 0x123456789ABCDEF0, 0x0F0F0F0F0F0F0F0F), new UInt256(0x00000000FFFFFFFF, 0x55555555AAAAAAAA, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new UInt256(0x0000000000000000, 0x0000000000000000, 0x020406080A0C0E00, 0x0000000000000000));
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_BitwiseOrTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (new UInt256(0x00000000FFFFFFFF, 0xAAAAAAAA00000000, 0x00000000AAAAAAAA, 0x1234567890ABCDEF), new UInt256(0xFFFFFFFF00000000, 0x0000000055555555, 0x5555555500000000, 0xFEDCBA9876543210), new UInt256(0xFFFFFFFFFFFFFFFF, 0xAAAAAAAA55555555, 0x55555555AAAAAAAA, 0xFEFCFEF8F6FFFFFF));
	}
	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> op_BitwiseXorTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), UInt256.Zero);
		yield return () => (new UInt256(0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x1234567890ABCDEF, 0xFEDCBA9876543210), UInt256.MaxValue, new UInt256(ulong.MaxValue, 0x0, 0xEDCBA9876F543210, 0x0123456789ABCDEF));
		yield return () => (new UInt256(0x1234567890ABCDEF, 0xAAAAAAAA00000000, 0x00000000AAAAAAAA, 0xFFFFFFFFFFFFFFFF), new UInt256(0xFFFFFFFFFFFFFFFF, 0x00000000AAAAAAAA, 0xAAAAAAAA00000000, 0x0000000000000000), new UInt256(0xEDCBA9876F543210, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xFFFFFFFFFFFFFFFF));
	}
	public static IEnumerable<Func<(UInt256, int, UInt256)>> op_ShiftLeftTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), 0, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0, 1), 1, new UInt256(0, 0, 0, 2));
		yield return () => (new UInt256(0, 0, 0, 1), 63, new UInt256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt256(0, 0, 0, 1), 64, new UInt256(0, 0, 1, 0));
		yield return () => (new UInt256(0, 0, 0, 1), 65, new UInt256(0, 0, 2, 0));
		yield return () => (new UInt256(0, 0, 0, 1), 128, new UInt256(0, 1, 0, 0));
		yield return () => (new UInt256(0, 0, 0, 1), 192, new UInt256(1, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 0, 1), 255, new UInt256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 0, 1), 256, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 1), 260, new UInt256(0, 0, 0, 16));
	}
	public static IEnumerable<Func<(UInt256, int, UInt256)>> op_ShiftRightTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), 0, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0, 16), 1, new UInt256(0, 0, 0, 8));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 63, new UInt256(1, 0, 0, 0));
		yield return () => (new UInt256(1, 2, 3, 4), 64, new UInt256(0, 1, 2, 3));
		yield return () => (new UInt256(1, 0, 0, 0), 65, new UInt256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => (new UInt256(1, 0, 0, 0), 128, new UInt256(0, 0, 1, 0));
		yield return () => (new UInt256(1, 0, 0, 0), 192, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 255, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 1), 256, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 16), 260, new UInt256(0, 0, 0, 1));
	}
	public static IEnumerable<Func<(UInt256, int, UInt256)>> op_UnsignedShiftRightTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), 0, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0, 16), 1, new UInt256(0, 0, 0, 8));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 63, new UInt256(1, 0, 0, 0));
		yield return () => (new UInt256(1, 2, 3, 4), 64, new UInt256(0, 1, 2, 3));
		yield return () => (new UInt256(1, 0, 0, 0), 65, new UInt256(0, 0, 0x8000_0000_0000_0000, 0));
		yield return () => (new UInt256(1, 0, 0, 0), 128, new UInt256(0, 0, 1, 0));
		yield return () => (new UInt256(1, 0, 0, 0), 192, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 255, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 1), 256, new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 16), 260, new UInt256(0, 0, 0, 1));
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_EqualityTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, true);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 5), false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 4, 4), false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 3, 3, 4), false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(2, 2, 3, 4), false);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_InequalityTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 5), true);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 4, 4), true);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 3, 3, 4), true);
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(2, 2, 3, 4), true);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_LessThanTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(0, 9, 9, 9), new UInt256(1, 0, 0, 0), true);
		yield return () => (new UInt256(2, 0, 0, 0), new UInt256(1, 9, 9, 9), false);
		yield return () => (new UInt256(1, 1, 3, 4), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(1, 2, 1, 4), new UInt256(1, 2, 2, 4), true);
		yield return () => (new UInt256(1, 2, 3, 3), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(1, 2, 3, 5), new UInt256(1, 2, 3, 4), false);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_LessThanOrEqualTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(0, 9, 9, 9), new UInt256(1, 0, 0, 0), true);
		yield return () => (new UInt256(2, 0, 0, 0), new UInt256(1, 9, 9, 9), false);
		yield return () => (new UInt256(1, 1, 3, 4), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(1, 2, 1, 4), new UInt256(1, 2, 2, 4), true);
		yield return () => (new UInt256(1, 2, 3, 3), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(1, 2, 3, 5), new UInt256(1, 2, 3, 4), false);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_GreaterThanTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(0, 9, 9, 9), new UInt256(1, 0, 0, 0), false);
		yield return () => (new UInt256(2, 0, 0, 0), new UInt256(1, 9, 9, 9), true);
		yield return () => (new UInt256(1, 1, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 1, 4), new UInt256(1, 2, 2, 4), false);
		yield return () => (new UInt256(1, 2, 3, 3), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 3, 5), new UInt256(1, 2, 3, 4), true);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_GreaterThanOrEqualTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), true);
		yield return () => (new UInt256(0, 9, 9, 9), new UInt256(1, 0, 0, 0), false);
		yield return () => (new UInt256(2, 0, 0, 0), new UInt256(1, 9, 9, 9), true);
		yield return () => (new UInt256(1, 1, 3, 4), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 1, 4), new UInt256(1, 2, 2, 4), false);
		yield return () => (new UInt256(1, 2, 3, 3), new UInt256(1, 2, 3, 4), false);
		yield return () => (new UInt256(1, 2, 3, 5), new UInt256(1, 2, 3, 4), true);
	}
}
