using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using Abi.OrchardCore.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Models;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using System;
using System.Threading.Tasks;

namespace Abi.OrchardCore
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRecipeMigrator _recipeMigrator;

        public Migrations(IContentDefinitionManager contentDefinitionManager, IRecipeMigrator recipeMigrator)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(ExperimentSubjectPart), part => part
                .Attachable());

            _contentDefinitionManager.AlterTypeDefinition(Constants.Types.Experiment, type => type
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(nameof(TitlePart))
                .WithPart(nameof(FlowPart), part =>
                {
                    part.WithDisplayName("Variants");
                })
            );

            SchemaBuilder.CreateMapIndexTable(nameof(VisitorIndex), table => table
                .Column<string>(nameof(VisitorIndex.VisitorId))
                .Column<int?>(nameof(VisitorIndex.UserId), col => col.Nullable()));

            SchemaBuilder.CreateMapIndexTable(nameof(SessionIndex), table => table
                .Column<string>(nameof(SessionIndex.SessionId))
                .Column<string>(nameof(SessionIndex.VisitorId)));

            SchemaBuilder.CreateMapIndexTable(nameof(VariantIndex), table => table
                .Column<string>(nameof(VariantIndex.VariantId))
                .Column<string>(nameof(VariantIndex.ContentItemid)));

            SchemaBuilder.CreateTable(Constants.CustomTables.Encounters, table => table
                .Column<int>(nameof(Encounter.Id), col => col.PrimaryKey().Identity())
                .Column<string>(nameof(Encounter.EncounterId), col => col.Unique())
                .Column<string>(nameof(Encounter.SessionId))
                .Column<string>(nameof(Encounter.VariantId))
                .Column<DateTime?>(nameof(Encounter.CreatedUtc), col => col.Nullable())
                .Column<DateTime?>(nameof(Encounter.ModifiedUtc), col => col.Nullable())
            );

            await _recipeMigrator.ExecuteAsync("abi.recipe.json", this);

            return 1;
        }

        public Task<int> UpdateFrom1Async()
        {
            SchemaBuilder.AlterTable(nameof(VariantIndex), table => table
                .AddColumn<string>(nameof(VariantIndex.ExperimentId)));

            return Task.FromResult(2);
        }
    }
}
