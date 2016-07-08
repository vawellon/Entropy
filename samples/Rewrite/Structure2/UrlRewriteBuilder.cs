// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;

namespace Rewrite.Structure2
{
    public class UrlRewriteBuilder
    {
        private List<ModRewriteRule> _rules = new List<ModRewriteRule>();

        public List<ModRewriteRule> Build()
        {
            return new List<ModRewriteRule>(_rules);
        }
        public void AddRules(List<ModRewriteRule> rules)
        {
            _rules.AddRange(rules);
        }
        public void AddRule(ModRewriteRule rule)
        {
            _rules.Add(rule);
        }
    }
}
