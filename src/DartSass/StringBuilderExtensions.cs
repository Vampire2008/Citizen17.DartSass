using System.Text;

namespace Citizen17.DartSass;

internal static class StringBuilderExtensions
{
    extension(StringBuilder sb)
    {
        internal void AppendJoinParameter(string parameterName, IEnumerable<string> values)
        {
            sb.Append("--");
            sb.Append(parameterName);
            sb.Append('=');
            sb.AppendJoin(',', values);
            sb.Append(' ');
        }

        internal void AppendYesNoParameter(string parameterName, bool value)
        {
            sb.Append("--");
            if (!value) 
                sb.Append("no-");
            sb.Append(parameterName);
            sb.Append(' ');
        }
    }
}
