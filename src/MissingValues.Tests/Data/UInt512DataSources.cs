using MissingValues.Tests.Data.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Tests.Data;

public class UInt512DataSources
	: IMathOperatorsDataSource<UInt512>,
	IShiftOperatorsDataSource<UInt512>,
	IBitwiseOperatorsDataSource<UInt512>,
	IEqualityOperatorsDataSource<UInt512>,
	IComparisonOperatorsDataSource<UInt512>,
	INumberBaseDataSource<UInt512>,
	INumberDataSource<UInt512>,
	IBinaryNumberDataSource<UInt512>,
	IBinaryIntegerDataSource<UInt512>
{
	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_AdditionTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.Zero, UInt512.One);
		yield return () => (UInt512.One, UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => (new UInt512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), UInt512.One, new UInt512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (UInt512.One, UInt512.MaxValue, UInt512.Zero);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedAdditionTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero, false);
		yield return () => (UInt512.One, UInt512.Zero, UInt512.One, false);
		yield return () => (UInt512.One, UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 1, 0), false);
		yield return () => (new UInt512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), UInt512.One, new UInt512(1, 0, 0, 0, 0, 0, 0, 0), false);
		yield return () => (UInt512.One, UInt512.MaxValue, UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_CheckedDecrementTestData()
	{
		yield return () => (UInt512.Zero, UInt512.MaxValue, true);
		yield return () => (UInt512.One, UInt512.Zero, false);
		yield return () => (UInt512.MaxValue, UInt512.MaxValue - UInt512.One, false);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_CheckedIncrementTestData()
	{
		yield return () => (UInt512.Zero, UInt512.One, false);
		yield return () => (UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 0, 2), false);
		yield return () => (UInt512.MaxValue, UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedMultiplyTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero, false);
		yield return () => (UInt512.One, UInt512.One, UInt512.One, false);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 3),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 6),
			false
		);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			new UInt512(0, 0, 0, 0, 0, 1, 0, 0),
			false
		);
		yield return () => (
			new UInt512(0, 0, 0, 1, 0, 0, 0, 0),
			new UInt512(0, 0, 0, 0, 1, 0, 0, 0),
			new UInt512(1, 0, 0, 0, 0, 0, 0, 0),
			false
		);
		yield return () => (
			UInt512.MaxValue,
			UInt512.One,
			UInt512.MaxValue,
			false
		);
		yield return () => (
			UInt512.MaxValue,
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			default,
			true
		);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedSubtractionTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero, false);
		yield return () => (UInt512.One, UInt512.Zero, UInt512.One, false);
		yield return () => (UInt512.One, UInt512.One, UInt512.Zero, false);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), UInt512.One, UInt512.One, false);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			UInt512.One,
			new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue),
			false
		);
		yield return () => (UInt512.Zero, UInt512.One, UInt512.MaxValue, true);
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_DecrementTestData()
	{
		yield return () => (UInt512.Zero, UInt512.MaxValue);
		yield return () => (UInt512.One, UInt512.Zero);
		yield return () => (UInt512.MaxValue, UInt512.MaxValue - UInt512.One);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_DivisionTestData()
	{
		yield return () => (UInt512.Zero, UInt512.One, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.One, UInt512.One);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, 6),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 3)
		);
		yield return () => (UInt512.MaxValue, UInt512.One, UInt512.MaxValue);
		yield return () => (UInt512.MaxValue, new UInt512(0, 0, 0, 0, 0, 1, 0, 0), UInt512.Parse("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306815"));
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, ulong.MaxValue - 1),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue)
		);
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_IncrementTestData()
	{
		yield return () => (UInt512.Zero, UInt512.One);
		yield return () => (UInt512.One, new UInt512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (UInt512.MaxValue, UInt512.Zero);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_ModulusTestData()
	{
		yield return () => (UInt512.Zero, UInt512.One, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.One, UInt512.Zero);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, 5),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			UInt512.One
		);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, 7),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 3),
			UInt512.One
		);
		yield return () => (UInt512.MaxValue, UInt512.One, UInt512.Zero);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue),
			UInt512.One
		);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_MultiplyTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.One, UInt512.One);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 3),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 6)
		);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue),
			new UInt512(0, 0, 0, 0, 0, 0, 0, 2),
			new UInt512(0, 0, 0, 0, 0, 0, 1, ulong.MaxValue - 1)
		);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			new UInt512(0, 0, 0, 0, 0, 1, 0, 0)
		);
		yield return () => (
			new UInt512(0, 0, 0, 1, 0, 0, 0, 0),
			new UInt512(0, 0, 0, 0, 1, 0, 0, 0),
			new UInt512(1, 0, 0, 0, 0, 0, 0, 0)
		);
		yield return () => (UInt512.MaxValue, UInt512.One, UInt512.MaxValue);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_SubtractionTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.Zero, UInt512.One);
		yield return () => (UInt512.One, UInt512.One, UInt512.Zero);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), UInt512.One, UInt512.One);
		yield return () => (
			new UInt512(0, 0, 0, 0, 0, 0, 1, 0),
			UInt512.One,
			new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue)
		);
		yield return () => (
			UInt512.MaxValue,
			UInt512.One,
			new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue,
				ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue - 1)
		);
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_ShiftLeftTestData()
	{
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 0, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 1, new UInt512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 63, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 64, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 128, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 256, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 511, new UInt512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 512, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 513, new UInt512(0, 0, 0, 0, 0, 0, 0, 2));
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_ShiftRightTestData()
	{
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 0, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), 1, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000), 63, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1, 0), 64, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, 0, 0), 128, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 1, 0, 0, 0, 0), 256, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0), 511, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 512, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), 513, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_UnsignedShiftRightTestData()
	{
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 0, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), 1, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000), 63, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1, 0), 64, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, 0, 0), 128, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 1, 0, 0, 0, 0), 256, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0), 511, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 512, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), 513, new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseAndTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.Zero, UInt512.MaxValue, UInt512.Zero);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), UInt512.MaxValue, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseOrTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.Zero, UInt512.MaxValue, UInt512.MaxValue);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), UInt512.MaxValue, UInt512.MaxValue);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseXorTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.Zero, UInt512.MaxValue, UInt512.MaxValue);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), UInt512.Zero);
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_OnesComplementTestData()
	{
		yield return () => (UInt512.Zero, UInt512.MaxValue);
		yield return () => (UInt512.MaxValue, UInt512.Zero);
		yield return () => (new UInt512(0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555), new UInt512(0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA, 0x5555555555555555, 0xAAAAAAAAAAAAAAAA));
		yield return () => (new UInt512(0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0, 0x0123456789ABCDEF, 0xFEDCBA9876543210, 0x0F0F0F0F0F0F0F0F, 0xF0F0F0F0F0F0F0F0), new UInt512(~0x0123456789ABCDEFU, ~0xFEDCBA9876543210U, ~0x0F0F0F0F0F0F0F0FU, ~0xF0F0F0F0F0F0F0F0U, ~0x0123456789ABCDEFU, ~0xFEDCBA9876543210U, ~0x0F0F0F0F0F0F0F0FU, ~0xF0F0F0F0F0F0F0F0U));
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_EqualityTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_InequalityTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_GreaterThanOrEqualTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_GreaterThanTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_LessThanOrEqualTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_LessThanTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 5, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 7, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 4, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 5, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 1, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 3, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 9), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 5, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 4, 5, 7, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 3, 4, 4, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 2, 3, 5, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(1, 2, 1, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
		yield return () => (new UInt512(1, 3, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), false);
		yield return () => (new UInt512(0, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), true);
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> AbsTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.One, UInt512.One);
		yield return () => (UInt512.MaxValue, UInt512.MaxValue);
		yield return () => (UInt512.MinValue, UInt512.MinValue);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsCanonicalTestData()
	{
		yield return () => (UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsComplexNumberTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsEvenIntegerTestData()
	{
		yield return () => (UInt512.Zero, true);
		
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsFiniteTestData()
	{
		yield return () => (UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsImaginaryNumberTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsInfinityTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsIntegerTestData()
	{
		yield return () => (UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNaNTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNegativeTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNegativeInfinityTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNormalTestData()
	{
		yield return () => (UInt512.Zero, false);
		yield return () => (UInt512.One, true);
		yield return () => (UInt512.MaxValue, true);
		yield return () => (UInt512.MinValue, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsOddIntegerTestData()
	{
		yield return () => (UInt512.Zero, false);
		yield return () => (UInt512.One, true);
		yield return () => (UInt512.MaxValue, true);
		yield return () => (UInt512.MinValue, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPositiveTestData()
	{
		yield return () => (UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPositiveInfinityTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsRealNumberTestData()
	{
		yield return () => (UInt512.Zero, true);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsSubnormalTestData()
	{
		yield return () => (UInt512.Zero, false);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsZeroTestData()
	{
		yield return () => (UInt512.Zero, true);
		yield return () => (UInt512.One, false);
		yield return () => (UInt512.MaxValue, false);
		yield return () => (UInt512.MinValue, true);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxMagnitudeTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxMagnitudeNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinMagnitudeTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinMagnitudeNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, UInt512)>> MultiplyAddEstimateTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.Zero, UInt512.Zero);
		yield return () => (UInt512.Zero, UInt512.Zero, UInt512.One, UInt512.One);
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, UInt512)>> ParseTestData()
	{
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656", NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095", NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.MaxValue);
		
		yield return () => ("123456789ABCDEF0", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF", 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, UInt512)>> ParseSpanTestData()
	{
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.MaxValue);
		
		yield return () => ("123456789ABCDEF0".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF".ToCharArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111".ToCharArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, UInt512)>> ParseUtf8TestData()
	{
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("2135987035920910082395021706169552114602704522356652769947041607822219725780640550022962086936576"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 0, 1, 0, 0, 0, 0, 0));
		yield return () => ("39402006196394479212279040100143613805079739270465446667948293404245721771497210611414266254884915640806627990306816"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(0, 1, 0, 0, 0, 0, 0, 0));
		yield return () => ("726838724295606890549323807888004534353641360687318060281490199180639288113397923326191050713763565560762521606266177933534601628614656"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, new UInt512(1, 0, 0, 0, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, UInt512.MaxValue);
		
		yield return () => ("123456789ABCDEF0"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x123456789ABCDEF0));
		yield return () => ("FF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFF));
		yield return () => ("FFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFF));
		yield return () => ("FFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF00000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF));
		yield return () => ("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF"u8.ToArray(), 
			NumberStyles.HexNumber, CultureInfo.InvariantCulture, new UInt512(0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0xFFFFFFFFFFFFFFFF));
		
		yield return () => ("1010101010101010"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1010101010101010));
		yield return () => ("11111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111));
		yield return () => ("1111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111));
		yield return () => ("11111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b11111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111000000000000000000000000000000000000000000000000000000000000000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0, 0, 0, 0, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
		yield return () => ("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"u8.ToArray(), 
			NumberStyles.BinaryNumber, CultureInfo.InvariantCulture, new UInt512(0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111, 0b1111111111111111111111111111111111111111111111111111111111111111));
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseTestData()
	{
		yield return () => ("0", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.Zero);
		yield return () => ("1", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.One);
		yield return () => ("4294967296", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936", NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095", NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.MaxValue);
		yield return () => ("-1", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("20000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseSpanTestData()
	{
		yield return () => ("0".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.Zero);
		yield return () => ("1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.One);
		yield return () => ("4294967296".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.MaxValue);
		yield return () => ("-1".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("20000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000".ToCharArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseUtf8TestData()
	{
		yield return () => ("0"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.Zero);
		yield return () => ("1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.One);
		yield return () => ("4294967296"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 0, 4294967296));
		yield return () => ("18446744073709551616"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 0, 1, 0));
		yield return () => ("340282366920938463463374607431768211456"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => ("6277101735386680763835789423207666416102355444464034512896"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 0, 1, 0, 0, 0));
		yield return () => ("115792089237316195423570985008687907853269984665640564039457584007913129639936"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, new UInt512(0, 0, 0, 1, 0, 0, 0, 0));
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, true, UInt512.MaxValue);
		yield return () => ("-1"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("2.25"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
		yield return () => ("20000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"u8.ToArray(), NumberStyles.Integer, CultureInfo.InvariantCulture, false, default);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, UInt512)>> ClampTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 15), new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 15));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 10));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 20));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 05), new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 10));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 25), new UInt512(0, 0, 0, 0, 0, 0, 0, 10), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), new UInt512(0, 0, 0, 0, 0, 0, 0, 20));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 25), new UInt512(0, 0, 0, 0, 0, 0, 0, 30), new UInt512(0, 0, 0, 0, 0, 0, 0, 20), default);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> CopySignTestData()
	{
		yield return () => (UInt512.MaxValue, UInt512.MaxValue, UInt512.MaxValue);
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxTestData()
	{
		yield return () => (UInt512.One, UInt512.One, UInt512.One);
		yield return () => (UInt512.MinValue, UInt512.MaxValue, UInt512.MaxValue);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), UInt512.MaxValue, UInt512.MaxValue);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 1, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 1, 0, 0));
		yield return () => (new UInt512(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt512(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxNumberTestData()
	{
		return MaxTestData();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinTestData()
	{
		yield return () => (UInt512.One, UInt512.One, UInt512.One);
		yield return () => (UInt512.MinValue, UInt512.MaxValue, UInt512.MinValue);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), UInt512.MaxValue, new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, 0, ulong.MaxValue));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue), new UInt512(0, 0, 0, 0, 0, 0, ulong.MaxValue, ulong.MaxValue));
		yield return () => (new UInt512(1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), new UInt512(0, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinNumberTestData()
	{
		return MinTestData();
	}

	public static IEnumerable<Func<(UInt512, int)>> SignTestData()
	{
		yield return () => (UInt512.MaxValue, 1);
		yield return () => (UInt512.Zero, 0);
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPow2TestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), true);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), true);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 4), true);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 8), true);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 16), true);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1UL << 63), true);
		yield return () => (new UInt512(0, 0, 0, 0, 1UL << 63, 0, 0, 0), true);
		yield return () => (new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), true);
		yield return () => (UInt512.Zero, false);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 3), false);
		yield return () => (UInt512.MaxValue, false);
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> Log2TestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 2), new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 4), new UInt512(0, 0, 0, 0, 0, 0, 0, 2));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 8), new UInt512(0, 0, 0, 0, 0, 0, 0, 3));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1UL << 63), new UInt512(0, 0, 0, 0, 0, 0, 0, 63));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1UL << 5, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 69));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 1UL << 42, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 170));
		yield return () => (new UInt512(0, 0, 0, 0, 1UL << 13, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 205));
		yield return () => (new UInt512(0, 0, 0, 0, 1UL << 63, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 255));
		yield return () => (new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 511));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(UInt512, UInt512, (UInt512, UInt512))>> DivRemTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> LeadingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> PopCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, UInt512)>> ReadBigEndianTestData()
	{
		yield return () => ([], true, UInt512.Zero);
		yield return () => ([0x01], true, UInt512.One);
		yield return () =>
		{
			byte[] array = new byte[64];
			Array.Fill(array, byte.MaxValue);
			return (array, true, UInt512.MaxValue);
		};
		yield return () =>
		{
			byte[] array = new byte[67];
			for (int i = 3; i < 67; i++)
				array[i] = byte.MaxValue;
			return (array, true, UInt512.MaxValue);
		};
		yield return () => ([0x12, 0x34], true, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[64];
			array[0] = 0x80;
			return (array, true, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(byte[], bool, UInt512)>> ReadLittleEndianTestData()
	{
		yield return () => ([], true, UInt512.Zero);
		yield return () => ([0x01], true, UInt512.One);
		yield return () =>
		{
			byte[] array = new byte[64];
			Array.Fill(array, byte.MaxValue);
			return (array, true, UInt512.MaxValue);
		};
		yield return () =>
		{
			byte[] array = new byte[67];
			for (int i = 0; i < 64; i++)
				array[i] = byte.MaxValue;
			return (array, true, UInt512.MaxValue);
		};
		yield return () => ([0x34, 0x12], true, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x1234));
		yield return () =>
		{
			byte[] array = new byte[64];
			array[63] = 0x80;
			return (array, true, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
		};
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> RotateLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> RotateRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> TrailingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int)>> GetByteCountTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), Unsafe.SizeOf<UInt512>());
	}

	public static IEnumerable<Func<(UInt512, int)>> GetShortestBitLengthTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), 0);
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), 1);
		yield return () => (new UInt512(0, 0, 0, 0, 1, 0, 0, 0), 193);
		yield return () => (new UInt512(1, 0, 0, 0, 0, 0, 0, 0), 449);
		yield return () => (UInt512.MaxValue, 512);
	}

	public static IEnumerable<Func<(UInt512, byte[], int)>> WriteBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, byte[], int)>> WriteLittleEndianTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, uint)>> ConvertToCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, uint)>> ConvertToSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, uint)>> ConvertToTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> ConvertToCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, UInt512)>> ConvertToSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, UInt512)>> ConvertToTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, short)>> ConvertToCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, short)>> ConvertToSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, short)>> ConvertToTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int)>> ConvertToCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, int)>> ConvertToSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, int)>> ConvertToTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, long)>> ConvertToCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, long)>> ConvertToSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, long)>> ConvertToTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Half)>> ConvertToCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Half)>> ConvertToSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Half)>> ConvertToTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, float)>> ConvertToCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, float)>> ConvertToSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, float)>> ConvertToTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, double)>> ConvertToCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, double)>> ConvertToSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, double)>> ConvertToTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromCheckedByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromSaturatingByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromTruncatingByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromCheckedUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromSaturatingUInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromTruncatingUInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromCheckedUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromSaturatingUInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromTruncatingUInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromCheckedUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromSaturatingUInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromTruncatingUInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromCheckedUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromSaturatingUInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromTruncatingUInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertFromCheckedUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertFromSaturatingUInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(UInt256, UInt512)>> ConvertFromTruncatingUInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromCheckedSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromSaturatingSByteTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromTruncatingSByteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(short, UInt512)>> ConvertFromCheckedInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, UInt512)>> ConvertFromSaturatingInt16TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(short, UInt512)>> ConvertFromTruncatingInt16TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(int, UInt512)>> ConvertFromCheckedInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, UInt512)>> ConvertFromSaturatingInt32TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(int, UInt512)>> ConvertFromTruncatingInt32TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(long, UInt512)>> ConvertFromCheckedInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, UInt512)>> ConvertFromSaturatingInt64TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(long, UInt512)>> ConvertFromTruncatingInt64TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromCheckedInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromSaturatingInt128TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromTruncatingInt128TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, UInt512)>> ConvertFromCheckedInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertFromSaturatingInt256TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int256, UInt512)>> ConvertFromTruncatingInt256TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, UInt512)>> ConvertFromCheckedInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt512)>> ConvertFromSaturatingInt512TestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Int512, UInt512)>> ConvertFromTruncatingInt512TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromCheckedHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromSaturatingHalfTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromTruncatingHalfTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(float, UInt512)>> ConvertFromCheckedSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, UInt512)>> ConvertFromSaturatingSingleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(float, UInt512)>> ConvertFromTruncatingSingleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(double, UInt512)>> ConvertFromCheckedDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, UInt512)>> ConvertFromSaturatingDoubleTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(double, UInt512)>> ConvertFromTruncatingDoubleTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Quad, UInt512)>> ConvertFromCheckedQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertFromSaturatingQuadTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Quad, UInt512)>> ConvertFromTruncatingQuadTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Octo, UInt512)>> ConvertFromCheckedOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertFromSaturatingOctoTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(Octo, UInt512)>> ConvertFromTruncatingOctoTestData()
	{
		throw new NotImplementedException();
	}
}
