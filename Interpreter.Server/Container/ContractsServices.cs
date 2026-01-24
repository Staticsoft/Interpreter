using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Contracts.ASP.Server;
using Staticsoft.Interpreter.Contracts;
using Staticsoft.PartitionedStorage.Abstractions;
using Staticsoft.Serialization.Net;
using System.Reflection;

namespace Staticsoft.Interpreter.Server;

static class ContractsServices
{
	public static IServiceCollection UseContracts(this IServiceCollection services)
		=> services
			.UseServerAPI<Schema>(Assembly.GetExecutingAssembly())
			.UseSystemJsonSerializer()
			.AddSingleton<ItemSerializer, JsonItemSerializer>();
}