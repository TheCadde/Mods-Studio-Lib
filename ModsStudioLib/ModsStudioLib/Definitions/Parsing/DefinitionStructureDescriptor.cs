using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModsStudioLib.Utils.Helpers;

namespace ModsStudioLib.Definitions.Parsing {
    public class DefinitionStructureDescriptor {
        public string TypeName { get; set; }

        public string SuperClass { get; set; }

        public Dictionary<string, DefinitionStructureValueDescriptor> ValueDescriptors = new Dictionary<string, DefinitionStructureValueDescriptor>();

        public override string ToString() {
            return $"{TypeName} : {SuperClass}";
        }

        public string ToStructuredString() {
            var sb = new StringBuilder();
            sb.AppendLine($"{TypeName} : {SuperClass} {{");
            var typePadding = StringHelper.FindColumnAlignedPadding(ValueDescriptors.Values.Select(v => v.VariableDefinitionType));
            var namePadding = StringHelper.FindColumnAlignedPadding(ValueDescriptors.Values.Select(v => v.VariableName));
            foreach (var valueDescriptor in ValueDescriptors.Values) {
                sb.AppendLine(valueDescriptor.ToStructuredString(typePadding, namePadding));
            }
            sb.AppendLine("}\n");

            return sb.ToString();
        }
    }
}
