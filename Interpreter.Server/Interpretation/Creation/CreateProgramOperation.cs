using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class CreateProgramOperation(
	ProgramWriter program
) : Operation<CreateProgramOperation.Input, CreateProgramOperation.Output>
{
	public class Input
	{
		public required string Requirements { get; init; }
	}
	public class Output
	{
		public required string Code { get; init; }
	}

	readonly ProgramWriter Program = program;

	public async Task<Output> Execute(Input input)
	{
		var code = await Program.Write(input.Requirements);

		return new Output() { Code = code };
	}
}
