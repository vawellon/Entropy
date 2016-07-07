// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rewrite.ConditionParser
{
    public class ConditionFlagParser
    {
        public static List<string> TokenizeAndParseFlags(string flags) {
            // simply just split on commas.
            if (!flags.StartsWith("[") || !flags.EndsWith("]")) {
                throw new FormatException();
            }
            string[] tokens = flags.Substring(1, flags.Count() - 2).Split(',');
            return new List<string>(tokens);
        }
    }
}
