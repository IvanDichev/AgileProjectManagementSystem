using AutoMapper;
using DataModels.Models.Error;
using DataModels.Models.Projects;
using DataModels.Models.Users;
using DataModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using Services.Users;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Extentions;
using Web.Helpers;

namespace Web.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly IProjectsService projectsService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;

        public ProjectsController(IProjectsService projectsService,
            IMapper mapper,
            IUsersService usersService) : base(projectsService)
        {
            this.projectsService = projectsService;
            this.mapper = mapper;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Get(int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(await this.projectsService.GetAsync(projectId));

            return View(project);
        }

        public async Task<IActionResult> AddUserToProject(int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var AddUserToProjectInputModel = new AddUserToProjectInputModel
            {
                UsersDropdown = this.mapper.Map<ICollection<UsersDropdown>>(await this.usersService.GetPublicUsersAsync()),
            };

            return View(AddUserToProjectInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToProject(AddUserToProjectInputModel inputModel, int projectId)
        {
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var AddUserToProjectInputModel = new AddUserToProjectInputModel
            {
                UsersDropdown = this.mapper.Map<ICollection<UsersDropdown>>(await this.usersService.GetPublicUsersAsync()),
            };

            return View(AddUserToProjectInputModel);
        }

        public async Task<IActionResult> GetAll(PaginationFilter paginationFilter)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var paginatedViewModel = this.mapper.Map<PaginatedProjectViewModel>
                (await this.projectsService.GetAllAsync(userId, paginationFilter));

            return View(paginatedViewModel);
        }

        [NoDirectAccess]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectInputModel createInputModel, PaginationFilter paginationFilter)
        {
            bool isValid;
            string responseHtml;

            if (!ModelState.IsValid)
            {
                isValid = false;
                responseHtml = await this.RenderViewAsStringAsync("Create", createInputModel, true);
                return Json(new { isValid = isValid, html = responseHtml });
            }

            if (this.projectsService.IsNameTaken(createInputModel.Name))
            {
                ModelState.AddModelError(string.Empty, $"The project '{createInputModel.Name}' already exists.");
                isValid = false;
                responseHtml = await this.RenderViewAsStringAsync("Create", createInputModel, true);
                return Json(new { isValid = isValid, html = responseHtml });
            }

            try
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await this.projectsService.CreateAsync(createInputModel, userId);

                var responseModel = this.mapper.Map<PaginatedProjectViewModel>
                    (await this.projectsService.GetAllAsync(userId, paginationFilter));

                isValid = true;
                responseHtml = await this.RenderViewAsStringAsync("_ViewAll", responseModel, false);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, ErrorConstants.ContactSupportMessage);
                isValid = false;
                responseHtml = await this.RenderViewAsStringAsync("Create", createInputModel, true);
            }

            return Json(new { isValid = isValid, html = responseHtml });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int projectId)
        {
            // If projectId does not exist or user has no ref to the project.
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<EditProjectInputModel>(await this.projectsService.GetAsync(projectId));
            var responseHtml = await this.RenderViewAsStringAsync("Edit", project);

            return Json(new { html = responseHtml });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectInputModel editModel)
        {
            bool isValid;

            // If projectId does not exist or user has no ref to the project.
            if (!IsCurrentUserInProject(editModel.ProjectId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                isValid = false;
                var responseHtml = await this.RenderViewAsStringAsync("Edit", editModel, true);
                return Json(new { isValid = isValid, html = responseHtml });
            }

            try
            {
                await this.projectsService.UpdateAsync(editModel);
                isValid = true;

                return Json(new { isValid = isValid, newDescription = editModel.Description });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, ErrorConstants.ContactSupportMessage);
                isValid = false;

                return Json(new { isValid = isValid, newDescription = editModel.Description });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int projectId)
        {
            // If projectId does not exist or user has no ref to the project.
            if (!IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            try
            {
                await this.projectsService.DeleteAsync(projectId);
                return RedirectToAction(nameof(GetAll), new PaginationFilter());
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}
