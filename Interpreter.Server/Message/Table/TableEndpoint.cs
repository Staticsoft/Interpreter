using Staticsoft.Contracts.Abstractions;
using Staticsoft.Interpreter.Contracts;
using System.Text.Json;

namespace Staticsoft.Interpreter.Server;

public class TableEndpoint(
	PartitionTables tables,
	Identity identity
) : ParametrizedHttpEndpoint<EmptyRequest, Tables.TableResponse>
{
	readonly PartitionTables Tables = tables;
	readonly Identity Identity = identity;

	public async Task<Tables.TableResponse> Execute(string tableId, EmptyRequest request)
	{
		var table = await Tables.Get(Identity.UserId).Get(tableId);
		return new()
		{
			Columns = JsonSerializer.Deserialize<object[]>(table.Data.Columns)!,
			Rows = JsonSerializer.Deserialize<object[]>(table.Data.Rows)!
		};
	}
}
