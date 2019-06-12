using Abi.Data;
using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Data
{
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(User model)
        {
            throw new NotImplementedException();
        }
    }
}
