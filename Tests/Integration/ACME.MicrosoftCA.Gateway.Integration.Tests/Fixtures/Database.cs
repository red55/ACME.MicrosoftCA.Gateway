using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;

namespace ACME.MicrosoftCA.Gateway.Integration.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture ()
        {
            var o = new DbContextOptionsBuilder<Services.DataBase>();
            o.UseSqlite(Fakes.Startup.SQLITE_CONNECTION);

            var v = new ACME.MicrosoftCA.Gateway.Services.DataBase(o.Options);
            v.MigrateDatabase();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            System.IO.File.Delete(Fakes.Startup.DB_FILE);
        }
    }
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
