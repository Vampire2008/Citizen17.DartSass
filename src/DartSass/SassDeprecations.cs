﻿namespace Citizen17.DartSass;

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
    /// Mixins and functions whose names begin with --
    /// </summary>
    public const string CssFunctionMixing = "css-function-mixin";

    public static class Future
    {
        public const string Import = "import";
    }
}
