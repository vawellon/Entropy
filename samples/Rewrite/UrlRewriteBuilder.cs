using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRewriteBuilder
    {
        private List<Rule> _rules = new List<Rule>();
        public void RewritePath(string regex, string newPath)
        {
            _rules.Add(new UrlRewriteRule { MatchPattern = new Regex(regex), OnMatch = newPath });
        }
        public void RedirectPath()
        {
            _rules.Add(new UrlRedirectRule { });
        }
        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }
    }
}
