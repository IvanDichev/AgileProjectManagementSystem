using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.UserStories
{
    public class UserStoriesService : IUserStoriesService
    {
        private readonly IRepository<UserStory> repo;
        private readonly IMapper mapper;
        private readonly ILogger<UserStoriesService> logger;

        public UserStoriesService(IRepository<UserStory> repo, IMapper mapper, ILogger<UserStoriesService> logger)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<UserStoryAllDto>> GetAllAsync(int projectId)
        {
            return await this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<UserStoryAllDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task CreateAsync(UserStoryInputModel model)
        {
            var userStory = this.mapper.Map<UserStory>(model);
            userStory.AddedOn = DateTime.UtcNow;

            await this.repo.AddAsync(userStory);

            await this.repo.SaveChangesAsync();
        }

        public async Task<UserStoryDto> GetAsync(int userStoryId)
        {
            return await this.repo.All()
                .Where(x => x.Id == userStoryId)
                .ProjectTo<UserStoryDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int userStoryId)
        {
            var toRemove = await this.repo.All()
                .Where(x => x.Id == userStoryId)
                .FirstOrDefaultAsync();

            this.repo.Delete(toRemove);

            await this.repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserStoryUpdateModel updateModel)
        {
            try
            {
                var addedOn = await this.repo.AllAsNoTracking()
                    .Where(x => x.Id == updateModel.Id)
                    .Select(x => x.AddedOn)
                    .FirstOrDefaultAsync();

                var toUpdate = this.mapper.Map<UserStory>(updateModel);
                toUpdate.AddedOn = addedOn;
                toUpdate.ModifiedOn = DateTime.UtcNow;

                this.repo.Update(toUpdate);
                await this.repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                this.logger.LogWarning(e, $"Error while updating entity with id {updateModel.Id}.");
            }
        }
    }
}
