using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues.Benchmarks;
[MemoryDiagnoser]
public class NumberFormatBenchmarks
{
	private readonly NumberFormatInfo? Custom = new NumberFormatInfo() 
	{
		CurrencyPositivePattern = 3,
		CurrencyNegativePattern = 14,
		NumberNegativePattern = 2,
	};

	[Benchmark(Baseline = true)]
	public string Format_Numeric()
	{
		return Int256.MinValue.ToString("C", Custom);
	}
	[Benchmark]
	public string Format_Positive_Currency()
	{
		return Int256.MaxValue.ToString("N", Custom);
	}
	[Benchmark]
	public string Format_Negative_Currency()
	{
		return Int256.MinValue.ToString("N", Custom);
	}
}
