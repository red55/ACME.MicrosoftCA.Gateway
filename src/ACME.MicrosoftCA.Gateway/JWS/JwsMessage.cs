using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.JWS
{
    [Serializable]
    public class JwsMessage
    {

        [JsonProperty("header")]
        public JwtHeader Header { get; set; }
        //public JsonWeb
        [JsonProperty("payload")]
        public string Payload { get; set; }
        public JwtPayload DecodedPayload
        {
            get
            {
                return JsonConvert.DeserializeObject<JwtPayload>(Base64UrlEncoder.Decode(Payload));
            }
        }
        public bool ShouldSerializeDecodedPayload() => false;
        [JsonProperty("protected")]
        public string Protected { get; set; }
        public JwtPayload DecodedProtected
        {
            get
            {
                return JsonConvert.DeserializeObject<JwtPayload>(Base64UrlEncoder.Decode(Protected));
            }
        }
        public bool ShouldSerializeDecodedProtected() => false;
        [JsonProperty("signature")]
        public string Signature { get; set; }

        public JwtSecurityToken ToJwtToken()
        {
            var t = new JwtSecurityToken(Header, DecodedPayload, Protected, Payload, Signature);

            //if Header.kid not null -> lookup serialized key in datastore;

            return t;
        }

        public static void Test()
        {
            var s = "{\"header\": {\"alg\": \"RS256\",\"jwk\": {\"e\": \"AQAB\",\"kty\": \"RSA\",\"n\": \"sznzfdPq_nSsKIeNwZ9tuWihchpEigUAj7wfdiYak6H13gRNUzxWSHTCe6kYZ_UYRsT7SrFbu8CbFf843Rc0T8fKMhqcCvTlhIEQ84It_pAtQeNpRr8ISaC_gJYIy9smL50P-WN8yG2CWdh0e5jlN4-BMayr8M5duGVadD2aLdfLT-5Mr28RRzJZ0oayUnWQNqF51kcOwTwF_0MUcRO7IFLAP_DvhrZJe-hZO_5qFlYctTGlWDcDCOFMaXeDJE18Z-s5aw5ArbqU1e_zRXpooSllaG3forNikka53yAw2E-xXZdb1VHmX3-Gjk46HioK7SZqtH92VO8G9hOLuRk8gQ\"}},\"protected\": \"eyJub25jZSI6IlppbUROdTFuRlV5X1N2OHFMUWUwV0EifQ\",\"payload\": \"eyJDb250YWN0IjpbIm1haWx0bzppdF9kZXB0QGNzaS5ncm91cCJdLCJBZ3JlZW1lbnQiOm51bGwsIkF1dGhvcml6YXRpb25zIjpudWxsLCJDZXJ0aWZpY2F0ZXMiOm51bGwsIlJlc291cmNlIjoibmV3LXJlZyJ9\",\"signature\": \"rjn1iuvVnTFUngCnYtcEAKnM4pCFyRMCxPnQTmPZRDrbvR_FdlNbzERS1_eWIylENSILddXG7KXL40uEQGm_SeH-7HLdaDSE7YXkVr4QXNuFL-9gK5eGRwzz-rHjHOZT4gLfinePDUDzCY8O9hJ-wbjZt2AZVhxDyj-Tn_AyLxaavxlla-4sCicT0SUN3AYcSr8dL8Mo9wtBJxX1Y_fKrHFx55Hf1WSd1MB2RfBBD-n_h_u-UA_8rt19kCftes_VZianx_kuNcsjOcrY7WRYQngWED8ykPyrns4HtTErnciU1ZLYAng6VACwJNhI_iDHUvLgt-evRlpZKpb7AJzMAw\"}";
            //"{\"header\": {\"alg\": \"RS256\",\"jwk\": {\"E\": \"AQAB\",\"kty\": \"RSA\",\"N\": \"sznzfdPq_nSsKIeNwZ9tuWihchpEigUAj7wfdiYak6H13gRNUzxWSHTCe6kYZ_UYRsT7SrFbu8CbFf843Rc0T8fKMhqcCvTlhIEQ84It_pAtQeNpRr8ISaC_gJYIy9smL50P-WN8yG2CWdh0e5jlN4-BMayr8M5duGVadD2aLdfLT-5Mr28RRzJZ0oayUnWQNqF51kcOwTwF_0MUcRO7IFLAP_DvhrZJe-hZO_5qFlYctTGlWDcDCOFMaXeDJE18Z-s5aw5ArbqU1e_zRXpooSllaG3forNikka53yAw2E-xXZdb1VHmX3-Gjk46HioK7SZqtH92VO8G9hOLuRk8gQ\"}},\"protected\": \"eyJub25jZSI6IlppbUROdTFuRlV5X1N2OHFMUWUwV0EifQ\",\"payload\": \"eyJDb250YWN0IjpbIm1haWx0bzppdF9kZXB0QGNzaS5ncm91cCJdLCJBZ3JlZW1lbnQiOm51bGwsIkF1dGhvcml6YXRpb25zIjpudWxsLCJDZXJ0aWZpY2F0ZXMiOm51bGwsIlJlc291cmNlIjoibmV3LXJlZyJ9\",\"signature\": \"rjn1iuvVnTFUngCnYtcEAKnM4pCFyRMCxPnQTmPZRDrbvR_FdlNbzERS1_eWIylENSILddXG7KXL40uEQGm_SeH-7HLdaDSE7YXkVr4QXNuFL-9gK5eGRwzz-rHjHOZT4gLfinePDUDzCY8O9hJ-wbjZt2AZVhxDyj-Tn_AyLxaavxlla-4sCicT0SUN3AYcSr8dL8Mo9wtBJxX1Y_fKrHFx55Hf1WSd1MB2RfBBD-n_h_u-UA_8rt19kCftes_VZianx_kuNcsjOcrY7WRYQngWED8ykPyrns4HtTErnciU1ZLYAng6VACwJNhI_iDHUvLgt-evRlpZKpb7AJzMAw\"}";


            var acmeToken = JsonConvert.DeserializeObject<JwsMessage>(s);

            var jwsth = new AcmeJwsSecurityTokenHandler();

            var para = new TokenValidationParameters
            {
                /*  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = acmeToken.Header.jwk,
                  CryptoProviderFactory = CryptoProviderFactory.Default
                */
                RequireExpirationTime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                /*
                TokenReader = (token, p) =>
                {
                    var t = JsonConvert.DeserializeObject<JwsMessage>(token);

                    return t.ToJwtToken();
                },
                */
                IssuerSigningKeyResolver = (token, jwtToken, kid, vp) =>
                {
                    var jwk = JsonConvert.SerializeObject((jwtToken as JwtSecurityToken).Header ["jwk"]);
                    return new SecurityKey [] { new JsonWebKey(jwk) };
                }
            };

            var claims = jwsth.ValidateToken(s, para, out var validToken);

        }
    }
}
