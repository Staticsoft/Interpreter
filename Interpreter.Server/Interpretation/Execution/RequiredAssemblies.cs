using Microsoft.CodeAnalysis;
using Staticsoft.Flow;
using System.Reflection;
using System.Text.Json;

namespace Staticsoft.Interpreter.Server;

public class RequiredAssemblies(

) : ProgramAssemblies
{
	public PortableExecutableReference[] References { get; } =
	[
		MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
		MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
		MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
		MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
		MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location),
		MetadataReference.CreateFromFile(typeof(Job<,>).Assembly.Location)
	];
}
