using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ACME.MicrosoftCA.Gateway.JWS;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;

namespace ACME.MicrosoftCA.Gateway.Authentication
{
    public class AcmeJwsPKAuthenticationHandler : AuthenticationHandler<AcmeJwsAuthenticationOptions>
    {
        protected readonly ISecurityTokenValidator _tokenValidator;
        protected readonly Services.IReplayNonceRegistry _nonceRegistry;

        public AcmeJwsPKAuthenticationHandler(IOptionsMonitor<AcmeJwsAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            JWS.AcmeJwsSecurityTokenHandler validator,
            Services.IReplayNonceRegistry nonceRegistry) : base(options, logger, encoder, clock)
        {
            _tokenValidator = validator;
            _nonceRegistry = nonceRegistry;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //See https://gist.github.com/elanderson/c50b2107de8ee2ed856353dfed9168a2
            Stream tmpBody;
            string sBody;
            Context.Request.EnableRewind();
            tmpBody = Context.Request.Body;
            try
            {
                var buffer = new byte [Context.Request.ContentLength?? 0];
                var read = await Context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                sBody = Encoding.UTF8.GetString(buffer, 0, read);

                Context.Request.Body.Seek(0, SeekOrigin.Begin);

            }
            finally
            {
                Context.Request.Body = tmpBody;
            }

            var msg = JsonConvert.DeserializeObject<JWS.JwsMessage>(sBody);

            if (msg == null)
            {
                return AuthenticateResult.NoResult();
            }

            var para = new TokenValidationParameters
            {
                ValidateTokenReplay = true,
                RequireExpirationTime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                TokenReader = (token, p) =>
                {
                    var t = JsonConvert.DeserializeObject<JwsMessage>(token);

                    return t.ToJwtToken();
                }
                ,
                IssuerSigningKeyResolver = (token, jwtToken, kid, vp) =>
                {
                    //TODO: if Header.kid not null -> lookup serialized key in datastore;
                    //var jwk = JsonConvert.SerializeObject((jwtToken as JwtSecurityToken).Header ["jwk"]);
                    //return new SecurityKey [] { new JsonWebKey(jwk) };

                    return new SecurityKey [] { msg.Header.Key };
                }
                ,
                TokenReplayValidator = (expirationTime, securityToken, validationParameters) =>
                {
                    var nonce = msg.DecodedProtected.Nonce;

                    //TODO: make check for nonce lifetime
                    var b = _nonceRegistry.VerifyNonceAsync(nonce).Result;
                    _nonceRegistry.SetNonceUsedAsync(nonce).Wait();

                    if (!b)
                    {
                        throw new Exceptions.BadReplayNonceException();
                    }

                    return b;
                }
            };

            //var validator = Context.Features.Get<JWS.AcmeJwsSecurityTokenHandler>();

            var claimsPrincipal = _tokenValidator.ValidateToken(sBody, para, out var validatedToken);

            if ((validatedToken == null) || (claimsPrincipal == null))
            {
                return AuthenticateResult.NoResult();
            }

            var ticket = new AuthenticationTicket(claimsPrincipal, AcmeJwsAuthenticationDefaults.AuthenticationScheme);


            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
        }
        protected virtual async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await base.HandleForbiddenAsync(properties);
        }
    }
}
