using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Authentication
{
    public static class AcmeJwsAuthenticationDefaults
    {
        public static string AuthenticationScheme => @"AcmeJwsScheme";
    }

    public class AcmeJwsAuthenticationOptions : AuthenticationSchemeOptions
    {

    }

    public class AcmeJwsAuthenticationPostConfigureOptions : IPostConfigureOptions<AcmeJwsAuthenticationOptions>
    {
        public void PostConfigure(string name, AcmeJwsAuthenticationOptions options)
        {
            //TODO: validate AcmeJwsAuthenticationOptions here
        }
    }


}
