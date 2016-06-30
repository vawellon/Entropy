using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class Condition
    {

        public List<ConditionsTestStringSegment> TestString { get; set;}
        public Regex CondPattern { get; set; }

        public Condition(string condition)
        {
            TestString = ConditionParser.ParseCondition(condition);
        }
        // eventually a private call
       

        public ConditionContext ApplyCondition(Match ruleVars, ConditionContext previous)
        {
            // based on the type of the condition, apply the condition to the teststring
            // For backreferences, throw if we dont actually have a back reference.
            return null;
        }
        public ConditionFlags Flags { get; set; }

        
    }
}
