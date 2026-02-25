using System.Text.Json;

namespace Staticsoft.Interpreter.Server;

public class FallbackTableFactory<Data> : TableFactory<Data>
{
	static readonly Column[] Columns = typeof(Data)
		.GetProperties()
		.OrderBy(p => p.Name)
		.Select(p => new Column
		{
			title = p.Name,
			key = p.Name,
			dataType = GetColumnType(p.PropertyType)
		})
		.ToArray();

	public TableView Create(IEnumerable<Data> data)
		=> new()
		{
			Rows = JsonSerializer.Serialize(data),
			Columns = JsonSerializer.Serialize(Columns)
		};

	static string GetColumnType(Type type)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;

		if (type == typeof(string)) return nameof(ColumnType.text);
		if (type == typeof(DateTime)) return nameof(ColumnType.date);

		if (IsInteger(type)) return nameof(ColumnType.number);
		if (IsFloatingPoint(type)) return nameof(ColumnType.money);

		return nameof(ColumnType.text);
	}

	static bool IsInteger(Type type)
		=> type == typeof(byte) || type == typeof(sbyte)
		|| type == typeof(short) || type == typeof(ushort)
		|| type == typeof(int) || type == typeof(uint)
		|| type == typeof(long) || type == typeof(ulong);

	static bool IsFloatingPoint(Type type)
		=> type == typeof(float)
		|| type == typeof(double)
		|| type == typeof(decimal);
}
