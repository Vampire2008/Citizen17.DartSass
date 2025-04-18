﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Citizen17.DartSass;

public class DartSassCompiler
{
    private readonly DartSassRuntime _runtime;

    /// <summary>
    /// Get or set options that passed to compiler if method options is null
    /// </summary>
    public SassCompileOptions CompileOptions { get; set; }

    /// <summary>
    /// Creates compiler instance
    /// </summary>
    /// <param name="pathToExecutable">Path to Dart Sass executable. If not passed tries to find in dependencies or in environment variable PATH</param>
    /// <exception cref="ArgumentException">Throws if Dart Sass executable not found.</exception>
    public DartSassCompiler(string pathToExecutable = null)
    {
        if (File.Exists(pathToExecutable))
        {
            _runtime = new(pathToExecutable);
            return;
        }

        var executionLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.WinX64);
            }
            if (RuntimeInformation.OSArchitecture == Architecture.X86 || !File.Exists(Path.Combine(executionLocation, pathToExecutable)))
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.WinX86);
            }
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.WinArm64);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.LinuxX64);
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.X86)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.LinuxX86);
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.LinuxArm64);
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.Arm)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.LinuxArm);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.MacOSX64);
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                pathToExecutable = GetCompilerPathByNativeType(DartSassNativeType.MacOSArm64);
            }
        }

        if (!string.IsNullOrEmpty(pathToExecutable))
            pathToExecutable = Path.Combine(executionLocation, pathToExecutable);

        if (!File.Exists(pathToExecutable))
        {
            var environmentPath = Environment.GetEnvironmentVariable("PATH");

            var paths = environmentPath.Split(';');
            pathToExecutable = paths.SelectMany(x => new[] { "", ".bat", ".sh" }.Select(e => Path.Combine(x, $"sass{e}")))
                .FirstOrDefault(File.Exists);

            if (string.IsNullOrEmpty(pathToExecutable))
            {
                throw new ArgumentException("Dart Sass executable not found.");
            }
        }

        _runtime = new(pathToExecutable);
    }

    /// <summary>
    /// Creates compiler instance
    /// </summary>
    /// <param name="nativeType">Dart Sass native that you want to use (corresponding Nuget must be added)</param>
    public DartSassCompiler(DartSassNativeType nativeType) : this(GetCompilerPathByNativeType(nativeType))
    {
    }

    /// <summary>
    /// Compile file to CSS and returns code
    /// </summary>
    /// <param name="inputFilePath">Path to file for compilation</param>
    /// <param name="options">Compile options</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Compiled CSS code</returns>
    public async Task<SassCodeCompilationResult> CompileAsync(string inputFilePath, SassCompileOptions options = null, CancellationToken cancellationToken = default)
    {
        var args = options?.BuildArgs(true) ?? CompileOptions?.BuildArgs(true);
        var result = await _runtime.ExecuteAsync($"{inputFilePath} {args}", null, cancellationToken).ConfigureAwait(false);

        var parsed = ParseStdErr(result.StdErr);
        EnsureSuccess(result, parsed);

        return new(result.StdOut,
            parsed.Warnings,
            parsed.DeprecationWarnings,
            parsed.Debug);
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
    public Task<SassFilesCompilationResult> CompileToFileAsync(string inputFilePath, string outputFilePath = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
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
    public Task<SassFilesCompilationResult> CompileToFilesAsync(IEnumerable<string> files, string outputDir = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
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
    public Task<SassFilesCompilationResult> CompileToFilesAsync(IDictionary<string, string> files,
        string outputDir = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
    {
        if (files == null)
        {
            throw new ArgumentNullException(nameof(files));
        }
        if (!files.Any())
        {
            return Task.FromResult(new SassFilesCompilationResult());
        }

        return CompileToFilesInternalAsync(files, outputDir, options, cancellationToken);
    }

    private async Task<SassFilesCompilationResult> CompileToFilesInternalAsync(IDictionary<string, string> files, string outputDir = null, SassCompileOptions options = null, CancellationToken cancellationToken = default)
    {
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
        var output = await _runtime.ExecuteAsync($"{filesArgs} {args}", null, cancellationToken).ConfigureAwait(false);

        var parsed = ParseStdErr(output.StdErr);
        EnsureSuccess(output, parsed);

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
        return new(resultFiles,
            parsed.Warnings,
            parsed.DeprecationWarnings,
            parsed.Debug);
    }

    /// <summary>
    /// Compile code
    /// </summary>
    /// <param name="code">Code for compilation</param>
    /// <param name="options">Compile options</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Compiled code</returns>
    public async Task<SassCodeCompilationResult> CompileCodeAsync(string code, SassCompileOptions options = null, CancellationToken cancellationToken = default)
    {
        options ??= CompileOptions;
        var args = options?.BuildArgs(false) ?? string.Empty;
        args += " --stdin";
        var result = await _runtime.ExecuteAsync(args, code, cancellationToken).ConfigureAwait(false);

        var parsed = ParseStdErr(result.StdErr);
        EnsureSuccess(result, parsed);

        return new(result.StdOut,
            parsed.Warnings,
            parsed.DeprecationWarnings,
            parsed.Debug);
    }

    /// <summary>
    /// Get Dart Sass version
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Dart Sass version</returns>
    public async Task<string> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        var result = await _runtime.ExecuteAsync("--version", null, cancellationToken).ConfigureAwait(false);
        return result.StdOut;
    }

    private static StdErrParseResult
        ParseStdErr(string output)
    {
        var warnings = new List<SassMessage>();
        var deprecationWarnings = new List<SassDeprecationWarning>();
        var debug = new List<SassMessage>();
        var errors = new List<SassMessage>();
        var lines = output.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Warning", StringComparison.InvariantCultureIgnoreCase))
            {
                warnings.Add(ParseWarning(lines, i));
                continue;
            }

            if (lines[i].StartsWith("Deprecation Warning:", StringComparison.InvariantCultureIgnoreCase))
            {
                deprecationWarnings.Add(ParseDeprecationWarning(lines, i));
            }

            if (lines[i].Contains("Debug:", StringComparison.InvariantCultureIgnoreCase))
            {
                debug.Add(ParseDebug(lines[i]));
            }

            if (lines[i].StartsWith("Error:", StringComparison.InvariantCultureIgnoreCase))
            {
                errors.Add(ParseError(lines, i));
            }
        }
        return new(errors, warnings, deprecationWarnings, debug);
    }

    private static SassMessage ParseWarning(IReadOnlyList<string> lines, int position)
    {
        var message = lines[position][9..].Trim();
        var rawMessageBuilder = new StringBuilder(lines[position].TrimEnd());
        var j = position + 1;
        var stackTraceBuilder = new StringBuilder();
        while (!string.IsNullOrWhiteSpace(lines[j]) && j < lines.Count)
        {
            stackTraceBuilder.AppendLine(lines[j].Trim());
            rawMessageBuilder.AppendLine(lines[j].TrimEnd());
            j++;
        }
        return new(message, stackTraceBuilder.ToString(), rawMessageBuilder.ToString());
    }

    private static SassDeprecationWarning ParseDeprecationWarning(IReadOnlyList<string> lines, int position)
    {
        var message = lines[position][21..].Trim();
        var rawMessageBuilder = new StringBuilder(lines[position].TrimEnd());
        var recommendationBuilder = new StringBuilder();
        var stackTraceBuilder = new StringBuilder();
        var j = position + 1;
        var stage = MessageStage.Recommendation;
        var dispRegEx = new Regex(@"^\d*\s+[╷│╵]", RegexOptions.CultureInvariant);
        while (stage != MessageStage.End && j < lines.Count)
        {
            switch (stage)
            {
                case MessageStage.Recommendation:
                    if (dispRegEx.IsMatch(lines[j]))
                    {
                        stage = MessageStage.Display;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(lines[j]))
                    {
                        recommendationBuilder.AppendLine(lines[j].Trim());
                    }
                    break;
                case MessageStage.Display:
                    if (!dispRegEx.IsMatch(lines[j]))
                    {
                        stage = MessageStage.StackTrace;
                        continue;
                    }
                    break;
                case MessageStage.StackTrace:
                    if (string.IsNullOrWhiteSpace(lines[j]))
                    {
                        stage = MessageStage.End;
                        continue;
                    }

                    stackTraceBuilder.AppendLine(lines[j].Trim());
                    break;
            }

            rawMessageBuilder.AppendLine(lines[j].TrimEnd());
            j++;
        }
        return new(message, stackTraceBuilder.ToString(), rawMessageBuilder.ToString(), recommendationBuilder.ToString());
    }

    private static SassMessage ParseDebug(string line)
    {
        var message = line[(line.IndexOf("Debug:", StringComparison.InvariantCultureIgnoreCase) + 7)..].Trim();
        var stackTrace = line[..line.IndexOf("Debug", StringComparison.InvariantCultureIgnoreCase)].Trim();
        return new(message, stackTrace, line);
    }

    private static SassMessage ParseError(IReadOnlyList<string> lines, int position)
    {
        var message = lines[position][7..].Trim();
        var rawMessageBuilder = new StringBuilder(lines[position].TrimEnd());
        var j = position + 1;
        var stackTraceBuilder = new StringBuilder();
        var stage = MessageStage.Display;
        var dispRegEx = new Regex(@"^\d?\s+[╷│╵]", RegexOptions.CultureInvariant);
        while (!string.IsNullOrWhiteSpace(lines[j]) && j < lines.Count)
        {
            if (stage == MessageStage.Display)
                if (!dispRegEx.IsMatch(lines[j]))
                    stage = MessageStage.StackTrace;
                else
                {
                    rawMessageBuilder.AppendLine(lines[j].TrimEnd());
                    j++;
                    continue;
                }

            stackTraceBuilder.AppendLine(lines[j].Trim());
            rawMessageBuilder.AppendLine(lines[j].TrimEnd());
            j++;

        }
        return new(message, stackTraceBuilder.ToString(), rawMessageBuilder.ToString());
    }

    private static void EnsureSuccess(RuntimeResult result, StdErrParseResult parsed)
    {
        if (result.ExitCode != 0)
        {
            if (string.IsNullOrWhiteSpace(result.StdErr))
            {
                var lines = result.StdOut.Split('\n');

                var error = lines.Length > 0
                    ? lines[0]
                    : MessageStrings.UnknownError;

                throw new SassCompileException(error, result.StdOut);
            }
            throw new SassCompileException(parsed.Errors,
                result.StdErr,
                parsed.Warnings,
                parsed.DeprecationWarnings,
                parsed.Debug);
        }
    }

    private static string GetCompilerPathByNativeType(DartSassNativeType nativeType)
    {
        switch (nativeType) {
            case DartSassNativeType.WinX64:
                return FormatCompilerPathWithPlatform("win-x64", true);
            case DartSassNativeType.WinX86:
                return FormatCompilerPathWithPlatform("win-x86", true);
            case DartSassNativeType.WinArm64:
                return FormatCompilerPathWithPlatform("win-arm64", true);
            case DartSassNativeType.LinuxX64:
                return FormatCompilerPathWithPlatform("linux-x64");
            case DartSassNativeType.LinuxX86:
                return FormatCompilerPathWithPlatform("linux-x86");
            case DartSassNativeType.LinuxArm:
                return FormatCompilerPathWithPlatform("linux-arm");
            case DartSassNativeType.LinuxArm64:
                return FormatCompilerPathWithPlatform("linux-arm64");
            case DartSassNativeType.LinuxRiscv64:
                return FormatCompilerPathWithPlatform("linux-riscv64");
            case DartSassNativeType.LinuxMuslX64:
                return FormatCompilerPathWithPlatform("linux-musl-x64");
            case DartSassNativeType.LinuxMuslX86:
                return FormatCompilerPathWithPlatform("linux-musl-x86");
            case DartSassNativeType.LinuxMuslArm:
                return FormatCompilerPathWithPlatform("linux-musl-arm");
            case DartSassNativeType.LinuxMuslArm64:
                return FormatCompilerPathWithPlatform("linux-musl-arm64");
            case DartSassNativeType.LinuxMuslRiscv64:
                return FormatCompilerPathWithPlatform("linux-musl-riscv64");
            case DartSassNativeType.MacOSX64:
                return FormatCompilerPathWithPlatform("macos-x64");
            case DartSassNativeType.MacOSArm64:
                return FormatCompilerPathWithPlatform("macos-arm64");
            case DartSassNativeType.AndroidX64:
                return FormatCompilerPathWithPlatform("android-x64");
            case DartSassNativeType.AndroidX86:
                return FormatCompilerPathWithPlatform("android-x86");
            case DartSassNativeType.AndroidArm:
                return FormatCompilerPathWithPlatform("android-arm");
            case DartSassNativeType.AndroidArm64:
                return FormatCompilerPathWithPlatform("android-arm64");
            case DartSassNativeType.AndroidRiscv64:
                return FormatCompilerPathWithPlatform("android-riscv64");
            default:
                throw new ArgumentOutOfRangeException(nameof(nativeType), nativeType, "Undefined Dart Sass native type");
        }
    }

    private static string FormatCompilerPathWithPlatform(string nativePlatform, bool isBat = false)
    {
        return $"./dart-sass.{nativePlatform}/sass{(isBat ? ".bat" : "")}";
    }
}
