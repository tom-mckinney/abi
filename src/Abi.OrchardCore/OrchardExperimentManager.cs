using Abi.Services;
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
        private readonly ICookieService _cookieService;
        private readonly ContentBalancer _contentBalancer;

        public OrchardExperimentManager(ICookieService cookieService, ContentBalancer contentBalancer)
        {
            _cookieService = cookieService;
            _contentBalancer = contentBalancer;
        }

        public Task<BagPart> GetOrSetVariantAsync(BagPart content)
        {
            string experimentId = content.ContentItem.ContentItemId;

            if (!_cookieService.TryGetExperimentCookie(experimentId, out string variantContentId)
                || !content.ContentItems.Any(c => c.ContentItemId == variantContentId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.ContentItems); // TODO: make this personalized/influenced by history
                variantContentId = content.ContentItems.ElementAt(variantIndex).ContentItemId;
            }

            _cookieService.AddExperimentCookie(experimentId, variantContentId);

            content.ContentItems.RemoveAll(c => c.ContentItemId != variantContentId);

            return Task.FromResult(content);
        }
    }
}
