using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using System.Diagnostics;
using Web.Models.Error;

namespace Web.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

		[Route("/Error/HttpStatusCodeHandler/{statusCode}")]
		public IActionResult HttpStatusCodeHandler(string statusCode)
		{
			switch (statusCode)
			{
				case "404":
					ViewData["ErrorMessage"] = ErrorConstants.NotFoundMessage;
					return View("NotFound");
			}

			return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult ErrorHandler()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
