using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRedirectHttp : RedirectRule
    {
        public override bool ApplyRule(HttpContext context)
        {
            // TODO this only does http to https, add more features in the future. 
            if (!context.Request.IsHttps)
            {
                //if (!string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
                //{
                //    context.Response.Redirect(new StatusCodeResult(StatusCodes.Status403Forbidden));
                //}
                //var optionsAccessor = context.RequestServices.GetServices<>();

                var req = context.Request;
                var newUrl = string.Concat(
                    "https://",
                    //host.ToUriComponent(),
                    req.PathBase.ToUriComponent(),
                    req.Path.ToUriComponent(),
                    req.QueryString.ToUriComponent());
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
        public bool StopApplyingRules { get; set; }
        public override bool ApplyRule(HttpContext context)
        {
            var matches = MatchPattern.Match(context.Request.Path);
            if (matches.Success)
            {
                // New method here to translate the outgoing format string to the correct value.
                try
                {
                    context.Request.Path = String.Format(matches.Result(OnMatch));
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
