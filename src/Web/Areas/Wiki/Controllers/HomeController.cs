using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

namespace Web.Areas.Wiki.Controllers
{
    [Area("Wiki")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
