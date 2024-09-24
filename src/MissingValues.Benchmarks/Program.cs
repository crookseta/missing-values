using MissingValues.Benchmarks;
using BenchmarkDotNet.Running;
using MissingValues;
using System.Numerics;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Windows.Markup;
using MissingValues.Internals;

#if DEBUG

Console.WriteLine(Quad.Sqrt(10));
Console.WriteLine(Octo.Sqrt(10));

#else
BenchmarkSwitcher.FromTypes(
	[
		typeof(UInt256Benchmarks.MathOperators),
		typeof(UInt256Benchmarks.ParsingAndFormatting),
		typeof(Int256Benchmarks.MathOperators),
		typeof(Int256Benchmarks.ParsingAndFormatting),
		typeof(UInt512Benchmarks.MathOperators),
		typeof(UInt512Benchmarks.ParsingAndFormatting),
		typeof(Int512Benchmarks.MathOperators),
		typeof(Int512Benchmarks.ParsingAndFormatting),
		typeof(QuadBenchmarks.MathOperators),
		typeof(QuadBenchmarks.Parsing),
		typeof(GenericIntegerBenchmarks<>),
		typeof(BigIntegerBenchmarks)
	]
	).Run(args); 
#endif
Console.ReadLine();