using Abi.Data;
using Abi.Models;
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

        public ExperimentRepository(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<ContentItem>> GetAllAsync()
        {
            var query = _session.Query<ContentItem, ContentItemIndex>();

            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Experiment));

            return await query.ListAsync();
        }
    }
}
