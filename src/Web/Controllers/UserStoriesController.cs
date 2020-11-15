using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.UserStories;

namespace Web.Controllers
{
    [Authorize]
    [Route("Projects")]
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;

        public UserStoriesController(IUserStoriesService userStoriesService)
        {
            this.userStoriesService = userStoriesService;
        }

        [Route("{projectId}/{controller}")]
        public IActionResult Index(int projectId)
        {

            var all = userStoriesService.GetAll(projectId);
            return View(all);
        }
    }
}
