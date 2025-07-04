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
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Environments;

#if DEBUG
Console.WriteLine("Hello World!");
#else
BenchmarkSwitcher
	.FromAssembly(typeof(Program).Assembly)
	.Run(args, DefaultConfig.Instance
		.AddJob(Job
			.Default
			.WithRuntime(CoreRuntime.Core80)
			.WithRuntime(CoreRuntime.Core90)
			));
#endif
Console.ReadLine();