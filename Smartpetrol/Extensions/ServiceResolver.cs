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

namespace Smartpetrol
{
    public class ServiceResolver
    {
        private static WindsorContainer container;
        private static IServiceProvider serviceProvider;

        public ServiceResolver(IServiceCollection services)
        {
            container = new WindsorContainer();

            //container.Register(Component.For<>().LifestyleSingleton());
            
            serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, services);

            ADD_INIT_VALUES_INTO_DB_TEMPORARY_FUNC(
                serviceProvider.GetService<SmartDbContext>(),
                serviceProvider.GetService<RoleManager<IdentityRole>>(),
                serviceProvider.GetService<UserManager<IdentityUser>>())
                .Wait();
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        private async Task ADD_INIT_VALUES_INTO_DB_TEMPORARY_FUNC(SmartDbContext smartDbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (smartDbContext.Roles.Count() == 0)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Roles.Librarian));
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));
            }

            if (smartDbContext.Users.Count() == 0)
            {
                var admin = new IdentityUser
                {
                    UserName = "Administrator",
                };
                await userManager.CreateAsync(admin);
                await userManager.AddPasswordAsync(admin, "Qwerty123!");
                await userManager.SetEmailAsync(admin, "red@black.me");
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
