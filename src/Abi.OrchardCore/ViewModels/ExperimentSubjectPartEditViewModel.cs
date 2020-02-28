using Abi.Models;
using Abi.OrchardCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.ViewModels
{
    public class ExperimentSubjectPartEditViewModel
    {
        public bool? CreateNewExperiment { get; set; }

        public string ExperimentName { get; set; }

        public string ExperimentId { get; set; }

        [BindNever]
        public Experiment Experiment { get; set; }
    }
}
