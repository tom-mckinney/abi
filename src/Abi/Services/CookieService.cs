using Abi.Data;
using Abi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Services
{
    public interface ICookieService
    {
        void AddSessionCookie(string sessionPublicId);
        void AddVariantCookie(string zone, string experimentId, string variantId);
        void AddVisitorCookie(string visitorPublicId);
        bool TryGetSessionCookie(out string sessionId);
        bool TryGetVariantCookie(string zone, string experimentId, out string variantId);
        bool TryGetVisitorCookie(out string visitorId);
        int? GetUserIdOrDefault();
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
            AddCookie(cookieName, cookieValue, DateTimeOffset.Now.AddYears(5));
        }

        private void AddCookie(string cookieName, string cookieValue, DateTimeOffset? expires)
        {
            var options = new CookieOptions
            {
                Expires = expires,
                Secure = true
            };

            _httpContext.Response.Cookies.Append(cookieName, cookieValue, options);
        }

        public void AddSessionCookie(string sessionPublicId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Session);

            AddCookie(cookieName, sessionPublicId, null);
        }

        public void AddVariantCookie(string zone, string experimentId, string variantId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Variant, zone, experimentId);

            AddCookie(cookieName, variantId);
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

        public bool TryGetSessionCookie(out string sessionId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Session);

            return TryGetCookie(cookieName, out sessionId);
        }

        public bool TryGetVariantCookie(string zone, string experimentId, out string variantId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Variant, zone, experimentId);

            return TryGetCookie(cookieName, out variantId);
        }

        public bool TryGetVisitorCookie(out string visitorId)
        {
            string cookieName = BuildCookieName(Constants.Cookies.Visitor);

            return TryGetCookie(cookieName, out visitorId);
        }

        public int? GetUserIdOrDefault()
        {
            var userIdClaim = _httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim?.Value, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}
