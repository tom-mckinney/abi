using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class EncounterIndex : MapIndex
    {
        public int SessionId { get; set; }

        public string ContentVariantId { get; set; }
    }

    public class EncounterIndexProvider : IndexProvider<Encounter>
    {
        public override void Describe(DescribeContext<Encounter> context)
        {
            context.For<EncounterIndex>().Map(encounter =>
            {
                return new EncounterIndex
                {
                    SessionId = encounter.SessionId,
                    ContentVariantId = encounter.ContentVariantId
                };
            });
        }
    }
}
