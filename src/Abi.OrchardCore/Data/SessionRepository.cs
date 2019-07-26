using System;
using System.Threading.Tasks;
using Abi.Data;
using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using YesSql;

using AbiSession = Abi.Models.Session;

namespace Abi.OrchardCore.Data
{
    public class SessionRepository : YesSqlRepository<AbiSession>, ISessionRepository
    {
        public SessionRepository(ISession session) : base(session)
        {
        }

        public async Task<AbiSession> CreateAsync(string visitorId)
        {
            var session = new AbiSession
            {
                SessionId = Guid.NewGuid().ToString("n"),
                VisitorId = visitorId
            };

            await SaveAsync(session);

            return session;
        }

        public Task<AbiSession> GetByPublicIdAsync(string publicId)
        {
            return _session.Query<AbiSession, SessionIndex>(s => s.SessionId == publicId).FirstOrDefaultAsync();
        }
    }
}
