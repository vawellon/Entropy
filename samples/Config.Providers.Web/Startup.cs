using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddIniFile("Config.Providers.ini")
            .Build();

        app.Run(async ctx =>
        {
            ctx.Response.ContentType = "text/plain";

            Func<String, String> formatKeyValue = key => "[" + key + "] " + config[key] + "\r\n";
            await ctx.Response.WriteAsync(formatKeyValue("Services:One.Two"));
            await ctx.Response.WriteAsync(formatKeyValue("Services:One.Two:Six"));
            await ctx.Response.WriteAsync(formatKeyValue("Data:DefaultConnection:ConnectionString"));
            await ctx.Response.WriteAsync(formatKeyValue("Data:DefaultConnection:Provider"));
            await ctx.Response.WriteAsync(formatKeyValue("Data:Inventory:ConnectionString"));
            await ctx.Response.WriteAsync(formatKeyValue("Data:Inventory:Provider"));
        });
    }

    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddCommandLine(args).Build();

        var host = new WebHostBuilder()
            .UseKestrel()
            .UseConfiguration(config)
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }

    private static async Task DumpConfig(HttpResponse response, IConfiguration config, string indentation = "")
    {
        foreach (var child in config.GetChildren())
        {
            await response.WriteAsync(indentation + "[" + child.Key + "] " + config[child.Key] + "\r\n");
            await DumpConfig(response, child, indentation + "  ");
        }
    }
}

