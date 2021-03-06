﻿using JobPlatform.Services.Data.Interfaces;

namespace JobPlatform.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using JobPlatform.Data.Common.Repositories;
    using JobPlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class RolesService : IRolesService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {

            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task ChangeRoleAsync(string userId, string roleId, bool onOff)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var role = await this.roleManager.FindByIdAsync(roleId);

            if (onOff)
            {
                await this.userManager.AddToRoleAsync(user, role.Name);
            }
            else
            {
                await this.userManager.RemoveFromRoleAsync(user, role.Name);
            }
        }
    }
}
