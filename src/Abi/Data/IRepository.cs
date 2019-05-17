using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
