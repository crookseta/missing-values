using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IEqualityOperatorsDataSource<T>
	where T : IEqualityOperators<T, T, bool>
{
	static abstract IEnumerable<Func<(T, T, bool)>> op_EqualityTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_InequalityTestData();
}