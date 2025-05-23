using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IBitwiseOperatorsDataSource<T>
	where T : IBitwiseOperators<T, T, T>
{
	static abstract IEnumerable<Func<(T, T, T)>> op_BitwiseAndTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_BitwiseOrTestData();
	static abstract IEnumerable<Func<(T, T, T)>> op_BitwiseXorTestData();
	static abstract IEnumerable<Func<(T, T)>> op_OnesComplementTestData();
}