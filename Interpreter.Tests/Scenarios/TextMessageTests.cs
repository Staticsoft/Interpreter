using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;

namespace Staticsoft.Interpreter.Server.Tests;

public class TextMessageTests : TestBase
{
	[Test]
	public async Task ReturnsTextMessage()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.Text == "Task completed"
		);

		messages
			.Should()
			.BeSimilarTo(
				new { Text = "2 + 2", Type = "User" },
				new { Text = "4", Type = "System" },
				new { Text = "Task completed", Type = "System" }
			);
	}

	[Test]
	public async Task ReturnsTextMessageHistory()
	{
		await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.Text == "Task completed"
		);

		var history = await Api.Chat.History.Execute(new());
		history.Messages
			.Should()
			.BeSimilarTo(
				new { Text = "2 + 2", Type = "User" },
				new { Text = "4", Type = "System" },
				new { Text = "Task completed", Type = "System" }
			);
	}

	[Test]
	public async Task IdsMustMatch()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.Text == "Task completed"
		);

		var history = await Api.Chat.History.Execute(new());
		history.Messages
			.Should()
			.BeSimilarTo(messages.Select(message => new { message.Id }).ToArray());
	}
}

public class TableMessageTests : TestBase
{
	[Test]
	public async Task ReturnsTable()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"table",
			message => message.Text == "Task completed"
		);

		var tableMessage = messages
			.Should()
			.ContainSingle(message => message.As<Chat.TableMessage>() != null)
			.Which
			.As<Chat.TableMessage>();

		tableMessage.TableId
			.Should()
			.NotBeNullOrEmpty();

		var table = await Api.Tables.Get.Execute(tableMessage.TableId);
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
}