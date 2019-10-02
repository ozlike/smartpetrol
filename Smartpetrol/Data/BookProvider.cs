using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Configuration;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models.Books;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Data
{
    public class BookProvider : IBookProvider
    {
        readonly SmartDbContext _context;
        readonly IMapper _mapper;

        public BookProvider(SmartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<BookViewModel>> GetAllBooksAsync()
        {
            var books = await _context.Books.OrderBy(x => x.Status)
                .ThenBy(x=>x.Title).ToListAsync();
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public async Task<BookViewModel> GetBookToManipulateAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book.Status != BookStatus.Free) throw new Exception();
            return _mapper.Map<BookViewModel>(book);
        }
        
        public List<BookViewModel> GetBooksToReserve()
        {
            var books = _context.Books.Where(x => x.Status == BookStatus.Free).ToList();
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public async Task<bool> EditBookAsync(BookViewModel model)
        {
            try
            {
                _context.Update(_mapper.Map<Book>(model));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e) { }

            return false;
        }

        public async Task<bool> DeleteBookAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book == null) return false;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        private Task<Book> GetBookByIdAsync(Guid bookId) => _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

        public async Task CreateBookAsync(BookViewModel model)
        {
            await _context.Books.AddAsync(_mapper.Map<Book>(model));
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookViewModel>> GetReservedBooksAsync()
        {
            return _mapper.Map<List<BookViewModel>>(await _context.Books.Where(x => x.Status == BookStatus.Reserved).OrderBy(x => x.Title).ToListAsync());
        }

        public async Task<BookViewModel> GetBookToGiveOutAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book.Status != BookStatus.Reserved) throw new Exception();
            return _mapper.Map<BookViewModel>(book);
        }

        public async Task<bool> GiveOutBookAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book == null || book.Status != BookStatus.Reserved || 
                book.ReservationTime.Value.AddHours(GlobalValues.ReservationHours) < DateTime.UtcNow) return false;
            book.Status = BookStatus.Rented;
            book.RentalTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<BookViewModel>> GetRentedBooksAsync()
        {
            var books = await _context.Books.Where(x => x.Status == BookStatus.Rented).ToListAsync();
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public async Task<BookViewModel> GetBookToTakeFromUserAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book.Status != BookStatus.Rented) return null;
            return _mapper.Map<BookViewModel>(book);
        }

        public async Task<bool> TakeBookFromUserAsync(Guid bookId)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book.Status != BookStatus.Rented) return false;

            book.Status = BookStatus.Free;
            book.Tenant = null;
            book.TenantId = null;
            book.RentalTime = null;
            book.ReservationTime = null;
            await _context.SaveChangesAsync(true);
            return true;
        }
    }
}
