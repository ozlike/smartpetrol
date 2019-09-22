using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Data;
using Smartpetrol.Extensions;

namespace Smartpetrol.Models.Users
{
    public class RolesList
    {
        public List<RoleSelection> Roles { get; set; }

        public RolesList()
        {
            Roles = new List<RoleSelection>();
            foreach (var roleName in EnumHelper.GetValues<RoleName>())
            {
                Roles.Add(new RoleSelection(roleName));
            }
        }
    }
}
