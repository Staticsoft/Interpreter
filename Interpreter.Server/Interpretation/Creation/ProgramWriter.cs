namespace Staticsoft.Interpreter.Server;

public interface ProgramWriter
{
	Task<string> Write(string requirements);
}
