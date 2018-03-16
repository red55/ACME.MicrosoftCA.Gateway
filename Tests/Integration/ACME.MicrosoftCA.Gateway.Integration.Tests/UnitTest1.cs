using System;
using Xunit;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit.Abstractions;

namespace ACME.MicrosoftCA.Gateway.Unit.Tests
{
    public class UnitTest1
    {
        TestServer _WebServer;
        public TestServer WebServer { get => _WebServer; set => _WebServer = value; }       

        public UnitTest1 ()
        {
            WebServer = new TestServer(new WebHostBuilder()
                .UseStartup<ACME.MicrosoftCA.Gateway.Startup>());
        }
        
        [Fact]
        public async Task Get_Directory()
        {
            using (var _pk = new RSACryptoServiceProvider())
            {        
                var ac = new Oocx.Acme.AcmeClient(WebServer.CreateClient(), _pk);             
                var dir = await ac.GetDirectoryAsync();

                dir.
                    Should().
                        NotBeNull().
                            And.
                        BeOfType<Oocx.Acme.Protocol.Directory>();
            }
        }
    }
}
