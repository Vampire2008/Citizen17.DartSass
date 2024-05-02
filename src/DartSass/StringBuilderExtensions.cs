﻿using System.Collections.Generic;
using System.Text;

namespace Citizen17.DartSass;

internal static class StringBuilderExtensions
{
    internal static void AppendJoinParameter(this StringBuilder sb, string parameterName, IEnumerable<string> values)
    {
        sb.Append("--");
        sb.Append(parameterName);
        sb.Append("=");
        sb.AppendJoin(',', values);
        sb.Append(' ');
    }

    internal static void AppendYesNoParameter(this StringBuilder sb, string parameterName, bool value)
    {
        sb.Append("--");
        if (!value) 
            sb.Append("no-");
        sb.Append(parameterName);
        sb.Append(' ');
    }
}
