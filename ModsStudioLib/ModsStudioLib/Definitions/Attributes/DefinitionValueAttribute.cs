using System;

using ModsStudioLib.Definitions.Parsing;

namespace ModsStudioLib.Definitions.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    class DefinitionValueAttribute : Attribute {
        public string VariableName { get; set; }

        public DefinitionValueTypes ValueType { get; set; }

        public object DefaultCommentValue { get; set; }

        public string ValueComment { get; set; }

        public DefinitionValueAttribute(string variableName, DefinitionValueTypes valueType, string defaultCommentValue = null, string comment = null) {
            VariableName = variableName;
            ValueType = valueType;
            DefaultCommentValue = defaultCommentValue;
            ValueComment = comment;
        }
    }
}
