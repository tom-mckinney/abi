using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.Data.Abstractions
{
    public interface IRepositoryBase<TInterface, TModel, TKey>
        where TModel : TInterface
        where TKey : struct
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetAsync(TKey id);
    }

    public interface IRepositoryBase<TModel, TKey> : IRepositoryBase<IEntity<TKey>, TModel, TKey>
        where TModel : class, IEntity<TKey>
        where TKey : struct
    {
    }
}
