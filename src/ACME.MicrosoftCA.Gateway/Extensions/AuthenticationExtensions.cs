using ACME.MicrosoftCA.Gateway.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Extensions
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddAcmeJws(this AuthenticationBuilder builder)
        =>
            AddAcmeJws(builder, AcmeJwsAuthenticationDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddAcmeJws(this AuthenticationBuilder builder, string authenticationScheme)
            =>
            AddAcmeJws(builder, authenticationScheme, _ => { });

        public static AuthenticationBuilder AddAcmeJws(this AuthenticationBuilder builder, Action<AcmeJwsAuthenticationOptions> configureOptions)
            =>
            AddAcmeJws(builder, AcmeJwsAuthenticationDefaults.AuthenticationScheme, configureOptions);


        public static AuthenticationBuilder AddAcmeJws(this AuthenticationBuilder builder, string authenticationScheme, Action<AcmeJwsAuthenticationOptions> configureOptions)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<AcmeJwsAuthenticationOptions>, AcmeJwsAuthenticationPostConfigureOptions>();
            //builder.Services.AddTransient<IBasicAuthenticationService, TAuthService>();

            return builder.AddScheme<AcmeJwsAuthenticationOptions, AcmeJwsPKAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
