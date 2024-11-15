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

UInt512 b = UInt512.MaxValue / UInt512.Parse("10000000000000000000000000000000000000000");
UInt256 a = UInt256.Parse("100000000000000000000000000000000000000");
UInt512 c = UInt512.Parse("13407807929942597099574024998205846127479365820592393377723561443721764030073");

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
		typeof(BigIntegerBenchmarks),
		typeof(WriteUnalignedBenchmarks),
		typeof(ReadUnalignedBenchmarks)
	]
	).Run(args);

#endif
Console.ReadLine();