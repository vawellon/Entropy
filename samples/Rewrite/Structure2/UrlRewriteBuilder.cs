using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class UrlRewriteBuilder
    {
        private List<Rule> _rules = new List<Rule>();
        public void RewritePath(string regex, string newPath, bool stopRewriteOnSuccess)
        {
            _rules.Add(new PathRule { MatchPattern = new Regex(regex), OnMatch = newPath, StopApplyingRulesOnSuccess = stopRewriteOnSuccess, IsRedirect = false });
        }
        public void RewriteScheme()
        {
            _rules.Add(new SchemeRule { IsRedirect = false});
        }

        public void RedirectPath(string regex, string newPath, bool stopRewriteOnSuccess)
        {
            _rules.Add(new PathRule { MatchPattern = new Regex(regex), OnMatch = newPath, StopApplyingRulesOnSuccess = stopRewriteOnSuccess, IsRedirect = true });
        }
        public void RedirectScheme(int? sslPort)
        {
            _rules.Add(new SchemeRule { SSLPort = sslPort, IsRedirect = true });
        }

        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }
    }
}
