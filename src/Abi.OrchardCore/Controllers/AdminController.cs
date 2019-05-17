using Abi.Data;
using Abi.OrchardCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IRepository<ExperimentShape> _experimentRepository;
        private readonly ISession _session;

        public AdminController(
            IAuthorizationService authorizationService,
            IContentItemDisplayManager contentItemDisplayManager,
            IRepository<ExperimentShape> experimentRepository,
            ISession session)
        {
            _authorizationService = authorizationService;
            _contentItemDisplayManager = contentItemDisplayManager;
            _experimentRepository = experimentRepository;
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            //if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageOwnExperiments))
            //{
            //    return Unauthorized();
            //}

            var experiments = await _experimentRepository.GetAllAsync();

            var experimentSummaries = new List<dynamic>();
            foreach (var experiment in experiments)
            {
                experimentSummaries.Add(await _contentItemDisplayManager.BuildDisplayAsync(experiment.ContentItem, this, "SummaryAdmin"));
            }

            return View(experimentSummaries);

            //var query = _session.Query<ContentItem, ContentItemIndex>();

            //query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Models.Experiment));

            //var experiments = await query.ListAsync();

            //return View(experiments);
        }
    }
}
