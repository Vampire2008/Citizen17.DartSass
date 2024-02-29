using System.Collections.Generic;

namespace Citizen17.DartSass;

internal class StdErrParseResult(IEnumerable<SassMessage> errors, IEnumerable<SassMessage> warnings, IEnumerable<SassDeprecationWarning> deprecationWarnings, IEnumerable<SassMessage> debug)
{
    internal IEnumerable<SassMessage> Errors { get; } = errors;
    internal IEnumerable<SassMessage> Warnings { get; } = warnings;
    internal IEnumerable<SassDeprecationWarning> DeprecationWarnings { get; } = deprecationWarnings;
    internal IEnumerable<SassMessage> Debug { get; } = debug;
}
