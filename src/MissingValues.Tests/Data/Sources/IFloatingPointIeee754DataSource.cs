using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IFloatingPointIeee754DataSource<T> 
    where T : IFloatingPointIeee754<T>
{
    static abstract IEnumerable<Func<(T, T, T)>> Atan2TestData();
    static abstract IEnumerable<Func<(T, T, T)>> Atan2PiTestData();
    static abstract IEnumerable<Func<(T, T)>> BitDecrementTestData();
    static abstract IEnumerable<Func<(T, T)>> BitIncrementTestData();
    static abstract IEnumerable<Func<(T, T, T, T)>> FusedMultiplyAddTestData();
    static abstract IEnumerable<Func<(T, T, T)>> Ieee754RemainderTestData();
    static abstract IEnumerable<Func<(T, T, T)>> ILogBTestData();
    static abstract IEnumerable<Func<(T, T, T, T)>> LerpTestData();
    static abstract IEnumerable<Func<(T, T)>> ReciprocalEstimateTestData();
    static abstract IEnumerable<Func<(T, T)>> ReciprocalSqrtEstimateTestData();
    static abstract IEnumerable<Func<(T, int, T)>> ScaleBTestData();
}