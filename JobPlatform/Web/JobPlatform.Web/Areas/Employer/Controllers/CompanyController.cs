﻿namespace JobPlatform.Web.Areas.Employer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JobPlatform.Common;
    using JobPlatform.Data.Models;
    using JobPlatform.Services.Data.Interfaces;
    using JobPlatform.Web.Controllers;
    using JobPlatform.Web.ViewModels.Companies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.Employer)]
    [Area("Employer")]
    public class CompanyController : BaseController
    {
            private readonly IFileService fileService;
            private readonly ICompanyService companyService;
            private readonly UserManager<ApplicationUser> userManager;

            public CompanyController(
                IFileService fileService,
                ICompanyService companyService,
                UserManager<ApplicationUser> userManager)
            {
                this.fileService = fileService;
                this.companyService = companyService;
                this.userManager = userManager;
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.companyService.CompanyByUserId<CompanySimpleViewModel>(user.Id);

            return this.View(viewModel);
            }

            public IActionResult Details(string id)
            {
                var viewModel = this.companyService.CompanyById<CompanyDetailsViewModel>(id);

                return this.View(viewModel);
            }

            public IActionResult CreateCompany()
            {
                return this.View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateCompany(CompanyCreateViewModel input)
            {
                var user = await this.userManager.GetUserAsync(this.User);

                if (!this.ModelState.IsValid)
                {
                    return this.View(input);
                }

                var img = await this.fileService.UploadImageFileAsync(input.PictureFile, user.Id, "CompanyLogo");

                int result = this.companyService.AddCompany(
                    input.CompanyName,
                    input.SanitizedCompanyDescription,
                    input?.CompanyWebsite,
                    input?.FacebookWebsite,
                    input?.TwitterWebsite,
                    input?.LinkedInWebsite,
                    img?.SecureUri.ToString(),
                    user.Id).Result;

                if (result < 0)
                {
                    return this.View(input);
                }

                return this.RedirectToAction("Index");
            }

            [HttpDelete]
            public IActionResult DeleteCompany()
            {
                var viewModel = new List<CompanySimpleViewModel>();

                return this.View(viewModel);
            }

            [HttpPatch]
            public IActionResult EditCompany()
            {
                var viewModel = new List<CompanySimpleViewModel>();

                return this.View(viewModel);
            }
        }
}