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

	public async Task<string> SystemTextMessage(string text, string userId)
	{
		var connection = await Connections.Get(userId);

		return await PrintTextMessage(text, userId, connection.Data.Id, Message.Type.System);
	}

	public async Task<string> SystemTableMessage(string tableId, string userId)
	{
		var connection = await Connections.Get(userId);

		return await PrintTableMessage(tableId, userId, connection.Data.Id, Message.Type.System);
	}

	public Task<string> UserMessage(string text, string userId, string connectionId)
		=> PrintTextMessage(text, userId, connectionId, Message.Type.User);

	Task<string> PrintTextMessage(string text, string userId, string connectionId, Message.Type type)
		=> PrintMessage(
			id => new Chat.TextMessage
			{
				Id = id,
				Text = text,
				Type = $"{type}"
			},
			text,
			userId,
			connectionId,
			type
		);

	Task<string> PrintTableMessage(string tableId, string userId, string connectionId, Message.Type type)
		=> PrintMessage(
			id => new Chat.TableMessage()
			{
				Id = id,
				TableId = tableId
			},
			tableId,
			userId,
			connectionId,
			type
		);

	async Task<string> PrintMessage<T>(Func<string, T> data, string text, string userId, string connectionId, Message.Type type)
	{
		var messageId = NewId.FromTimestamp();

		await History.SaveMessage(messageId, text, userId, type);
		await Server.Send(connectionId, data(messageId));

		return messageId;
	}
}
