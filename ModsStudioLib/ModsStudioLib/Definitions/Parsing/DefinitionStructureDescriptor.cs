using System.Collections.Generic;

namespace ModsStudioLib.Definitions.Parsing {
    public class DefinitionStructureDescriptor {
        public string TypeName { get; set; }

        public string SuperClass { get; set; }

        public Dictionary<string, DefinitionStructureValueDescriptor> ValueDescriptors = new Dictionary<string, DefinitionStructureValueDescriptor>();

        public override string ToString() {
            return $"{TypeName} : {SuperClass}";
        }
    }
}
