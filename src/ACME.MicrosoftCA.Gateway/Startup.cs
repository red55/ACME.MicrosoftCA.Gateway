using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACME.MicrosoftCA.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //According to draft-ietf-acme-acme.html 6.1
            services.AddCors((options) =>
            {
                options.AddPolicy(@"AllowAllOrigins", (policy) => policy.AllowAnyOrigin());
            });
            services.Configure<Config>(Config.SECTION_NAME, Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use( async (context, next) =>
            {
                context.Response.OnStarting (() =>
                {
                    context.Response.Headers.Add(@"Replay-Nonce", Guid.NewGuid().ToString(@"N"));
                    return Task.FromResult(0);
                });

                if (null != next)
                    await next();
            });
            app.UseMvc();
        }
    }
}
