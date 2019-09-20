using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Data
{
    public struct Roles
    {
        public const string Admin = "Admin";
        public const string Librarian = "Librarian";
        public const string Client = "Client";
    }

    public enum RoleName
    {
        Admin = 0,
        Librarian = 1,
        Client = 2,
    }
}
