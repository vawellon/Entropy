// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.ConditionParser;
using System.Collections.Generic;

namespace Rewrite.Structure2
{
    //public enum PartToInspect { get; set; }
    public class Rule
    {
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public string Description { get; set; } = string.Empty;
        public GeneralExpression InitialRule { get; set; }
        public List<ConditionTestStringSegment> Transforms { get; set; }
        public List<string> Flags { get; set; }
        // TODO [Flag]
        // None, Last, etc.
    }
}
