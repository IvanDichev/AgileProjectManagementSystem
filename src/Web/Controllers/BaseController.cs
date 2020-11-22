using Microsoft.AspNetCore.Mvc;
using Services.Projects;
using System.Security.Claims;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProjectsService projectsService;

        public BaseController() {   }

        public BaseController(IProjectsService projectsService)
        {
            this.projectsService = projectsService;
        }

        /// <summary>
        /// Check if project has relation to the project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        protected internal bool IsUserInProject(int projectId)
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return this.projectsService.IsUserInProject(projectId, userId);
        }
    }
}
