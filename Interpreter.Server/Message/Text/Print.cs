using Staticsoft.Interpreter.Contracts;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class Print(
	WsServer server,
	PartitionConnectionsByUsers connections,
	MessageHistory history
)
{
	readonly WsServer Server = server;
	readonly PartitionConnectionsByUsers Connections = connections;
	readonly MessageHistory History = history;

	public Task<string> UserTextMessage(string userId, string connectionId, string text)
		=> PrintTextMessage(userId, connectionId, text, "User");

	public async Task<string> SystemTextMessage(string userId, string text)
	{
		var connection = await Connections.Get(userId);

		return await PrintTextMessage(userId, connection.Data.Id, text, "System");
	}

	public async Task<string> SystemTableMessage(string userId, string tableId)
	{
		var connection = await Connections.Get(userId);

		return await PrintTableMessage(userId, connection.Data.Id, tableId);
	}

	Task<string> PrintTextMessage(string userId, string connectionId, string text, string type)
		=> PrintMessage(
			userId,
			connectionId,
			id => new Chat.TextMessage
			{
				Id = id,
				Text = text,
				Type = type
			});

	Task<string> PrintTableMessage(string userId, string connectionId, string tableId)
		=> PrintMessage(
			userId,
			connectionId,
			id => new Chat.TableMessage()
			{
				Id = id,
				TableId = tableId
			});

	async Task<string> PrintMessage<T>(string userId, string connectionId, Func<string, T> getData)
		where T : Chat.Message
	{
		var messageId = NewId.FromTimestamp();
		var data = getData(messageId);

		await History.SaveMessage(userId, messageId, data);
		await Server.Send(connectionId, getData(messageId));

		return messageId;
	}
}
