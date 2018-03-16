using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using ACME.MicrosoftCA.Gateway.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Controllers
{
#pragma warning disable S101 // Types should be named in camel case

    [EnableCors(@"AllowAllOrigins")]
    public class DirectoryController : APIController
    {
        
        public DirectoryController (IOptionsSnapshot<Config> cfg) : base (cfg)
        { }

        [Route(@"directory")]
        [HttpGet]
        public JsonResult Directory()
        {
            var myUri = Request.MyOrProxyUri();

            return Json(new Models.API.Directory
            {
                newNonce = new Uri($"{myUri}/acme/new-nonce"),
                newAccount = new Uri($"{myUri}/acme/new-account"),
                newOrder = new Uri($"{myUri}/acme/new-order"),
                newAuthz = new Uri($"{myUri}/acme/new-authz"),
                revokeCert = new Uri($"{myUri}/acme/revoke-cert"),
                keyChange = new Uri($"{myUri}/acme/key-change")
            });
        }
        

    }
}
