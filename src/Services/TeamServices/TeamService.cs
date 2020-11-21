using AutoMapper;
using Data.Models;
using DataModels.Models.Teams;
using DataModels.Models.UserStories;
using Repo;
using System.Linq;

namespace Services.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> repo;
        private readonly IMapper mapper;

        public TeamService(IRepository<Team> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public UserDto GetUsersInProject(int userId, int projectId)
        {
            //var query = this.repo.All();
            //var usersInProject = 
            return new UserDto();
        }
    }
}
