using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class EncounterIndex : MapIndex
    {
        public string EncounterId { get; set; }

        public string SessionId { get; set; }

        public string VariantId { get; set; }
    }

    public class EncounterIndexProvider : IndexProvider<Encounter>
    {
        public override void Describe(DescribeContext<Encounter> context)
        {
            context.For<EncounterIndex>().Map(encounter =>
            {
                return new EncounterIndex
                {
                    EncounterId = encounter.EncounterId,
                    SessionId = encounter.SessionId,
                    VariantId = encounter.VariantId
                };
            });
        }
    }
}
