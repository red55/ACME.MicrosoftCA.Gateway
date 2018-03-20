using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Controllers
{
    public class APIController : Controller
    {
        protected Config Settings
        {
            get;
            private set;
        }

        public APIController(IOptionsSnapshot<Config> cfg) =>
            Settings = cfg.Value;


#if DEBUG
        public APIController(IOptionsSnapshot<Config> cfg, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            Settings = cfg.Value;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

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