using Staticsoft.Interpreter.Contracts;
using System.Text.Json;

namespace Staticsoft.Interpreter.Server;

public class MessageSerializer
{
	readonly static Dictionary<string, Type> Types = new Type[]
	{
		typeof(Chat.TextMessage),
		typeof(Chat.TableMessage)
	}
	.ToDictionary(type => type.Name);

	public Message Serialize<T>(T message)
		where T : Chat.Message
		=> new()
		{
			Data = JsonSerializer.Serialize(message),
			Type = typeof(T).Name
		};

	public Chat.Message Deserialize(Message message)
	{
		if (!Types.TryGetValue(message.Type, out var type)) throw NotSupported(message.Type);

		return (Chat.Message)JsonSerializer.Deserialize(message.Data, type)!;
	}

	static NotSupportedException NotSupported(string type)
		=> new($"{nameof(Message)} type '{type}' is not supported");
}
