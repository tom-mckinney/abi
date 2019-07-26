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
            //IStore store = await CreateStore();
            using (var store = await CreateStore())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                var encounter = TestData.Create<Encounter>(); //new Encounter { SessionId = 20, ContentVariantId = "123" };

                using (var session = store.CreateSession())
                {
                    session.Save(encounter);

                    var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                    var newEncounter = await repository.GetAsync(encounter.Id);

                    Assert.Same(encounter, newEncounter);
            }


            }
        }

        [Fact]
        public async Task GetAllAsync_returns_all_Encounters()
        {
            var encounter1 = new Encounter { SessionId = "20", VariantId = "123" };
            var encounter2 = new Encounter { SessionId = "30", VariantId = "321" };
            
            using (IStore store = await CreateStore())
            using (var session = store.CreateSession())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                session.Save(encounter1);
                session.Save(encounter2);

                //var repository = new EncounterRepository(session);
                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

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
            
            using (IStore store = await CreateStore())
            using (var session = store.CreateSession())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                //var repository = new EncounterRepository(session);
                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                await repository.SaveAsync(encounter);

                Assert.True(encounter.Id > 0);
            }
        }

        [Fact]
        public async Task Create_will_create_a_new_entry_for_new_model()
        {
            //var encounter = TestData.Create<Encounter>();

            using (IStore store = await CreateStore())
            using (var session = store.CreateSession())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                //var repository = new EncounterRepository(session);
                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var encounter = await repository.CreateAsync("123", "456");

                Assert.True(encounter.Id > 0);
            }
        }
    }
}
