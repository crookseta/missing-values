using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IBinaryIntegerDataSource<T>
    where T : IBinaryInteger<T>
{
    static abstract IEnumerable<Func<(T, T, (T, T))>> DivRemTestData();
    static abstract IEnumerable<Func<(T, T)>> LeadingZeroCountTestData();
    static abstract IEnumerable<Func<(T, T)>> PopCountTestData();
    static abstract IEnumerable<Func<(byte[], bool, T)>> ReadBigEndianTestData();
    static abstract IEnumerable<Func<(byte[], bool, T)>> ReadLittleEndianTestData();
    static abstract IEnumerable<Func<(T, int, T)>> RotateLeftTestData();
    static abstract IEnumerable<Func<(T, int, T)>> RotateRightTestData();
    static abstract IEnumerable<Func<(T, T)>> TrailingZeroCountTestData();
    static abstract IEnumerable<Func<(T, int)>> GetByteCountTestData();
    static abstract IEnumerable<Func<(T, int)>> GetShortestBitLengthTestData();
    static abstract IEnumerable<Func<(T, byte[], int)>> WriteBigEndianTestData();
    static abstract IEnumerable<Func<(T, byte[], int)>> WriteLittleEndianTestData();
}