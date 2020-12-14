using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Severity;
using DataModels.Models.WorkItems.Bugs.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.Bugs
{
    public class BugsService : IBugsService
    {
        private readonly IRepository<Bug> bugsRepo;
        private readonly IRepository<Severity> severityRepo;
        private readonly IMapper mapper;
        private readonly IProjectsService projectsService;
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IBoardsService boardService;
        private readonly IRepository<BurndownData> burndownRepo;
        private readonly IRepository<KanbanBoardColumn> boardRepo;

        public BugsService(IRepository<Bug> bugsRepo, 
            IRepository<Severity> severityRepo, 
            IMapper mapper, 
            IProjectsService projectsService,
            IRepository<UserStory> userStoryRepo,
            IBoardsService boardService,
            IRepository<BurndownData> burndownRepo,
            IRepository<KanbanBoardColumn> boardRepo)
        {
            this.bugsRepo = bugsRepo;
            this.severityRepo = severityRepo;
            this.mapper = mapper;
            this.projectsService = projectsService;
            this.userStoryRepo = userStoryRepo;
            this.boardService = boardService;
            this.burndownRepo = burndownRepo;
            this.boardRepo = boardRepo;
        }

        public async Task CreateBugAsync(int projectId, BugInputModelDto inputModel)
        {
            var nextId = await this.projectsService.GetNextIdForWorkItemAsync(projectId);
            var sprintId = await userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == inputModel.UserStoryId)
                .Select(x => x.SprintId)
                .FirstOrDefaultAsync() ?? default;

            var columnId = (await this.boardService.GetAllColumnsAsync(projectId, sprintId)).FirstOrDefault().Id;

            var toCreate = new Bug()
            {
                AcceptanceCriteria = inputModel.AcceptanceCriteria,
                Description = inputModel.Description,
                AddedOn = DateTime.UtcNow,
                SeverityId = inputModel.SeverityId,
                Title = inputModel.Title,
                UserStoryId = inputModel.UserStoryId,
                IdForProject = nextId,
                KanbanBoardColumnId = columnId,
            };

            await this.bugsRepo.AddAsync(toCreate);
            await this.bugsRepo.SaveChangesAsync();

            await this.UpdateBurndownTsks(sprintId, columnId, true);
        }

        public async Task DeleteAsync(int bugId)
        {
            var toRemove = await this.bugsRepo.All()
                .Where(x => x.Id == bugId)
                .Include(x => x.UserStory.SprintId)
                .FirstOrDefaultAsync();

            this.bugsRepo.Delete(toRemove);
            await this.bugsRepo.SaveChangesAsync();

            await this.RemoveFromBurndownData(toRemove.UserStory.SprintId, toRemove.KanbanBoardColumnId);
        }

        public async Task<ICollection<SeverityDropDownModel>> GetSeverityDropDown()
        {
            var severityDropDown = await this.severityRepo
                .AllAsNoTracking()
                .ProjectTo<SeverityDropDownModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return severityDropDown;
        }

        public async Task ChangeColumnAsync(int itemId, int columnId)
        {
            var bugToMove = await this.bugsRepo.All()
               .Where(x => x.Id == itemId)
               .Include(x => x.UserStory)
               .FirstOrDefaultAsync();

            await this.UpdateBurndownTsks(bugToMove.UserStory.SprintId, columnId, oldColId: bugToMove.KanbanBoardColumnId);

            bugToMove.KanbanBoardColumnId = columnId;
            await this.bugsRepo.SaveChangesAsync();
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
