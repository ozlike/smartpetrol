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
        readonly UserManager<IdentityUser> _userManager;
        readonly HttpContext _httpContext;

        public UserProvider(SmartDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
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
                    UserName = user.UserName,
                    RoleFullNames = string.Join(", ", (await _userManager.GetRolesAsync(user))),
                });
            }
            return users;
        }



        public async Task<List<string>> GetRoles()
        {
            return (await _userManager.GetRolesAsync(await GetCurrentUserAsync())).ToList();
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContext.User.Identity.IsAuthenticated;
            }
        }

        public Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(_httpContext.User);

        public async Task<bool> UserHasRole(RoleName role)
        {
            return (await GetRoles()).Any(x =>
                string.Equals(x, role.ToString()));
        }
    }
}
