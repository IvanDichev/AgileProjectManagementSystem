using AutoMapper;
using DataModels.Models.Project;
using Microsoft.AspNetCore.Mvc;
using Services.Projects;
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
        public async Task<IActionResult> Get(int id)
        {
            var project = mapper.Map<ProjectViewModel>(await this.projectsService.GetAsync(id));
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
    }
}
