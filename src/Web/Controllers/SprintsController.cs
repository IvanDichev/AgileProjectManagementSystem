using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class SprintsController : BaseController
    {
        public SprintsController()
        {

        }

        public IActionResult Sprints()
        {
            return View();
        }
    }
}
