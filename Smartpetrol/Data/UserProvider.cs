using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartpetrol.Extensions;
using Smartpetrol.Models;

namespace Smartpetrol.Data
{
    public class UserProvider : IUserProvider
    {
        readonly SmartDbContext _context;
        readonly UserManager<User> _userManager;
        readonly HttpContext _httpContext;
        readonly SignInManager<User> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public UserProvider(SmartDbContext context, SignInManager<User> signInManager, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContext = httpContextAccessor.HttpContext;
        }


        public async Task<ICollection<UserModel>> GetAllUsersAsync()
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


        private async Task<List<string>> GetRolesAsync()
        {
            return await GetRolesAsync(await GetCurrentUserAsync());
        }

        private async Task<List<string>> GetRolesAsync(User user)
        {
            if (user == null) return new List<string>();
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public bool IsAuthenticated
        {
            get { return _httpContext.User.Identity.IsAuthenticated; }
        }

        public Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(_httpContext.User);
        private Task<User> GetUserByIdAsync(string userId) => _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

        public async Task<bool> UserHasRoleAsync(RoleName role)
        {
            return await UserHasRoleAsync(await GetCurrentUserAsync(), role);
        }

        private async Task<bool> UserHasRoleAsync(User user, RoleName role)
        {
            return (await GetRolesAsync(user)).Any(x =>
                string.Equals(x, role.ToString()));
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserViewModel model)
        {
            var user = new User {UserName = model.Email, Email = model.Email, FirstName = model.FirstName};

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return result;

            foreach (var role in model.RolesList.Roles)
            {
                if (role.Selected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName.ToString());
                }
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(LoginUserViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);
        }

        public Task LogoutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public async Task<EditUserViewModel> GetUserToEditAsync(string userId)
        {
            if (userId == null) return null;
            var user = await GetUserByIdAsync(userId);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
            };

            var roles = await GetRolesAsync(user);
            model.RolesList.Roles.ForEach(x => x.Selected = roles.Contains(x.RoleName.ToString()));

            return model;
        }

        public async Task<IdentityResult> EditUserAsync(EditUserViewModel model)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            var user = await GetUserByIdAsync(model.Id);
            results.Add(await UpdateFirstNameAsync(user, model.FirstName));
            results.AddRange(await UpdateEmailAsync(user, model.Email));
            results.AddRange(await UpdatePasswordAsync(user, model.Password));
            results.AddRange(await UpdateRolesAsync(user, model.RolesList.Roles));
            return MergeResult(results);
        }

        private IdentityResult MergeResult(List<IdentityResult> results)
        {
            IdentityResult badResult = null;
            foreach (var result in results)
            {
                if (!result.Succeeded)
                {
                    if (badResult == null) badResult = new IdentityResult();
                    foreach (var error in result.Errors)
                        badResult.Errors.Append(error);
                }
            }

            if (badResult != null) return badResult;
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> UpdateFirstNameAsync(User user, string newFirstName)
        {
            if (!string.Equals(user.FirstName, newFirstName))
            {
                user.FirstName = newFirstName;
                return await _userManager.UpdateAsync(user);
            }

            return IdentityResult.Success;
        }

        private async Task<List<IdentityResult>> UpdateEmailAsync(User user, string newEmail)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            if (!string.Equals(user.Email, newEmail, StringComparison.CurrentCultureIgnoreCase))
            {
                results.Add(await _userManager.SetEmailAsync(user, newEmail));
                if (!results.Last().Succeeded) return results;
                await _userManager.UpdateNormalizedEmailAsync(user);

                results.Add(await _userManager.SetUserNameAsync(user, newEmail));
                if (!results.Last().Succeeded) return results;
                await _userManager.UpdateNormalizedUserNameAsync(user);

                results.Add(await _userManager.UpdateSecurityStampAsync(user));
            }

            return results;
        }

        private async Task<List<IdentityResult>> UpdatePasswordAsync(User user, string newPassword)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            if (!string.IsNullOrEmpty(newPassword))
            {
                var passwordValidator = new PasswordValidator<User>();
                results.Add(await passwordValidator.ValidateAsync(_userManager, null, newPassword));
                if (!results.Last().Succeeded) return results;

                results.Add(await _userManager.RemovePasswordAsync(user));
                if (!results.Last().Succeeded) return results;
                results.Add(await _userManager.AddPasswordAsync(user, newPassword));

                results.Add(await _userManager.UpdateSecurityStampAsync(user));
            }

            return results;
        }

        private async Task<List<IdentityResult>> UpdateRolesAsync(User user, List<RoleSelection> newRoles)
        {
            var userRole = (await GetRolesAsync(user)).ToEnumList<RoleName>();
            var isLastAdmin = await IsLastAdminAsync(user);
            List<IdentityResult> results = new List<IdentityResult>();
            foreach (var roleSelection in newRoles)
            {
                if (userRole.Contains(roleSelection.RoleName))
                {
                    if (!roleSelection.Selected)
                    {
                        if (isLastAdmin && roleSelection.RoleName == RoleName.Admin)
                            continue;
                        results.Add(await _userManager.RemoveFromRoleAsync(user, roleSelection.RoleName.ToString()));
                    }
                }
                else
                {
                    if (roleSelection.Selected)
                    {
                        results.Add(await _userManager.AddToRoleAsync(user, roleSelection.RoleName.ToString()));
                    }
                }
            }

            if (results.Count != 0)
            {
                results.Add(await _userManager.UpdateSecurityStampAsync(user));
            }

            return results;
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            var user = await GetUserByIdAsync(userId);
            if (await IsLastAdminAsync(user)) return IdentityResult.Failed();

            var userRoles = await _userManager.GetRolesAsync(user);

            using (var transaction = _context.Database.BeginTransaction())
            {
                foreach (var role in userRoles)
                {
                    results.Add(await _userManager.RemoveFromRoleAsync(user, role));
                }

                results.Add(await _userManager.DeleteAsync(user));
                transaction.Commit();
            }

            return MergeResult(results);
        }

        private async Task<bool> IsLastAdminAsync(User user)
        {
            var isAdmin = await UserHasRoleAsync(user, RoleName.Admin);
            if (!isAdmin) return false;
            var adminRole = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Name == RoleName.Admin.ToString());
            return await _context.UserRoles.CountAsync(x => string.Equals(x.RoleId, adminRole.Id)) == 1;
        }
    }
}