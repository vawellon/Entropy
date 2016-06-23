using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntropyTests.RewriteTests
{
    public class RewriteMiddlewareTests
    {
        private static Task Success(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Items["test.PathBase"] = context.Request.PathBase.Value;
            context.Items["test.Path"] = context.Request.Path.Value;
            return Task.FromResult<object>(null);
        }
        private static void UseSuccess(IApplicationBuilder app)
        {
            app.Run(Success);
        }

        [Theory]
        [InlineData("/foo", "", "/foo", "/yes")]
        [InlineData("/foo", "", "/foo/", "/yes")]
        [InlineData("/foo", "/Bar", "/foo", "/yes")]
        [InlineData("/foo", "/Bar", "/foo/cho", "/yes")]
        [InlineData("/foo", "/Bar", "/foo/cho/", "/yes")]
        [InlineData("/foo/cho", "/Bar", "/foo/cho", "/yes")]
        [InlineData("/foo/cho", "/Bar", "/foo/cho/do", "/yes")]
        public void PathMatchFunc_RewriteDone(string matchPath, string basePath, string requestPath, string rewrite)
        {
            HttpContext context = CreateRequest(basePath, requestPath);
            var builder = new ApplicationBuilder(serviceProvider: null);
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RewritePath(matchPath, rewrite, false);
            builder.UseRewriter(rewriteBuilder.Build());
            var app = builder.Build();
            app.Invoke(context).Wait();
            Assert.Equal(rewrite, context.Request.Path);
        }
        [Theory]
        [InlineData(@"/(?<name>\w+)?/(?<id>\w+)?", @"", "/hey/hello", "/${id}/${name}", "/hello/hey")]
        [InlineData(@"/(?<name>\w+)?/(?<id>\w+)?/(?<temp>\w+)?", @"", "/hey/hello/what", "/${temp}/${id}/${name}", "/what/hello/hey")]
        public void PathMatchFunc_RegexRewriteDone(string matchPath, string basePath, string requestPath, string rewrite, string expected)
        {
            HttpContext context = CreateRequest(basePath, requestPath);
            var builder = new ApplicationBuilder(serviceProvider: null);
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RewritePath(matchPath, rewrite, false);
            builder.UseRewriter(rewriteBuilder.Build());
            var app = builder.Build();
            app.Invoke(context).Wait();
            Assert.Equal(expected, context.Request.Path);
        }

        [Theory]
        [InlineData(@"/", "/", "/hey/hello", "https://")]
        public void PathMatchFunc_Redirect(string matchPath, string basePath, string requestPath, string rewrite)
        {
            HttpContext context = CreateRequest(basePath, requestPath);
            context.Request.Scheme = "http";
            var builder = new ApplicationBuilder(serviceProvider: null);
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RedirectHttp();
            builder.UseRewriter(rewriteBuilder.Build());
            var app = builder.Build();
            app.Invoke(context).Wait();
            Assert.True(context.Response.Headers["location"].First().StartsWith("https"));
        }

        private HttpContext CreateRequest(string basePath, string requestPath)
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.PathBase = new PathString(basePath);
            context.Request.Path = new PathString(requestPath);
            return context;
        }
    }
}
