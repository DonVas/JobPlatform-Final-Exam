﻿namespace JobPlatform.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using JobPlatform.Services.Data.Interfaces;
    using JobPlatform.Web.ViewModels.Administration.Dashboard;
    using JobPlatform.Web.ViewModels.Administration.Dashboard.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Users()
        {
            var viewModel = new UsersViewModel
            {
                Users = await this.userService.GetAllUsersAsync(),
                Roles = this.userService.GetAllRoles<RoleViewModel>(),
            };

            if (viewModel.Users == null && viewModel.Roles == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult UserById(string id)
        {
            var viewModel = this.userService
                .GetAllUsers<SelectedUserViewModel>()
                .FirstOrDefault(x => x.Id == id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

    }
}
