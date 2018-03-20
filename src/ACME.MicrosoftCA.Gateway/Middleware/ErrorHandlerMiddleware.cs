using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next) => _next = next;
#pragma warning disable CC0061 // Async method can be terminating with 'Async' name.
        public async Task Invoke(HttpContext context)
#pragma warning restore CC0061 // Async method can be terminating with 'Async' name.
        {
            try
            {
                if (null != _next)
                {
                    await _next(context);
                }

            }
            catch (Exceptions.ApiException e)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();
                context.Response.ContentType = @"application/problem+json";
                context.Response.StatusCode = (int)e.HttpStatus;

                var problemJson = JsonConvert.SerializeObject(e.Problem);
                await context.Response.WriteAsync(problemJson);
            }
#pragma warning disable CC0003
            catch
            {
                context.Response.StatusCode = 500;
            }
#pragma warning restore CC0003
        }
    }
}