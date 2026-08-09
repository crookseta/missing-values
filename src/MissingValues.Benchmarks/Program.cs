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
Job enough = Job.Default.WithRuntime(CoreRuntime.Core10_0);
	
IConfig config = DefaultConfig.Instance
	.HideColumns(Column.RatioSD, Column.Error)
	.AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig
		(exportGithubMarkdown: true, printInstructionAddresses: false)))
	.AddJob(enough.WithEnvironmentVariable("DOTNET_EnableHWIntrinsic", "0").WithId("Scalar").AsBaseline());

if (Vector512.IsHardwareAccelerated)
{
	config = config
		.AddJob(enough.WithId("Vector512"))
		.AddJob(enough.WithEnvironmentVariable("DOTNET_EnableAVX512F", "0").WithId("Vector256"))
		.AddJob(enough.WithEnvironmentVariable("DOTNET_EnableAVX512F", "0").WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128"));
}
else if (Vector256.IsHardwareAccelerated)
{
	config = config
		.AddJob(enough.WithId("Vector256"))
		.AddJob(enough.WithEnvironmentVariable("DOTNET_EnableAVX2", "0").WithId("Vector128"));
}

BenchmarkSwitcher
	.FromAssembly(typeof(Program).Assembly)
	.Run(args, config);
#endif
Console.ReadLine();