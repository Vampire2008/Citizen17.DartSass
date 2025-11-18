namespace Citizen17.DartSass;

/// <summary>
/// Contains names of SASS deprecations that can be used to <see cref="SassCompileOptions.FatalDeprecation"/> option
/// </summary>
public static class SassDeprecations
{
    /// <summary>
    /// Passing a string directly to meta.call().
    /// </summary>
    public const string CallString = "call-string";

    /// <summary>
    /// Using @elseif instead of @else if.
    /// </summary>
    public const string ElseIf = "elseif";

    /// <summary>
    /// Using @-moz-document (except for the empty url prefix hack).
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/moz-document/"/>
    /// </summary>
    public const string MozDocument = "moz-document";

    /// <summary>
    /// Imports using relative canonical URLs.
    /// </summary>
    public const string RelativeCanonical = "relative-canonical";

    /// <summary>
    /// Declaring new variables with !global.
    /// </summary>
    public const string NewGlobal = "new-global";

    /// <summary>
    /// Using color module functions in place of plain CSS functions.
    /// </summary>
    public const string ColorModuleCompat = "color-module-compat";

    /// <summary>
    /// Using the / operator for division.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/slash-div"/>
    /// </summary>
    public const string SlashDiv = "slash-div";

    /// <summary>
    /// Leading, trailing, and repeated combinators.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/bogus-combinators"/>
    /// </summary>
    public const string BogusCombinators = "bogus-combinators";

    /// <summary>
    /// Ambiguous + and - operators.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/strict-unary"/>
    /// </summary>
    public const string StrictUnary = "strict-unary";

    /// <summary>
    /// Passing invalid units to built-in functions.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/function-units"/>
    /// </summary>
    public const string FunctionUnits = "function-units";

    /// <summary>
    /// Using multiple copies of !global or !default in a single variable declaration.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/duplicate-var-flags"/>
    /// </summary>
    public const string DuplicateVarFlags = "duplicate-var-flags";

    /// <summary>
    /// Passing percentages to the Sass abs() function.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/abs-percent/"/>
    /// </summary>
    public const string AbsPercent = "abs-percent";

    /// <summary>
    /// Mixins and functions whose names begin with --
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/css-function-mixin/"/>
    /// </summary>
    public const string CssFunctionMixing = "css-function-mixin";

    /// <summary>
    /// Declarations after or between nested rules.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/mixed-decls/"/>
    /// </summary>
    public const string MixedDeclarations = "mixed-decls";

    /// <summary>
    /// meta.feature-exists
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/feature-exists/"/>
    /// </summary>
    public const string FeatureExists = "feature-exists";

    /// <summary>
    /// Certain uses of built-in sass:color functions.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/color-4-api"/>
    /// </summary>
    public const string Color4Api = "color-4-api";

    /// <summary>
    /// Using global color functions instead of sass:color
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/color-functions"/>
    /// </summary>
    public const string ColorFunctions = "color-functions";

    /// <summary>
    /// @import rules.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/import"/>
    /// </summary>
    public const string Import = "import";

    /// <summary>
    /// Global built-in functions that are available in sass: modules
    /// </summary>
    public const string GlobalBuildIn = "global-builtin";

    /// <summary>
    /// Functions named “type”.
    /// </summary>
    public const string TypeFunction = "type-function";

    /// <summary>
    /// Passing a relative url to compileString().
    /// </summary>
    public const string CompileStringRelativeUrl = "compile-string-relative-url";

    /// <summary>
    /// A rest parameter before a positional or named parameter.
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/misplaced-rest"/>
    /// </summary>
    public const string MisplacedRest = "misplaced-rest";

    /// <summary>
    /// Configuring private variables in @use, @forward, or load-css().
    /// <seealso href="https://sass-lang.com/documentation/breaking-changes/with-private"/>
    /// </summary>
    public const string WithPrivate = "with-private";

    public static class Future
    {
        // Currently there aren't future deprecations
    }
}
