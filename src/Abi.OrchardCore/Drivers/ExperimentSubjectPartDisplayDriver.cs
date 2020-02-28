using Abi.Models;
using Abi.OrchardCore.Data;
using Abi.OrchardCore.Models;
using Abi.OrchardCore.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Drivers
{
    public class ExperimentSubjectPartDisplayDriver : ContentPartDisplayDriver<ExperimentSubjectPart>
    {
        private readonly IExperimentRepository _experimentRepository;

        public ExperimentSubjectPartDisplayDriver(IExperimentRepository experimentRepository)
        {
            _experimentRepository = experimentRepository;
        }

        public override IDisplayResult Edit(ExperimentSubjectPart part, BuildPartEditorContext context)
        {
            return Initialize<ExperimentSubjectPartEditViewModel>("ExperimentSubjectPart_Edit", model =>
            {
                model.ExperimentId = part.ExperimentId;
                model.ExperimentName = part.Name;

                if (!string.IsNullOrEmpty(part.ExperimentId))
                {
                    _experimentRepository.GetByContentItemIdAsync(part.ExperimentId);
                }

                return Task.CompletedTask;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(ExperimentSubjectPart part, IUpdateModel updater)
        {
            var viewModel = new ExperimentSubjectPartEditViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                if (viewModel.CreateNewExperiment == true)
                {
                    var experiment = await _experimentRepository.CreateAsync(viewModel.ExperimentName);

                    part.ExperimentId = experiment.ExperimentId;
                }

                part.Name = viewModel.ExperimentName;
            }

            return Edit(part);
        }
    }
}
