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

        public UserStoryService(IRepository<UserStory> userStoryRepo, IMapper mapper,
            IProjectsService projectsService,
            IBoardsService boardService)
        {
            this.userStoryRepo = userStoryRepo;
            this.mapper = mapper;
            this.projectsService = projectsService;
            this.boardService = boardService;
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

        public async Task<UserStoryDto> GetAsync(int WorkItemId)
        {
            var workItem = await this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == WorkItemId)
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
                this.userStoryRepo.Delete(toRemove);

                await this.userStoryRepo.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(UserStoryUpdateModel updateModel)
        {
            var toUpdate = this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.Id == updateModel.Id)
                .FirstOrDefault();

            toUpdate.ModifiedOn = DateTime.UtcNow;
            toUpdate.Title = updateModel.Title;
            toUpdate.StoryPoints = updateModel.StoryPoints;
            toUpdate.AcceptanceCriteria = updateModel.AcceptanceCriteria;
            toUpdate.BacklogPriorityId = updateModel.BacklogPriorityid;
            toUpdate.Description = updateModel.Description;
            if(toUpdate.SprintId != updateModel.SprintId || toUpdate.SprintId == null)
            {
                int sprintId = updateModel.SprintId ?? default;
                var columnId = (await this.boardService.GetAllColumnsAsync(toUpdate.ProjectId, sprintId)).FirstOrDefault().Id;

                toUpdate.SprintId = sprintId;
                toUpdate.KanbanBoardColumnId = columnId;
            }

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

            this.userStoryRepo.Update(toUpdate);
            await this.userStoryRepo.SaveChangesAsync();
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

            userStoryToMove.KanbanBoardColumnId = columnId;
            await this.userStoryRepo.SaveChangesAsync();
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
                _ => query,
            };
        }
    }
}
