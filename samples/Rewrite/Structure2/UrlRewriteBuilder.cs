// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;

namespace Rewrite.Structure2
{
    public class UrlRewriteBuilder
    {
        private List<Rule> _rules = new List<Rule>();

        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }
        public void AddRules(List<Rule> rules)
        {
            _rules.AddRange(rules);
        }
        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }
    }
}
