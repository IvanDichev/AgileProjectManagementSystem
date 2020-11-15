using AutoMapper;
using DataModels.Models.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
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

        public ProjectsController(IProjectsService projectsService, IMapper mapper)
        {
            this.projectsService = projectsService;
            this.mapper = mapper;
        }

        [Route("{controller}/{id}")]
        public IActionResult Get(int id)
        {
            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(id));

            return View(project);
        }

        [Route("{controller}")]
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
            if (ModelState.IsValid)
            {
                if (!this.projectsService.IsNameTaken(inputModel.Name))
                {
                    var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    await this.projectsService.CreateAsync(inputModel, userId);
                    var all = mapper.Map<IEnumerable<ProjectViewModel>>(this.projectsService.GetAll(userId));
                    return Json(new { isValid = true, html = await this.RenderAsync("_ViewAll", all, false)});
                }

                ModelState.AddModelError("", $"The project '{inputModel.Name}' already exists.");
            }
            return Json(new { isValid = false, html = await this.RenderAsync("Create", inputModel, true) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(id));
            return Json(new { html = await this.RenderAsync("Edit", project, false) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            await this.projectsService.Edit(model);
            return Json(new { isValid = true, newDescription = model.Description });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.projectsService.Delete(id);

            return RedirectToAction("GetAll", "Projects");
        }
    }
}
