using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Books;

namespace Smartpetrol.Data.Interfaces
{
    public interface IBooksProvider
    {
        Task CreateBookAsync(BookViewModel model);
        Task<ICollection<BookViewModel>> GetAllBooksAsync();
        Task<BookViewModel> GetBookToEditAsync(Guid bookId);
        Task<bool> EditBookAsync(BookViewModel model);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}
