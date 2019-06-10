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

        public Task<FlowPart> GetOrSetVariantAsync(FlowPart content)
        {
            string zone = "Content";
            string experimentId = content.ContentItem.ContentItemId;

            if (!_cookieService.TryGetExperimentCookie(zone, experimentId, out string variantContentId)
                || !content.Widgets.Any(c => c.ContentItemId == variantContentId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.Widgets); // TODO: make this personalized/influenced by history
                variantContentId = content.Widgets.ElementAt(variantIndex).ContentItemId;
            }

            _cookieService.AddExperimentCookie(zone, experimentId, variantContentId);

            content.Widgets.RemoveAll(c => c.ContentItemId != variantContentId);

            return Task.FromResult(content);
        }

        //public Task<WidgetsListPart> GetOrSetVariantAsync(WidgetsListPart content)
        //{
        //    string experimentId = content.ContentItem.ContentItemId;

        //    foreach (var widgetZone in content.Widgets)
        //    {
        //        var zone = widgetZone.Key;
        //        var widgetList = widgetZone.Value;

        //        if (!_cookieService.TryGetExperimentCookie(zone, experimentId, out string variantContentId)
        //            || !widgetList.Any(c => c.ContentItemId == variantContentId))
        //        {
        //            int variantIndex = _contentBalancer.GetRandomIndex(widgetList); // TODO: make this personalized/influenced by history
        //            variantContentId = widgetList.ElementAt(variantIndex).ContentItemId;
        //        }

        //        _cookieService.AddExperimentCookie(zone, experimentId, variantContentId);

        //        widgetList.RemoveAll(c => c.ContentItemId != variantContentId);
        //    }

        //    return Task.FromResult(content);
        //}
    }
}
