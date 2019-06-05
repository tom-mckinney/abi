using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Services
{
    public interface ICookieService
    {
        void AddExperimentCookie(string experimentId, string variantId);
        bool TryGetExperimentCookie(string experimentId, out string variantId);
    }

    public class CookieService : ICookieService
    {
        private readonly HttpContext _httpContext;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public void AddExperimentCookie(string experimentId, string variantId)
        {
            string cookieName = CookieName(experimentId);

            _httpContext.Response.Cookies.Append(cookieName, variantId);
        }

        public bool TryGetExperimentCookie(string experimentId, out string variantId)
        {
            string cookieName = CookieName(experimentId);

            return _httpContext.Request.Cookies.TryGetValue(cookieName, out variantId);
        }

        private string CookieName(string experimentId)
        {
            return $"abi_{experimentId}";
        }
    }
}
