using AutoMapper;
using Data.Models;
using DataModels.Models.WorkItems.Tests.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Tests
{
    public class TestsService : ITestsService
    {
        private readonly IRepository<Test> repo;
        private readonly IProjectsService projectsService;
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IBoardsService boardService;
        private readonly IRepository<BurndownData> burndownRepo;
        private readonly IRepository<KanbanBoardColumn> boardRepo;

        public TestsService(IRepository<Test> repo,
            IProjectsService projectsService,
            IRepository<UserStory> userStoryRepo,
            IBoardsService boardService,
            IRepository<BurndownData> burndownRepo,
            IRepository<KanbanBoardColumn> boardRepo)
        {
            this.repo = repo;
            this.projectsService = projectsService;
            this.userStoryRepo = userStoryRepo;
            this.boardService = boardService;
            this.burndownRepo = burndownRepo;
            this.boardRepo = boardRepo;
        }

        public async Task ChangeColumnAsync(int itemId, int columnId)
        {
            var testToMove = await this.repo.All()
               .Where(x => x.Id == itemId)
               .Include(x => x.UserStory.SprintId)
               .FirstOrDefaultAsync();

            testToMove.KanbanBoardColumnId = columnId;
            await this.repo.SaveChangesAsync();

            await this.UpdateBurndownTsks(testToMove.UserStory.SprintId, columnId);
        }

        public async Task CreateAsync(int projectId, TestInputModelDto inputModel)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModel.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var testToCreate = new Test()
            {
                AddedOn = DateTime.UtcNow,
                AcceptanceCriteria = inputModel.AcceptanceCriteria,
                Description = inputModel.Description,
                UserStoryId = inputModel.UserStoryId,
                Title = inputModel.Title,
                IdForProject = nextId,
                KanbanBoardColumnId = columnId,
            };

            await this.UpdateBurndownTsks(sprintId, columnId, true);

            await this.repo.AddAsync(testToCreate);
            await this.repo.SaveChangesAsync();
        }

        private async Task UpdateBurndownTsks(int? sprintId, int? columnId, bool isNewlyAdded = false)
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
                    else
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

        public async Task DeleteAsync(int testId)
        {
            var toRemove = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == testId)
                .Include(x => x.UserStory.SprintId)
                .FirstOrDefaultAsync();

            await this.RemoveFromBurndownData(toRemove.UserStory.SprintId, toRemove.KanbanBoardColumnId);

            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }
    }
}
