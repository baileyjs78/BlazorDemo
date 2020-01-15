using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MovieDB.Client.Services;

namespace MovieDB.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(s =>
            {
                var baseUri = s.GetRequiredService<NavigationManager>().BaseUri;

                return new MovieService(baseUri);
            });
            
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
