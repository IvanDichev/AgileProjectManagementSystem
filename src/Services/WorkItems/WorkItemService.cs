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
        private readonly IRepository<WorkItem> repo;
        private readonly IMapper mapper;
        private readonly ILogger<WorkItemService> logger;

        public WorkItemService(IRepository<WorkItem> repo, IMapper mapper, ILogger<WorkItemService> logger)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<WokrItemAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter)
        {
            var query = this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId);
            var srotedQuery = Sort(sortingFilter, query);

           var sortedWorkItems = await srotedQuery.ProjectTo<WokrItemAllDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return sortedWorkItems;
        }

        public async Task CreateAsync(WorkItemInputModel model)
        {
            var workItem = this.mapper.Map<WorkItem>(model);
            workItem.AddedOn = DateTime.UtcNow;

            await this.repo.AddAsync(workItem);

            await this.repo.SaveChangesAsync();
        }

        public async Task<WorkItemDto> GetAsync(int WorkItemId)
        {
            var workItem = await this.repo.All()
                .Where(x => x.Id == WorkItemId)
                .ProjectTo<WorkItemDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return workItem;
        }

        public async Task DeleteAsync(int userStoryId)
        {
            var toRemove = await this.repo.All()
                .Where(x => x.Id == userStoryId)
                .FirstOrDefaultAsync();

            if (toRemove != null)
            {
                this.repo.Delete(toRemove);

                await this.repo.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(WorkItemUpdateModel updateModel)
        {
            try
            {
                var toUpdate = this.repo.AllAsNoTracking()
                    .Where(x => x.Id == updateModel.Id)
                    .FirstOrDefault();

                toUpdate.ModifiedOn = DateTime.UtcNow;
                toUpdate.Title = updateModel.Title;
                toUpdate.StoryPoints = updateModel.StoryPoints;
                toUpdate.WorkItemTypeId = updateModel.WorkItemTypeId;
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

                this.repo.Update(toUpdate);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, $"Error while updating workitem entity with id {updateModel.Id}.");
            }
        }

        private static IQueryable<WorkItem> Sort(SortingFilter sortingFilter, IQueryable<WorkItem> query)
        {
            return sortingFilter.SortingParams switch
            {
                UserStoriesSortingConstants.IdAsc => query.OrderBy(x => x.Id),
                UserStoriesSortingConstants.IdDesc => query.OrderByDescending(x => x.Id),
                UserStoriesSortingConstants.TitleAsc => query.OrderBy(x => x.Title),
                UserStoriesSortingConstants.TitleDesc => query.OrderByDescending(x => x.Title),
                UserStoriesSortingConstants.TypeAsc => query.OrderBy(x => x.WorkItemType),
                UserStoriesSortingConstants.TypeDesc => query.OrderByDescending(x => x.WorkItemType),
                UserStoriesSortingConstants.StoryPointsAsc => query.OrderBy(x => x.StoryPoints),
                UserStoriesSortingConstants.StoryPointsDesc => query.OrderByDescending(x => x.StoryPoints),
                UserStoriesSortingConstants.PriorityAsc => query.OrderBy(x => x.BacklogPriority.Weight),
                UserStoriesSortingConstants.PriorityDesc => query.OrderByDescending(x => x.BacklogPriority.Weight),
                _ => query,
            };
        }
    }
}
