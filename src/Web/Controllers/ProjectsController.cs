using AutoMapper;
using DataModels.Models.Error;
using DataModels.Models.Projects;
using DataModels.Models.TeamRoles;
using DataModels.Models.Users;
using DataModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using Services.TeamRoles;
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
        private readonly ITeamRolesService teamRolesService;

        public ProjectsController(IProjectsService projectsService,
            IMapper mapper,
            ITeamRolesService teamRolesService) : base(projectsService)
        {
            this.projectsService = projectsService;
            this.mapper = mapper;
            this.teamRolesService = teamRolesService;
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
                UsersDropdown = this.mapper.Map<ICollection<UsersDropdown>>(await this.projectsService.GetUsersDropDown(projectId)),
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

            if (!ModelState.IsValid)
            {
                var AddUserToProjectInputModel = new AddUserToProjectInputModel
                {
                    UsersDropdown = this.mapper.Map<ICollection<UsersDropdown>>(await this.projectsService.GetUsersDropDown(projectId)),
                };
            }

            await this.projectsService.AddUserToProject(inputModel.UserId, projectId);

            return RedirectToAction(nameof(Get), new { projectId = projectId });
        }

        public async Task<IActionResult> GetAll(PaginationFilter paginationFilter)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var paginatedViewModel = this.mapper.Map<PaginatedProjectViewModel>
                (await this.projectsService.GetAllAsync(userId, paginationFilter));

            return View(paginatedViewModel);
        }

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

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromProject(int userId, int projectId)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            if (await this.projectsService.IsLastUserInProjectAsync(userId, projectId))
            {
                TempData["RemoveError"] = "Last user in project cannot be removed!";

                return RedirectToAction(nameof(Get), new { projectId = projectId });
            }

            try
            {
                await this.projectsService.RemoveUserFromProjectAsync(userId, projectId);
                TempData["RemoveSuccess"] = "You removed " + this.User.FindFirstValue(ClaimTypes.Email) + " from project";

                return RedirectToAction(nameof(Get), new { projectId = projectId });
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}
