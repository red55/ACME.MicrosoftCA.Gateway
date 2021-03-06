﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Models.API
{
    [Serializable]
    public class DirectoryMeta
    {
        [JsonProperty("terms-of-service")]
        public Uri TermsOfService { get; set; }

        [JsonProperty("website")]
        public Uri Website { get; set; }

        [JsonProperty("caa-identities")]
        public string [] CaaIdentities { get; set; }
    }

    [Serializable]
    public class Directory
    {
        [JsonProperty(@"new-nonce")]
        public Uri NewNonce { get; set; }
        [JsonProperty(@"new-reg")]
        public Uri NewRegistration { get; set; }
        [JsonProperty(@"recover-reg")]
        public Uri RecoverRegistration { get; set; }
        [JsonProperty(@"new-cert")]
        public Uri NewCertificate { get; set; }
        [JsonProperty(@"new-authz")]
        public Uri NewAuthz { get; set; }
        [JsonProperty(@"revoke-cert")]
        public Uri RevokeCert { get; set; }
        [JsonProperty(@"key-change")]
        public Uri KeyChange { get; set; }

        [JsonProperty(@"meta")]
        public DirectoryMeta Meta { get; set; }

        public static Directory Default(Uri myUri) =>
            new Directory
            {
                NewNonce = new Uri(myUri, @"acme/v1/new-nonce"),
                NewRegistration = new Uri(myUri, @"acme/v1/new-account"),
                NewCertificate = new Uri(myUri, @"acme/v1/new-order"),
                NewAuthz = new Uri(myUri, @"acme/v1/new-authz"),
                RevokeCert = new Uri(myUri, @"acme/v1/revoke-cert"),
                KeyChange = new Uri(myUri, @"acme/v1/key-change"),
                Meta = new DirectoryMeta
                {
                    TermsOfService = new Uri(myUri, @"tos"),
                    Website = (new UriBuilder(schemeName: @"http", hostName:"csi.group")).Uri
                }
            };
    }
}
