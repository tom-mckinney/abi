using Abi.OrchardCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.ViewModels
{
    public class ExperimentVariantViewModel
    {
        public string Title { get; set; }

        [BindNever]
        public ExperimentVariantPart ExperimentVariantPart { get; set; }
    }
}
