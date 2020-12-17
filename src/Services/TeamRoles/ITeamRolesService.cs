using DataModels.Models.TeamRoles.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.TeamRoles
{
    public interface ITeamRolesService
    {
        Task<ICollection<TeamRolesDto>> GetTeamRolesAsync();
    }
}
