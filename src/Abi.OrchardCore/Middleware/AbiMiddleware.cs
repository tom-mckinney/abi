using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Middleware
{
    public class AbiMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(Constants.ExperimentHeader, out StringValues experiment))
            {
                var random = new Random().Next(2);

                experiment = random.ToString();
            }

            context.Response.Headers.Add(Constants.ExperimentHeader, experiment);

            await next(context);
        }
    }
}
