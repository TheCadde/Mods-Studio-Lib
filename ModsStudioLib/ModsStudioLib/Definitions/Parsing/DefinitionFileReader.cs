using System;
using System.IO;
using ModsStudioLib.Exceptions;
using ModsStudioLib.Types;

using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Parsing {

    public class DefinitionFileReader {
        private readonly string[] lines;

        protected DefinitionFileReader(string filePath) {
            lines = File.ReadAllLines(filePath);
        }

        protected int CursorColumn { get; private set; }

        protected int CursorLine { get; private set; }

        private string CurrentLine {
            get
            {
                if (lines.Length <= CursorLine)
                    throw new DefinitionEndOfFileException($"End of file encountered when trying to read line {CursorLine}.");
                return lines[CursorLine];
            }
        }

        protected void Ensure(DefinitionFileMarkers marker, bool caseIndifferent = true, bool consumeWhiteSpace = true, string attemptedOperationMessage = null) {
            Ensure(GetMarker(marker), caseIndifferent, consumeWhiteSpace, attemptedOperationMessage);
        }

        protected void Ensure(char marker, bool caseIndifferent = true, bool consumeWhiteSpace = true, string attemptedOperationMessage = null) {
            Ensure(marker.ToString(), caseIndifferent, consumeWhiteSpace, attemptedOperationMessage);
        }

        protected void Ensure(string search, bool caseIndifferent = true, bool consumeWhiteSpace = true, string attemptedOperationMessage = null) {
            if (consumeWhiteSpace)
                ConsumeWhitespaceAndComments();
            var read = Read(search.Length, extraMessage: $" Expected '{search}'.");

            if ((caseIndifferent && !string.Equals(search, read, StringComparison.InvariantCultureIgnoreCase))
                || search != read)
                throw new DefinitionParseException($"Parsing defintion file or stream failed. Expected '{search}' but found '{read}' on line {CursorLine} column {CursorColumn}.{(string.IsNullOrEmpty(attemptedOperationMessage) ? "" : "\nATTEMPTED: " + attemptedOperationMessage)}");
        }

        protected bool Check(DefinitionFileMarkers marker, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            return Check(GetMarker(marker), caseIndifferent, consumeWhiteSpace);
        }

        protected bool Check(char marker, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            return Check(marker.ToString(), caseIndifferent, consumeWhiteSpace);
        }

        protected bool Check(string search, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            if (consumeWhiteSpace)
                ConsumeWhitespaceAndComments();
            var read = Read(search.Length, consume: false, extraMessage: $" Expected '{search}'.");

            return (!caseIndifferent || string.Equals(search, read, StringComparison.InvariantCultureIgnoreCase)) && search == read;
        }

        protected void ConsumeWhitespace() {
            while (GetMarker(DefinitionFileMarkers.WhiteSpaces).Contains(TryPeek() ?? "")) { Advance(); }
        }

        protected void ConsumeWhitespaceAndComments() {
            while (true) {
                if (GetMarker(DefinitionFileMarkers.WhiteSpaces).Contains(TryPeek() ?? ""))
                    Advance();
                else if (IsComment())
                    AdvanceLine();
                else
                    break;
            }
        }

        private bool IsComment() {
            return TryPeek() == GetMarker(DefinitionFileMarkers.SingleComment)
                || TryPeek(2) == GetMarker(DefinitionFileMarkers.DoubleComment);
        }

        public string Read(int count = 1, bool consume = true, string extraMessage = null) {
            if (CursorColumn + count > CurrentLine.Length)
                throw new DefinitionEndOfLineException($"End of line reached when trying to read {count} characters from line {CursorLine} at column {CursorColumn}.{extraMessage ?? ""}");
            var read = CurrentLine.Substring(CursorColumn, count);
            if (consume)
                Advance(count);
            return read;
        }

        protected string Read(string validCharacters, bool consumeWhitespace = true) {
            if (consumeWhitespace)
                ConsumeWhitespaceAndComments();
            var result = "";
            while (validCharacters.Contains(TryPeek() ?? ""))
                result += Read();
            return result;
        }

        public string Peek(int count = 1) {
            return Read(count, false);
        }

        public string TryPeek(int count = 1) {
            return CanRead(count) ? Read(count, false) : null;
        }

        private bool CanRead(int count) {
            try {
                return CurrentLine.Length >= CursorColumn + count;
            } catch (DefinitionEndOfFileException) {
                return false;
            }
        }

        protected void Advance(int count = 1) {
            for (var i = 0; i < count; i++) {
                if (CurrentLine.Length > CursorColumn + 1)
                    CursorColumn++;
                else {
                    CursorLine++;
                    CursorColumn = 0;
                }
            }
        }

        protected void AdvanceLine(int count = 1) {
            CursorLine += count;
            CursorColumn = 0;
        }

        protected object ReadValue(Type type) {
            var readValue = "";
            var startLine = CursorLine;
            var startColumn = CursorColumn;
            switch (type.Name) {
                case nameof(String):
                    Ensure(DefinitionFileMarkers.DoubleQuote);
                    while (!Check(DefinitionFileMarkers.DoubleQuote))
                        readValue += Read();
                    Ensure(DefinitionFileMarkers.DoubleQuote);
                    return readValue;

                case nameof(Int64):
                    try {
                        readValue = Read(SignedIntegerChars);
                        return long.Parse(readValue);
                    } catch (ArgumentNullException ex) {
                        throw new DefinitionParseException($"Could not read signed 64 bit integer from definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (FormatException ex) {
                        throw new DefinitionParseException($"Could not convert value '{readValue}' to a signed 64 bit integer in the definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (OverflowException ex) {
                        throw new DefinitionParseException($"Could value '{readValue}' was too large to be converted into a signed 64 bit integer in the definition file or stream on line {startLine} column {startColumn}", ex);
                    }

                case nameof(UInt32):
                    try {
                        readValue = Read(IntegerChars);
                        return uint.Parse(readValue);
                    } catch (ArgumentNullException ex) {
                        throw new DefinitionParseException($"Could not read unsigned 32 bit integer from definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (FormatException ex) {
                        throw new DefinitionParseException($"Could not convert value '{readValue}' to a unsigned 32 bit integer in the definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (OverflowException ex) {
                        throw new DefinitionParseException($"Could value '{readValue}' was too large to be converted into an unsigned 32 bit integer in the definition file or stream on line {startLine} column {startColumn}", ex);
                    }

                case nameof(Single):
                    try {
                        readValue = Read(FloatChars);
                        return float.Parse(readValue);
                    } catch (ArgumentNullException ex) {
                        throw new DefinitionParseException($"Could not read 32 bit floating point from definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (FormatException ex) {
                        throw new DefinitionParseException($"Could not convert value '{readValue}' to a 32 bit floating point in the definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (OverflowException ex) {
                        throw new DefinitionParseException($"Could value '{readValue}' was too large to be converted into an 32 bit floating point in the definition file or stream on line {startLine} column {startColumn}", ex);
                    }

                case nameof(Float3):
                    Ensure(DefinitionFileMarkers.OpenBrace);
                    var floatValue1 = (float)ReadValue(typeof(float));
                    Ensure(DefinitionFileMarkers.Comma);
                    var floatValue2 = (float)ReadValue(typeof(float));
                    Ensure(DefinitionFileMarkers.Comma);
                    var floatValue3 = (float)ReadValue(typeof(float));
                    Ensure(DefinitionFileMarkers.CloseBrace);
                    return new Float3(floatValue1, floatValue2, floatValue3);

                case nameof(Boolean):
                    if (Check(True)) {
                        Read(True.Length);
                        return true;
                    }
                    if (Check(False)) {
                        Read(False.Length);
                        return false;
                    }
                    throw new DefinitionParseException($"Could not read boolean value from definition file or stream on line {startLine} column {startColumn}.");

                default:
                    throw new ArgumentException($"Unhandled type {type} when trying to read value on line {startLine} column {startColumn}.");
            }
        }

        public static string ValueToString(object value) {
            switch (value.GetType().Name) {
                case nameof(String):
                    return $"\"{value}\"";

                case nameof(UInt32):
                case nameof(Int32):
                case nameof(UInt64):
                case nameof(Int64):
                    return $"{value}";

                case nameof(Single):
                    return $"{value:0.000}";

                case nameof(Float3):
                    var float3 = (Float3)value;
                    return $"({float3.X}, {float3.Y}, {float3.Z})";

                case nameof(Boolean):
                    return $"{value.ToString().ToLower()}";

                default:
                    throw new InvalidOperationException($"Could not convert value of type {value.GetType().Name} to a string as there was no coverter available.");
            }
        }
    }
}
