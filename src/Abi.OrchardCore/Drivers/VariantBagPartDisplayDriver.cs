using Abi.Models;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Flows.Models;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Drivers
{
    public class VariantBagPartDisplayDriver : ContentPartDisplayDriver<BagPart>
    {
        private readonly IExperimentManager _experimentManager;

        public VariantBagPartDisplayDriver(IExperimentManager experimentManager)
        {
            _experimentManager = experimentManager;
        }

        public override async Task<IDisplayResult> DisplayAsync(BagPart part, BuildPartDisplayContext context)
        {
            if (context?.TypePartDefinition?.ContentTypeDefinition?.Name == nameof(Experiment) && context?.DisplayType == "Detail")
            {
                //part = await _experimentManager.GetOrSetVariantAsync(part);
            }

            return await base.DisplayAsync(part, context);
        }
    }
}
