using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface INumberDataSource<T>
    where T : INumber<T>
{
    static abstract IEnumerable<Func<(T, T, T, T)>> ClampTestData();
    static abstract IEnumerable<Func<(T, T, T)>> CopySignTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MaxTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MaxNumberTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MinTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MinNumberTestData();
    static abstract IEnumerable<Func<(T, int)>> SignTestData();
}