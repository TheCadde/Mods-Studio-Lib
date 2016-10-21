using System.Linq;
using System.Text;
using System.Xml.Serialization;

using ModsStudioLib.Annotations;
using ModsStudioLib.Types;
using ModsStudioLib.Utils.Helpers;
// ReSharper disable ExplicitCallerInfoArgument

namespace ModsStudioLib.Definitions.Parsing {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DefinitionStructureDescriptor : BindableAttributeClass {
        private string typeName;
        private string superClass;
        private string comment;
        private string className;
        private string ns;

        public string TypeName {
            get { return typeName; }
            set { Set(value, out typeName); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string SuperClass {
            get { return superClass; }
            set { Set(value, out superClass); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string Comment {
            get { return comment; }
            set { Set(value, out comment); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string ClassName {
            get { return className; }
            set { Set(value, out className); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string Namespace {
            get { return ns; }
            set { Set(value, out ns); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        [XmlIgnore]
        public bool ReadFromDatabase { get; private set; }

        [XmlIgnore]
        public int Progress {
            get {
                var p = 0;
                p += string.IsNullOrEmpty(Comment) ? 0 : 30;
                p += string.IsNullOrEmpty(SuperClass) ? 0 : 10;
                p += Namespace == "Definitions.Structures.Other" || !Namespace.StartsWith("Definitions.Structures") ? 0 : 10;
                p += ValueDescriptors.Count > 0 ? 50 * ValueDescriptors.Count(kvp => kvp.Value.Progress == 100) / ValueDescriptors.Count : 50;
                p = string.IsNullOrEmpty(TypeName) ? 0 : p;
                p = string.IsNullOrEmpty(ClassName) ? 0 : p;
                p = string.IsNullOrEmpty(Namespace) ? 0 : p;
                return p;
            }
        }

        [XmlIgnore]
        public double ProgressNormal => Progress / 100D;

        [XmlIgnore]
        public double ProgressNormalOffset => (Progress + 10) / 100D;

        public SerializableDictionary<string, DefinitionValueDescriptor> ValueDescriptors = new SerializableDictionary<string, DefinitionValueDescriptor>();

        public DefinitionStructureDescriptor() {
        }

        public DefinitionStructureDescriptor(string typeName) {
            TypeName = typeName;
            ReadFromDatabase = true;
        }

        public void InitializePropertyChangedEvents() {
            foreach (var definitionValueDescriptor in ValueDescriptors.Values) {
                definitionValueDescriptor.PropertyChanged += (sender, args) => {
                    if (args.PropertyName == "Progress")
                        Notify("Progress", "ProgressNormal", "ProgressNormalOffset");
                };
            }
        }

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
