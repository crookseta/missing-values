using MissingValues;
using MissingValues.Benchmarks;
using System.Globalization;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run(
//	new Type[] 
//	{
//		typeof(UInt256Benchmarks.MathOperators), 
//		typeof(UInt256Benchmarks.Formatting), 
//		typeof(UInt256Benchmarks.Parsing), 
//		typeof(UInt256Benchmarks.Operations)
//	}
//);

BenchmarkRunner.Run<IntegerBenchmarks>();

Console.ReadLine();