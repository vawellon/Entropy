using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRedirectScheme : RedirectRule
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
    public class UrlRedirectPath : RedirectRule
    {
        public Regex MatchPattern { get; set; }
        public string OnMatch { get; set; }
        public bool StopApplyingRulesOnSuccess { get; set; }
        public override bool ApplyRule(HttpContext context)
        {
            var matches = MatchPattern.Match(context.Request.Path);
            if (matches.Success)
            {
                try
                {
                    context.Request.Path = String.Format(matches.Result(OnMatch));
                    var req = context.Request;
                    var newUrl = string.Concat(
                        "https://",
                        req.PathBase,
                        req.Path,
                        req.QueryString);
                    context.Response.Redirect(newUrl);
                    return true;
                }
                catch (FormatException fe)
                {
                    // TODO handle format exception correctly
                }
                catch (ArgumentNullException ane)
                {
                    // TODO handle argment null exception correctly
                }
            }
            return false;
        }
    }
}
