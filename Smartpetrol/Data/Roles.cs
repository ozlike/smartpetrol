using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Администратор")]
        Admin = 0,
        [DisplayName("Библиотекарь")]
        Librarian = 1,
        [DisplayName("Клиент")]
        Client = 2,
    }
}
