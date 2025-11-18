using System.Diagnostics;
using System.Text;

namespace Citizen17.DartSass;

internal sealed class DartSassRuntime
{
    private readonly string _executable;

    internal DartSassRuntime(string executable)
    {
        _executable = executable;
    }

    internal async Task<RuntimeResult> ExecuteAsync(string args, string? input, CancellationToken cancellationToken)
    {
        using var process = new Process();
        process.StartInfo.FileName = _executable;
        process.StartInfo.Arguments = args;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
        process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

        var outputBuilder = new StringBuilder();
        process.OutputDataReceived += (_, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);

        var errorBuilder = new StringBuilder();
        process.ErrorDataReceived += (_, eventArgs) => errorBuilder.AppendLine(eventArgs.Data);

        process.Start();
        if (!string.IsNullOrEmpty(input))
        {
            await process.StandardInput.WriteAsync(input);
            process.StandardInput.Close();
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        process.CancelOutputRead();
        process.CancelErrorRead();

        return new(process.ExitCode, outputBuilder.ToString(), errorBuilder.ToString());
    }
}
