using System.Collections.Generic;
using System.Linq;

namespace Citizen17.DartSass;

public class SassCompilationResult
{
    public IEnumerable<SassMessage> Warnings { get; }
    public IEnumerable<SassDeprecationWarning> DeprecationWarnings { get; }
    public IEnumerable<SassMessage> Debug { get; }

    internal SassCompilationResult(IEnumerable<SassMessage> warnings, IEnumerable<SassDeprecationWarning> deprecationWarnings, IEnumerable<SassMessage> debug)
    {
        Warnings = warnings;
        DeprecationWarnings = deprecationWarnings;
        Debug = debug;
    }

    internal SassCompilationResult()
    {
        Warnings = Enumerable.Empty<SassMessage>();
        DeprecationWarnings = Enumerable.Empty<SassDeprecationWarning>();
        Debug = Enumerable.Empty<SassMessage>();
    }
}
