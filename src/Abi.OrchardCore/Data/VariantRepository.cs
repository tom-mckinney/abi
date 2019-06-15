using System;
using System.Threading.Tasks;
using Abi.Data;
using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public class VariantRepository : YesSqlRepository<Variant>, IVariantRepository
    {
        public VariantRepository(ISession session) : base(session)
        {
        }

        public async Task<Variant> CreateAsync(string contentItemId)
        {
            var variant = new Variant
            {
                VariantId = Guid.NewGuid().ToString("n"),
                ContentItemId = contentItemId
            };

            _session.Save(variant);
            await _session.CommitAsync();

            return variant;
        }

        public Task UpdateAsync(Variant variant)
        {
            _session.Save(variant);

            return _session.CommitAsync();
        }

        public Task<Variant> GetByPublicIdAsync(string publicId)
        {
            return _session.Query<Variant, VariantIndex>(s => s.VariantId == publicId).FirstOrDefaultAsync();
        }
    }
}
