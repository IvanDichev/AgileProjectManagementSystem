using AutoMapper;
using DataModels.Models.Board;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BoardColumns;
using Services.Projects;
using System.Collections.Generic;
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

        public async Task<IActionResult> Options(int projectId)
        {
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var alreadyBoard = await this.boardColumnsService.GetColumnsNamesPositionAsync(projectId);

            var board = new BoardOptionsInputModel()
            {
                AlreadyColumns = alreadyBoard,
            };

            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> AddColumn(int projectId, BoardOptionsInputModel inputModel)
        {
            if (!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            await this.boardColumnsService.AddcolumnToTheLeftAsync(inputModel);

            return RedirectToAction(nameof(Options), new { projectId = projectId });
        }
    }
}
