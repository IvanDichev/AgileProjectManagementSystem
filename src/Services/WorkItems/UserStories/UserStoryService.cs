using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repo;
using Services.BoardColumns;
using Services.Projects;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.UserStories
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IMapper mapper;
        private readonly IProjectsService projectsService;
        private readonly IBoardsService boardService;
        private readonly IRepository<UserStoryTask> taskRepo;
        private readonly IRepository<Test> testRepo;
        private readonly IRepository<BurndownData> burndownRepo;
        private readonly IRepository<KanbanBoardColumn> boardRepo;

        public UserStoryService(IRepository<UserStory> userStoryRepo, IMapper mapper,
            IProjectsService projectsService,
            IBoardsService boardService,
            IRepository<UserStoryTask> taskRepo,
            IRepository<Test> testRepo,
            IRepository<BurndownData> burndownRepo,
            IRepository<KanbanBoardColumn> boardRepo)
        {
            this.userStoryRepo = userStoryRepo;
            this.mapper = mapper;
            this.projectsService = projectsService;
            this.boardService = boardService;
            this.taskRepo = taskRepo;
            this.testRepo = testRepo;
            this.burndownRepo = burndownRepo;
            this.boardRepo = boardRepo;
        }

        public async Task<IEnumerable<UserStoryAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter)
        {
            var query = this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId);
            var srotedQuery = Sort(sortingFilter, query);

            var sortedWorkItems = await srotedQuery.ProjectTo<UserStoryAllDto>(this.mapper.ConfigurationProvider)
                 .ToListAsync();

            return sortedWorkItems;
        }

        public async Task CreateAsync(UserStoryInputDto inputDto)
        {
            var userStory = new UserStory()
            {
                AddedOn = DateTime.UtcNow,
                IdForProject = await projectsService.GetNextIdForWorkItemAsync(inputDto.ProjectId),
                AcceptanceCriteria = inputDto.SanitizedAcceptanceCriteria,
                BacklogPriorityId = inputDto.BacklogPriorityid,
                Description = inputDto.SanitizedDescription,
                ProjectId = inputDto.ProjectId,
                StoryPoints = inputDto.StoryPoints,
                Title = inputDto.Title,
            };

            await this.userStoryRepo.AddAsync(userStory);
            await this.userStoryRepo.SaveChangesAsync();

            var userStoryId = this.userStoryRepo.AllAsNoTracking().Where(x => x == userStory).Select(x => x.Id).FirstOrDefault();

            userStory.Mockups.Add(new Mockup()
            {
                AddedOn = DateTime.UtcNow,
                MockUpPath = inputDto.MockupPath,
                UserStoryId = userStoryId,
            });

            await this.userStoryRepo.SaveChangesAsync();
        }

        public async Task<UserStoryDto> GetAsync(int UserStoryId)
        {
            var workItem = await this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == UserStoryId)
                .ProjectTo<UserStoryDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return workItem;
        }

        public async Task DeleteAsync(int userStoryId)
        {
            var toRemove = await this.userStoryRepo.All()
                .Where(x => x.Id == userStoryId)
                .FirstOrDefaultAsync();

            if (toRemove != null)
            {
                await this.RemoveFromBurndownData(toRemove.SprintId, toRemove.KanbanBoardColumnId);

                this.userStoryRepo.Delete(toRemove);

                await this.userStoryRepo.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(UserStoryUpdateModel updateModel)
        {
            var toUpdate = this.userStoryRepo.All()
                .Where(x => x.Id == updateModel.Id)
                .Include(x => x.Tests)
                .Include(x => x.Tasks)
                .FirstOrDefault();

            toUpdate.ModifiedOn = DateTime.UtcNow;
            toUpdate.Title = updateModel.Title;
            toUpdate.StoryPoints = updateModel.StoryPoints;
            toUpdate.AcceptanceCriteria = updateModel.AcceptanceCriteria;
            toUpdate.BacklogPriorityId = updateModel.BacklogPriorityid;
            toUpdate.Description = updateModel.Description;

            // If sprint is changed
            if (toUpdate.SprintId != updateModel.SprintId && updateModel.SprintId != null)
            {
                int sprintId = updateModel.SprintId ?? default;
                var columnId = (await this.boardService.GetAllColumnsAsync(toUpdate.ProjectId, sprintId)).FirstOrDefault().Id;

                if (toUpdate.SprintId != null)
                {
                    await RemoveFromBurndownData(toUpdate.SprintId, toUpdate.KanbanBoardColumnId);
                }

                await UpdateBurndownTsks(updateModel.SprintId, columnId, true);

                toUpdate.SprintId = sprintId;
                toUpdate.KanbanBoardColumnId = columnId;

                foreach (var test in toUpdate.Tests)
                {
                    test.KanbanBoardColumnId = columnId;
                }

                foreach (var task in toUpdate.Tasks)
                {
                    task.KanbanBoardColumnId = columnId;
                }
            }
            // If sprint is set to null then userStory should be removed from table
            else if (updateModel.SprintId == null)
            {
                if (toUpdate.SprintId != null)
                {
                    await RemoveFromBurndownData(toUpdate.SprintId, toUpdate.KanbanBoardColumnId);
                }
                // Remove from sprint and board.
                toUpdate.SprintId = updateModel.SprintId;
                toUpdate.KanbanBoardColumnId = null;

                foreach (var test in toUpdate.Tests)
                {
                    test.KanbanBoardColumnId = null;
                }

                foreach (var task in toUpdate.Tasks)
                {
                    task.KanbanBoardColumnId = null;
                }
            }

            // Add comments if there are any
            if (updateModel.Comment != null)
            {
                var comment = new UserStoryComment
                {
                    UserId = updateModel.Comment.AddedById,
                    Description = updateModel.Comment.SanitizedDescription,
                    AddedOn = DateTime.UtcNow
                };

                toUpdate.Comments.Add(comment);
            }

            // Add mockups
            foreach (var mockup in updateModel.MockupPaths)
            {
                toUpdate.Mockups.Add(new Mockup()
                {
                    AddedOn = DateTime.UtcNow,
                    MockUpPath = mockup,
                    UserStoryId = toUpdate.Id,
                });
            }

            this.userStoryRepo.Update(toUpdate);
            await this.userStoryRepo.SaveChangesAsync();
            await this.taskRepo.SaveChangesAsync();
            await this.testRepo.SaveChangesAsync();
        }

        public async Task<ICollection<UserStoryDropDownModel>> GetUserStoryDropDownsAsync(int projectId)
        {
            var dropdowns = await this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<UserStoryDropDownModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return dropdowns;
        }

        public async Task ChangeColumnAsync(int userStoryId, int columnId)
        {
            var userStoryToMove = await this.userStoryRepo.All()
                .Where(x => x.Id == userStoryId)
                .FirstOrDefaultAsync();

            await UpdateBurndownTsks(userStoryToMove.SprintId, columnId, oldColId: userStoryToMove.KanbanBoardColumnId);

            userStoryToMove.KanbanBoardColumnId = columnId;
            await this.userStoryRepo.SaveChangesAsync();
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
                    // If item is removed from done column
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

        private static IQueryable<UserStory> Sort(SortingFilter sortingFilter, IQueryable<UserStory> query)
        {
            return sortingFilter.SortingParams switch
            {
                UserStorySortingConstants.IdAsc => query.OrderBy(x => x.IdForProject),
                UserStorySortingConstants.IdDesc => query.OrderByDescending(x => x.IdForProject),
                UserStorySortingConstants.TitleAsc => query.OrderBy(x => x.Title),
                UserStorySortingConstants.TitleDesc => query.OrderByDescending(x => x.Title),
                UserStorySortingConstants.StoryPointsAsc => query.OrderBy(x => x.StoryPoints),
                UserStorySortingConstants.StoryPointsDesc => query.OrderByDescending(x => x.StoryPoints),
                UserStorySortingConstants.PriorityAsc => query.OrderBy(x => x.BacklogPriority.Weight),
                UserStorySortingConstants.PriorityDesc => query.OrderByDescending(x => x.BacklogPriority.Weight),
                UserStorySortingConstants.SprintAsc => query.OrderBy(x => x.Sprint.Name),
                UserStorySortingConstants.SprintDesc => query.OrderByDescending(x => x.Sprint.Name),
                _ => query,
            };
        }
    }
}
