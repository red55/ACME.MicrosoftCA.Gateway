using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Configuration
{
#pragma warning disable S101 // Types should be named in camel case
    public class SSLConfig
    {
        public bool Enabled { get; set; }
        public string StorageLocation { get; set; }
        public string CertificateThumbprint { get; set; }
    }
    public class Config
    {
        public static readonly string SECTION_NAME = @"ACME.MicrosoftCA.Gateway.Config";
        public Uri HostOverride { get; set; }
        public SSLConfig SSL { get; set; }
    }

#pragma warning restore S101 // Types should be named in camel case
}
