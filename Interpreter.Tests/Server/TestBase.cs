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
using System.Threading.Channels;

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

	protected Task<Chat.Message[]> RunUntil<T>(string userMessage)
		where T : class, Chat.Message
		=> RunUntil<T>(userMessage, _ => true);

	protected async Task<Chat.Message[]> RunUntil<T>(string userMessage, Func<T, bool> predicate)
		where T : class, Chat.Message
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

		using var cancellation = new CancellationTokenSource();
		var aggregate = Channel.CreateUnbounded<Chat.Message>();

		StreamToAggregate(connection.Receive<Chat.TextMessage>(), aggregate.Writer, cancellation.Token);
		StreamToAggregate(connection.Receive<Chat.TableMessage>(), aggregate.Writer, cancellation.Token);

		var messages = new List<Chat.Message>();

		await foreach (var message in aggregate.Reader.ReadAllAsync())
		{
			messages.Add(message);

			if (message is T typedMessage && predicate(typedMessage))
			{
				cancellation.Cancel();
				break;
			}
		}

		return messages.ToArray();
	}

	static void StreamToAggregate<TMessage>(
		ChannelReader<TMessage> source,
		ChannelWriter<Chat.Message> destination,
		CancellationToken cancellationToken
	)
		where TMessage : Chat.Message
	{
		_ = Task.Run(async () =>
		{
			try
			{
				await foreach (var message in source.ReadAllAsync(cancellationToken))
				{
					await destination.WriteAsync(message, cancellationToken);
				}
			}
			catch (OperationCanceledException)
			{

			}
		}, cancellationToken);
	}
}
