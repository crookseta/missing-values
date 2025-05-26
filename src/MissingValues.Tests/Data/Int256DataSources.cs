using System.Globalization;
using MissingValues.Tests.Data.Sources;

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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedAdditionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedDecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedIncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedMultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedSubtractionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_DecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_DivisionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_IncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_ModulusTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_MultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_SubtractionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> op_UnsignedShiftRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseAndTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseOrTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseXorTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> op_OnesComplementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_EqualityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_InequalityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanOrEqualTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> AbsTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsCanonicalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsComplexNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsEvenIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsFiniteTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsImaginaryNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNaNTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNegativeInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsNormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsOddIntegerTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPositiveInfinityTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsRealNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsSubnormalTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsZeroTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinMagnitudeNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> MultiplyAddEstimateTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, Int256)>> ParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, Int256)>> ParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, Int256)>> ParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, Int256)>> TryParseTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseSpanTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, Int256)>> TryParseUtf8TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256, Int256)>> ClampTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> CopySignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MaxNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, Int256)>> MinNumberTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> SignTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, bool)>> IsPow2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> Log2TestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256, (Int256, Int256))>> DivRemTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> LeadingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> PopCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(byte[], bool, Int256)>> ReadLittleEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateLeftTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int, Int256)>> RotateRightTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, Int256)>> TrailingZeroCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> GetByteCountTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, int)>> GetShortestBitLengthTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteBigEndianTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int256, byte[], int)>> WriteLittleEndianTestData()
	{
		throw new NotImplementedException();
	}
}
