using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class Rule
    {
        public List<Condition> Conditions { get; set; }
        public string Description { get; set; }
        // TODO trash class name
        public InvertExpression InitialRule { get; set; }
        public List<ConditionTestStringSegment> OnMatch { get; set; }
        public List<string> Flags { get; set; }
        public bool ApplyRule(HttpContext context)
        {
            return true;
        }
    }
}
