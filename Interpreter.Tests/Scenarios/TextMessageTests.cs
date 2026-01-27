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
