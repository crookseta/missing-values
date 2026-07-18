using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using MissingValues.Tests.Data.Sources;
using MissingValues.Tests.Extensions;

namespace MissingValues.Tests.Data;

public class UInt256DataSources
	: IMathOperatorsDataSource<UInt256>,
	IShiftOperatorsDataSource<UInt256>,
	IBitwiseOperatorsDataSource<UInt256>,
	IEqualityOperatorsDataSource<UInt256>, 
	IComparisonOperatorsDataSource<UInt256>,
	INumberBaseDataSource<UInt256>,
	INumberDataSource<UInt256>,
	IBinaryNumberDataSource<UInt256>,
	IBinaryIntegerDataSource<UInt256>
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

	public static IEnumerable<Func<(UInt256, UInt256)>> op_UnaryNegationTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero);
	}
	public static IEnumerable<Func<(UInt256, UInt256, bool)>> op_CheckedUnaryNegationTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, false);
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

	public static IEnumerable<Func<(UInt256, UInt256)>> AbsTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.One, UInt256.One);
		yield return () => (UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (UInt256.MinValue, UInt256.MinValue);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsCanonicalTestData()
	{
		yield return () => (UInt256.Zero, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsComplexNumberTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsEvenIntegerTestData()
	{
		yield return () => (UInt256.Zero, true);
		yield return () => (UInt256.One, false);
		yield return () => (new UInt256(0, 0, 0, 2), true);
		yield return () => (new UInt256(0, 0, 0, 3), false);
		yield return () => (new UInt256(0, 0, 0, 0x8000_0000_0000_0000), true);
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), true);
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 1), false);
		yield return () => (UInt256.MaxValue, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsFiniteTestData()
	{
		yield return () => (UInt256.Zero, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsImaginaryNumberTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsInfinityTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsIntegerTestData()
	{
		yield return () => (UInt256.Zero, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsNaNTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsNegativeTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsNegativeInfinityTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsNormalTestData()
	{
		yield return () => (UInt256.Zero, false);
		yield return () => (UInt256.One, true);
		yield return () => (new UInt256(0, 0, 1, 0), true);
		yield return () => (new UInt256(0, 1, 0, 0), true);
		yield return () => (new UInt256(1, 0, 0, 0), true);
		yield return () => (UInt256.MaxValue, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsOddIntegerTestData()
	{
		yield return () => (UInt256.Zero, false);
		yield return () => (UInt256.One, true);
		yield return () => (new UInt256(0, 0, 0, 2), false);
		yield return () => (new UInt256(0, 0, 0, 3), true);
		yield return () => (new UInt256(0, 0, 0, 0x8000_0000_0000_0000), false);
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), false);
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 1), true);
		yield return () => (UInt256.MaxValue, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsPositiveTestData()
	{
		yield return () => (UInt256.Zero, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsPositiveInfinityTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsRealNumberTestData()
	{
		yield return () => (UInt256.Zero, true);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsSubnormalTestData()
	{
		yield return () => (UInt256.Zero, false);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsZeroTestData()
	{
		yield return () => (UInt256.Zero, true);
		yield return () => (UInt256.One, false);
		yield return () => (new UInt256(0, 0, 1, 0), false);
		yield return () => (new UInt256(0, 1, 0, 0), false);
		yield return () => (new UInt256(1, 0, 0, 0), false);
		yield return () => (UInt256.MaxValue, false);
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MaxMagnitudeTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MaxMagnitudeNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MinMagnitudeTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MinMagnitudeNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256, UInt256)>> MultiplyAddEstimateTestData()
	{
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.Zero, UInt256.Zero);
		yield return () => (UInt256.Zero, UInt256.Zero, UInt256.One, UInt256.One);
		yield return () => (UInt256.One, UInt256.One, UInt256.One, new UInt256(0, 0, 0, 2));
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue), UInt256.One, new UInt256(0, 0, ulong.MaxValue - 1, 2));
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue - 1), new UInt256(0, 0, ulong.MaxValue - 1, ulong.MaxValue));
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, UInt256)>> ParseTestData()
	{
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.MaxValue);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, UInt256)>> ParseSpanTestData()
	{
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.MaxValue);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, UInt256)>> ParseUtf8TestData()
	{
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt256.MaxValue);
		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt256(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt256(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, UInt256)>> TryParseTestData()
	{
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.MaxValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("1000000000000000000000000000000000000000000000000000000000000000000000000000000", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, UInt256)>> TryParseSpanTestData()
	{
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.MaxValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("1000000000000000000000000000000000000000000000000000000000000000000000000000000".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, UInt256)>> TryParseUtf8TestData()
	{
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt256(1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639935"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt256.MaxValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("1000000000000000000000000000000000000000000000000000000000000000000000000000000"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(UInt256, string, IFormatProvider?, string)>> ToStringTestData()
	{
		yield return () => ((UInt256)Int256.MaxValue, "e25", CultureInfo.InvariantCulture, "5.7896044618658097711785493e+76");
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256, UInt256)>> ClampTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 15), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 15));
		yield return () => (new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 10));
		yield return () => (new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 20));
		yield return () => (new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 10));
		yield return () => (new UInt256(0, 0, 0, 25), new UInt256(0, 0, 0, 10), new UInt256(0, 0, 0, 20), new UInt256(0, 0, 0, 20));
		yield return () => (new UInt256(0, 0, 0, 25), new UInt256(0, 0, 0, 30), new UInt256(0, 0, 0, 20), default);
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> CopySignTestData()
	{
		yield return () => (UInt256.MaxValue, UInt256.MaxValue, UInt256.MaxValue);
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MaxTestData()
	{
		yield return () => (UInt256.One, UInt256.One, UInt256.One);
		yield return () => (UInt256.MinValue, UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), UInt256.MaxValue, UInt256.MaxValue);
		yield return () => (new UInt256(0, 0, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt256(0, 1, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 0, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 1, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt256(0, ulong.MaxValue, ulong.MaxValue, 0), new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt256(1, 0, 0, 0), new UInt256(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, 0, 0, 0));
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MaxNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MinTestData()
	{
		yield return () => (UInt256.One, UInt256.One, UInt256.One);
		yield return () => (UInt256.MinValue, UInt256.MaxValue, UInt256.MinValue);
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue - 1), new UInt256(0, 0, 0, ulong.MaxValue - 1));
		yield return () => (new UInt256(0, 0, 0, ulong.MaxValue), new UInt256(0, 0, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 0, 0, ulong.MaxValue));
		yield return () => (new UInt256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(2, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(UInt256, UInt256, UInt256)>> MinNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt256, int)>> SignTestData()
	{
		yield return () => (UInt256.MaxValue, 1);
		yield return () => (UInt256.Zero, 0);
	}

	public static IEnumerable<Func<(UInt256, bool)>> IsPow2TestData()
	{
		yield return () => (new UInt256(0, 0, 0, 1), true);
		yield return () => (new UInt256(0, 0, 0, 2), true);
		yield return () => (new UInt256(0, 0, 0, 4), true);
		yield return () => (new UInt256(0, 0, 0, 8), true);
		yield return () => (new UInt256(0, 0, 0, 16), true);
		yield return () => (new UInt256(0, 0, 0, 1UL << 63), true);
		yield return () => (new UInt256(1UL << 63, 0, 0, 0), true);
		yield return () => (UInt256.Zero, false);
		yield return () => (new UInt256(0, 0, 0, 3), false);
		yield return () => (UInt256.MaxValue, false);
	}

	public static IEnumerable<Func<(UInt256, UInt256)>> Log2TestData()
	{
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 0, 2), new UInt256(0, 0, 0, 1));
		yield return () => (new UInt256(0, 0, 0, 4), new UInt256(0, 0, 0, 2));
		yield return () => (new UInt256(0, 0, 0, 8), new UInt256(0, 0, 0, 3));
		yield return () => (new UInt256(0, 0, 0, 1UL << 63), new UInt256(0, 0, 0, 63));
		yield return () => (new UInt256(0, 0, 1UL << 5, 0), new UInt256(0, 0, 0, 69));
		yield return () => (new UInt256(0, 1UL << 42, 0, 0), new UInt256(0, 0, 0, 170));
		yield return () => (new UInt256(1UL << 13, 0, 0, 0), new UInt256(0, 0, 0, 205));
		yield return () => (new UInt256(1UL << 63, 0, 0, 0), new UInt256(0, 0, 0, 255));
		yield return () => (new UInt256(0, 0, 0, 0), new UInt256(0, 0, 0, 0));
	}

	public static IEnumerable<Func<(UInt256, UInt256, Pair<UInt256>)>> DivRemTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), UInt256.One, (new UInt256(1, 2, 3, 4), new UInt256(0, 0, 0, 0)));
		yield return () => (new UInt256(1, 2, 3, 4), new UInt256(1, 2, 3, 4), (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 0)));
		yield return () => (new UInt256(0, 0, 0, 5), new UInt256(0, 0, 0, 10), (new UInt256(0, 0, 0, 0), new UInt256(0, 0, 0, 5)));
		yield return () => (new UInt256(0, 0, 0, 100), new UInt256(0, 0, 0, 30), (new UInt256(0, 0, 0, 3), new UInt256(0, 0, 0, 10)));
		yield return () => (new UInt256(0, 1, 0, 0), new UInt256(0, 0, 1, 0), (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 0)));
	}

	public static IEnumerable<Func<(UInt256, UInt256)>> LeadingZeroCountTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), new UInt256(0, 0, 0, 256));
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 255));
		yield return () => (new UInt256(0, 0, 1, 0), new UInt256(0, 0, 0, 191));
		yield return () => (new UInt256(0, 1, 0, 0), new UInt256(0, 0, 0, 127));
		yield return () => (new UInt256(1, 0, 0, 0), new UInt256(0, 0, 0, 63));
		yield return () => (new UInt256(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt256(0, 0, 0, 63));
		yield return () => (new UInt256(0, 0, 0, 1UL << 63), new UInt256(0, 0, 0, 192));
		yield return () => (new UInt256(0, 0, 1UL << 63, 0), new UInt256(0, 0, 0, 128));
		yield return () => (new UInt256(0, 1UL << 63, 0, 0), new UInt256(0, 0, 0, 64));
		yield return () => (new UInt256(1UL << 63, 0, 0, 0), new UInt256(0, 0, 0, 0));
		yield return () => (new UInt256(1UL << 62, 0, 0, 0), new UInt256(0, 0, 0, 1));
	}

	public static IEnumerable<Func<(UInt256, UInt256)>> PopCountTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), new UInt256(0, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 1));
		yield return () => (UInt256.MaxValue, new UInt256(0, 0, 0, 256));
		yield return () => (new UInt256(ulong.MaxValue, 0, 0, 0), new UInt256(0, 0, 0, 64));
		yield return () => (new UInt256(0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA), new UInt256(0, 0, 0, 128));
		yield return () => (new UInt256(1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60), new UInt256(0, 0, 0, 4));
	}

	public static IEnumerable<Func<(byte[], bool, UInt256)>> ReadBigEndianTestData()
	{
		yield return () => ([], true, UInt256.Zero);
		yield return () => ([0x01], true, UInt256.One);
		yield return () =>
		{
			byte[] array = new byte[32];
			Array.Fill(array, byte.MaxValue);
			return (array, true, UInt256.MaxValue);
		};
		yield return () =>
		{
			byte[] array = new byte[35];
			for (int i = 3; i < 35; i++)
				array[i] = byte.MaxValue;
			return (array, true, UInt256.MaxValue);
		};
		yield return () => ([0x12, 0x34], true, new UInt256(0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[32];
			array[0] = 0x80;
			return (array, true, new UInt256(1UL << 63, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(byte[], bool, UInt256)>> ReadLittleEndianTestData()
	{
		yield return () => ([], true, UInt256.Zero);
		yield return () => ([0x01], true, UInt256.One);
		yield return () =>
		{
			byte[] array = new byte[32];
			Array.Fill(array, byte.MaxValue);
			return (array, true, UInt256.MaxValue);
		};
		yield return () =>
		{
			byte[] array = new byte[35];
			for (int i = 0; i < 32; i++)
				array[i] = byte.MaxValue;
			return (array, true, UInt256.MaxValue);
		};
		yield return () => ([0x34, 0x12], true, new UInt256(0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[32];
			array[31] = 0x80;
			return (array, true, new UInt256(1UL << 63, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(UInt256, int, UInt256)>> RotateLeftTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), 0, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), 256, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0x8000_0000_0000_0000, 0), 64, new UInt256(0, 0x8000_0000_0000_0000, 0, 0));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 64, new UInt256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt256(0, 0, 0x8000_0000_0000_0000, 0), 128, new UInt256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 128, new UInt256(0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(UInt256, int, UInt256)>> RotateRightTestData()
	{
		yield return () => (new UInt256(1, 2, 3, 4), 0, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(1, 2, 3, 4), 256, new UInt256(1, 2, 3, 4));
		yield return () => (new UInt256(0, 0, 0x8000_0000_0000_0000, 0), 64, new UInt256(0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt256(0, 0, 0, 0x8000_0000_0000_0000), 64, new UInt256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 0x8000_0000_0000_0000, 0), 128, new UInt256(0x8000_0000_0000_0000, 0, 0, 0));
		yield return () => (new UInt256(0x8000_0000_0000_0000, 0, 0, 0), 128, new UInt256(0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(UInt256, UInt256)>> TrailingZeroCountTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), new UInt256(0, 0, 0, 256));
		yield return () => (new UInt256(0, 0, 0, 1), new UInt256(0, 0, 0, 0));
		yield return () => (new UInt256(0, 0, 8, 0), new UInt256(0, 0, 0, 67));
		yield return () => (new UInt256(0, 0x10, 0, 0), new UInt256(0, 0, 0, 132));
		yield return () => (new UInt256(0x200, 0, 0, 0), new UInt256(0, 0, 0, 201));
	}

	public static IEnumerable<Func<(UInt256, int)>> GetByteCountTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), Unsafe.SizeOf<UInt256>());
	}

	public static IEnumerable<Func<(UInt256, int)>> GetShortestBitLengthTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), 0);
		yield return () => (new UInt256(0, 0, 0, 1), 1);
		yield return () => (new UInt256(1, 0, 0, 0), 193);
		yield return () => (UInt256.MaxValue, 256);
	}

	public static IEnumerable<Func<(UInt256, byte[], int)>> WriteBigEndianTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), new byte[32], Unsafe.SizeOf<UInt256>());
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 31; i++)
				buffer[i] = 0;

			buffer[31] = 1;
			
			return (new UInt256(0, 0, 0, 1), buffer, Unsafe.SizeOf<UInt256>());
		};
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 32; i++)
				buffer[i] = 0xFF;
			
			return (UInt256.MaxValue, buffer, Unsafe.SizeOf<UInt256>());
		};
	}

	public static IEnumerable<Func<(UInt256, byte[], int)>> WriteLittleEndianTestData()
	{
		yield return () => (new UInt256(0, 0, 0, 0), new byte[32], Unsafe.SizeOf<UInt256>());
		yield return () =>
		{
			var buffer = new byte[32];
			
			buffer[0] = 1;
			for (int i = 1; i < 32; i++)
				buffer[i] = 0;
			
			return (new UInt256(0, 0, 0, 1), buffer, Unsafe.SizeOf<UInt256>());
		};
		yield return () =>
		{
			var buffer = new byte[32];
			
			for (int i = 0; i < 32; i++)
				buffer[i] = 0xFF;
			
			return (UInt256.MaxValue, buffer, Unsafe.SizeOf<UInt256>());
		};
	}

	public static IEnumerable<Func<(UInt256, byte)>> ConvertToCheckedByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, byte)>> ConvertToSaturatingByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.MaxValue, byte.MaxValue);
		yield return () => (UInt256.ByteMaxValue + UInt256.One, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, byte)>> ConvertToTruncatingByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.MaxValue, byte.MaxValue);
		yield return () => (UInt256.ByteMaxValue + UInt256.One, 0);
	}

	public static IEnumerable<Func<(UInt256, ushort)>> ConvertToCheckedUInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt16MaxValue + UInt256.One, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt16MaxValue + UInt256.One, 0);
	}

	public static IEnumerable<Func<(UInt256, uint)>> ConvertToCheckedUInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, uint)>> ConvertToSaturatingUInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt32MaxValue + UInt256.One, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, uint)>> ConvertToTruncatingUInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt32MaxValue + UInt256.One, 0);
	}

	public static IEnumerable<Func<(UInt256, ulong)>> ConvertToCheckedUInt64TestData()
	{
		yield return () => (UInt256.One, 1U);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		yield return () => (UInt256.One, 1U);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.MaxValue, ulong.MaxValue);
		yield return () => (UInt256.UInt64MaxValue + UInt256.One, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		yield return () => (UInt256.One, 1U);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.MaxValue, ulong.MaxValue);
		yield return () => (UInt256.UInt64MaxValue + UInt256.One, 0);
	}

	public static IEnumerable<Func<(UInt256, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		yield return () => (UInt256.One, UInt128.One);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		yield return () => (UInt256.One, UInt128.One);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt256.MaxValue, UInt128.MaxValue);
		yield return () => (UInt256.UInt128MaxValue + UInt256.One, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		yield return () => (UInt256.One, UInt128.One);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt256.MaxValue, UInt128.MaxValue);
		yield return () => (UInt256.UInt128MaxValue + UInt256.One, UInt128.Zero);
	}

	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		yield return () => (UInt256.One, UInt512.One);
		yield return () => (UInt256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (UInt256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (UInt256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (UInt256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (UInt256.MaxValue, UInt512.UInt256MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		yield return () => (UInt256.One, UInt512.One);
		yield return () => (UInt256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (UInt256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (UInt256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (UInt256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (UInt256.MaxValue, UInt512.UInt256MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		yield return () => (UInt256.One, UInt512.One);
		yield return () => (UInt256.ByteMaxValue, UInt512.ByteMaxValue);
		yield return () => (UInt256.UInt16MaxValue, UInt512.UInt16MaxValue);
		yield return () => (UInt256.UInt32MaxValue, UInt512.UInt32MaxValue);
		yield return () => (UInt256.UInt64MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt256.UInt128MaxValue, UInt512.UInt128MaxValue);
		yield return () => (UInt256.MaxValue, UInt512.UInt256MaxValue);
	}

	public static IEnumerable<Func<(UInt256, sbyte)>> ConvertToCheckedSByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.MaxValue, sbyte.MaxValue);
		yield return () => (UInt256.SByteMaxValue + UInt256.One, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.MaxValue, unchecked((sbyte)0xFF));
		yield return () => (UInt256.SByteMaxValue + UInt256.One, unchecked((sbyte)0x80));
	}

	public static IEnumerable<Func<(UInt256, short)>> ConvertToCheckedInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, short)>> ConvertToSaturatingInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.MaxValue, short.MaxValue);
		yield return () => (UInt256.Int16MaxValue + UInt256.One, short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, short)>> ConvertToTruncatingInt16TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.MaxValue, unchecked((short)0xFFFF));
		yield return () => (UInt256.Int16MaxValue + UInt256.One, unchecked((short)0x8000));
	}

	public static IEnumerable<Func<(UInt256, int)>> ConvertToCheckedInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, int)>> ConvertToSaturatingInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.MaxValue, int.MaxValue);
		yield return () => (UInt256.Int32MaxValue + UInt256.One, int.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, int)>> ConvertToTruncatingInt32TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.MaxValue, unchecked((int)0xFFFF_FFFF));
		yield return () => (UInt256.Int32MaxValue + UInt256.One, unchecked((int)0x8000_0000));
	}

	public static IEnumerable<Func<(UInt256, long)>> ConvertToCheckedInt64TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, long)>> ConvertToSaturatingInt64TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.MaxValue, long.MaxValue);
		yield return () => (UInt256.Int64MaxValue + UInt256.One, long.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, long)>> ConvertToTruncatingInt64TestData()
	{
		yield return () => (UInt256.One, 1);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.MaxValue, unchecked((long)0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt256.Int64MaxValue + UInt256.One, unchecked((long)0x8000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt256, Int128)>> ConvertToCheckedInt128TestData()
	{
		yield return () => (UInt256.One, Int128.One);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int128)>> ConvertToSaturatingInt128TestData()
	{
		yield return () => (UInt256.One, Int128.One);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int128.MaxValue);
		yield return () => (UInt256.MaxValue, Int128.MaxValue);
		yield return () => (UInt256.Int128MaxValue + UInt256.One, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int128)>> ConvertToTruncatingInt128TestData()
	{
		yield return () => (UInt256.One, Int128.One);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int128.MaxValue);
		yield return () => (UInt256.MaxValue, new Int128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt256.Int128MaxValue + UInt256.One, new Int128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt256, Int256)>> ConvertToCheckedInt256TestData()
	{
		yield return () => (UInt256.One, Int256.One);
		yield return () => (UInt256.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int256)>> ConvertToSaturatingInt256TestData()
	{
		yield return () => (UInt256.One, Int256.One);
		yield return () => (UInt256.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int256.MaxValue);
		yield return () => (UInt256.MaxValue, Int256.MaxValue);
		yield return () => (UInt256.Int256MaxValue + UInt256.One, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int256)>> ConvertToTruncatingInt256TestData()
	{
		yield return () => (UInt256.One, Int256.One);
		yield return () => (UInt256.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int256.MaxValue);
		yield return () => (UInt256.MaxValue, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt256.Int256MaxValue + UInt256.One, new Int256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt256, Int512)>> ConvertToCheckedInt512TestData()
	{
		yield return () => (UInt256.One, Int512.One);
		yield return () => (UInt256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt256.MaxValue, Int512.UInt256MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int512)>> ConvertToSaturatingInt512TestData()
	{
		yield return () => (UInt256.One, Int512.One);
		yield return () => (UInt256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt256.MaxValue, Int512.UInt256MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Int512)>> ConvertToTruncatingInt512TestData()
	{
		yield return () => (UInt256.One, Int512.One);
		yield return () => (UInt256.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt256.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt256.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt256.MaxValue, Int512.UInt256MaxValue);
	}

	public static IEnumerable<Func<(UInt256, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, Half)>> ConvertToCheckedHalfTestData()
	{
		yield return () => (UInt256.One, Half.One);
		yield return () => (UInt256.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, (Half)short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Half)>> ConvertToSaturatingHalfTestData()
	{
		yield return () => (UInt256.One, Half.One);
		yield return () => (UInt256.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Half.MaxValue);
		yield return () => (UInt256.MaxValue, Half.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Half)>> ConvertToTruncatingHalfTestData()
	{
		yield return () => (UInt256.One, Half.One);
		yield return () => (UInt256.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Half.MaxValue);
		yield return () => (UInt256.MaxValue, Half.MaxValue);
	}

	public static IEnumerable<Func<(UInt256, float)>> ConvertToCheckedSingleTestData()
	{
		yield return () => (UInt256.One, 1f);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, float)>> ConvertToSaturatingSingleTestData()
	{
		yield return () => (UInt256.One, 1f);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, float)>> ConvertToTruncatingSingleTestData()
	{
		yield return () => (UInt256.One, 1f);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
	}

	public static IEnumerable<Func<(UInt256, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (UInt256.One, 1d);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt256.MaxValue, 115792089237316195423570985008687907853269984665640564039457584007913129639935.0d);
		
		yield return () => (UInt256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
	}
	
	public static IEnumerable<Func<(UInt256, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (UInt256.One, 1d);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt256.MaxValue, 115792089237316195423570985008687907853269984665640564039457584007913129639935.0d);
		
		yield return () => (UInt256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
	}
	
	public static IEnumerable<Func<(UInt256, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (UInt256.One, 1d);
		yield return () => (UInt256.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt256.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt256.Int16MaxValue, short.MaxValue);
		yield return () => (UInt256.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt256.Int32MaxValue, int.MaxValue);
		yield return () => (UInt256.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt256.Int64MaxValue, long.MaxValue);
		yield return () => (UInt256.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt256.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt256.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt256.MaxValue, 115792089237316195423570985008687907853269984665640564039457584007913129639935.0d);
		
		yield return () => (UInt256.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
	}

	// TODO: Add test for BigInteger conversion
	// public static IEnumerable<Func<(UInt256, BigInteger)>> ConvertToCheckedBigIntegerTestData();

	public static IEnumerable<Func<(UInt256, Quad)>> ConvertToCheckedQuadTestData()
	{
		yield return () => (UInt256.One, Quad.One);
		yield return () => (UInt256.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Quad.UInt64MaxValue);
		// TODO: Also test for higher numeric values.
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertToSaturatingQuadTestData()
	{
		yield return () => (UInt256.One, Quad.One);
		yield return () => (UInt256.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Quad.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Quad)>> ConvertToTruncatingQuadTestData()
	{
		yield return () => (UInt256.One, Quad.One);
		yield return () => (UInt256.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Quad.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt256, Octo)>> ConvertToCheckedOctoTestData()
	{
		yield return () => (UInt256.One, Octo.One);
		yield return () => (UInt256.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Octo.UInt64MaxValue);
		// TODO: Also test for higher numeric values.
	}
	
	public static IEnumerable<Func<(UInt256, Octo)>> ConvertToSaturatingOctoTestData()
	{
		yield return () => (UInt256.One, Octo.One);
		yield return () => (UInt256.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Octo.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(UInt256, Octo)>> ConvertToTruncatingOctoTestData()
	{
		yield return () => (UInt256.One, Octo.One);
		yield return () => (UInt256.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt256.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt256.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt256.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt256.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt256.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt256.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt256.UInt64MaxValue, Octo.UInt64MaxValue);
	}

	public static IEnumerable<Func<(byte, UInt256)>> ConvertFromCheckedByteTestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, UInt256)>> ConvertFromSaturatingByteTestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, UInt256)>> ConvertFromTruncatingByteTestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
	}

	public static IEnumerable<Func<(ushort, UInt256)>> ConvertFromCheckedUInt16TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, UInt256)>> ConvertFromSaturatingUInt16TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, UInt256)>> ConvertFromTruncatingUInt16TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}

	public static IEnumerable<Func<(uint, UInt256)>> ConvertFromCheckedUInt32TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, UInt256)>> ConvertFromSaturatingUInt32TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, UInt256)>> ConvertFromTruncatingUInt32TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}

	public static IEnumerable<Func<(ulong, UInt256)>> ConvertFromCheckedUInt64TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, UInt256)>> ConvertFromSaturatingUInt64TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, UInt256)>> ConvertFromTruncatingUInt64TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt128, UInt256)>> ConvertFromCheckedUInt128TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, UInt256)>> ConvertFromSaturatingUInt128TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt256.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, UInt256)>> ConvertFromTruncatingUInt128TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt256.UInt128MaxValue);
	}

	public static IEnumerable<Func<(sbyte, UInt256)>> ConvertFromCheckedSByteTestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, UInt256)>> ConvertFromSaturatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, UInt256.Zero);
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, UInt256)>> ConvertFromTruncatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}

	public static IEnumerable<Func<(short, UInt256)>> ConvertFromCheckedInt16TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, UInt256)>> ConvertFromSaturatingInt16TestData()
	{
		yield return () => (short.MinValue, UInt256.Zero);
		yield return () => (sbyte.MinValue, UInt256.Zero);
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, UInt256)>> ConvertFromTruncatingInt16TestData()
	{
		yield return () => (short.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
	}

	public static IEnumerable<Func<(int, UInt256)>> ConvertFromCheckedInt32TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, UInt256)>> ConvertFromSaturatingInt32TestData()
	{
		yield return () => (int.MinValue, UInt256.Zero);
		yield return () => (short.MinValue, UInt256.Zero);
		yield return () => (sbyte.MinValue, UInt256.Zero);
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, UInt256)>> ConvertFromTruncatingInt32TestData()
	{
		yield return () => (int.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
	}

	public static IEnumerable<Func<(long, UInt256)>> ConvertFromCheckedInt64TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, UInt256)>> ConvertFromSaturatingInt64TestData()
	{
		yield return () => (long.MinValue, UInt256.Zero);
		yield return () => (int.MinValue, UInt256.Zero);
		yield return () => (short.MinValue, UInt256.Zero);
		yield return () => (sbyte.MinValue, UInt256.Zero);
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, UInt256)>> ConvertFromTruncatingInt64TestData()
	{
		yield return () => (long.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));
		yield return () => (int.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int128, UInt256)>> ConvertFromCheckedInt128TestData()
	{
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, UInt256)>> ConvertFromSaturatingInt128TestData()
	{
		yield return () => (Int128.MinValue, UInt256.Zero);
		yield return () => (long.MinValue, UInt256.Zero);
		yield return () => (int.MinValue, UInt256.Zero);
		yield return () => (short.MinValue, UInt256.Zero);
		yield return () => (sbyte.MinValue, UInt256.Zero);
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt256.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, UInt256)>> ConvertFromTruncatingInt128TestData()
	{
		yield return () => (Int128.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
		yield return () => (long.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));
		yield return () => (int.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt256.One);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (long.MaxValue, UInt256.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt256.Int128MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, UInt256)>> ConvertFromCheckedBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt256.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt256.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt256.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt256.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt256.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt256.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt256.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, UInt256)>> ConvertFromSaturatingBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt256.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt256.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt256.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt256.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt256.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt256.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt256.MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639936"), UInt256.MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"), UInt256.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, UInt256)>> ConvertFromTruncatingBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt256.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt256.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt256.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt256.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt256.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt256.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt256.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt256.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt256.MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639936"), UInt256.Zero);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"), UInt256.MaxValue);
	}

	public static IEnumerable<Func<(Half, UInt256)>> ConvertFromCheckedHalfTestData()
	{
		yield return () => (Half.One, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, UInt256)>> ConvertFromSaturatingHalfTestData()
	{
		yield return () => (Half.MinValue, UInt256.Zero);
		yield return () => (Half.One, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, UInt256)>> ConvertFromTruncatingHalfTestData()
	{
		yield return () => (Half.MinValue, UInt256.Zero);
		yield return () => (Half.One, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
	}

	public static IEnumerable<Func<(float, UInt256)>> ConvertFromCheckedSingleTestData()
	{
		yield return () => (1f, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, UInt256)>> ConvertFromSaturatingSingleTestData()
	{
		yield return () => (float.MinValue, UInt256.Zero);
		yield return () => (1f, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, UInt256)>> ConvertFromTruncatingSingleTestData()
	{
		yield return () => (float.MinValue, UInt256.Zero);
		yield return () => (1f, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
	}

	public static IEnumerable<Func<(double, UInt256)>> ConvertFromCheckedDoubleTestData()
	{
		yield return () => (1d, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, UInt256)>> ConvertFromSaturatingDoubleTestData()
	{
		yield return () => (double.MinValue, UInt256.Zero);
		yield return () => (1d, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, UInt256)>> ConvertFromTruncatingDoubleTestData()
	{
		yield return () => (double.MinValue, UInt256.Zero);
		yield return () => (1d, UInt256.One);
		yield return () => (byte.MaxValue, UInt256.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt256.SByteMaxValue);
		yield return () => (short.MaxValue, UInt256.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt256.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt256.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt256.UInt32MaxValue);
	}
}
