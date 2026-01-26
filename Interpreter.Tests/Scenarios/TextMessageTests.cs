using Staticsoft.Interpreter.Contracts;

namespace Staticsoft.Interpreter.Server.Tests;

public class TextMessageTests : TestBase
{
	[Test]
	public async Task ReturnsTextMessage()
	{
		var results = await RunUntil<Chat.TextMessage>(
			"2 + 2",
			message => message.Text == "Task completed"
		);

		results.Should()
			.BeSimilarTo(
				new { Text = "2 + 2", Type = "User" },
				new { Text = "4", Type = "System" },
				new { Text = "Task completed", Type = "System" }
			);
	}
}
