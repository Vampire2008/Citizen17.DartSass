namespace Citizen17.DartSass;

public class SassCodeCompilationResult : SassCompilationResult
{
    public string Code { get; }

    internal SassCodeCompilationResult(string code,
        IEnumerable<SassMessage> warnings,
        IEnumerable<SassDeprecationWarning> deprecationWarnings,
        IEnumerable<SassMessage> debug) 
        : base(warnings,
        deprecationWarnings,
        debug)
    {
        Code = code;
    }
}
