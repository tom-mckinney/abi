using Abi.Data;
using Abi.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using OrchardCore.Data;
using OrchardCore.Environment.Shell;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public class EncounterRepository : IEncounterRepository
    {
        private readonly IDbConnectionAccessor _dbAccessor;
        private readonly string _tablePrefix;

        public EncounterRepository(IDbConnectionAccessor dbAccessor, ShellSettings settings)
        {
            _dbAccessor = dbAccessor;
            _tablePrefix = settings["TablePrefix"];
        }

        public async Task<Encounter> CreateAsync(string sessionId, string variantId)
        {
            var encounter = new Encounter
            {
                EncounterId = Guid.NewGuid().ToString("n"),
                SessionId = sessionId,
                VariantId = variantId,
                CreatedUtc = DateTime.UtcNow
            };

            using (var connection = _dbAccessor.CreateConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    encounter.Id = await connection.InsertAsync(encounter, transaction);
                
                    transaction.Commit();
                }
            }

            return encounter;
        }

        public Task<IEnumerable<Encounter>> GetAllAsync()
        {
            using (var connection = _dbAccessor.CreateConnection())
            {
                connection.Open();

                return connection.GetAllAsync<Encounter>();
            }
        }

        public Task<Encounter> GetAsync(int id)
        {
            using (var connection = _dbAccessor.CreateConnection())
            {
                connection.Open();

                return connection.GetAsync<Encounter>(id);
            }
        }

        public Task<Encounter> GetByPublicIdAsync(string encounterId)
        {
            using (var connection = _dbAccessor.CreateConnection())
            {
                connection.Open();

                var dialect = SqlDialectFactory.For(connection);
                var customTable = dialect.QuoteForTableName(_tablePrefix + Constants.CustomTables.Encounters);

                var selectCommand = $"SELECT * FROM {customTable} WHERE {dialect.QuoteForColumnName(nameof(Encounter.EncounterId))} = @EncounterId";

                return connection.QueryFirstOrDefaultAsync<Encounter>(selectCommand, new { EncounterId = encounterId });
            }
        }
    }
}
