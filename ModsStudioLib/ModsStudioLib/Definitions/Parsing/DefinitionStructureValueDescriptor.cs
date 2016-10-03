using System;
using System.Collections.Generic;

namespace ModsStudioLib.Definitions.Parsing {
    public class DefinitionStructureValueDescriptor {
        public string VariableName { get; set; }

        public string PropertyName { get; set; }

        public string VariableGroup { get; set; }

        public string VariableDefinitionType { get; set; }

        public Type VariableType { get; set; }

        public object DefaultValue { get; set; }

        public string DefaultValueComment { get; set; }

        public string Comment { get; set; }

        public List<string> Values { get; } = new List<string>();

        public override string ToString() {
            return $"{VariableDefinitionType} {VariableName}";
        }
    }
}