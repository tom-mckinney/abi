using Abi.Data;
using Abi.Models;
using Abi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi
{
    public abstract class ExperimentManagerBase
    {
        protected readonly IVisitorRepository _visitorRepository;
        protected readonly ISessionRepository _sessionRepository;
        protected readonly IVariantRepository _variantRepository;
        protected readonly ICookieService _cookieService;

        public ExperimentManagerBase(
            IVisitorRepository visitorRepository,
            ISessionRepository sessionRepository,
            IVariantRepository variantRepository,
            ICookieService cookieService)
        {
            _visitorRepository = visitorRepository;
            _sessionRepository = sessionRepository;
            _variantRepository = variantRepository;
            _cookieService = cookieService;
        }

        protected virtual async Task<Visitor> GetOrCreateVisitorAsync()
        {
            Visitor visitor = null;

            if (_cookieService.TryGetVisitorCookie(out string visitorId))
            {
                visitor = await _visitorRepository.GetByPublicIdAsync(visitorId);
            }

            if (visitor == null)
            {
                visitor = await _visitorRepository.CreateAsync();
            }

            return visitor;
        }

        protected virtual async Task<Session> GetOrCreateSessionAsync(string visitorId)
        {
            Session session = null;

            if (_cookieService.TryGetSessionCookie(out string sessionId))
            {
                session = await _sessionRepository.GetByPublicIdAsync(sessionId);
            }

            if (session == null)
            {
                session = await _sessionRepository.CreateAsync(visitorId);
            }

            return session;
        }

        protected virtual async Task<Variant> GetVariantAsync(string zone, string experimentId)
        {
            Variant variant = null;

            if (_cookieService.TryGetVariantCookie(zone, experimentId, out string variantId))
            {
                variant = await _variantRepository.GetByPublicIdAsync(variantId);
            }

            return variant;
        }
    }
}
