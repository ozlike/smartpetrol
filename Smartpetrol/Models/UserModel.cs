using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string RoleFullName { get; set; }
    }
}
