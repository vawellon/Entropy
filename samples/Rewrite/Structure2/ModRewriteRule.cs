using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ModRewriteRule :Rule
    {
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public string Description { get; set; } = string.Empty;
        public GeneralExpression InitialRule { get; set; }
        public Pattern Transforms { get; set; }
        public Flags Flags { get; set; }
        public override void ApplyRule()
        {

        }
    }
}
