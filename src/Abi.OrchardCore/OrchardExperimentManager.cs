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
        private readonly IContentBalancer _contentBalancer;

        public OrchardExperimentManager(
            IVisitorRepository visitorRepository,
            ISessionRepository sessionRepository,
            IVariantRepository variantRepository,
            IEncounterRepository encounterRepository,
            ICookieService cookieService,
            IContentBalancer contentBalancer)
            : base(
                  visitorRepository,
                  sessionRepository,
                  variantRepository,
                  encounterRepository,
                  cookieService)
        {
            _contentBalancer = contentBalancer;
        }

        public async Task<FlowPart> GetOrSetVariantAsync(FlowPart content)
        {
            string zone = "content";
            string experimentId = content.ContentItem.ContentItemId;

            Visitor visitor = await GetOrCreateVisitorAsync();

            Session session = await GetOrCreateSessionAsync(visitor.VisitorId);

            Variant variant = await GetVariantOrDefaultAsync(zone, experimentId);

            variant = await SetVariantAsync(variant, content, zone, experimentId);

            await CreateEncounterAsync(session.SessionId, variant.VariantId);

            content.Widgets.RemoveAll(c => c.ContentItemId != variant.ContentItemId);

            return content;
        }

        public virtual async Task<Variant> SetVariantAsync(Variant variant, FlowPart content, string zone, string experimentId)
        {
            if (variant == null || !content.Widgets.Any(c => c.ContentItemId == variant.ContentItemId))
            {
                int variantIndex = _contentBalancer.GetRandomIndex(content.Widgets); // TODO: make this personalized/influenced by history
                var variantContent = content.Widgets.ElementAt(variantIndex);

                if (variant == null)
                {
                    variant = await _variantRepository.CreateAsync(experimentId, variantContent.ContentItemId, variantContent.ContentType);
                }
                else
                {
                    variant.ContentItemId = variantContent.ContentItemId;
                    variant.ContentItemType = variantContent.ContentType;

                    await _variantRepository.UpdateAsync(variant);
                }
            }

            _cookieService.AddVariantCookie(zone, experimentId, variant.VariantId);

            return variant;
        }
    }
}
