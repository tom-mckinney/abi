using Abi.Data.Abstractions;
using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface ISessionRepository : IRepository<Session, int>
    {
        Task<Session> GetByPublicIdAsync(string publicId);
        Task<Session> CreateAsync(string visitorId);
    }
}
