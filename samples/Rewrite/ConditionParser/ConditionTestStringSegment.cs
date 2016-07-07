// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Rewrite.ConditionParser
{
    public enum TestStringType
    {
        Literal,
        ServerParameter,
        ConditionParameter,
        RuleParameter
    }
    public class ConditionTestStringSegment
    {
        // TODO make immutable
        public string Variable { get; set; }
        public TestStringType Type { get; set; }
    }
}
