using Smartpetrol.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Data.Interfaces
{
    public interface IClientProvider
    {
        Task<bool> ReserveBookAsync(Guid bookId);
        Task<List<BookViewModel>> GetReservedBooksAsync();
        Task<bool> CancelReservation(Guid bookId);
        Task<List<BookViewModel>> GetRentedBooksAsync();
    }
}
