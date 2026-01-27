using Staticsoft.Interpreter.Server;

namespace Staticsoft.TestServer;

public class TestToolset(
	PrintTools printTools,
	TableTools tableTools
)
{
	readonly PrintTools PrintTools = printTools;
	readonly TableTools TableTools = tableTools;

	public Task PrintTextMessage(string text)
		=> PrintTools.SystemMessage(text);

	public Task PrintTableMessage<T>(IEnumerable<T> objects)
		where T : notnull
		=> TableTools.Show(objects);
}
