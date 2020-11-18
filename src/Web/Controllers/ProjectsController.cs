using AutoMapper;
using DataModels.Models.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IActionResult Get(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(projectId));

            return View(project);
        }

        public IActionResult GetAll()
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var model = mapper.Map<IEnumerable<ProjectViewModel>>
                (this.projectsService.GetAll(userId));

            return View(model);
        }

        [NoDirectAccess]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = await this.RenderViewAsStringAsync("Create", inputModel, true) });
            }

            if (!this.projectsService.IsNameTaken(inputModel.Name))
            {
                var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await this.projectsService.CreateAsync(inputModel, userId);
                var all = mapper.Map<IEnumerable<ProjectViewModel>>(this.projectsService.GetAll(userId));
                return Json(new { isValid = true, html = await this.RenderViewAsStringAsync("_ViewAll", all, false) });
            }

            ModelState.AddModelError("", $"The project '{inputModel.Name}' already exists.");

            return Json(new { isValid = false, html = await this.RenderViewAsStringAsync("Create", inputModel, true) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsUserInProject(id))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(id));

            return Json(new { html = await this.RenderViewAsStringAsync("Edit", project, false) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (!IsUserInProject(model.Id))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.projectsService.Edit(model);

            return Json(new { isValid = true, newDescription = model.Description });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsUserInProject(id))
            {
                return Unauthorized();
            }

            await this.projectsService.Delete(id);

            return RedirectToAction("GetAll", "Projects");
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
