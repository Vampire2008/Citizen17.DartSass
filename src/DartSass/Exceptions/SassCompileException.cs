using System;
using System.Runtime.Serialization;

namespace Citizen17.DartSass
{
    [Serializable]
    public class SassCompileException : SassException
    {
        public string ErrorMessage { get; }
        public string ErrorPosition { get; }

        public SassCompileException(string errorMessage, string errorPosition, string rawError) : base(rawError)
        {
            ErrorMessage = errorMessage;
            ErrorPosition = errorPosition;
        }

        protected SassCompileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
