using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite
{
    public abstract class Rule
    {
        /// <summary>
        /// Apply the rule to the HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>If the rule matched the Http Context</returns>
        public abstract bool ApplyRule(HttpContext context);
    }
}
