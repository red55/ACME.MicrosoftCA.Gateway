using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }

}