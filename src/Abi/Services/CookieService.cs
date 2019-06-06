using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Services
{
    public interface ICookieService
    {
        void AddExperimentCookie(string zone, string experimentId, string variantId);
        bool TryGetExperimentCookie(string zone, string experimentId, out string variantId);
    }

    public class CookieService : ICookieService
    {
        private readonly HttpContext _httpContext;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public void AddExperimentCookie(string zone, string experimentId, string variantId)
        {
            string cookieName = CookieName(zone, experimentId);

            _httpContext.Response.Cookies.Append(cookieName, variantId);
        }

        public bool TryGetExperimentCookie(string zone, string experimentId, out string variantId)
        {
            string cookieName = CookieName(zone, experimentId);

            return _httpContext.Request.Cookies.TryGetValue(cookieName, out variantId);
        }

        //private string CookieName(string experimentId)
        //{
        //    return $"abi_{experimentId}";
        //}

        //private string CookieName(string experimentId, string zoneName)
        //{
        //    return $"abi_{zoneName}_{experimentId}";
        //}

        private string CookieName(params string[] displayIds)
        {
            return $"abi_{string.Join("_", displayIds)}";
        }
    }
}
