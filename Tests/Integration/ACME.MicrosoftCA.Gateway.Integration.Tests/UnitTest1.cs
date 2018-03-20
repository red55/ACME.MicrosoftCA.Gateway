using System;
using System.Linq;
using Xunit;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;

namespace ACME.MicrosoftCA.Gateway.Integration.Tests
{
    [Collection("Database collection")]
    public class IntegrationTest1
    {
        TestServer _WebServer;
        public TestServer WebServer { get => _WebServer; set => _WebServer = value; }

        readonly Fixtures.DatabaseFixture _dbf;
        public IntegrationTest1(Fixtures.DatabaseFixture dbf)
        {
            _dbf = dbf;
            WebServer = new TestServer(new WebHostBuilder()
                .UseStartup<ACME.MicrosoftCA.Gateway.Integration.Tests.Fakes.Startup>());
        }

        [Fact]
        public async Task Get_DirectoryAsync()
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

        [Fact]
        public async Task New_Nonce()
        {

            var req = new HttpRequestMessage
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri(uriString: @"/acme/new-nonce", uriKind: UriKind.Relative)
            };

            var c = WebServer.CreateClient();
            var resp = await c.SendAsync(req);


            resp.StatusCode.
                Should().
                    Be(HttpStatusCode.OK);
            resp.Headers.CacheControl.NoStore.
                Should().
                    Be(true);


            resp.Headers.TryGetValues(@"Replay-Nonce", out var vals);

            var nonce = vals.FirstOrDefault();

            nonce.
                Should().
                    NotBeNull().
                        And.
                    BeOfType<string>();

        }
        [Fact]
        public async Task RegisterAsync()
        {
            using (var _pk = new RSACryptoServiceProvider())
            {
                var contact = new string [] { "it_dept@csi.group" };

                var ac = new Oocx.Acme.AcmeClient(WebServer.CreateClient(), _pk);

                var req = new Oocx.Acme.Protocol.NewRegistrationRequest
                {
                    
                    Contact = contact
                };
                var r = await ac.RegisterAsync(req);


                r.Should().
                    NotBeNull().
                        And.
                    BeOfType<Oocx.Acme.Protocol.RegistrationResponse>();


            }
        }
    }
}
