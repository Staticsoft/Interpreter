using Microsoft.Extensions.DependencyInjection;

namespace Staticsoft.Interpreter.Server;

static class ToolsServices
{
	public static IServiceCollection UseProgramRunner(this IServiceCollection services)
		=> services
			.AddSingleton<Print>()
			.AddScoped<UserContext>()

			.AddScoped<PrintTools>()
			.AddScoped<TableTools>()

			.AddSingleton<ProgramAssemblies, RequiredAssemblies>();
}
