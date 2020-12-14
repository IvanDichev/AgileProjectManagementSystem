using Data.Models;
using DataModels.Models.WorkItems.Tasks.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Tasks
{
    public class TasksService : ITasksService
    {
        private readonly IRepository<UserStoryTask> taskRepo;
        private readonly IProjectsService projectsService;
        private readonly IBoardsService boardService;
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IRepository<KanbanBoardColumn> boardRepo;
        private readonly IRepository<BurndownData> burndownRepo;

        public TasksService(IRepository<UserStoryTask> repo, 
            IProjectsService projectsService, 
            IBoardsService boardService,
            IRepository<UserStory> userStoryRepo,
            IRepository<KanbanBoardColumn> boardRepo,
            IRepository<BurndownData> burndownRepo)
        {
            this.taskRepo = repo;
            this.projectsService = projectsService;
            this.boardService = boardService;
            this.userStoryRepo = userStoryRepo;
            this.boardRepo = boardRepo;
            this.burndownRepo = burndownRepo;
        }

        public async Task ChangeColumnAsync(int itemId, int columnId)
        {
            var taskToMove = await this.taskRepo.All()
               .Where(x => x.Id == itemId)
               .Include(x => x.UserStory)
               .FirstOrDefaultAsync();

            await this.UpdateBurndownTsks(taskToMove.UserStory.SprintId, columnId, oldColId: taskToMove.KanbanBoardColumnId);

            taskToMove.KanbanBoardColumnId = columnId;
            await this.taskRepo.SaveChangesAsync();
        }

        public async Task CreateAsync(TaskInputModelDto inputModelDto, int projectId)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModelDto.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var taskToCreate = new UserStoryTask()
            {
                AddedOn = DateTime.UtcNow,
                IdForProject = nextId,
                Description = inputModelDto.Description,
                Title = inputModelDto.Title,
                UserId = inputModelDto.UserId,
                UserStoryId = inputModelDto.UserStoryId,
                AcceptanceCriteria = inputModelDto.AcceptanceCriteria,
                KanbanBoardColumnId = columnId,
            };

            await this.taskRepo.AddAsync(taskToCreate);
            await this.taskRepo.SaveChangesAsync();

            await this.UpdateBurndownTsks(sprintId, columnId, true);
        }

        public async Task DeleteAsync(int taskId)
        {
            var toRemove = await this.taskRepo.All()
                .Where(x => x.Id == taskId)
                .Include(x => x.UserStory.SprintId)
                .FirstOrDefaultAsync();

            this.taskRepo.Delete(toRemove);
            await this.taskRepo.SaveChangesAsync();

            await this.RemoveFromBurndownData(toRemove.UserStory.SprintId, toRemove.KanbanBoardColumnId);
        }

        private async Task UpdateBurndownTsks(int? sprintId, int? columnId, bool isNewlyAdded = false, int? oldColId = null)
        {
            if (sprintId != null && columnId != null)
            {
                // Get burndown record for the day 
                var burndownToUpdate = await this.burndownRepo.All()
                    .Where(x => x.SprintId == sprintId && x.DayOfSprint.Date == DateTime.UtcNow.Date)
                    .FirstOrDefaultAsync();

                // Take id of the last column in the baord which is the Done/Finished column
                var doneColumnId = await this.boardRepo.AllAsNoTracking()
                    .Where(x => x.SprintId == sprintId)
                    .OrderByDescending(x => x.KanbanBoardColumnOption.PositionLTR)
                    .Take(1)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (isNewlyAdded)
                {
                    burndownToUpdate.TotalTasks += 1;
                }
                else
                {
                    // Change finished tasks if item is placed in done column
                    if (columnId == doneColumnId)
                    {
                        burndownToUpdate.FinishedTasks += 1;
                    }
                    else if (oldColId == doneColumnId)
                    {
                        burndownToUpdate.FinishedTasks -= 1;
                    }
                }

                await this.burndownRepo.SaveChangesAsync();
            }
        }

        private async Task RemoveFromBurndownData(int? sprintId, int? columnId)
        {
            if (sprintId != null && columnId != null)
            {
                // Get burndown record for the day 
                var burndownToUpdate = await this.burndownRepo.All()
                    .Where(x => x.SprintId == sprintId && x.DayOfSprint.Date == DateTime.UtcNow.Date)
                    .FirstOrDefaultAsync();

                // Take id of the last column in the baord which is the Done/Finished column
                var doneColumnId = await this.boardRepo.AllAsNoTracking()
                    .Where(x => x.SprintId == sprintId)
                    .OrderByDescending(x => x.KanbanBoardColumnOption.PositionLTR)
                    .Take(1)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                // Change finished tasks if item is in done column
                if (columnId == doneColumnId)
                {
                    burndownToUpdate.FinishedTasks -= 1;
                }

                burndownToUpdate.TotalTasks -= 1;

                await this.burndownRepo.SaveChangesAsync();
            }
        }

    }
}
