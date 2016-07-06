using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.RuleParser
{
    public static class RuleRegexParser
    {
        public static GeneralExpression ParseRuleRegex(string regex)
        {
            if (regex == null || regex == String.Empty)
            {
                throw new FormatException();
            }
            if (regex.StartsWith("!"))
            {
                return new GeneralExpression { Invert = true, Variable = regex.Substring(1) };
            }
            else
            {
                return new GeneralExpression { Invert = false, Variable = regex};
            }
        }
    }
}
