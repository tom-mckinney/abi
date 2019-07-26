using Abi.Models;
using Abi.OrchardCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YesSql;

namespace Abi.Test.Data
{
    public class YesSqlRepositoryTests : RepositoryTestBase
    {
        public class Wumbo : AbiEntity
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }

        public class WumboRepository : YesSqlRepository<Wumbo>
        {
            public WumboRepository(ISession session) : base(session)
            {
            }
        }

        [Fact]
        public async Task SaveAsync_will_create_an_entry_for_new_model()
        {
            var wumbo = TestData.Create<Wumbo>();

            using (IStore store = await CreateStore())
            {
                using (var session = store.CreateSession())
                {
                    var repository = new WumboRepository(session);

                    await repository.SaveAsync(wumbo);
                }

                using (var session = store.CreateSession())
                {
                    Assert.True(wumbo.Id > 0);
                    var newWumbo = Assert.Single(await session.Query<Wumbo>().ListAsync());
                    CustomAssert.AllPropertiesMapped(wumbo, newWumbo);
                }
            }
        }

        [Fact]
        public async Task SaveAsync_will_update_existing_entry()
        {
            var existingWumbo = TestData.Create<Wumbo>();
            Wumbo wumbo;

            using (IStore store = await CreateStore())
            {
                using (var previousSession = store.CreateSession())
                {
                    previousSession.Save(existingWumbo);
                }

                using (var session = store.CreateSession())
                {
                    wumbo = new Wumbo
                    {
                        Id = existingWumbo.Id,
                        Foo = "So much Wumbo",
                        Bar = 123456,
                        CreatedUtc = existingWumbo.CreatedUtc,
                        ModifiedUtc = existingWumbo.ModifiedUtc
                    };

                    var repository = new WumboRepository(session);

                    await repository.SaveAsync(wumbo);
                }

                using (var session = store.CreateSession())
                {
                    Assert.True(wumbo.Id > 0);
                    Assert.Equal(existingWumbo.Id, wumbo.Id);
                    var updatedWumbo = Assert.Single(await session.Query<Wumbo>().ListAsync());
                    CustomAssert.AllPropertiesMapped(wumbo, updatedWumbo);

                    Assert.NotEqual(existingWumbo.Foo, updatedWumbo.Foo);
                    Assert.NotEqual(existingWumbo.Bar, updatedWumbo.Bar);
                }
            }
        }

        [Fact]
        public async Task GetAllAsync_returns_all_entries()
        {
            var existingWumbo1 = TestData.Create<Wumbo>();
            var existingWumbo2 = TestData.Create<Wumbo>();
            var existingWumbo3 = TestData.Create<Wumbo>();

            using (IStore store = await CreateStore())
            {
                using (var previousSession = store.CreateSession())
                {
                    previousSession.Save(existingWumbo1);
                    previousSession.Save(existingWumbo2);
                    previousSession.Save(existingWumbo3);
                }

                using (var session = store.CreateSession())
                {
                    var repository = new WumboRepository(session);

                    var allWumbos = await repository.GetAllAsync();

                    CustomAssert.AllPropertiesMapped(existingWumbo1, allWumbos.First(w => w.Id == existingWumbo1.Id));
                    CustomAssert.AllPropertiesMapped(existingWumbo2, allWumbos.First(w => w.Id == existingWumbo2.Id));
                    CustomAssert.AllPropertiesMapped(existingWumbo3, allWumbos.First(w => w.Id == existingWumbo3.Id));
                }
            }
        }

        [Fact]
        public async Task GetAllAsync_returns_empty_list_if_no_entries_exist()
        {
            using (IStore store = await CreateStore())
            {
                using (var session = store.CreateSession())
                {
                    var repository = new WumboRepository(session);

                    var allWumbos = await repository.GetAllAsync();

                    Assert.Empty(allWumbos);
                    Assert.NotNull(allWumbos);
                    Assert.IsType<Wumbo[]>(allWumbos);
                }
            }
        }

        [Fact]
        public async Task GetAsync_returns_entry_with_corresponding_id()
        {
            var existingWumbo = TestData.Create<Wumbo>();

            using (IStore store = await CreateStore())
            {
                using (var previousSession = store.CreateSession())
                {
                    previousSession.Save(existingWumbo);
                }

                using (var session = store.CreateSession())
                {
                    var repository = new WumboRepository(session);

                    var wumbo = await repository.GetAsync(existingWumbo.Id);

                    CustomAssert.AllPropertiesMapped(existingWumbo, wumbo);
                }
            }
        }

        [Fact]
        public async Task GetAsync_returns_null_if_id_does_not_exist()
        {
            using (IStore store = await CreateStore())
            {
                using (var session = store.CreateSession())
                {
                    var repository = new WumboRepository(session);

                    var wumbo = await repository.GetAsync(1);

                    Assert.Null(wumbo);
                }
            }
        }
    }
}
