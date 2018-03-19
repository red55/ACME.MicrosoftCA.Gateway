using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Models.Data
{
    public class ReplayNonce
    {
        public DateTimeOffset IssedAt { get; set; }

        [Key]
        public string Nonce { get; set; }
    }
}
