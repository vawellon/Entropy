using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRewriteRule : Rule
    {
        public Regex MatchPattern { get; set; }
        public string OnMatch { get; set; }

        public override bool ApplyRule(HttpContext context)
        {
            var matches = MatchPattern.Match(context.Request.Path);
            if (matches.Success)
            {
                // New method here to translate the outgoing format string to the correct value.
                try
                {
                    context.Request.Path = String.Format(matches.Result(OnMatch));
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
            return true;
        }
    }
}
