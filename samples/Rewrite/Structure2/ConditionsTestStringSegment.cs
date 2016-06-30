using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public enum StringCondtionType
    {
        Literal, ServerVariable, ConditionParameter, RuleParameter
    }
    public class ConditionsTestStringSegment
    {
        public string Variable { get; set; }
        public StringCondtionType Type { get; set; }
    }
}
