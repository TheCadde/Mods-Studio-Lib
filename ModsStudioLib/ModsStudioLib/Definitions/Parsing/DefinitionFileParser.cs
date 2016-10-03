
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.Structures;
using ModsStudioLib.Exceptions;
using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Parsing {
    public class DefinitionFileParser : DefinitionFileReader {
        private readonly Stack<DefinitionFileParserStates> state = new Stack<DefinitionFileParserStates>();

        public DefinitionFileParser(string filePath) : base(filePath) {
            CurrentState = DefinitionFileParserStates.None;
        }

        public DefinitionFileParserStates CurrentState {
            get {
                return state.Peek();
            }
            set {
                state.Push(value);
            }
        }

        public List<DefinitionStructure> Parse() {
            var structures = new List<DefinitionStructure>();
            CheckMagicMarker();

            while (!Check(DefinitionFileMarkers.BlockEnd))
                structures.Add(ParseStructure());

            Ensure(DefinitionFileMarkers.BlockEnd, attemptedOperationMessage: $"Find closing block for {GetMarker(DefinitionFileMarkers.MagicMarker)}");
            state.Pop();

            if (state.Peek() != DefinitionFileParserStates.None)
                throw new DefinitionParseException($"Not all blocks were closed when parsing defintion file or stream. Expected closing block at line {CursorLine} column {CursorColumn}.");
            return structures;
        }

        private DefinitionStructure ParseStructure() {
            var type = Read(StructureTypeChars);
            Ensure(DefinitionFileMarkers.StructureSeparator, attemptedOperationMessage: "Finding structure separator while parsing structure descriptor.");
            var path = Read(StructurePathChars);

            Ensure(DefinitionFileMarkers.BlockStart, attemptedOperationMessage: "Find opening block for newly parsed structure.");
            CurrentState = DefinitionFileParserStates.Structure;

            var structure = CreateStructureFromTypeString(type);
            structure.StructurePath = path;
            var structureType = structure.GetType();

            var valueProperties = structureType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefinitionStructureValueAttribute)));
            var knownVariableProperties = new Dictionary<string, PropertyInfo>();
            var knownVariableAttributes = new Dictionary<string, DefinitionStructureValueAttribute>();
            foreach (var valueProperty in valueProperties) {
                var attribs = (DefinitionStructureValueAttribute)valueProperty.GetCustomAttribute(typeof(DefinitionStructureValueAttribute));
                knownVariableProperties[attribs.VariableName] = valueProperty;
                knownVariableAttributes[attribs.VariableName] = attribs;
            }

            while (true) {
                if (Check(DefinitionFileMarkers.BlockEnd)) {
                    state.Pop();
                    return structure;
                }
                var variableName = Read(VariableNameChars);
                Ensure(DefinitionFileMarkers.VariableSeparator, attemptedOperationMessage: "Find variable separator while parsing variable value.");
                if (knownVariableAttributes.ContainsKey(variableName))
                    knownVariableProperties[variableName].SetValue(structure, ReadValue(knownVariableProperties[variableName].PropertyType));
                else
                    AdvanceLine();
            }
        }

        private DefinitionStructure CreateStructureFromTypeString(string structureType) {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes()
                               .Where(typeof(DefinitionStructure).IsAssignableFrom)
                               .First(t => t.Name.ToLower() == structureType.ToLower().Replace("_", ""));
            return (DefinitionStructure)Activator.CreateInstance(type);
        }

        private void CheckMagicMarker() {
            Ensure(DefinitionFileMarkers.MagicMarker, caseIndifferent: false, consumeWhiteSpace: false, attemptedOperationMessage: $"Ensuring the first characters in file is {GetMarker(DefinitionFileMarkers.MagicMarker)}");
            Ensure(DefinitionFileMarkers.BlockStart, attemptedOperationMessage: $"Find opening block for {GetMarker(DefinitionFileMarkers.MagicMarker)}.");
            CurrentState = DefinitionFileParserStates.Nunit;
        }
    }
}
