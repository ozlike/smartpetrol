using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartpetrol.Data;
using Smartpetrol.Models;

namespace Smartpetrol.Controllers
{
    public class HomeController : Controller
    {
        readonly IUserProvider _userProvider;
        public HomeController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userProvider.GetCurrentUserAsync());
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
