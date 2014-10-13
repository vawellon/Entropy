using Microsoft.AspNet.Builder;
using Microsoft.AspNet.WebPages;
using Microsoft.Framework.DependencyInjection;

namespace RazorPre
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UsePerRequestServices(services =>
            {
                services.AddMvc();
                services.AddWebPages();

                // This flag is a reasonable option when using precompilation and
                // deploying to a remote server with kpm --no-source
                services.ConfigureOptions<WebPagesOptions>(o =>
                {
                    o.UpdateRoutesFromPrecompilationAtStartup = true;
                });
            });

            app.UseMvc();
        }
    }
}
