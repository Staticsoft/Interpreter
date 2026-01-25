using Staticsoft.Flow;

namespace Staticsoft.Interpreter.Server;

public class CurrentDateTimeOperation(

) : Operation<CurrentDateTimeOperation.Input, CurrentDateTimeOperation.Output>
{
    public class Input { }
    public class Output
    {
        public required DateTime Now { get; init; }
    }

    public Task<Output> Execute(Input input)
        => Task.FromResult(new Output() { Now = DateTime.UtcNow });
}
