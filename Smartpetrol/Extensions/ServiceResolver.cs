using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartpetrol.Data;
using Smartpetrol.Data.Interfaces;
using Smartpetrol.Models;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Extensions
{
    public class ServiceResolver
    {
        private static WindsorContainer container;
        private static IServiceProvider serviceProvider;

        public ServiceResolver(IServiceCollection services)
        {
            container = new WindsorContainer();

            container.Register(Component.For<IUserProvider>().ImplementedBy<UserProvider>().LifestyleTransient());
            container.Register(Component.For<IBooksProvider>().ImplementedBy<BooksProvider>().LifestyleTransient());
            //container.Register(Component.For<IMapper>());

            serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, services);

            ADD_INIT_VALUES_INTO_DB_TEMPORARY_FUNC(
                serviceProvider.GetService<SmartDbContext>(),
                serviceProvider.GetService<RoleManager<IdentityRole>>(),
                serviceProvider.GetService<UserManager<User>>())
                .Wait();
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        private async Task ADD_INIT_VALUES_INTO_DB_TEMPORARY_FUNC(SmartDbContext smartDbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (!smartDbContext.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Roles.Librarian));
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));
            }

            if (!smartDbContext.Users.Any())
            {
                var email = "red@black.me";
                var admin = new User { UserName = email, Email = email };
                var result = await userManager.CreateAsync(admin, "Qwerty123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Client);
                    await userManager.AddToRoleAsync(admin, Roles.Librarian);
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
