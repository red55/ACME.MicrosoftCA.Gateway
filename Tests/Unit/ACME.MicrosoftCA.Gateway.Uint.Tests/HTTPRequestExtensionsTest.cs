using Microsoft.AspNetCore.Http;
using System;
using Xunit;
using ACME.MicrosoftCA.Gateway.Extensions;
using FluentAssertions;

namespace ACME.MicrosoftCA.Gateway.Unit.Tests
{
    public class HTTPRequestExtensionsTest
    {
        static int DefaulPort
        {
            get {
                return 4992;
            }
        }

        static HttpRequest BuildRequest()
        {
            HttpRequest rq = new Fakes.HttpRequest
            {
                Scheme = @"https",
                Host = new HostString($"localhost:{DefaulPort}"),
            };

            return rq;
        }

        [Fact]
        public void X_Forwarded_Host_Is_Correct()
        {
            var correct = new HostString(@"proxy.local");

            var rq = BuildRequest();

            rq.Headers.Add(@"X-Forwarded-Host", "proxy.local");

            var h = rq.HostOverride();

            h.Should().BeEquivalentTo(correct);
        }

        [Fact]
        public void Host_Is_Correct()
        {            
            var rq = BuildRequest();            

            var h = rq.HostOverride();

            h.Should().BeEquivalentTo(rq.Host);
        }

        [Fact]
        public void X_Forwarded_Scheme_Is_Correct()
        {
            var correct = @"https";

            var rq = BuildRequest();

            rq.Headers.Add(@"X-Forwarded-Proto", "https");

            var h = rq.SchemeOverride();

            h.Should().BeSameAs(correct);
        }

        [Fact]
        public void Scheme_Is_Correct()
        {
            var rq = BuildRequest();

            var h = rq.SchemeOverride();

            h.Should().BeSameAs(rq.Scheme);
        }
        [Fact]
        public void X_Forwarded_Port_Is_Correct()
        {
            var correct = 90;
            var rq = BuildRequest();
            rq.Headers.Add(@"X-Forwarded-Port", correct.ToString());
            var p = rq.PortOverride();

            p.Should().
                HaveValue().
                    And.
                Be(correct);            
        }                
        [Fact]
        public void X_Forwarded_Port_Is_Default()
        {            
            var rq = BuildRequest();            
            var p = rq.PortOverride();

            p.Should().
                HaveValue().
                    And.
                Be(DefaulPort);
        }
        [Fact]
        public void X_Forwarded_Port_Is_Null()
        {
            var rq = BuildRequest();

            rq.Host = new HostString("localhost");

            var p = rq.PortOverride();

            p.Should().NotHaveValue();
        }

        [Fact]
        public void Test_My_Url()
        {            
            var rq = BuildRequest();
            var correct = new UriBuilder(scheme: rq.Scheme, host: rq.Host.Host, portNumber: rq.Host.Port.Value);

            var s = rq.MyOrProxyUri();

            s.Should().
                BeEquivalentTo(correct.Uri);
        }

        [Fact]
        public void Test_Proxy_Url()
        {
            var rq = BuildRequest();
            rq.Headers.Add(@"X-Forwarded-Scheme", @"https");
            rq.Headers.Add(@"X-Forwarded-Host", @"proxy.local");
            rq.Headers.Add(@"X-Forwarded-Port", @"90");

            var correct = new UriBuilder(scheme: @"https", host: @"proxy.local", portNumber: 90);

            var s = rq.MyOrProxyUri();

            s.Should().
                BeEquivalentTo(correct.Uri);
        }
    }
}
