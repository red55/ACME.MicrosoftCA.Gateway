using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Services
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options) { }
        public DbSet<Models.Data.ReplayNonce> IssuedNonces{ get; set; }

        public void MigrateDatabase()
        {
            Database.Migrate();
        }
    }
}
