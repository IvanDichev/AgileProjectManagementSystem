using AutoMapper;
using DataModels.Models.Project;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
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

                    await this.projectsService.CreateAsync(inputModel);
                    int id = projectsService.GetAll().Where(x => x.Name == inputModel.Name).FirstOrDefault().Id;
                    return RedirectToAction("Get", new { id = id });
                }
            }
            return View(inputModel);
        }

        [Route("Projects")]
        public IActionResult GetAll()
        {
            var model = mapper.Map<IEnumerable<ProjectViewModel>>(this.projectsService.GetAll());

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.projectsService.Delete(id);
            return RedirectToAction("GetAll", "Projects");
        }
    }
}
