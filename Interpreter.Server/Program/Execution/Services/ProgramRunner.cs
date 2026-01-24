using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Staticsoft.Interpreter.Server;

public interface ProgramRunner
{
	Task Run(string code, string userId, string conversationId);
}

public class ProgramRunner<ProgramToolset>(
	ProgramAssemblies assemblies,
	ProgramToolset toolset
) : ProgramRunner
{
	readonly ProgramToolset Toolset = toolset;
	readonly ProgramAssemblies Assemblies = assemblies;

	public async Task Run(string code, string userId, string conversationId)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);

		var compilation = CSharpCompilation.Create("RuntimeAssembly")
			.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
			.AddReferences(Assemblies.References)
			.AddSyntaxTrees(syntaxTree);

		using var ms = new MemoryStream();
		var result = compilation.Emit(ms);

		if (result.Success)
		{
			ms.Seek(0, SeekOrigin.Begin);
			var assembly = Assembly.Load(ms.ToArray());

			var type = assembly.GetType("RuntimeCompilation.Program")!;
			var instance = Activator.CreateInstance(type);
			var method = type.GetMethod("Run")!;

			var task = (Task)method.Invoke(instance, [Toolset])!;
			await task;
		}
		else
		{
			var errors = result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error);
			foreach (var error in errors)
			{
				Console.Error.WriteLine(error);
			}
		}
	}
}