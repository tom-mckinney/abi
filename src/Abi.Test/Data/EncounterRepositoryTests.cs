using Abi.Models;
using Abi.OrchardCore;
using Abi.OrchardCore.Data;
using Abi.OrchardCore.Data.Indexes;
using Moq;
using OrchardCore.ContentManagement.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YesSql;
using YesSql.Sql;

namespace Abi.Test.Data
{
    public class EncounterRepositoryTests : RepositoryTestBase
    {
        [Fact]
        public async Task GetAsync_returns_Encounter_by_id()
        {
            IStore store = await CreateStore();

            var encounter = TestData.Create<Encounter>(); //new Encounter { SessionId = 20, ContentVariantId = "123" };

            using (var session = store.CreateSession())
            {
                session.Save(encounter);

                var repository = new EncounterRepository(session);

                var newEncounter = await repository.GetAsync(encounter.Id);

                Assert.Same(encounter, newEncounter);
            }
        }

        [Fact]
        public async Task GetAllAsync_returns_all_Encounters()
        {
            IStore store = await CreateStore();

            var encounter1 = new Encounter { SessionId = "20", VariantId = "123" };
            var encounter2 = new Encounter { SessionId = "30", VariantId = "321" };

            using (var session = store.CreateSession())
            {
                session.Save(encounter1);
                session.Save(encounter2);

                var repository = new EncounterRepository(session);

                var newEncounter = await repository.GetAllAsync();

                Assert.Equal(2, newEncounter.Count());
                Assert.Same(encounter1, newEncounter.First(e => e.Id == encounter1.Id));
                Assert.Same(encounter2, newEncounter.First(e => e.Id == encounter2.Id));
            }
        }

        [Fact]
        public async Task SaveAsync_will_create_a_new_entry_for_new_model()
        {
            var encounter = TestData.Create<Encounter>();

            IStore store = await CreateStore();

            using (var session = store.CreateSession())
            {
                var repository = new EncounterRepository(session);

                await repository.SaveAsync(encounter);

                Assert.True(encounter.Id > 0);
            }
        }

        protected override Task CreateTables(IStore store)
        {
            store.RegisterIndexes<EncounterIndexProvider>();

            using (var connection = store.Configuration.ConnectionFactory.CreateConnection())
            {
                connection.Open();

                void RunMigration(Action<Migrations> action)
                {
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

                RunMigration(m => m.Create());
                RunMigration(m => m.UpdateFrom1());
                RunMigration(m => m.UpdateFrom2());
                RunMigration(m => m.UpdateFrom3());
                RunMigration(m => m.UpdateFrom4());
                RunMigration(m => m.UpdateFrom5());

                //using (var transaction = connection.BeginTransaction(store.Configuration.IsolationLevel))
                //{
                //    var builder = new SchemaBuilder(store.Configuration, transaction);

                //    var migrations = new Migrations(new Mock<IContentDefinitionManager>().Object)
                //    {
                //        SchemaBuilder = builder
                //    };

                //    migrations.Create();
                //    transaction.Commit();
                //    migrations.UpdateFrom1();
                //    transaction.Commit();
                //    migrations.UpdateFrom2();
                //    transaction.Commit();
                //    migrations.UpdateFrom3();
                //    transaction.Commit();
                //    migrations.UpdateFrom4();
                //    transaction.Commit();
                //    migrations.UpdateFrom5();
                //    transaction.Commit();

                //    //builder.CreateMapIndexTable(nameof(EncounterIndex), table => table
                //    //    .Column<int>(nameof(EncounterIndex.SessionId))
                //    //    .Column<string>(nameof(EncounterIndex.VariantId)));

                //}
            }

            return Task.CompletedTask;
        }
    }
}
