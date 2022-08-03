using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Citizen17.DartSass;

[Serializable]
public class SassCompileException : Exception
{
    public string RawOutput { get; }
    public IEnumerable<SassMessage> Errors { get; }
    public IEnumerable<SassMessage> Warnings { get; }
    public IEnumerable<SassDeprecationWarning> DeprecationWarnings { get; }
    public IEnumerable<SassMessage> Debug { get; }

    public SassCompileException(IEnumerable<SassMessage> errors, string rawOutput, IEnumerable<SassMessage> warnings, IEnumerable<SassDeprecationWarning> deprecationWarnings, IEnumerable<SassMessage> debug)
    {
        Errors = errors;
        RawOutput = rawOutput;
        Warnings = warnings;
        DeprecationWarnings = deprecationWarnings;
        Debug = debug;
    }

    protected SassCompileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
