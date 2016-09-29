using System;
using System.Runtime.Serialization;

namespace ModsStudioLib.Exceptions {
    [Serializable]
    internal class DefinitionEndOfFileException : Exception {
        public DefinitionEndOfFileException() {
        }

        public DefinitionEndOfFileException(string message) : base(message) {
        }

        public DefinitionEndOfFileException(string message, Exception innerException) : base(message, innerException) {
        }

        protected DefinitionEndOfFileException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}