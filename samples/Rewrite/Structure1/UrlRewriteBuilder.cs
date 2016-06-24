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
        public void RewritePath(string regex, string newPath, bool stopRewriteOnSuccess)
        {
            _rules.Add(new UrlRewritePath { MatchPattern = new Regex(regex), OnMatch = newPath, StopApplyingRulesOnSuccess = stopRewriteOnSuccess });
        }
        public void RedirectScheme(int? sslPort)
        {
            _rules.Add(new UrlRedirectScheme { SSLPort = sslPort });
        }
        public void RedirectPath(string regex, string newPath, bool stopRewriteOnSuccess)
        {
            _rules.Add(new UrlRedirectPath { MatchPattern = new Regex(regex), OnMatch = newPath, StopApplyingRulesOnSuccess = stopRewriteOnSuccess });
        }
        public void RewriteScheme()
        {
            _rules.Add(new UrlRewriteScheme { });
        }
        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }
    }
}
