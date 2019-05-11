using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Filters
{
    class AbiFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AbiFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            if (input is NumberValue contentIdValue)
            {
                // TODO
            }
            else if (input is StringValue contentNameValue)
            {
                // TODO
            }

            if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(Constants.ExperimentHeader, out string experimentId))
            {
                return new ValueTask<FluidValue>(new StringValue("No experiment"));
            }

            return new ValueTask<FluidValue>(new StringValue($"Experiment: {experimentId}"));
        }
    }
}
