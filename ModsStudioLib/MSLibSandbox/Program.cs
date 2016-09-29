using System;
using System.IO;

using ModsStudioLib.Definitions.Parsing;

using static System.Console;
namespace MSLibSandbox {
    internal static class Program {
        private static void Main(string[] args) {
            try {
                var parser = new DefinitionParser(@"G:\ETS 2 extracts\extracts 1.25.1.2\def\vehicle\truck\volvo.fh16\paint_job\color_m1.dlc_metallics2.sii");
                var structures = parser.Parse();

            } catch (Exception ex) {
                WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            WriteLine();
            WriteLine("Press any key...");
            ReadKey();
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
