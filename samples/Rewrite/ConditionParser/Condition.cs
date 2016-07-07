// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;

namespace Rewrite.ConditionParser
{
    public class Condition
    {
        public List<ConditionTestStringSegment> TestStringSegments { get; }
        public GeneralExpression ConditionRegex { get; }
        public List<string> Flags { get; }
        public Condition(List<ConditionTestStringSegment> testStringSegments, GeneralExpression conditionRegex, List<string> flags)
        {
            TestStringSegments = testStringSegments;
            ConditionRegex = conditionRegex;
            Flags = flags;
        }
    }
}
