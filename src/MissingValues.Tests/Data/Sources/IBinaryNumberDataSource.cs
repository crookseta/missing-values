using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IBinaryNumberDataSource<T>
    where T : IBinaryNumber<T>
{
    static abstract IEnumerable<Func<(T, bool)>> IsPow2TestData();
    static abstract IEnumerable<Func<(T, T)>> Log2TestData();
}