using Abi.Data.Abstractions;
using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IEncounterRepository : IRepository<Encounter, int>
    {
        Task<Encounter> GetByPublicIdAsync(string encounterId);

        Task<Encounter> CreateAsync(string sessionId, string variantId);
    }
}
