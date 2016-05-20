using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mvc.FormUploadSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets()
                .Build();

            var blobConfiguration = new BlobConfiguration(config["blob-endpoint"], config["blob-saskey"]);
            services.AddSingleton(blobConfiguration);

            services.AddMvc();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMvcWithDefaultRoute();
        }
    }
}
