using System;

namespace ModsStudioLib.Definitions.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    public class DefinitionValueAttribute : Attribute {
        public string VariableName { get; set; }

        public string VariableGroup { get; set; }

        public object DefaultValue { get; set; }

        public string DefaultCommentValue { get; set; }

        public string ValueComment { get; set; }

        public DefinitionValueAttribute(string variableName, string variableGroup = null, object defaultValue = null, string defaultCommentValue = null, string comment = null) {
            VariableName = variableName;
            VariableGroup = variableGroup;
            DefaultValue = defaultValue;
            DefaultCommentValue = defaultCommentValue;
            ValueComment = comment;
        }
    }
}
