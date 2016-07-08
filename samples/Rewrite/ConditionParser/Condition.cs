// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;

namespace Rewrite.ConditionParser
{
    public class Condition
    {
        public Pattern TestStringSegments { get; }
        public GeneralExpression ConditionRegex { get; }
        public Flags Flags { get; }
        public Condition(Pattern testStringSegments, GeneralExpression conditionRegex, Flags flags)
        {
            TestStringSegments = testStringSegments;
            ConditionRegex = conditionRegex;
            Flags = flags;
        }
    }
}
