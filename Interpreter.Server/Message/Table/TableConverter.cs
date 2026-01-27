namespace Staticsoft.Interpreter.Server;

public interface TableConverter<Data, View>
{
	View Convert(Data data);
}
