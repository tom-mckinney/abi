using Abi.Data;
using Abi.Models;
using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using System.Collections.Generic;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public interface IExperimentRepository : IExperimentRepository<ContentItem>
    {
    }

    public class ExperimentRepository : IExperimentRepository
    {
        private readonly ISession _session;
        private readonly IOrchardHelper _helper;
        private readonly IContentManager _contentManager;

        public ExperimentRepository(ISession session, IOrchardHelper helper)
        {
            _session = session;
            _helper = helper;
        }

        public Task<IEnumerable<ContentItem>> GetAllAsync()
        {
            var query = _session.Query<ContentItem, ContentItemIndex>();

            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Experiment));

            return query.ListAsync();
        }

        public Task<ContentItem> GetAsync(string id)
        {
            return _helper.GetContentItemByIdAsync(id);
            //return _contentManager.GetAsync("4fattanaaq5mf7bxxn41d3eewh");
            //return _session.Query<ContentItem, ContentItemIndex>(c => c.Id == id)
            //    //.With<ContentItemIndex>(x => x.ContentType == nameof(Experiment))
            //    //.Where(x => x.Id == id)
            //    .FirstOrDefaultAsync();
        }
    }
}
