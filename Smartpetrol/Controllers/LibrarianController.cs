using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartpetrol.Data;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models;
using Smartpetrol.Models.Books;

namespace Smartpetrol.Controllers
{
    [Authorize(Roles = Roles.Librarian)]
    public class LibrarianController : Controller
    {
        readonly IBookProvider _booksProvider;
        public LibrarianController(IBookProvider booksProvider)
        {
            _booksProvider = booksProvider;
        }

        public async Task<IActionResult>Index()
        {
            return View(await _booksProvider.GetAllBooksAsync());
        }

        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(BookViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _booksProvider.CreateBookAsync(model);
            return View("ShowMessage",
                new MessageModel("/Librarian/Index", "Книга успешно добавлена", false, 1));
        }

        public async Task<IActionResult> BookDelivery()
        {
            return View(await _booksProvider.GetReservedBooksAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GiveOutBook(Guid bookId)
        {
            return View(await _booksProvider.GetBookToGiveOutAsync(bookId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveOutBook(BookViewModel model)
        {
            var success = await _booksProvider.GiveOutBookAsync(model.Id);
            if (!success) return View("ShowMessage", new MessageModel("/Librarian/BookDelivery", "Ошибка при выдаче книги", true));
            return View("ShowMessage", new MessageModel("/Librarian/BookDelivery", "Книга успешно выдана", false, 1));
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(Guid bookId)
        {
            return View(await _booksProvider.GetBookToManipulateAsync(bookId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(BookViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _booksProvider.EditBookAsync(model);
            if (!success) return View(model);
            return View("ShowMessage", new MessageModel("/Librarian/Index", "Книга успешно обновлена", false, 1));
        }
        
        public async Task<IActionResult>DeleteBook(Guid bookId)
        {
            var success = await _booksProvider.DeleteBookAsync(bookId);
            if (success) return View("ShowMessage", new MessageModel("/Librarian/Index", "Книга успешно удалена", false, 1));
            return View("ShowMessage", new MessageModel("/Librarian/Index", "Произошла ошибки при удалении", true, 1));
        }

        public async Task<IActionResult> RentedBooks()
        {
            return View(await _booksProvider.GetRentedBooksAsync());
        }

        [HttpGet]
        public async Task<IActionResult> TakeBook(Guid bookId)
        {
            return View(await _booksProvider.GetBookToTakeFromUserAsync(bookId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeBook(BookViewModel model)
        {
            var success = await _booksProvider.TakeBookFromUserAsync(model.Id);
            if (!success) return View("ShowMessage", new MessageModel("/Librarian/RentedBooks", "Ошибка при получении книги", true, 2));
            return View("ShowMessage", new MessageModel("/Librarian/RentedBooks", "Книга успешно получена", false, 1));
        }
    }
}