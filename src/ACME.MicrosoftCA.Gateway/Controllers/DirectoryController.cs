using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using ACME.MicrosoftCA.Gateway.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Controllers
{
#pragma warning disable S101 // Types should be named in camel case

    [EnableCors(@"AllowAllOrigins")]
    public class DirectoryController : APIController
    {
        public DirectoryController (IOptionsSnapshot<Config> cfg, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) : base (cfg)
        {
#if DEBUG
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
#endif
        }

        [Route(@"directory")]
        [HttpGet]
        public JsonResult Index()
        {
            var myUri = Request.MyOrProxyUri();

            return Json(new Models.API.Directory
            {
                newNonce = new Uri(myUri, @"acme/new-nonce"),
                newAccount = new Uri(myUri, @"acme/new-account"),
                newOrder = new Uri(myUri, @"acme/new-order"),
                newAuthz = new Uri(myUri, @"acme/new-authz"),
                revokeCert = new Uri(myUri,@"acme/revoke-cert"),
                keyChange = new Uri(myUri, @"acme/key-change")
            });
        }
#if DEBUG
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public class RouteInfo
        {
            public string Template { get; set; }
            public string Name { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Constraint { get; set; }
        }
        [HttpGet]
        [Route(@"routes")]
        public JsonResult Routes()
        {

            var Routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                    .Select(x => new RouteInfo
                    {
                        Action = x.RouteValues ["Action"],
                        Controller = x.RouteValues ["Controller"],
                        Name = x.AttributeRouteInfo?.Name,
                        Template = x.AttributeRouteInfo?.Template,
                        Constraint = x.ActionConstraints == null ? "" : JsonConvert.SerializeObject(x.ActionConstraints)
                    })
                .OrderBy(r => r.Template);


            return Json(Routes);
        }
#endif

    }
}
