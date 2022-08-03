# Citizen17.DartSass
Library for compiling SASS using Dart Sass runtime.

## Install

Install package using NuGet.

## Runtime

By default package doesn't contain Dart Sass runtime. It can use installed in system Dart Sass or you can one of packages with runtime:
 * DartSass.Native.win-x64
 * DartSass.Native.win-x86
 * DartSass.Native.linux-x64
 * DartSass.Native.linux-arm64
 * DartSass.Native.linux-x86
 * DartSass.Native.macos-x64
 * DartSass.Native.macos-arm64

## Usage

Create `DartSassCompiller` instance.

```csharp
using Citizen17.DartSass;

var compiler = new DartSassCompiller();
```

When instance creates it try find Dart Sass runtime. First it search in project. If not found it try search in system using environmen variable `PATH`.

If you have dedicated Dart Sass runtime you can pass it as parameter to constructor.

```csharp
var compiler = new DartSassCompiller("/path/to/sass/executable");
``` 

### Options
You can set default options that will be used on evert compile.
```csharp
compiler.CompileOptions = new SassCompileOptions
{
    StyleType = StyleType.Expanded,
    EmitCharset = true,
    Update = false,
    ImportPaths = new List<string>
    {
        "/path/to/imports1",
        "/path/to/imports2"
    },

    GenerateSourceMap = true,
    SourceMapUrlType = SourceMapUrlType.Relative,
    EmbedSources = false,
    EmbedSourceMap = false,
    Quiet = false,
    QuietDeps = false

};
```

Also every compile method can accept options. If options passed they override defaults.

### Compile from file

Use `CompileAsync` method to get compiled code from file source.
```csharp
SassCodeCompilationResult result = await compiler.CompileAsync("/path/to/source.scss");
string code = result.Code;
```

### Compile from code

Use `CompileCodeAsync` method to get compiled code from string source.

```csharp
var sourceSassCode = ".some-class { color: red; }";
SassCodeCompilationResult result = await compiler.CompileCodeAsync(sourceSassCode);
string code = result.Code;
```

### Compile from file to file

Use `CompileToFileAsync` method to compile source SASS file to CSS.

```csharp
SassFilesCompilationResult result = await compiler.CompileToFileAsync("/path/to/source/source.scss");
IEnumerable<string> files = result.Files;

```
Result file: `/path/to/source/source.css` and `/path/to/source/source.css.map` if Source maps enabled.

Also you can pass custom name for output file.

```csharp
SassFilesCompilationResult result = await compiler.CompileToFileAsync("/path/to/source/source.scss", "dest.css");
IEnumerable<string> files = result.Files;
```

Result file: `/path/to/source/dest.css` and `/path/to/source/dest.css.map` if Source maps enabled.


```csharp
SassFilesCompilationResult result = await compiler.CompileToFileAsync("/path/to/source/source.scss", "/path/to/dest/dest.css");
IEnumerable<string> files = result.Files;
```

Result file: `/path/to/dest/dest.css` and `/path/to/dest/dest.css.map` if Source maps enabled.

### Compile multiple files

Use `CompileToFilesAsync` method to compile multiple files.
It has 2 overload.

First accept list of strings with source files.

```csharp
var sourceFiles = new string[]
{
    "path/to/source1.sass",
    "path/to/source2.sass"
};

SassFilesCompilationResult result = await compiler.CompileToFilesAsync(sourceFiles);
IEnumerable<string> files = result.Files;
```

All output files will be placed near it source file. Or you can pass additional parameter to specify output directory.

```csharp
SassFilesCompilationResult result = await compiler.CompileToFilesAsync(sourceFiles, "/path/to/dest");
IEnumerable<string> files = result.Files;
```

Second accept dictionary where key is source file and value is output file.

```csharp
var sourceFiles = new Dictionaty<string, string>
{
    { "path/to/source.sass", "dest.css" }, // Will be placed near source file
    { "path/to/source2.sass", "path/to/dest2.css" },
    { "path/to/source3.sass", null } // Will be generated source3.css file and placed near source file
    { "path/to/source4.sass", "path/to/dest/" } // Will be generated source4.css file and placed in path/to/dest/
}

SassFilesCompilationResult result = await compiler.CompileToFilesAsync(sourceFiles);
IEnumerable<string> files = result.Files;
```

Also you can pass as second parameter output directory and all files without reletive or ablsolute path will be placed in that directory

### Messages

If output contains some Warnings, Deprecation Warnings or Debug messages they are will be presented in result.

```csharp
SassCodeCompilationResult result = await compiler.CompileAsync("/path/to/source.scss");
string code = result.Code;

// Warnings
IEnumerable<SassMessage> warnings = result.Warnings;

foreach (var warning in warnings) {
    string message = warning.Message;
    string stackTrace = warning.StackTrace;
    string rawMessage = warning.RawMessage;
}

// Deprecation warnings
IEnumerable<SassDeprecationWarning> deprecationWarnings = result.DeprecationWarnings;

foreach (var deprecationWarning in deprecationWarnings) {
    string message = deprecationWarning.Message;
    // Recomendation from Sass
    string recomendation = deprecationWarning.Recommendation;
    string stackTrace = deprecationWarning.StackTrace;
    string rawMessage = deprecationWarning.RawMessage;
}

// Debug
IEnumerable<SassMessage> debugMessages = result.Debug;

foreach (var debugMessage in debugMessages) {
    string message = debugMessage.Message;
    string stackTrace = debugMessage.StackTrace;
    string rawMessage = debugMessage.RawMessage;
}
```

### Errors

If compilation fails with errors it throws `SassCompileException`. It contains next properties:
 * `string RawOutput`
 * `IEnumerable<SassMessage> Errors`
 * `IEnumerable<SassMessage> Warnings`
 * `IEnumerable<SassDeprecationWarning> DeprecationWarnings`
 * `IEnumerable<SassMessage> Debug`

### Version

Also you can get version of used Dart Sass runtime.

```csharp
var version = await compiler.GetVersionAsync();
```
