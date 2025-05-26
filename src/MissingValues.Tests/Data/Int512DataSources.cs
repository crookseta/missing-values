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
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedAdditionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedDecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedIncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedMultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedSubtractionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_DecrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_DivisionTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512)>> op_IncrementTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_ModulusTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_MultiplyTestData()
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_SubtractionTestData()
	{
		throw new NotImplementedException();
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

	public static IEnumerable<Func<(Int512, Int512)>> SignTestData()
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
}
