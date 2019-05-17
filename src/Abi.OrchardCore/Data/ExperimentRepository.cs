using Abi.Data;
using Abi.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Shapes;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Data
{
    public class ExperimentShape : Shape, IContent
    {
        public ExperimentShape(ContentItem content)
        {
            ContentItem = content;
        }

        public ContentItem ContentItem { get; }
    }

    public class ExperimentRepository : IRepository<ExperimentShape>
    {
        private readonly ISession _session;

        public ExperimentRepository(ISession session)
        {
            _session = session;
        }

        public async Task<IList<ExperimentShape>> GetAllAsync()
        {
            var query = _session.Query<ContentItem, ContentItemIndex>();

            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Experiment));

            var experiments = await query.ListAsync();

            return experiments.Select(e => new ExperimentShape(e)).ToList();
        }
    }

    public class OldExperimentRepository : IExperimentRepository
    {
        private readonly ISession _session;

        public OldExperimentRepository(ISession session)
        {
            _session = session;
        }

        public async Task<IList<Experiment>> GetAllAsync()
        {
            var query = _session.Query<ContentItem, ContentItemIndex>();

            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Experiment));

            var experiments = await query.ListAsync();

            return experiments.Select(e => new Experiment { Name = e.DisplayText }).ToList();
        }
    }
}
