using Abi.Data.Abstractions;
using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IVariantRepository : IRepository<Variant, int>
    {
        Task<Variant> GetByPublicIdAsync(string variantId);
        Task<Variant> CreateAsync(string experimentId, string contentItemId, string contentItemType = null);
        Task UpdateAsync(Variant variant);
    }
}
