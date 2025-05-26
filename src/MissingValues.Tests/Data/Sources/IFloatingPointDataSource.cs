using System.Numerics;

namespace MissingValues.Tests.Data.Sources;

public interface IFloatingPointDataSource<T> 
    where T : IFloatingPoint<T>
{
    static abstract IEnumerable<Func<(T, T)>> CeilingTestData();
    static abstract IEnumerable<Func<(T, T)>> FloorTestData();
    static abstract IEnumerable<Func<(T, int, MidpointRounding, T)>> RoundTestData();
    static abstract IEnumerable<Func<(T, T)>> TruncateTestData();
    static abstract IEnumerable<Func<(T, int)>> GetExponentByteCountTestData();
    static abstract IEnumerable<Func<(T, int)>> GetExponentShortestBitLengthTestData();
    static abstract IEnumerable<Func<(T, int)>> GetSignificandBitLengthTestData();
    static abstract IEnumerable<Func<(T, int)>> GetSignificandByteCountTestData();
    static abstract IEnumerable<Func<(T, byte[], bool, int)>> TryWriteExponentBigEndianTestData();
    static abstract IEnumerable<Func<(T, byte[], bool, int)>> TryWriteExponentLittleEndianTestData();
    static abstract IEnumerable<Func<(T, byte[], bool, int)>> TryWriteSignificandBigEndianTestData();
    static abstract IEnumerable<Func<(T, byte[], bool, int)>> TryWriteSignificandLittleEndianTestData();
}