using Xunit;

namespace Staticsoft.Interpreter.Server.Tests;

public class TextMessageTests : TestBase
{
	[Fact]
	public async Task ReturnsTextMessage()
	{
		var results = await Run("2 + 2");
		Assert.Contains("4", results);
	}
}