using Staticsoft.PartitionedStorage.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class MessageHistory(
    PartitionMessages messages
)
{
    const int MaxReturnedMessages = 50;

    readonly PartitionMessages Messages = messages;

    public Task SaveMessage(string messageId, string text, string userId, Message.Type type)
        => Messages
            .Get(userId)
            .Save(messageId, new Message()
            {
                Text = text,
                Origin = type
            });

    public async Task<Item<Message>[]> GetMessages(string userId)
    {
        var messages = await Messages
            .Get(userId)
            .Scan(new()
            {
                MaxItems = MaxReturnedMessages,
                Order = ScanOrder.Descending
            });
        return messages.Reverse().ToArray();
    }
}