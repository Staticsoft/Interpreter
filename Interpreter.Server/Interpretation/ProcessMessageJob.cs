using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class ProcessMessageJob(
	Operation<FindConversationOperation.Input, FindConversationOperation.Output> findConversation,
	Operation<CreateProgramOperation.Input, CreateProgramOperation.Output> createProgram,
	Operation<PrintOperation.Input, PrintOperation.Output> print,
	ProgramRunner runner,
	UserContext context
) : Job<ProcessMessageJob.Input, ProcessMessageJob.Output>
{
	readonly Operation<FindConversationOperation.Input, FindConversationOperation.Output> FindConversation = findConversation;
	readonly Operation<CreateProgramOperation.Input, CreateProgramOperation.Output> CreateProgram = createProgram;
	readonly Operation<PrintOperation.Input, PrintOperation.Output> Print = print;
	readonly ProgramRunner Runner = runner;
	readonly UserContext Context = context;

	public class Input
	{
		public required string MessageId { get; init; }
		public required string Text { get; init; }
		public required string UserId { get; init; }
	}
	public class Output { }

	public async Task<Output> Execute(Input input)
	{
		var conversation = await FindConversation.Execute(new()
		{
			MessageId = input.MessageId,
			UserId = input.UserId
		});

		(Context.UserId, Context.ConversationId) = (input.UserId, conversation.Id);

		var program = await CreateProgram.Execute(new() { Requirements = input.Text });

		await Runner.Run(program.Code, input.UserId, conversation.Id);

		await Print.Execute(new()
		{
			Text = "Task completed",
			UserId = input.UserId,
			ConversationId = conversation.Id
		});

		return new();
	}
}

public class UserContext
{
	public string UserId { get; set; } = string.Empty;
	public string ConversationId { get; set; } = string.Empty;
}