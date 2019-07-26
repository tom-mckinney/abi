using OrchardCore.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using YesSql;

namespace Abi.Test.Stubs
{
    public class DbConnectionAccessorStub : IDbConnectionAccessor
    {
        private readonly IConnectionFactory _connectionFactory;

        public DbConnectionAccessorStub(IStore store)
        {
            _connectionFactory = store.Configuration.ConnectionFactory;
        }

        public DbConnection CreateConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}
