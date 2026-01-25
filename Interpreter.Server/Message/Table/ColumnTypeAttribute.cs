namespace Staticsoft.Interpreter.Server;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnTypeAttribute(
	ColumnType type
) : Attribute
{
	public readonly ColumnType Type = type;
}
