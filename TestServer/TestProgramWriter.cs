using Staticsoft.Interpreter.Server;

namespace Staticsoft.TestServer;

public class TestProgramWriter : ProgramWriter
{
	const string Declarations = """
		Task Print(string text)
		    => tools.Print(text);
		""";

	public Task<string> Write(string requirements)
		=> Task.FromResult($"{Declarations}{Environment.NewLine}{WriteProgram(requirements)}");

	static string WriteProgram(string requirements)
		=> requirements switch
		{
			"2 + 2" => """
			await Print("4");
			""",
			_ => "Unrecognized input"
		};
}