using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartpetrol.Data
{
    public class ClientProvider : IClientProvider
    {
        readonly SmartDbContext _context;
        readonly IMapper _mapper;
        readonly IUserProvider _userProvider;
        public ClientProvider(SmartDbContext context, IMapper mapper, IUserProvider userProvider)
        {
            _userProvider = userProvider;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CancelReservation(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book == null) return false;
            book.Status = BookStatus.Free;
            book.Tenant = null;
            book.TenantId = null;
            book.ReservationTime = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BookViewModel>> GetRentedBooksAsync()
        {
            var user = await _userProvider.GetCurrentUserAsync();
            var books = await _context.Books.Where(x => x.TenantId == user.Id && x.Status == BookStatus.Rented).ToListAsync();
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public async Task<List<BookViewModel>> GetReservedBooksAsync()
        {
            var user = await _userProvider.GetCurrentUserAsync();
            var books = await _context.Books.Where(x => x.TenantId == user.Id && x.Status == BookStatus.Reserved).ToListAsync();
            return _mapper.Map<List<BookViewModel>>(books).OrderBy(x => x.ReservationEndTime).ToList();
        }

        public async Task<bool> ReserveBookAsync(Guid bookId)
        {
            if (bookId == Guid.Empty) return false;
            var book = await GetBookByIdAsync(bookId);
            if (book.Status != BookStatus.Free) return false;
            book.TenantId = (await _userProvider.GetCurrentUserAsync()).Id;
            book.ReservationTime = DateTime.UtcNow;
            book.Status = BookStatus.Reserved;
            await _context.SaveChangesAsync();
            return true;
        }

        private Task<Book> GetBookByIdAsync(Guid bookId) => _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);



    }
}
