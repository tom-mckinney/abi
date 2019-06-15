using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class SessionIndex : MapIndex
    {
        public string PublicId { get; set; }

        public int VisitorId { get; set; }
    }

    public class SessionIndexProvider : IndexProvider<Session>
    {
        public override void Describe(DescribeContext<Session> context)
        {
            context.For<SessionIndex>().Map(session => new SessionIndex
            {
                PublicId = session.PublicId,
                VisitorId = session.VisitorId
            });
        }
    }
}
