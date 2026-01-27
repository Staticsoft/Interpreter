using Staticsoft.Interpreter.Server;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Staticsoft.TestServer;

public class TestToolset(
	PrintTools printTools,
	TableTools tableTools
)
{
	readonly PrintTools PrintTools = printTools;
	readonly TableTools TableTools = tableTools;

	public Task PrintTextMessage(string text)
		=> PrintTools.SystemMessage(text);

	public Task PrintTableMessage<T>(IEnumerable<T> objects)
		where T : notnull
		=> TableTools.Show(objects);
}

public class TestTable
{
	public required int Id { get; init; }
	public required string FirstName { get; init; }
	public required string LastName { get; init; }
	public required decimal Salary { get; init; }
	public required DateTime HireDate { get; init; }
}

public class TestView
{
	[JsonPropertyOrder(1)]
	[ColumnType(ColumnType.Number)]
	public required int Id { get; init; }

	[JsonPropertyOrder(2)]
	[ColumnType(ColumnType.Text)]
	public required string Name { get; init; }

	[JsonPropertyOrder(3)]
	[ColumnType(ColumnType.Money)]
	public required decimal Salary { get; init; }

	[JsonPropertyOrder(4)]
	[ColumnType(ColumnType.Date)]
	public required string HireDate { get; init; }
}

public class TestTableConverter : TableConverter<TestTable, TestView>
{
	public TestView Convert(TestTable data)
		=> new()
		{
			Id = data.Id,
			Name = $"{data.FirstName} {data.LastName}",
			Salary = data.Salary,
			HireDate = data.HireDate.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture)
		};
}