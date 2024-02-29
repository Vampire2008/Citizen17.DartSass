using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Citizen17.DartSass;

#if !NET8_0_OR_GREATER
[Serializable]
#endif
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
        Warnings = Enumerable.Empty<SassMessage>();
        DeprecationWarnings = Enumerable.Empty<SassDeprecationWarning>();
        Debug = Enumerable.Empty<SassMessage>();
        RawOutput = rawOutput;
    }

#if !NET8_0_OR_GREATER
    protected SassCompileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
#endif

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
