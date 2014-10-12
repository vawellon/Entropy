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
            });

            app.UseMvc();
        }
    }
}
