using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System.Security.Claims;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProjectsService workItemService;

        public BaseController() {   }

        public BaseController(IProjectsService workItemService)
        {
            this.workItemService = workItemService;
        }

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        protected internal bool IsCurrentUserInProject(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return this.workItemService.IsUserInProject(projectId, userId);
        }
    }
}
