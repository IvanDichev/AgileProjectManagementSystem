using AutoMapper;
using Data.Models;
using DataModels.Models.UserStories.Dtos;
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
            return this.mapper.Map<IEnumerable<UserStoryDto>>(this.repo.All());
        }
    }
}
