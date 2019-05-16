using System;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Permission permission)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }

        public Permission Permission { get; set; }
    }
}
