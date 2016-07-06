using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class Condition
    {
        public List<ConditionTestStringSegment> TestStringSegments { get; set; }
        public InvertExpression ConditionRegex { get; set; }
        public List<string> Flags { get; set; }

    }
}
