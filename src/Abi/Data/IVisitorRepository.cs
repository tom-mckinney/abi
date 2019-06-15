using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IVisitorRepository : IRepository<Visitor, int>
    {
        Task<Visitor> GetByPublicIdAsync(string publicId);
        Task<Visitor> CreateAsync();
    }
}
