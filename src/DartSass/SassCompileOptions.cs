using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Citizen17.DartSass;

public class SassCompileOptions
{
    /// <summary>
    /// Generate source map files (default true)
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#no-source-map"/>
    /// </summary>
    public bool? GenerateSourceMap { get; set; }

    /// <summary>
    /// Controls how the source maps that Sass generates link back to the Sass files that contributed to the generated CSS
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#source-map-urls"/>
    /// </summary>
    public SourceMapUrlType? SourceMapUrlType { get; set; }

    /// <summary>
    /// This flag tell Sass to embed the entire contents of the Sass files that contributed to the generated CSS in the source map
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#embed-sources"/>
    /// </summary>
    public bool? EmbedSources { get; set; }
    /// <summary>
    /// This flag tells Sass to embed the contents of the source map file in the generated CSS, rather than creating a separate file and linking to it from the CSS
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#embed-source-map"/>
    /// </summary>
    public bool? EmbedSourceMap { get; set; }

    /// <summary>
    /// Output style of resulting CSS
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#style"/>
    /// </summary>
    public StyleType? StyleType { get; set; }

    /// <summary>
    /// Insert charset declaration
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#error-css"/>
    /// </summary>
    public bool? EmitCharset { get; set; }

    /// <summary>
    /// Additional load path for Sass to look for stylesheets.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#load-path"/>
    /// </summary>
    public IList<string> ImportPaths { get; set; }
    /// <summary>
    /// Compile only stylesheets whose dependencies have been modified more recently than the corresponding CSS file was generated
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#update"/>
    /// </summary>
    public bool Update { get; set; }

    /// <summary>
    /// Tells Sass not to emit any warnings when compiling.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#quiet"/>
    /// </summary>
    public bool? Quiet { get; set; }

    /// <summary>
    /// Tells Sass not to emit deprecation warnings that comes from dependencies when compiling.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass#quiet-deps"/>
    /// </summary>
    public bool? QuietDeps { get; set; }

    internal string BuildArgs(bool outputToSdtout)
    {
        var sb = new StringBuilder();

        if (GenerateSourceMap.HasValue)
        {
            if (GenerateSourceMap.Value)
            {
                if (EmbedSourceMap.HasValue && EmbedSourceMap.Value || !outputToSdtout)
                {
                    sb.Append("--source-map ");
                }
                else
                {
                    throw new SassCompileException(new[] {
                            new SassMessage(Messages.ErrorCombineGenerateSourceMapAndFalseEmbed,
                        null,
                        Messages.RawErrorCombineGenerateSourceMapAndFalseEmbed)
                        },
                        Messages.RawErrorCombineGenerateSourceMapAndFalseEmbed,
                        Enumerable.Empty<SassMessage>(), 
                        Enumerable.Empty<SassDeprecationWarning>(),
                        Enumerable.Empty<SassMessage>());
                }
            }
            else
            {
                sb.Append("--no-source-map ");
            }
        }

        if (SourceMapUrlType.HasValue)
        {
            if (GenerateSourceMap.HasValue && !GenerateSourceMap.Value)
            {
                throw new SassCompileException(new[] {
                        new SassMessage(Messages.ErrorCombineSourceMapUrlTypeAndFalseSourceMaps,
                            null,
                            Messages.RawErrorCombineSourceMapUrlTypeAndFalseSourceMaps)
                    },
                    Messages.RawErrorCombineSourceMapUrlTypeAndFalseSourceMaps,
                    Enumerable.Empty<SassMessage>(),
                    Enumerable.Empty<SassDeprecationWarning>(),
                    Enumerable.Empty<SassMessage>());
            }

            if (SourceMapUrlType.Value == DartSass.SourceMapUrlType.Relative && outputToSdtout)
            {
                throw new SassCompileException(new[] {
                        new SassMessage(Messages.ErrorCombineSourceMapUrlRelativeWithStdOut,
                            null,
                            Messages.RawErrorCombineSourceMapUrlRelativeWithStdOut)
                    },
                    Messages.RawErrorCombineSourceMapUrlRelativeWithStdOut,
                    Enumerable.Empty<SassMessage>(),
                    Enumerable.Empty<SassDeprecationWarning>(),
                    Enumerable.Empty<SassMessage>());
            }
            sb.Append("--source-map-urls ");
            sb.Append(SourceMapUrlType.Value.ToString().ToLowerInvariant());
            sb.Append(' ');
        }

        if (EmbedSources.HasValue)
        {
            if (GenerateSourceMap.HasValue && !GenerateSourceMap.Value)
            {
                throw new SassCompileException(new[] {
                        new SassMessage(Messages.ErrorCombineEmbedSourcesAndFalseSourceMaps,
                            null,
                            Messages.RawErrorCombineEmbedSourcesAndFalseSourceMaps)
                    },
                    Messages.RawErrorCombineEmbedSourcesAndFalseSourceMaps,
                    Enumerable.Empty<SassMessage>(),
                    Enumerable.Empty<SassDeprecationWarning>(),
                    Enumerable.Empty<SassMessage>());
            }
            if (EmbedSources.Value)
            {
                if (EmbedSourceMap.HasValue && EmbedSourceMap.Value || !outputToSdtout)
                {
                    sb.Append("--embed-sources ");
                }
                else
                {
                    throw new SassCompileException(new[] {
                            new SassMessage(Messages.ErrorEmbedSourcesWithoutEmbedSourceMap,
                        null,
                        Messages.RawErrorEmbedSourcesWithoutEmbedSourceMap)
                        },
                        Messages.RawErrorEmbedSourcesWithoutEmbedSourceMap,
                        Enumerable.Empty<SassMessage>(),
                        Enumerable.Empty<SassDeprecationWarning>(),
                        Enumerable.Empty<SassMessage>());
                }
            }
            else
            {
                sb.Append("--no-embed-sources ");
            }
        }

        if (EmbedSourceMap.HasValue)
        {
            if (!GenerateSourceMap.HasValue || GenerateSourceMap.Value)
            {
                sb.Append(EmbedSourceMap.Value ? "--embed-source-map " : "--no-embed-source-map ");
            }
            else
            {
                throw new SassCompileException(new[] {
                        new SassMessage(Messages.ErrorCombineEmbedSourceMapAndFalseSourceMaps,
                    null,
                    Messages.RawErrorCombineEmbedSourceMapAndFalseSourceMaps)
                    },
                    Messages.RawErrorCombineEmbedSourceMapAndFalseSourceMaps,
                    Enumerable.Empty<SassMessage>(),
                    Enumerable.Empty<SassDeprecationWarning>(),
                    Enumerable.Empty<SassMessage>());
            }
        }

        if (StyleType.HasValue)
        {
            sb.Append("-s ");
            sb.Append(StyleType.Value.ToString().ToLowerInvariant());
            sb.Append(' ');
        }

        if (EmitCharset.HasValue)
        {
            sb.Append(EmitCharset.Value ? "--charset " : "--no-charset ");
        }

        if (Update && !outputToSdtout)
        {
            sb.Append("--update ");
        }

        if (ImportPaths != null)
        {
            foreach (var path in ImportPaths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                sb.Append("-I \"");
                sb.Append(path);
                sb.Append("\" ");
            }
        }

        if (Quiet.HasValue)
        {
            sb.Append(Quiet.Value ? "-q " : "--no-quiet ");
        }

        if (QuietDeps.HasValue)
        {
            sb.Append($"--{(!QuietDeps.Value ? "no-" : string.Empty)}quiet-deps ");
        }

        return sb.ToString();
    }
}
