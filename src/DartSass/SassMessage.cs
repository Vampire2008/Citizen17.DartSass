namespace Citizen17.DartSass;

public class SassMessage
{
    public string Message { get; }
    public string StackTrace { get; }

    /// <summary>
    /// Original message output
    /// </summary>
    public string RawMessage { get; }

    internal SassMessage(string message, string stackTrace, string rawMessage)
    {
        Message = message;
        StackTrace = stackTrace;
        RawMessage = rawMessage;
    }
}
