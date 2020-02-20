using Abi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Abi.Test.Services
{
    public class CookieServiceTests
    {
        private readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        private ICookieService CreateService()
        {
            return new CookieService(_httpContextAccessor);
        }

        [Fact]
        public void GetUserIdOrDefault_returns_id_as_int_if_it_exists()
        {
            string userIdString = "123";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdString));
            _httpContextAccessor.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            };

            var service = CreateService();

            int? userId = service.GetUserIdOrDefault();

            Assert.Equal(123, userId);
        }

        [Fact]
        public void GetUserIdOrDefault_returns_null_if_no_user_id()
        {
            _httpContextAccessor.HttpContext = new DefaultHttpContext();

            var service = CreateService();

            int? userId = service.GetUserIdOrDefault();

            Assert.Null(userId);
        }

        [Theory]
        [InlineData(1, "abi_wumbo")]
        [InlineData(2, "abi_wumbo_wumbo")]
        [InlineData(3, "abi_wumbo_wumbo_wumbo")]
        public void BuildCookieName_will_join_all_parts_with_abi_separated_by_underscores(int partCount, string expectedCookieName)
        {
            List<string> parts = new List<string>();
            for (int i = 0; i < partCount; i++)
            {
                parts.Add("wumbo");
            }

            var service = CreateService();

            string actualCookieName = service.BuildCookieName(parts.ToArray());

            Assert.Equal(expectedCookieName, actualCookieName);
        }
    }
}
