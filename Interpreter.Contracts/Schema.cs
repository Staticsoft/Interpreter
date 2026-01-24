namespace Staticsoft.Interpreter.Contracts;

public class Schema(
	Chat chat,
	WebSocket webSocket
)
{
	public Chat Chat { get; } = chat;
	public WebSocket WebSocket { get; } = webSocket;
}