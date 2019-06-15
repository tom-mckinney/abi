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
    public class VisitorRepository : YesSqlRepository<Visitor>, IVisitorRepository
    {
        public VisitorRepository(ISession session) : base(session)
        {
        }

        public async Task<Visitor> CreateAsync()
        {
            var visitor = new Visitor
            {
                VisitorId = Guid.NewGuid().ToString("n")
            };

            _session.Save(visitor);
            await _session.CommitAsync();

            return visitor;
        }

        public Task<Visitor> GetByPublicIdAsync(string publicId)
        {
            return _session.Query<Visitor, VisitorIndex>(v => v.VisitorId == publicId).FirstOrDefaultAsync();
        }
    }
}
