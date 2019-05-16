using OrchardCore.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageExperiments = new Permission("ManageExperiments", "Manage Experiments");
        public static readonly Permission ManageOwnExperiments = new Permission("ManageOwnExperiments", "Manage Own Experiments", new[] { ManageExperiments });

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ManageExperiments, ManageOwnExperiments };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageExperiments }
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { ManageExperiments }
                },
                new PermissionStereotype
                {
                    Name = "Author",
                    Permissions = new[] { ManageOwnExperiments }
                },
                new PermissionStereotype
                {
                    Name = "Moderator",
                },
                new PermissionStereotype
                {
                    Name = "Contributor",
                },
            };
        }
    }
}
