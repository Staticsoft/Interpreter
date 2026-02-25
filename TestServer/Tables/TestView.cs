using Staticsoft.Interpreter.Server;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Staticsoft.TestServer;

public class TestView
{
	[JsonPropertyOrder(1)]
	[ColumnType(ColumnType.number)]
	[DisplayName("Id")]
	public required int Id { get; init; }

	[JsonPropertyOrder(2)]
	[ColumnType(ColumnType.text)]
	[DisplayName("Name")]
	public required string Name { get; init; }

	[JsonPropertyOrder(3)]
	[ColumnType(ColumnType.money)]
	[DisplayName("Salary")]
	public required decimal Salary { get; init; }

	[JsonPropertyOrder(4)]
	[ColumnType(ColumnType.date)]
	[DisplayName("Hire Date")]
	public required string HireDate { get; init; }
}
