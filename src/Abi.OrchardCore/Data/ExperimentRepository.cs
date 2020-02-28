using Abi.Data;
using Abi.Data.Abstractions;
using Abi.Models;
using OrchardCore;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Data
{
    public interface IOrchardExperimentRepository : IExperimentRepository<string>
    {

    }

    public interface IExperimentRepository : IRepository<ContentItem, ContentItem, int>
    {
        Task<Experiment> CreateAsync(string name);
        Task<ContentItem> GetByContentItemIdAsync(string id);
    }

    public class ExperimentRepository : IExperimentRepository
    {
        private readonly IContentManager _contentManager;
        private readonly IOrchardHelper _helper;

        public ExperimentRepository(IContentManager contentManager, IOrchardHelper helper)
        {
            _contentManager = contentManager;
            _helper = helper;
        }

        public Task<Experiment> CreateAsync(string name)
        {
            //_contentManager.NewAsync(Constants.Types.Experiment);
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContentItem>> GetAllAsync()
        {
            return _helper.GetRecentContentItemsByContentTypeAsync(Constants.Types.Experiment);
        }

        public Task<ContentItem> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentItem> GetByContentItemIdAsync(string id)
        {
            return _helper.GetContentItemByIdAsync(id);
        }

        public Task SaveAsync(ContentItem model)
        {
            throw new System.NotImplementedException();
        }
    }
}
