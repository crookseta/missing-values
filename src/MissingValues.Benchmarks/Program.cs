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

//BenchmarkRunner.Run(
//	new Type[]
//	{
//		typeof(UInt256Benchmarks.MathOperators),
//		typeof(UInt256Benchmarks.Formatting),
//		typeof(UInt256Benchmarks.Parsing),
//		typeof(UInt256Benchmarks.Operations),
//		typeof(UInt512Benchmarks.MathOperators),
//		typeof(UInt512Benchmarks.Formatting),
//		typeof(UInt512Benchmarks.Parsing),
//		typeof(UInt512Benchmarks.Operations),
//		typeof(QuadBenchmarks.MathOperators),
//		typeof(QuadBenchmarks.Parser),
//	}
//);

BenchmarkRunner.Run<IntegerBenchmarks>();
//var s = NumberFormatter.FormatUnsignedNumber<UInt512, Int512>(new UInt512(0x0, 0x0, 0x0, 0x0, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), "d", NumberStyles.Integer, CultureInfo.CurrentCulture);

//Console.WriteLine(s);
Console.ReadLine();