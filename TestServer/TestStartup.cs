using Microsoft.CodeAnalysis;
using Staticsoft.Flow.Memory;
using Staticsoft.Interpreter.Server;
using Staticsoft.Messages.Memory;
using Staticsoft.PartitionedStorage.Abstractions;
using Staticsoft.PartitionedStorage.Memory;
using Staticsoft.WsCommunication.Server.Local;

namespace Staticsoft.TestServer;

public class TestStartup : InterpreterStartup
{
	protected override IServiceCollection RegisterServices(IServiceCollection services)
		=> base.RegisterServices(services)
			.AddSingleton<Partitions, MemoryPartitions>()
			.UseLocalFlow(_ => new() { Paralellism = 10 })
			.UseMemoryQueue(_ => new()
			{
				Invisibility = TimeSpan.FromMinutes(10),
				PollingInterval = TimeSpan.FromMilliseconds(100)
			})
			.UseLocalWsCommunication()
			.AddSingleton(new HttpClient() { BaseAddress = new Uri(BackendBaseAddress()) })
			.AddScoped<Identity, TestIdentity>()
			.AddSingleton<ProgramWriter, TestProgramWriter>()
			.AddScoped<ProgramRunner, ProgramRunner<TestToolset>>()
			.AddScoped<TestToolset>()
			.Decorate<ProgramAssemblies, TestAssemblies>();

	static string BackendBaseAddress()
		=> $"http://localhost:{Environment.GetEnvironmentVariable("ASPNETCORE_PORT")}";

	protected override IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
		=> base.ConfigureApp(app, env)
			.UseLocalWsCommunication();
}

public class TestAssemblies(
	ProgramAssemblies assemblies
) : ProgramAssemblies
{
	public PortableExecutableReference[] References { get; } =
	[
		..assemblies.References,
		MetadataReference.CreateFromFile(typeof(TestToolset).Assembly.Location),
	];
}