using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartpetrol.Models.Books;

namespace Smartpetrol.Data.Interfaces
{
    public interface IBookProvider
    {
        Task CreateBookAsync(BookViewModel model);
        Task<ICollection<BookViewModel>> GetAllBooksAsync();
        Task<BookViewModel> GetBookToManipulateAsync(Guid bookId);
        Task<BookViewModel> GetBookToGiveOutAsync(Guid bookId);
        Task<bool> EditBookAsync(BookViewModel model);
        Task<bool> GiveOutBookAsync(Guid bookId);
        Task<bool> DeleteBookAsync(Guid bookId);
        List<BookViewModel> GetBooksToReserve();
        Task<List<BookViewModel>> GetReservedBooksAsync();
        Task<List<BookViewModel>> GetRentedBooksAsync();
        Task<BookViewModel> GetBookToTakeFromUserAsync(Guid bookId);
        Task<bool> TakeBookFromUserAsync(Guid bookId);
    }
}
