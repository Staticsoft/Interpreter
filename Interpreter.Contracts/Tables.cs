using Staticsoft.Contracts.Abstractions;

namespace Staticsoft.Interpreter.Contracts;

public class Tables(
	ParametrizedHttpEndpoint<EmptyRequest, Tables.TableResponse> get
)
{
	[Endpoint(HttpMethod.Get)]
	public ParametrizedHttpEndpoint<EmptyRequest, TableResponse> Get { get; init; } = get;

	public class TableResponse
	{
		public required object[] Columns { get; init; }
		public required object[] Rows { get; init; }
	}
}