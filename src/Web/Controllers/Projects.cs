using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class Projects : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
