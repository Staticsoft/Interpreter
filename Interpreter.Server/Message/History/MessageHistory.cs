using Staticsoft.Interpreter.Contracts;
using Staticsoft.PartitionedStorage.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class MessageHistory(
	PartitionMessages messages,
	MessageSerializer serializer
)
{
	const int MaxReturnedMessages = 50;

	readonly PartitionMessages Messages = messages;
	readonly MessageSerializer Serializer = serializer;

	public Task SaveMessage<T>(string userId, string messageId, T message)
		where T : Chat.Message
		=> Messages
			.Get(userId)
			.Save(messageId, Serializer.Serialize(message));

	public async Task<Chat.Message[]> GetMessages(string userId)
	{
		var messages = await Messages
			.Get(userId)
			.Scan(new()
			{
				MaxItems = MaxReturnedMessages,
				Order = ScanOrder.Descending
			});
		return messages
			.Reverse()
			.Select(item => Serializer.Deserialize(item.Data))
			.ToArray();
	}
}