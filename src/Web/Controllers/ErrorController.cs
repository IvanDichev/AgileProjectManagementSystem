using DataModels.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using System.Diagnostics;

namespace Web.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["ErrorMessage"] = ErrorConstants.NotFoundMessage;
                    return View("NotFound");
                case 401:
                    ViewData["ErrorMessage"] = ErrorConstants.UnauthorizedMessage;
                    return View("Unauthorized");
                default: return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
