using Abi.Models;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.Models
{
    public class ExperimentSubjectPart : ContentPart
    {
        public string Name { get; set; }
        public string ExperimentId { get; set; }
    }
}
