using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.RuleParser
{
    public static class RuleRegexParser
    {
        public static InvertExpression ParseRuleRegex(string regex)
        {
            if (regex == null || regex == String.Empty)
            {
                throw new FormatException();
            }
            if (regex.StartsWith("!"))
            {
                return new InvertExpression { Invert = true, Expression = new Expression { Variable = regex.Substring(1) } };
            }
            else
            {
                return new InvertExpression { Invert = false, Expression = new Expression { Variable = regex } };
            }
        }
    }
}
