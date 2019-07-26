using Abi.Models;
using Abi.OrchardCore.Data.Indexes;
using Abi.OrchardCore.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Model;
using OrchardCore.Title.Model;
using System;

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
            _contentDefinitionManager.AlterTypeDefinition(Constants.Types.Experiment, type => type
                .Stereotype("Widget")
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart(nameof(TitlePart))
                .WithPart(nameof(FlowPart), part =>
                {
                    part.WithDisplayName("Variants");
                })
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(EncounterIndex), table => table
                .Column<int>(nameof(EncounterIndex.SessionId))
                .Column<string>(nameof(EncounterIndex.VariantId)));

            return 3;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(SessionIndex), table => table
                .Column<string>(nameof(SessionIndex.SessionId))
                .Column<string>(nameof(SessionIndex.VisitorId)));

            SchemaBuilder.CreateMapIndexTable(nameof(VariantIndex), table => table
                .Column<string>(nameof(VariantIndex.VariantId))
                .Column<string>(nameof(VariantIndex.ContentItemid)));

            SchemaBuilder.CreateMapIndexTable(nameof(VisitorIndex), table => table
                .Column<string>(nameof(VisitorIndex.VisitorId))
                .Column<string>(nameof(VisitorIndex.UserId)));

            return 4;
        }

        public int UpdateFrom4()
        {
            SchemaBuilder.AlterTable(nameof(EncounterIndex), table =>
            {
                table.DropColumn(nameof(EncounterIndex.SessionId));
                table.DropColumn("ContentVariantId");
            });

            return 5;
        }

        public int UpdateFrom5()
        {
            SchemaBuilder.AlterTable(nameof(EncounterIndex), table =>
            {
                table.AddColumn<string>(nameof(EncounterIndex.EncounterId));
                //table.AddColumn<string>(nameof(EncounterIndex.SessionId));
                //table.AddColumn<string>(nameof(EncounterIndex.VariantId));
            });

            return 6;
        }

        public int UpdateFrom6()
        {
            SchemaBuilder.CreateTable(Constants.CustomTables.Encounters, table => table
                .Column<int>(nameof(Encounter.Id), col => col.PrimaryKey().Identity())
                .Column<string>(nameof(Encounter.EncounterId), col => col.Unique())
                .Column<string>(nameof(Encounter.SessionId))
                .Column<string>(nameof(Encounter.VariantId))
                .Column<DateTime?>(nameof(Encounter.CreatedUtc), col => col.Nullable())
                .Column<DateTime?>(nameof(Encounter.ModifiedUtc), col => col.Nullable())
            );

            return 7;
        }
    }
}
