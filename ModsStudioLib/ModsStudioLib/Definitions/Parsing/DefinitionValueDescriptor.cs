using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using ModsStudioLib.Annotations;
using ModsStudioLib.Utils.Helpers;
// ReSharper disable ExplicitCallerInfoArgument

namespace ModsStudioLib.Definitions.Parsing {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DefinitionValueDescriptor : BindableAttributeClass {
        private string variableName;
        private string propertyName;
        private string variableGroup;
        private string variableDefinitionType;
        private Type variableType;
        private object defaultValue;
        private string defaultValueComment;
        private string comment;

        public string VariableName {
            get { return variableName; }
            set { Set(value, out variableName); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string PropertyName {
            get { return propertyName; }
            set { Set(value, out propertyName); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string VariableGroup {
            get { return variableGroup; }
            set { Set(value, out variableGroup); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string VariableDefinitionType {
            get { return variableDefinitionType; }
            set { Set(value, out variableDefinitionType); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public Type VariableType {
            get { return variableType; }
            set { Set(value, out variableType); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public object DefaultValue {
            get { return defaultValue; }
            set { Set(value, out defaultValue); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string DefaultValueComment {
            get { return defaultValueComment; }
            set { Set(value, out defaultValueComment); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public string Comment {
            get { return comment; }
            set { Set(value, out comment); Notify("Progress", "ProgressNormal", "ProgressNormalOffset"); }
        }

        public List<string> Values { get; } = new List<string>();

        [XmlIgnore]
        public bool ReadFromDatabase { get; private set; }

        [XmlIgnore]
        public int Progress {
            get {
                var p = 0;
                p += string.IsNullOrEmpty(Comment) ? 5 : 20;
                p += string.IsNullOrEmpty(PropertyName) ? 5 : 20;
                p += string.IsNullOrEmpty(VariableGroup) ? 5 : 20;
                p += string.IsNullOrEmpty(DefaultValueComment) ? 5 : 20;
                p += DefaultValue == null ? 5 : 20;
                p = string.IsNullOrEmpty(VariableName) ? 0 : p;
                p = string.IsNullOrEmpty(VariableDefinitionType) ? 0 : p;
                return p;
            }
        }

        [XmlIgnore]
        public double ProgressNormal => Progress / 100D;

        [XmlIgnore]
        public double ProgressNormalOffset => (Progress + 10) / 100D;

        public DefinitionValueDescriptor() {
        }

        public DefinitionValueDescriptor(string variableName) {
            VariableName = variableName;
            ReadFromDatabase = true;
        }

        public override string ToString() {
            return $"{VariableDefinitionType} {VariableName}";
        }

        public string ToStructuredString(int typePadding = 0, int namePadding = 0) {
            var values = Values.Count > 0 ? $" [{string.Join(", ", Values)}]" : "" ;
            if (values == "")
                namePadding = 0;
            return $"    {VariableDefinitionType.PadRight(typePadding)} {VariableName.PadRight(namePadding)}{values}";
        }
    }
}