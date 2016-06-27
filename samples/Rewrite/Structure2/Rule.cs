using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public enum Transformation
    {
        Rewrite,
        TerminatingRewrite,
        Redirect

    }
    public abstract class Rule
    {
        /// <summary>
        /// Apply the rule to the HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>If the rule matched the Http Context</returns>
        public Transformation RuleState { get; set; }
        public abstract bool ApplyRule(HttpContext context);
        public string Description { get; set; }
    }
}
