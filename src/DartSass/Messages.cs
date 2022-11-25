namespace Citizen17.DartSass;

public static class Messages
{
    public const string ErrorSassNotFound = "Sass not found";
    public const string ErrorEmptyFiePath = "Input file cannot be empty";
    public const string ErrorNotFound = "Source file not found";
    public const string MultipleErrorsOccurred = "Multiple errors occurred durning compilation";

    public const string ErrorCombineGenerateSourceMapAndFalseEmbed = $"When compiling to string, {nameof(SassCompileOptions.GenerateSourceMap)} requires {nameof(SassCompileOptions.EmbedSourceMap)} to be set to true";
    public const string RawErrorCombineGenerateSourceMapAndFalseEmbed = "When printing to stdout, --source-map requires --embed-source-map.";
    public const string ErrorCombineSourceMapUrlTypeAndFalseSourceMaps = $"{nameof(SassCompileOptions.SourceMapUrlType)} isn't allowed when {nameof(SassCompileOptions.GenerateSourceMap)} is set to false.";
    public const string RawErrorCombineSourceMapUrlTypeAndFalseSourceMaps = "--source-map-urls isn't allowed with --no-source-map";
    public const string ErrorCombineSourceMapUrlRelativeWithStdOut = $"{nameof(SassCompileOptions.SourceMapUrlType)} with value {nameof(SourceMapUrlType.Relative)} isn't allowed when compiling to code";
    public const string RawErrorCombineSourceMapUrlRelativeWithStdOut = "--source-map-urls=relative isn't allowed when printing to stdout.";
    public const string ErrorCombineEmbedSourcesAndFalseSourceMaps = $"{nameof(SassCompileOptions.EmbedSources)} isn't allowed when {nameof(SassCompileOptions.GenerateSourceMap)} is set to false.";
    public const string RawErrorCombineEmbedSourcesAndFalseSourceMaps = "--embed-sources isn't allowed with --no-source-map";
    public const string ErrorEmbedSourcesWithoutEmbedSourceMap = $"When compiling to string, {nameof(SassCompileOptions.EmbedSources)} requires {nameof(SassCompileOptions.EmbedSourceMap)} to be set to true.";
    public const string RawErrorEmbedSourcesWithoutEmbedSourceMap = "When printing to stdout, --source-map requires --embed-source-map.";
    public const string ErrorCombineEmbedSourceMapAndFalseSourceMaps = $"{nameof(SassCompileOptions.EmbedSourceMap)} isn't allowed when {nameof(SassCompileOptions.GenerateSourceMap)} is set to false. Now options is ignored";
    public const string RawErrorCombineEmbedSourceMapAndFalseSourceMaps = "--embed-source-map isn't allowed with --no-source-map.";
}