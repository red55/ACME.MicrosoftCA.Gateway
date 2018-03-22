using ACME.MicrosoftCA.Gateway.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ACME.MicrosoftCA.Gateway.Controllers.acme.Controllers
{
    [EnableCors(@"AllowAllOrigins")]
    [Route(@"/acme/v1/new-account")]
    public class NewAccount : APIController
    {
        public NewAccount(IOptionsSnapshot<Config> cfg) : base(cfg) { }

        [HttpPost]
        public async Task<JsonResult> Index()
        {
           
            using (var tr = new StreamReader(Request.Body))
            {
                var s = tr.ReadToEnd();
                var payload JwtCore.JsonWebToken.Decode()

                return Json(tp);
            }
        }
    }
}
