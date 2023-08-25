using MissingValues.Benchmarks;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run(
	new Type[]
	{
		typeof(UInt256Benchmarks.MathOperators),
		typeof(UInt256Benchmarks.Formatting),
		typeof(UInt256Benchmarks.Parsing),
		typeof(UInt256Benchmarks.Operations), 
		typeof(UInt512Benchmarks.MathOperators),
		typeof(UInt512Benchmarks.Formatting),
		typeof(UInt512Benchmarks.Parsing),
		typeof(UInt512Benchmarks.Operations)
	}
);

Console.ReadLine();