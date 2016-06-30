using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class HostRule : Rule
    {
        public int? SSLPort { get; set; }
        public override bool ApplyRule(HttpContext context)
        {

            // TODO this only does http to https, add more features in the future. 
            if (!context.Request.IsHttps)
            {
                var host = context.Request.Host;
                if (SSLPort.HasValue && SSLPort.Value > 0)
                {
                    // a specific SSL port is specified
                    host = new HostString(host.Host, SSLPort.Value);
                }
                else
                {
                    // clear the port
                    host = new HostString(host.Host);
                }

                if ((RuleState != Transformation.Redirect))
                {
                    context.Request.Scheme = "https";
                    context.Request.Host = host;
                    return true;
                }

                var req = context.Request;
                var newUrl = string.Concat(
                    "https://",
                    host,
                    req.PathBase,
                    req.Path,
                    req.QueryString);
                context.Response.Redirect(newUrl);
                return true;
            }
            return false;
        }
    }
}
