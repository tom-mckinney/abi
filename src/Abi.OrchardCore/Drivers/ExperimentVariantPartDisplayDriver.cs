using Abi.OrchardCore.Models;
using Abi.OrchardCore.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentVariantPartDisplayDriver : ContentPartDisplayDriver<ExperimentVariantPart>
    {
        public override IDisplayResult Edit(ExperimentVariantPart part)
        {
            return Initialize<ExperimentVariantViewModel>("ExperimentVariantPart_Edit", model =>
            {
                model.Title = "Test";
                model.ExperimentVariantPart = part;

                return Task.CompletedTask;
            });
        }
    }
}
