﻿using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YesSql.Indexes;

namespace Abi.OrchardCore.Data.Indexes
{
    public class VariantIndex : MapIndex
    {
        public string VariantId { get; set; }

        public string ExperimentId { get; set; }

        public string ContentItemid { get; set; }

        public string ContentItemType { get; set; }
    }

    public class VariantIndexProvider : IndexProvider<Variant>
    {
        public override void Describe(DescribeContext<Variant> context)
        {
            context.For<VariantIndex>().Map(v => new VariantIndex
            {
                VariantId = v.VariantId,
                ExperimentId = v.ExperimentId,
                ContentItemid = v.ContentItemId,
                ContentItemType = v.ContentItemType,
            });
        }
    }
}
