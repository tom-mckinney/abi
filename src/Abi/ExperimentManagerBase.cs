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
        protected readonly ICookieService _cookieService;

        public ExperimentManagerBase(
            IVisitorRepository visitorRepository,
            ISessionRepository sessionRepository,
            ICookieService cookieService)
        {
            _visitorRepository = visitorRepository;
            _sessionRepository = sessionRepository;
            _cookieService = cookieService;
        }

        protected virtual async Task<Visitor> GetOrCreateVisitorAsync()
        {
            Visitor visitor = null;

            if (await _cookieService.TryGetVisitorCookieAsync(out string visitorId))
            {
                visitor = await _visitorRepository.GetByPublicIdAsync(visitorId);
            }

            if (visitor == null)
            {
                visitor = await _visitorRepository.CreateAsync();
            }

            return visitor;
        }

        protected virtual async Task<Session> GetOrCreateSessionAsync(int visitorId)
        {
            Session session = null;

            if (await _cookieService.TryGetSessionCookie(out string sessionId))
            {
                session = await _sessionRepository.GetByPublicIdAsync(sessionId);
            }

            if (session == null)
            {
                session = await _sessionRepository.CreateAsync(visitorId);
            }

            return session;
        }
    }
}
