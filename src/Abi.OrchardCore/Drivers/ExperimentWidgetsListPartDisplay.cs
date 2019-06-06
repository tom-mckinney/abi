using Abi.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using OrchardCore.Widgets.Drivers;
using OrchardCore.Widgets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentWidgetsListPartDisplay : ContentPartDisplayDriver<WidgetsListPart> //: WidgetsListPartDisplay
    {
        private readonly IExperimentManager _experimentManager;

        public ExperimentWidgetsListPartDisplay(IExperimentManager experimentManager)
            //IContentManager contentManager,
            //IContentDefinitionManager contentDefinitionManager,
            //IServiceProvider serviceProvider,
            //ISiteService siteService)
            //: base(contentManager, contentDefinitionManager, serviceProvider, siteService)
        {
            _experimentManager = experimentManager;
        }

        public override async Task<IDisplayResult> DisplayAsync(WidgetsListPart part, BuildPartDisplayContext context)
        {
            if (context?.TypePartDefinition?.ContentTypeDefinition?.Name == nameof(Experiment) && context?.DisplayType == "Detail")
            {
                part = await _experimentManager.GetOrSetVariantAsync(part);
            }

            return await base.DisplayAsync(part, context);
        }
    }
}
