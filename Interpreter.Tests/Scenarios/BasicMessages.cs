using Xunit;

namespace Staticsoft.Interpreter.Server.Tests;

public class BasicMessages : TestBase
{
	[Fact]
	public async Task RunsBasicProgram()
	{
		var results = await Run("2 + 2");
		Assert.Contains("4", results);
	}
}