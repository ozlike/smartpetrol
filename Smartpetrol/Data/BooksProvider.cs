using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public async Task CreateBook(CreateBookViewModel model)
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
