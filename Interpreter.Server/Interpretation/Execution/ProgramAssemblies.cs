using Microsoft.CodeAnalysis;

namespace Staticsoft.Interpreter.Server;

public interface ProgramAssemblies
{
	PortableExecutableReference[] References { get; }
}
