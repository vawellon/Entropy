// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.ConditionParser;
using System;

namespace Rewrite.RuleParser
{
    public static class RuleRegexParser
    {
        public static GeneralExpression ParseRuleRegex(string regex)
        {
            if (regex == null || regex == String.Empty)
            {
                throw new FormatException();
            }
            if (regex.StartsWith("!"))
            {
                return new GeneralExpression { Invert = true, Variable = regex.Substring(1) };
            }
            else
            {
                return new GeneralExpression { Invert = false, Variable = regex};
            }
        }
    }
}
