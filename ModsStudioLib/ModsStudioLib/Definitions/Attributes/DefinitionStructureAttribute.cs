using System;

namespace ModsStudioLib.Definitions.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class DefinitionStructureAttribute : Attribute {
        public string TypeName { get; set; }

        public string StructureComment { get; set; }

        public DefinitionStructureAttribute(string typeName, string comment = null) {
            TypeName = typeName;
            StructureComment = comment;
        }
    }
}