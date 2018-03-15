using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Models.API
{
    public class Account
    {
        public string status;
        public string [] contact;
        public bool termsOfServiceAgreed = false;
        public string orders;
    }
}
