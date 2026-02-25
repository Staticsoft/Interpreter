using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;

namespace Staticsoft.Interpreter.Server.Tests;

public class TableMessageTests : TestBase
{
	[Test]
	public async Task ReturnsTable()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"table",
			message => message.text == "Task completed"
		);

		var tableId = messages
			.OfType<Chat.TableMessage>()
			.Should()
			.ContainSingle()
			.Which
			.tableId
			.Should()
			.NotBeNullOrEmpty()
			.And
			.Subject;

		var table = await Api.tables.get.Execute(tableId);
		table.columns
			.Should()
			.BeSimilarTo(
				new { key = "Id", title = "Id", dataType = "number" },
				new { key = "Name", title = "Name", dataType = "text" },
				new { key = "Salary", title = "Salary", dataType = "money" },
				new { key = "HireDate", title = "Hire Date", dataType = "date" }
			);
		table.rows
			.Should()
			.BeSimilarTo(
				new { Id = 1, Name = "John Smith", Salary = 50_000, HireDate = "2020-01-02T03:04:05.0000000Z" },
				new { Id = 2, Name = "Alice Brown", Salary = 100_000, HireDate = "2025-04-03T02:01:00.0000000Z" }
			);
	}

	[Test]
	public async Task ReturnsTableMessageHistory()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"table",
			message => message.text == "Task completed"
		);

		var tableId = messages
			.OfType<Chat.TableMessage>()
			.Single()
			.tableId;

		var history = await Api.chat.history.Execute(new());
		history.messages
			.Should()
			.BeSimilarTo(
				new { text = "table", type = "user" },
				new { tableId = tableId, type = "system" },
				new { text = "Task completed", type = "system" }
			);
	}
}