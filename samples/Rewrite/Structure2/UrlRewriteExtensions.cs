using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{

    using Predicate = Func<HttpContext, bool>;
    public static class UrlRewriteExtensions
    {
        public static IApplicationBuilder UseRewriter(this IApplicationBuilder app, List<Rule> rules)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            // create the predicate
            var options = new UrlRewriteOptions
            {
                Rules = rules,
            };
            return app.Use(next => new UrlRewriteMiddleware(next, options).Invoke);
        }
    }
}
