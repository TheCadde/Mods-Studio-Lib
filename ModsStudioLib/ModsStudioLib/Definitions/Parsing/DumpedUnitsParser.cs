﻿using System.Collections.Generic;
using System.Globalization;

using ModsStudioLib.Exceptions;
using ModsStudioLib.Types;

using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Parsing {
    public class DumpedUnitsParser : DefinitionFileReader {
        private readonly Stack<DumpedUnitsParserStates> state = new Stack<DumpedUnitsParserStates>();

        public DumpedUnitsParser(string filePath) : base(filePath) {
            CurrentState = DumpedUnitsParserStates.None;
        }

        public DumpedUnitsParserStates CurrentState {
            get {
                return state.Peek();
            }
            set {
                state.Push(value);
            }
        }

        public SerializableDictionary<string, DefinitionStructureDescriptor> Parse() {
            var result = new SerializableDictionary<string, DefinitionStructureDescriptor>();

            SkipUninterestingBits();

            while (!CheckForEndBlock())
                ParseStructure(result);

            if (Check(DefinitionFileMarkers.Comma))
                Advance();
            Ensure(DefinitionFileMarkers.BlockEnd, attemptedOperationMessage: "Find closing block for 'root'.");
            state.Pop();

            if (CurrentState != DumpedUnitsParserStates.None)
                throw new DefinitionParseException($"Not all blocks were closed when parsing dumped units file. Expected closing block at line {CursorLine + 1} column {CursorColumn}.");

            return result;
        }

        private void ParseStructure(SerializableDictionary<string, DefinitionStructureDescriptor> result) {
            var typeName = (string)ReadValue(typeof(string));
            Ensure(DefinitionFileMarkers.StructureSeparator);
            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DumpedUnitsParserStates.Structure;

            result[typeName] = new DefinitionStructureDescriptor {
                TypeName = typeName,
                ClassName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(typeName).Replace("_", ""),
                Namespace =  "Definitions.Structures.Other"
            };

            while (!CheckForEndBlock())
                ParseStructureBlock(result, typeName);

            if (Check(DefinitionFileMarkers.Comma))
                Advance();
        }

        private void ParseStructureBlock(SerializableDictionary<string, DefinitionStructureDescriptor> result, string typeName) {
            var dataBlock = (string)ReadValue(typeof(string));
            switch (dataBlock) {
                case "superclass":
                    Ensure(DefinitionFileMarkers.StructureSeparator);
                    result[typeName].SuperClass = (string)ReadValue(typeof(string));
                    if (Check(DefinitionFileMarkers.Comma))
                        Advance();
                    break;
                case "attrs":
                    Ensure(DefinitionFileMarkers.StructureSeparator);
                    Ensure(DefinitionFileMarkers.BlockStart);
                    CurrentState = DumpedUnitsParserStates.Variable;

                    while (!CheckForEndBlock())
                        ParseVariable(result, typeName);

                    if (Check(DefinitionFileMarkers.Comma))
                        Advance();
                    break;
                default:
                    throw new DefinitionParseException($"Unexpected data block type '{dataBlock}' while parsing dumped units at line {CursorLine + 1} column {CursorColumn}.");
            }
        }

        private void ParseVariable(SerializableDictionary<string, DefinitionStructureDescriptor> result, string typeName) {
            var variableName = (string)ReadValue(typeof(string));
            Ensure(DefinitionFileMarkers.StructureSeparator);
            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DumpedUnitsParserStates.VariableData;

            while (!CheckForEndBlock())
                ParseVariableBlock(result, typeName, variableName);

            if (Check(DefinitionFileMarkers.Comma))
                Advance();
        }

        private void ParseVariableBlock(SerializableDictionary<string, DefinitionStructureDescriptor> result, string typeName, string variableName) {
            var dataType = (string)ReadValue(typeof(string));
            Ensure(DefinitionFileMarkers.StructureSeparator);

            switch (dataType) {
                case "type":
                    var variableType = (string)ReadValue(typeof(string));
                    result[typeName].ValueDescriptors[variableName] = new DefinitionValueDescriptor {
                        VariableName = variableName,
                        VariableDefinitionType = variableType,
                    };
                    break;
                case "values":

                    Ensure(DefinitionFileMarkers.OpenSquareBracket);
                    CurrentState = DumpedUnitsParserStates.VariableEnum;

                    while (!CheckForEndBlock(DefinitionFileMarkers.CloseSquareBracket))
                        result[typeName].ValueDescriptors[variableName].Values.Add((string)ReadValue(typeof(string)));

                    break;
                default:
                    throw new DefinitionParseException($"Unexpected variable block type '{dataType}' while parsing dumped units at line {CursorLine + 1} column {CursorColumn}.");
            }
        }

        private bool CheckForEndBlock(DefinitionFileMarkers marker = DefinitionFileMarkers.BlockEnd) {
            if (Check(DefinitionFileMarkers.Comma))
                Advance();
            // ReSharper disable once InvertIf
            if (Check(marker)) {
                Advance();
                state.Pop();
                return true;
            }
            return false;
        }

        private void SkipUninterestingBits() {
            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DumpedUnitsParserStates.Root;

            Ensure(GetMarker(DefinitionFileMarkers.DoubleQuote) + "units" + GetMarker(DefinitionFileMarkers.DoubleQuote));
            Ensure(DefinitionFileMarkers.StructureSeparator);
            Ensure(DefinitionFileMarkers.BlockStart);
            CurrentState = DumpedUnitsParserStates.Units;
        }
    }
}
