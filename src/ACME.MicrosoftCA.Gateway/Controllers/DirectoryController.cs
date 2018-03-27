using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using ACME.MicrosoftCA.Gateway.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Controllers
{
#pragma warning disable S101 // Types should be named in camel case

    [EnableCors(@"AllowAllOrigins")]
    [Authorize]
    public class DirectoryController : APIController
    {
        public DirectoryController (IOptionsSnapshot<Config> cfg
#if DEBUG
            , IActionDescriptorCollectionProvider actionDescriptorCollectionProvider
#endif
            ) : base (cfg
#if DEBUG
                , actionDescriptorCollectionProvider
#endif
                )

        {
        }
        
        [AllowAnonymous]
        [Route(@"directory")]
        [HttpGet]
        public JsonResult Index()
        {
            var myUri = Request.MyOrProxyUri();

            return Json(Models.API.Directory.Default(myUri));
        }
    }
}
