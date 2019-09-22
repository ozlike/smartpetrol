using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartpetrol.Data;
using Smartpetrol.Models;

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
            return View(await _userProvider.GetAllUsers());
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userProvider.RegisterUser(model);
            if (result.Succeeded)
            {
                return View("ShowMessage", new MessageModel("/Admin/Index", "Пользователь успешно создан", false));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        [HttpGet]
        public IActionResult EditUser(string userId)
        {

            return View();
        }
    }
}