using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Variant : AbiEntity
    {
        public string VariantId { get; set; } = null!;

        public string ExperimentId { get; set; } = null!;

        public string ContentItemId { get; set; } = null!;

        public string? ContentItemType { get; set; }
    }
}
