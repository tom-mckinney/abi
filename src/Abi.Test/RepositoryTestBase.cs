using Abi.OrchardCore;
using Abi.OrchardCore.Data.Indexes;
using Abi.Test.Stubs;
using Moq;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data;
using OrchardCore.Environment.Shell;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using YesSql;
using YesSql.Provider.Sqlite;
using YesSql.Sql;

namespace Abi.Test
{
    public abstract class RepositoryTestBase : IDisposable
    {
        protected virtual string TablePrefix => "tp";

        protected TemporaryFolder _tempFolder;

        private IStore _store;
        protected async Task<IStore> CreateStore()
        {
            if (_store == null)
                _store = await StoreFactory.CreateAsync(CreateConfiguration());

            await CreateTables(_store);

            return _store;
        }

        protected IDbConnectionAccessor CreateDbConnectionAccessor(IStore store)
        {
            return new DbConnectionAccessorStub(store);
        }

        protected ShellSettings DefaultShellSettings => new ShellSettings();

        protected virtual IConfiguration CreateConfiguration()
        {
            _tempFolder = new TemporaryFolder();
            var connectionString = @"Data Source=" + _tempFolder.Folder + "yessql.db;Cache=Shared";
            //var connectionString = "Data Source=:memory:";

            return new Configuration()
                .UseSqLite(connectionString)
                .SetTablePrefix(TablePrefix)

                .UseDefaultIdGenerator();
        }

        protected virtual Task CreateTables(IStore store)
        {
            store.RegisterIndexes<VisitorIndexProvider>();
            store.RegisterIndexes<SessionIndexProvider>();
            store.RegisterIndexes<VariantIndexProvider>();
            store.RegisterIndexes<EncounterIndexProvider>();

            RunMigration(store, m => m.Create());
            RunMigration(store, m => m.UpdateFrom1());
            RunMigration(store, m => m.UpdateFrom2());
            RunMigration(store, m => m.UpdateFrom3());
            RunMigration(store, m => m.UpdateFrom4());
            RunMigration(store, m => m.UpdateFrom5());
            RunMigration(store, m => m.UpdateFrom6());

            return Task.CompletedTask;
        }        

        protected void RunMigration(IStore store, Action<Migrations> action)
        {
            using (var connection = store.Configuration.ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(store.Configuration.IsolationLevel))
                {
                    var builder = new SchemaBuilder(store.Configuration, transaction);

                    var migrations = new Migrations(new Mock<IContentDefinitionManager>().Object)
                    {
                        SchemaBuilder = builder
                    };

                    action(migrations);

                    transaction.Commit();
                }
            }
        }

        public void Dispose()
        {
            _store?.Dispose();
        }
    }
}
