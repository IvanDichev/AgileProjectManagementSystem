using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class EditorController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditorController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

