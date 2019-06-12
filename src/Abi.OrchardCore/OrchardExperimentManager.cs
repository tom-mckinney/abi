using Abi.Services;
using OrchardCore.Flows.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Abi.OrchardCore
{
    public interface IExperimentManager : IExperimentManager<FlowPart>
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

        public async Task<FlowPart> GetOrSetVariantAsync(FlowPart content)
        {
            string zone = "Content";
            string experimentId = content.ContentItem.ContentItemId;

            // _cookieService.TryGetVisitorCookie
            // _cookieService.TryGetSessionCookie

            if (!await _cookieService.TryGetExperimentCookie(zone, experimentId, out string variantContentId)
                || !content.Widgets.Any(c => c.ContentItemId == variantContentId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.Widgets); // TODO: make this personalized/influenced by history
                variantContentId = content.Widgets.ElementAt(variantIndex).ContentItemId;
            }

            _cookieService.AddExperimentCookie(zone, experimentId, variantContentId);

            content.Widgets.RemoveAll(c => c.ContentItemId != variantContentId);

            return content;
        }
    }
}
