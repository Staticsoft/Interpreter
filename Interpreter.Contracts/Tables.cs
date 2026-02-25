using Staticsoft.Contracts.Abstractions;

namespace Staticsoft.Interpreter.Contracts;

public class Tables(
	ParametrizedHttpEndpoint<EmptyRequest, Tables.TableResponse> get
)
{
	[Endpoint(HttpMethod.Get)]
	public ParametrizedHttpEndpoint<EmptyRequest, TableResponse> get { get; init; } = get;

	public class TableResponse
	{
		public required object[] columns { get; init; }
		public required object[] rows { get; init; }
	}
}
