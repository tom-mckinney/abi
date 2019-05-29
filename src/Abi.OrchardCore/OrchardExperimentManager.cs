using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<BagPart> GetOrSetVariantAsync(BagPart content)
        {
            string cookieName = $"abi_{content.ContentItem.ContentItemId}";

            if (!_httpContext.Request.Cookies.TryGetValue(cookieName, out string variantContentId) || !content.ContentItems.Any(c => c.ContentItemId == variantContentId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.ContentItems); // TODO: make this personalized/influenced by history
                variantContentId = content.ContentItems.ElementAt(variantIndex).ContentItemId;
            }

            _httpContext.Response.Cookies.Append(cookieName, variantContentId);

            content.ContentItems.RemoveAll(c => c.ContentItemId != variantContentId);

            return Task.FromResult(content);
        }
    }
}
