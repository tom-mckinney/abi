﻿using Abi.Services;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Widgets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abi.OrchardCore
{
    public interface IExperimentManager : IExperimentManager<WidgetsListPart>
    {
    }

    public class OrchardExperimentManager : IExperimentManager
    {
        private readonly ICookieService _cookieService;
        private readonly ContentBalancer _contentBalancer;

        public OrchardExperimentManager(ICookieService cookieService, ContentBalancer contentBalancer)
        {
            _cookieService = cookieService;
            _contentBalancer = contentBalancer;
        }

        //public Task<BagPart> GetOrSetVariantAsync(BagPart content)
        //{
        //    string experimentId = content.ContentItem.ContentItemId;

        //    if (!_cookieService.TryGetExperimentCookie(experimentId, out string variantContentId)
        //        || !content.ContentItems.Any(c => c.ContentItemId == variantContentId))
        //    {
        //        int variantIndex = _contentBalancer.GetRandomIndex(content.ContentItems); // TODO: make this personalized/influenced by history
        //        variantContentId = content.ContentItems.ElementAt(variantIndex).ContentItemId;
        //    }

        //    _cookieService.AddExperimentCookie(experimentId, variantContentId);

        //    content.ContentItems.RemoveAll(c => c.ContentItemId != variantContentId);

        //    return Task.FromResult(content);
        //}

        public Task<WidgetsListPart> GetOrSetVariantAsync(WidgetsListPart content)
        {
            string experimentId = content.ContentItem.ContentItemId;

            foreach (var widgetZone in content.Widgets)
            {
                var zone = widgetZone.Key;
                var widgetList = widgetZone.Value;

                if (!_cookieService.TryGetExperimentCookie(zone, experimentId, out string variantContentId)
                    || !widgetList.Any(c => c.ContentItemId == variantContentId))
                {
                    int variantIndex = _contentBalancer.GetRandomIndex(widgetList); // TODO: make this personalized/influenced by history
                    variantContentId = widgetList.ElementAt(variantIndex).ContentItemId;
                }

                _cookieService.AddExperimentCookie(zone, experimentId, variantContentId);

                widgetList.RemoveAll(c => c.ContentItemId != variantContentId);
            }

            //if (!_cookieService.TryGetExperimentCookie(experimentId, out string variantContentId)
            //    || !content.Widgets.Any(w => w.Value.Any(c => c.ContentItemId == variantContentId)))
                //|| !content.ContentItems.Any(c => c.ContentItemId == variantContentId))
            {
                //int variantIndex = _contentBalancer.GetRandomIndex(content.ContentItems); // TODO: make this personalized/influenced by history
                //variantContentId = content.ContentItems.ElementAt(variantIndex).ContentItemId;
            }


            return Task.FromResult(content);
        }
    }
}
