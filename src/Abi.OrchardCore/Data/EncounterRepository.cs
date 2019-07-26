using Abi.Data;
using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using Dapper;
using OrchardCore.Data;
using OrchardCore.Environment.Shell;
using System;
using System.Collections.Generic;
using System.Text;
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
                    var dialect = SqlDialectFactory.For(connection);
                    var encountersTable = dialect.QuoteForTableName($"{_tablePrefix}{Constants.CustomTables.Encounters}");

                    var insertCommand = $"INSERT INTO {encountersTable} ({dialect.QuoteForColumnName("EncounterId")}, {dialect.QuoteForColumnName("SessionId")}, {dialect.QuoteForColumnName("VariantId")}, {dialect.QuoteForColumnName("CreatedUtc")}) VALUES (@EncounterId, @SessionId, @VariantId, @CreatedUtc)";

                    await connection.ExecuteAsync(insertCommand, encounter);
                
                    transaction.Commit(); // If an exception occurs the transaction is disposed and rollbacked
                }
            }

            return encounter;
        }

        public Task<IEnumerable<Encounter>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Encounter> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Encounter> GetByPublicIdAsync(string publicId)
        {
            throw new NotImplementedException();
            //using (var connection = _dbAccessor.CreateConnection())
            //using (var transaction = connection.BeginTransaction())
            //{
            //    var dialect = SqlDialectFactory.For(connection);
            //    var customTable = dialect.QuoteForTableName($"{_tablePrefix}CustomTable");

            //    var selectCommand = $"SELECT * FROM {customTable}";

            //    connection.QueryFirstOrDefaultAsync<Encounter>(selectCommand, new {  })
            //    var model = connection.QueryAsync<CustomTable>(selectCommand);

            //    transaction.Commit(); // If an exception occurs the transaction is disposed and rollbacked
            //}
        }

        public Task SaveAsync(Encounter model)
        {
            throw new NotImplementedException();
        }
    }
}
