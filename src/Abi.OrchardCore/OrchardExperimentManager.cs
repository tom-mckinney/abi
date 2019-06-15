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
            ICookieService cookieService,
            ContentBalancer contentBalancer)
            : base(visitorRepository, sessionRepository, cookieService)
        {
            //_visitorRepository = visitorRepository;
            //_sessionRepository = sessionRepository;
            //_cookieService = cookieService;
            _contentBalancer = contentBalancer;
        }

        public async Task<FlowPart> GetOrSetVariantAsync(FlowPart content)
        {
            string zone = "Content";
            string experimentId = content.ContentItem.ContentItemId;

            //if (!await _cookieService.TryGetVisitorCookieAsync(out string visitorId) || await _visitorRepository.GetByPublicIdAsync(visitorId) == null)
            //{
            //    Visitor visitor = await _visitorRepository.CreateAsync();
            //    visitorId = visitor.PublicId;
            //}
            Visitor visitor = await GetOrCreateVisitorAsync();
            _cookieService.AddVisitorCookie(visitor.PublicId);

            //if (!await _cookieService.TryGetSessionCookie(out string sessionId) || await _sessionRepository.GetByPublicIdAsync(sessionId) == null)
            //{
            //    Session session = await _sessionRepository.CreateAsync()
            //}

            Session session = await GetOrCreateSessionAsync(visitor.Id);
            _cookieService.AddSessionCookie(session.PublicId);

            // _cookieService.TryGetSessionCookie

            if (!await _cookieService.TryGetExperimentCookie(zone, experimentId, out string variantContentId) || !content.Widgets.Any(c => c.ContentItemId == variantContentId))
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
