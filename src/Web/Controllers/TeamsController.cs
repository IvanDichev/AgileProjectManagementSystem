using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TeamsController : BaseController
    {
        //private readonly ITeamsService teamsService;

        public TeamsController()
        {
        }

        [Route("Teams")]
        public IActionResult GetAll()
        {
            //var teams = this.teamsService.GetAllAsync();
            return View();
        }

        [Route("Teams/{id}")]
        public IActionResult Get(int id)
        {
            //var teams = this.teamsService.Get(id);
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create()
        //{
        //    //this.teamsService.CreateAsync(inputModel);
        //    return View();
        //}
    }
}
