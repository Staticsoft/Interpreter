using Staticsoft.Contracts.Abstractions;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.Interpreter.Contracts;

public class Chat(
	HttpEndpoint<WsServerInMessage<Chat.SendMessageRequest>, Chat.SendMessageResponse> sendMessage,
	HttpEndpoint<EmptyRequest, Chat.HistoryResponse> history
)
{
	[Endpoint(HttpMethod.Post)]
	public HttpEndpoint<WsServerInMessage<SendMessageRequest>, SendMessageResponse> SendMessage { get; } = sendMessage;

	[Endpoint(HttpMethod.Get)]
	public HttpEndpoint<EmptyRequest, HistoryResponse> History { get; } = history;

	public class SendMessageRequest
	{
		public required string Text { get; init; }
	}

	public class SendMessageResponse
	{

	}

	public class TableMessage
	{
		public required string Id { get; init; }
		public required string TableId { get; init; }
	}

	public class TextMessage
	{
		public required string Id { get; init; }
		public required string Text { get; init; }
		public required string Type { get; init; }
	}

	public class HistoryResponse
	{
		public class Message
		{
			public required string Id { get; init; }
			public required string Text { get; init; }
			public required string Type { get; init; }
		}

		public required Message[] Messages { get; init; }
	}
}