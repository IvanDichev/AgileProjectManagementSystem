using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;
using Microsoft.EntityFrameworkCore;
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
        public UserStoriesService(IRepository<UserStory> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserStoryDto>> GetAllAsync(int projectId)
        {
            var userStories = await this.repo.All()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<UserStoryDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<UserStoryDto>>(userStories);
        }

        public async Task CreateAsync(CreateUserStoryInputModel model)
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
            var toRemove = this.repo.All()
                .Where(x => x.Id == userStoryId)
                .FirstOrDefault();

            this.repo.Delete(toRemove);

            await this.repo.SaveChangesAsync();
        }
    }
}
