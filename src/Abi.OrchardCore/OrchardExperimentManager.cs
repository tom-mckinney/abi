using Abi.Data;
using Abi.Models;
using Abi.Services;
using OrchardCore.Flows.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Abi.OrchardCore
{
    public interface IExperimentManager : IExperimentManager<FlowPart>
    {
    }

    public class OrchardExperimentManager : ExperimentManagerBase, IExperimentManager
    {
        //private readonly IVisitorRepository _visitorRepository;
        //private readonly ISessionRepository _sessionRepository;
        //private readonly ICookieService _cookieService;
        private readonly ContentBalancer _contentBalancer;

        public OrchardExperimentManager(
            IVisitorRepository visitorRepository,
            ISessionRepository sessionRepository,
            IVariantRepository variantRepository,
            IEncounterRepository encounterRepository,
            ICookieService cookieService,
            ContentBalancer contentBalancer)
            : base(
                  visitorRepository,
                  sessionRepository,
                  variantRepository,
                  encounterRepository,
                  cookieService)
        {
            //_visitorRepository = visitorRepository;
            //_sessionRepository = sessionRepository;
            //_cookieService = cookieService;
            _contentBalancer = contentBalancer;
        }

        public async Task<FlowPart> GetOrSetVariantAsync(FlowPart content)
        {
            string zone = "content";
            string experimentId = content.ContentItem.ContentItemId;

            Visitor visitor = await GetOrCreateVisitorAsync();
            _cookieService.AddVisitorCookie(visitor.VisitorId);

            Session session = await GetOrCreateSessionAsync(visitor.VisitorId);
            _cookieService.AddSessionCookie(session.SessionId);

            Variant variant = await GetVariantAsync(zone, experimentId);

            if (variant == null || !content.Widgets.Any(c => c.ContentItemId == variant.ContentItemId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.Widgets); // TODO: make this personalized/influenced by history
                string variantContentId = content.Widgets.ElementAt(variantIndex).ContentItemId;

                if (variant == null)
                {
                    variant = await _variantRepository.CreateAsync(variantContentId);
                }
                else
                {
                    variant.ContentItemId = variantContentId;
                    await _variantRepository.UpdateAsync(variant);
                }
            }

            _cookieService.AddVariantCookie(zone, experimentId, variant.VariantId);

            await CreateEncounterAsync(session.SessionId, variant.VariantId);

            content.Widgets.RemoveAll(c => c.ContentItemId != variant.ContentItemId);

            return content;
        }
    }
}
