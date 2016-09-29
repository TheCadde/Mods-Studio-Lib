namespace ModsStudioLib.Definitions.Structures {
    public abstract class DefinitionStructure {
        public void Parse(string filePath) { }

        public void Write(string filePath, bool writeHidden = false) { }

        public string StructurePath { get; set; }
    }
}
