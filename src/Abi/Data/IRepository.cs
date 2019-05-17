using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync();
    }
}
