using BenchmarkDotNet.Running;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Environments;

#if DEBUG
Console.WriteLine("Hello World!");
#else
Job dotnet10 = Job.Default.WithRuntime(CoreRuntime.Core10_0);
Job dotnet11 = Job.Default.WithRuntime(CoreRuntime.Core11_0);
	
IConfig config = DefaultConfig.Instance
	.HideColumns(Column.RatioSD, Column.Error)
	.AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig
		(exportGithubMarkdown: true, printInstructionAddresses: false)))
	.AddJob(dotnet10.WithEnvironmentVariable("DOTNET_EnableHWIntrinsic", "0").WithId("Scalar 10.0").AsBaseline())
	.AddJob(dotnet11.WithEnvironmentVariable("DOTNET_EnableHWIntrinsic", "0").WithId("Scalar 11.0"))
	;

if (Vector512.IsHardwareAccelerated)
{
	config = config
		.AddJob(dotnet10.WithId("Vector512 10.0"))
		.AddJob(dotnet10.WithEnvironmentVariable("DOTNET_EnableAVX512", "0").WithId("Vector256 10.0"))
		.AddJob(dotnet10.WithEnvironmentVariable("DOTNET_EnableAVX512", "0").WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128 10.0"))
		;
	config = config
		.AddJob(dotnet11.WithId("Vector512 11.0"))
		.AddJob(dotnet11.WithEnvironmentVariable("DOTNET_EnableAVX512", "0").WithId("Vector256 11.0"))
		.AddJob(dotnet11.WithEnvironmentVariable("DOTNET_EnableAVX512", "0").WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128 11.0"))
		;
}
else if (Vector256.IsHardwareAccelerated)
{
	config = config
		.AddJob(dotnet10.WithId("Vector256 10.0"))
		.AddJob(dotnet10.WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128 10.0"))
		;
	config = config
		.AddJob(dotnet11.WithId("Vector256 11.0"))
		.AddJob(dotnet11.WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128 11.0"))
		;
}

BenchmarkSwitcher
	.FromAssembly(typeof(Program).Assembly)
	.Run(args, config);
#endif
Console.ReadLine();