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
    public class EncounterRepository : IEncounterRepository
    {
        private readonly ISession _session;

        public EncounterRepository(ISession session)
        {
            _session = session;
        }

        public Task<IEnumerable<Encounter>> GetAllAsync()
        {
            return _session.Query<Encounter>().ListAsync();
        }

        public Task<Encounter> GetAsync(int id)
        {
            return _session.GetAsync<Encounter>(id);
        }

        public Task SaveAsync(Encounter model)
        {
            _session.Save(model);

            return Task.CompletedTask;
        }
    }
}
