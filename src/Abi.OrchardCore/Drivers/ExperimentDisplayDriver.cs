using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.ViewModels;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentDisplayDriver : ContentDisplayDriver
    {
        public override Task<IDisplayResult> UpdateAsync(ContentItem model, IUpdateModel updater)
        {
            return Task.FromResult<IDisplayResult>(Shape("ContentPreview_Button", new ContentItemViewModel(model)).Location("Actions:after"));
            //return Task.FromResult(Shape("Wumbo", new ContentItemViewModel(model)));
            //return base.UpdateAsync(model, updater);
        }

        public override Task<IDisplayResult> DisplayAsync(ContentItem model, IUpdateModel updater)
        {
            return base.DisplayAsync(model, updater);
        }

        public override Task<IDisplayResult> EditAsync(ContentItem model, BuildEditorContext context)
        {
            return base.EditAsync(model, context);
        }

        public override Task<IDisplayResult> EditAsync(ContentItem model, IUpdateModel updater)
        {
            return base.EditAsync(model, updater);
        }

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
