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

        public bool IsUserInProject(int projectId, int userId)
        {
            var i = this.repo.All().Any(x => x.Project.Team.TeamsUsers.Any(x => x.UserId == userId));
            return i;
                //.Where(x => x.ProjectId == projectId).Any(x => x.Project.Team.TeamsUsers.Any(x => x.UserId == userId));
        }

        public async Task<int> CreateAsync(CreateUserStoryInputModel model)
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
            int userstoryId = this.repo.AllAsNoTracking().Where(x => x == userStory).FirstOrDefault().Id;
            return userstoryId;
        }

        public UserStoryDto Get(int userStoryId)
        {
            throw new NotImplementedException();
        }
    }
}
