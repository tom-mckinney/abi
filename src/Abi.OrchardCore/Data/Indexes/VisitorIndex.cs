using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class VisitorIndex : MapIndex
    {
        public string PublicId { get; set; }

        public string UserId { get; set; }
    }

    public class VisitorIndexProvider : IndexProvider<Visitor>
    {
        public override void Describe(DescribeContext<Visitor> context)
        {
            context.For<VisitorIndex>().Map(visitor =>
            {
                return new VisitorIndex
                {
                    PublicId = visitor.PublicId,
                    UserId = visitor.UserId
                };
            });
        }
    }
}
