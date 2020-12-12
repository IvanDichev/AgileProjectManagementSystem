﻿using AutoMapper;
using DataModels.Models.Board;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.BoardColumns;
using Services.Projects;
using Services.Sprints;
using Services.WorkItems.Tasks;
using Services.WorkItems.Tests;
using Services.WorkItems.UserStories;
using System.Collections.Generic;
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
        private readonly ITestsService testsService;
        private readonly ITasksService tasksService;

        public BoardsController(IMapper mapper,
            IProjectsService projectsService,
            IBoardsService boardColumnsService,
            ISprintsService sprintsService,
            IUserStoryService userStoryService,
            ITestsService testsService,
            ITasksService tasksService)
            : base(projectsService)
        {
            this.mapper = mapper;
            this.boardColumnsService = boardColumnsService;
            this.sprintsService = sprintsService;
            this.userStoryService = userStoryService;
            this.testsService = testsService;
            this.tasksService = tasksService;
        }

        public async Task<IActionResult> Board(int projectId, int sprintId)
        {
            if (!this.IsCurrentUserInProject(projectId))
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
        public async Task<IActionResult> ChangeColumn(int columnId, int itemId, bool isUserStory, bool isTask, bool isTest)
        {
            if (isUserStory)
            {
                await this.userStoryService.ChangeColumnAsync(itemId, columnId);
            }
            else if (isTask)
            {
                await this.tasksService.ChangeColumnAsync(itemId, columnId);
            }
            else if (isTest)
            {
                await this.testsService.ChangeColumnAsync(itemId, columnId);
            }

            return Json(new { changed = "changed" });
        }

        public async Task<IActionResult> Options(int projectId)
        {
            if (!this.IsCurrentUserInProject(projectId))
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

        public IActionResult Burndown()
        {
            return View();
        }

        public async Task<IActionResult> GetBurndownData(int projectId, int sprintId)
        {
            var viewModel = await this.boardColumnsService.GetBurndownData(projectId, sprintId);
            
            return Json( new { DaysInSprint = viewModel.DaysInSprint, ScopeChanges = viewModel.ScopeChanges, TasksRemaining = viewModel.TasksRemaining });
        }
    }
}
