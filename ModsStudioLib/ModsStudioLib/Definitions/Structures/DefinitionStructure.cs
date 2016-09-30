using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.Parsing;

using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Structures {
    public abstract class DefinitionStructure {
        public void Parse(string filePath) { }

        public void Write(string filePath, bool writeHidden = false) { }

        public string StructurePath { get; set; }

        public DefinitionStructureAttribute GetStructureAttribute() {
            return (DefinitionStructureAttribute)GetType().GetCustomAttribute(typeof(DefinitionStructureAttribute));
        }

        public Dictionary<string, PropertyInfo> GetValueProperties() {
            var result = new Dictionary<string, PropertyInfo>();
            var valueProperties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefinitionValueAttribute)));
            foreach (var valueProperty in valueProperties) {
                var attribs = (DefinitionValueAttribute)valueProperty.GetCustomAttribute(typeof(DefinitionValueAttribute));
                result[attribs.VariableName] = valueProperty;
            }
            return result;
        }
        public Dictionary<string, DefinitionValueAttribute> GetValueAttributes() {
            var result = new Dictionary<string, DefinitionValueAttribute>();
            var valueProperties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefinitionValueAttribute)));
            foreach (var valueProperty in valueProperties) {
                var attribs = (DefinitionValueAttribute)valueProperty.GetCustomAttribute(typeof(DefinitionValueAttribute));
                result[attribs.VariableName] = attribs;
            }
            return result;
        }

        public override string ToString() {
            var attrib = GetStructureAttribute();
            return $"{attrib.TypeName} : {StructurePath} {{ ... }}";
        }

        public string ToDefinitionString() {
            var sb = new StringBuilder();

            //sb.AppendLine(GetMarker(DefinitionFileMarkers.MagicMarker));
            //sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockStart));

            var attrib = GetStructureAttribute();
            sb.AppendLine($"{attrib.TypeName} {GetMarker(DefinitionFileMarkers.StructureSeparator)} {StructurePath}");
            sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockStart));

            var valueAttributes = GetValueAttributes();
            var valueProperties = GetValueProperties();
            foreach (var varName in valueAttributes.Keys) {
                sb.AppendLine($"    {varName} {GetMarker(DefinitionFileMarkers.VariableSeparator)} {DefinitionFileReader.ValueToString(valueProperties[varName].GetValue(this))}");
            }

            sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockEnd));
            //sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockEnd));
            return sb.ToString();
        }
    }
}
