using Staticsoft.Interpreter.Contracts;

namespace Staticsoft.Interpreter.Server.Tests;

public class TextMessageTests : TestBase
{
	[Test]
	public async Task ReturnsTextMessage()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.text == "Task completed"
		);

		messages
			.Should()
			.BeSimilarTo(
				new { text = "2 + 2", type = "User" },
				new { text = "4", type = "System" },
				new { text = "Task completed", type = "System" }
			);
	}

	[Test]
	public async Task ReturnsTextMessageHistory()
	{
		await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.text == "Task completed"
		);

		var history = await Api.chat.history.Execute(new());
		history.messages
			.Should()
			.BeSimilarTo(
				new { text = "2 + 2", type = "User" },
				new { text = "4", type = "System" },
				new { text = "Task completed", type = "System" }
			);
	}

	[Test]
	public async Task IdsMustMatch()
	{
		var messages = await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.text == "Task completed"
		);

		var history = await Api.chat.history.Execute(new());
		history.messages
			.Should()
			.BeSimilarTo(messages.Select(message => new { message.id }).ToArray());
	}
}
