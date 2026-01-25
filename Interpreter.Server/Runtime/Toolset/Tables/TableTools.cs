using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class TableTools(
	TableFactory factory,
	UserContext context,
	Operation<ShowTableOperation.Input, ShowTableOperation.Output> showTable
)
{
	readonly TableFactory Factory = factory;
	readonly UserContext Context = context;
	readonly Operation<ShowTableOperation.Input, ShowTableOperation.Output> ShowTable = showTable;

	public async Task Show<T>(IEnumerable<T> dataset)
		where T : notnull
	{
		var table = Factory.Resolve<T>().Create(dataset);
		await ShowTable.Execute(new()
		{
			Table = table,
			UserId = Context.UserId,
			ConversationId = Context.ConversationId
		});
	}
}
