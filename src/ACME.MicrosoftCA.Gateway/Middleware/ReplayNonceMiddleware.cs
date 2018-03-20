using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Middleware
{    
    public class ReplayNonceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Services.IReplayNonceRegistry _reg;
        public ReplayNonceMiddleware(RequestDelegate next, Services.IReplayNonceRegistry reg)
        {
            _next = next;
            _reg = reg;
        }

#pragma warning disable CC0061 // Async method can be terminating with 'Async' name.
        public async Task Invoke(HttpContext context)
#pragma warning restore CC0061 // Async method can be terminating with 'Async' name.
        {
            context.Response.OnStarting(async () => {

                var nonce = await _reg.NewNonceAsync();

                context.Response.Headers.Add(@"Replay-Nonce",
                    nonce.Nonce);
            }
            );

            if (null != _next)
            {
                await _next(context);
            }
        }
    }
}
