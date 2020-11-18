using AutoMapper;
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

        public IEnumerable<UserStoryDto> GetAll(int projectId)
        {
            return this.mapper.Map<IEnumerable<UserStoryDto>>(this.repo.All()
                .Include(x => x.BacklogPriority).Where(x => x.ProjectId == projectId));
        }

        public async Task CreateAsync(CreateUserStoryInputModel model)
        {
            var userStory = new UserStory()
            {
                AddedOn = DateTime.UtcNow,
                Title = model.Title,
                Description = model.Description,
                AcceptanceCriteria = model.AcceptanceCriteria,
                ProjectId = model.ProjectId,
                StoryPoints = model.StoryPoints,
                BacklogPriorityId = int.Parse(model.BacklogPriorityid),
            };

            await this.repo.AddAsync(userStory);

            await this.repo.SaveChangesAsync();
        }

        public UserStoryDto Get(int userStoryId)
        {
            var userStory = this.repo.All().Where(x => x.Id == userStoryId).FirstOrDefault();
            return this.mapper.Map<UserStoryDto>(userStory);
        }

        public async Task Delete(int userStoryId)
        {
            var toRemove = this.repo.All().Where(x => x.Id == userStoryId).FirstOrDefault();
            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }
    }
}
