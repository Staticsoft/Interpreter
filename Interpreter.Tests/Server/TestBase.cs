using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Contracts.Abstractions;
using Staticsoft.Contracts.ASP.Client;
using Staticsoft.HttpCommunication.Json;
using Staticsoft.Interpreter.Contracts;
using Staticsoft.PartitionedStorage.Abstractions;
using Staticsoft.PartitionedStorage.Memory;
using Staticsoft.Serialization.Net;
using Staticsoft.Testing.Integration;
using Staticsoft.TestServer;
using Staticsoft.WsCommunication.Client.Abstractions;
using Staticsoft.WsCommunication.Client.Testing;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Server.Tests;

public class TestBase : IntegrationTestBase<TestStartup>
{
	readonly GracefullDisconnect Disconnect = new();

	protected override IServiceCollection ClientServices(IServiceCollection services)
		=> base.ClientServices(services)
			.UseTestHostWsClient()
			.UseClientAPI<Schema>()
			.UseSystemJsonSerializer()
			.UseJsonHttpCommunication()

			.Decorate<WsClient, GracefulWsClient>()
			.AddSingleton(Disconnect);

	protected override IServiceCollection ServerServices(IServiceCollection services)
		=> base.ServerServices(services)
			.AddSingleton<Partitions, MemoryPartitions>()
			.AddSingleton(_ => Client<HttpClient>())

			.Decorate<HttpEndpoint<WsDisconnectRequest, WebSocket.DisconnectResponse>, GracefulDisconnectEndpoint>()
			.AddSingleton(Disconnect);

	protected Schema Api
		=> Client<Schema>();

	protected WsClient Client
		=> Client<WsClient>();

	protected async Task<IReadOnlyCollection<string>> Run(string userMessage)
	{
		await using var connection = await Client.Connect();

		await connection.Send<Chat.SendMessageRequest>(new()
		{
			Path = "/Chat/SendMessage",
			Body = new()
			{
				Text = userMessage
			}
		});

		var textMessagesChannel = connection.Receive<Chat.TextMessage>();
		var tableMessagesChannel = connection.Receive<Chat.TableMessage>();

		var textMessages = new List<string>();

		var waitText = textMessagesChannel.ReadAsync().AsTask();
		var waitTable = tableMessagesChannel.ReadAsync().AsTask();

		var text = string.Empty;

		while (text != "Task completed")
		{
			var task = await Task.WhenAny([waitText, waitTable]);

			if (task == waitText)
			{
				text = (await waitText).Text;
				waitText = textMessagesChannel.ReadAsync().AsTask();
			}
			else
			{
				text = (await waitTable).TableId;
				waitTable = tableMessagesChannel.ReadAsync().AsTask();
			}

			textMessages.Add(text);
		}

		return textMessages;
	}
}
