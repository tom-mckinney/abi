namespace Abi.Data.Abstractions
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
