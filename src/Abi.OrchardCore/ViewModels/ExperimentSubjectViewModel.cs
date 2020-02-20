using Abi.Models;
using Abi.OrchardCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.ViewModels
{
    public class ExperimentSubjectPartViewModel
    {
        [BindNever]
        public Experiment Experiment { get; set; }
    }
}
