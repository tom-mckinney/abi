using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.Data.Abstractions
{
    public interface IRepository<TInterface, TModel, TKey>
        where TModel : TInterface
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetAsync(TKey id);
    }

    public interface IRepository<TModel, TKey> : IRepository<IEntity<TKey>, TModel, TKey>
        where TModel : class, IEntity<TKey>
    {
    }
}
