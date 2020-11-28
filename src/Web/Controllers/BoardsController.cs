using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class BoardsController : BaseController
    {
        public IActionResult Board()
        {
            return View();
        }
    }
}
