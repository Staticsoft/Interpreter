using Staticsoft.PartitionedStorage.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class ConversationHistory(
	PartitionConversations conversations
)
{
	readonly PartitionConversations Conversations = conversations;

	public async Task<string> Create(string userId)
	{
		var conversationId = NewId.FromTimestamp();

		await Conversations
			.Get(userId)
			.Save(conversationId, new() { MessageIds = [] });

		return conversationId;
	}

	public async Task AddMessage(string userId, string conversationId, string messageId)
	{
		var conversation = await Conversations
			.Get(userId)
			.Get(conversationId);


		var updatedConversation = conversation.Data with
		{
			MessageIds = [.. conversation.Data.MessageIds, messageId]
		};

		await Conversations
			.Get(userId)
			.Save(conversationId, updatedConversation, conversation.Version);
	}
}
