using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class UrlRewriteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UrlRewriteOptions _options;
        public UrlRewriteMiddleware(RequestDelegate next, UrlRewriteOptions options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            foreach (Rule rule in _options.Rules)
            {
                // 1. Check if the certain component of the path matches with the initial match.
                // TODO logic for flags.
                // TODO dont initialize the regex here for every request, have it initialized in 
                //      either the parsing on lazily initialize and save here
                Regex compare = new Regex(rule.InitialRule.Expression.Variable);
                Match results = compare.Match(context.Request.Path.ToString());
                // If the match came back negative (the rule xor'd with whether or not it is inverted)
                // go to the next rule.
                if (!(results.Success ^ rule.InitialRule.Invert))
                {
                    continue;
                }
                // 2. Go through all conditions and compare them to the created string
                Match previous = null;
                foreach (Condition condition in rule.Conditions)
                {
                    string concatTestString = CollectTestStringFromContext(condition, context, previous, results);
                    // TODO change invert expression name and make it collapsed into condition expression and normal expression.
                }
            }
            await _next(context);
        }

        private string CollectTestStringFromContext(Condition condition, HttpContext context, Match previous, Match ruleResults)
        {
            String res = String.Empty;
            foreach (ConditionTestStringSegment segment in condition.TestStringSegments)
            {

                // TODO check Groups body and verify it is 1 indexed, else subtract 1?
                // TODO handle case when segment.Variable is 0.
                switch (segment.Type)
                {
                    case TestStringType.Literal:
                        res = String.Concat(res, segment.Variable);
                        break;
                    case TestStringType.ServerParameter:
                        res = String.Concat(res, ServerVariables.LookupServerVariable(context, segment.Variable));
                        break;
                    case TestStringType.RuleParameter:
                        res = String.Concat(res, ruleResults.Groups[segment.Variable]);
                        break;
                    case TestStringType.ConditionParameter:
                        res = String.Concat(res, previous.Groups[segment.Variable]);
                        break;
                }
            }
            return res;
        }
    }
}
