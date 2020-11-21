using DataModels.Models.Teams;

namespace Services.TeamServices
{
    public interface ITeamService
    {
        UserDto GetUsersInProject(int userId, int projectId);
    }
}
