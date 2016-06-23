using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Rewrite
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RewritePath(new PathString("/hello"), new PathString("/hey"), false);
            app.UseRewriter(rewriteBuilder.Build());
            app.Run(context => context.Response.WriteAsync(context.Request.Path));
        }

    }
}
