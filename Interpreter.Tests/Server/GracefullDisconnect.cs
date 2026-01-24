namespace Staticsoft.Interpreter.Server.Tests;

public class GracefullDisconnect
{
    readonly SemaphoreSlim Lock = new(initialCount: 0, maxCount: 1);

    public void NotifyDisconnected()
        => Lock.Release();

    public Task WaitDisconnected()
        => Lock.WaitAsync();
}
