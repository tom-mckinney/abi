using Abi.Data;
using Abi.Models;
using OrchardCore;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Data
{
    public interface IExperimentRepository : IExperimentRepository<ContentItem>
    {
    }

    public class ExperimentRepository : IExperimentRepository
    {
        private readonly IOrchardHelper _helper;

        public ExperimentRepository(IOrchardHelper helper)
        {
            _helper = helper;
        }

        public Task<IEnumerable<ContentItem>> GetAllAsync()
        {
            return _helper.GetRecentContentItemsByContentTypeAsync(nameof(Experiment));
        }

        public Task<ContentItem> GetAsync(string id)
        {
            return _helper.GetContentItemByIdAsync(id);
        }
    }
}
