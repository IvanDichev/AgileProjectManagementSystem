using AutoMapper;
using DataModels.Models.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Extentions;

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

        [Route("Projects/{id}")]
        public IActionResult Get(int id)
        {
            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(id));

            return View(project);
        }

        [Route("Projects")]
        public IActionResult GetAll()
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var model = mapper.Map<IEnumerable<ProjectViewModel>>
                (this.projectsService.GetAll(userId));

            return View(model);
        }

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
                    int id = await this.projectsService.CreateAsync(inputModel, userId);
                    var all = mapper.Map<IEnumerable<ProjectViewModel>>(this.projectsService.GetAll(userId));
                    var html = await this.RenderAsync("_ViewAll", all);
                    return Json(new { isValid = true, html});
                    //return RedirectToAction("Get", new { id = id });
                }

                ModelState.AddModelError("", $"The project '{inputModel.Name}' already exists.");
            }
            return Json(new { isValid = false, html = await this.RenderAsync("Create", inputModel) });
            //return View(inputModel);
        }

        public IActionResult Edit(int id)
        {
            var project = mapper.Map<ProjectViewModel>(this.projectsService.Get(id));

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            await this.projectsService.Edit(model);

            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.projectsService.Delete(id);

            return RedirectToAction("GetAll", "Projects");
        }
    }
}
