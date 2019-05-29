using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public interface IExperimentManager : IExperimentManager<BagPart>
    {
    }

    public class OrchardExperimentManager : IExperimentManager
    {
        private readonly HttpContext _httpContext;
        private readonly ContentBalancer _contentBalancer;

        public OrchardExperimentManager(IHttpContextAccessor httpContextAccessor, ContentBalancer contentBalancer)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _contentBalancer = contentBalancer;
        }

        public int GetVariantIndex(BagPart content)
        {
            int variantIndex = -1;
            string cookieName = $"abi_{content.ContentItem.ContentItemId}";

            if (_httpContext.Request.Cookies.TryGetValue(cookieName, out string cookieValue))
            {
                variantIndex = int.Parse(cookieValue);
            }
            else
            {
                variantIndex = _contentBalancer.GetRandomIndex(content.ContentItems);
            }

            cookieValue = variantIndex.ToString();

            _httpContext.Response.Cookies.Append(cookieName, cookieValue);

            return variantIndex;
        }
    }
}
