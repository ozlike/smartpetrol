using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Smartpetrol.Models;

namespace Smartpetrol.Data
{
    public interface IUserProvider
    {
        Task<ICollection<UserModel>> GetAllUsers();
        Task<User> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
        Task<bool> UserHasRole(RoleName role);
        Task<IdentityResult> RegisterUser(RegisterUserViewModel model);
        Task<SignInResult> LoginUser(LoginUserViewModel model);
        Task Logout();
    }
}
