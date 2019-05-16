﻿using Abi.OrchardCore.Filters;
using Abi.OrchardCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Liquid;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<AbiMiddleware>();

            services.AddLiquidFilter<AbiFilter>("abi");
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            builder.UseMiddleware<AbiMiddleware>();
        }
    }
}
