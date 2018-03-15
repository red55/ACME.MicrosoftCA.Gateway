using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ACME.MicrosoftCA.Gateway.Controllers
{
#pragma warning disable S101 // Types should be named in camel case

    [Route("[controller]")]

    public class ACMEController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Directory()
        {
            var myHostName = this.HttpContext.Request.Host;

            return new JsonResult(new Models.API.Directory
            {
                newNonce = new Uri($"http://{myHostName}/acme/new-nonce"),
                newAccount = new Uri($"http://{myHostName}/acme/new-account"),
                newOrder = new Uri($"http://{myHostName}/acme/new-order"),
                newAuthz = new Uri($"http://{myHostName}/acme/new-authz"),
                revokeCert = new Uri($"http://{myHostName}/acme/revoke-cert"),
                keyChange = new Uri($"http://{myHostName}/acme/key-change")
            }).ToString();
        }

    }
}
