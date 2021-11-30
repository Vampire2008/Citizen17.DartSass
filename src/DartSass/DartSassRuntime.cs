using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Citizen17.DartSass
{
    internal class DartSassRuntime
    {
        private readonly string _executable;

        internal DartSassRuntime(string executable)
        {
            _executable = executable;
        }

        internal async Task<string> ExecuteAsync(string args, string input, CancellationToken cancellationToken)
        {
            using var process = new Process();
            process.StartInfo.FileName = _executable;
            process.StartInfo.Arguments = args;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            if (!string.IsNullOrEmpty(input))
            {
                await process.StandardInput.WriteAsync(input);
                process.StandardInput.Close();
            }
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            if (process.ExitCode != 0)
            {
                throw new SassException(process.StandardError.ReadToEnd());
            }
            return process.StandardOutput.ReadToEnd();
        }
    }
}
