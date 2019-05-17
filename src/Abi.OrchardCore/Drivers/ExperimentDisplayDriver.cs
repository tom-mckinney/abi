using Abi.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentDisplayDriver : ContentDisplayDriver
    {
        public override IDisplayResult Display(ContentItem model)
        {
            if (model.ContentType != nameof(Experiment))
            {
                return null;
            }

            return Shape("Experiment_Summary", null);
        }
    }
}
