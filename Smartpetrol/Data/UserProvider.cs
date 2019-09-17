using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Models;

namespace Smartpetrol.Data
{
    public class UserProvider : IUserProvider
    {
        private readonly SmartDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserProvider(SmartDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                    RoleName = (await _userManager.GetRolesAsync(user)).SingleOrDefault(),
                });
            }
            return users;
        }
    }
}
