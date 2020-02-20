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
    public class ExperimentSubjectPartDisplayDriver : ContentPartDisplayDriver<ExperimentSubjectPart>
    {
        public override IDisplayResult Edit(ExperimentSubjectPart part, BuildPartEditorContext context)
        {
            return Initialize<ExperimentSubjectPartViewModel>("ExperimentSubjectPart_Edit", model =>
            {
                model.Title = "Wumbo";

                return Task.CompletedTask;
            });
        }
    }
}
