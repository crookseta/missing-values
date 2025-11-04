using System.Globalization;
using System.Text.Json;

namespace MissingValues.Tests.Core;

public class JsonConverterTests
{
	[Test]
	public async Task JsonWriteTest()
	{
		await Assert.That(JsonSerializer.Serialize(new object[] { UInt256.MaxValue, UInt256.MinValue })).EqualTo($"[{UInt256.MaxValue},{UInt256.MinValue}]");
		await Assert.That(JsonSerializer.Serialize(new object[] { Int256.MaxValue, Int256.MinValue })).EqualTo($"[{Int256.MaxValue},{Int256.MinValue}]");
		await Assert.That(JsonSerializer.Serialize(new object[] { UInt512.MaxValue, UInt512.MinValue })).EqualTo($"[{UInt512.MaxValue},{UInt512.MinValue}]");
		await Assert.That(JsonSerializer.Serialize(new object[] { Int512.MaxValue, Int512.MinValue })).EqualTo($"[{Int512.MaxValue},{Int512.MinValue}]");
		await Assert.That(JsonSerializer.Serialize(new object[] { Quad.MaxValue, Quad.MinValue })).EqualTo($"[{Quad.MaxValue.ToString("G", CultureInfo.InvariantCulture)},{Quad.MinValue.ToString("G", CultureInfo.InvariantCulture)}]");
		await Assert.That(JsonSerializer.Serialize(new object[] { Octo.MaxValue, Octo.MinValue })).EqualTo($"[{Octo.MaxValue.ToString("G", CultureInfo.InvariantCulture)},{Octo.MinValue.ToString("G", CultureInfo.InvariantCulture)}]");
	}
	[Test]
	public async Task JsonReadTest()
	{
		await Assert.That(JsonSerializer.Deserialize<UInt256>(UInt256.MaxValue.ToString())).EqualTo(UInt256.MaxValue);
		await Assert.That(JsonSerializer.Deserialize<Int256[]>($"[{Int256.MaxValue},{Int256.MinValue}]")).IsEquivalentTo([Int256.MaxValue, Int256.MinValue]);
		await Assert.That(JsonSerializer.Deserialize<UInt512>(UInt512.MaxValue.ToString())).EqualTo(UInt512.MaxValue);
		await Assert.That(JsonSerializer.Deserialize<Int512[]>($"[{Int512.MaxValue},{Int512.MinValue}]")).IsEquivalentTo([Int512.MaxValue, Int512.MinValue]);
		await Assert.That(JsonSerializer.Deserialize<Quad>(Quad.MaxValue.ToString("G", CultureInfo.InvariantCulture))).EqualTo(Quad.MaxValue);
		await Assert.That(JsonSerializer.Deserialize<Octo>(Octo.MaxValue.ToString("G", CultureInfo.InvariantCulture))).EqualTo(Octo.MaxValue);
	}
}