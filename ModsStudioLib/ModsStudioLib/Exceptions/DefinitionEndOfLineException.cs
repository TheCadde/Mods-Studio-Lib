using System;
using System.Runtime.Serialization;

namespace ModsStudioLib.Exceptions {
    [Serializable]
    internal class DefinitionEndOfLineException : Exception {
        public DefinitionEndOfLineException() {
        }

        public DefinitionEndOfLineException(string message) : base(message) {
        }

        public DefinitionEndOfLineException(string message, Exception innerException) : base(message, innerException) {
        }

        protected DefinitionEndOfLineException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}