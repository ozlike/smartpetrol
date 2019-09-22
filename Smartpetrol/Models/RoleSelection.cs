using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Data;

namespace Smartpetrol.Models
{
    public class RoleSelection
    {
        public RoleName RoleName { get; set; }
        public bool Selected { get; set; }

        public RoleSelection() { }
        public RoleSelection(RoleName RoleName, bool Selected = false)
        {
            this.RoleName = RoleName;
            this.Selected = Selected;
        }
    }
}
