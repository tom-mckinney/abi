using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.Models
{
    public class ContentVariantBagPart : BagPart
    {
        [BindNever]
        public string TargetContentId { get; set; }
    }
}
