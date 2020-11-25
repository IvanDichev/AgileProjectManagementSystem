using AutoMapper;
using DataModels.Models.Projects;
using DataModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
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
            var model = this.mapper.Map<PaginatedProjectViewModel>
                (await this.projectsService.GetAllAsync(userId, paginationFilter));

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
                var model = this.mapper.Map<PaginatedProjectViewModel>
                    (await this.projectsService.GetAllAsync(userId, paginationFilter));

                return Json(new { isValid = true, html = await this.RenderViewAsStringAsync("_ViewAll", model, false) });
            }

            ModelState.AddModelError("", $"The project '{inputModel.Name}' already exists.");

            return Json(new { isValid = false, html = await this.RenderViewAsStringAsync("Create", inputModel, true) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!IsUserInProject(userId))
            {
                return Unauthorized();
            }

            var project = mapper.Map<ProjectViewModel>(await this.projectsService.GetAsync(projectId));

            return Json(new { html = await this.RenderViewAsStringAsync("Edit", project) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectInputModel model)
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

            await this.projectsService.UpdateAsync(model);

            return Json(new { isValid = true, newDescription = model.Description });
        }

        [HttpPost]
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
    }
}
