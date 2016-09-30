using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.Parsing;
using ModsStudioLib.Utils.Helpers;

using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Structures {
    public abstract class DefinitionStructure {
        private Dictionary<string, PropertyInfo> valueProperties;
        private Dictionary<string, DefinitionValueAttribute> valueAttributes;

        public void Parse(string filePath) { }

        public void Write(string filePath, bool writeHidden = false) { }

        public string StructurePath { get; set; }

        public Dictionary<string, PropertyInfo> ValueProperties {
            get {
                if (valueProperties == null || valueAttributes == null)
                    InitCollections();
                return valueProperties;
            }
        }

        public DefinitionStructureAttribute StructureAttribute => (DefinitionStructureAttribute)GetType().GetCustomAttribute(typeof(DefinitionStructureAttribute));

        public Dictionary<string, DefinitionValueAttribute> ValueAttributes {
            get {
                if (valueProperties == null || valueAttributes == null)
                    InitCollections();
                return valueAttributes;
            }
        }

        public Dictionary<string, string> ValueStrings { get; private set; }

        private void InitCollections() {
            valueProperties = new Dictionary<string, PropertyInfo>();
            valueAttributes = new Dictionary<string, DefinitionValueAttribute>();

            var props = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefinitionValueAttribute)));
            foreach (var valueProperty in props) {
                var attribs = (DefinitionValueAttribute)valueProperty.GetCustomAttribute(typeof(DefinitionValueAttribute));
                valueProperties[attribs.VariableName] = valueProperty;
                valueAttributes[attribs.VariableName] = attribs;
            }
        }

        public void RefreshValueStrings() {
            ValueStrings = new Dictionary<string, string>();
            foreach (var varName in ValueProperties.Keys)
                ValueStrings[varName] = DefinitionFileReader.ValueToString(ValueProperties[varName].GetValue(this));

        }

        public override string ToString() {
            return $"{StructureAttribute.TypeName} : {StructurePath} {{ ... }}";
        }

        public string ToDefinitionString(int indentBy = 4) {
            RefreshValueStrings();

            var namePadding = StringHelper.FindColumnAlignedPadding(ValueProperties.Keys);
            var valuePadding = StringHelper.FindColumnAlignedPadding(ValueStrings.Values);
            var defaultCommentPadding = StringHelper.FindColumnAlignedPadding(ValueAttributes.Values.Select(x => DefaultComment + x.DefaultCommentValue));

            var sb = new StringBuilder();

            //sb.AppendLine(GetMarker(DefinitionFileMarkers.MagicMarker));
            //sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockStart));

            sb.AppendLine($"{StructureAttribute.TypeName} {GetMarker(DefinitionFileMarkers.StructureSeparator)} {StructurePath}");
            sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockStart));

            foreach (var varName in valueAttributes.Keys) {
                var attribs = ValueAttributes[varName];
                sb.AppendLine($"{"".PadRight(indentBy)}{varName.PadRight(namePadding)}{GetMarker(DefinitionFileMarkers.VariableSeparator)} {ValueStrings[varName].PadRight(valuePadding)} {(DefaultComment + attribs.DefaultCommentValue).PadRight(defaultCommentPadding)} {attribs.ValueComment}");
            }

            sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockEnd));
            //sb.AppendLine(GetMarker(DefinitionFileMarkers.BlockEnd));
            return sb.ToString();
        }
    }
}
