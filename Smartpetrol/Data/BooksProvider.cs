using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models.Books;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Data
{
    public class BooksProvider : IBooksProvider
    {
        readonly SmartDbContext _context;
        readonly IMapper _mapper;

        public BooksProvider(SmartDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<BookViewModel>> GetAllBooksAsync()
        {
            return await _context.Books.Select(x => _mapper.Map<BookViewModel>(x)).ToListAsync();
        }

        public async Task<BookViewModel> GetBookToEditAsync(Guid bookId)
        {
            if (bookId == Guid.Empty) return null;
            return _mapper.Map<BookViewModel>(await GetBookByIdAsync(bookId));
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

    }
}
