using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Views;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentDisplayDriver : ContentDisplayDriver
    {
        public override IDisplayResult Edit(ContentItem model)
        {
            if (model.ContentType == Constants.Types.Experiment)
            {
                //model.Content
            }

            return base.Edit(model);
        }
    }
}
