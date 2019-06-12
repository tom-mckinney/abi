using Abi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Abi.Test.Services
{
    public class CookieServiceTests
    {
        private ICookieService CreateService()
        {
            return new CookieService(new HttpContextAccessor(), null);
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
