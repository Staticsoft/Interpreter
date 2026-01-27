using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Staticsoft.Interpreter.Server;

public interface TableFactory<T>
{
	TableView Create(IEnumerable<T> data);
}

public class TableFactory(
	IServiceProvider provider
)
{
	readonly IServiceProvider Provider = provider;

	public TableFactory<T> Resolve<T>()
		=> Provider.GetService<TableFactory<T>>() ?? new FallbackTableFactory<T>();
}

public class TableFactory<Data, View>(
	TableConverter<Data, View> converter
) : TableFactory<Data>
{
	readonly TableConverter<Data, View> Converter = converter;

	public TableView Create(IEnumerable<Data> data)
	{
		var rows = data.Select(Converter.Convert);
		var columns = typeof(View).GetProperties()
			.OrderBy(property => property.GetCustomAttribute<JsonPropertyOrderAttribute>()!.Order)
			.Select(property => new Column()
			{
				Title = property.Name,
				DataType = $"{property.GetCustomAttribute<ColumnTypeAttribute>()!.Type}"
			});
		return new()
		{
			Rows = JsonSerializer.Serialize(rows),
			Columns = JsonSerializer.Serialize(columns)
		};
	}
}