using MissingValues.Tests.Data.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedAdditionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_CheckedDecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_CheckedIncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedMultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, bool)>> op_CheckedSubtractionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_DecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_DivisionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_IncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_ModulusTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_MultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_SubtractionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_ShiftLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_ShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int, UInt512)>> op_UnsignedShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseAndTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseOrTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> op_BitwiseXorTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> op_OnesComplementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_EqualityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_InequalityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_GreaterThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_GreaterThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_LessThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, bool)>> op_LessThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> AbsTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsCanonicalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsComplexNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsEvenIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsFiniteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsImaginaryNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNaNTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNegativeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNegativeInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsNormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsOddIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPositiveTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPositiveInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsRealNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsSubnormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsZeroTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, UInt512)>> MultiplyAddEstimateTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, UInt512)>> ParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, UInt512)>> ParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, UInt512)>> ParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, UInt512)>> TryParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512, UInt512)>> ClampTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> CopySignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MaxNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512, UInt512)>> MinNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> SignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, bool)>> IsPow2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, UInt512)>> Log2TestData()
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, UInt512)>> ReadLittleEndianTestData()
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, int)>> GetShortestBitLengthTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, byte[], int)>> WriteBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(UInt512, byte[], int)>> WriteLittleEndianTestData()
	{
		throw new NotImplementedException();
	}
}
