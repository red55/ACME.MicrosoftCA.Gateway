using System;
using System.Collections.Generic;
using System.Text;
using ACME.MicrosoftCA.Gateway.Configuration;
using ACME.MicrosoftCA.Gateway.Middleware;
using ACME.MicrosoftCA.Gateway.Services;

namespace ACME.MicrosoftCA.Gateway.Integration.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;


    public class Startup
    {
        public static readonly string DB_FILE = "./IntegrationTests.db";
        public static readonly string SQLITE_CONNECTION = $"Data Source={DB_FILE}";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Config>((cfg) =>
            {
                cfg.Database = new DataBaseConfig
                {
                    DatabaseType = DataBaseConfig.DbType.SqlLite,
                    ConnectionString = SQLITE_CONNECTION
                };            
            
            });

            //According to draft-ietf-acme-acme.html 6.1
            services.AddCors((options) =>
            {
                options.AddPolicy(@"AllowAllOrigins", (policy) => policy.AllowAnyOrigin());
            });

            services.AddMvc();

            services.AddDbContext<Services.DataBase>(contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient, optionsAction: (options) => {                
                options.UseSqlite(SQLITE_CONNECTION);                
            });

            services.AddTransient<IReplayNonceRegistry, ReplayNocneRegistry>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CC0091 // Use static method
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

