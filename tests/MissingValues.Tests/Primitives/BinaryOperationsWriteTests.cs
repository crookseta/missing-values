using MissingValues.Primitives;
using MissingValues.Tests.Data;
using static MissingValues.Tests.Data.BinaryOperationsDataSources;

namespace MissingValues.Tests.Primitives;

public class BinaryOperationsWriteTests
{
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256WriteBigEndianTest))]
    public async Task UInt256_Write_BigEndianTest(UInt256 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteUInt256BigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int256WriteBigEndianTest))]
    public async Task Int256_Write_BigEndianTest(Int256 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteInt256BigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512WriteBigEndianTest))]
    public async Task UInt512_Write_BigEndianTest(UInt512 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(64);
        
        BinaryOperations.WriteUInt512BigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int512WriteBigEndianTest))]
    public async Task Int512_Write_BigEndianTest(Int512 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(64);
        
        BinaryOperations.WriteInt512BigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(QuadWriteBigEndianTest))]
    public async Task Quad_Write_BigEndianTest(Quad value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(16);
        
        BinaryOperations.WriteQuadBigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(OctoWriteBigEndianTest))]
    public async Task Octo_Write_BigEndianTest(Octo value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteOctoBigEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256WriteLittleEndianTest))]
    public async Task UInt256_Write_LittleEndianTest(UInt256 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteUInt256LittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int256WriteLittleEndianTest))]
    public async Task Int256_Write_LittleEndianTest(Int256 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteInt256LittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512WriteLittleEndianTest))]
    public async Task UInt512_Write_LittleEndianTest(UInt512 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(64);
        
        BinaryOperations.WriteUInt512LittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int512WriteLittleEndianTest))]
    public async Task Int512_Write_LittleEndianTest(Int512 value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(64);
        
        BinaryOperations.WriteInt512LittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(QuadWriteLittleEndianTest))]
    public async Task Quad_Write_LittleEndianTest(Quad value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(16);
        
        BinaryOperations.WriteQuadLittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(OctoWriteLittleEndianTest))]
    public async Task Octo_Write_LittleEndianTest(Octo value, byte[] expected)
    {
        var actual = new byte[expected.Length];
        await Assert.That(actual).HasAtLeast(32);
        
        BinaryOperations.WriteOctoLittleEndian(actual, in value);
        await Assert.That(actual).IsEquivalentTo(expected);
    }
    
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256TryWriteBigEndianTest))]
    public async Task UInt256_TryWrite_BigEndianTest(UInt256 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteUInt256BigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int256TryWriteBigEndianTest))]
    public async Task Int256_TryWrite_BigEndianTest(Int256 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteInt256BigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512TryWriteBigEndianTest))]
    public async Task UInt512_TryWrite_BigEndianTest(UInt512 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteUInt512BigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int512TryWriteBigEndianTest))]
    public async Task Int512_TryWrite_BigEndianTest(Int512 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteInt512BigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(QuadTryWriteBigEndianTest))]
    public async Task Quad_TryWrite_BigEndianTest(Quad value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteQuadBigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(OctoTryWriteBigEndianTest))]
    public async Task Octo_TryWrite_BigEndianTest(Octo value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteOctoBigEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256TryWriteLittleEndianTest))]
    public async Task UInt256_TryWrite_LittleEndianTest(UInt256 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteUInt256LittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int256TryWriteLittleEndianTest))]
    public async Task Int256_TryWrite_LittleEndianTest(Int256 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteInt256LittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512TryWriteLittleEndianTest))]
    public async Task UInt512_TryWrite_LittleEndianTest(UInt512 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteUInt512LittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(Int512TryWriteLittleEndianTest))]
    public async Task Int512_TryWrite_LittleEndianTest(Int512 value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteInt512LittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(QuadTryWriteLittleEndianTest))]
    public async Task Quad_TryWrite_LittleEndianTest(Quad value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteQuadLittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
    [Test]
    [MethodDataSource<BinaryOperationsDataSources>(nameof(OctoTryWriteLittleEndianTest))]
    public async Task Octo_TryWrite_LittleEndianTest(Octo value, byte[] expected, bool successful)
    {
        var actual = new byte[expected.Length];
        
        bool result = BinaryOperations.TryWriteOctoLittleEndian(actual, in value);
        await Assert.That(result).IsEqualTo(successful);
        
        if (result)
        {
            await Assert.That(actual).IsEquivalentTo(expected);
        }
    }
}