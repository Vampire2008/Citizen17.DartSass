using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Citizen17.DartSass
{
    public class DartSassCompiler
    {
        private readonly DartSassRuntime _runtime;

        /// <summary>
        /// Get or set options that passed to compiller if method options is null
        /// </summary>
        public SassCompileOptions CompileOptions { get; set; }

        /// <summary>
        /// Creates compiler instance
        /// </summary>
        /// <param name="pathToExecutable">Path to Dart Sass executable. If not passed tries to find in dependences or in evironment variable PATH</param>
        /// <exception cref="ArgumentException">Throws if Dart Sass executable not found.</exception>
        public DartSassCompiler(string pathToExecutable = null)
        {
            if (File.Exists(pathToExecutable))
            {
                _runtime = new(pathToExecutable);
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (RuntimeInformation.OSArchitecture == Architecture.X64)
                {
                    pathToExecutable = "./dart-sass.win-x64/sass.bat";
                }
                if (RuntimeInformation.OSArchitecture == Architecture.X86 || !File.Exists(pathToExecutable))
                {
                    pathToExecutable = "./dart-sass.win-x86/sass.bat";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.OSArchitecture == Architecture.X64)
                {
                    pathToExecutable = "./dart-sass.linux-x64/sass";
                }
                else if (RuntimeInformation.OSArchitecture == Architecture.X86)
                {
                    pathToExecutable = "./dart-sass.linux-x86/sass";
                }
                else if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
                {
                    pathToExecutable = "./dart-sass.linux-arm64/sass";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (RuntimeInformation.OSArchitecture == Architecture.X64)
                {
                    pathToExecutable = "./dart-sass.macos-x64/sass";
                }
                else if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
                {
                    pathToExecutable = "./dart-sass.macos-arm64/sass";
                }
            }

            if (!File.Exists(pathToExecutable))
            {
                var environmentPath = Environment.GetEnvironmentVariable("PATH");

                var paths = environmentPath.Split(';');
                pathToExecutable = paths.SelectMany(x => new[] { "", ".bat", ".sh" }.Select(e => Path.Combine(x, $"sass{e}")))
                    .FirstOrDefault(x => File.Exists(x));

                if (string.IsNullOrEmpty(pathToExecutable))
                {
                    throw new ArgumentException("Sass not found");
                }
            }

            _runtime = new(pathToExecutable);
        }

        /// <summary>
        /// Compile file to CSS and returns code
        /// </summary>
        /// <param name="inputFilePath">Path to file for compilation</param>
        /// <param name="options">Compile options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Compiled CSS code</returns>
        public async Task<string> CompileAsync(string inputFilePath, SassCompileOptions options = null, CancellationToken cancellationToken = default)
        {
            var args = options?.BuildArgs(true) ?? CompileOptions?.BuildArgs(true);
            try
            {
                return await _runtime.ExecuteAsync($"{inputFilePath} {args}", null, cancellationToken).ConfigureAwait(false);
            }
            catch (SassException e)
            {
                throw GetErrors(e.RawError);
            }
        }

        /// <summary>
        /// Compile file and outputs result to file 
        /// </summary>
        /// <param name="inputFilePath">File to compile</param>
        /// <param name="outputFilePath">Result file. If null result files placed near source file with same name but css (and map) extension</param>
        /// <param name="options">Compile options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of result files</returns>
        /// <exception cref="ArgumentNullException">Throws if inputFilePath is empty</exception>
        public Task<IEnumerable<string>> CompileToFileAsync(string inputFilePath, string outputFilePath = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }

            return CompileToFilesAsync(new Dictionary<string, string> { { inputFilePath, outputFilePath } }, null, options, cancellationToken);
        }

        /// <summary>
        /// Compile files and outputs result to files 
        /// </summary>
        /// <param name="files">List of files to compile</param>
        /// <param name="outputDir">Directory where output files will be placed. If null result files placed near source files.</param>
        /// <param name="options">Compile options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of result files</returns>
        /// <exception cref="ArgumentNullException">Throws if files is null</exception>
        public Task<IEnumerable<string>> CompileToFilesAsync(IEnumerable<string> files, string outputDir = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }
            return CompileToFilesAsync(files.ToDictionary(f => f, _ => (string)null), outputDir, options, cancellationToken);
        }

        /// <summary>
        /// Compile files and outputs result to files 
        /// </summary>
        /// <param name="files">Dictionary of files to compile where Key is file path and Value is output path</param>
        /// <param name="outputDir">Directory where output files will be placed if output path for file doesn't contain directory. If null result files placed near source files.</param>
        /// <param name="options">Compile options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of result files</returns>
        /// <exception cref="ArgumentNullException">Throws if dictionary is null</exception>
        /// <exception cref="ArgumentException">Throws if anyone file path is empty</exception>
        public async Task<IEnumerable<string>> CompileToFilesAsync(IDictionary<string, string> files, string outputDir = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }
            if (!files.Any())
            {
                return Enumerable.Empty<string>();
            }
            options ??= CompileOptions;
            var args = options?.BuildArgs(false);
            var filesArgs = new StringBuilder();
            foreach (var pair in files)
            {
                if (string.IsNullOrWhiteSpace(pair.Key))
                {
                    throw new ArgumentException("Input file cannot be empty", nameof(files));
                }
                var outputFile = pair.Value;
                if (string.IsNullOrWhiteSpace(outputFile))
                {
                    var outputFileDir = string.IsNullOrWhiteSpace(outputDir)
                        ? Path.GetDirectoryName(pair.Key)
                        : outputDir;
                    outputFile = Path.Combine(outputFileDir, $"{Path.GetFileNameWithoutExtension(pair.Key)}.css");
                }
                else if (string.IsNullOrEmpty(Path.GetDirectoryName(outputFile)))
                {
                    if (!string.IsNullOrWhiteSpace(outputDir))
                    {
                        outputFile = Path.Combine(outputDir, outputFile);
                    }
                }
                else if (string.IsNullOrEmpty(Path.GetFileName(outputFile)))
                {
                    outputFile = Path.Combine(outputFile, $"{Path.GetFileNameWithoutExtension(pair.Key)}.css");
                }
                files[pair.Key] = outputFile;
                filesArgs.Append(pair.Key)
                    .Append(':')
                    .Append(outputFile)
                    .Append(' ');
            }
            string output;
            try
            {
                output = await _runtime.ExecuteAsync($"{filesArgs} {args}", null, cancellationToken).ConfigureAwait(false);
            }
            catch (SassException ex)
            {
                throw GetErrors(ex.RawError);
            }
            var resultFiles = new List<string>();
            var sourceMapEnabled = options == null || (!options.GenerateSourceMap.HasValue || options.GenerateSourceMap.Value)
                && (!options.EmbedSourceMap.HasValue || !options.EmbedSourceMap.Value);
            foreach (var input in files)
            {
                if (File.Exists(input.Value))
                {
                    resultFiles.Add(input.Value);
                }
                if (sourceMapEnabled)
                {
                    var sourceMapFile = Path.Combine(Path.GetDirectoryName(input.Value), $"{Path.GetFileName(input.Value)}.map");
                    if (File.Exists(sourceMapFile))
                    {
                        resultFiles.Add(sourceMapFile);
                    }
                }
            }
            return resultFiles;
        }

        /// <summary>
        /// Compile code
        /// </summary>
        /// <param name="code">Code for compilation</param>
        /// <param name="options">Compile options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Compiled code</returns>
        public async Task<string> CompileCodeAsync(string code, SassCompileOptions options = null, CancellationToken cancellationToken = default)
        {
            options ??= CompileOptions;
            var args = options?.BuildArgs(false) ?? string.Empty;
            args += " --stdin";
            try
            {
                return await _runtime.ExecuteAsync(args, code, cancellationToken);
            }
            catch (SassException ex)
            {
                throw GetErrors(ex.RawError);
            }
        }

        /// <summary>
        /// Get Dart Sass version
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Dart Sass version</returns>
        public async Task<string> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            return await _runtime.ExecuteAsync("--version", null, cancellationToken).ConfigureAwait(false);
        }


        private SassException GetErrors(string output)
        {
            if (output.StartsWith("Error:", StringComparison.InvariantCulture))
            {
                try
                {
                    var lines = output.Split('\n');
                    var errorMessage = lines[0][7..(lines[0].Length - 1)];
                    var position = lines[5].Trim();
                    return new SassCompileException(errorMessage, position, output);
                }
                catch
                {
                    return new(output);
                }
            }
            else
            {
                return new(output);
            }
        }
    }
}