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
    public class EncounterRepository : SessionRepository<Encounter>
    {
        public EncounterRepository(ISession session) : base(session)
        {
        }
    }
}
