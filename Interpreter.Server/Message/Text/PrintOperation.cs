using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class PrintOperation(
	Print print,
	ConversationHistory conversations
) : Operation<PrintOperation.Input, PrintOperation.Output>
{
	public class Input
	{
		public required string Text { get; init; }
		public required string UserId { get; init; }
		public required string ConversationId { get; init; }
	}

	public class Output { }

	readonly Print Print = print;
	readonly ConversationHistory Conversations = conversations;

	public async Task<Output> Execute(Input input)
	{
		var messageId = await Print.SystemTextMessage(input.UserId, input.Text);

		await Conversations.AddMessage(input.UserId, input.ConversationId, messageId);

		return new();
	}
}
