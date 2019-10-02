using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartpetrol.Configuration;
using Smartpetrol.Data;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models;
using Smartpetrol.Models.Books;

namespace Smartpetrol.Controllers
{
    [Authorize(Roles = Roles.Client)]
    public class ClientController : Controller
    {
        readonly IClientProvider _clientProvider;
        readonly IBookProvider _bookProvider;
        public ClientController(IBookProvider bookProvider, IClientProvider clientProvider)
        {
            _bookProvider = bookProvider;
            _clientProvider = clientProvider;
        }

        public IActionResult Index()
        {
            return View(_bookProvider.GetBooksToReserve());
        }


        [HttpGet]
        public async Task<IActionResult> ReserveBook(Guid bookId)
        {
            return View(await _bookProvider.GetBookToManipulateAsync(bookId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveBook(BookViewModel model)
        {
            var success = await _clientProvider.ReserveBookAsync(model.Id);
            if (!success) return View("ShowMessage", new MessageModel("/Client/Index", "Произошла ошибка при бронировании книги", true));
            return View("ShowMessage", new MessageModel("/Client/ReservedBooks", $"Книга успешно забронирована (на {GlobalValues.ReservationHours} час.)"));
        }

        public async Task<IActionResult> ReservedBooks()
        {
            return View(await _clientProvider.GetReservedBooksAsync());
        }

        public async Task<IActionResult> CancelReservation(Guid bookId)
        {
            var success = await _clientProvider.CancelReservation(bookId);
            if (!success) return View("ShowMessage", new MessageModel("/Client/ReservedBooks", "Произошла ошибка при отмене бронирования", true));
            return View("ShowMessage", new MessageModel("/Client/ReservedBooks", "Бронирование книги успешно отменено"));
        }

        public async Task<IActionResult> RentedBooks()
        {
            return View(await _clientProvider.GetRentedBooksAsync());
        }
    }
}