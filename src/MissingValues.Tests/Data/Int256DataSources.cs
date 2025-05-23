using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class Int256DataSources
	: IMathOperatorsDataSource<Int256>,
	IShiftOperatorsDataSource<Int256>,
	IBitwiseOperatorsDataSource<Int256>,
	IEqualityOperatorsDataSource<Int256>,
	IComparisonOperatorsDataSource<Int256>
{
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_AdditionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseAndTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseOrTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_BitwiseXorTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedAdditionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedDecrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_CheckedIncrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedMultiplyTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256, bool)>> op_CheckedSubtractionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256)>> op_DecrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_DivisionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_EqualityTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanOrEqualTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_GreaterThanTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256)>> op_IncrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_InequalityTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanOrEqualTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, bool)>> op_LessThanTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_ModulusTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_MultiplyTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256)>> op_OnesComplementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftLeftTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, int, Int256)>> op_ShiftRightTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, Int256, Int256)>> op_SubtractionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int256, int, Int256)>> op_UnsignedShiftRightTestData() => throw new NotImplementedException();
}
