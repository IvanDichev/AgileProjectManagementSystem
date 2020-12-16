using AutoMapper;
using DataModels.Models.Error;
using DataModels.Models.Projects;
using DataModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using Shared.Constants;
using System;
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

        public ProjectsController(IProjectsService projectsService,
            IMapper mapper) : base(projectsService)
        {
            this.projectsService = projectsService;
            this.mapper = mapper;
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
