using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Models.Users
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string RoleFullNames { get; set; }
    }
}
