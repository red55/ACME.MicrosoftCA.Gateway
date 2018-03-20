using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using ACME.MicrosoftCA.Gateway.Middleware;
using ACME.MicrosoftCA.Gateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            var cs = Configuration.GetSection(Config.SECTION_NAME);

            services.Configure<Config>(cs);

            //According to draft-ietf-acme-acme.html 6.1
            services.AddCors((options) =>
            {
                options.AddPolicy(@"AllowAllOrigins", (policy) => policy.AllowAnyOrigin());
            });

            services.AddMvc();

            services.AddDbContext<Services.DataBase>(contextLifetime: ServiceLifetime.Transient, optionsLifetime:ServiceLifetime.Transient, optionsAction: (options) => {
                var cfg = cs.Get<Config>();

                switch (cfg.Database.DatabaseType)
                {
                    case DataBaseConfig.DbType.SqlLite:
                        {
                            options.UseSqlite(cfg.Database.ConnectionString);
                            break;
                        }
                    case DataBaseConfig.DbType.SqlServer:
                        {
                            options.UseSqlServer(cfg.Database.ConnectionString);
                            break;
                        }
                    default:
                        {
                            throw new InvalidCastException($"Unsupported DB type: {cfg.Database.DatabaseType}");
                        }

                }
            });

            services.AddTransient<IReplayNonceRegistry, ReplayNocneRegistry>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CC0091 // Use static method
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CC0091 // Use static method
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMiddleware<ReplayNonceMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMvc();
        }
    }
}
