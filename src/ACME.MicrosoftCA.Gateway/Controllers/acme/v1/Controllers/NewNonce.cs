using ACME.MicrosoftCA.Gateway.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Controllers.acme.Controllers
{
    [EnableCors(@"AllowAllOrigins")]
    [Route(@"/acme/v1/new-nonce")]
    public class NewNonce : ACME.MicrosoftCA.Gateway.Controllers.APIController
    {
        public NewNonce(IOptionsSnapshot<Config> cfg) : base(cfg) { }

#if DEBUG
        [HttpGet]
#endif
        [HttpHead]
        public EmptyResult Index()
        {
            Response.Headers.Add("Cache-Control", "no-store");

            return new EmptyResult();
        }
    }
}
