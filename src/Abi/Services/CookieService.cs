using Abi.Data;
using Abi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Services
{
    public interface ICookieService
    {
        void AddExperimentCookie(string zone, string experimentId, string variantId);
        void AddSessionCookie(string sessionPublicId);
        void AddVisitorCookie(string visitorPublicId);
        Task<bool> TryGetExperimentCookie(string zone, string experimentId, out string variantId);
        Task<bool> TryGetSessionCookie(out string sessionId);
        Task<bool> TryGetVisitorCookieAsync(out string visitorId);
        string BuildCookieName(params string[] parts);
    }

    public class CookieService : ICookieService
    {
        private readonly HttpContext _httpContext;
        private readonly IUserRepository _userRepository;

        public CookieService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
        }

        private void AddCookie(string cookieName, string cookieValue)
        {
            _httpContext.Response.Cookies.Append(cookieName, cookieValue);
        }

        public void AddExperimentCookie(string zone, string experimentId, string variantId)
        {
            string cookieName = BuildCookieName(zone, experimentId);

            AddCookie(cookieName, variantId);
        }

        public void AddSessionCookie(string sessionPublicId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Session);

            AddCookie(cookieName, sessionPublicId);
        }

        public void AddVisitorCookie(string visitorPublicId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Visitor);

            AddCookie(cookieName, visitorPublicId);
        }

        public string BuildCookieName(params string[] parts)
        {
            return $"{Constants.Cookies.Prefix}_{string.Join("_", parts)}";
        }

        private bool TryGetCookie(string cookieName, out string cookieValue)
        {
            return _httpContext.Request.Cookies.TryGetValue(cookieName, out cookieValue);
        }

        public Task<bool> TryGetExperimentCookie(string zone, string experimentId, out string variantId)
        {
            string cookieName = BuildCookieName(zone, experimentId);

            return Task.FromResult(TryGetCookie(cookieName, out variantId));
        }

        public Task<bool> TryGetSessionCookie(out string sessionId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Session);

            return Task.FromResult(TryGetCookie(cookieName, out sessionId));
        }

        public Task<bool> TryGetVisitorCookieAsync(out string visitorId)
        {
            //if (_httpContext?.User?.Identity?.IsAuthenticated == true)
            //{
            //    User user = await _userRepository.GetByUserNameAsync(_httpContext.User.Identity.Name);
            //}

            string cookieName = BuildCookieName(Constants.Cookies.Visitor);

            return Task.FromResult(TryGetCookie(cookieName, out visitorId));
        }
    }
}
