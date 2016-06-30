using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ConditionContext
    {
        public bool Result { get; set; }
        // A list of captured parameters, is in the results of Regex (TODO only have Result?)
        public Match MatchedParameters { get; set; }
    }
}
