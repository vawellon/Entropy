// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Text;

namespace Rewrite.ConditionParser
{
    // TODO give reasons for all format exceptions
    public static class ConditionActionParser
    {
        private const char Not = '!';
        private const char Dash = '-';
        private const char Less = '<';
        private const char Greater = '>';
        private const char EqualSign = '=';

        // Given a Condition Regex Expression, obtain a ConditionRegexStatement
        public static GeneralExpression ParseActionCondition(string condition)
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
                if (results.Type == ConditionType.PropertyTest)
                {
                    return results;
                }
                context.Next();
            }
            else
            {
                results.Type = ConditionType.Regex; 
            }
            // TODO fix this to do substring?
            context.Mark();
            while (context.Next()) ;
            results.Variable = context.Capture();
            return results;
        }

        public static GeneralExpression ParseAttributeTest(ConditionParserContext context)
        {
            if (context.Current == 'd')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Directory };
            }
            else if (context.Current == 'f')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.RegularFile };
            }
            else if (context.Current == 'F')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.ExistingFile };
            }
            else if (context.Current == 'h' || context.Current == 'L')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.SymbolicLink };
            }
            else if (context.Current == 's')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Size };
            }
            else if (context.Current == 'U')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.ExistingUrl };
            }
            else if (context.Current == 'x')
            {
                return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Executable };
            }
            else if (context.Current == 'e')
            {
                if (!context.Next() || context.Current != 'q')
                {
                    // Illegal statement.
                    throw new FormatException();
                }
                return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Equal };
            }
            else if (context.Current == 'g')
            {
                if (!context.Next())
                {
                    throw new FormatException();
                }
                if (context.Current == 't')
                {
                    return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Greater };
                }
                else if (context.Current == 'e')
                {
                    return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.GreaterEqual };
                }
                else
                {
                    throw new FormatException();
                }
            }
            else if (context.Current == 'l')
            {
                if (!context.Next())
                {
                    return new GeneralExpression { Type = ConditionType.PropertyTest, Operation = OperationType.SymbolicLink };
                }
                if (context.Current == 't')
                {
                    return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.Less };
                }
                else if (context.Current == 'e')
                {
                    return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.LessEqual };
                }
                else
                {
                    throw new FormatException();
                }
            }
            else if (context.Current == 'n')
            {
                if (!context.Next() || context.Current == 'e')
                {
                    throw new FormatException();
                }
                return new GeneralExpression { Type = ConditionType.IntComp, Operation = OperationType.NotEqual };
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}
