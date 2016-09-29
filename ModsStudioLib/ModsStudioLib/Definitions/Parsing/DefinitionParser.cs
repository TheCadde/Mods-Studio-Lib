﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ModsStudioLib.Definitions.Attributes;
using ModsStudioLib.Definitions.Structures;
using ModsStudioLib.Exceptions;

namespace ModsStudioLib.Definitions.Parsing {
    public class DefinitionParser : DefinitionReader {
        private readonly Stack<DefinitionParserStates> state = new Stack<DefinitionParserStates>();

        public DefinitionParser(string filePath) : base(filePath) {
            CurrentState = DefinitionParserStates.None;
        }

        public DefinitionParserStates CurrentState {
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

            while (!Check(DefinitionFileMarkers.BlockEnd)) { structures.Add(ParseStructure()); }

            Ensure(DefinitionFileMarkers.BlockEnd);
            state.Pop();

            if (state.Peek() != DefinitionParserStates.None)
                throw new DefinitionParseException($"Not all blocks were closed when parsing defintion file or stream. Expected closing block at line {CursorLine} column {CursorColumn}.");
            return structures;
        }

        private DefinitionStructure ParseStructure() {
            var type = Read(DefinitionFileConstants.StructureTypeChars);
            Ensure(DefinitionFileMarkers.StructureSeparator);
            var path = Read(DefinitionFileConstants.StructurePathChars);

            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DefinitionParserStates.Structure;

            var structure = CreateStructureFromTypeString(type);
            structure.StructurePath = path;
            var structureType = structure.GetType();

            var valueProperties = structureType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DefinitionValueAttribute))).ToList();
            var knownVariableProperties = new Dictionary<string, PropertyInfo>();
            var knownVariableAttributes = new Dictionary<string, DefinitionValueAttribute>();
            foreach (var valueProperty in valueProperties) {
                var attribs = (DefinitionValueAttribute)valueProperty.GetCustomAttribute(typeof(DefinitionValueAttribute));
                knownVariableProperties[attribs.VariableName] = valueProperty;
                knownVariableAttributes[attribs.VariableName] = attribs;
            }

            while (true) {
                if (Check(DefinitionFileMarkers.BlockEnd)) {
                    state.Pop();
                    return structure;
                }
                var variableName = Read(DefinitionFileConstants.VariableNameChars);
                Ensure(DefinitionFileMarkers.VariableSeparator);
                if (knownVariableAttributes.ContainsKey(variableName))
                    knownVariableProperties[variableName].SetValue(structure, ReadValue(knownVariableAttributes[variableName].ValueType));
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
            Ensure(DefinitionFileMarkers.MagicMarker, caseIndifferent: false, consumeWhiteSpace: false);
            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DefinitionParserStates.Nunit;
        }
    }
}
