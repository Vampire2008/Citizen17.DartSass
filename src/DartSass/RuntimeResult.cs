namespace Citizen17.DartSass;

internal class RuntimeResult
{
    public int ExitCode { get; }
    public string StdOut { get; }
    public string StdErr { get; }

    public RuntimeResult(int exitCode, string stdOut, string stdErr)
    {
        ExitCode = exitCode;
        StdOut = stdOut;
        StdErr = stdErr;
    }
}
