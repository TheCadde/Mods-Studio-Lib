using System;
using System.Collections.Generic;
using System.Linq;

namespace ModsStudioLib.Utils.Helpers {
    public static class StringHelper {
        public static int FindColumnAlignedPadding(IEnumerable<string> strings, int tabSpacing = 4, int targetPadding = 2) {
            return tabSpacing * (int)Math.Round((strings.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + targetPadding) / (double)tabSpacing);
        }
    }
}
