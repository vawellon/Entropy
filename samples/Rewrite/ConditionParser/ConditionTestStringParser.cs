using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class ConditionTestStringParser
    {
        private const char Percent = '%';
        private const char Money = '$';
        private const char Separator = ' ';
        private const char Colon = ':';
        private const char OpenBrace = '{';
        private const char CloseBrace = '}';
        public static List<ConditionTestStringSegment> ParseConditionTestString(string testString)
        {
            // TODO Create different parsers, regex, condition, etc.
            if (testString == null)
            {
                testString = String.Empty;
            }
            var context = new ConditionParserContext(testString);
            var results = new List<ConditionTestStringSegment>();
            while (context.Next())
            {
                if (context.Current == Percent)
                {
                    // check for server request param
                    if (!context.Next())
                    {
                        throw new ArgumentException();
                    }
                    if (!ParseConditionParameter(context, results))
                    {
                        throw new ArgumentException();
                    }
                    // here store the information necessary to do an easy lookup
                }
                else if (context.Current == Money)
                {
                    context.Next();
                    context.Mark();
                    // variable
                    if (context.Current >= '0' && context.Current <= '9')
                    {
                        context.Next();
                        var ruleVariable = context.Capture();
                        context.Back();
                        results.Add(new ConditionTestStringSegment { Type = TestStringType.RuleParameter, Variable = ruleVariable });
                    } else
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    if (!ParseLiteral(context, results))
                    {
                        throw new ArgumentException();
                    }
                }
            }
            return results;
        }

        // pre: will only be called if this is an unescaped '%' 
        // post: adds an application to the teststring results that will be concatinated on success
        private static bool ParseConditionParameter(ConditionParserContext context, List<ConditionTestStringSegment> results)
        {
            if (context.Current == OpenBrace)
            {
                if (!context.Next())
                {
                    throw new ArgumentException();
                }
                context.Mark();
                while (context.Current != CloseBrace)
                {
                    if (!context.Next())
                    {
                        // reached end of string, its bad no matter what because no condition string
                        throw new ArgumentException();
                    }
                    else if (context.Current == Colon)
                    {
                        // Have a sub group in the server variable, do a lookup based on that.
                        // This means I need to return a dict form the lookup?
                    }
                    // TODO check if character is valid?
                }

                // capture.
                // TODO return result into list of operations.
                var rawServerVariable = context.Capture();
                if (IsValidVariable(context, rawServerVariable))
                {
                    results.Add(new ConditionTestStringSegment { Type = TestStringType.ServerParameter, Variable = rawServerVariable });

                }
                else
                {
                    return false;
                }
            }
            else if (context.Current >= '0' && context.Current <= '9')
            {
                // means we have a segmented lookup
                // store information in the testString result to know what to look up.
                context.Mark();
                context.Next();
                var rawConditionParameter = context.Capture();
                context.Back();
                if (IsValidVariable(context, rawConditionParameter))
                {
                    results.Add(new ConditionTestStringSegment { Type = TestStringType.ConditionParameter, Variable = rawConditionParameter });
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // illegal escape of a character
                throw new ArgumentException();
            }
            return true;
        }

        private static bool ParseLiteral(ConditionParserContext context, List<ConditionTestStringSegment> results)
        {
            context.Mark();
            string encoded;
            while (true)
            {
                if (context.Current == Percent || context.Current == Money)
                {
                    encoded = context.Capture();
                    context.Back();
                    break;
                }
                if (!context.Next())
                {
                    encoded = context.Capture();
                    break;
                }
            }
            if (IsValidLiteral(context, encoded))
            {
                // add results
                results.Add(new ConditionTestStringSegment { Type = TestStringType.Literal, Variable = encoded });
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool IsValidLiteral(ConditionParserContext context, string encoded)
        {
            // TODO
            return true;
        }
        private static bool IsValidVariable(ConditionParserContext context, string encoded)
        {
            return true;
        }
    }
}
