namespace Citizen17.DartSass
{
    public enum SourceMapUrlType
    {
        /// <summary>
        /// Uses relative URLs from the location of the source map file to the locations of the Sass source file
        /// </summary>
        Relative,
        /// <summary>
        /// Uses the absolute file: URLs of the Sass source files. Note that absolute URLs will only work on the same computer that the CSS was compiled
        /// </summary>
        Absolute
    }
}
