using Abi.Models;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore.Drivers
{
    public class VariantBagPartDisplayDriver : ContentPartDisplayDriver<BagPart>
    {
        private readonly IExperimentManager _experimentManager;

        public VariantBagPartDisplayDriver(IExperimentManager experimentManager)
        {
            _experimentManager = experimentManager;
        }

        public override IDisplayResult Display(BagPart part, BuildPartDisplayContext context)
        {
            if (context?.TypePartDefinition?.ContentTypeDefinition?.Name == nameof(Experiment))
            {
                part.ContentItems.RemoveAt(_experimentManager.GetVariantIndex(part));
            }

            return base.Display(part, context);
        }
    }
}
