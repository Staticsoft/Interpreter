global using PartitionConnectionsByUsers = Staticsoft.PartitionedStorage.Abstractions.Partition<Staticsoft.Interpreter.Server.ConnectionByUser>;
global using PartitionConversations = Staticsoft.PartitionedStorage.Abstractions.PartitionFactory<Staticsoft.Interpreter.Server.Conversation>;
global using PartitionMessages = Staticsoft.PartitionedStorage.Abstractions.PartitionFactory<Staticsoft.Interpreter.Server.Message>;
global using PartitionTables = Staticsoft.PartitionedStorage.Abstractions.PartitionFactory<Staticsoft.Interpreter.Server.Table>;
global using PartitionUsersByConnections = Staticsoft.PartitionedStorage.Abstractions.Partition<Staticsoft.Interpreter.Server.UserByConnection>;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.PartitionedStorage.Abstractions;

namespace Staticsoft.Interpreter.Server;

static class DatabaseServices
{
	public static IServiceCollection UseDatabaseServices(this IServiceCollection services) => services
		.AddSingleton(p => p.GetRequiredService<Partitions>().Get<UserByConnection>())
		.AddSingleton(p => p.GetRequiredService<Partitions>().Get<ConnectionByUser>())
		.AddSingleton(p => p.GetRequiredService<Partitions>().GetFactory<Conversation>("Conversations-"))
		.AddSingleton(p => p.GetRequiredService<Partitions>().GetFactory<Message>("Messages-"))
		.AddSingleton(p => p.GetRequiredService<Partitions>().GetFactory<Table>("Tables-"));
}