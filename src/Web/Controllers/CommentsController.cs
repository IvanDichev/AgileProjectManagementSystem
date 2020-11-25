using Microsoft.AspNetCore.Mvc;
using Services.Projects;

namespace Web.Controllers
{
    public class CommentsController : BaseController
    {
        public CommentsController(IProjectsService projectsService)
            : base(projectsService)
        {

        }

        public IActionResult Get(int porjectId, int commentId)
        {
            return View();
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
