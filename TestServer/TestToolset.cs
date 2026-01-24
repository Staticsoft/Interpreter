namespace Staticsoft.Interpreter.Server;

public class TestToolset(
	PrintTools printTools
)
{
	readonly PrintTools PrintTools = printTools;

	public Task Print(string text)
		=> PrintTools.SystemMessage(text);
}
