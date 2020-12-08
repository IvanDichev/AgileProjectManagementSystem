using AutoMapper;
using DataModels.Models.Board;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BoardColumns;
using Services.Projects;
using Services.Sprints;
using Services.WorkItems.UserStories;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class BoardsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IBoardsService boardColumnsService;
        private readonly ISprintsService sprintsService;
        private readonly IUserStoryService userStoryService;

        public BoardsController(IMapper mapper, 
            IProjectsService projectsService, 
            IBoardsService boardColumnsService,
            ISprintsService sprintsService,
            IUserStoryService userStoryService)
            : base(projectsService)
        {
            this.mapper = mapper;
            this.boardColumnsService = boardColumnsService;
            this.sprintsService = sprintsService;
            this.userStoryService = userStoryService;
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

        [HttpPost]
        public async Task<IActionResult> ChangeColumn(int columnId, int userStoryId)
        {
            await this.userStoryService.ChangeColumnAsync(userStoryId, columnId);

            return Json(new { changed = "changed" });
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
