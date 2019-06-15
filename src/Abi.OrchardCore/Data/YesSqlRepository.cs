using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abi.Data;
using OrchardCore.ContentManagement;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public abstract class YesSqlRepository<TModel> : IRepository<TModel, int>
        where TModel : class, IEntity<int>
    {
        protected readonly ISession _session;

        public YesSqlRepository(ISession session)
        {
            _session = session;
        }

        public virtual Task<IEnumerable<TModel>> GetAllAsync()
        {
            return _session.Query<TModel>().ListAsync();
        }

        public virtual Task<TModel> GetAsync(int id)
        {
            return _session.GetAsync<TModel>(id);
        }

        public virtual Task SaveAsync(TModel model)
        {
            _session.Save(model);

            return Task.CompletedTask;
        }
    }
}
