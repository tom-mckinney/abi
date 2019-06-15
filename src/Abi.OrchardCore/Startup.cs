using Abi.Data;
using Abi.OrchardCore.Data;
using Abi.OrchardCore.Drivers;
using Abi.OrchardCore.Filters;
using Abi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement.Display.ContentDisplay;
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
            services.AddHttpContextAccessor();

            services.AddSingleton<ContentBalancer>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IExperimentManager, OrchardExperimentManager>();
            services.AddScoped<IContentPartDisplayDriver, ExperimentFlowPartDisplay>();

            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            // Repositories
            services.AddScoped<IExperimentRepository, ExperimentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Liquid Filters
            services.AddLiquidFilter<AbiFilter>("abi");
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "ListExperiments",
                areaName: "Abi.OrchardCore",
                template: "/Admin/Experiments",
                defaults: new { controller = "Admin", action = "List" });

            routes.MapAreaRoute(
                name: "DisplayExperiment",
                areaName: "Abi.OrchardCore",
                template: "/Admin/Experiments/{contentItemId}",
                defaults: new { controller = "Admin", action = "Display" });
        }
    }
}
