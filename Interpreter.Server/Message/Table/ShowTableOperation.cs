using Staticsoft.Flow;
using Staticsoft.PartitionedStorage.Abstractions;

namespace Staticsoft.Interpreter.Server;

public class ShowTableOperation(
	Print print,
	ConversationHistory conversations,
	PartitionTables tables
) : Operation<ShowTableOperation.Input, ShowTableOperation.Output>
{
	readonly Print Print = print;
	readonly ConversationHistory Conversations = conversations;
	readonly PartitionTables Tables = tables;

	public class Input
	{
		public required TableView Table { get; init; }
		public required string UserId { get; init; }
		public required string ConversationId { get; init; }
	}

	public class Output { }

	public async Task<Output> Execute(Input input)
	{
		var tableId = NewId.FromTimestamp();

		await Tables
			.Get(input.UserId)
			.Save(tableId, new()
			{
				Rows = input.Table.Rows,
				Columns = input.Table.Columns
			});

		var messageId = await Print.SystemTableMessage(input.UserId, tableId);
		await Conversations.AddMessage(input.UserId, input.ConversationId, messageId);

		return new();
	}
}
