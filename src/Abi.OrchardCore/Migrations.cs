using Abi.Models;
using Abi.OrchardCore.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Model;
using OrchardCore.Html.Settings;
using OrchardCore.Title.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition(nameof(ContentVariant), type => type
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(nameof(TitlePart))
                .WithPart(nameof(FlowPart))
                .WithPart(nameof(HtmlBodyPart), part =>
                {
                    part.WithSetting("Editor", "Wysiwyg");
                })
            );

            _contentDefinitionManager.AlterTypeDefinition(nameof(Experiment), type => type
                .Stereotype("Widget")
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(nameof(TitlePart))
                .WithPart(nameof(BagPart), part =>
                {
                    part.WithDisplayName("Variants");
                    part.WithSetting("ContainedContentTypes", new string[] { nameof(ContentVariant) });
                })
            );

            return 1;
        }
    }
}
