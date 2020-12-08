using AutoMapper;
using DataModels.Models.Board;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BoardColumns;
using Services.Projects;
using Services.Sprints;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class BoardsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IBoardColumnsService boardColumnsService;
        private readonly ISprintsService sprintsService;

        public BoardsController(IMapper mapper, 
            IProjectsService projectsService, 
            IBoardColumnsService boardColumnsService,
            ISprintsService sprintsService)
            : base(projectsService)
        {
            this.mapper = mapper;
            this.boardColumnsService = boardColumnsService;
            this.sprintsService = sprintsService;
        }

        public async Task<IActionResult> Board(int projectId, int sprintId)
        {
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var boardViewModel = new BoardColumnAllViewModel()
            {
                BoardColumnAllDto = await this.boardColumnsService.GetAllColumnsAsync(projectId, sprintId),
                SprintDropDown = await this.sprintsService.GetSprintDropDownAsync(projectId),
            };

            return View(boardViewModel);
        }

        public async Task<IActionResult> Options(int projectId)
        {
            if(!this.IsCurrentUserInProject(projectId))
            {
                return Unauthorized();
            }

            var alreadyBoard = await this.boardColumnsService.GetColumnOptionsAsync(projectId);

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
