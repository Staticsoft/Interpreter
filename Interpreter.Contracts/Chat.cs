using Staticsoft.Contracts.Abstractions;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Contracts;

public class Chat(
	HttpEndpoint<WsServerInMessage<Chat.SendMessageRequest>, Chat.SendMessageResponse> sendMessage,
	HttpEndpoint<EmptyRequest, Chat.HistoryResponse> history
)
{
	[Endpoint(HttpMethod.Post)]
	public HttpEndpoint<WsServerInMessage<SendMessageRequest>, SendMessageResponse> sendMessage { get; } = sendMessage;

	[Endpoint(HttpMethod.Get)]
	public HttpEndpoint<EmptyRequest, HistoryResponse> history { get; } = history;

	public class SendMessageRequest
	{
		public required string text { get; init; }
	}

	public class SendMessageResponse
	{

	}

	public interface Message
	{
		string id { get; }
		string type { get; }
	}

	public class TableMessage : Message
	{
		public required string id { get; init; }
		public string type { get; } = "System";
		public required string tableId { get; init; }
	}

	public class TextMessage : Message
	{
		public required string id { get; init; }
		public required string type { get; init; }
		public required string text { get; init; }
	}

	public class HistoryResponse
	{
		public required object[] messages { get; init; }
	}
}