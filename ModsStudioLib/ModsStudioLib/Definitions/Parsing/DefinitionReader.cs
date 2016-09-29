using System;
using System.IO;
using ModsStudioLib.Exceptions;
using ModsStudioLib.Types;

using static ModsStudioLib.Definitions.Parsing.DefinitionFileConstants;

namespace ModsStudioLib.Definitions.Parsing {

    public class DefinitionReader {
        private readonly string[] lines;

        protected DefinitionReader(string filePath) {
            lines = File.ReadAllLines(filePath);
        }

        protected int CursorColumn { get; private set; }

        protected int CursorLine { get; private set; }

        private string CurrentLine {
            get
            {
                if (lines.Length < CursorLine)
                    throw new DefinitionEndOfFileException($"End of file encountered when trying to read line {CursorLine}.");
                return lines[CursorLine];
            }
        }

        protected void Ensure(DefinitionFileMarkers marker, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            Ensure(GetMarker(marker), caseIndifferent, consumeWhiteSpace);
        }

        protected void Ensure(char marker, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            Ensure(marker.ToString(), caseIndifferent, consumeWhiteSpace);
        }

        // TODO: Introduce a message stating what was being attempted here as well.
        protected void Ensure(string search, bool caseIndifferent = true, bool consumeWhiteSpace = true) {
            if (consumeWhiteSpace)
                ConsumeWhitespaceAndComments();
            var read = Read(search.Length, extraMessage: $" Expected '{search}'.");

            if ((caseIndifferent && !string.Equals(search, read, StringComparison.InvariantCultureIgnoreCase))
                || search != read)
                throw new DefinitionParseException($"Parsing defintion file or stream failed. Expected '{search}' but found '{read}' on line {CursorLine} column {CursorColumn}.");
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
            return CurrentLine.Length >= CursorColumn + count;
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

        protected object ReadValue(DefinitionValueTypes type) {
            var readValue = "";
            var startLine = CursorLine;
            var startColumn = CursorColumn;
            switch (type) {
                case DefinitionValueTypes.String:
                    Ensure(DefinitionFileMarkers.DoubleQuote);
                    while (!Check(DefinitionFileMarkers.DoubleQuote))
                        readValue += Read();
                    Ensure(DefinitionFileMarkers.DoubleQuote);
                    return readValue;

                case DefinitionValueTypes.S64:
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

                case DefinitionValueTypes.UInt:
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

                case DefinitionValueTypes.Float:
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

                case DefinitionValueTypes.Float3:
                    try {
                        Ensure(DefinitionFileMarkers.OpenBrace);
                        readValue = Read(FloatChars);
                        Ensure(DefinitionFileMarkers.Comma);
                        var floatValue1 = float.Parse(readValue);
                        readValue = Read(FloatChars);
                        Ensure(DefinitionFileMarkers.Comma);
                        var floatValue2 = float.Parse(readValue);
                        readValue = Read(FloatChars);
                        var floatValue3 = float.Parse(readValue);
                        Ensure(DefinitionFileMarkers.CloseBrace);
                        return new Float3(floatValue1, floatValue2, floatValue3);
                    } catch (ArgumentNullException ex) {
                        throw new DefinitionParseException($"Could not read 32 bit floating point from definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (FormatException ex) {
                        throw new DefinitionParseException($"Could not convert value '{readValue}' to a 32 bit floating point in the definition file or stream on line {startLine} column {startColumn}", ex);
                    } catch (OverflowException ex) {
                        throw new DefinitionParseException($"Could value '{readValue}' was too large to be converted into an 32 bit floating point in the definition file or stream on line {startLine} column {startColumn}", ex);
                    }

                case DefinitionValueTypes.Bool:
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
    }
}
