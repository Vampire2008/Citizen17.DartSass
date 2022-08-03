using System.Collections.Generic;
using System.Linq;

namespace Citizen17.DartSass;

public class SassFilesCompilationResult : SassCompilationResult
{
    public IEnumerable<string> Files { get; }

    internal SassFilesCompilationResult(IEnumerable<string> files,
        IEnumerable<SassMessage> warnings,
        IEnumerable<SassDeprecationWarning> deprecationWarnings,
        IEnumerable<SassMessage> debug) 
        : base(warnings,
        deprecationWarnings,
        debug)
    {
        Files = files;
    }

    internal SassFilesCompilationResult()
    {
        Files = Enumerable.Empty<string>();
    }
}
