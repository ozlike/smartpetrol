using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Models;

namespace Smartpetrol.Data
{
    public class UserProvider : IUserProvider
    {
        readonly SmartDbContext _context;
        readonly UserManager<User> _userManager;
        readonly HttpContext _httpContext;
        readonly SignInManager<User> _signInManager;

        public UserProvider(SmartDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        
        public async Task<ICollection<UserModel>> GetAllUsers()
        {
            //TODO: Automapper
            List<UserModel> users = new List<UserModel>();
            foreach (var user in _context.Users.Select(x => x))
            {
                users.Add(new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.FirstName,
                    RoleFullNames = string.Join(", ", (await _userManager.GetRolesAsync(user))),
                });
            }
            return users;
        }



        private async Task<List<string>> GetRoles()
        {
            var user = await GetCurrentUserAsync();
            if(user == null) return  new List<string>();
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContext.User.Identity.IsAuthenticated;
            }
        }

        public Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(_httpContext.User);

        public async Task<bool> UserHasRole(RoleName role)
        {
            return (await GetRoles()).Any(x =>
                string.Equals(x, role.ToString()));
        }

        public async Task<IdentityResult> RegisterUser(RegisterUserViewModel model)
        {
            var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<SignInResult> LoginUser(LoginUserViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);
        }

        public Task Logout()
        {
            return _signInManager.SignOutAsync();
        }
    }
}