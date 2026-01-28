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
			message => message.Text == "Task completed"
		);

		var tableId = messages
			.OfType<Chat.TableMessage>()
			.Should()
			.ContainSingle()
			.Which
			.TableId
			.Should()
			.NotBeNullOrEmpty()
			.And
			.Subject;

		var table = await Api.Tables.Get.Execute(tableId);
		table.Columns
			.Should()
			.BeSimilarTo(
				new { Title = "Id", DataType = "Number" },
				new { Title = "Name", DataType = "Text" },
				new { Title = "Salary", DataType = "Money" },
				new { Title = "HireDate", DataType = "Date" }
			);
		table.Rows
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
			message => message.Text == "Task completed"
		);

		var tableId = messages
			.OfType<Chat.TableMessage>()
			.Single()
			.TableId;

		var history = await Api.Chat.History.Execute(new());
		history.Messages
			.Should()
			.BeSimilarTo(
				new { Text = "table", Type = "User" },
				new { TableId = tableId, Type = "System" },
				new { Text = "Task completed", Type = "System" }
			);
	}
}