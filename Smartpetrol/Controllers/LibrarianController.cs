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
        readonly IBooksProvider _booksProvider;
        public LibrarianController(IBooksProvider booksProvider)
        {
            _booksProvider = booksProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(CreateBookViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _booksProvider.CreateBook(model);
            return View("ShowMessage", new MessageModel("/Librarian/Index", "Книга успешно создана", false, secondsToRedirect: 1));
        }
    }
}