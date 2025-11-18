namespace Citizen17.DartSass;

public class SassCompileException : Exception
{
    public string RawOutput { get; }
    public IEnumerable<SassMessage> Errors { get; }
    public IEnumerable<SassMessage> Warnings { get; }
    public IEnumerable<SassDeprecationWarning> DeprecationWarnings { get; }
    public IEnumerable<SassMessage> Debug { get; }

    internal SassCompileException(IEnumerable<SassMessage> errors, string rawOutput, IEnumerable<SassMessage> warnings, IEnumerable<SassDeprecationWarning> deprecationWarnings, IEnumerable<SassMessage> debug)
        : base(GetErrorMessage(errors))
    {
        Errors = errors;
        RawOutput = rawOutput;
        Warnings = warnings;
        DeprecationWarnings = deprecationWarnings;
        Debug = debug;
    }

    internal SassCompileException(string error, string rawOutput) : base(error)
    {
        Errors = [new(error, string.Empty, error)];
        Warnings = [];
        DeprecationWarnings = [];
        Debug = [];
        RawOutput = rawOutput;
    }
    
    private static string GetErrorMessage(IEnumerable<SassMessage> errors)
    {
        if (errors.Count() > 1)
        {
            return $"{MessageStrings.MultipleErrors}:\n{string.Join("\n", errors.Select(e => e.Message))}";
        }

        var message = errors.SingleOrDefault();

        return message == null
            ? MessageStrings.UnknownError
            : message.Message;
    }
}
