using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Contracts.ASP.Server;
using Staticsoft.Interpreter.Contracts;

namespace Staticsoft.Interpreter.Server;

public abstract class InterpreterStartup
{
	public void ConfigureServices(IServiceCollection services)
		=> RegisterServices(services);

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		=> ConfigureApp(app, env);

	protected virtual IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
		=> app
			.UseRouting()
			.UseServerAPIRouting<Schema>()
			.UseEndpoints(Endpoints);

	protected virtual IServiceCollection RegisterServices(IServiceCollection services)
		=> services
			.UseContracts()
			.UseDatabaseServices()
			.UseFlowServices()
			.UseClaimIdentity()
			.UseProgramRunner()

			.AddSingleton<MessageHistory>()
			.AddSingleton<ConversationHistory>()

			.AddSingleton<TableFactory>();

	void Endpoints(IEndpointRouteBuilder endpoints)
		=> ConfigureEndpoints(endpoints);

	protected virtual IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
		=> endpoints;
}
