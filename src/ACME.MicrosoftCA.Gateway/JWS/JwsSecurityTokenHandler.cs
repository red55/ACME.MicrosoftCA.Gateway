using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.JWS
{
    public class AcmeJwsSecurityTokenHandler : JwtSecurityTokenHandler
    {
        protected virtual SecurityToken DoReadToken(string token)
        {

            var t = JsonConvert.DeserializeObject<JwsMessage>(token);

            //t.RawData = token;

            return t.ToJwtToken();
        }
        public override SecurityToken ReadToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            if (token.Length > MaximumTokenSizeInBytes)
                throw new ArgumentException($"Token length {token.Length} exceeds maximum allowed ({MaximumTokenSizeInBytes}");

            if (!CanReadToken(token))
                throw new ArgumentException($"JWS is not well formed");

            return DoReadToken(token);
        }
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {

            if (CanReadToken(token))
            {
                validatedToken = ValidateSignature(token, validationParameters);
                return ValidateTokenPayload(validatedToken as JwtSecurityToken, validationParameters);
            }

            return base.ValidateToken(token, validationParameters, out validatedToken);


        }

        public override Type TokenType
        {
            get { return typeof(JwtSecurityToken); }
        }

        public override bool CanReadToken(string token)
        {
            try
            {

                var tt = DoReadToken(token);

                return tt is JwtSecurityToken ? true : false;

            }
            catch
            {
                return false;
            }
        }
    }
}
