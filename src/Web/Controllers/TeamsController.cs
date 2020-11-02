using Data.Models;
using DataModels.Models.Teams;
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

        [Route("Teams")]
        public IActionResult GetAll()
        {
            var teams = this.teamsService.GetAllAsync();
            return View(teams);
        }

        [Route("Teams/{id}")]
        public IActionResult Get(int id)
        {
            var teams = this.teamsService.Get(id);
            return View(teams);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTeamInputModel inputModel)
        {
            this.teamsService.CreateAsync(inputModel);
            return View();
        }
    }
}
