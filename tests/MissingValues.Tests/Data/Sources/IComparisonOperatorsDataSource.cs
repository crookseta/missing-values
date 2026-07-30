using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IComparisonOperatorsDataSource<T>
	where T : IComparisonOperators<T, T, bool>
{
	static abstract IEnumerable<Func<(T, T, bool)>> op_GreaterThanOrEqualTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_GreaterThanTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_LessThanOrEqualTestData();
	static abstract IEnumerable<Func<(T, T, bool)>> op_LessThanTestData();
}