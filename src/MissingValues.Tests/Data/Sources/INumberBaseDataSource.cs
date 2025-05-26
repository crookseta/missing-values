using System.Globalization;
using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface INumberBaseDataSource<T>
    where T : INumberBase<T>
{
    static abstract IEnumerable<Func<(T, T)>> AbsTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsCanonicalTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsComplexNumberTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsEvenIntegerTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsFiniteTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsImaginaryNumberTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsInfinityTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsIntegerTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsNaNTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsNegativeTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsNegativeInfinityTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsNormalTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsOddIntegerTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsPositiveTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsPositiveInfinityTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsRealNumberTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsSubnormalTestData();
    static abstract IEnumerable<Func<(T, bool)>> IsZeroTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MaxMagnitudeTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MaxMagnitudeNumberTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MinMagnitudeTestData();
    static abstract IEnumerable<Func<(T, T, T)>> MinMagnitudeNumberTestData();
    static abstract IEnumerable<Func<(T, T, T, T)>> MultiplyAddEstimateTestData();
    static abstract IEnumerable<Func<(string, NumberStyles, IFormatProvider?, T)>> ParseTestData();
    static abstract IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, T)>> ParseSpanTestData();
    static abstract IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, T)>> ParseUtf8TestData();
    static abstract IEnumerable<Func<(string, NumberStyles, IFormatProvider?, bool, T)>> TryParseTestData();
    static abstract IEnumerable<Func<(char[], NumberStyles, IFormatProvider?, bool, T)>> TryParseSpanTestData();
    static abstract IEnumerable<Func<(byte[], NumberStyles, IFormatProvider?, bool, T)>> TryParseUtf8TestData();
}