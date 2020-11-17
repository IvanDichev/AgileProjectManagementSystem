using AutoMapper;
using DataModels.Models.UserStories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.Projects;
using Services.UserStories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    [Route("Projects/{projectId}/[controller]")]
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IProjectsService projectsService;
        private readonly IMapper mapper;

        public UserStoriesController(IUserStoriesService userStoriesService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IProjectsService projectsService,
            IMapper mapper)
        {
            this.userStoriesService = userStoriesService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.projectsService = projectsService;
            this.mapper = mapper;
        }

        [Route("")]
        public IActionResult Index(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            var all = userStoriesService.GetAll(projectId);

            return View(all);
        }

        [Route("Create")]
        public async Task<IActionResult> Create(int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            var createViewModel = new CreateUserStoryInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                (await this.backlogPrioritiesService.GetAllAsync())
            };

            return View(createViewModel);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateUserStoryInputModel inputModel, int projectId)
        {
            if (!IsUserInProject(projectId))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!this.userStoriesService.IsUserInProject(projectId, userId))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            await  this.userStoriesService.CreateAsync(inputModel);

            return RedirectToAction("Index");
        }

        private bool IsUserInProject(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return this.projectsService.IsUserInProject(projectId, userId);
        }
    }
}
