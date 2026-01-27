using Staticsoft.Interpreter.Server;

namespace Staticsoft.TestServer;

public class TestProgramWriter : ProgramWriter
{
	const string Declarations = """
		Task TextMessage(string text)
		    => tools.PrintTextMessage(text);

		Task TableMessage<T>(IEnumerable<T> objects)
			=> tools.PrintTableMessage(objects);
		""";

	public Task<string> Write(string requirements)
		=> Task.FromResult($"{Declarations}{Environment.NewLine}{WriteProgram(requirements)}");

	static string WriteProgram(string requirements)
		=> requirements switch
		{
			"2 + 2" => """
			await TextMessage("4");
			""",

			"table" => $$"""
			var data = new TestTable[]
			{
				new() { Id = 1, FirstName = "John", LastName = "Smith", Salary = 50_000, HireDate = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc) },
				new() { Id = 2, FirstName = "Alice", LastName = "Brown", Salary = 100_000, HireDate = new DateTime(2025, 4, 3, 2, 1, 0, DateTimeKind.Utc) }
			};

			await TableMessage(data);
			""",

			_ => "Unrecognized input"
		};
}