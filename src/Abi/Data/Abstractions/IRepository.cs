namespace Abi.Data.Abstractions
{
    public interface IRepository<TInterface, TModel, TKey> : IRepositoryBase<TInterface, TModel, TKey>, IDocumentRepositoryBase<TInterface, TModel, TKey>
        where TModel : TInterface
        where TKey : struct
    {
    }

    public interface IRepository<TModel, TKey> : IRepository<IEntity<TKey>, TModel, TKey>
        where TModel : class, IEntity<TKey>
        where TKey : struct
    {
    }
}
