using MissingValues.Primitives;
using MissingValues.Tests.Data;
using static MissingValues.Tests.Data.BinaryOperationsDataSources;

namespace MissingValues.Tests.Primitives;

public class BinaryOperationsReadTests
{
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256ReadBigEndianTest))]
	public async Task UInt256_Read_BigEndianTest(byte[] source, UInt256 expected)
	{
		await Assert.That(BinaryOperations.ReadUInt256BigEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int256ReadBigEndianTest))]
	public async Task Int256_Read_BigEndianTest(byte[] source, Int256 expected)
	{
		await Assert.That(BinaryOperations.ReadInt256BigEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512ReadBigEndianTest))]
	public async Task UInt512_Read_BigEndianTest(byte[] source, UInt512 expected)
	{
		await Assert.That(BinaryOperations.ReadUInt512BigEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int512ReadBigEndianTest))]
	public async Task Int512_Read_BigEndianTest(byte[] source, Int512 expected)
	{
		await Assert.That(BinaryOperations.ReadInt512BigEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(QuadReadBigEndianTest))]
	public async Task Quad_Read_BigEndianTest(byte[] source, Quad expected)
	{
		await Assert.That(BinaryOperations.ReadQuadBigEndian(source)).IsBitwiseEquivalentTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(OctoReadBigEndianTest))]
	public async Task Octo_Read_BigEndianTest(byte[] source, Octo expected)
	{
		await Assert.That(BinaryOperations.ReadOctoBigEndian(source)).IsBitwiseEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256ReadLittleEndianTest))]
	public async Task UInt256_Read_LittleEndianTest(byte[] source, UInt256 expected)
	{
		await Assert.That(BinaryOperations.ReadUInt256LittleEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int256ReadLittleEndianTest))]
	public async Task Int256_Read_LittleEndianTest(byte[] source, Int256 expected)
	{
		await Assert.That(BinaryOperations.ReadInt256LittleEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512ReadLittleEndianTest))]
	public async Task UInt512_Read_LittleEndianTest(byte[] source, UInt512 expected)
	{
		await Assert.That(BinaryOperations.ReadUInt512LittleEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int512ReadLittleEndianTest))]
	public async Task Int512_Read_LittleEndianTest(byte[] source, Int512 expected)
	{
		await Assert.That(BinaryOperations.ReadInt512LittleEndian(source)).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(QuadReadLittleEndianTest))]
	public async Task Quad_Read_LittleEndianTest(byte[] source, Quad expected)
	{
		await Assert.That(BinaryOperations.ReadQuadLittleEndian(source)).IsBitwiseEquivalentTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(OctoReadLittleEndianTest))]
	public async Task Octo_Read_LittleEndianTest(byte[] source, Octo expected)
	{
		await Assert.That(BinaryOperations.ReadOctoLittleEndian(source)).IsBitwiseEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256TryReadBigEndianTest))]
	public async Task UInt256_TryRead_BigEndianTest(byte[] source, UInt256 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadUInt256BigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int256TryReadBigEndianTest))]
	public async Task Int256_TryRead_BigEndianTest(byte[] source, Int256 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadInt256BigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512TryReadBigEndianTest))]
	public async Task UInt512_TryRead_BigEndianTest(byte[] source, UInt512 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadUInt512BigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int512TryReadBigEndianTest))]
	public async Task Int512_TryRead_BigEndianTest(byte[] source, Int512 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadInt512BigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(QuadTryReadBigEndianTest))]
	public async Task Quad_TryRead_BigEndianTest(byte[] source, Quad expected, bool successful)
	{
		bool result = BinaryOperations.TryReadQuadBigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsBitwiseEquivalentTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(OctoTryReadBigEndianTest))]
	public async Task Octo_TryRead_BigEndianTest(byte[] source, Octo expected, bool successful)
	{
		bool result = BinaryOperations.TryReadOctoBigEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsBitwiseEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256TryReadLittleEndianTest))]
	public async Task UInt256_TryRead_LittleEndianTest(byte[] source, UInt256 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadUInt256LittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int256TryReadLittleEndianTest))]
	public async Task Int256_TryRead_LittleEndianTest(byte[] source, Int256 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadInt256LittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512TryReadLittleEndianTest))]
	public async Task UInt512_TryRead_LittleEndianTest(byte[] source, UInt512 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadUInt512LittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(Int512TryReadLittleEndianTest))]
	public async Task Int512_TryRead_LittleEndianTest(byte[] source, Int512 expected, bool successful)
	{
		bool result = BinaryOperations.TryReadInt512LittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsEqualTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(QuadTryReadLittleEndianTest))]
	public async Task Quad_TryRead_LittleEndianTest(byte[] source, Quad expected, bool successful)
	{
		bool result = BinaryOperations.TryReadQuadLittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsBitwiseEquivalentTo(expected);
	}
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(OctoTryReadLittleEndianTest))]
	public async Task Octo_TryRead_LittleEndianTest(byte[] source, Octo expected, bool successful)
	{
		bool result = BinaryOperations.TryReadOctoLittleEndian(source, out var actual);
		await Assert.That(result).IsEqualTo(successful);
		await Assert.That(actual).IsBitwiseEquivalentTo(expected);
	}
}