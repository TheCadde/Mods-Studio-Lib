using System.IO;
using System.Xml.Serialization;

using ModsStudioLib.Definitions.Parsing;
using ModsStudioLib.Types;

namespace ModsStudioLib.Databases {
    public static class DefinitionDatabase {
        public const string DatabaseFilePath = "db.xml";
        public static SerializableDictionary<string, DefinitionStructureDescriptor> StructureDescriptors = new SerializableDictionary<string, DefinitionStructureDescriptor>();

        public static void ReadDatabase() {
            using (var file = File.OpenRead(DatabaseFilePath)) {
                var serializer = new XmlSerializer(typeof(SerializableDictionary<string, DefinitionStructureDescriptor>));
                StructureDescriptors = (SerializableDictionary<string, DefinitionStructureDescriptor>)serializer.Deserialize(file);
            }
            foreach (var definitionStructureDescriptor in StructureDescriptors.Values) {
                definitionStructureDescriptor.InitializePropertyChangedEvents();
            }
        }
    }
}
