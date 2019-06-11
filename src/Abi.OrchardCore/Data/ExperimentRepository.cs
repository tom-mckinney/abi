using Abi.Data;
using Abi.Models;
using OrchardCore;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Data
{
    public interface IExperimentRepository : IRepository<ContentItem, ContentItem, int>
    {
        Task<ContentItem> GetByContentItemIdAsync(string id);
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

        public Task<ContentItem> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentItem> GetByContentItemIdAsync(string id)
        {
            return _helper.GetContentItemByIdAsync(id);
        }
    }
}
