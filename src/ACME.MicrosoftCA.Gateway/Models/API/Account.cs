using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace ACME.MicrosoftCA.Gateway.Models.API
{
    public enum AccountStatus
    {
        valid,
        deactivated,
        revoked
    }

    [Serializable]
    public class Account
    {
        
        [JsonConverter(typeof(StringEnumConverter), false)]
        public AccountStatus status;
        public string [] contact;
        public bool termsOfServiceAgreed = false;
        public string orders;
    }
}
