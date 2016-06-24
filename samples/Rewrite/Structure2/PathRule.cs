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
        public Regex MatchPattern { get; set; }
        public string OnMatch { get; set; }

        public override bool ApplyRule(HttpContext context)
        {
            var pathAndQuery = string.Concat(context.Request.Path, context.Request.QueryString);
            var matches = MatchPattern.Match(pathAndQuery);
            if (matches.Success)
            {
                // New method here to translate the outgoing format string to the correct value.
                try
                {
                    if (IsRedirect)
                    {
                        context.Request.Path = string.Format(matches.Result(OnMatch));
                        var req = context.Request;
                        var newUrl = string.Concat(
                            "https://",
                            req.PathBase,
                            req.Path,
                            req.QueryString);
                        context.Response.Redirect(newUrl);
                    }
                    else
                    {
                        context.Request.Path = string.Format(matches.Result(OnMatch));
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
