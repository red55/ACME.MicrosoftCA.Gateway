using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Extensions
{
    public static class HTTPRequest
    {
        public static Microsoft.AspNetCore.Http.HostString HostOverride (this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var xhost = request.Headers [@"X-Forwarded-Host"].FirstOrDefault();

            return string.IsNullOrEmpty(xhost) ? request.Host : new Microsoft.AspNetCore.Http.HostString(xhost);
        }

        public static string SchemeOverride(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var xproto = request.Headers [@"X-Forwarded-Proto"].FirstOrDefault();

            return string.IsNullOrEmpty(xproto) ? request.Scheme : xproto;
        }
        public static int? PortOverride(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var xport = request.Headers [@"X-Forwarded-Port"].FirstOrDefault();

            return null == xport ? request.Host.Port : int.Parse(xport);
        }
        
        public static Uri MyOrProxyUri(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var p = request.PortOverride();
            var ub = new UriBuilder(schemeName: request.SchemeOverride(), hostName: request.HostOverride().Host);
            if (p.HasValue)
            {

                ub.Port = p.Value;
            }
            return ub.Uri;
        }
        
    }
}
