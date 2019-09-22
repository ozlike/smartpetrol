using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Books;

namespace Smartpetrol.Data.Interfaces
{
    public interface IBooksProvider
    {
        Task CreateBook(CreateBookViewModel model);

    }
}
