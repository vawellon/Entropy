using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rewrite.Structure2
{
    public class FunctionalRule : Rule
    {
        public Func<HttpContext, bool> OnApplyRule { get; set; }

        public override bool ApplyRule(HttpContext context) => OnApplyRule(context);
    }
}
