using Abi.OrchardCore.Data;
using Abi.OrchardCore.Filters;
using Abi.OrchardCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Liquid;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using System;

namespace Abi.OrchardCore
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<AbiMiddleware>();

            // Repositories
            services.AddScoped<IExperimentRepository, ExperimentRepository>();

            // Liquid Filters
            services.AddLiquidFilter<AbiFilter>("abi");
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            builder.UseMiddleware<AbiMiddleware>();

            routes.MapAreaRoute(
                name: "ListExperiments",
                areaName: "Abi.OrchardCore",
                template: "/Admin/Experiments",
                defaults: new { controller = "Admin", action = "List" });
        }
    }
}
