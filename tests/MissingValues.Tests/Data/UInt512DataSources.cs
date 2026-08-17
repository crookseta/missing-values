using MissingValues.Tests.Data.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MissingValues.Tests.Extensions;

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
		yield return () => (UInt512.MaxValue, new UInt512(0, 0, 0, 0, 0, 1, 0, 0), new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
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

	public static IEnumerable<Func<(UInt512, UInt512)>> op_UnaryNegationTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero);
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_CheckedUnaryNegationTestData()
	{
		yield return () => (UInt512.Zero, UInt512.Zero, false);
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

	public static IEnumerable<Func<(UInt512, string, IFormatProvider?, string)>> ToStringTestData()
	{
		yield return () => (UInt512.UInt32MaxValue, "x", CultureInfo.InvariantCulture, "ffffffff");
		yield return () => (UInt512.UInt32MaxValue, "X", CultureInfo.InvariantCulture, "FFFFFFFF");
		yield return () => (UInt512.UInt64MaxValue, "X", CultureInfo.InvariantCulture, "FFFFFFFFFFFFFFFF");
		yield return () => (UInt512.UInt128MaxValue, "X", CultureInfo.InvariantCulture, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		yield return () => (UInt512.UInt256MaxValue, "X", CultureInfo.InvariantCulture, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		yield return () => (UInt512.MaxValue, "X", CultureInfo.InvariantCulture, "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
		
		yield return () => (UInt512.UInt32MaxValue, "B", CultureInfo.InvariantCulture, "11111111111111111111111111111111");
		yield return () => (UInt512.UInt64MaxValue, "B", CultureInfo.InvariantCulture, "1111111111111111111111111111111111111111111111111111111111111111");
		yield return () => (UInt512.UInt128MaxValue, "B", CultureInfo.InvariantCulture, "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		yield return () => (UInt512.UInt256MaxValue, "B", CultureInfo.InvariantCulture, "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		yield return () => (UInt512.MaxValue, "B", CultureInfo.InvariantCulture, "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
		
		yield return () => ((UInt512)Int256.MaxValue, "e25", CultureInfo.InvariantCulture, "5.7896044618658097711785493e+76");
		yield return () => ((UInt512)Int512.MaxValue, "e25", CultureInfo.InvariantCulture, "6.7039039649712985497870125e+153");
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

	public static IEnumerable<Func<(UInt512, UInt512, Pair<UInt512>)>> DivRemTestData()
	{
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 0, 0, 0, 0, 0, 0, 1), (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 8), (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 0)));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), new UInt512(1, 2, 3, 4, 5, 6, 7, 9), (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new UInt512(1, 2, 3, 4, 5, 6, 7, 8)));
		yield return () => (UInt512.MaxValue, UInt256.MaxValue, (new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001), UInt512.Zero));
		yield return () => (UInt512.MaxValue, new UInt512(0, 0, 0, 1, 0, 0, 0, 0), (new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), new UInt512(0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)));
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> LeadingZeroCountTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 512));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 511));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 447));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1UL << 36, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 411));
		yield return () => (new UInt512(0, 0, 0, 1, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 255));
		yield return () => (new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> PopCountTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 1));
		yield return () => (UInt512.MaxValue, new UInt512(0, 0, 0, 0, 0, 0, 0, 512));
		yield return () => (new UInt512(ulong.MaxValue, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 64));
		yield return () => (new UInt512(0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA, 0xAAAAAAAAAAAAAAAA), new UInt512(0, 0, 0, 0, 0, 0, 0, 256));
		yield return () => (new UInt512(0, 0, 0, 0, 1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60), new UInt512(0, 0, 0, 0, 0, 0, 0, 4));
		yield return () => (new UInt512(1UL << 63, 1UL << 62, 1UL << 61, 1UL << 60, 1UL << 59, 1UL << 58, 1UL << 57, 1UL << 56), new UInt512(0, 0, 0, 0, 0, 0, 0, 8));
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
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 0, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 512, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0), 64, new UInt512(0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0));
		yield return () => (new UInt512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0), 64, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt512(0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0, 0), 128, new UInt512(0, 0, 0x8000_0000_0000_0000, 0, 0, 0, 0, 0));
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> RotateRightTestData()
	{
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 0, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(1, 2, 3, 4, 5, 6, 7, 8), 512, new UInt512(1, 2, 3, 4, 5, 6, 7, 8));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0), 64, new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000), 64, new UInt512(0x8000_0000_0000_0000, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0x8000_0000_0000_0000, 0, 0, 0), 128, new UInt512(0, 0, 0, 0, 0, 0, 0x8000_0000_0000_0000, 0));
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> TrailingZeroCountTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 512));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), new UInt512(0, 0, 0, 0, 0, 0, 0, 0));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 64));
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 1UL << 36, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 100));
		yield return () => (new UInt512(0, 0, 0, 1, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 256));
		yield return () => (new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), new UInt512(0, 0, 0, 0, 0, 0, 0, 511));
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
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], Unsafe.SizeOf<UInt512>());
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 63; i++)
				buffer[i] = 0;

			buffer[63] = 1;
			
			return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer, Unsafe.SizeOf<UInt512>());
		};
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 64; i++)
				buffer[i] = 0xFF;
			
			return (UInt512.MaxValue, buffer, Unsafe.SizeOf<UInt512>());
		};
	}

	public static IEnumerable<Func<(UInt512, byte[], int)>> WriteLittleEndianTestData()
	{
		yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], Unsafe.SizeOf<UInt512>());
		yield return () =>
		{
			var buffer = new byte[64];
			
			buffer[0] = 1;
			for (int i = 1; i < 64; i++)
				buffer[i] = 0;
			
			return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer, Unsafe.SizeOf<UInt512>());
		};
		yield return () =>
		{
			var buffer = new byte[64];
			
			for (int i = 0; i < 64; i++)
				buffer[i] = 0xFF;
			
			return (UInt512.MaxValue, buffer, Unsafe.SizeOf<UInt512>());
		};
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToCheckedByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToSaturatingByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.MaxValue, byte.MaxValue);
		yield return () => (UInt512.ByteMaxValue + UInt512.One, byte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, byte)>> ConvertToTruncatingByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.MaxValue, byte.MaxValue);
		yield return () => (UInt512.ByteMaxValue + UInt512.One, 0);
	}

	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToCheckedUInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToSaturatingUInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt16MaxValue + UInt512.One, ushort.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, ushort)>> ConvertToTruncatingUInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt16MaxValue + UInt512.One, 0);
	}

	public static IEnumerable<Func<(UInt512, uint)>> ConvertToCheckedUInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, uint)>> ConvertToSaturatingUInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt32MaxValue + UInt512.One, uint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, uint)>> ConvertToTruncatingUInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt32MaxValue + UInt512.One, 0);
	}

	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToCheckedUInt64TestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToSaturatingUInt64TestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt64MaxValue + UInt512.One, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, ulong)>> ConvertToTruncatingUInt64TestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt64MaxValue + UInt512.One, 0);
	}

	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToCheckedUInt128TestData()
	{
		yield return () => (UInt512.One, UInt128.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToSaturatingUInt128TestData()
	{
		yield return () => (UInt512.One, UInt128.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.UInt128MaxValue + UInt512.One, UInt128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, UInt128)>> ConvertToTruncatingUInt128TestData()
	{
		yield return () => (UInt512.One, UInt128.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.UInt128MaxValue + UInt512.One, UInt128.Zero);
	}

	public static IEnumerable<Func<(UInt512, UInt256)>> ConvertToCheckedUInt256TestData()
	{
		yield return () => (UInt512.One, UInt256.One);
		yield return () => (UInt512.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (UInt512.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (UInt512.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (UInt512.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (UInt512.UInt256MaxValue, UInt256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, UInt256)>> ConvertToSaturatingUInt256TestData()
	{
		yield return () => (UInt512.One, UInt256.One);
		yield return () => (UInt512.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (UInt512.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (UInt512.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (UInt512.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (UInt512.UInt256MaxValue, UInt256.MaxValue);
		yield return () => (UInt512.MaxValue, UInt256.MaxValue);
		yield return () => (UInt512.UInt256MaxValue + UInt512.One, UInt256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, UInt256)>> ConvertToTruncatingUInt256TestData()
	{
		yield return () => (UInt512.One, UInt256.One);
		yield return () => (UInt512.ByteMaxValue, UInt256.ByteMaxValue);
		yield return () => (UInt512.UInt16MaxValue, UInt256.UInt16MaxValue);
		yield return () => (UInt512.UInt32MaxValue, UInt256.UInt32MaxValue);
		yield return () => (UInt512.UInt64MaxValue, UInt256.UInt64MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt256.UInt128MaxValue);
		yield return () => (UInt512.UInt256MaxValue, UInt256.MaxValue);
		yield return () => (UInt512.MaxValue, UInt256.MaxValue);
		yield return () => (UInt512.UInt256MaxValue + UInt512.One, UInt256.Zero);
	}
	
	public static IEnumerable<Func<(UInt512, nuint)>> ConvertToCheckedUIntPtrTestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UIntPtrMaxValue, nuint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, nuint)>> ConvertToSaturatingUIntPtrTestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UIntPtrMaxValue, nuint.MaxValue);
		yield return () => (UInt512.MaxValue, nuint.MaxValue);
		yield return () => (UInt512.UIntPtrMaxValue + UInt512.One, nuint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, nuint)>> ConvertToTruncatingUIntPtrTestData()
	{
		yield return () => (UInt512.One, 1U);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UIntPtrMaxValue, nuint.MaxValue);
		yield return () => (UInt512.MaxValue, nuint.MaxValue);
		yield return () => (UInt512.UIntPtrMaxValue + UInt512.One, 0);
	}

	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToCheckedSByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToSaturatingSByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.MaxValue, sbyte.MaxValue);
		yield return () => (UInt512.SByteMaxValue + UInt512.One, sbyte.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, sbyte)>> ConvertToTruncatingSByteTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.MaxValue, unchecked((sbyte)0xFF));
		yield return () => (UInt512.SByteMaxValue + UInt512.One, unchecked((sbyte)0x80));
	}

	public static IEnumerable<Func<(UInt512, short)>> ConvertToCheckedInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, short)>> ConvertToSaturatingInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.MaxValue, short.MaxValue);
		yield return () => (UInt512.Int16MaxValue + UInt512.One, short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, short)>> ConvertToTruncatingInt16TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.MaxValue, unchecked((short)0xFFFF));
		yield return () => (UInt512.Int16MaxValue + UInt512.One, unchecked((short)0x8000));
	}

	public static IEnumerable<Func<(UInt512, int)>> ConvertToCheckedInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, int)>> ConvertToSaturatingInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.MaxValue, int.MaxValue);
		yield return () => (UInt512.Int32MaxValue + UInt512.One, int.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, int)>> ConvertToTruncatingInt32TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.MaxValue, unchecked((int)0xFFFF_FFFF));
		yield return () => (UInt512.Int32MaxValue + UInt512.One, unchecked((int)0x8000_0000));
	}

	public static IEnumerable<Func<(UInt512, long)>> ConvertToCheckedInt64TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, long)>> ConvertToSaturatingInt64TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.MaxValue, long.MaxValue);
		yield return () => (UInt512.Int64MaxValue + UInt512.One, long.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, long)>> ConvertToTruncatingInt64TestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.MaxValue, unchecked((long)0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt512.Int64MaxValue + UInt512.One, unchecked((long)0x8000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToCheckedInt128TestData()
	{
		yield return () => (UInt512.One, Int128.One);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToSaturatingInt128TestData()
	{
		yield return () => (UInt512.One, Int128.One);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int128.MaxValue);
		yield return () => (UInt512.MaxValue, Int128.MaxValue);
		yield return () => (UInt512.Int128MaxValue + UInt512.One, Int128.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int128)>> ConvertToTruncatingInt128TestData()
	{
		yield return () => (UInt512.One, Int128.One);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int128.MaxValue);
		yield return () => (UInt512.MaxValue, new Int128(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt512.Int128MaxValue + UInt512.One, new Int128(0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToCheckedInt256TestData()
	{
		yield return () => (UInt512.One, Int256.One);
		yield return () => (UInt512.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToSaturatingInt256TestData()
	{
		yield return () => (UInt512.One, Int256.One);
		yield return () => (UInt512.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int256.MaxValue);
		yield return () => (UInt512.MaxValue, Int256.MaxValue);
		yield return () => (UInt512.Int256MaxValue + UInt512.One, Int256.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int256)>> ConvertToTruncatingInt256TestData()
	{
		yield return () => (UInt512.One, Int256.One);
		yield return () => (UInt512.SByteMaxValue, Int256.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int256.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int256.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int256.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int256.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int256.MaxValue);
		yield return () => (UInt512.MaxValue, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt512.Int256MaxValue + UInt512.One, new Int256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}

	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToCheckedInt512TestData()
	{
		yield return () => (UInt512.One, Int512.One);
		yield return () => (UInt512.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt512.Int512MaxValue, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToSaturatingInt512TestData()
	{
		yield return () => (UInt512.One, Int512.One);
		yield return () => (UInt512.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt512.Int512MaxValue, Int512.MaxValue);
		yield return () => (UInt512.MaxValue, Int512.MaxValue);
		yield return () => (UInt512.Int512MaxValue + UInt512.One, Int512.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Int512)>> ConvertToTruncatingInt512TestData()
	{
		yield return () => (UInt512.One, Int512.One);
		yield return () => (UInt512.SByteMaxValue, Int512.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Int512.Int16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Int512.Int32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Int512.Int64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Int512.Int128MaxValue);
		yield return () => (UInt512.Int256MaxValue, Int512.Int256MaxValue);
		yield return () => (UInt512.Int512MaxValue, Int512.MaxValue);
		yield return () => (UInt512.MaxValue, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
		yield return () => (UInt512.Int512MaxValue + UInt512.One, new Int512(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000));
	}
	
	public static IEnumerable<Func<(UInt512, nint)>> ConvertToCheckedIntPtrTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.IntPtrMaxValue, nint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, nint)>> ConvertToSaturatingIntPtrTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.IntPtrMaxValue, nint.MaxValue);
		yield return () => (UInt512.MaxValue, nint.MaxValue);
		yield return () => (UInt512.IntPtrMaxValue + UInt512.One, nint.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, nint)>> ConvertToTruncatingIntPtrTestData()
	{
		yield return () => (UInt512.One, 1);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.IntPtrMaxValue, nint.MaxValue);
		yield return () => (UInt512.MaxValue, nint.Size == 8 ? unchecked((nint)0xFFFF_FFFF_FFFF_FFFF) : unchecked((nint)0xFFFF_FFFF));
		yield return () => (UInt512.IntPtrMaxValue + UInt512.One, nint.Size == 8 ? unchecked((nint)0x8000_0000_0000_0000) : unchecked((nint)0x8000_0000));
	}
	
	public static IEnumerable<Func<(UInt512, BigInteger)>> ConvertToCheckedBigIntegerTestData()
	{
		yield return () => (UInt512.One, BigInteger.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.UInt256MaxValue, BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"));
		yield return () => (UInt512.MaxValue, BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
	}

	public static IEnumerable<Func<(UInt512, BigInteger)>> ConvertToSaturatingBigIntegerTestData()
	{
		yield return () => (UInt512.One, BigInteger.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.UInt256MaxValue, BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"));
		yield return () => (UInt512.MaxValue, BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
	}

	public static IEnumerable<Func<(UInt512, BigInteger)>> ConvertToTruncatingBigIntegerTestData()
	{
		yield return () => (UInt512.One, BigInteger.One);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, UInt128.MaxValue);
		yield return () => (UInt512.UInt256MaxValue, BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"));
		yield return () => (UInt512.MaxValue, BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"));
	}

	public static IEnumerable<Func<(UInt512, Half)>> ConvertToCheckedHalfTestData()
	{
		yield return () => (UInt512.One, Half.One);
		yield return () => (UInt512.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, (Half)short.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Half)>> ConvertToSaturatingHalfTestData()
	{
		yield return () => (UInt512.One, Half.One);
		yield return () => (UInt512.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (UInt512.MaxValue, Half.PositiveInfinity);
	}
	
	public static IEnumerable<Func<(UInt512, Half)>> ConvertToTruncatingHalfTestData()
	{
		yield return () => (UInt512.One, Half.One);
		yield return () => (UInt512.ByteMaxValue, (Half)byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, (Half)sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, (Half)short.MaxValue);
		yield return () => (UInt512.MaxValue, Half.PositiveInfinity);
	}

	public static IEnumerable<Func<(UInt512, float)>> ConvertToCheckedSingleTestData()
	{
		yield return () => (UInt512.One, 1f);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, float)>> ConvertToSaturatingSingleTestData()
	{
		yield return () => (UInt512.One, 1f);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, float)>> ConvertToTruncatingSingleTestData()
	{
		yield return () => (UInt512.One, 1f);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
	}

	public static IEnumerable<Func<(UInt512, double)>> ConvertToCheckedDoubleTestData()
	{
		yield return () => (UInt512.One, 1d);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (UInt512.Parse("693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}
	
	public static IEnumerable<Func<(UInt512, double)>> ConvertToSaturatingDoubleTestData()
	{
		yield return () => (UInt512.One, 1d);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (UInt512.Parse("693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}
	
	public static IEnumerable<Func<(UInt512, double)>> ConvertToTruncatingDoubleTestData()
	{
		yield return () => (UInt512.One, 1d);
		yield return () => (UInt512.ByteMaxValue, byte.MaxValue);
		yield return () => (UInt512.SByteMaxValue, sbyte.MaxValue);
		yield return () => (UInt512.Int16MaxValue, short.MaxValue);
		yield return () => (UInt512.UInt16MaxValue, ushort.MaxValue);
		yield return () => (UInt512.Int32MaxValue, int.MaxValue);
		yield return () => (UInt512.UInt32MaxValue, uint.MaxValue);
		yield return () => (UInt512.Int64MaxValue, long.MaxValue);
		yield return () => (UInt512.UInt64MaxValue, ulong.MaxValue);
		yield return () => (UInt512.Int128MaxValue, (double)Int128.MaxValue);
		yield return () => (UInt512.UInt128MaxValue, (double)UInt128.MaxValue);
		
		yield return () => (UInt512.Parse("781377183594418599030564404241984000000000000000000"),
			781377183594418599030564404241984000000000000000000.0d);
		yield return () => (UInt512.Parse("693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455"),
			693167423530203714894603546035770925859109268843954143792619895153655326951406405759993601526034894524347802740350892957243539455.0d);
	}

	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToCheckedQuadTestData()
	{
		yield return () => (UInt512.One, Quad.One);
		yield return () => (UInt512.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Quad.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToSaturatingQuadTestData()
	{
		yield return () => (UInt512.One, Quad.One);
		yield return () => (UInt512.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Quad.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Quad)>> ConvertToTruncatingQuadTestData()
	{
		yield return () => (UInt512.One, Quad.One);
		yield return () => (UInt512.ByteMaxValue, Quad.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Quad.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Quad.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Quad.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Quad.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Quad.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Quad.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Quad.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToCheckedOctoTestData()
	{
		yield return () => (UInt512.One, Octo.One);
		yield return () => (UInt512.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Octo.Int128MaxValue);
		yield return () => (UInt512.UInt128MaxValue, Octo.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToSaturatingOctoTestData()
	{
		yield return () => (UInt512.One, Octo.One);
		yield return () => (UInt512.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Octo.Int128MaxValue);
		yield return () => (UInt512.UInt128MaxValue, Octo.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, Octo)>> ConvertToTruncatingOctoTestData()
	{
		yield return () => (UInt512.One, Octo.One);
		yield return () => (UInt512.ByteMaxValue, Octo.ByteMaxValue);
		yield return () => (UInt512.SByteMaxValue, Octo.SByteMaxValue);
		yield return () => (UInt512.Int16MaxValue, Octo.Int16MaxValue);
		yield return () => (UInt512.UInt16MaxValue, Octo.UInt16MaxValue);
		yield return () => (UInt512.Int32MaxValue, Octo.Int32MaxValue);
		yield return () => (UInt512.UInt32MaxValue, Octo.UInt32MaxValue);
		yield return () => (UInt512.Int64MaxValue, Octo.Int64MaxValue);
		yield return () => (UInt512.UInt64MaxValue, Octo.UInt64MaxValue);
		yield return () => (UInt512.Int128MaxValue, Octo.Int128MaxValue);
		yield return () => (UInt512.UInt128MaxValue, Octo.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt512, NFloat)>> ConvertToCheckedNFloatTestData()
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
	
	public static IEnumerable<Func<(UInt512, NFloat)>> ConvertToSaturatingNFloatTestData()
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
	
	public static IEnumerable<Func<(UInt512, NFloat)>> ConvertToTruncatingNFloatTestData()
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

	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromCheckedByteTestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromSaturatingByteTestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
	}
	
	public static IEnumerable<Func<(byte, UInt512)>> ConvertFromTruncatingByteTestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
	}

	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromCheckedUInt16TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromSaturatingUInt16TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(ushort, UInt512)>> ConvertFromTruncatingUInt16TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}

	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromCheckedUInt32TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromSaturatingUInt32TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(uint, UInt512)>> ConvertFromTruncatingUInt32TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}

	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromCheckedUInt64TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromSaturatingUInt64TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
	}
	
	public static IEnumerable<Func<(ulong, UInt512)>> ConvertFromTruncatingUInt64TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
	}

	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromCheckedUInt128TestData()
	{
		yield return () => (UInt128.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt512.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromSaturatingUInt128TestData()
	{
		yield return () => (UInt128.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt512.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(UInt128, UInt512)>> ConvertFromTruncatingUInt128TestData()
	{
		yield return () => (UInt128.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => (ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => (UInt128.MaxValue, UInt512.UInt128MaxValue);
	}
	
	public static IEnumerable<Func<(nuint, UInt512)>> ConvertFromCheckedUIntPtrTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(nuint, UInt512)>> ConvertFromSaturatingUIntPtrTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(nuint, UInt512)>> ConvertFromTruncatingUIntPtrTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromCheckedSByteTestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromSaturatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, UInt512.Zero);
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(sbyte, UInt512)>> ConvertFromTruncatingSByteTestData()
	{
		yield return () => (sbyte.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}

	public static IEnumerable<Func<(short, UInt512)>> ConvertFromCheckedInt16TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, UInt512)>> ConvertFromSaturatingInt16TestData()
	{
		yield return () => (short.MinValue, UInt512.Zero);
		yield return () => (sbyte.MinValue, UInt512.Zero);
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
	}
	
	public static IEnumerable<Func<(short, UInt512)>> ConvertFromTruncatingInt16TestData()
	{
		yield return () => (short.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
	}

	public static IEnumerable<Func<(int, UInt512)>> ConvertFromCheckedInt32TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, UInt512)>> ConvertFromSaturatingInt32TestData()
	{
		yield return () => (int.MinValue, UInt512.Zero);
		yield return () => (short.MinValue, UInt512.Zero);
		yield return () => (sbyte.MinValue, UInt512.Zero);
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
	}
	
	public static IEnumerable<Func<(int, UInt512)>> ConvertFromTruncatingInt32TestData()
	{
		yield return () => (int.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
	}

	public static IEnumerable<Func<(long, UInt512)>> ConvertFromCheckedInt64TestData()
	{
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, UInt512)>> ConvertFromSaturatingInt64TestData()
	{
		yield return () => (long.MinValue, UInt512.Zero);
		yield return () => (int.MinValue, UInt512.Zero);
		yield return () => (short.MinValue, UInt512.Zero);
		yield return () => (sbyte.MinValue, UInt512.Zero);
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
	}
	
	public static IEnumerable<Func<(long, UInt512)>> ConvertFromTruncatingInt64TestData()
	{
		yield return () => (long.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));
		yield return () => (int.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (1, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
	}

	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromCheckedInt128TestData()
	{
		yield return () => (Int128.One, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt512.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromSaturatingInt128TestData()
	{
		yield return () => (Int128.MinValue, UInt512.Zero);
		yield return () => (long.MinValue, UInt512.Zero);
		yield return () => (int.MinValue, UInt512.Zero);
		yield return () => (short.MinValue, UInt512.Zero);
		yield return () => (sbyte.MinValue, UInt512.Zero);
		yield return () => (Int128.One, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt512.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(Int128, UInt512)>> ConvertFromTruncatingInt128TestData()
	{
		yield return () => (Int128.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000, 0x0000_0000_0000_0000));
		yield return () => (long.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x8000_0000_0000_0000));
		yield return () => (int.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_8000_0000));
		yield return () => (short.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_8000));
		yield return () => (sbyte.MinValue, new UInt512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FF80));
		yield return () => (Int128.One, UInt512.One);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (long.MaxValue, UInt512.Int64MaxValue);
		yield return () => (Int128.MaxValue, UInt512.Int128MaxValue);
	}
	
	public static IEnumerable<Func<(nint, UInt512)>> ConvertFromCheckedIntPtrTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(nint, UInt512)>> ConvertFromSaturatingIntPtrTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(nint, UInt512)>> ConvertFromTruncatingIntPtrTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(BigInteger, UInt512)>> ConvertFromCheckedBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt512.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt512.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt512.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt512.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt512.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt512.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt512.UInt256MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"), UInt512.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, UInt512)>> ConvertFromSaturatingBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt512.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt512.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt512.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt512.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt512.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt512.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt512.UInt256MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"), UInt512.MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"), UInt512.MaxValue);
	}

	public static IEnumerable<Func<(BigInteger, UInt512)>> ConvertFromTruncatingBigIntegerTestData()
	{
		yield return () => (BigInteger.One, UInt512.One);
		yield return () => ((BigInteger)byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => ((BigInteger)sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => ((BigInteger)short.MaxValue, UInt512.Int16MaxValue);
		yield return () => ((BigInteger)ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => ((BigInteger)int.MaxValue, UInt512.Int32MaxValue);
		yield return () => ((BigInteger)uint.MaxValue, UInt512.UInt32MaxValue);
		yield return () => ((BigInteger)long.MaxValue, UInt512.Int64MaxValue);
		yield return () => ((BigInteger)ulong.MaxValue, UInt512.UInt64MaxValue);
		yield return () => ((BigInteger)Int128.MaxValue, UInt512.Int128MaxValue);
		yield return () => ((BigInteger)UInt128.MaxValue, UInt512.UInt128MaxValue);
		yield return () => (BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129639935"), UInt512.UInt256MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084095"), UInt512.MaxValue);
		yield return () => (BigInteger.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073546976801874298166903427690031858186486050853753882811946569946433649006084096"), UInt512.Zero);
	}

	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromCheckedHalfTestData()
	{
		yield return () => (Half.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromSaturatingHalfTestData()
	{
		yield return () => (Half.MinValue, UInt512.Zero);
		yield return () => (Half.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}
	
	public static IEnumerable<Func<(Half, UInt512)>> ConvertFromTruncatingHalfTestData()
	{
		yield return () => (Half.MinValue, UInt512.Zero);
		yield return () => (Half.One, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
	}

	public static IEnumerable<Func<(float, UInt512)>> ConvertFromCheckedSingleTestData()
	{
		yield return () => (1f, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, UInt512)>> ConvertFromSaturatingSingleTestData()
	{
		yield return () => (float.MinValue, UInt512.Zero);
		yield return () => (1f, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}
	
	public static IEnumerable<Func<(float, UInt512)>> ConvertFromTruncatingSingleTestData()
	{
		yield return () => (float.MinValue, UInt512.Zero);
		yield return () => (1f, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
	}

	public static IEnumerable<Func<(double, UInt512)>> ConvertFromCheckedDoubleTestData()
	{
		yield return () => (1d, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, UInt512)>> ConvertFromSaturatingDoubleTestData()
	{
		yield return () => (double.MinValue, UInt512.Zero);
		yield return () => (1d, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(double, UInt512)>> ConvertFromTruncatingDoubleTestData()
	{
		yield return () => (double.MinValue, UInt512.Zero);
		yield return () => (1d, UInt512.One);
		yield return () => (byte.MaxValue, UInt512.ByteMaxValue);
		yield return () => (sbyte.MaxValue, UInt512.SByteMaxValue);
		yield return () => (short.MaxValue, UInt512.Int16MaxValue);
		yield return () => (ushort.MaxValue, UInt512.UInt16MaxValue);
		yield return () => (int.MaxValue, UInt512.Int32MaxValue);
		yield return () => (uint.MaxValue, UInt512.UInt32MaxValue);
	}
	
	public static IEnumerable<Func<(NFloat, UInt512)>> ConvertFromCheckedNFloatTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(NFloat, UInt512)>> ConvertFromSaturatingNFloatTestData()
	{
		throw new NotImplementedException();
	}
	
	public static IEnumerable<Func<(NFloat, UInt512)>> ConvertFromTruncatingNFloatTestData()
	{
		throw new NotImplementedException();
	}
}
