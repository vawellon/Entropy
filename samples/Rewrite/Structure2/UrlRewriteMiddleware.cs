// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Text;
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
            // TODO move this all to a RuleEvaluator class in the rewrite project.
            foreach (ModRewriteRule rule in _options.Rules)
            {
                // 1. Check if the certain component of the path matches with the initial match.
                // TODO logic for flags.
                // TODO dont initialize the regex here for every request, have it initialized in 
                //      either the parsing on lazily initialize and save here
                var results = Regex.Match(context.Request.Path.ToString(), rule.InitialRule.Operand, RegexOptions.None, TimeSpan.FromMilliseconds(1)); // TODO timeout
                // If the match came back negative (the rule xor'd with whether or not it is inverted)
                // go to the next rule.
                if (!(results.Success ^ rule.InitialRule.Invert))
                {
                    continue;
                }
                // 2. Go through all conditions and compare them to the created string
                var previous = Match.Empty;
                var i = 0;
                // TODO consider visitor pattern here.
                for (i = 0; i < rule.Conditions.Count; i++)
                {
                    var condition = rule.Conditions[i];
                    var concatTestString = condition.TestStringSegments.ApplyPattern(context, results, previous);
                    var pass = false;
                    switch (condition.ConditionRegex.Type)
                    {
                        case ConditionType.PropertyTest:
                            pass = CheckFileCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.IntComp:
                            pass = CheckIntCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.StringComp:
                            pass = CheckStringCondition(concatTestString, condition, context);
                            break;
                        case ConditionType.Regex:
                            previous = Regex.Match(concatTestString, condition.ConditionRegex.Operand);
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
                var result = rule.Transforms.ApplyPattern(context, results, previous);
                // for now just replace the path, TODO add flag options here
                context.Request.Path = new PathString(result);
            }
            await _next(context);
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
