using System;
using System.IO;
using System.Xml.Serialization;

using ModsStudioLib.Annotations;
using ModsStudioLib.Databases;
using ModsStudioLib.Definitions.Parsing;
using ModsStudioLib.Types;

using static System.Console;
namespace MSLibSandbox {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal static class Program {
        private static void Main(string[] args) {
            try {
                if (args.Length == 0) {
                    DefinitionDatabase.ReadDatabase();
                    ParseDumpUnits();
                }
            } catch (Exception ex) {
                WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            WriteLine();
            WriteLine("Press any key...");
            ReadKey();
        }

        private static void ParseDumpUnits() {
            var parser = new DumpedUnitsParser(@"DebugFiles\Dumped units\ETS2Units 1.25.txt");
            var result = parser.Parse();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var serializer = new XmlSerializer(typeof(SerializableDictionary<string, DefinitionStructureDescriptor>));
            using (var file = File.Create("db.xml"))
                serializer.Serialize(file, result, ns);
            //foreach (var definitionStructureDescriptor in result.Values) {
            //    WriteLine(definitionStructureDescriptor.ToStructuredString());
            //}
        }

        private static void PrintStructures() {
            var parser = new DefinitionFileParser(@"DebugFiles\Definitions\Paintjobs\scania.r\color_m1.dlc_metallics2.sii");
            var structures = parser.Parse();

            foreach (var definitionStructure in structures) {
                var text = definitionStructure.ToDefinitionString();
                WriteLine(text);
            }
        }

        private static void GetFilesWithoutMarker() {
            var files = Directory.GetFiles(@"G:\ETS 2 extracts\extracts 1.25.1.2", "*.sii", SearchOption.AllDirectories);
            foreach (var file in files) {
                var contents = File.ReadAllText(file);
                if (!contents.ToLower().StartsWith("siinunit"))
                    WriteLine(file);
            }
        }
    }
}
