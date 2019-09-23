using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartpetrol.Data;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IUserProvider _userProvider;
        public AdminController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userProvider.GetAllUsersAsync());
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userProvider.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return View("ShowMessage", new MessageModel("/Admin/Index", "Пользователь успешно создан", false));
            }

            foreach (var err in result.Errors) ModelState.AddModelError("", err.Description);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            return View(await _userProvider.GetUserToEditAsync(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userProvider.EditUserAsync(model);
            if (result.Succeeded)
            {
                return View("ShowMessage", new MessageModel("/Admin/Index", "Пользователь успешно обновлен", false));
            }

            foreach (var err in result.Errors) ModelState.AddModelError("", err.Description);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userProvider.DeleteUserAsync(userId);
            if (result.Succeeded)
            {
                return View("ShowMessage", new MessageModel("/Admin/Index", "Пользователь успешно удалён", false));
            }

            return View("ShowMessage", new MessageModel("/Admin/Index", "Произошла ошибки при удалении", true));
        }
    }
}