using System;
using System.Runtime.Serialization;

namespace ModsStudioLib.Exceptions {
    [Serializable]
    internal class DefinitionParseException : Exception {
        public DefinitionParseException() {
        }

        public DefinitionParseException(string message) : base(message) {
        }

        public DefinitionParseException(string message, Exception innerException) : base(message, innerException) {
        }

        protected DefinitionParseException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}