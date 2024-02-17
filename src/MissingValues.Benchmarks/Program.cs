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

//BenchmarkRunner.Run<IntegerBenchmarks>();

while (true)
{
	var s = Console.ReadLine();
	if (s is not null)
	{
		Console.WriteLine((LongDouble)Half.Parse(s));
		Console.WriteLine((LongDouble)float.Parse(s));
		Console.WriteLine((LongDouble)double.Parse(s));
		Console.WriteLine((LongDouble)Quad.Parse(s));
	}
	else 
	{ 
		Console.WriteLine("err."); 
	}
}

Console.WriteLine();
Console.ReadLine();