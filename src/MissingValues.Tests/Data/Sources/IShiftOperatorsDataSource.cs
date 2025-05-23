using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IShiftOperatorsDataSource<T>
	where T : IShiftOperators<T, int, T>
{
	static abstract IEnumerable<Func<(T, int, T)>> op_ShiftLeftTestData();
	static abstract IEnumerable<Func<(T, int, T)>> op_ShiftRightTestData();
	static abstract IEnumerable<Func<(T, int, T)>> op_UnsignedShiftRightTestData();
}