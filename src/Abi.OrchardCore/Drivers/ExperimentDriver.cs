using Abi.Models;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.Contents.Drivers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Flows.Models;
using OrchardCore.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentDriver : ContentDisplayDriver
    {
        public override IDisplayResult Display(ContentItem model)
        {
            if (model.ContentType == nameof(Experiment))
            {
                var test = "test";
            }


            //return Ini

            //throw new NotImplementedException();

            return base.Display(model);
        }
    }

    public class VariantBagPartDisplayDriver : ContentPartDisplayDriver<BagPart>
    {
        private readonly IExperimentManager _experimentManager;

        public VariantBagPartDisplayDriver(IExperimentManager experimentManager)
        {
            _experimentManager = experimentManager;
        }

        public override IDisplayResult Display(BagPart part, BuildPartDisplayContext context)
        {
            if (context.TypePartDefinition.ContentTypeDefinition.Name == nameof(Experiment))
            {
                part.ContentItems.RemoveAt(_experimentManager.GetVariantIndex(part));
            }
            
            //context.
            return base.Display(part, context);
        }
    }

    //public class TestDriver : ContentDisplayDriver
    //{

    //}

    //public class ExperimentPart : ContentPart
    //{
        
    //}

    //class Test : ContentPartDisplayDriver
}
