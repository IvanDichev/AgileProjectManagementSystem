using AutoMapper;
using Data.Models;
using DataModels.Models.UserStories.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Linq;

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
            if(this.repo.All().Where(x => x.ProjectId == projectId).Count() == 0)
            {
                return true;
            }
            return this.repo.All().Any(x => x.Project.Team.TeamsUsers.Any(x => x.UserId == userId));
        }
    }
}
