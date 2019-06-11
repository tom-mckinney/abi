using Abi.OrchardCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using YesSql;

namespace Abi.OrchardCore.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IExperimentRepository _experimentRepository;

        public AdminController(
            IAuthorizationService authorizationService,
            IContentItemDisplayManager contentItemDisplayManager,
            IExperimentRepository experimentRepository)
        {
            _authorizationService = authorizationService;
            _contentItemDisplayManager = contentItemDisplayManager;
            _experimentRepository = experimentRepository;
        }

        public async Task<IActionResult> List()
        {
            //if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageOwnExperiments))
            //{
            //    return Unauthorized();
            //}

            var experiments = await _experimentRepository.GetAllAsync();

            var experimentSummaries = new List<dynamic>();
            foreach (var experiment in experiments)
            {
                experimentSummaries.Add(await _contentItemDisplayManager.BuildDisplayAsync(experiment, this, "SummaryAdmin"));
            }

            return View(experimentSummaries);
        }

        public async Task<IActionResult> Display(string contentItemId)
        {
            var experiment = await _experimentRepository.GetByContentItemIdAsync(contentItemId);

            var shape = await _contentItemDisplayManager.BuildDisplayAsync(experiment, this, "DisplayAdmin");

            return View(shape);
        }
    }
}
