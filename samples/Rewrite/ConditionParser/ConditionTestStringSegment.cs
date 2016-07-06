using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public enum TestStringType
    {
        Literal, ServerParameter, ConditionParameter, RuleParameter
    }
    public class ConditionTestStringSegment
    {
        public string Variable { get; set; }
        public TestStringType Type { get; set; }
    }
}
