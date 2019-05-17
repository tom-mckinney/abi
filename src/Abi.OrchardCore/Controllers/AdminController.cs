using Abi.Data;
using Abi.OrchardCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRepository<ExperimentShape> _experimentRepository;
        private readonly ISession _session;

        public AdminController(IAuthorizationService authorizationService, IRepository<ExperimentShape> experimentRepository, ISession session)
        {
            _authorizationService = authorizationService;
            _experimentRepository = experimentRepository;
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            //if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageOwnExperiments))
            //{
            //    return Unauthorized();
            //}

            return View(await _experimentRepository.GetAllAsync());

            //var query = _session.Query<ContentItem, ContentItemIndex>();

            //query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Models.Experiment));

            //var experiments = await query.ListAsync();

            //return View(experiments);
        }
    }
}
