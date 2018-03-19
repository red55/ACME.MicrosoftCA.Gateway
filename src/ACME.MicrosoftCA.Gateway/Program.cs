using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ACME.MicrosoftCA.Gateway
{
    public class Program
    {
        public class Options
        {
            [Option('m', "migrate",
            HelpText = "Do a database migration.")]
            public bool Migrate { get; set; }
        }

        static int ExitCode { get; set; }

        public static int Main(string[] args)
        {
            var wh = BuildWebHost(args);
            ExitCode = 0;

            var flagExit = false;
            var programArguments = Parser.Default.ParseArguments<Options>(args);
            programArguments.WithParsed((o) =>
            {
                if (o.Migrate)
                {
                    var db = wh.Services.GetService(typeof(Services.DataBase)) as Services.DataBase;

                    if (db is null)
                    {
                        throw new InvalidCastException("DB is null");
                    }
                    db.MigrateDatabase();
                    flagExit = true;
                }
            }).WithNotParsed((errors) =>
            {
                ExitCode = 1;
                flagExit = true;
            });

            if (flagExit is false)
            {
                wh.Run();
            }

            return ExitCode;
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
