using System.Threading.Tasks;

namespace Abi
{
    public partial interface IExperimentManager<TModel> where TModel : class
    {
        Task<TModel> GetOrSetVariantAsync(TModel content);
    }
}
