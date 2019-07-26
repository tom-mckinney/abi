using System.Threading.Tasks;

namespace Abi.Data.Abstractions
{
    public interface IDocumentRepositoryBase<TInterface, TModel, TKey>
        where TModel : TInterface
        where TKey : struct
    {
        Task SaveAsync(TModel model);
    }

    public interface IDocumentRepositoryBase<TModel, TKey> : IDocumentRepositoryBase<IEntity<TKey>, TModel, TKey>
        where TModel : IEntity<TKey>
        where TKey : struct
    {
    }
}
