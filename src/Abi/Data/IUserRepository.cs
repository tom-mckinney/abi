using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User> GetByUserNameAsync(string userName);
    }
}
