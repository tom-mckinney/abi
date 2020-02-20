using Abi.OrchardCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.ViewModels
{
    public class ExperimentSubjectPartViewModel
    {
        public string Title { get; set; }

        [BindNever]
        public ExperimentSubjectPart ExperimentVariantPart { get; set; }
    }
}
