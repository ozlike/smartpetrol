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
    }
}
