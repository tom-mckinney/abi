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
            if (!context.Request.Cookies.ContainsKey(Constants.ExperimentHeader))
            {
                var random = new Random().Next(2);

                string experiment = random.ToString();

                context.Response.Cookies.Append(Constants.ExperimentHeader, experiment);
            }

            await next(context);
        }
    }
}
