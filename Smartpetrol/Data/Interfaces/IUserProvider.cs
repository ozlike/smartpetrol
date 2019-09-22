using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Smartpetrol.Models;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Data.Interfaces
{
    public interface IUserProvider
    {
        Task<ICollection<UserModel>> GetAllUsersAsync();
        bool IsAuthenticated { get; }
        Task<bool> UserHasRoleAsync(RoleName role);
        Task<IdentityResult> RegisterUserAsync(RegisterUserViewModel model);
        Task<SignInResult> LoginUserAsync(LoginUserViewModel model);
        Task LogoutAsync();
        Task<User> GetCurrentUserAsync();
        Task<EditUserViewModel> GetUserToEditAsync(string userId);
        Task<IdentityResult> EditUserAsync(EditUserViewModel model);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}
