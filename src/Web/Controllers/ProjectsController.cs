using AutoMapper;
using DataModels.Models.Projects;
using DataModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System.Collections.Generic;
using System.Linq;
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
            IMapper mapper)
        {
            this.projectsService = projectsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Get(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(await this.projectsService.GetAsync(projectId));

            return View(project);
        }

        public async Task<IActionResult> GetAll(PaginationFilter paginationFilter)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var model = await this.projectsService.GetAllAsync(userId, paginationFilter);

            return View(model);
        }

        [NoDirectAccess]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectInputModel inputModel, PaginationFilter paginationFilter)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = await this.RenderViewAsStringAsync("Create", inputModel, true) });
            }

            if (!this.projectsService.IsNameTaken(inputModel.Name))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await this.projectsService.CreateAsync(inputModel, userId);
                var model = await this.projectsService.GetAllAsync(userId, paginationFilter);
                return Json(new { isValid = true, html = await this.RenderViewAsStringAsync("_ViewAll", model, false) });
            }

            ModelState.AddModelError("", $"The project '{inputModel.Name}' already exists.");

            return Json(new { isValid = false, html = await this.RenderViewAsStringAsync("Create", inputModel, true) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!IsUserInProject(userId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(await this.projectsService.GetAsync(id));

            return Json(new { html = await this.RenderViewAsStringAsync("Edit", project, false) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!IsUserInProject(userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.projectsService.EditAsync(model);

            return Json(new { isValid = true, newDescription = model.Description });
        }

        public async Task<IActionResult> Delete(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!IsUserInProject(userId))
            {
                return Unauthorized();
            }

            await this.projectsService.DeleteAsync(projectId);

            return RedirectToAction("GetAll", "Projects", new PaginationFilter());
        }

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private bool IsUserInProject(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return this.projectsService.IsUserInProject(projectId, userId);
        }
    }
}
