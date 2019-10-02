using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Models.Books
{
    public enum BookStatus
    {
        [DisplayName("Свободна")]
        Free = 0,
        [DisplayName("Зарезервирована")]
        Reserved = 1,
        [DisplayName("Арендована")]
        Rented = 2,
    }
}
