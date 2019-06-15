using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class SessionIndex : MapIndex
    {
        public string SessionId { get; set; }

        public string VisitorId { get; set; }
    }

    public class SessionIndexProvider : IndexProvider<Session>
    {
        public override void Describe(DescribeContext<Session> context)
        {
            context.For<SessionIndex>().Map(session => new SessionIndex
            {
                SessionId = session.SessionId,
                VisitorId = session.VisitorId
            });
        }
    }
}
