using Abi.Models;
using Abi.OrchardCore;
using Abi.OrchardCore.Data;
using Abi.OrchardCore.Data.Indexes;
using Dapper;
using Dapper.Contrib.Extensions;
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
            var encounter1 = new Encounter { SessionId = "20", VariantId = "123" };
            var encounter2 = new Encounter { SessionId = "30", VariantId = "321" };

            using (IStore store = await CreateStore())
            {
                int id;

                var dbAccessor = CreateDbConnectionAccessor(store);

                using (var connection = dbAccessor.CreateConnection())
                {
                    connection.Open();

                    await connection.InsertAsync(encounter1);
                    id = await connection.InsertAsync(encounter2);
                }

                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var actualEncounter = await repository.GetAsync(id);

                CustomAssert.AllPropertiesMapped(encounter2, actualEncounter);
            }
        }

            [Fact]
        public async Task GetAsync_returns_null_if_id_does_not_exist()
        {
            using (IStore store = await CreateStore())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var actualEncounter = await repository.GetAsync(1);

                Assert.Null(actualEncounter);
            }
        }

        [Fact]
        public async Task GetByPublicIdAsync_returns_Encounter_by_EncounterId()
        {
            string targetEncounterId = Guid.NewGuid().ToString("n");
            var encounter1 = new Encounter { EncounterId = targetEncounterId, SessionId = "20", VariantId = "123" };
            var encounter2 = new Encounter { EncounterId = Guid.NewGuid().ToString("n"), SessionId = "30", VariantId = "321" };

            using (IStore store = await CreateStore())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                using (var connection = dbAccessor.CreateConnection())
                {
                    connection.Open();

                    await connection.InsertAsync(encounter1);
                    await connection.InsertAsync(encounter2);
                }

                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var actualEncounter = await repository.GetByPublicIdAsync(targetEncounterId);

                CustomAssert.AllPropertiesMapped(encounter1, actualEncounter);
            }
        }

        [Fact]
        public async Task GetAllAsync_returns_all_Encounters()
        {
            var encounter1 = new Encounter { SessionId = "20", VariantId = "123" };
            var encounter2 = new Encounter { SessionId = "30", VariantId = "321" };

            using (IStore store = await CreateStore())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                using (var connection = dbAccessor.CreateConnection())
                {
                    connection.Open();

                    await connection.InsertAsync(encounter1);
                    await connection.InsertAsync(encounter2);
                }

                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var allEncounters = await repository.GetAllAsync();

                Assert.Equal(2, allEncounters.Count());
                CustomAssert.AllPropertiesMapped(encounter1, allEncounters.First(e => e.Id == encounter1.Id));
                CustomAssert.AllPropertiesMapped(encounter2, allEncounters.First(e => e.Id == encounter2.Id));
            }
        }

        [Fact]
        public async Task Create_will_create_an_entry_for_new_model()
        {
            //var encounter = TestData.Create<Encounter>();

            using (IStore store = await CreateStore())
            //using (var session = store.CreateSession())
            {
                var dbAccessor = CreateDbConnectionAccessor(store);

                //var repository = new EncounterRepository(session);
                var repository = new EncounterRepository(dbAccessor, DefaultShellSettings);

                var encounter = await repository.CreateAsync("123", "456");


                using (var connection = dbAccessor.CreateConnection())
                {
                    await connection.OpenAsync();

                    var newEncounter = await connection.QuerySingleAsync<Encounter>("SELECT * FROM Encounters");

                    CustomAssert.AllPropertiesMapped(encounter, newEncounter);
                }
            }
        }
    }
}
