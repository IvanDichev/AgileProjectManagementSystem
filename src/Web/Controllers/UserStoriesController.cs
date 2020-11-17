using AutoMapper;
using DataModels.Models.UserStories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BacklogPriorities;
using Services.UserStories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;
        private readonly IBacklogPrioritiesService backlogPrioritiesService;
        private readonly IMapper mapper;

        public UserStoriesController(IUserStoriesService userStoriesService,
            IBacklogPrioritiesService backlogPrioritiesService,
            IMapper mapper)
        {
            this.userStoriesService = userStoriesService;
            this.backlogPrioritiesService = backlogPrioritiesService;
            this.mapper = mapper;
        }

        [Route("Projects/{projectId}/[controller]/")]
        public IActionResult Index(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!this.userStoriesService.IsUserInProject(projectId, userId))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            var all = userStoriesService.GetAll(projectId);

            return View(all);
        }

        public async Task<IActionResult> Create()
        {
            var createViewModel = new CreateUserStoryInputModel()
            {
                PrioritiesDropDown = this.mapper.Map<ICollection<BacklogPriorityDropDownModel>>
                (await this.backlogPrioritiesService.GetAllAsync())
            };
            ;  
            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserStoryInputModel inputModel)
        {
            var userStoryId = await  this.userStoriesService.CreateAsync(inputModel);

            return RedirectToAction("index");
        }
    }
}
