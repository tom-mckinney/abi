using Abi.OrchardCore;
using Abi.OrchardCore.Data.Indexes;
using Abi.Test.Stubs;
using Moq;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data;
using OrchardCore.Environment.Shell;
using OrchardCore.Recipes.Services;
using System;
using System.Threading.Tasks;
using YesSql;
using YesSql.Provider.Sqlite;
using YesSql.Sql;

namespace Abi.Test
{
    public abstract class RepositoryTestBase : IDisposable
    {
        protected virtual string TablePrefix => "";

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
            var connectionString = $@"Data Source={_tempFolder.Folder}yessql.db;Cache=Shared";

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

            RunMigration(store, async m => await m.CreateAsync());

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

                    var migrations = new Migrations(new Mock<IContentDefinitionManager>().Object, new Mock<IRecipeMigrator>().Object)
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
            _tempFolder?.Dispose();
        }
    }
}
