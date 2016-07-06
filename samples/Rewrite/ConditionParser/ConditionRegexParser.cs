using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public static class ConditionRegexParser
    {
        private const char Not = '!';
        private const char Dash = '-';
        private const char Less = '<';
        private const char Greater = '>';
        private const char EqualSign = '=';

        // Given a Condition Regex Expression, obtain a ConditionRegexStatement 
        public static GeneralExpression ParseCondition(string condition)
        {
            if (condition == null)
            {
                condition = String.Empty;
            }
            var context = new ConditionParserContext(condition);
            var results = new GeneralExpression();
            if (!context.Next())
            {
                return null;
            }

            if (context.Current == Not)
            {
                results.Invert = true;
                if (!context.Next())
                {
                    return null;
                }
            }

            if (context.Current == Greater)
            {
                if (!context.Next())
                {
                    throw new FormatException(); 
                }
                if (context.Current == EqualSign)
                {
                    if (!context.Next())
                    {
                        throw new FormatException();
                    }
                    results.Operation = OperationType.GreaterEqual;
                    results.Type = ConditionType.StringComp;
                } 
                else
                {
                    results.Operation = OperationType.Greater;
                    results.Type = ConditionType.StringComp;
                }
            }
            else if (context.Current == Less)
            {
                if (!context.Next())
                {
                    throw new FormatException();
                }
                if (context.Current == EqualSign)
                {
                    if (!context.Next())
                    {
                        throw new FormatException();
                    }
                    results.Operation = OperationType.LessEqual;
                    results.Type = ConditionType.StringComp;
                }
                else
                {
                    results.Operation = OperationType.Less;
                    results.Type = ConditionType.StringComp;
                }
            }
            else if (context.Current == EqualSign)
            { 
                if (!context.Next())
                {
                    throw new FormatException();
                }
                results.Operation = OperationType.Equal;
                results.Type = ConditionType.StringComp;
            }
            else if (context.Current == Dash)
            {
                results = ParseAttributeTest(context);
                if (results.Type == ConditionType.FileTest)
                {
                    return results;
                }
                context.Next();
            }
            else
            {
                results.Type = ConditionType.Regex; 
            }

            context.Mark();
            while (context.Next()) ;
            results.Variable = context.Capture();
            return results;
        }

        public static GeneralExpression ParseAttributeTest(ConditionParserContext context)
        {
            var current = String.Empty;
            while (true)
            {
                if (!context.Next())
                {
                    throw new FormatException();
                }
                current = String.Concat(current, context.Current);
                switch (current)
                {
                    case "eq":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Equal };
                    case "ge":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.GreaterEqual };
                    case "gt":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Greater };
                    case "le":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.LessEqual};
                    case "lt":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Less };
                    case "ne":
                        return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.NotEqual };
                    case "d":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.Directory };
                    case "f":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.RegularFile };
                    case "F":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.ExistingFile };
                    case "h":
                    case "L":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.SymbolicLink };
                    case "s":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.Size };
                    case "U":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.ExistingUrl };
                    case "x":
                        return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.Executable };
                    case "l":
                        if (!context.HasNext())
                        {
                            return new GeneralExpression { Type = ConditionType.FileTest, Operation = OperationType.SymbolicLink };
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
