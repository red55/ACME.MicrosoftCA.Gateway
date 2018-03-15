using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Models.API
{
    [Serializable]
    public class Directory
    {
        public Uri newNonce { get; set; }
        public Uri newAccount { get; set; }
        public Uri newOrder { get; set; }
        public Uri newAuthz { get; set; }
        public Uri revokeCert { get; set; }
        public Uri keyChange { get; set; }

    }
}
