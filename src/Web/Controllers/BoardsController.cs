using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BoardColumns;
using Services.Projects;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class BoardsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IBoardColumnsService boardColumnsService;

        public BoardsController(IMapper mapper, 
            IProjectsService projectsService, 
            IBoardColumnsService boardColumnsService)
            : base(projectsService)
        {
            this.mapper = mapper;
            this.boardColumnsService = boardColumnsService;
        }

        public async Task<IActionResult> Board(int projectId)
        {
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var boardColumns = await this.boardColumnsService.GetAllColumnsAsync(projectId);

            return View(boardColumns);
        }
    }
}
