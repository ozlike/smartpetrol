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
        Task<List<string>> GetRoles();
        Task<IdentityUser> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
        Task<bool> UserHasRole(RoleName role);
    }
}
