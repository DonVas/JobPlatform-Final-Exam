﻿namespace JobPlatform.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using JobPlatform.Common;
    using JobPlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.Administrator);
            await SeedRoleAsync(roleManager, GlobalConstants.Employer);
            await SeedRoleAsync(roleManager, GlobalConstants.Guest);
            await SeedRoleAsync(roleManager, GlobalConstants.Moderator);
            await SeedRoleAsync(roleManager, GlobalConstants.UserRole);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
