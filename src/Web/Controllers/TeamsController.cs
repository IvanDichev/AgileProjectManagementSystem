using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.TeamsServices;

namespace Web.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        public IActionResult GetAll()
        {
            var teams = this.teamsService.GetAllAsync();
            return View(teams);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team model)
        {

            return View();
        }
    }
}
