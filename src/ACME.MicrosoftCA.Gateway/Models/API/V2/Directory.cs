using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Models.API
{
    [Serializable]
    public class DirectoryMetaV2
    {
        [JsonProperty("terms-of-service")]
        public Uri TermsOfService { get; set; }

        [JsonProperty("website")]
        public Uri Website { get; set; }

        [JsonProperty("caa-identities")]
        public string [] CaaIdentities { get; set; }
    }

    [Serializable]
    public class DirectoryV2
    {
        [JsonProperty(@"newNonce")]
        public Uri NewNonce { get; set; }
        [JsonProperty(@"newAccount")]
        public Uri NewAccount { get; set; }
        [JsonProperty(@"newOrder")]
        public Uri NewOrder { get; set; }
        [JsonProperty(@"newAuthz")]
        public Uri NewAuthz { get; set; }
        [JsonProperty(@"revokeCert")]
        public Uri RevokeCert { get; set; }
        [JsonProperty(@"keyChange")]
        public Uri KeyChange { get; set; }

        [JsonProperty(@"meta")]
        public DirectoryMeta Meta { get; set; }

        public static Directory Default(Uri myUri) =>
            new Directory
            {
                NewNonce = new Uri(myUri, @"acme/v2/new-nonce"),
                NewRegistration = new Uri(myUri, @"acme/v2/new-account"),
                NewCertificate = new Uri(myUri, @"acme/v2/new-order"),
                //newAuthz = new Uri(myUri, @"acme/new-authz"),
                RevokeCert = new Uri(myUri, @"acme/v2/revoke-cert"),
                KeyChange = new Uri(myUri, @"acme/v2/key-change"),
                Meta = new DirectoryMeta
                {
                    TermsOfService = new Uri(myUri, @"tos"),
                    Website = (new UriBuilder(schemeName: @"http", hostName:"csi.group")).Uri
                }
            };
    }
}
