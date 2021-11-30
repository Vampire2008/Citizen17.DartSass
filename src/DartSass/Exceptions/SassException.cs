using System;
using System.Runtime.Serialization;

namespace Citizen17.DartSass
{
    [Serializable]
    public class SassException : Exception
    {
        public string RawError { get; }

        public SassException(string rawError)
        {
            RawError = rawError;
        }

        protected SassException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
