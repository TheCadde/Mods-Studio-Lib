using System.Collections.Generic;
using System.Collections.ObjectModel;

using ModsStudioLib.Annotations;

namespace ModsStudioLib.Definitions.Parsing {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public enum DefinitionFileMarkers {
        MagicMarker,
        IncludeStatement,
        WhiteSpaces,
        DoubleComment,

        Dot,
        Comma,
        Colon,
        SemiColon,
        Equal,
        OpenCurlyBracket,
        CloseCurlyBracket,
        OpenBrace,
        CloseBrace,
        OpenSquareBracket,
        CloseSquareBracket,
        At,
        Hash,
        Slash,
        DoubleQuote,

        SingleComment = Hash,

        Assignment = Equal,

        StructureSeparator = Colon,
        VariableSeparator = StructureSeparator,

        BlockStart = OpenCurlyBracket,
        BlockEnd = CloseCurlyBracket,
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class DefinitionFileConstants {
        public const string StructureTypeChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_?";
        public const string StructurePathChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_?0123456789.";
        public const string VariableNameChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_?";
        public const string IntegerChars = "0123456789";
        public const string FloatChars = "0123456789.";
        public const string SignedIntegerChars = "-0123456789";
        public const string True = "true";
        public const string False = "false";
        public static readonly ReadOnlyDictionary<DefinitionFileMarkers, string> Markers = new ReadOnlyDictionary<DefinitionFileMarkers, string>(
            new Dictionary<DefinitionFileMarkers, string> {
                {DefinitionFileMarkers.MagicMarker, "SiiNunit"},
                {DefinitionFileMarkers.IncludeStatement, "@include"},
                {DefinitionFileMarkers.WhiteSpaces, " \t\r\n"},

                {DefinitionFileMarkers.DoubleComment, "//"},

                {DefinitionFileMarkers.Dot, "."},
                {DefinitionFileMarkers.Comma, ","},
                {DefinitionFileMarkers.Colon, ":"},
                {DefinitionFileMarkers.SemiColon, ";"},
                {DefinitionFileMarkers.Equal, ""},
                {DefinitionFileMarkers.OpenCurlyBracket, "{"},
                {DefinitionFileMarkers.CloseCurlyBracket, "}"},
                {DefinitionFileMarkers.OpenBrace, "("},
                {DefinitionFileMarkers.CloseBrace, ")"},
                {DefinitionFileMarkers.OpenSquareBracket, "["},
                {DefinitionFileMarkers.CloseSquareBracket, "]"},
                {DefinitionFileMarkers.Hash, "#"},
                {DefinitionFileMarkers.Slash, "/"},
                {DefinitionFileMarkers.DoubleQuote, "\""},
            });


        public static string GetMarker(DefinitionFileMarkers marker) {
            return Markers[marker];
        }
    }
}