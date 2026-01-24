namespace Staticsoft.Interpreter.Server;

public interface Converter<Data, View>
{
	View Convert(Data data);
}
