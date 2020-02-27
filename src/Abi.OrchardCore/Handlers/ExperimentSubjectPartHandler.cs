using Abi.OrchardCore.Models;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore.Handlers
{
    public class ExperimentSubjectPartHandler : ContentPartHandler<ExperimentSubjectPart>
    {
        public override Task UpdatedAsync(UpdateContentContext context, ExperimentSubjectPart instance)
        {
            instance.Name = "Wumbo";

            return base.UpdatedAsync(context, instance);
        }
    }
}
