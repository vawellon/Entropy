using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    //public enum PartToInspect { get; set; }
    public class Rule
    {
        public List<Condition> Conditions { get; set; }
        public string Description { get; set; }
        public GeneralExpression InitialRule { get; set; }
        public List<ConditionTestStringSegment> OnMatch { get; set; }
        public List<string> Flags { get; set; }
        // TODO [Flag]
        // None, Last, etc.
    }
}
