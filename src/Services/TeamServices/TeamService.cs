using AutoMapper;
using Data.Models;
using DataModels.Models.Teams;
using Repo;

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
            return new UserDto();
        }
    }
}
