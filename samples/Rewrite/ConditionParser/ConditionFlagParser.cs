using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class ConditionFlagParser
    {
        public static List<string> TokenizeAndParseFlags(string flags) {
            // simply just split on commas.
            if (!flags.StartsWith("[") || !flags.EndsWith("]")) {
                throw new FormatException();
            }
            string[] tokens = flags.Substring(1, flags.Count() - 2).Split(',');
            // TODO eventually make flags an enum
            return new List<string>(tokens);
        }
    }
}
