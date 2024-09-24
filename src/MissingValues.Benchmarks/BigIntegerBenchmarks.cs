using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace MissingValues.Benchmarks;

[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[ShortRunJob]
public class BigIntegerBenchmarks
{
	[ParamsSource(nameof(ValuesSource))]
	public UInt128 Value { get; set; }
	BigInteger b256, b512;
	UInt256 u256;
	UInt512 u512;

	[GlobalSetup]
	public void Setup()
	{
		u256 = (UInt256)(Int256.MaxValue / 2);
		u512 = (UInt512)(Int512.MaxValue / 2);

		Span<byte> bytes = stackalloc byte[UInt256.Size];
		Int256.MaxValue.TryWriteLittleEndian(bytes);

		b256 = new BigInteger(bytes, true);

		bytes = stackalloc byte[UInt512.Size];
		Int512.MaxValue.TryWriteLittleEndian(bytes);

		b512 = new BigInteger(bytes, true);
	}

	[Benchmark]
	public BigInteger Add_BigInteger256()
	{
		return b256 + Value;
	}
	[Benchmark]
	public BigInteger Add_BigInteger512()
	{
		return b512 + Value;
	}
	[Benchmark]
	public UInt256 Add_UInt256()
	{
		return u256 + Value;
	}
	[Benchmark]
	public UInt512 Add_UInt512()
	{
		return u512 + Value;
	}

	[Benchmark]
	public BigInteger Subtract_BigInteger256()
	{
		return b256 - Value;
	}
	[Benchmark]
	public BigInteger Subtract_BigInteger512()
	{
		return b512 - Value;
	}
	[Benchmark]
	public UInt256 Subtract_UInt256()
	{
		return u256 - Value;
	}
	[Benchmark]
	public UInt512 Subtract_UInt512()
	{
		return u512 - Value;
	}

	[Benchmark]
	public BigInteger Multiply_BigInteger256()
	{
		return b256 * Value;
	}
	[Benchmark]
	public BigInteger Multiply_BigInteger512()
	{
		return b512 * Value;
	}
	[Benchmark]
	public UInt256 Multiply_UInt256()
	{
		return u256 * Value;
	}
	[Benchmark]
	public UInt512 Multiply_UInt512()
	{
		return u512 * Value;
	}

	[Benchmark]
	public BigInteger Divide_BigInteger256()
	{
		return b256 / Value;
	}
	[Benchmark]
	public BigInteger Divide_BigInteger512()
	{
		return b512 / Value;
	}
	[Benchmark]
	public UInt256 Divide_UInt256()
	{
		return u256 / Value;
	}
	[Benchmark]
	public UInt512 Divide_UInt512()
	{
		return u512 / Value;
	}

	[Benchmark]
	public BigInteger Remainder_BigInteger256()
	{
		return b256 % Value;
	}
	[Benchmark]
	public BigInteger Remainder_BigInteger512()
	{
		return b512 % Value;
	}
	[Benchmark]
	public UInt256 Remainder_UInt256()
	{
		return u256 % Value;
	}
	[Benchmark]
	public UInt512 Remainder_UInt512()
	{
		return u512 % Value;
	}

	public IEnumerable<UInt128> ValuesSource()
	{
		yield return UInt128.Parse("1");
		yield return UInt128.Parse("2");
		yield return UInt128.Parse("10000000000000000000");
		yield return UInt128.Parse("100000000000000000000000000000000000000");
	}
}