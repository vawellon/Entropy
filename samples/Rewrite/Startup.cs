using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using Rewrite.FileParser;
namespace Rewrite.Structure2
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            RewriteConfigurationExtensions.AddRewriteFile("Rewrite.txt");
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            var rewriteBuilder = new UrlRewriteBuilder();

            app.UseRewriter(rewriteBuilder.Build());
            app.Run(context => context.Response.WriteAsync(context.Request.Path));
        }

    }
}
