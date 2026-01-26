namespace Staticsoft.Interpreter.Server;

static class NewId
{
	public static string FromTimestamp()
		=> $"{Timestamp:D15}";

	static long Timestamp
		=> ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
}