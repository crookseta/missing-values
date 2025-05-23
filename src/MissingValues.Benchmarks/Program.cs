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
string? s = UInt512.MaxValue.ToString();
Console.WriteLine(s);
Console.WriteLine("Hello World!");
#else
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
#endif
Console.ReadLine();