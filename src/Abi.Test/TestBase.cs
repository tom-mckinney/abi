using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesSql;
using YesSql.Provider.Sqlite;

namespace Abi.Test
{
    public abstract class TestBase : IDisposable
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

        protected abstract Task CreateTables(IStore store);

        protected virtual IConfiguration CreateConfiguration()
        {
            _tempFolder = new TemporaryFolder();
            var connectionString = @"Data Source=" + _tempFolder.Folder + "yessql.db;Cache=Shared";

            return new Configuration()
                .UseSqLite(connectionString)
                .SetTablePrefix(TablePrefix)
                .UseDefaultIdGenerator();
        }

        public void Dispose()
        {
            _store?.Dispose();
        }
    }
}
