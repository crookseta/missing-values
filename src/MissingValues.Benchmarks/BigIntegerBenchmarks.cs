using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace MissingValues.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[ShortRunJob]
public class BigIntegerBenchmarks
{
	[ParamsSource(nameof(ValuesSource))]
	public UInt128 Value { get; set; }

	[GlobalSetup]
	public void Setup()
	{
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public UInt256 Add_UInt256(UInt256 a)
	{
		return a + Value;
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public UInt512 Add_UInt512(UInt512 a)
	{
		return a + Value;
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public UInt256 Subtract_UInt256(UInt256 a)
	{
		return a - Value;
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public UInt512 Subtract_UInt512(UInt512 a)
	{
		return a - Value;
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public UInt256 Multiply_UInt256(UInt256 a)
	{
		return a * Value;
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public UInt512 Multiply_UInt512(UInt512 a)
	{
		return a * Value;
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public UInt256 Divide_UInt256(UInt256 a)
	{
		return a / Value;
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public UInt512 Divide_UInt512(UInt512 a)
	{
		return a / Value;
	}

	[Benchmark]
	[ArgumentsSource(nameof(UInt256ParamsSource))]
	public UInt256 Remainder_UInt256(UInt256 a)
	{
		return a % Value;
	}
	[Benchmark]
	[ArgumentsSource(nameof(UInt512ParamsSource))]
	public UInt512 Remainder_UInt512(UInt512 a)
	{
		return a % Value;
	}

	public IEnumerable<UInt128> ValuesSource()
	{
		yield return UInt128.Parse("1");
		yield return UInt128.Parse("2");
		yield return UInt128.Parse("10000000000000000000");
		yield return UInt128.Parse("100000000000000000000000000000000000000");
	}
	public IEnumerable<UInt256> UInt256ParamsSource()
	{
		yield return UInt256.One;
		yield return uint.MaxValue;
		yield return ulong.MaxValue;
		yield return UInt128.MaxValue;
		yield return UInt256.MaxValue / UInt256.Parse("10000000000000000000000000000000000000000");
	}
	public IEnumerable<UInt512> UInt512ParamsSource()
	{
		yield return UInt512.One;
		yield return uint.MaxValue;
		yield return ulong.MaxValue;
		yield return UInt128.MaxValue;
		yield return UInt512.MaxValue / UInt512.Parse("10000000000000000000000000000000000000000");
	}
}