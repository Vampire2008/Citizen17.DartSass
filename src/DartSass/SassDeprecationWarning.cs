namespace Citizen17.DartSass;

public class SassDeprecationWarning : SassMessage
{
    public string Recommendation { get; }

    internal SassDeprecationWarning(string message, string stackTrace, string rawMessage, string recommendation)
        : base(message, stackTrace, rawMessage)
    {
        Recommendation = recommendation;
    }
}
