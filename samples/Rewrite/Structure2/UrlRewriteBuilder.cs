using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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


        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }
    }
}
