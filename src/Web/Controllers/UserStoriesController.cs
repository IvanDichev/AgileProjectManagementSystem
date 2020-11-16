using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.UserStories;
using System.Net;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class UserStoriesController : BaseController
    {
        private readonly IUserStoriesService userStoriesService;

        public UserStoriesController(IUserStoriesService userStoriesService)
        {
            this.userStoriesService = userStoriesService;
        }

        [Route("Projects/{projectId}/{controller}/")]
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


    }
}
