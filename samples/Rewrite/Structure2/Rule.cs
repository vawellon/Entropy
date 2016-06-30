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
        public Transformation RuleState { get; set; }
        public List<Condition> Conditions { get; set; }
        public abstract bool ApplyRule(HttpContext context);
        public string Description { get; set; }
    }
}
