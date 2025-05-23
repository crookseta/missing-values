using MissingValues.Tests.Data.Sources;

namespace MissingValues.Tests.Data;

public class Int512DataSources
	: IMathOperatorsDataSource<Int512>,
	IShiftOperatorsDataSource<Int512>,
	IBitwiseOperatorsDataSource<Int512>,
	IEqualityOperatorsDataSource<Int512>,
	IComparisonOperatorsDataSource<Int512>
{
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_AdditionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseAndTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseOrTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_BitwiseXorTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedAdditionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedDecrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_CheckedIncrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedMultiplyTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512, bool)>> op_CheckedSubtractionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512)>> op_DecrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_DivisionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_EqualityTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanOrEqualTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_GreaterThanTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512)>> op_IncrementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_InequalityTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanOrEqualTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, bool)>> op_LessThanTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_ModulusTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_MultiplyTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512)>> op_OnesComplementTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, int, Int512)>> op_ShiftLeftTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, int, Int512)>> op_ShiftRightTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, Int512, Int512)>> op_SubtractionTestData() => throw new NotImplementedException();
	public static IEnumerable<Func<(Int512, int, Int512)>> op_UnsignedShiftRightTestData() => throw new NotImplementedException();
}
