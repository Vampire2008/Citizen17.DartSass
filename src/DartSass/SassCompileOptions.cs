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
    public IList<string>? ImportPaths { get; set; }
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

    /// <summary>
    /// Tells Sass to parse the input file as the indented syntax.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#indented"/>
    /// </summary>
    public bool? Indented { get; set; }

    /// <summary>
    /// Built-in importer(s) to use for pkg: URLs.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#pkg-importer-nodejs"/>
    /// </summary>
    public SassPkgImporterType? PkgImporter { get; set; }

    /// <summary>
    /// Tells Sass whether to emit a CSS file when an error occurs during compilation.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#error-css"/>
    /// </summary>
    public bool? ErrorCSS { get; set; }

    /// <summary>
    /// This option tells Sass to treat a particular type of deprecation warning as an error.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#fatal-deprecation"/>
    /// </summary>
    public IEnumerable<string>? FatalDeprecation { get; set; }

    /// <summary>
    /// This option tells Sass to opt-in to a future type of deprecation warning early, emitting warnings even though the deprecation is not yet active.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#future-deprecation"/>
    /// </summary>
    public IEnumerable<string>? FutureDeprecation { get; set; }

    /// <summary>
    /// This option tells Sass to silence a particular type of deprecation warning if you wish to temporarily ignore the deprecation.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#silence-deprecation"/>
    /// </summary>
    public IEnumerable<string>? SilenceDeprecation { get; set; }

    /// <summary>
    /// This flag tells Sass to stop compiling immediately when an error is detected, rather than trying to compile other Sass files that may not contain errors.
    /// <seealso href="https://sass-lang.com/documentation/cli/dart-sass/#stop-on-error"/>
    /// </summary>
    public bool StopOnError { get; set; }

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
            }
            else
            {
                sb.Append("--no-source-map ");
            }
        }

        if (SourceMapUrlType.HasValue)
        {
            sb.Append("--source-map-urls ");
            sb.Append(SourceMapUrlType.Value.ToString().ToLowerInvariant());
            sb.Append(' ');
        }

        if (EmbedSources.HasValue)
        {
            if (EmbedSources.Value)
            {
                if (EmbedSourceMap.HasValue && EmbedSourceMap.Value || !outputToSdtout)
                {
                    sb.Append("--embed-sources ");
                }
            }
            else
            {
                sb.Append("--no-embed-sources ");
            }
        }

        if (EmbedSourceMap.HasValue)
        {
            sb.AppendYesNoParameter("embed-source-map", EmbedSourceMap.Value);
        }

        if (StyleType.HasValue)
        {
            sb.Append("-s ");
            sb.Append(StyleType.Value.ToString().ToLowerInvariant());
            sb.Append(' ');
        }

        if (EmitCharset.HasValue)
        {
            sb.AppendYesNoParameter("charset", EmitCharset.Value);
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

                sb.Append("-I ");
                sb.Append(path);
                sb.Append(' ');
            }
        }

        if (Quiet.HasValue)
        {
            sb.Append(Quiet.Value ? "-q " : "--no-quiet ");
        }

        if (QuietDeps.HasValue)
        {
            sb.AppendYesNoParameter("quiet-deps", QuietDeps.Value);
        }

        if (Indented.HasValue)
        {
            sb.AppendYesNoParameter("indented", Indented.Value);
        }

        if (PkgImporter.HasValue)
        {
            sb.Append($"--pkg-importer={PkgImporter.Value.ToString().ToLowerInvariant()} ");
        }

        if (ErrorCSS.HasValue)
        {
            sb.AppendYesNoParameter("error-css", ErrorCSS.Value);
        }

        if (FatalDeprecation?.Any() ?? false)
        {
            sb.AppendJoinParameter("fatal-deprecation", FatalDeprecation);
        }

        if (FutureDeprecation?.Any() ?? false)
        {
            sb.AppendJoinParameter("future-deprecation", FutureDeprecation);
        }

        if (SilenceDeprecation?.Any() ?? false)
        {
            sb.AppendJoinParameter("silence-deprecation", SilenceDeprecation);
        }

        if (StopOnError)
        {
            sb.Append("--stop-on-error ");
        }

        return sb.ToString();
    }
}
