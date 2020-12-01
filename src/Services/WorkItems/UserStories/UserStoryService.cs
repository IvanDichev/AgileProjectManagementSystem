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
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.WorkItems.UserStories
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IRepository<UserStory> repo;
        private readonly IMapper mapper;
        private readonly ILogger<UserStoryService> logger;

        public UserStoryService(IRepository<UserStory> repo, IMapper mapper, ILogger<UserStoryService> logger)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<UserStoryAllDto>> GetAllAsync(int projectId, SortingFilter sortingFilter)
        {
            var query = this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId);
            var srotedQuery = Sort(sortingFilter, query);

            var sortedWorkItems = await srotedQuery.ProjectTo<UserStoryAllDto>(this.mapper.ConfigurationProvider)
                 .ToListAsync();

            return sortedWorkItems;
        }

        public async Task CreateAsync(UserStoryInputModel model)
        {
            var userStory = this.mapper.Map<UserStory>(model);
            userStory.AddedOn = DateTime.UtcNow;
            // Increment IfForProject TODO Fix separate this column in another table
            var workItemsId = (this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == model.ProjectId)
                .Select(x => x.Project.WorkItemsId)
                .FirstOrDefault() + 1);
            
            userStory.IdForProject = this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == model.ProjectId)
                .Select(x => x.Project.WorkItemsId)
                .FirstOrDefault();


            await this.repo.AddAsync(userStory);

            await this.repo.SaveChangesAsync();
        }

        public async Task<UserStoryDto> GetAsync(int WorkItemId)
        {
            var workItem = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == WorkItemId)
                .ProjectTo<UserStoryDto>(this.mapper.ConfigurationProvider)
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

        public async Task UpdateAsync(UserStoryUpdateModel updateModel)
        {
            try
            {
                var toUpdate = this.repo.AllAsNoTracking()
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
                    var comment = new UserStoryComment
                    {
                        UserId = updateModel.Comment.AddedById,
                        Description = updateModel.Comment.SanitizedDescription,
                        AddedOn = DateTime.UtcNow
                    };

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

        public async Task<ICollection<UserStoryDropDownModel>> GetUserStoryDropDownsAsync(int projectId)
        {
            var dropdowns = await this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<UserStoryDropDownModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return dropdowns;
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
