using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class FindConversationOperation(
    ConversationHistory conversations
) : Operation<FindConversationOperation.Input, FindConversationOperation.Output>
{
    public class Input
    {
        public required string MessageId { get; init; }
        public required string UserId { get; init; }
    }
    public class Output
    {
        public required string Id { get; init; }
    }

    readonly ConversationHistory Conversations = conversations;

    public async Task<Output> Execute(Input input)
    {
        var isNewConversation = await IsNewConversation(input.MessageId, input.UserId);

        var conversationId = isNewConversation
            ? await CreateConversation(input.UserId)
            : await FindLastConversation(input.UserId);

        return new() { Id = conversationId };
    }

    static Task<bool> IsNewConversation(string text, string userId)
        => Task.FromResult(!string.IsNullOrEmpty(text));

    Task<string> CreateConversation(string userId)
        => Conversations.Create(userId);

    Task<string> FindLastConversation(string userId)
        => throw new NotImplementedException();
}
