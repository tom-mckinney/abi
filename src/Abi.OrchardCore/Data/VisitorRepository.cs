using Abi.Data;
using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public class VisitorRepository : SessionRepository<Visitor>, IVisitorRepository
    {
        public VisitorRepository(ISession session) : base(session)
        {
        }

        public Task<Visitor> CreateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Visitor> GetByPublicIdAsync(string publicId)
        {
            return _session.Query<Visitor, VisitorIndex>(v => v.PublicId == publicId).FirstOrDefaultAsync();
        }
    }
}
