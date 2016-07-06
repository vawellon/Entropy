using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class InvertExpression : Expression
    {
        public Expression Expression { get; set; }
        public bool Invert { get; set; }
    }
}
