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
                Regex compare = new Regex(rule.InitialRule.Variable);
                Match results = compare.Match(context.Request.Path.ToString());
                // If the match came back negative (the rule xor'd with whether or not it is inverted)
                // go to the next rule.
                if (!(results.Success ^ rule.InitialRule.Invert))
                {
                    continue;
                }
                // 2. Go through all conditions and compare them to the created string
                Match previous = Match.Empty;
                var i = 0;
                for (i = 0; i < rule.Conditions.Count; i++)
                {
                    Condition condition = rule.Conditions[i];
                    string concatTestString = CollectTestStringFromContext(condition.TestStringSegments, context, previous, results);
                    bool pass = false;
                    switch (condition.ConditionRegex.Type)
                    {
                        case ConditionType.FileTest:
                            pass = CheckFileCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.IntComp:
                            pass = CheckIntCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.StringComp:
                            pass = CheckStringCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.Regex:
                            previous = Regex.Match(concatTestString, condition.ConditionRegex.Variable);
                            pass = previous.Success;
                            break;
                    }
                    if (!(pass ^ condition.ConditionRegex.Invert))
                    {
                        break;
                    }
                }
                if (i < rule.Conditions.Count)
                {
                    continue;
                }
                // at this point, our rule passed, we can now apply the on match function
                string result = CollectTestStringFromContext(rule.OnMatch, context, previous, results);
                // for now just replace the path, TODO add flag options here
                context.Request.Path = new PathString(result);
            }
            await _next(context);
        }

        private string CollectTestStringFromContext(List<ConditionTestStringSegment> testStrings, HttpContext context, Match previous, Match ruleResults)
        {
            String res = String.Empty;
            foreach (ConditionTestStringSegment segment in testStrings)
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

        private bool CheckFileCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }

        private bool CheckIntCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }

        private bool CheckStringCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }
    }
}
