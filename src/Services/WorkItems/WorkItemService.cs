using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Sorting;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repo;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IRepository<UserStory> userStoryRepo;
        private readonly IRepository<Bug> BugRepo;
        private readonly IRepository<Task> TaskRepo;
        private readonly IRepository<Test> TestRepo;
        private readonly IMapper mapper;
        private readonly ILogger<WorkItemService> logger;

        public WorkItemService(IRepository<UserStory> repo, IMapper mapper, ILogger<WorkItemService> logger)
        {
            this.userStoryRepo = repo;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<WokrItemAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter)
        {
            var query = this.userStoryRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId);
            var srotedQuery = Sort(sortingFilter, query);

            var sortedWorkItems = await srotedQuery.ProjectTo<WokrItemAllDto>(this.mapper.ConfigurationProvider)
                 .ToListAsync();

            return sortedWorkItems;
        }

        public async Task CreateUserStoryAsync(WorkItemInputModel model)
        {
            var workItem = this.mapper.Map<UserStory>(model);
            workItem.AddedOn = DateTime.UtcNow;

            await this.userStoryRepo.AddAsync(workItem);

            await this.userStoryRepo.SaveChangesAsync();
        }

        public async Task<WorkItemDto> GetAsync(int WorkItemId)
        {
            var workItem = await this.userStoryRepo.All()
                .Where(x => x.Id == WorkItemId)
                .ProjectTo<WorkItemDto>(this.mapper.ConfigurationProvider)
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

        public async Task UpdateAsync(WorkItemUpdateModel updateModel)
        {
            try
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

                if (updateModel.Comment != null)
                {
                    var comment = this.mapper.Map<Comment>(updateModel.Comment);
                    comment.Description = updateModel.Comment.SanitizedDescription;
                    comment.AddedOn = DateTime.UtcNow;

                    toUpdate.Comments.Add(comment);
                }

                this.userStoryRepo.Update(toUpdate);
                await this.userStoryRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, $"Error while updating workitem entity with id {updateModel.Id}.");
            }
        }

        private static IQueryable<UserStory> Sort(SortingFilter sortingFilter, IQueryable<UserStory> query)
        {
            return sortingFilter.SortingParams switch
            {
                WorkItemsSortingConstants.IdAsc => query.OrderBy(x => x.Id),
                WorkItemsSortingConstants.IdDesc => query.OrderByDescending(x => x.Id),
                WorkItemsSortingConstants.TitleAsc => query.OrderBy(x => x.Title),
                WorkItemsSortingConstants.TitleDesc => query.OrderByDescending(x => x.Title),
                WorkItemsSortingConstants.StoryPointsAsc => query.OrderBy(x => x.StoryPoints),
                WorkItemsSortingConstants.StoryPointsDesc => query.OrderByDescending(x => x.StoryPoints),
                WorkItemsSortingConstants.PriorityAsc => query.OrderBy(x => x.BacklogPriority.Weight),
                WorkItemsSortingConstants.PriorityDesc => query.OrderByDescending(x => x.BacklogPriority.Weight),
                _ => query,
            };
        }
    }
}
