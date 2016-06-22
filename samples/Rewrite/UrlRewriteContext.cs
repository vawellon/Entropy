using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite
{
    public class UrlRewriteContext
    {
        public HttpContext HttpContext { get; set; }
        public string this[string key] { get { return null; } set { } }
    }
}
