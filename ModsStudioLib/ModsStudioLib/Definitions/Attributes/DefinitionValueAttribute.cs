using System;

namespace ModsStudioLib.Definitions.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    public class DefinitionValueAttribute : Attribute {
        public string VariableName { get; set; }

        public object DefaultCommentValue { get; set; }

        public string ValueComment { get; set; }

        public DefinitionValueAttribute(string variableName, string defaultCommentValue = null, string comment = null) {
            VariableName = variableName;
            DefaultCommentValue = defaultCommentValue;
            ValueComment = comment;
        }
    }
}
