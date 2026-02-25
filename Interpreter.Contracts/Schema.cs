namespace Staticsoft.Interpreter.Contracts;

public class Schema(
	Chat chat,
	Tables tables,
	WebSocket webSocket
)
{
	public Chat chat { get; } = chat;
	public Tables tables { get; } = tables;
	public WebSocket webSocket { get; } = webSocket;
}
