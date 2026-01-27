using Staticsoft.Interpreter.Server;
using System.Text.Json.Serialization;

namespace Staticsoft.TestServer;

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
