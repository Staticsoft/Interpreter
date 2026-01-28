using Staticsoft.Contracts.Abstractions;
using Staticsoft.Flow;
using Staticsoft.Interpreter.Server;
using Staticsoft.WsCommunication.Server.Abstractions;
using static Staticsoft.Interpreter.Contracts.Chat;

namespace Staticsoft.Interpreter;

public class SendMessageEndpoint(
	Job<ProcessMessageJob.Input, ProcessMessageJob.Output> processMessage,
	PartitionUsersByConnections users,
	Print print
) : HttpEndpoint<WsServerInMessage<SendMessageRequest>, SendMessageResponse>
{
	readonly Job<ProcessMessageJob.Input, ProcessMessageJob.Output> ProcessMessage = processMessage;
	readonly PartitionUsersByConnections Users = users;
	readonly Print Print = print;

	public async Task<SendMessageResponse> Execute(WsServerInMessage<SendMessageRequest> request)
	{
		var user = await Users.Get(request.ConnectionId);
		var (text, userId) = (request.Body.Text, user.Data.Id);

		var messageId = await Print.UserTextMessage(userId, request.ConnectionId, text);

		await ProcessMessage.Create(new()
		{
			MessageId = messageId,
			Text = text,
			UserId = userId
		});

		return new();
	}
}
