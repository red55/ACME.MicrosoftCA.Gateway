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
    public class JwsMessageHeader : JwtHeader
    {

        JsonWebKey _key;
        public JsonWebKey Key
        {
            get
            {
                if (null == _key)
                {
                    var jwk = JsonConvert.SerializeObject(this ["jwk"]);
                    _key = new JsonWebKey(jwk);
                }

                return _key;
            }
        }

    }
    [Serializable]
    public class JwsMessage
    {
        protected JwtPayload _decodedPayload;
        protected JwtPayload _decodedProtected;

        [JsonProperty("header")]
        public JwsMessageHeader Header { get; set; }
        //public JsonWeb
        [JsonProperty("payload")]
        public string Payload { get; set; }
        public JwtPayload DecodedPayload
        {
            get
            {
                if (null == _decodedPayload)
                {
                    _decodedPayload = JsonConvert.DeserializeObject<JwtPayload>(Base64UrlEncoder.Decode(Payload));
                }

                return _decodedPayload;
            }
        }
        public bool ShouldSerializeDecodedPayload() => false;

        [JsonProperty("protected")]
        public string Protected { get; set; }
        public JwtPayload DecodedProtected
        {
            get
            {
                if (null == _decodedProtected)
                {
                    _decodedProtected = JsonConvert.DeserializeObject<JwtPayload>(Base64UrlEncoder.Decode(Protected));
                }

                return _decodedProtected;
            }
        }
        public bool ShouldSerializeDecodedProtected() => false;
        [JsonProperty("signature")]

        public string Signature { get; set; }

        public JwtSecurityToken ToJwtToken() =>         
            new JwtSecurityToken(Header, DecodedPayload, Protected, Payload, Signature);

        public static void Test()
        {
            const string s = "{\"header\": {\"alg\": \"RS256\",\"jwk\": {\"e\": \"AQAB\",\"kty\": \"RSA\",\"n\": \"sznzfdPq_nSsKIeNwZ9tuWihchpEigUAj7wfdiYak6H13gRNUzxWSHTCe6kYZ_UYRsT7SrFbu8CbFf843Rc0T8fKMhqcCvTlhIEQ84It_pAtQeNpRr8ISaC_gJYIy9smL50P-WN8yG2CWdh0e5jlN4-BMayr8M5duGVadD2aLdfLT-5Mr28RRzJZ0oayUnWQNqF51kcOwTwF_0MUcRO7IFLAP_DvhrZJe-hZO_5qFlYctTGlWDcDCOFMaXeDJE18Z-s5aw5ArbqU1e_zRXpooSllaG3forNikka53yAw2E-xXZdb1VHmX3-Gjk46HioK7SZqtH92VO8G9hOLuRk8gQ\"}},\"protected\": \"eyJub25jZSI6IlppbUROdTFuRlV5X1N2OHFMUWUwV0EifQ\",\"payload\": \"eyJDb250YWN0IjpbIm1haWx0bzppdF9kZXB0QGNzaS5ncm91cCJdLCJBZ3JlZW1lbnQiOm51bGwsIkF1dGhvcml6YXRpb25zIjpudWxsLCJDZXJ0aWZpY2F0ZXMiOm51bGwsIlJlc291cmNlIjoibmV3LXJlZyJ9\",\"signature\": \"rjn1iuvVnTFUngCnYtcEAKnM4pCFyRMCxPnQTmPZRDrbvR_FdlNbzERS1_eWIylENSILddXG7KXL40uEQGm_SeH-7HLdaDSE7YXkVr4QXNuFL-9gK5eGRwzz-rHjHOZT4gLfinePDUDzCY8O9hJ-wbjZt2AZVhxDyj-Tn_AyLxaavxlla-4sCicT0SUN3AYcSr8dL8Mo9wtBJxX1Y_fKrHFx55Hf1WSd1MB2RfBBD-n_h_u-UA_8rt19kCftes_VZianx_kuNcsjOcrY7WRYQngWED8ykPyrns4HtTErnciU1ZLYAng6VACwJNhI_iDHUvLgt-evRlpZKpb7AJzMAw\"}";
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
                    //TODO: if Header.kid not null -> lookup serialized key in datastore;
                    var jwk = JsonConvert.SerializeObject((jwtToken as JwtSecurityToken).Header ["jwk"]);
                    return new SecurityKey [] { new JsonWebKey(jwk) };
                }
            };

            var claims = jwsth.ValidateToken(s, para, out var validToken);

        }
    }
}
