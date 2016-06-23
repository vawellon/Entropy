using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRewriteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UrlRewriteOptions _options;
        public UrlRewriteMiddleware(RequestDelegate next, UrlRewriteOptions options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            foreach (Rule rule in _options.Rules)
            {
                if (rule.ApplyRule(context))
                {
                    if (rule is RedirectRule)
                    {
                        return;
                    } else if (rule is RewriteRule)
                    {
                        await _next(context);
                    }
                }
                // TODO check type of rule and go from there, either check class type or a private bool
                // This will be used for the _next and return logic.
            }
            await _next(context);
        }
    }
}
