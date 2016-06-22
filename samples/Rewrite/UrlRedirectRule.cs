using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRedirectRule : Rule
    {
        public override bool ApplyRule(HttpContext context)
        {
            // TODO this only does http to https, add more features in the future. 
            if (!context.Request.IsHttps)
            {
                var req = context.Request;
                var newUrl = "https://" + req.Host + req.PathBase + req.Path + req.QueryString;
                context.Response.Redirect(newUrl);
                return false;
            }
            return true;
        }
    }
}
