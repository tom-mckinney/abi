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
        Task<bool> TryGetExperimentCookie(string zone, string experimentId, out string variantId);
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

        public void AddExperimentCookie(string zone, string experimentId, string variantId)
        {
            string cookieName = BuildCookieName(zone, experimentId);

            _httpContext.Response.Cookies.Append(cookieName, variantId);
        }

        public string BuildCookieName(params string[] parts)
        {
            return string.Join("_", "abi", parts);
        }

        public Task<bool> TryGetExperimentCookie(string zone, string experimentId, out string variantId)
        {
            string cookieName = BuildCookieName(zone, experimentId);

            return Task.FromResult(_httpContext.Request.Cookies.TryGetValue(cookieName, out variantId));
        }

        public Task<bool> TryGetVisitorCookieAsync(out string visitorId)
        {
            //if (_httpContext?.User?.Identity?.IsAuthenticated == true)
            //{
            //    User user = await _userRepository.GetByUserNameAsync(_httpContext.User.Identity.Name);


            //}

            visitorId = null;

            throw new NotImplementedException();
        }
    }
}
