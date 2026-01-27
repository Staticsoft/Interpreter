using Staticsoft.Contracts.Abstractions;
using static Staticsoft.Interpreter.Contracts.Chat;

namespace Staticsoft.Interpreter.Server;

public class HistoryEndpoint(
	Identity identity,
	MessageHistory history
) : HttpEndpoint<EmptyRequest, HistoryResponse>
{
	readonly Identity Identity = identity;
	readonly MessageHistory History = history;

	public async Task<HistoryResponse> Execute(EmptyRequest request)
	{
		var messages = await History.GetMessages(Identity.UserId);

		return new()
		{
			Messages = messages
				.Select(message => new TextMessage
				{
					Id = message.Id,
					Text = message.Data.Text,
					Type = $"{message.Data.Origin}"
				}).ToArray()
		};
	}
}
