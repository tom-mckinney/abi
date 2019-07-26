using Abi.Data;
using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using System.Security.Claims;

namespace Abi.OrchardCore.Data
{
    public class VisitorRepository : YesSqlRepository<Visitor>, IVisitorRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitorRepository(IHttpContextAccessor httpContextAccessor, YesSql.ISession session) : base(session)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Visitor> CreateAsync(int? userId)
        {
            var visitor = new Visitor
            {
                VisitorId = Guid.NewGuid().ToString("n"),
                UserId = userId
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
