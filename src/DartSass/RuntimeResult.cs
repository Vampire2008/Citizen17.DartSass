namespace Citizen17.DartSass;

internal sealed class RuntimeResult(int exitCode, string stdOut, string stdErr)
{
    public int ExitCode { get; } = exitCode;
    public string StdOut { get; } = stdOut;
    public string StdErr { get; } = stdErr;
}
