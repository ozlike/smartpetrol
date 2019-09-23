using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public BooksProvider(SmartDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BookViewModel>> GetAllBooksAsync()
        {
            //TODO: Automapper
            var books = new List<BookViewModel>();
            foreach (var book in _context.Books.Select(x => x))
            {
                books.Add(new BookViewModel
                {
                    Id = book.Id,
                    Genre = book.Genre,
                    Author = book.Author,
                    Publisher =  book.Publisher,
                    Title = book.Title,
                });
            }

            return books;
        }

        public async Task<BookViewModel> GetBookToEditAsync(Guid bookId)
        {
            if (bookId == Guid.Empty) return null;
            var book = await GetBookByIdAsync(bookId);

            var model = new BookViewModel
            {
                Id = book.Id,
                Genre = book.Genre,
                Author = book.Author,
                Publisher = book.Publisher,
                Title = book.Title,
            };

            return model;
        }

        public async Task<bool> EditBookAsync(BookViewModel model)
        {
            var book = await GetBookByIdAsync(model.Id);
            if (book == null) return false;
            book.Genre = model.Genre;
            book.Author = model.Author;
            book.Publisher = model.Publisher;
            book.Title = model.Title;
            await _context.SaveChangesAsync();
            return true;
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
            await _context.Books.AddAsync(new Book
            {
                Author = model.Author,
                Genre =  model.Genre,
                Title = model.Title,
                Publisher = model.Publisher,
            });
            await _context.SaveChangesAsync();
        }

    }
}
