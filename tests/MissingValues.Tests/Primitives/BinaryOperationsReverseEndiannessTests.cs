using MissingValues.Primitives;
using MissingValues.Tests.Data;
using static MissingValues.Tests.Data.BinaryOperationsDataSources;

namespace MissingValues.Tests.Primitives;

public class BinaryOperationsReverseEndiannessTests
{
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt256ReverseEndiannessTest))]
	public async Task UInt256_ReverseEndianness(UInt256 value, UInt256 expected)
	{
		var actual = BinaryOperations.ReverseEndianness(value);
		
		await Assert.That(actual).IsEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(UInt512ReverseEndiannessTest))]
	public async Task UInt512_ReverseEndianness(UInt512 value, UInt512 expected)
	{
		var actual = BinaryOperations.ReverseEndianness(value);
		
		await Assert.That(actual).IsEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(SpanUInt256ReverseEndiannessTest))]
	public async Task SpanUInt256_ReverseEndianness(UInt256[] value, UInt256[] expected)
	{
		UInt256[] destination = new UInt256[value.Length];
		BinaryOperations.ReverseEndianness(value, destination);
		
		await Assert.That(destination).IsEquivalentTo(expected);
	}
	
	[Test]
	[MethodDataSource<BinaryOperationsDataSources>(nameof(SpanUInt512ReverseEndiannessTest))]
	public async Task SpanUInt512_ReverseEndianness(UInt512[] value, UInt512[] expected)
	{
		UInt512[] destination = new UInt512[value.Length];
		BinaryOperations.ReverseEndianness(value, destination);
		
		await Assert.That(destination).IsEquivalentTo(expected);
	}
}