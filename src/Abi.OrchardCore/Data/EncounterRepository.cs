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
    public class EncounterRepository : YesSqlRepository<Encounter>, IEncounterRepository
    {
        public EncounterRepository(ISession session) : base(session)
        {
        }

        public async Task<Encounter> CreateAsync(string sessionId, string variantId)
        {
            var encounter = new Encounter
            {
                EncounterId = Guid.NewGuid().ToString("n"),
                SessionId = sessionId,
                VariantId = variantId
            };

            _session.Save(encounter);
            await _session.CommitAsync();

            return encounter;
        }

        public Task<Encounter> GetByPublicIdAsync(string publicId)
        {
            return _session.Query<Encounter, EncounterIndex>(e => e.EncounterId == publicId).FirstOrDefaultAsync();
        }
    }
}
