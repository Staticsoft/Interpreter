namespace Staticsoft.Interpreter.Contracts;

public class Schema(
	Chat chat,
	Tables tables,
	WebSocket webSocket
)
{
	public Chat Chat { get; } = chat;
	public Tables Tables { get; } = tables;
	public WebSocket WebSocket { get; } = webSocket;
}
