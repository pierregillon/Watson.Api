using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;

namespace Watson.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.Authority = "{yourAuthorizationServerAddress}";
                        options.Audience = "{yourAudience}";
                    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var settings = configuration.Get<AppSettings>();
            
            app.UseAuthentication();
            app.UseOwin(x => x.UseNancy(options => options.Bootstrapper = new Bootstrapper(settings)));
        }
    }
}
