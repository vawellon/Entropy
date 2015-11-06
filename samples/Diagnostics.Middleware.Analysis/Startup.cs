using System;
using System.Diagnostics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;

namespace Diagnostics.Middleware.Analysis
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
            services.AddMiddlewareAnalysis();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory factory)
        {
            // Displays all log levels
            factory.AddConsole(LogLevel.Verbose);

            var diagnosticListener = app.ApplicationServices.GetRequiredService<DiagnosticListener>();
            var listener = new TestDiagnosticListener();
            diagnosticListener.SubscribeWithAdapter(listener);

            app.UseIISPlatformHandler();

            // Anonymous method inline middleware
            app.Use((context, next) =>
            {
                // No-op
                return next();
            });

            // Named via app.UseMiddleware<T>()
            app.UseDefaultFiles();
            app.UseDirectoryBrowser();

            // Low level anonymous method inline middleware, named Diagnostics.Middleware.Analysis.Startup+<>c by default
            app.Use(next =>
            {
                return context =>
                {
                    return next(context);
                };
            });

            // Override the name that would have been specified by app.UseMiddleware<T>()
            app.NameNextMiddleware("RenamedStaticFiles").UseStaticFiles();

            app.Map("/map", subApp =>
            {
                subApp.Run(context =>
                {
                    return context.Response.WriteAsync("Hello World");
                });
            });

            // Note there's always a default 404 middleware at the end of the pipeline.
        }

        public class TestDiagnosticListener
        {
            [DiagnosticName("Microsoft.AspNet.Hosting.MiddlewareStarting")]
            public virtual void OnMiddlewareStarting(HttpContext httpContext, string name)
            {
                Console.WriteLine($"MiddlewareStarting: {name}; {httpContext.Request.Path}");
            }

            [DiagnosticName("Microsoft.AspNet.Hosting.MiddlewareFinished")]
            public virtual void OnMiddlewareFinished(HttpContext httpContext, string name)
            {
                Console.WriteLine($"MiddlewareFinished: {name}; {httpContext.Response.StatusCode}");
            }
        }
    }
}
