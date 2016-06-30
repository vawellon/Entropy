using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class PathRule : Rule
    {
        public Regex Pattern { get; set; }
        public string Substitution { get; set; }
        public RuleFlags RuleFlags { get; set; }

        public override bool ApplyRule(HttpContext context)
        {
            var matches = Pattern.Match(context.Request.Path);
            var previous = (ConditionContext) null;
            if (matches.Success)
            {
                // Initial match succeeded, now apply each condition
                try
                {
                    // TODO probably separate this into another method, just call ApplyConditions with the matches.
                    foreach (var condition in Conditions) {
                        previous = condition.ApplyCondition(matches, previous);
                        if (!previous.Result)
                        {
                            return false;
                        }
                    }
                    var path = matches.Result(Substitution);
                    // TODO handle substituting last condition into path.
                    if (RuleState == Transformation.Redirect)
                    {
                        var req = context.Request;
                        var newUrl = string.Concat(
                            req.Scheme,
                            "://",
                            req.PathBase,
                            path,
                            req.QueryString);
                        context.Response.Redirect(newUrl);
                    }
                    else
                    {
                        context.Request.Path = path;
                    }
                    return true;
                }
                catch (FormatException fe)
                {
                    // TODO handle format exception correctly
                }
                catch (ArgumentNullException ane)
                {
                    // TODO handle argment null exception correctly
                }
            }
            return false;
        }
    }
}
