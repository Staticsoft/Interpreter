namespace Staticsoft.Interpreter.Server;

public class Message
{
    public string Text { get; set; } = string.Empty;
    public Type Origin { get; set; } = Type.Unknown;

    public enum Type
    {
        Unknown,
        User,
        System
    }
}