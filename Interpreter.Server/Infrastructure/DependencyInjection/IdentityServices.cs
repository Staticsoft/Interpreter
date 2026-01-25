using Microsoft.Extensions.DependencyInjection;

namespace Staticsoft.Interpreter.Server;

static class IdentityServices
{
	public static IServiceCollection UseClaimIdentity(this IServiceCollection services)
		=> services
			.AddScoped<Identity, HttpContextIdentity>()
			.AddHttpContextAccessor();
}