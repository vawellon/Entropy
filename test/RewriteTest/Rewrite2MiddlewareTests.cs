using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Rewrite.Structure2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntropyTests.Rewrite2Tests
{
    public class RewriteMiddlewareTests
    {

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

        [Fact]
        public void PathMatchFunc_RedirectScheme()
        {
            HttpContext context = CreateRequest("/", "/");
            context.Request.Scheme = "http";
            var builder = new ApplicationBuilder(serviceProvider: null);
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RedirectScheme(30);
            builder.UseRewriter(rewriteBuilder.Build());
            var app = builder.Build();
            app.Invoke(context).Wait();
            Assert.True(context.Response.Headers["location"].First().StartsWith("https"));
        }

        [Theory]
        [InlineData(@"/(?<name>\w+)?\?(\w+)=(?<name2>\d+)", @"", "/hey", "?hello=1", "/${name}/${name2}", "/hey/1")]
        public void PathMatchFunc_MoveQueryParamsToPath(string matchPath, string basePath, string requestPath, string queryString, string rewrite, string expected)
        {
            return;
            //HttpContext context = CreateRequest(basePath, requestPath);
            //context.Request.QueryString = new QueryString(queryString);
            //var builder = new ApplicationBuilder(serviceProvider: null);
            //var rewriteBuilder = new UrlRewriteBuilder();
            //rewriteBuilder.RewritePath(matchPath, rewrite, false);
            //builder.UseRewriter(rewriteBuilder.Build());
            //var app = builder.Build();
            //app.Invoke(context).Wait();
            //Assert.Equal(expected, context.Request.Path);
        }

        [Fact]
        public void PathMatchFunc_RewriteScheme()
        {
            HttpContext context = CreateRequest("/", "/");
            context.Request.Scheme = "http";
            var builder = new ApplicationBuilder(serviceProvider: null);
            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.RewriteScheme();
            builder.UseRewriter(rewriteBuilder.Build());
            var app = builder.Build();
            app.Invoke(context).Wait();
            Assert.True(context.Request.Scheme.Equals("https"));
        }

        private HttpContext CreateRequest(string basePath, string requestPath)
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.PathBase = new PathString(basePath);
            context.Request.Path = new PathString(requestPath);
            context.Request.Host = new HostString("example.com");
            return context;
        }
    }
}
