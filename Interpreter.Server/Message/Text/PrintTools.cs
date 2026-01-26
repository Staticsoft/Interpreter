using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class PrintTools(
	Operation<PrintOperation.Input, PrintOperation.Output> print,
	UserContext context
)
{
	readonly Operation<PrintOperation.Input, PrintOperation.Output> Print = print;
	readonly UserContext Context = context;

	public async Task SystemMessage(string text)
		=> await Print.Execute(new()
		{
			Text = text,
			UserId = Context.UserId,
			ConversationId = Context.ConversationId
		});
}
